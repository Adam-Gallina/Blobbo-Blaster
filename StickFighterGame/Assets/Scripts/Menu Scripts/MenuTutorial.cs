using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTutorial : MonoBehaviour
{

    public static bool run_tut = false;
    public static bool upgrade = false;
    public static bool assault = false;
    public static bool equipped = false;
    public static bool tut_complete;

    public Text text;
    public GameObject image;

    private void Start()
    {
        tut_complete = PlayerPrefsX.GetBool("tut_complete", false);
    }
    void Update()
    {
        bool[] gunsUnlocked = PlayerPrefsX.GetBoolArray("gunsUnlocked");
        if (gunsUnlocked == null)
        {
            return;
        }
        if (run_tut && !tut_complete && (PlayerPrefs.GetInt("coins", 0) >= 50 || assault))// && gunsUnlocked[GunData.nameToGun["AssaultRifle"]] == false )
        {
            image.SetActive(true);
            text.enabled = true;
            text.text = "Click on the 'Shop' tab to buy a new gun";
            if (upgrade)
            {
                text.text = "Click on the Assault Rifle, and click 'Buy'";
                if (assault)
                {
                    text.text = "Click on the Assault Rifle again, and click '2' to equip it to slot 2";
                    if (equipped)
                    {
                        text.text = "Press 'Q' and 'E' to switch guns in levels\n<Press 'Enter' to end the tutorial>";
                    }
                    
                    if (Input.GetAxis("Submit") > 0)
                    {
                        PlayerPrefsX.SetBool("tut_complete", true);
                        tut_complete = true;
                    }
                }
            }
        }
        else
        {
            image.SetActive(false);
            text.enabled = false;
        }
    }
    public void ClearProgress()
    {
        PlayerPrefsX.SetBool("tut_complete", false);
        run_tut = false;
        upgrade = false;
        assault = false;
        equipped = false;
        tut_complete = false;
    }
}
