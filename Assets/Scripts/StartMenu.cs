using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : MonoBehaviour
{
    public static StartMenu Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    private TMP_Text playerNameText;
    [SerializeField]
    private TMP_Text bestScoresText;

    public Score currentScore;
    public PersistentData persistentData;

    private void Start()
    {
        Instance.persistentData.LoadBestScores();
        foreach (var score in Instance.persistentData.bestScores)
        {
            Instance.bestScoresText.text += score.ToString() + "\n";
        }
    }

    public void LoadMainScene()
    {
        Instance.currentScore = new Score(playerNameText.text, 0);
        SceneManager.LoadScene(1);
    }

    [System.Serializable]
    public class PersistentData
    {
        public List<Score> bestScores;
        
        public void LoadBestScores()
        {
            if (bestScores == null)
            {
                bestScores = new List<Score>();
            }
            if (File.Exists(Application.persistentDataPath + "/bestScores.json"))
            {
                string json = File.ReadAllText(Application.persistentDataPath + "/bestScores.json");
                bestScores = JsonUtility.FromJson<PersistentData>(json).bestScores;
            }
        }

        public void SaveBestScores()
        {
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(Application.persistentDataPath + "/bestScores.json", json);
        }
    }
    
    public class Score
    {
        public string playerName;
        public int score;

        public Score(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }

        public void SaveScore()
        {
            Instance.persistentData.bestScores.Add(this);
            Instance.persistentData.SaveBestScores();
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}