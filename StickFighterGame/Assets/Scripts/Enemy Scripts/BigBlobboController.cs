using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlobboController : EnemyMovement {

    private bool landAnim = false;
    private float fireNext;
    public GameObject bullet;

    new private void Awake()
    {
        health = maxHealth;
        jumpStrength = 3.5f;

        StartCommands();

        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            Movement();
            UpdateAnim();

            if (canJump && landAnim) { anim.SetTrigger("Landed"); landAnim = false; } else if (!canJump) { landAnim = true; }

            //Follow/Shoot the player
            Vector2 rayCast_dir = new Vector2(-(transform.position.x - player.transform.position.x), -(transform.position.y - player.transform.position.y));
            RaycastHit2D playerCheck = Physics2D.Raycast(transform.position, rayCast_dir, stats.range, raycastTargets);
            if (playerCheck != false)
            {
                if ((Mathf.Sign(rayCast_dir.x) == dir) && playerCheck.transform.gameObject.tag == "Player")
                {
                    //Shoot
                    if (Time.time > fireNext && rayCast_dir.y < 0.5f && rayCast_dir.y > -0.5f)
                    {
                        fireNext = Time.time + stats.fireRate;
                        FireBullet(Instantiate(bullet, new Vector3(transform.position.x + (0.5f * dir), transform.position.y, transform.position.z), transform.rotation), dir);
                    }
                    following = true;
                }
                else
                {
                    following = false;
                }
            }
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private void FireBullet(GameObject bullet, float dir)
    {
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.bulletSpeed * dir, 0);
        bullet.transform.localScale = new Vector3(-dir * 1.5f, transform.localScale.y/1.5f, transform.localScale.z);
        bullet.GetComponent<BulletController>().target = 9;
        bullet.GetComponent<BulletController>().damage = stats.damage;
    }
}
