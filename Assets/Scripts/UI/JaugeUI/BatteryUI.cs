using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryUI : JaugeUI
{
    private static BatteryUI instance;
    public static BatteryUI Instance
    {
        get
        {
            if (!instance) instance = FindObjectOfType<BatteryUI>();
            return instance;
        }
    }

    [SerializeField] private ImageFill batteryFill;
    private float initialBattery;

    private void Start()
    {
        if(VariableManager.variableManager)
            initialBattery = VariableManager.variableManager.battery;
    }

    private void Update()
    {
        float currentBattery = VariableManager.variableManager.battery;
        if (batteryFill) batteryFill.SetFill(initialBattery, currentBattery);
    }
}
