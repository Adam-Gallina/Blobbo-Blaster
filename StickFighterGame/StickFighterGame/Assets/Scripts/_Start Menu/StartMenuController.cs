using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour {
    
	public void StartPressed()
    {
        ChangeToScene(1);
    }

    public void ChangeToScene(int scene)
    {
        if (scene == -1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }
}
