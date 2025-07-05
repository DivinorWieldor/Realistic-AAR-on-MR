using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;
using System;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Data;

class CsvLogger
{
    StreamWriter file;

    public CsvLogger(string filename)
    {
        file = new StreamWriter(filename);
    }

    public CsvLogger(string filename, params string[] columnNames) : this(filename)
    {
        file.WriteLine(string.Join(",", columnNames));
    }

    public void AddRow(params double[] values)
    {
        file.WriteLine(string.Join(",", values.Select(f=>f.ToString())));
    }

    public void Close()
    {
        file.Close();
    }
}


public class dataLogging : MonoBehaviour
{
    
    [Header("Test Information")]
    [Tooltip("what is the participant ID?")]
    public string participantID;

    //2_sound_R is 1, 2_sound_L is 2, 1_sound_L is 3, 1_sound_R is 4
    [Tooltip("what is the active sound?")]
    public string targetID;

    [Header("Folder structure")]
    [Tooltip("what is the folder this will be saved to? (default is 'experimentOutcomes')")]
    public string folderName = "experimentOutcomes";

    public GameObject playerObject;

    [Range(0, 1)]
    public int testStarted = 0;


    CsvLogger logger;
    void Start()
    {
        testStarted = 0;
        string fileName = participantID + "_" + targetID;
        string saveLoc = folderName + "/" + fileName + ".csv";

        // make sure the directory exists
        if (!Directory.Exists(folderName))
            Directory.CreateDirectory(folderName);

        // make sure the file name is unique
        while (File.Exists(saveLoc))
        {
            saveLoc = folderName + "/" + fileName + "-" + DateTimeOffset.Now.ToUnixTimeSeconds() + ".csv";
        }

        logger = new CsvLogger(saveLoc, "time (app)", "time (unix-ms)",
            "position x", "position y", "position z",
            "rotation (Euler) x", "rotation (Euler) y", "rotation (Euler) z",
            "rotation (Quaternion) x", "rotation (Quaternion) y", "rotation (Quaternion) z", "rotation (Quaternion) w",
            "test Started"
            );
    }

    void Update()
    {
        // use "=DATE(1970;1;1) + 1708341418/60/60/24" to convert to a normal date format in excel
        // row limit is 1.048.576
        // this creates ~510 rows per second
        // so we can log for ~34 minutes

        logger.AddRow(  Time.time, DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                        playerObject.transform.position.x, playerObject.transform.position.y, playerObject.transform.position.z,
                        playerObject.transform.rotation.eulerAngles.x, playerObject.transform.rotation.eulerAngles.y, playerObject.transform.rotation.eulerAngles.z,
                        playerObject.transform.rotation.x, playerObject.transform.rotation.y, playerObject.transform.rotation.z, playerObject.transform.rotation.w,
                        testStarted
                    );
    }

    void OnApplicationQuit()
    {
        logger.Close();
    }
}
