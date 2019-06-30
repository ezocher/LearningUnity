using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Technique for getting a multi-dimensional array in the Unity inspector
// https://answers.unity.com/questions/148447/how-do-make-a-public-array-of-arrays-appear-in-the.html

[System.Serializable]
public class ClipList
{
    public AudioClip[] clips;
}

public class MusicClips : MonoBehaviour
{
    public ClipList[] clipLists;
}
