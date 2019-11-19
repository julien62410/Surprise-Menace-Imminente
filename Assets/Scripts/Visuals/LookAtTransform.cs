using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
    [SerializeField] private Transform lookAt;

    private void OnDrawGizmos()
    {
        UpdateLookAt();   
    }

    private void Update()
    {
        UpdateLookAt();
    }

    private void UpdateLookAt()
    {
        if (lookAt)
        {
            transform.LookAt(lookAt);
        }
    }
}
