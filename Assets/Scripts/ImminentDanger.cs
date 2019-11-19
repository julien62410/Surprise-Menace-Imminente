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
        Collider[] overlaped = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider c in overlaped)
        {
            if ((c.transform.position - transform.position).magnitude < ratio)
            {
                ratio = (c.transform.position - transform.position).magnitude;
            }
        }
        effect.intensity = 1 - ratio;
        effect.flipIntensity = 1 - ratio;
        effect.colorIntensity = 1 - ratio;
    }
}
