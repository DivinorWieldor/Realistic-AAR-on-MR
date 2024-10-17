using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class note : MonoBehaviour {
    //TextAreaAttribute(int minLines, int maxLines);
    [TextArea(1, 20)]
    public string Notes_Field;
}
