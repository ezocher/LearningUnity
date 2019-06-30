using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSequencer : MonoBehaviour
{
    // Lane numbers are 1..N and Lane indexes are 0..N-1
    private int laneNumber;
    private int laneIndex;

    private AudioClip[] clips;
    
    private const string MusicClipsGameObjectName = "MusicClips";

    public void Initialize(int laneNumber, int numberOfClipsPerLane)
    {
        this.laneNumber = laneNumber;
        laneIndex = laneNumber - 1;
        MusicClips musicClips = GameObject.Find(MusicClipsGameObjectName).GetComponent<MusicClips>();
        clips = musicClips.clipLists[laneIndex].clips;

        if (clips.Length < numberOfClipsPerLane)
        {
            Debug.Log("*** Error: Lane #" + laneNumber + " - Only " + clips.Length + " clips available when " + numberOfClipsPerLane + " were expected");
        }
        else
        {
            // Try to preload all clips
            AudioSource sound = this.GetComponent<AudioSource>();
            for (int clipNumber = 0; clipNumber < clips.Length; clipNumber++)
                sound.clip = clips[clipNumber];
            Debug.Log("All clips loaded for Lane #" + laneNumber);
        }
    }

    private int nextClipNumber;

    // Start is called before the first frame update
    public void StartWithClipNumber(int clipNumber)
    {
    }

    public void TogglePlayPause()
    {
    }

    public void StopMusic()
    {
    }

    public void SetNextClipNumber(int clipNumber)
    {
        nextClipNumber = clipNumber;
        Debug.Log("* Lane #" + laneNumber + " - Next clip set to: " + nextClipNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
