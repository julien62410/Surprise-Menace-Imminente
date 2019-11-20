using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class DetectSurfaces : MonoBehaviour
{
    public GameObject plane;
    public List<DetectedPlane> detectedPlanes = new List<DetectedPlane>();

    public bool isInit = false;

    private void Update()
    {
        Session.GetTrackables<DetectedPlane>(detectedPlanes, TrackableQueryFilter.All);

        if (detectedPlanes.Count > 0)
        {
            for (int i = 0; i < detectedPlanes.Count; i++)
            {
                Pose pose = detectedPlanes[i].CenterPose;
                GameObject _plane = Instantiate(plane, pose.position, pose.rotation);
                _plane.transform.localScale = new Vector3(detectedPlanes[i].ExtentX / 10, detectedPlanes[i].ExtentX / 10, detectedPlanes[i].ExtentZ / 10);
            }
        }

    }
}
