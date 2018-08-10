using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GunSelection : MonoBehaviour
{
    int[] gunsEquipped;
    Button[] buttons;
    GameObject slots;
    GameObject buyGunRect;
    bool[] gunsUnlocked;
    Color unlocked = new Color32(255, 255, 255, 255);
    Color locked = new Color32(133, 133, 133, 255);
    private void Awake()
    {
        slots = transform.Find("Slots").gameObject;
        buyGunRect = transform.Find("Buy Gun").gameObject;
    }
    private void Start()
    {
        buttons = GetComponentsInChildren<Button>(true);
        gunsUnlocked = PlayerPrefsX.GetBoolArray("gunsUnlocked", false, buttons.Length - 2);
        gunsUnlocked[0] = true;
        if (buttons.Length - 2 != gunsUnlocked.Length)
        {
            bool[] gunsUnlockedNew = new bool[buttons.Length - 2];
            gunsUnlockedNew.Copy<bool>(gunsUnlocked);
            gunsUnlocked = gunsUnlockedNew;
        }
        PlayerPrefsX.SetBoolArray("gunsUnlocked", gunsUnlocked);
        gunsEquipped = PlayerPrefsX.GetIntArray("gunsEquipped", 0, 2);
        for (int i = 0; i < buttons.Length; i++)
        {
            int x = i;
            Button currentButton = buttons[x];
            if (GameObject.ReferenceEquals(currentButton.transform.parent.gameObject, slots))
            {
                currentButton.onClick.RemoveAllListeners();
                currentButton.onClick.AddListener(delegate { setGunSlot(Int32.Parse(currentButton.name) - 1); });
                //Debug.Log(buttons[x].name);
                continue;
            }
            if (currentButton.gameObject.transform.name.Equals("Buy"))
            {
                currentButton.onClick.RemoveAllListeners();
                currentButton.onClick.AddListener(delegate
                {
                    buyGun(currentButton);
                });
                continue;
            }
            if (gunsUnlocked[GunData.nameToGun[currentButton.gameObject.transform.name]] == true)
            {
                currentButton.image.color = unlocked;
                currentButton.onClick.RemoveAllListeners();
                currentButton.onClick.AddListener(delegate { openSlots(currentButton.gameObject); });
            }
            else
            {
                currentButton.image.color = locked;
                currentButton.onClick.RemoveAllListeners();
                currentButton.onClick.AddListener(delegate { openBuy(currentButton); });
            }
        }

    }
    private void openSlots(GameObject button)
    {
        if (slots.activeSelf == true && slots.transform.parent == button.transform.parent)
        {
            slots.SetActive(false);
            return;
        }
        slots.transform.SetParent(button.transform.parent, false);
        slots.SetActive(true);
    }
    private void openBuy(Button button)
    {
        if (buyGunRect.activeSelf == true && buyGunRect.transform.parent == button.transform.parent)
        {
            buyGunRect.SetActive(false);
            return;
        }
        buyGunRect.transform.SetParent(button.transform.parent, false);
        Text buyText = buyGunRect.transform.Find("Buy").Find("Text").GetComponent<Text>();
        buyText.text = "Buy for $" + GunData.guns[GunData.nameToGun[button.transform.parent.transform.name]].price;
        buyGunRect.SetActive(true);
    }
    private void setGunSlot(int slot)
    {
        int newGun = GunData.nameToGun[slots.transform.parent.name];
        if (slots.transform.parent.name.Equals("AssaultRifle"))
        {
            MenuTutorial.equipped = true;
        }
        int otherSlot = slot > 0 ? 0 : 1;
        if (gunsEquipped[otherSlot] == newGun)
        {
            gunsEquipped[otherSlot] = gunsEquipped[slot];
        }
        gunsEquipped[slot] = GunData.nameToGun[slots.transform.parent.name];
        PlayerPrefsX.SetIntArray("gunsEquipped", gunsEquipped);
    }
    private void buyGun(Button currentButton)
    {
        if (PlayerPrefs.GetInt("coins", 0) >= GunData.guns[GunData.nameToGun[buyGunRect.transform.parent.name]].price)
        {
            Debug.Log(buyGunRect.transform.parent.name);
            Debug.Log(MenuTutorial.run_tut);
            if (buyGunRect.transform.parent.name.Equals("AssaultRifle") && MenuTutorial.run_tut == true)
            {
                Debug.Log("worked?");
                MenuTutorial.assault = true;
            }
            int newBalance = PlayerPrefs.GetInt("coins", 0) - GunData.guns[GunData.nameToGun[buyGunRect.transform.parent.name]].price;
            PlayerPrefs.SetInt("coins", newBalance);
            gunsUnlocked[GunData.nameToGun[buyGunRect.transform.parent.name]] = true;
            PlayerPrefsX.SetBoolArray("gunsUnlocked", gunsUnlocked);
            buyGunRect.SetActive(false);
            Start();
        }
    }
    public void clearProgress()
    {
        bool[] gunsUnlocked = PlayerPrefsX.GetBoolArray("gunsUnlocked");
        gunsUnlocked.Fill<bool>(false);
        gunsUnlocked[0] = true;
        PlayerPrefsX.SetBoolArray("gunsUnlocked", gunsUnlocked);
        gunsEquipped[0] = 0;
        gunsEquipped[1] = 0;
        PlayerPrefsX.SetIntArray("gunsEquipped", gunsEquipped);
        Start();
    }
}
