using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAndTimerManager : MonoBehaviour
{
    public int numberOfLanes = 4;

    public float clipDurationsSeconds = 8f;

    public float playerMoveIncrementMeters = 0.02f;

    private const string lanesManagerObjectName = "LanesAndPlayers";
    private const string musicSequencerObjectName = "MusicSequencer";

    private LanesManager lanesManager;
    private MusicSequencer musicSequencer;

    private int numberOfPlayersActive;
    private bool[] playerActive;

    private bool musicPaused = false;

    void Start()
    {
        lanesManager = GameObject.Find(lanesManagerObjectName).GetComponent<LanesManager>();
        musicSequencer = GameObject.Find(musicSequencerObjectName).GetComponent<MusicSequencer>();

        lanesManager.Initialize(numberOfLanes);
        musicSequencer.Initialize(numberOfLanes);

        playerActive = new bool[numberOfLanes];
        numberOfPlayersActive = numberOfLanes;
        for (int playerIndex = 0; playerIndex < numberOfLanes; playerIndex++)
        {
            playerActive[playerIndex] = true;
            ActivateDeactivatePlayer(playerIndex + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateCounter++;
        CheckForKeyboardInput();
    }


    void TogglePlayPause()
    {
        if (musicPaused)
        {
            musicSequencer.UnpauseAll();
            // RESTART HEARTBEAT
        }
        else
        {
            musicSequencer.PauseAll();
            // PAUSE HEARTBEAT
        }
        musicPaused = !musicPaused;
    }


    void ActivateDeactivatePlayer(int laneNumber)
    {
        int laneIndex = laneNumber - 1;

        if (playerActive[laneIndex])
        {
            lanesManager.HidePlayer(laneNumber);
            numberOfPlayersActive--;

            if (numberOfPlayersActive == 0)
            {
                // STOP HEARTBEAT
            }
        }
        else
        {
            lanesManager.ShowPlayer(laneNumber);
            numberOfPlayersActive++;

            if (numberOfPlayersActive == 1)
            {
                // START HEARTBEAT, START MUSIC
            }
        }
        playerActive[laneIndex] = !playerActive[laneIndex];

        Debug.Log("Number of players active = " + numberOfPlayersActive);
    }

    void PlayerMove(int playerNumber, float distance) => lanesManager.MovePlayer(playerNumber, distance);

    // -------------- Keyboard input -------------- 
    int updateCounter = 0;  // For keyboard input
    int lastKeyCounter = 0;
    KeyCode[] addRemovePlayerKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    KeyCode[] playerStepBackKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O };
    KeyCode[] playerStepForwardKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L };

    KeyCode[] togglePlayPauseKeys = { KeyCode.Space, KeyCode.Escape, KeyCode.Tab };

    private void CheckForKeyboardInput()
    {
        bool keyPressed = false;
        foreach (KeyCode keyCode in togglePlayPauseKeys)
            if (Input.GetKey(keyCode))
                keyPressed = true;
        if (keyPressed && DebounceKeys())
                TogglePlayPause();

        for (int keyIndex = 0; keyIndex < numberOfLanes; keyIndex++)
        {
            if (Input.GetKey(addRemovePlayerKeys[keyIndex]))
            {
                if (DebounceKeys())
                    ActivateDeactivatePlayer(keyIndex + 1);
            }

            if (playerActive[keyIndex])
            {
                if (Input.GetKey(playerStepBackKeys[keyIndex]))
                    PlayerMove(keyIndex + 1, playerMoveIncrementMeters);

                if (Input.GetKey(playerStepForwardKeys[keyIndex]))
                    PlayerMove(keyIndex + 1, -playerMoveIncrementMeters);
            }
        }
    }

    private bool DebounceKeys()
    {
        if (updateCounter == (lastKeyCounter + 1))
        {
            lastKeyCounter = updateCounter;
            return false;
        }
        else
        {
            lastKeyCounter = updateCounter;
            return true;
        }
    }
}

