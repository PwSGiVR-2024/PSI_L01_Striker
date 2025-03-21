using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isReversing = false;
    private float animationTime = 0.0f;
    public float animationSpeed = 0.1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Scene", 0, 0);
            animator.speed = 0; // Zatrzymanie automatycznego odtwarzania animacji
        }
        else
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (animator != null)
        {
            if (!isReversing)
            {
                animationTime += Time.deltaTime * animationSpeed;
                if (animationTime >= 1.0f)
                {
                    animationTime = 1.0f;
                    isReversing = true;
                }
            }
            else
            {
                animationTime -= Time.deltaTime * animationSpeed;
                if (animationTime <= 0.0f)
                {
                    animationTime = 0.0f;
                    isReversing = false;
                }
            }

            animator.Play("Scene", 0, animationTime);
        }
    }
}



