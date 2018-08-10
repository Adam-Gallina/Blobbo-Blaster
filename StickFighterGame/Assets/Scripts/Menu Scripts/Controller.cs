using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public static List<int> CanvasList = new List<int>();
    public static List<bool> levelswonlist = new List<bool>();

    public static void levelcompleted(int level)
    {
        levelswonlist[level - 1] = true;
        CanvasList[level]++;
        GameObject.Find((GameObject.Find("Levels Tab").transform.GetChild(level - 1)).transform.name).SetActive(true);
    }

    void Update()
    {

    }
}
