using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneRunner : MonoBehaviour
{
    private float laneClosestDistanceMeters = 1f;
    private float laneFarthestDistanceMeters = 5f;

    private float visualPingDuration = 0.5f;

    private int laneNumber;  // Lane numbers are 1..N and Lane indexes are 0..N-1

    private GameObject playerObject;
    private const string playerObjectName = "Player";

    private Material[] playerColorMaterials;

    // playerNumberOfColors should equal MusicSequencer's numberOfClipsPerLane
    private int playerNumberOfColors;
    private const string playerColorMaterialsGameObjectName = "PlayerColorMaterials";

    public void Initialize(int laneNum)
    {
        laneNumber = laneNum;

        playerObject = this.transform.Find(playerObjectName).gameObject;

        PlayerColorMaterials matContainer = GameObject.Find(playerColorMaterialsGameObjectName).GetComponent<PlayerColorMaterials>();
        playerColorMaterials = matContainer.colorMaterials;
        playerNumberOfColors = matContainer.numberOfColors;
    }

    public void HidePlayer()
    {
        Debug.Log("-@ Player #" + laneNumber + ": Deactivated");
        playerObject.SetActive(false);
    }

    public void ShowPlayer()
    {
        Debug.Log("+@ Player #" + laneNumber + ": Activated");
        playerObject.SetActive(true);
    }

    //Returns distance bucket number that this player is in after moving 
    // Ranges from 1 to Number of colors (also = Number of clips per lane)
    public int MovePlayer(float distance)
    {
        float newZ = playerObject.transform.localPosition.z + distance;
        if ((newZ <= laneFarthestDistanceMeters) && (newZ >= laneClosestDistanceMeters))
        {
            Debug.Log("@ Player #" + laneNumber + ": Moving " + distance + " meters");
            playerObject.transform.Translate(0f, 0f, distance);
        }

        return GetPlayerBucket();
    }

    public int GetPlayerBucket()
    {
        // Math.Min is necessary because this would return 1 too high when GetPlayerPosition is exactly 1.0
        return Math.Min((int)(GetPlayerPosition() * (float)playerNumberOfColors) + 1, playerNumberOfColors);
    }

    // Default ("no color") is index 0
    // Colors are indexes 1..NumberOfColors
    public void SetPlayerColor(int colorNumber)
    {
        if ((colorNumber >= 1) && (colorNumber <= playerNumberOfColors))
        {
            Debug.Log("@ Player #" + laneNumber + ": Color set to #" + colorNumber);
            playerObject.GetComponent<Renderer>().material = playerColorMaterials[colorNumber];
        }
    }

    // Default color is index 0
    public void SetPlayerDefaultColor()
    {
        Debug.Log("@ Player #" + laneNumber + ": Color set to default");
        playerObject.GetComponent<Renderer>().material = playerColorMaterials[0];
    }

    // Returns a value from 0.0 to 1.0
    public float GetPlayerPosition()
    {
        return (playerObject.transform.localPosition.z - laneClosestDistanceMeters) / (laneFarthestDistanceMeters - laneClosestDistanceMeters);
    }

    public void VisualPing()
    {
        int colorNumber = GetPlayerBucket();
        SetPlayerColor(colorNumber);
        Invoke("SetPlayerDefaultColor", visualPingDuration);
    }
}

