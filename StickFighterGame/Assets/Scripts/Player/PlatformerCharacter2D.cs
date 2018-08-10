using System;
using UnityEngine;


public class PlatformerCharacter2D : MonoBehaviour
{
    private int godMode = -1;
    public static int funDrops = -1;
    [SerializeField]
    private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField]
    private float m_JumpForce = 400f;                // Amount of force added when the player jumps.
    [SerializeField]
    private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
    [SerializeField]
    private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField]
    private PhysicsMaterial2D Slippery;

    public GameObject Pistol;
    public GameObject AR;
    public GameObject Shotgun;
    public GameObject Sniper;

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    private Transform RightCheck;
    private Transform LeftCheck;
    private float CrouchHeight;         //The crouch height of the player
    private BoxCollider2D boxCollider;
    private bool m_Grounded;            // Whether or not the player is grounded.
    private int airJumps = 1;
    private int airJumpsLeft;              //How many jumps the player has left

    private Rigidbody2D m_Rigidbody2D;
    private Player player;

    private Animator anim;

    private float gunAngle;
    private float distToGun;
    private bool gunAboveCenter;
    //[HideInInspector]
    private GameObject playerGun;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        RightCheck = transform.Find("RightCheck");
        LeftCheck = transform.Find("LeftCheck");
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        CrouchHeight = boxCollider.size.y / 2;
        airJumpsLeft = airJumps;
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("g"))
        {
            float health = this.gameObject.GetComponent<Player>().health;
            godMode *= -1;
            if (godMode > 0) { m_MaxSpeed *= 2; m_JumpForce *= 1.5f; health *= 10; }
            else { m_MaxSpeed /= 2; m_JumpForce /= 1.5f; health = 100; }
            this.gameObject.GetComponent<Player>().health = health;

        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown("f")) { funDrops *= -1; Debug.Log(funDrops); }
        m_Grounded = false;
        Collider2D hit = Physics2D.OverlapArea(new Vector2(m_GroundCheck.position.x - 0.29f, m_GroundCheck.position.y), new Vector2(m_GroundCheck.position.x + 0.29f, m_GroundCheck.position.y - 0.1f), 1 << LayerMask.NameToLayer("Platform"));
        if (hit != null)
        {
            m_Grounded = true;
            airJumpsLeft = airJumps;
            //Debug.Log("Grounded");
            //sprite.color = new Color(1, 1, 1);
            return;
        }
        //Debug.Log("Not Grounded");
        //sprite.color = new Color(1, 1, 1);
    }
    private void Update()
    {
        Vector3 mousePosScreen = Input.mousePosition;
        mousePosScreen.z = 15;
        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(mousePosScreen);
        Vector2 mousePos = new Vector2(mousePos3.x, mousePos3.y);
        if (mousePos.x < transform.position.x)
        {
            if (transform.localScale.x > 0)
            {
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;
            }
        }
        if (mousePos.x > transform.position.x)
        {
            if (transform.localScale.x < 0)
            {
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;
            }
        }
        playerGun = transform.Find("Guns").Find(player.gun.gunName).gameObject;
        findGunAngle();
        Transform gunTransform = playerGun.transform;
        Transform shotOriginT = gunTransform.Find("ShotOrigin");
        Transform gunshotT = playerGun.transform.Find("Gunshot");
        Vector2 newRotation = mousePos - (Vector2)gunTransform.position;
        if (transform.localScale.x < 0)
        {

            newRotation.x *= -1;
            newRotation.y *= -1;

        }

        gunTransform.right = newRotation;
        float distToTarget = Vector2.Distance(gunTransform.position, mousePos);
        float alpha = (float)Math.Asin(distToGun * Math.Sin(gunAngle) / distToTarget);
        //Debug.Log("alpha:" + alpha);
        float extraAdjust = alpha + gunAngle - ((float)Math.PI / 2);
        // Debug.Log("extra adjust:" + extraAdjust);
        extraAdjust = extraAdjust * (180 / (float)Math.PI);
        Vector3 newRotate = gunTransform.localEulerAngles;
        // Debug.Log("extra adjust:" + extraAdjust);
        if (float.IsNaN(extraAdjust))
        {
            Debug.Log("Mouse is too close to gun center");
            return;
        }
        if (gunAboveCenter == true)
        {
            newRotate.z -= extraAdjust;
        }
        else
        {
            newRotate.z += extraAdjust;
        }
        gunTransform.localEulerAngles = newRotate;


    }

    public void Move(float move, bool jump, bool crouch)
    {
        playerGun = transform.Find("Guns").Find(player.gun.gunName).gameObject;
        if (crouch == true && m_Grounded == true)
        {
            //Slow move speed
            move *= 0.5f;

            player.gun.accuracy = player.crouchAccuracy;
            //Lower box collider
            boxCollider.offset = new Vector2(0, -0.25f);
            boxCollider.size = new Vector2(boxCollider.size.x, CrouchHeight);
            if (playerGun != null) { playerGun.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0); }
        }
        else
        {
            player.gun.accuracy = player.gunAccuracy;
            boxCollider.offset = new Vector2(0, 0);
            boxCollider.size = new Vector2(boxCollider.size.x, CrouchHeight * 2);
            if (playerGun != null) { playerGun.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 0); }
        }
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            Collider2D hitRight = Physics2D.OverlapArea(new Vector2(RightCheck.position.x - 0.2f, RightCheck.position.y + 1f), new Vector2(RightCheck.position.x + 0.2f, RightCheck.position.y - 0.99f), 1 << LayerMask.NameToLayer("Platform"));
            Collider2D hitLeft = Physics2D.OverlapArea(new Vector2(LeftCheck.position.x - 0.2f, LeftCheck.position.y + 1f), new Vector2(LeftCheck.position.x + 0.2f, LeftCheck.position.y - 0.99f), 1 << LayerMask.NameToLayer("Platform"));

            if (hitRight != null || hitLeft != null)
            {
                m_Rigidbody2D.sharedMaterial = Slippery;
            }
            else
            {
                m_Rigidbody2D.sharedMaterial = null;
            }

            //Debug.Log(m_FacingRight);

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

        }
        // If the player should jump...
        if ((airJumpsLeft > 0 || m_Grounded == true) && jump == true)
        {
            // Add a vertical force to the player.
            if (m_Grounded == true)
            {
                m_Grounded = false;
            }
            else
            {
                airJumpsLeft--;
            }
            Vector2 newVelocity = m_Rigidbody2D.velocity;
            newVelocity.y = m_JumpForce;
            m_Rigidbody2D.velocity = newVelocity;

            //Set Jump anim trigger 
            anim.SetTrigger("Jump");
        }

        //Update Anim 
        if (move != 0) { anim.SetBool("Moving", true); } else { anim.SetBool("Moving", false); }
        if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(m_Rigidbody2D.velocity.x)) { anim.SetBool("Backwards", true); } else { anim.SetBool("Backwards", false); }
        anim.SetBool("Grounded", m_Grounded);
        anim.SetBool("Crouch", crouch);
    }
    public void Fire(bool clickFire, bool autoFire)
    {
        //Debug.Log("clicks detected");
        //Debug.Log(player.gun.GetType().Name);
        player.gun.ShootGun(playerGun.transform.Find("ShotOrigin").position, playerGun.transform.Find("Gunshot").position, clickFire, autoFire);
    }
    public void Reload(bool reload)
    {
        if (reload == true)
        {
            player.gun.reload();
        }
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawCube(m_GroundCheck.position, new Vector3(0.6f, 0.2f, 0.1f));
    }*/

    public void findGunAngle()
    {
        Vector3 currentRotation = playerGun.transform.localEulerAngles;
        Vector3 defaultRotation = new Vector3(currentRotation.x, currentRotation.y, 0);
        playerGun.transform.localEulerAngles = defaultRotation;

        //use two points on gun to get equation y = mx + b
        // Debug.Log(playerGun.name);
        Vector2 point1 = playerGun.transform.Find("ShotOrigin").position;
        Vector2 point2 = playerGun.transform.Find("Gunshot").position;
        float m = (point1.y - point2.y) / (point1.x - point2.x);
        float b = point1.y - (m * point1.x);

        Vector2 intersection = new Vector2(playerGun.transform.position.x, (m * playerGun.transform.position.x) + b);

        gunAngle = Vector2.Angle((Vector2)playerGun.transform.position - intersection, point1 - intersection);
        // Debug.Log("gun angle:" + gunAngle);
        gunAngle *= ((float)Math.PI / 180);
        //Debug.Log("gun angle:" + gunAngle);
        distToGun = Vector2.Distance(playerGun.transform.position, intersection);
        if (point1.y > playerGun.transform.position.y)
        {
            gunAboveCenter = true;
        }
        else
        {
            gunAboveCenter = false;
        }
        playerGun.transform.localEulerAngles = currentRotation;
    }
    public static void DrawDebugBox(Vector2 start, Vector2 end, Color col)//Draw a Debug Box
    {
        Debug.DrawLine(new Vector2(start.x, start.y), new Vector2(start.x, end.y), col);
        Debug.DrawLine(new Vector2(start.x, end.y), new Vector2(end.x, end.y), col);
        Debug.DrawLine(new Vector2(end.x, end.y), new Vector2(end.x, start.y), col);
        Debug.DrawLine(new Vector2(end.x, start.y), new Vector2(start.x, start.y), col);
    }
}

