using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paintAnims : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator.SetBool("idle", true);  // Start in idle
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            StartPaint();
        }

        if (Input.GetMouseButtonDown(1)) // Right click
        {
            StartAbsorb();
        }
    }

    public void StartPaint()
    {
        animator.SetBool("paint", true);
        animator.SetBool("idle", false);
        StartCoroutine(WaitForPaintToFinish());
    }

    public void StartAbsorb()
    {
        animator.SetBool("absorb", true);
        animator.SetBool("idle", false);
        StartCoroutine(WaitForAbsorbToFinish());
    }

    private IEnumerator WaitForPaintToFinish()
    {
        // Wait until the paint animation state exits
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("paintbrushPaint") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        );

        animator.SetBool("paint", false);
        animator.SetBool("idle", true);
    }
    private IEnumerator WaitForAbsorbToFinish()
    {
        // Wait until the paint animation state exits
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("paintbrushAbsorb") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        );

        animator.SetBool("absorb", false);
        animator.SetBool("idle", true);
    }
}
