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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseAll()
    {
        Debug.Log("= Music paused");
    }

    public void UnpauseAll()
    {
        Debug.Log("> Music playing");
    }
}