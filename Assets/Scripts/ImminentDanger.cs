using UnityEngine;

public class ImminentDanger : MonoBehaviour
{
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
        battery = Mathf.Max(0, battery - VariableManager.variableManager.batteryUsage * Time.deltaTime);
        ratio = 1f;
        Collider[] overlaped = Physics.OverlapSphere(transform.position, VariableManager.variableManager.radiusGlitch, LayerMask.GetMask("Enemy"));
        if(overlaped.Length>0)
        {
            ratio = 0f;
        }
        VariableManager.variableManager.scriptGlitchEffect.intensity = 1 - ratio;
        VariableManager.variableManager.scriptGlitchEffect.flipIntensity = 1 - ratio;
        VariableManager.variableManager.scriptGlitchEffect.colorIntensity = 1 - ratio;
    }
}
