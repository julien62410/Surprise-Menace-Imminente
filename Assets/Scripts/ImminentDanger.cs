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
        Collider[] overlaped = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Battery"));
        BatteryUI.Instance.Lose();

        if(overlaped.Length > 0)
        {
            EnnemySpawn.Instance.CollisionWithPlayer(overlaped[0].gameObject);
        }
    }
}
