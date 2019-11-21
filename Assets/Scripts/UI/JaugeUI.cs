using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaugeUI : MonoBehaviour
{
    [Header("Jauge")]
    [SerializeField] private Animator animator;

    public virtual void Earn()
    {
        animator.SetTrigger("Earn");
    }

    public virtual void Lose()
    {
        animator.SetTrigger("Lose");
    }
}
