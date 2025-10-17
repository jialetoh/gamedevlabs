using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class JumpOverGoomba : Singleton<JumpOverGoomba>
{
    public GameManager gameManager;
    public EnemyManager enemyManager;
    public PlayerMovement playerMovement;
    public GameObject enemies;
    public Transform enemyLocation;
    //public TextMeshProUGUI scoreText;
    private bool onGroundState;

    //[System.NonSerialized] public static int score = 0;

    private bool countScoreState = false;

    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    public LayerMask enemyLayerMask;


    void Awake()
    {
        // other instructions
        // subscribe to Game Restart event
        // Debug.Log("PlayerMovement Awake called");
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        // mario jumps
        if (Keyboard.current.spaceKey.wasPressedThisFrame && OnGroundCheck())
        {
            onGroundState = false;
            countScoreState = true;
        }

        playerMovement.OnEnemyCheck = OnEnemyCheck();

        // when jumping, and Goomba is near Mario and we haven't registered our score
        // if (!onGroundState && countScoreState)
        // {
        //     // if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
        //     // {
        //     //     countScoreState = false;
        //     //     //Debug.Log("incrementing score..");
        //     //     // score++;
        //     //     // scoreText.text = "Score: " + score.ToString();
        //     //     gameManager.IncreaseScore(1);
        //     // }
        // }

        if (!onGroundState && countScoreState)
        {
            countScoreState = false;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Rebind scene-specific objects here
        enemies = GameObject.Find("Enemies");
        enemyManager = enemies.GetComponent<EnemyManager>();
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("collision with ground");
            onGroundState = true;
        }
    }

    private bool OnGroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask))
        {
            //Debug.Log("onGroundCheck: on ground");
            return true;
        }
        else
        {
            //Debug.Log("onGroundCheck: not on ground");
            return false;
        }
    }

    public bool OnEnemyCheck()
    {

        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, enemyLayerMask))
        {
            Debug.Log("Jumped on an enemy");
            return true;
        }
        else
        {
            return false;
        }
    }

    // helper
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
}
