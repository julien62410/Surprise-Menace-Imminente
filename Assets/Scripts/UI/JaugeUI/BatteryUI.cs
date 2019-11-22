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
    private VariableManager v;

    private void Start()
    {
        v = VariableManager.variableManager;
        if (v)
            initialBattery = v.battery;
    }

    private void Update()
    {
        if (v)
        {
            float currentBattery = v.battery;
            if (batteryFill) batteryFill.SetFill(initialBattery, currentBattery);
            animator.SetBool("empty", v.battery == 0);
        }
    }
}
