using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovement : MonoBehaviour
{
    public float speed = 30f;
    public float maxSpeed = 3f;
    public float upSpeed = 14.5f;
    private bool onGroundState = false;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool isFirstJump = true;

    public Transform gameCamera;

    public GameObject enemies;

    // UI
    public TextMeshProUGUI scoreText;
    public GameObject scorePanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    // Animation
    public Animator marioAnimator;

    // Audio
    public AudioSource marioAudio;

    // Death
    public AudioClip marioDeath;
    public float deathImpulse = 15;
    public bool alive = true;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    public void RestartButtonCallback()
    {
        // Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // reset speed
        marioBody.linearVelocity = Vector2.zero;

        // reset position
        marioBody.transform.position = new Vector3(-7.37f, -3f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        JumpOverGoomba.score = 0;
        gameOverPanel.SetActive(false);
        scorePanel.SetActive(true);

        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        isFirstJump = true;


        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);

        // reset Goombas
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
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
            Debug.Log("Mario collided with goomba!");

            // kill mario
            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    void ShowGameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;

        gameOverPanel.SetActive(true);
        scorePanel.SetActive(false);
        finalScoreText.text = "Score: " + JumpOverGoomba.score;
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
    }

    // Update is called once per frame
    void Update()
    {
        if ((Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if ((marioBody.linearVelocity.x > 0.1f) && onGroundState)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if ((Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if ((marioBody.linearVelocity.x < -0.1f) && onGroundState)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        if (alive)
        {
            // Horizontal movement
            float moveHorizontal = 0f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                moveHorizontal -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                moveHorizontal += 1f;

            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector2 movement = new(moveHorizontal, 0);
                // check if it doesn't go beyond maxSpeed
                if (Mathf.Abs(marioBody.linearVelocityX) < maxSpeed)
                    marioBody.AddForce(movement * speed);
            }

            // stop
            if (Keyboard.current.aKey.wasReleasedThisFrame || Keyboard.current.leftArrowKey.wasReleasedThisFrame ||
            Keyboard.current.dKey.wasReleasedThisFrame || Keyboard.current.rightArrowKey.wasReleasedThisFrame)
            {
                // stop
                marioBody.linearVelocityX = 0;
            }

            // Vertical movement
            if (Keyboard.current.spaceKey.wasPressedThisFrame && onGroundState)
            {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGroundState = false;

                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
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

}