using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAndTimerManager : MonoBehaviour
{
    public int numberOfLanes = 4;
    public int startingNumberOfActivePlayers = 1;

    public float clipDurationsSeconds = 8f;

    public float playerMoveIncrementMeters = 0.02f;

    private const string lanesManagerObjectName = "LanesAndPlayers";
    private const string musicSequencerObjectName = "MusicSequencer";

    private LanesManager lanesManager;
    private MusicSequencer musicSequencer;

    private int numberOfPlayersActive;
    private bool[] playerActive;

    private bool musicPaused = false;

    private IEnumerator sequencerHeartbeat;

    void Start()
    {
        lanesManager = GameObject.Find(lanesManagerObjectName).GetComponent<LanesManager>();
        musicSequencer = GameObject.Find(musicSequencerObjectName).GetComponent<MusicSequencer>();

        lanesManager.Initialize(numberOfLanes);
        musicSequencer.Initialize(numberOfLanes);

        playerActive = new bool[numberOfLanes];
        numberOfPlayersActive = numberOfLanes - startingNumberOfActivePlayers;
        for (int playerIndex = 0; playerIndex < numberOfLanes; playerIndex++)
        {
            if (playerIndex < startingNumberOfActivePlayers)
            {
                // Activate player
                playerActive[playerIndex] = false;
                ActivateDeactivatePlayer(playerIndex + 1);
            }
            else
            {
                // Deactivate player
                playerActive[playerIndex] = true;
                ActivateDeactivatePlayer(playerIndex + 1);
            }
        }

        if (numberOfPlayersActive > 0)
        {
            StartHeartbeat();
        }
    }


    // Update is called once per frame
    void Update()
    {
        updateCounter++;
        CheckAndDispatchKeyboardInput();
    }


    void TogglePlayPause()
    {
        if (numberOfPlayersActive == 0)
        {
            musicPaused = false;
            return;
        }
           
        if (musicPaused)
        {
            StartHeartbeat();
        }
        else
        {
            StopHeartbeat();
            StopAllMusic();
        }
        musicPaused = !musicPaused;
    }


    void ActivateDeactivatePlayer(int laneNumber)
    {
        int laneIndex = laneNumber - 1;

        if (playerActive[laneIndex])
        {
            playerActive[laneIndex] = false;
            lanesManager.HidePlayer(laneNumber);
            numberOfPlayersActive--;

            if (numberOfPlayersActive == 0)
            {
                StopHeartbeat();
                StopAllMusic();
                musicPaused = false;
            }
        }
        else
        {
            playerActive[laneIndex] = true;
            lanesManager.ShowPlayer(laneNumber);
            numberOfPlayersActive++;

            int bucketNumber = lanesManager.MovePlayer(laneIndex + 1, 0f);
            // lanesManager.SetPlayerColor(laneIndex + 1, bucketNumber);
            musicSequencer.SetNextClipNumber(laneIndex + 1, bucketNumber);

            if (numberOfPlayersActive == 1)
            {
                StartHeartbeat();
            }
        }
        
        Debug.Log("Number of players active = " + numberOfPlayersActive);
    }

    void StartMusic()
    {
        for (int playerIndex = 0; playerIndex < numberOfLanes; playerIndex++)
        {
            if (playerActive[playerIndex])
                musicSequencer.StartClip(playerIndex + 1);
        }
    }

    // Can't check for active players since they may have just been deactivated
    void StopAllMusic()
    {
        for (int playerIndex = 0; playerIndex < numberOfLanes; playerIndex++)
                musicSequencer.StopClip(playerIndex + 1);
    }

    void PlayersVisualPing()
    {
        for (int playerIndex = 0; playerIndex < numberOfLanes; playerIndex++)
        {
            if (playerActive[playerIndex])
                lanesManager.VisualPing(playerIndex + 1);
        }

    }

    void StartHeartbeat()
    {
        sequencerHeartbeat = PlayClipsRepeat();
        StartCoroutine(sequencerHeartbeat);
        Debug.Log("^^ Heartbeat started");
    }

    void StopHeartbeat()
    {
        StopCoroutine(sequencerHeartbeat);
        Debug.Log("^^ Heartbeat stopped");
    }

    public IEnumerator PlayClipsRepeat()
    {
        const float countdownInterval = 1f;

        while (true)
        {
            StartMusic();
            PlayersVisualPing();

            for (float remainingTime = clipDurationsSeconds; remainingTime > 0f; remainingTime -= countdownInterval)
            {
                Debug.Log("^^ Heartbeat countdown: " + remainingTime + " seconds left");
                yield return new WaitForSecondsRealtime(countdownInterval);
            }
            
        }
    }

    // ----------------------------------------------------------------------------------------------------------------
    //  Keyboard input 
    // ----------------------------------------------------------------------------------------------------------------

    int updateCounter = 0;  // For keyboard input
    int lastKeyCounter = 0;
    KeyCode[] addRemovePlayerKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    KeyCode[] playerStepBackKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O };
    KeyCode[] playerStepForwardKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L };

    KeyCode[] togglePlayPauseKeys = { KeyCode.Space, KeyCode.Tab, KeyCode.P };

    KeyCode[] quitKeys = { KeyCode.Escape, KeyCode.Delete, KeyCode.Backspace, KeyCode.End };


    private void CheckAndDispatchKeyboardInput()
    {
        bool keyPressed = false;
        foreach (KeyCode keyCode in quitKeys)
            if (Input.GetKey(keyCode))
                keyPressed = true;
        if (keyPressed && DebounceKeys())
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        keyPressed = false;
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
                int newBucketNumber = -1;
                if (Input.GetKey(playerStepBackKeys[keyIndex]))
                    newBucketNumber = lanesManager.MovePlayer(keyIndex + 1, playerMoveIncrementMeters);

                if (Input.GetKey(playerStepForwardKeys[keyIndex]))
                    newBucketNumber = lanesManager.MovePlayer(keyIndex + 1, -playerMoveIncrementMeters);

                if (newBucketNumber > -1)
                {
                    // lanesManager.SetPlayerColor(keyIndex + 1, newBucketNumber);
                    musicSequencer.SetNextClipNumber(keyIndex + 1, newBucketNumber);
                }
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

