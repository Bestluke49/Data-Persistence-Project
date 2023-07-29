using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText, highScoreText;
    public GameObject GameOverText, menuButton;
    
    private bool m_Started = false;
    private int m_Points, highScore;
    
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
                Vector3 position = new(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        if (Manager.Instance != null)
        {
            ScoreText.text = $"{Manager.Instance.playerName}'s Score : 0";
        }
        else
        {
            ScoreText.text = $"Score : 0";
        }

        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.highScore;
            highScoreText.text = $"Best Score : {data.highName} : {highScore}";
        }
        else
        {
            highScoreText.text = "Best Score : None";
        }
        
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                menuButton.SetActive(false);
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        if (Manager.Instance != null)
        {
            ScoreText.text = $"{Manager.Instance.playerName}'s Score : {m_Points}";
        }
        else
        {
            ScoreText.text = $"Score : {m_Points}";
        }
        
        if (m_Points > highScore && Manager.Instance != null)
        {
            highScoreText.text = $"Best Score : {Manager.Instance.playerName} : {m_Points}";
        }
    }

    public void GameOver()
    {
        if (m_Points > highScore && Manager.Instance != null)
        {
            SaveData data = new()
            {
                highName = Manager.Instance.playerName,
                highScore = m_Points
            };
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }        

        m_GameOver = true;
        menuButton.SetActive(true);
        GameOverText.SetActive(true);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

    [System.Serializable]
    class SaveData
    {
        public string highName;
        public int highScore;
    }
}
