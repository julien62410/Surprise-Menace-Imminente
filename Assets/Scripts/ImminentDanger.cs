using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImminentDanger : MonoBehaviour
{

    public GlitchEffect effect;
    public float radius;

    private float ratio;

    // Start is called before the first frame update
    void Start()
    {
        ratio = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ratio = 1f;
        Collider[] overlaped = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy"));
        foreach(Collider c in overlaped)
        {
            AudioSource a = c.GetComponent<AudioSource>();
            float mag = (c.transform.position - transform.position).magnitude;
            if (mag < ratio)
            {
                ratio = mag;
            }
            if (a)
            {
                a.volume = 0.5f - mag;
            }
        }
        effect.intensity = 1 - ratio;
        effect.flipIntensity = 1 - ratio;
        effect.colorIntensity = 1 - ratio;
    }
}
