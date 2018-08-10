using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class GunData : MonoBehaviour
{
    public static GunData instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public static List<Gun> guns = new List<Gun>();
    public static Dictionary<string, int> nameToGun = new Dictionary<string, int>(); //Dictionary to convert name of gun to index.
    public static bool[] gunsUnlocked;
    public TextAsset gunData;
    public GameObject ShotTracer;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            guns = new List<Gun>();
            nameToGun = new Dictionary<string, int>();
            //if not, set instance to this
            instance = this;
            setGuns();
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
    void setGuns()
    {
        StringReader GDLines = new StringReader(gunData.text);
        //Debug.Log(path);
        //Debug.Log(inputline);
        String inputline = "";
        GDLines.ReadLine();
        while ((inputline = GDLines.ReadLine()) != null)
        {
            string[] currentGunData = inputline.Split();
            string gunName = currentGunData[0];
            float gunDamage = float.Parse(currentGunData[1], CultureInfo.InvariantCulture.NumberFormat);
            float gunReload = float.Parse(currentGunData[2], CultureInfo.InvariantCulture.NumberFormat);
            float gunFireRate = float.Parse(currentGunData[3], CultureInfo.InvariantCulture.NumberFormat);
            //Debug.Log(currentGunData[4]);
            int gunClipSize = Int32.Parse(currentGunData[4]);
            bool gunAuto = currentGunData[5].Equals("1");
            float gunAccuracy = float.Parse(currentGunData[7], CultureInfo.InvariantCulture.NumberFormat);
            int gunPrice = Int32.Parse(currentGunData[8]);
            if (currentGunData[6].Equals("1"))
            {
                if (currentGunData[0].Contains("Shotgun"))
                {
                    Shotgun newShotgun = new Shotgun();
                    newShotgun.setVals(gunName, gunDamage, gunReload, gunFireRate, gunClipSize, gunAuto, gunAccuracy, ShotTracer, gunPrice);
                    guns.Add(newShotgun);
                }
                else if (currentGunData[0].Contains("Beam"))
                {
                    Beam newBeam = new Beam();
                    newBeam.setVals(gunName, gunDamage, gunReload, gunFireRate, gunClipSize, gunAuto, gunAccuracy, ShotTracer, gunPrice);
                    guns.Add(newBeam);
                }
                else
                {
                    Hitscan newHitscan = new Hitscan();
                    newHitscan.setVals(gunName, gunDamage, gunReload, gunFireRate, gunClipSize, gunAuto, gunAccuracy, ShotTracer, gunPrice);
                    guns.Add(newHitscan);
                }
            }
            else
            {
                Projectile newProj = new Projectile();
                newProj.setVals(gunName, gunDamage, gunReload, gunFireRate, gunClipSize, gunAuto, gunAccuracy, ShotTracer, gunPrice);
                guns.Add(newProj);

            }
            nameToGun.Add(gunName, guns.Count - 1);
        }
        //Debug.Log(guns.Count);
    }
}
