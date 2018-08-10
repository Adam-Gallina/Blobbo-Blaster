using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
    private Player player;
    private bool m_Jump;
    private bool AxisInUse = false;
    private PauseMenu pauseMenu;
    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        player = GetComponent<Player>();
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
    }


    private void Update()
    {
        if (pauseMenu.paused == true)
        {
            return;
        }
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        // Read the mouse input in Update so button presses aren't missed.
        bool m_ClickFire = CrossPlatformInputManager.GetButtonDown("Fire1");
        bool m_AutoFire = CrossPlatformInputManager.GetButton("Fire1");
        m_Character.Fire(m_ClickFire, m_AutoFire);
        bool m_Reload = CrossPlatformInputManager.GetButton("Reload");
        m_Character.Reload(m_Reload);
        float switchGun = CrossPlatformInputManager.GetAxis("Switch");
        if (switchGun != 0 && AxisInUse == false)
        {
            player.Switch(switchGun);
            AxisInUse = true;
        }
        if (switchGun == 0)
        {
            AxisInUse = false;
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // Pass all parameters to the character control script.
        bool isCrouch = CrossPlatformInputManager.GetButton("Crouch");
        m_Character.Move(h, m_Jump, isCrouch);
        m_Jump = false;

    }
}

