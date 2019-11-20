using UnityEngine;

public class ImminentDanger : MonoBehaviour
{
<<<<<<< HEAD

    public GlitchEffect effect;
    public float radius, batteryUsage;

    [HideInInspector]
    public float battery;

=======
>>>>>>> 1a6cf45db2614ecd1c8877e390ce6bcbea0a5c16
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
        Collider[] overlaped = Physics.OverlapSphere(transform.position, VariableManager.variableManager.radiusGlitchAndSound, LayerMask.GetMask("Enemy"));
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
        VariableManager.variableManager.scriptGlitchEffect.intensity = 1 - ratio;
        VariableManager.variableManager.scriptGlitchEffect.flipIntensity = 1 - ratio;
        VariableManager.variableManager.scriptGlitchEffect.colorIntensity = 1 - ratio;
    }
}
