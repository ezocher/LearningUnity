using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneRunner : MonoBehaviour
{
    public float laneClosestDistanceMeters = 1f;
    public float laneFarthestDistanceMeters = 5f;

    private int laneNumber;  // Lane numbers are 1..N and Lane indexes are 0..N-1
    private int laneIndex;

    public void Initialize(int laneNum)
    {
        laneNumber = laneNum;
        laneIndex = laneNumber - 1;
    }

    public void HidePlayer()
    {
        Debug.Log("- Player #" + laneNumber + ": Deactivated");
    }

    public void ShowPlayer()
    {
        Debug.Log("+ Player #" + laneNumber + ": Activated");
    }

    public void MovePlayer(float distance)
    {
        Debug.Log("<> Player #" + laneNumber + ": Moving " + distance + " meters");
    }
}

