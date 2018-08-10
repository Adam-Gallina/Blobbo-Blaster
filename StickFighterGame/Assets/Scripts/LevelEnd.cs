using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    GameObject panel;
    GameObject nextLevel;
    GameObject restart;
    Text message;
    Text coinText;
    GameManager GM;
    public Button[] buttons;
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        panel = transform.Find("Panel").gameObject;
        nextLevel = panel.transform.Find("NextLevel").gameObject;
        restart = panel.transform.Find("Restart").gameObject;
        message = panel.transform.Find("Message").GetComponent<Text>();
        coinText = panel.transform.Find("Coins").GetComponent<Text>();
        panel.SetActive(false);
        buttons = panel.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            switch (buttons[i].name)
            {
                case "NextLevel":
                    buttons[i].onClick.AddListener(delegate { GM.NextLevel(); });
                    break;
                case "Restart":
                    buttons[i].onClick.AddListener(delegate { GM.PlayerDeath(); });
                    break;
                case "LevelSelect":
                    buttons[i].onClick.AddListener(delegate { GM.GoLevelSelect(); });
                    break;
            }
        }
    }
    
    void Update()
    {
    }

    public void EndLevel(string condition)
    {
        panel.SetActive(true);
        int lastCoin = PlayerPrefs.GetInt("coins", 0);
        
        coinText.text = "You got " + Player.coins + " coins!";
        Time.timeScale = 0f;
        if (condition == "Dead")
        {
            PlayerPrefs.SetInt("coins", lastCoin + Player.coins);
            message.text = "You have died...";
            restart.SetActive(true);
            nextLevel.SetActive(false);
            Player.coins = 0;
        }
        else if (condition == "Win")
        {
            if (GameObject.Find("Player").GetComponent<Player>().health == 100)
            {
                coinText.text +=  "\n" + "Full health: +50 coins!";
                Player.coins += 50;
            }
            if(SceneManager.GetActiveScene().name == "_Tutorial")
            {
                MenuTutorial.run_tut = true;
                coinText.text += "\n" + "Tutorial Completed: +50 coins!";
                Player.coins += 50;
            }
            PlayerPrefs.SetInt("coins", lastCoin + Player.coins);
            Player.coins = 0;
            message.text = "Level Complete!";
            nextLevel.SetActive(true);
            restart.SetActive(false);
            bool[] levelProgress = PlayerPrefsX.GetBoolArray("LevelProgress", false, SceneManager.sceneCountInBuildSettings - 1);
            levelProgress[SceneManager.GetActiveScene().buildIndex - 1] = true;
            PlayerPrefsX.SetBoolArray("LevelProgress", levelProgress);
        }
    }
}
