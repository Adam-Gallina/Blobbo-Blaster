using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToMouse : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosScreen = Input.mousePosition;
        mousePosScreen.z = 15;
        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(mousePosScreen);
        Vector2 mousePos = new Vector2(mousePos3.x, mousePos3.y);
        transform.position = mousePos;
        Cursor.visible = false;
    }
}
