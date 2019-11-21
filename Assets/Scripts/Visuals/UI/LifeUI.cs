using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private ImageFill lifeFill;
    private VariableManager v;
    private int initialLife = 0;

    private void Start()
    {
        v = VariableManager.variableManager;
        if (v) initialLife = v.lifePlayer;
    }

    private void Update()
    {
        if(lifeFill) lifeFill.SetFill(initialLife, v.lifePlayer);
    }
}
