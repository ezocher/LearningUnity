using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanesManager : MonoBehaviour
{
    private float laneAngleTotalSpanDegrees = 60f;

    public GameObject lanePrefab;

    const string laneNamePrefix = "LaneAndPlayer";

    private int numberOfLanes;
    private LaneRunner[] laneRunners;

    public void Initialize(int numLanes)
    {

        numberOfLanes = numLanes;

        float laneSeparationAngleDegrees = laneAngleTotalSpanDegrees / (numberOfLanes - 1);

        laneRunners = new LaneRunner[numberOfLanes];

        float laneAngleY = -(laneSeparationAngleDegrees * (numberOfLanes - 1)) / 2;

        for (int laneNumber = 1; laneNumber <= numberOfLanes; laneNumber++)
        {
            Quaternion laneYRotation = Quaternion.Euler(0, laneAngleY, 0);
            laneAngleY += laneSeparationAngleDegrees;

            GameObject newLane = Instantiate(lanePrefab, Vector3.zero, laneYRotation);
            newLane.transform.parent = this.transform;
            newLane.transform.localPosition = Vector3.zero;  // Reset position
            newLane.name = laneNamePrefix + laneNumber.ToString();

            laneRunners[laneNumber - 1] = newLane.GetComponent<LaneRunner>();

            laneRunners[laneNumber - 1].Initialize(laneNumber);
        }
    }

    public void HidePlayer(int laneNumber) => laneRunners[laneNumber - 1].HidePlayer();

    public void ShowPlayer(int laneNumber) => laneRunners[laneNumber - 1].ShowPlayer();

    public int MovePlayer(int laneNumber, float distance) => laneRunners[laneNumber - 1].MovePlayer(distance);

    public void VisualPing(int laneNumber) => laneRunners[laneNumber - 1].VisualPing();

    public float GetPlayerPosition(int laneNumber) => laneRunners[laneNumber - 1].GetPlayerPosition();

    public void SetPlayerColor(int laneNumber, int colorNumber) => laneRunners[laneNumber - 1].SetPlayerColor(colorNumber);
}
