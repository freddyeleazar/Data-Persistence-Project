using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StartMenu;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text lbl_bestScore;
    public Text lbl_bestPlayer;
    public int bestScore;
    
    private bool m_Started = false;
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        RefreshBestScoreText();
        InitializeScoreText();
    }

    private void RefreshBestScoreText()
    {
        if(Instance != null)
        {
            List<Score> bestScores = Instance.persistentData.bestScores;

            Score bestScore = bestScores.Find(x => x.score == bestScores.Max(x => x.score));
            lbl_bestScore.text = bestScore.score.ToString();
            lbl_bestPlayer.text = bestScore.playerName;
        }
    }

    private void InitializeScoreText()
    {
        Instance.currentScore.score = 0;
        if (Instance != null)
        {
            ScoreText.text = "Score: " + Instance.currentScore.score.ToString();
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if(StartMenu.Instance.currentScore.score > StartMenu.Instance.persistentData.bestScores.Min(x => x.score))
            {
                Instance.persistentData.
                StartMenu.Instance.bestPlayerName = StartMenu.Instance.playerName;
                StartMenu.Instance.bestScore = StartMenu.Instance.score;
                RefreshBestScoreText();
                StartMenu.Instance.SaveBestScoreData();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        StartMenu.Instance.score += point;
        ScoreText.text = $"Score : {StartMenu.Instance.score}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
