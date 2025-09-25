using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;

    public Vector3 startPosition = new(0.0f, -3.824f, 0.0f);

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Goomba collided with: " + collision.gameObject.name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) >= maxOffset)
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
        }

        // move goomba
        Movegoomba();
    }
}