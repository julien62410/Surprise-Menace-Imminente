using UnityEngine;

public class ImminentDanger : MonoBehaviour
{
    void Start()
    {
        VariableManager.variableManager.battery = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        VariableManager.variableManager.battery = Mathf.Max(0, VariableManager.variableManager.battery - VariableManager.variableManager.batteryUsage * Time.deltaTime);
        Collider[] overlaped = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Battery", "X2", "Heart"));
        BatteryUI.Instance.Lose();

        foreach (Collider col in overlaped)
        {
            EnnemySpawn.Instance.CollisionWithPlayer(col.gameObject);
        }
    }
}
