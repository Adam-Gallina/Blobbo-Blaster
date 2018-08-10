using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    public Text money;
    public GameObject startMenu;
    public GameObject upgradesTab;
    public GameObject levelsTab;
    public GameObject upgrades;
    public GameObject levels;

    [SerializeField]
    private static bool started = false;

    private void Start()
    {
        
        if (!started)
        {
            startMenu.SetActive(true);
            levelsTab.SetActive(false);
            upgradesTab.SetActive(false);
            levels.SetActive(false);
            upgrades.SetActive(false);
            started = true;
        }
        else
        {
            startMenu.SetActive(false);
            levelsTab.SetActive(true);
            levels.SetActive(true);
            upgrades.SetActive(true);
        }
    }

    private void Update()
    {
        int coins = PlayerPrefs.GetInt("coins", 0);
        if (Input.GetKey("'") && Input.GetKey("="))
        {
            coins += 100;
        }
        PlayerPrefs.SetInt("coins", coins);
        money.text = coins.ToString();        
    }
    //Start Menu
    public void StartPressed()
    {
        levelsTab.SetActive(true);
        startMenu.SetActive(false);
        levels.SetActive(true);
        upgrades.SetActive(true);
    }

    //Upgrades/Level Select
    public void UpgradesPressed()
    {
        upgradesTab.SetActive(true);
        levelsTab.SetActive(false);
        MenuTutorial.upgrade = true;
    }
    public void LevelsPressed()
    {
        upgradesTab.SetActive(false);
        levelsTab.SetActive(true);
        MenuTutorial.upgrade = false;
    }

    public void tutPressed()
    {
        //Change guns to AR and Pistol
        int[] setDefaults = PlayerPrefsX.GetIntArray("gunsEquipped", 0, 2);
        setDefaults[0] = GunData.nameToGun["Pistol"];
        setDefaults[1] = GunData.nameToGun["AssaultRifle"];
        PlayerPrefsX.SetIntArray("gunsEquipped", setDefaults);
        ChangeToScene(1);
    }
    //Generic Scene Changer for level buttons
    public void ChangeToScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }




}
