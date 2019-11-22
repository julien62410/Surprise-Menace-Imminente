using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.Events;

public class DetectSurfaces : MonoBehaviour
{
    private static DetectSurfaces _instance;
    public static DetectSurfaces Instance { get { return _instance; } }

    public List<DetectedPlane> m_detectedPlanes = new List<DetectedPlane>();
    public List<GameObject> listPlaneInScene = new List<GameObject>();

    public UnityAction<GameObject> PlaneFoundInWorld;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        CheckLostTracking();

        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        foreach (GameObject plane in listPlaneInScene)
        {
            plane.SetActive(true);
        }

        Session.GetTrackables<DetectedPlane>(m_detectedPlanes, TrackableQueryFilter.New);

        for (int i = 0; i < m_detectedPlanes.Count; i++)
        {
            DisplayPlane(m_detectedPlanes[i]);

        }
    }

    private void DisplayPlane(DetectedPlane m_detectedPlane)
    {
        Pose pose = m_detectedPlane.CenterPose;
        Anchor anchor = m_detectedPlane.CreateAnchor(pose);
        PlaneFoundInWorld.Invoke(anchor.gameObject);

        GameObject planeObject =
            Instantiate(VariableManager.variableManager.surfacePrefab, anchor.transform.position, anchor.transform.rotation);

        listPlaneInScene.Add(planeObject);
        planeObject.transform.localScale = new Vector3(m_detectedPlane.ExtentX / 10, m_detectedPlane.ExtentX / 10, m_detectedPlane.ExtentZ / 10);

        planeObject.transform.SetParent(anchor.transform);
    }

    private void CheckLostTracking() // SOURCE : GOOGLE HELLO AR
    {
        if (Session.Status == SessionStatus.LostTracking &&
            Session.LostTrackingReason != LostTrackingReason.None)
        {
            foreach (GameObject plane in listPlaneInScene)
            {
                plane.SetActive(false);
            }

            switch (Session.LostTrackingReason)
            {
                case LostTrackingReason.InsufficientLight:
                    Debug.Log("Too dark. Try moving to a well-lit area.");
                    break;
                case LostTrackingReason.InsufficientFeatures:
                    Debug.Log("Aim device at a surface with more texture or color.");
                    break;
                case LostTrackingReason.ExcessiveMotion:
                    Debug.Log("Moving too fast. Slow down.");
                    break;
                default:
                    Debug.Log("Motion tracking is lost.");
                    break;
            }
            return;
        }
    }
}
