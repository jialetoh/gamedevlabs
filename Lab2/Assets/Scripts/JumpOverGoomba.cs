using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class JumpOverGoomba : MonoBehaviour
{
    public Transform enemyLocation;
    public TextMeshProUGUI scoreText;
    private bool onGroundState;

    [System.NonSerialized] public static int score = 0;

    private bool countScoreState = false;
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;

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

        // when jumping, and Goomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
            //Debug.Log("Entered jump + score state");
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                //Debug.Log("incrementing score..");
                score++;
                scoreText.text = "Score: " + score.ToString();
            }
        }
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

    // helper
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
}
