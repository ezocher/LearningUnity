using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public int numberOfLanes = 4;

    private const string lanesManagerObjectName = "LanesAndPlayers";
    private const string musicSequencerObjectName = "MusicSequencer";

    private LanesManager lanesManager;
    private MusicSequencer musicSequencer;

    void Start()
    {

        lanesManager = GameObject.Find(lanesManagerObjectName).GetComponent<LanesManager>();
        musicSequencer = GameObject.Find(musicSequencerObjectName).GetComponent<MusicSequencer>();

        lanesManager.Initialize(numberOfLanes);
        musicSequencer.Initialize(numberOfLanes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
