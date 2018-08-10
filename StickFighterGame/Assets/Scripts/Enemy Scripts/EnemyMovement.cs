using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyStats
{
    public float speed;
    public float jumpHeight;
    public float range;
    public float damage;
    public float fireRate;
    public int bulletSpeed;
}

public class EnemyMovement : EnemyController {

    public EnemyStats stats;
    public Vector2 dims;

    public List<Vector3> pathway;
    protected float jumpStrength = 2.5f;
    protected bool canJump = true;
    protected float errorMargin = 0.1f;

    protected LayerMask raycastTargets;
    protected LayerMask moveTargets;

    
    protected float dir;
    protected Vector3 target;
    protected int location = 0;
    [HideInInspector]
    public int total = 0;
    protected bool following = false;

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public GameObject player;
    private Vector3 trans;

    protected Vector2 box1 = new Vector2(0, 0);
    protected Vector2 box2 = new Vector2(0, 0);

    protected void StartCommands()
    {
        Physics2D.IgnoreLayerCollision(9, 10);

        total = pathway.Count;
        
        rb = GetComponent<Rigidbody2D>();
        raycastTargets = LayerMask.GetMask("Platform", "Player");
        moveTargets = LayerMask.GetMask("Platform", "Enemies");
        player = GameObject.Find("Player");
        trans = GetComponent<Transform>().position;
    }

    protected void Movement()
    {
        trans = GetComponent<Transform>().position;
        

        if (Physics2D.OverlapArea(new Vector2(trans.x - 0.4f, trans.y - 0.55f), new Vector2(trans.x + 0.4f, trans.y - .6f), moveTargets))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        //Determine where to go
        if (Mathf.Abs(transform.position.x - target.x) < errorMargin && !following)
        {
            location += 1;
            if (location > total - 1) { location = 0; }
            target = pathway[location];
        }
        if (following) { target = player.transform.position; } else { target = pathway[location]; }

        //Move the enemy
        dir = Mathf.Sign(target.x - transform.position.x);
        rb.velocity = new Vector2(dir * stats.speed, rb.velocity.y);

        box1 = new Vector2(trans.x + (dir * (0.1f + (dims.x / 2))), trans.y + 0.1f - dims.y / 2);
        box2 = new Vector2(box1.x + (dir * (stats.speed / 3)), trans.y + -0.5f * dims.y + 1);

        Collider2D obstCheck = Physics2D.OverlapArea(box1, box2, moveTargets);
        //DrawDebugBox(box1, box2, Color.red, 0f);
        if (obstCheck != null)
        {
            if ((obstCheck.tag == "Platform" || obstCheck.tag == "Enemy") && canJump)
            {
                //See if the obstruction is actually in your way
                bool correctionNeeded = false;
                RaycastHit2D distanceCheck = Physics2D.Linecast(new Vector2(trans.x, box1.y), new Vector2(target.x, box1.y), moveTargets);
                if (distanceCheck && Mathf.Abs(target.x - trans.x) > dims.x) { correctionNeeded = true; }
                if (obstCheck.tag == "Enemy" && obstCheck.gameObject.GetComponent<Rigidbody2D>().velocity.x != rb.velocity.x && dir == -1) { correctionNeeded = false; }
                //Debug.DrawLine(new Vector2(box1.x, box1.y +0.1f), new Vector2(target.x, box1.y + 0.1f), Color.yellow);

                Collider2D heightCheck = null;
                for (int i = 0; i <= stats.jumpHeight; i++)
                {
                    //Check the height of the jump
                    heightCheck = Physics2D.OverlapArea(new Vector2(box1.x, box1.y + i), new Vector2(box2.x, box2.y + i), moveTargets);
                    //DrawDebugBox(new Vector2(box1.x, box1.y + i), new Vector2(box2.x, box2.y + i), Color.green, 0.1f);
                    if (heightCheck == null && correctionNeeded)
                    {
                        canJump = false;
                        rb.velocity = new Vector2(rb.velocity.x, (jumpStrength * i) + 6.5f);
                        break;
                    }
                }
                if (heightCheck != null && correctionNeeded)
                {
                    location++;
                    if (location > total - 1) { location = 0; }
                }
            }
        }
    }
    protected void UpdateAnim()//Update animation direction
    {
        
        if (dir < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if (dir > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
    public static void DrawDebugBox(Vector2 start, Vector2 end, Color col, float time)//Draw a Debug Box
    {
        Debug.DrawLine(new Vector2(start.x, start.y), new Vector2(start.x, end.y), col, time);
        Debug.DrawLine(new Vector2(start.x, end.y), new Vector2(end.x, end.y), col, time);
        Debug.DrawLine(new Vector2(end.x, end.y), new Vector2(end.x, start.y), col, time);
        Debug.DrawLine(new Vector2(end.x, start.y), new Vector2(start.x, start.y), col, time);
    }
}
