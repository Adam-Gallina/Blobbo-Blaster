using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelUI : MonoBehaviour
{

    Button[] buttons;
    bool[] levelProgress;
    Color unlocked = new Color32(255, 255, 255, 255);
    Color locked = new Color32(10, 10, 10, 255);
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        levelProgress = PlayerPrefsX.GetBoolArray("LevelProgress", false, buttons.Length);
        if (buttons.Length != levelProgress.Length)
        {
            bool[] levelProgressNew = new bool[buttons.Length];
            levelProgressNew.Copy<bool>(levelProgress);
            levelProgress = levelProgressNew;
        }
        PlayerPrefsX.SetBoolArray("LevelProgress", levelProgress);
        for (int i = 0; i < buttons.Length; i++)
        {
            Button currentButton = buttons[i];
            int levelnum = Int32.Parse(currentButton.name);
            if (levelnum == 0 || levelProgress[levelnum - 1] == true)
            {
                currentButton.onClick.AddListener(delegate { LoadLevel(levelnum); });
                buttons[i].GetComponent<Image>().color = unlocked;
            }
            else
            {
                currentButton.onClick.RemoveAllListeners();
                buttons[i].GetComponent<Image>().color = locked;
            }

        }
        //Debug.Log(levelProgress.Length);
    }
    void LoadLevel(int levelnum)
    {
        if ((MenuTutorial.run_tut && MenuTutorial.tut_complete) || !MenuTutorial.run_tut)
        {
            SceneManager.LoadScene(levelnum + 1);
        }
    }
    public void ClearProgress()
    {
        levelProgress = PlayerPrefsX.GetBoolArray("LevelProgress", false, buttons.Length);
        levelProgress.Fill<bool>(false);
        PlayerPrefsX.SetBoolArray("LevelProgress", levelProgress);
        PlayerPrefs.SetInt("coins", 0);
        Start();
    }
    public void UnlockAll()
    {
        levelProgress = PlayerPrefsX.GetBoolArray("LevelProgress", true, buttons.Length);
        levelProgress.Fill<bool>(true);
        PlayerPrefsX.SetBoolArray("LevelProgress", levelProgress);
        PlayerPrefs.SetInt("coins", 1000000);
        Start();
    }
}
