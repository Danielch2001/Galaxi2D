using System.Collections;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    private Animator animator;
    private float animationLength;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            // Obtén el clip de animación actual
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                animationLength = clipInfo[0].clip.length;
                StartCoroutine(DestroyAfterDelay(animationLength));
            }
            else
            {
                Debug.LogWarning("No animation clip found on " + gameObject.name);
                Destroy(gameObject); // Si no hay clip, destruye el objeto inmediatamente
            }
        }
        else
        {
            Debug.LogWarning("No Animator component found on " + gameObject.name);
            Destroy(gameObject); // Si no hay Animator, destruye el objeto inmediatamente
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

