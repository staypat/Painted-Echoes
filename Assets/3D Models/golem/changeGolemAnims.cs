using UnityEngine;

public class changeGolemAnims : MonoBehaviour
{
    public Animator animator;
    public string animationStateName = "golemRoll";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (animator != null)
            {
                bool currentValue = animator.GetBool("roll");
                animator.SetBool("roll", !currentValue);
            }
            else
            {
                Debug.LogWarning("Animator not assigned!");
            }
        }
    }
}