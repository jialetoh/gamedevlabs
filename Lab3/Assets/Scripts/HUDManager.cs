using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    // private Vector3[] scoreTextPosition = {
    //     new Vector3(-805, 483, 0),
    //     new Vector3(0, 0, 0)
    //     };
    // private Vector3[] restartButtonPosition = {
    //     new Vector3(815, 472, 0),
    //     new Vector3(0, -120, 0)
    // };
    // private Vector3[] gameOverTextPosition =
    // {
    //     new Vector3(0, 100, 0)
    // };

    //public GameObject scoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public Transform restartButton;

    public GameObject gameOverPanel;
    public GameObject scorePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        Debug.Log("HUD started");
        // set active panels
        gameOverPanel.SetActive(false);
        scorePanel.SetActive(true);

        // scoreText.transform.localPosition = scoreTextPosition[0];
        // restartButton.localPosition = restartButtonPosition[0];

        // // reset score
        // scoreText.text = "Score: 0";
        // JumpOverGoomba.score = 0;
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        scorePanel.SetActive(false);
        // scoreText.transform.localPosition = scoreTextPosition[1];
        // restartButton.localPosition = restartButtonPosition[1];
    }

    //     gameOverPanel.SetActive(true);
    //     scorePanel.SetActive(false);
    //     finalScoreText.text = "Score: " + JumpOverGoomba.score;
    // }
}
