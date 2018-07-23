using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour {

    public GameObject upgradeTab;
    public GameObject levelsTab;

    //Start Menu
	public void StartPressed()
    {
        ChangeToScene(0);
    }

    //Level Select
    public void UpgradePressed()
    {
        upgradeTab.SetActive(true);
        levelsTab.SetActive(false);
    }
    public void LevelsPressed()
    {
        upgradeTab.SetActive(false);
        levelsTab.SetActive(true);
    }

    //Change Scene
    public void ChangeToScene(int scene)
    {
        if (scene == -1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(scene+1);
        }
    }
}
