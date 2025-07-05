using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

// @kurtdekker - Geiger Counter simulator
//
// To use:
//	- make a new scene with AudioListener (the one on the Camera will suffice)
// 	- drop this script on a blank GameObject
//	- make a UI.Slider
//		- if you don't then you will need to call DriveChance yourself

public class GeigerCounter : MonoBehaviour
{
    public class Tick
    {
        public float amplitude;
        public float phase;
    }

    List<Tick> ticks = new List<Tick>();

    [Header("This shows what the chance of the radiation is")]
    [Range(0, 1)]
    public float radiationChance = 0.5f;
    public GameObject gameCam;


    [Header("Volume")]
    public float gain = 0.5F;

    // radiation... it's all around you every day
    float backgroundRadiation = 0.000001f;

    // and this is when you stick your head in the nuclear furnace
    float span = 0.0100f;

    private bool running = false;

    // input
    float chance;
    float distance;

    int randomRover;
    const int numStoredRandoms = 256000;
    float[] storedRandoms;


    // expects 0.0 to 1.0
    public void SetRadiationLevel(float input)
    {
        // arbitrary power curve - use an AnimationCurve if you want to define the relationship
        input = input * input * input;

        int sampleRate = AudioSettings.outputSampleRate;
        input *= 44100f / sampleRate;

        chance = backgroundRadiation + span * input;
    }

    public float CustomDistance(Vector3 pos1, Vector3 pos2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + //x-distance
                                    Mathf.Pow(pos1.y - pos2.y, 2)*3 + //y-distance - decrease the influence of this by thrice
                                    Mathf.Pow(pos1.z - pos2.z, 2)); //z-distance
        return distance;
    }

    public float GetRadiationChance(float distance)
    {
        float Chance = 1 / Mathf.Pow((distance / 10 + 1), 2);
        //float Chance = math.tanh( 1 / Mathf.Pow((distance/4), 2) );
        return Chance;
    }

    IEnumerator Start()
    {
        chance = backgroundRadiation;

        storedRandoms = new float[numStoredRandoms];
        for (int i = 0; i < numStoredRandoms; i++)
        {
            storedRandoms[i] = UnityEngine.Random.value;
        }

        gameObject.AddComponent<AudioSource>();

        if(gameCam == null)
            gameCam = GameObject.Find("MainCamera");
        distance = CustomDistance(gameCam.transform.position, transform.position);
        radiationChance = GetRadiationChance(distance);
        SetRadiationLevel(radiationChance);

        running = true;

        // refresh the random source to keep it new and exciting for the ear
        {
            int fillRover = 0;
            while (true)
            {
                for (int i = 0; i < numStoredRandoms / 50; i++)
                {
                    storedRandoms[fillRover] = UnityEngine.Random.value;
                    fillRover++;
                    if (fillRover >= storedRandoms.Length)
                    {
                        fillRover = 0;
                    }
                }
                yield return null;
            }
        }
    }

    void Update()
    {
        distance = CustomDistance(gameCam.transform.position, transform.position);
        radiationChance = GetRadiationChance(distance);
        SetRadiationLevel(radiationChance);
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        int dataLen = data.Length / channels;

        int n = 0;
        while (n < dataLen)
        {
            // render all the ongoing ticks
            foreach (var tick in ticks)
            {
                float x = gain * tick.amplitude * Mathf.Sin(tick.phase);
                int i = 0;

                // random extra idea: might be kinda fun to have each
                // tick also track its left / right stereo position,
                // so that the ticks seem to come from everywhere...
                //
                // Volume wise:
                // To do this you'd need to choose and store $channels
                // worth of overall gain levels per tick and use them
                // here when writing the samples into a stream.
                //
                // Phase error wise:
                // I think if each Tick stored a phase error and used
                // it to render a second x sample above for left/right

                while (i < channels)
                {
                    data[n * channels + i] += x;
                    i++;
                }

                tick.phase += tick.amplitude * 0.3f;
                tick.amplitude *= 0.993f;
            }

            // did an ionizing radiation event occur this sample frame?
            float v = storedRandoms[randomRover];
            if (v < chance)
            {
                var tick = new Tick()
                {
                    amplitude = 1.0f,
                };
                ticks.Add(tick);
            }

            randomRover++;
            if (randomRover >= storedRandoms.Length)
            {
                randomRover = 0;
            }

            n++;
        }

        // tidy up lazily when ticks get pretty quiet
        ticks.RemoveAll(x => x.amplitude < 0.01f);
    }
}