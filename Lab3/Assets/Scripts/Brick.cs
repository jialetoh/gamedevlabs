using UnityEngine;

public class Brick : MonoBehaviour
{
    public Animator brickAnimator;
    public bool isCoinBrick;

    public Animator coinAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (collision.GetContact(0).normal.y > 0f)
        {
            // Debug.Log("collision with brick: " + collision.GetContact(0).normal.y);
            // Collision from below = move the brick
            brickAnimator.SetTrigger("hit");

            // If brick contains coin, spawn the coin
            if (isCoinBrick)
            {
                coinAnimator.SetTrigger("spawnCoin");
            }
        }
    }
}