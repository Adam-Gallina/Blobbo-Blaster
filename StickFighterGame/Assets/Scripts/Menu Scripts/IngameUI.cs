using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IngameUI : MonoBehaviour
{

    public Text health;
    public Text coins;
    public Text guns;
    public Text ammo;
    private Player player;
    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GameObject.Find("CameraUI").GetComponent<Camera>();
    }
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Print("");
    }

    private void Update()
    {
        coins.text = "Coins: " + Player.coins;
        health.text = "Health: " + Mathf.RoundToInt(player.health) + "/" + player.maxhealth;
        guns.text = "Gun: " + player.gun.gunName;
        if (player.gun.reloading == true && Time.time - player.gun.lastShot <= player.gun.reloadTime)
        {
            ammo.text = "Reloading";
        }
        else
        {
            ammo.text = player.gun.clip + "/" + player.gun.clipSize;
        }
    }

    public static void Print(string message)
    {
        Text textBox = GameObject.Find("Message Box").GetComponent<Text>();

        if (message == "")
        {
            textBox.enabled = false;
        }
        else
        {
            textBox.enabled = true;
            textBox.text = message;
        }

    }
}
