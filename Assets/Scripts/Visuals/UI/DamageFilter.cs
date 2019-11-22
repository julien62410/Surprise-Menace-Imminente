using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFilter : MonoBehaviour
{

    public Animator top, left, bottom, right;

    private float maxA;


    // Start is called before the first frame update
    void Start()
    {
        maxA = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trigger(int direction)
    {
        switch (direction)
        {
            case 0:
                StopAllCoroutines();
                StartCoroutine(TriggerGlitch());
                top.SetTrigger("In");
                break;
            case 1:
                StopAllCoroutines();
                StartCoroutine(TriggerGlitch());
                left.SetTrigger("In");
                break;
            case 2:
                StopAllCoroutines();
                StartCoroutine(TriggerGlitch());
                bottom.SetTrigger("In");
                break;
            case 3:
                StopAllCoroutines();
                StartCoroutine(TriggerGlitch());
                right.SetTrigger("In");
                break;
        }
    }
    IEnumerator TriggerGlitch()
    {
        maxA = 1;
        while (maxA > 0)
        {
            if (!VariableManager.variableManager.gameOver)
            {
                maxA = Mathf.Max(0, maxA - Time.deltaTime);
            }
            VariableManager.variableManager.scriptGlitchEffect.intensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.flipIntensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.colorIntensity = maxA;
            yield return 0;
        }

    }

}
