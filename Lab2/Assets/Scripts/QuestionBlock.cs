using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    public Animator questionBlockAnimator;
    public Animator coinAnimator;

    private bool isDisabled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDisabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || isDisabled) return;

        if (collision.GetContact(0).normal.y > 0f)
        {
            Debug.Log("collision with question?: " + collision.GetContact(0).normal.y);
            // Collision from below = move the brick
            questionBlockAnimator.SetTrigger("hit");

            coinAnimator.SetTrigger("spawnCoin");

            isDisabled = true;
        }
    }
}
