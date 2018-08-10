using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class HealthBar
{
    public LineRenderer bar;
    public LineRenderer health;

    public float barLength = 2f;
    public float barHeight = .5f;
}

public class Player : MonoBehaviour
{
    //int currentScene;

    GameManager GM;
    public Camera[] cameras;
    private GameObject levelEnd;
    public List<GameObject> endEnemies = new List<GameObject>();

    public float maxhealth;
    public float health;

    public HealthBar healthBar;

    public Gun gun;
    public GameObject gunObject;

    List<Gun> guns;
    Dictionary<string, int> nameToGun;

    public HashSet<int> gunsEquipped = new HashSet<int>();
    List<Gun> playerGuns = new List<Gun>();
    public Dictionary<int, GameObject> playerGunsObjects = new Dictionary<int, GameObject>();
    int currentGun = 0;
    public float gunAccuracy;
    public float crouchAccuracy;

    [HideInInspector]
    public static int coins;
    private bool ended = false;
    private void Awake()
    {
        cameras = gameObject.GetComponentsInChildren<Camera>();
    }
    private void Start()
    {
        Player.coins = 0;
        levelEnd = GameObject.Find("Portal");
        levelEnd.SetActive(false);
        //coins = PlayerPrefs.GetInt("coins", 0);
        Physics2D.IgnoreLayerCollision(9, 10);
        Physics2D.IgnoreLayerCollision(9, 2);
        Physics2D.IgnoreLayerCollision(10, 2);
        Physics2D.IgnoreLayerCollision(2, 2);
        //currentScene = SceneManager.GetActiveScene().buildIndex;
        GM = FindObjectOfType<GameManager>();

        gunObject = transform.Find("Guns").gameObject;
        guns = GunData.guns;
        nameToGun = GunData.nameToGun;
        int[] gunSlots = PlayerPrefsX.GetIntArray("gunsEquipped", 0, 2);
        gunsEquipped.Add(gunSlots[0]);
        gunsEquipped.Add(gunSlots[1]);
        //Debug.Log(gunSlots[0]);
        //Debug.Log(gunSlots[1]);
        for (int i = 0; i < guns.Count; i++)
        {
            if (gunsEquipped.Contains(i))
            {
                Gun clone = new Gun();
                if (guns[i] is Shotgun)
                {
                    clone = new Shotgun();
                }
                else if (guns[i] is Beam)
                {
                    clone = new Beam();
                }
                else if (guns[i] is Hitscan)
                {
                    clone = new Hitscan();
                }
                else if (guns[i] is Projectile)
                {
                    clone = new Projectile();
                }
                clone.setVals(guns[i]);
                playerGuns.Add(clone);
                Debug.Log(nameToGun[clone.gunName]);
                playerGunsObjects.Add(nameToGun[clone.gunName], gunObject.transform.Find(clone.gunName).gameObject);
            }
        }
        gun = playerGuns[currentGun];
        playerGunsObjects[nameToGun[playerGuns[currentGun].gunName]].SetActive(true);
        gunAccuracy = gun.accuracy;
        crouchAccuracy = gunAccuracy / 2;

        if (healthBar.barHeight == 0) { healthBar.barHeight = 0.5f; }
        if (healthBar.barLength == 0) { healthBar.barLength = 2f; }

        healthBar.bar.startWidth = healthBar.barHeight / 2;
        healthBar.bar.material.color = Color.red;
        healthBar.bar.positionCount = 2;

        healthBar.health.startWidth = healthBar.barHeight / 2;
        healthBar.health.material.color = Color.green;
        healthBar.health.positionCount = 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coins")
        {
            coins++;
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "HealthPack")
        {
            health += 25;
            if (health > 100) { health = 100; }
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Level End" && levelEnd.activeSelf)
        {
            GameObject.Find("EndMenu").GetComponent<LevelEnd>().EndLevel("Win");
            ended = true;
        }
    }
    private void Update()
    {
        DrawHealth();
        gun = playerGuns[currentGun];
        bool dead = true;
        if (gun.gunName.Contains("Heal"))
        {
            health += gun.damageDone;
            gun.damageDone = 0;
            if (health > 100)
            {
                health = 100;
            }
        }
        for (int i = 0; i < endEnemies.Count; i++)
        {
            if (endEnemies[i] != null) { dead = false; }
        }
        if (levelEnd != null && dead)
        {
            levelEnd.SetActive(true);
            levelEnd.transform.Rotate(0, 0, -1);
        }
        if (gun.gunName.Equals("SniperRifle"))
        {
            foreach (Camera cam in cameras)
            {
                cam.fieldOfView = 60;
            }
        }
        else
        {
            foreach (Camera cam in cameras)
            {
                cam.fieldOfView = 40;
            }
        }

    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            if (!ended)
            {
                GameObject.Find("EndMenu").GetComponent<LevelEnd>().EndLevel("Dead");
                ended = true;
            }
        }
    }

    public void DrawHealth()
    {
        Vector3[] barPositions =
            { new Vector3(transform.position.x - healthBar.barLength/2, transform.position.y + 1.5f,-0.01f),
            new Vector3(transform.position.x + healthBar.barLength/2, transform.position.y + 1.5f,-0.01f) };
        healthBar.bar.SetPositions(barPositions);
        Vector3[] healthPositions =
            { new Vector3(transform.position.x - healthBar.barLength/2, transform.position.y + 1.5f,-0.02f),
            new Vector3((transform.position.x - healthBar.barLength/2) + (health/maxhealth) * healthBar.barLength, transform.position.y + 1.5f,-0.02f) };
        healthBar.health.SetPositions(healthPositions);
    }
    public void Switch(float switchGun)
    {
        playerGunsObjects[nameToGun[gun.gunName]].SetActive(false);
        if (switchGun != 0 && gun is Beam && gun.lastTracer != null)
        {
            Destroy(gun.lastTracer);
        }
        if (switchGun != 0)
        {
            gun.accuracy = gunAccuracy;
        }
        if (switchGun < 0)
        {
            currentGun--;
            if (currentGun < 0)
            {
                currentGun = playerGuns.Count - 1;
            }
        }
        if (switchGun > 0)
        {
            currentGun++;
            if (currentGun >= playerGuns.Count)
            {
                currentGun = 0;
            }
        }
        gun = playerGuns[currentGun];
        int gunIndex = nameToGun[gun.gunName];
        playerGunsObjects[gunIndex].SetActive(true);
        gunAccuracy = gun.accuracy;
        crouchAccuracy = gunAccuracy / 2;
    }
}
