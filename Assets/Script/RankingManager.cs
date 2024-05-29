using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    public Text rankingText;
    public Text player1NameText;
    public Text scoreText; 

    void Start()
    {
        LoadDataFromJSON();
    }

    void LoadDataFromJSON()
    {
        string filePath = Application.streamingAssetsPath + "/highscores.json";

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);

            //chuyển dữ liệu từ json thành object trong C#
            HighScoreDataList dataList = JsonUtility.FromJson<HighScoreDataList>(jsonData);

            // sắp xếp dữ liệu theo điểm số giảm dần
            dataList.highscores = dataList.highscores.OrderByDescending(x => int.Parse(x.score)).ToArray();

            // hiện thị dữ liệu
            if (dataList != null && dataList.highscores.Length > 0)
            {
                for (int i = 0; i < dataList.highscores.Length; i++)
                {
                    HighScoreData scoreData = dataList.highscores[i];
                    int ranking = i + 1;
                    rankingText.text += ranking + "\n\n";
                    player1NameText.text += scoreData.player1Name + " - " + scoreData.player2Name + "\n\n";
                    scoreText.text += scoreData.score + "\n\n";
                }
            }
            else
            {
                player1NameText.text = "No data";
                scoreText.text = "No data";
            }
        }
        else
        {
            player1NameText.text = "File not found";
            scoreText.text = "";
        }
    }

    [System.Serializable]
    private class HighScoreDataList
    {
        public HighScoreData[] highscores;
    }

    [System.Serializable]
    private class HighScoreData
    {
        public string player1Name;
        public string player2Name;
        public string score;
    }
}
