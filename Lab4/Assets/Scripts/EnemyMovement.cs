using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private bool dead = false;

    public Vector3 startPosition = new(0.0f, -3.824f, 0.0f);

    public AudioSource goombaDeathAudio;

    public GameManager gameManager;

    // Animation
    public Animator goombaAnimator;

    void OnTriggerEnter2D(Collider2D collision)
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Goomba collision entered");
        moveRight *= -1;
        ComputeVelocity();
        MoveGoomba();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // other instructions
        // subscribe to Game Restart event
        // Debug.Log("PlayerMovement Awake called");
        SceneManager.sceneLoaded += OnSceneLoaded;
        goombaDeathAudio = GameObject.FindGameObjectWithTag("GoombaAudio").GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

    }
    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
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
        MoveGoomba();
    }

    public void GameRestart()
    {
        // reset Goombas
        
        gameObject.SetActive(true);
        dead = false;
        goombaAnimator.SetBool("dead", dead);
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void MoveGoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void ActivateObject()
    {
        gameObject.SetActive(false);
    }

    public void DestroyGoomba()
    {
        Debug.Log("EnemyMovement: Destroying Goomba");
        dead = true;
        goombaAnimator.SetBool("dead", dead);
        goombaDeathAudio.PlayOneShot(goombaDeathAudio.clip);
        Invoke(nameof(ActivateObject), 0.4f);

    }
}