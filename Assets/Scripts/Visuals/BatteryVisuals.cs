using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryVisuals : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ParticleSystem pickupFx;

    public void Pickup()
    {
        pickupFx.Play();
        StartCoroutine(Deactivating());
    }

    private IEnumerator Deactivating()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
