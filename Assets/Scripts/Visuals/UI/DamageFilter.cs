using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFilter : MonoBehaviour
{

    public Animator top, left, bottom, right;
    public float fadeSpeed;

    private Coroutine TT, TL, TB, TR;
    private float maxA;


    // Start is called before the first frame update
    void Start()
    {
        //maxA = 0;
        //top.color = new Color(1, 1, 1, 0);
        //left.color = new Color(1, 1, 1, 0);
        //bottom.color = new Color(1, 1, 1, 0);
        //right.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        maxA = 0;
    }

    public void Trigger(int direction)
    {
        switch (direction)
        {
            case 0:
                if (TT != null)
                {
                    StopCoroutine(TT);
                }
                //TT = StartCoroutine(TriggerTop());

                top.SetTrigger("In");
                break;
            case 1:
                if (TL != null)
                {
                    StopCoroutine(TL);
                }
                left.SetTrigger("In");
                //TL = StartCoroutine(TriggerLeft());
                break;
            case 2:
                if (TB != null)
                {
                    StopCoroutine(TB);
                }
                bottom.SetTrigger("In");
                //TB = StartCoroutine(TriggerBottom());
                break;
            case 3:
                if (TR != null)
                {
                    StopCoroutine(TR);
                }
                right.SetTrigger("In");
                //TR = StartCoroutine(TriggerRight());
                break;
        }
    }

    /*
    IEnumerator TriggerTop()
    {

        top.color = new Color(1, 1, 1, 1);
        while (top.color.a > 0)
        {
            top.color = new Color(1, 1, 1, Mathf.Max(0, top.color.a - fadeSpeed * Time.deltaTime));
            if (top.color.a > maxA)
            {
                maxA = top.color.a;
            }
            VariableManager.variableManager.scriptGlitchEffect.intensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.flipIntensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.colorIntensity = maxA;
            yield return 0;
        }

    }

    IEnumerator TriggerLeft()
    {

        left.color = new Color(1, 1, 1, 1);
        while (left.color.a > 0)
        {
            left.color = new Color(1, 1, 1, Mathf.Max(0, left.color.a - fadeSpeed * Time.deltaTime));
            if (left.color.a > maxA)
            {
                maxA = left.color.a;
            }
            VariableManager.variableManager.scriptGlitchEffect.intensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.flipIntensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.colorIntensity = maxA;
            yield return 0;
        }

    }

    IEnumerator TriggerBottom()
    {

        bottom.color = new Color(1, 1, 1, 1);
        while (bottom.color.a > 0)
        {
            bottom.color = new Color(1, 1, 1, Mathf.Max(0, bottom.color.a - fadeSpeed * Time.deltaTime));
            if (bottom.color.a > maxA)
            {
                maxA = bottom.color.a;
            }
            VariableManager.variableManager.scriptGlitchEffect.intensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.flipIntensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.colorIntensity = maxA;
            yield return 0;
        }

    }

    IEnumerator TriggerRight()
    {

        right.color = new Color(1, 1, 1, 1);
        while (right.color.a > 0)
        {
            right.color = new Color(1, 1, 1, Mathf.Max(0, right.color.a - fadeSpeed * Time.deltaTime));
            if (right.color.a > maxA)
            {
                maxA = right.color.a;
            }
            VariableManager.variableManager.scriptGlitchEffect.intensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.flipIntensity = maxA;
            VariableManager.variableManager.scriptGlitchEffect.colorIntensity = maxA;
            yield return 0;
        }

    }*/
}
