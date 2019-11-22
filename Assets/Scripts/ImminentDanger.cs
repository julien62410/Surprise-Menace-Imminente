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
        Collider[] overlaped = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Battery", "Multi", "Heart"));
        BatteryUI.Instance.Lose();

        foreach (Collider col in overlaped)
        {
            EnnemySpawn.Instance.CollisionWithPlayer(col.gameObject);
        }
    }
}
