using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{

    GameObject panel;
    GameManager GM;
    public bool paused = false;
    public Button[] buttons;
    void Start()
    {
        Time.timeScale = 1f;
        GM = FindObjectOfType<GameManager>();
        panel = transform.Find("Panel").gameObject;
        panel.SetActive(false);
        buttons = panel.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            switch (buttons[i].name)
            {
                case "Resume":
                    buttons[i].onClick.AddListener(delegate { Pause(); });
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
        if (Input.GetButtonDown("Restart"))
        {
            GM.PlayerDeath();
        }
    }
    public void Pause()
    {
        paused = !paused;
        if (paused == true)
        {
            Time.timeScale = 0f;
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
