using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryVisuals : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ParticleSystem pickupFx;
    [SerializeField] private Collider coll;
    [SerializeField] private GameObject[] meshes;

    public void Pickup()
    {
        pickupFx.Play();
        coll.enabled = false;
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].SetActive(false);
        }
        StartCoroutine(Deactivating());
    }

    private IEnumerator Deactivating()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].SetActive(true);
        }
        coll.enabled = true;
        transform.parent.gameObject.SetActive(false);
    }
}
