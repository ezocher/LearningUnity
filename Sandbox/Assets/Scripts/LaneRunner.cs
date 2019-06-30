using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneRunner : MonoBehaviour
{
    public float laneClosestDistanceMeters = 1f;
    public float laneFarthestDistanceMeters = 5f;
    public int laneNumberOfMusicClips = 3;
    public GameObject musicSequencerObject;

    private MusicSequencer musicSequencer;

    private int laneNumber;  // Lane numbers are 1..N and Lane indexes are 0..N-1
    private int laneIndex;


    void Start()
    {
        // This GameObject's name is set by its creator/parent in Awake(), so it's not guaranteed to have been done until all Awake()s are finished
        int laneNumber = int.Parse(this.name.Substring(this.name.Length - 1, 1)); // Use the last character of the GameObject's name as its number
        laneIndex = laneNumber - 1;

        musicSequencer = musicSequencerObject.GetComponent<MusicSequencer>();
        musicSequencer.Initialize(laneNumber, laneNumberOfMusicClips);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
