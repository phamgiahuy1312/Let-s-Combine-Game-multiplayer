using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Linq;

public class GameOver : MonoBehaviourPunCallbacks
{
    public Text scoreDisplay;
    public Text namePlayer1;
    public Text namePlayer2;
    public GameObject restartButton;
    public GameObject saveandreturnToMenuButton;
    public GameObject waitingText;

    void Start()
    {
        scoreDisplay.text = PlayerStats.score.ToString();

        // Hiển thị tên của hai người chơi
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            namePlayer1.text = PhotonNetwork.PlayerList[0].NickName;
            namePlayer2.text = PhotonNetwork.PlayerList[1].NickName;
        }

        // Xử lý hiển thị nút restart và nút quay lại menu
        if (!PhotonNetwork.IsMasterClient)
        {
            restartButton.SetActive(false);
            saveandreturnToMenuButton.SetActive(false);
            waitingText.SetActive(true);
            
        }
        else
        {
            restartButton.SetActive(true);
            saveandreturnToMenuButton.SetActive(true);
            waitingText.SetActive(false);


        }
    }

    public void OnClickRestart()
    {
        photonView.RPC("Restart", RpcTarget.All);   
    }

    public void OnClickSaveAndReturnToMenu()
    {
        photonView.RPC("SaveAndReturnToMenu", RpcTarget.All);
    }

    [PunRPC]
    void Restart()
    {
        Time.timeScale = 1;
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 1f;
        PhotonNetwork.LoadLevel("Game");
        if (!PhotonNetwork.IsMasterClient)
        {
            PlayerStats.score = 0;
            PlayerStats.health = 15;

        }
        else
        {
            PlayerStats.score = 0;
            PlayerStats.health = 15;
        }
    }

    [PunRPC]
    void SaveAndReturnToMenu()
    {
        Time.timeScale = 1;
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 1f;
        string filePath = Path.Combine(Application.streamingAssetsPath, "highscores.json");

        // Đọc dữ liệu hiện có từ tệp JSON nếu có
        string jsonData = "";
        if (File.Exists(filePath))
        {
            jsonData = File.ReadAllText(filePath);
        }

        // Chuyển đổi dữ liệu JSON thành danh sách điểm số
        HighScoreDataList dataList = new HighScoreDataList();
        if (!string.IsNullOrEmpty(jsonData))
        {
            dataList = JsonUtility.FromJson<HighScoreDataList>(jsonData);
        }

        // Tạo một đối tượng mới chứa điểm số cần lưu
        HighScoreData newData = new HighScoreData();
        newData.player1Name = namePlayer1.text;
        newData.player2Name = namePlayer2.text;
        newData.score = scoreDisplay.text;

        // Thêm điểm số mới vào danh sách
        dataList.highscores.Add(newData);

        // Sắp xếp lại danh sách theo thứ tự điểm từ cao đến thấp
        dataList.highscores = dataList.highscores.OrderByDescending(x => int.Parse(x.score)).ToList();

        // Chuyển đổi danh sách thành chuỗi JSON
        string updatedJsonData = JsonUtility.ToJson(dataList);

        // Ghi lại dữ liệu đã được cập nhật vào tệp JSON
        File.WriteAllText(filePath, updatedJsonData);
        if (!PhotonNetwork.IsMasterClient)
        {
            PlayerStats.score = 0;
            PlayerStats.health = 15;

        }
        else
        {
            PlayerStats.score = 0;
            PlayerStats.health = 15;
        }

        // Quay lại menu
        PhotonNetwork.LoadLevel("Main Menu");
    }

    [System.Serializable]
    private class HighScoreDataList
    {
        public List<HighScoreData> highscores = new List<HighScoreData>();
    }

    [System.Serializable]
    private class HighScoreData
    {
        public string player1Name;
        public string player2Name;
        public string score;
    }
}
