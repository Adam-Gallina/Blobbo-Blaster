using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGunImages : MonoBehaviour
{
    List<Gun> guns = GunData.guns;
    GameObject Weapon1;
    GameObject Weapon2;
    private void Awake()
    {
        Weapon1 = transform.Find("Weapon 1").gameObject;
        Weapon2 = transform.Find("Weapon 2").gameObject;

    }
    void Update()
    {
        guns = GunData.guns;
        int[] currentGuns = PlayerPrefsX.GetIntArray("gunsEquipped", 0, 2);
        //Debug.Log(guns.Count);
        GameObject Gun1 = transform.Find(guns[currentGuns[0]].gunName).gameObject;
        GameObject Gun2 = transform.Find(guns[currentGuns[1]].gunName).gameObject;
        foreach (Transform child in Weapon1.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in Weapon2.transform)
        {
            Destroy(child.gameObject);
        }
        if (currentGuns[0] == currentGuns[1])
        {
            GameObject Image1 = Object.Instantiate(Gun1);
            Image1.transform.SetParent(Weapon1.transform, false);
            Image1.SetActive(true);
            return;
        }
        GameObject newImage1 = Object.Instantiate(Gun1);
        GameObject newImage2 = Object.Instantiate(Gun2);
        newImage1.transform.SetParent(Weapon1.transform, false);
        newImage2.transform.SetParent(Weapon2.transform, false);
        newImage1.SetActive(true);
        newImage2.SetActive(true);
    }
}
