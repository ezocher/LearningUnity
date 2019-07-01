using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSequencer : MonoBehaviour
{
    public int numberOfClipsPerLane = 3;
    public GameObject musicPlayerPrefab;

    const string musicPlayerNamePrefix = "MusicPlayer";

    private int numberOfLanes;
    private MusicPlayer[] musicPlayers;

    public void Initialize(int numLanes)
    {
        numberOfLanes = numLanes;

        musicPlayers = new MusicPlayer[numberOfLanes];

        for (int laneNumber = 1; laneNumber <= numberOfLanes; laneNumber++)
        {
            GameObject newMusicPlayer = Instantiate(musicPlayerPrefab, Vector3.zero, Quaternion.identity);
            newMusicPlayer.name = musicPlayerNamePrefix + laneNumber.ToString();
            newMusicPlayer.transform.parent = this.transform;

            musicPlayers[laneNumber - 1] = newMusicPlayer.GetComponent<MusicPlayer>();

            musicPlayers[laneNumber - 1].Initialize(laneNumber, numberOfClipsPerLane);
        }
    }

    public void StartClip(int laneNumber) => musicPlayers[laneNumber - 1].StartClip();

    public void StopClip(int laneNumber) => musicPlayers[laneNumber - 1].StopClip();


    public void SetNextClipNumber(int laneNumber, int newClipNumber) => musicPlayers[laneNumber - 1].SetNextClipNumber(newClipNumber);

    public void PauseAll(bool[] playerActive)
    {
        for (int laneIndex = 0; laneIndex < playerActive.Length; laneIndex++)
            if (playerActive[laneIndex])
                musicPlayers[laneIndex].Pause();
    }

    public void UnpauseAll(bool[] playerActive)
    {
        for (int laneIndex = 0; laneIndex < playerActive.Length; laneIndex++)
            if (playerActive[laneIndex])
                musicPlayers[laneIndex].UnPause();
    }


}