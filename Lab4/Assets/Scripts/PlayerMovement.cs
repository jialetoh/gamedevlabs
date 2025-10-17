using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovement : Singleton<PlayerMovement>
{
    public float speed = 30f;
    public float maxSpeed = 3f;
    public float upSpeed = 14.5f;
    private bool onGroundState = false;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool isFirstJump = true;
    private bool moving = false;
    private bool jumpedState = false;

    [System.NonSerialized]
    public bool OnEnemyCheck = false;

    public Transform gameCamera;

    public GameObject enemies;

    public GameManager gameManager;
    public EnemyManager enemyManager;

    // Animation
    public Animator marioAnimator;

    // Audio
    public AudioSource marioAudio;

    // Death
    public AudioSource marioDeathAudio;
    public float deathImpulse = 15;

    [System.NonSerialized]
    public bool alive = true;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // public void RestartButtonCallback()
    // {
    //     // Debug.Log("Restart!");
    //     // reset everything
    //     GameRestart();
    //     // resume time
    //     Time.timeScale = 1.0f;
    // }

    public void GameRestart()
    {
        // reset speed
        marioBody.linearVelocity = Vector2.zero;

        // reset position
        marioBody.transform.position = new Vector3(-7.37f, -3f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        isFirstJump = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && alive)
        {
            if (OnEnemyCheck)
            {
                // stomp goomba
                Debug.Log("PlayerMovement stomping goomba");
                gameManager.StompGoomba();
                Debug.Log(collision.gameObject.ToString());
                enemyManager.DestroyGoomba(collision.gameObject);
            }
            else
            {
                // kill mario
                Debug.Log("Mario collided with Goomba and he is dead.");
                marioAnimator.Play("mario-die");
                marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
                alive = false;

                Invoke(nameof(TriggerGameOver), 1f);
            }
        }
    }
    void Awake()
    {
        // other instructions
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
        // Debug.Log("PlayerMovement Awake called");
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SetStartingPosition;
    }
    public void SetStartingPosition(Scene current, Scene next)
    {
        if (next.name == "World-1-2")
        {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(-10.2399998f, -4.3499999f, 0.0f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
        // Debug.Log("Mario position: " + marioBody.transform.position.ToString());
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
        if (!moving)
        {
            // stop
            marioBody.linearVelocityX = 0;
            // if (Keyboard.current.aKey.wasReleasedThisFrame || Keyboard.current.leftArrowKey.wasReleasedThisFrame ||
            // Keyboard.current.dKey.wasReleasedThisFrame || Keyboard.current.rightArrowKey.wasReleasedThisFrame)
            // {
            //     // stop
            //     marioBody.linearVelocityX = 0;
            // }
        }
    }

    void TriggerGameOver()
    {
        gameManager.GameOver();
    }

    void PlayJumpSound()
    {
        // Don't play the jump sound at the start
        if (isFirstJump)
        {
            isFirstJump = false;
            return;
        }

        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayDeathImpulse()
    {
        Debug.Log("Playing death impulse");
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");
        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }

        // if ((Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        // && faceRightState)
        // {
        //     faceRightState = false;
        //     marioSprite.flipX = true;
        //     if ((marioBody.linearVelocity.x > 0.1f) && onGroundState)
        //     {
        //         marioAnimator.SetTrigger("onSkid");
        //     }
        // }

        // if ((Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        // && !faceRightState)
        // {
        //     faceRightState = true;
        //     marioSprite.flipX = false;
        //     if ((marioBody.linearVelocity.x < -0.1f) && onGroundState)
        //     {
        //         marioAnimator.SetTrigger("onSkid");
        //     }
        // }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);

        // // Horizontal movement
        // float moveHorizontal = 0f;
        // if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        //     moveHorizontal -= 1f;
        // if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        //     moveHorizontal += 1f;

        // if (Mathf.Abs(moveHorizontal) > 0)
        // {
        //     Vector2 movement = new(moveHorizontal, 0);
        //     // check if it doesn't go beyond maxSpeed
        //     if (Mathf.Abs(marioBody.linearVelocityX) < maxSpeed)
        //         marioBody.AddForce(movement * speed);
        // }
    }
    private void OnSceneLoaded  (Scene scene, LoadSceneMode mode)
    {
        // Rebind scene-specific objects here
        enemies = GameObject.Find("Enemies");
        enemyManager = enemies.GetComponent<EnemyManager>();    
    }


    [SerializeField]
    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    [SerializeField]
    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
        // // Vertical movement
        // if (Keyboard.current.spaceKey.wasPressedThisFrame && onGroundState)
        // {
        //     marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        //     onGroundState = false;

        //     // update animator state
        //     marioAnimator.SetBool("onGround", onGroundState);
        // }
    }

    [SerializeField]
    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;
        }
    }
}