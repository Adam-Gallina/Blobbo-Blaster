using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : EnemyMovement {

    public float meleeRange;
    private float nextDash = 0f;
    private bool dashing = false;

    new private void Awake()
    {
        health = maxHealth;
        jumpStrength = 2.5f;

        StartCommands();

        anim = GetComponent<Animator>();
    }

    void Update ()
    {
		if (!dead)
        {
            if (!dashing) { Movement(); }
            UpdateAnim();

            Vector2 rayCast_dir = new Vector2(-(transform.position.x - player.transform.position.x), -(transform.position.y - player.transform.position.y));
            RaycastHit2D playerCheck = Physics2D.Raycast(transform.position, rayCast_dir, stats.range, raycastTargets);
            if (playerCheck != false)
            {
                if ((Mathf.Sign(rayCast_dir.x) == dir) && playerCheck.transform.gameObject.tag == "Player")
                {
                    following = true;
                    if (Vector3.Distance(player.transform.position, transform.position) <= meleeRange)
                    {
                        if (dashing)
                        {
                            GameObject.Find("Player").GetComponent<Player>().TakeDamage(stats.damage * 3);
                            Vector2 rb = GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity;
                            GameObject.Find("Player").GetComponent<Rigidbody2D>().AddForce(new Vector2(dir * 10, rb.y));
                        }
                        anim.SetBool("Melee", true);
                        dashing = false;
                    }
                    else if (Time.time > nextDash && Mathf.Abs(player.transform.position.x - transform.position.x) < stats.range)//&& rayCast_dir.y < 1 && rayCast_dir.y > -1)
                    {
                        //Debug.Log("Dashing");
                        anim.SetBool("Melee", false);
                        nextDash = Time.time + stats.fireRate;
                        dashing = true;
                        dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                        rb.velocity = new Vector2(dir * stats.speed * 2, rb.velocity.y);
                        Dash();
                    }
                    
                }
                else
                {
                    anim.SetBool("Melee", false);
                    following = false;
                }
            }
        }
	}

    private void Dash()
    {
        dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb.velocity = new Vector2(dir * stats.speed * 3.5f, rb.velocity.y);
    }
    private void Attack()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < meleeRange)
        {
            GameObject.Find("Player").GetComponent<Player>().TakeDamage(stats.damage);
        }
        else
        {
            anim.SetBool("Melee", false);
        }
    }
}
