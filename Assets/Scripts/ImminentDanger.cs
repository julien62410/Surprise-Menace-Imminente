using UnityEngine;

public class ImminentDanger : MonoBehaviour
{

    private float ratio;

    void Start()
    {
        VariableManager.variableManager.battery = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        VariableManager.variableManager.battery = Mathf.Max(0, VariableManager.variableManager.battery - VariableManager.variableManager.batteryUsage * Time.deltaTime);
        BatteryUI.Instance.Lose();
        ratio = 1f;
        Collider[] overlaped = Physics.OverlapSphere(transform.position, VariableManager.variableManager.radiusGlitch, LayerMask.GetMask("Enemy"));
        if(overlaped.Length>0)
        {
            ratio = 0f;
        }
        VariableManager.variableManager.scriptGlitchEffect.intensity = 1 - ratio;
        VariableManager.variableManager.scriptGlitchEffect.flipIntensity = 1 - ratio;
        VariableManager.variableManager.scriptGlitchEffect.colorIntensity = 1 - ratio;

        overlaped = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Battery"));
        if(overlaped.Length > 0)
        {
            EnnemySpawn.Instance.CollisionWithPlayer(overlaped[0].gameObject);
        }
    }
}
