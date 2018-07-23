using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField]
        private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField]
        private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)]
        [SerializeField]
        private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField]
        private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField]
        private PhysicsMaterial2D Slippery;

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        private Transform RightCheck;
        private Transform LeftCheck;
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            RightCheck = transform.Find("RightCheck");
            LeftCheck = transform.Find("LeftCheck");
        }


        private void FixedUpdate()
        {
            m_Grounded = false;
            RaycastHit2D hit = Physics2D.BoxCast(m_GroundCheck.position, new Vector2(1.59f, 0.1f), 0, new Vector2(0, -1), 0, 1 << LayerMask.NameToLayer("Platform"), -Mathf.Infinity, Mathf.Infinity);
            if (hit.collider != null)
            {
                m_Grounded = true;
                Debug.Log("Grounded");
                return;
            }
           Debug.Log("Not Grounded");
        }


        public void Move(float move, bool jump)
        {

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                RaycastHit2D hitRight = Physics2D.BoxCast(RightCheck.position, new Vector2(0.1f, 1.59f), 0, new Vector2(1, 0), 0, 1 << LayerMask.NameToLayer("Platform"), -Mathf.Infinity, Mathf.Infinity);
                RaycastHit2D hitLeft = Physics2D.BoxCast(LeftCheck.position, new Vector2(0.1f, 1.59f), 0, new Vector2(-1, 0), 0, 1 << LayerMask.NameToLayer("Platform"), -Mathf.Infinity, Mathf.Infinity);

                if (hitRight.collider != null || hitLeft.collider != null)
                {
                    m_Rigidbody2D.sharedMaterial = Slippery;
                }
                else
                {
                    m_Rigidbody2D.sharedMaterial = null;
                }
                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
    }
}
