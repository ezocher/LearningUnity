using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanesManager : MonoBehaviour
{
    public float laneSeparationAngleDegrees = 20f;
    public GameObject lanePrefab;

    const string laneNamePrefix = "LaneAndPlayer";

    private int numberOfLanes;
    private LaneRunner[] laneRunners;

    public void Initialize(int numLanes)
    {
        numberOfLanes = numLanes;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
