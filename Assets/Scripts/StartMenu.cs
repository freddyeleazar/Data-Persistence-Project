using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerNameText;
    [SerializeField]
    private TMP_Text bestScoreText;

    public string playerName;
    public int score;

    public string bestPlayerName;
    public int bestScore;
    
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

    private void Start()
    {
        LoadBestScoreData();
        bestScoreText.text = "Best Score: " + Instance.bestPlayerName + ": " + Instance.bestScore;
    }

    public void LoadMainScene()
    {
        playerName = playerNameText.text;
        score = 0;
        SceneManager.LoadScene(1);
    }

    [System.Serializable]
    class BestScoreData
    {
        public string newBestPlayerName;
        public int newBestScore;
    }

    public void SaveBestScoreData()
    {
        BestScoreData bestScoreData = new BestScoreData();
        bestScoreData.newBestPlayerName = Instance.bestPlayerName;
        bestScoreData.newBestScore = Instance.bestScore;

        string jsonBestScoreData = JsonUtility.ToJson(bestScoreData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", jsonBestScoreData);
    }

    public void LoadBestScoreData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string jsonBestScoreData = File.ReadAllText(path);
            BestScoreData bestScoreData = JsonUtility.FromJson<BestScoreData>(jsonBestScoreData);

            Instance.bestPlayerName = bestScoreData.newBestPlayerName;
            Instance.bestScore = bestScoreData.newBestScore;
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