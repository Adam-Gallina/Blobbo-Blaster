using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Doors
{
    public GameObject door;
    public bool toggle;
}


public class EnemyController : MonoBehaviour {
    
    public float maxHealth;
    public bool lootable;
    public int coinDrop;
    public int healthChance;
    protected float health;
    protected bool dead = false;
    public List<Doors> doors = new List<Doors>();
    public GameObject healthPack;
    public GameObject coin;
    protected Animator anim;
    BoxCollider2D coll;

    protected void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetSprite", 0.1f);

        if (health <= 0)
        {
            //GetComponent<BoxCollider2D>().enabled = false;
            anim.SetTrigger("Death");

            if ((!dead || PlatformerCharacter2D.funDrops > 0) && lootable)
            {
                for (int i = 0; i < Random.Range(coinDrop - Mathf.RoundToInt(coinDrop / 4), coinDrop); i++)
                {
                    GameObject last = Instantiate(coin, transform.position, transform.rotation);
                    last.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-8, 8), Random.Range(10, 16));
                    last.GetComponent<Animator>().Play(null, 0, UnityEngine.Random.Range(0.0f, 1.0f));
                }
                if (Random.Range(0,100) > healthChance)
                {
                    GameObject last = Instantiate(healthPack, transform.position, transform.rotation);
                    last.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-8, 8), Random.Range(10, 16));
                }
            }

            if (doors.Count > 0)
            {
                for (int i = 0; i < doors.Count; i++)
                {
                    doors[i].door.SetActive(doors[i].toggle);
                }
            }

            dead = true;
        }
    }

    protected void ResetSprite() { GetComponent<SpriteRenderer>().color = Color.white; }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
