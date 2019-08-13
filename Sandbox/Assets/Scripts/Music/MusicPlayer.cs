using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Lane numbers are 1..N and Lane indexes are 0..N-1
    private int laneNumber;
    private int laneIndex;

    private AudioClip[] clips;
    
    private const string MusicClipsGameObjectName = "MusicClips";

    private int nextClipNumber = 0;

    public void Initialize(int laneNumber, int numberOfClipsPerLane)
    {
        this.laneNumber = laneNumber;
        laneIndex = laneNumber - 1;
        MusicClips musicClips = GameObject.Find(MusicClipsGameObjectName).GetComponent<MusicClips>();
        clips = musicClips.clipLists[laneIndex].clips;

        if (clips.Length < numberOfClipsPerLane)
        {
            Debug.Log("*** FATAL ERROR: Lane #" + laneNumber + " - Only " + clips.Length + " clips available when " + numberOfClipsPerLane + " were expected");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            // Try to preload all clips
            AudioSource sound = this.GetComponent<AudioSource>();
            for (int clipNumber = 0; clipNumber < clips.Length; clipNumber++)
                sound.clip = clips[clipNumber];
            Debug.Log("> All music clips loaded for Lane #" + laneNumber);
        }
    }


    public void StartClip()
    {
        AudioSource music = this.GetComponent<AudioSource>();
        music.clip = clips[nextClipNumber];
        music.loop = false;

        music.Play();
        Debug.Log("> Lane #" + laneNumber + ": Playing clip #" + nextClipNumber);
    }

    public void StopClip()
    {
        AudioSource music = this.GetComponent<AudioSource>();
        if (music.isPlaying)
        {
            music.Stop();
            Debug.Log("> Lane #" + laneNumber + ": Music stopped");
        }

    }

    public void Pause()
    {
        AudioSource music = this.GetComponent<AudioSource>();

        if (music.isPlaying)
        {
            music.Pause();
            Debug.Log("> Lane #" + laneNumber + ": Music paused");
        }
    }

    public void UnPause()
    {
        AudioSource music = this.GetComponent<AudioSource>();

        music.UnPause();
        Debug.Log("> Lane #" + laneNumber + ": UnPaused");
    }

    public void SetNextClipNumber(int playerDistanceBucket)
    {
        nextClipNumber = playerDistanceBucket - 1;
        Debug.Log("> Lane #" + laneNumber + " - Next clip set to: " + nextClipNumber);
    }
}
