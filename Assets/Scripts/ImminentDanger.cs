﻿using UnityEngine;

public class ImminentDanger : MonoBehaviour
{

    public GlitchEffect effect;
    public float radius, batteryUsage;

    [HideInInspector]
    public float battery;

    private float ratio;

    void Start()
    {
        battery = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        battery = Mathf.Max(0, battery - batteryUsage * Time.deltaTime);
        ratio = 1f;
        Collider[] overlaped = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy"));
        foreach(Collider enemy in overlaped)
        {
            AudioSource audio = enemy.GetComponent<AudioSource>();
            float magnitude = (enemy.transform.position - transform.position).magnitude; // Distance entre le player et les enemy
            if (magnitude < ratio)
            {
                ratio = magnitude;
            }
            if (audio)
            {
                audio.volume = 1f - magnitude;
            }
        }
        effect.intensity = 1 - ratio;
        effect.flipIntensity = 1 - ratio;
        effect.colorIntensity = 1 - ratio;
    }
}
