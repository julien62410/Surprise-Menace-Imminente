using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAudio : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource sweesh;

    private Animator anim;
    private bool sweeshed;

    void Start()
    {
        sweeshed = false;
        anim = GetComponent<Animator>();
        StartCoroutine(Sweesh());
    }

    // Update is called once per frame
    void Update()
    {

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
