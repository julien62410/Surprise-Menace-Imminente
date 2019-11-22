using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAudio : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource sweesh;
    public GameObject tuto;
    public GameObject readyText;

    private Animator anim;
    private bool sweeshed;
    private VariableManager v;

    void Start()
    {
        sweeshed = false;
        v = VariableManager.variableManager;
        anim = GetComponent<Animator>();
        StartCoroutine(Sweesh());
        if(!v) anim.SetTrigger("In");
        else tuto.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (v)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tuto.SetActive(true);
                anim.SetTrigger("In");
                if (v) v.startGame = true;
            }

        }
    }

    IEnumerator Sweesh()
    {
        bool c = true;
        while (c && !sweeshed)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("In") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.24f && !sweeshed)
            {
                c = false;
                sweesh.Play();
                sweeshed = true;
            }
            yield return 0;
        }
        c = true;
        while (c)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Out"))
            {
                c = false;
                sweesh.Play();
            }
            yield return 0;
        }
    }
}
