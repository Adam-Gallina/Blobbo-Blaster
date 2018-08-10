using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Spawns
{
    public float healthLeft;
    public int enemies;
    [HideInInspector]
    public bool spawned = false;
    public GameObject specSpawn;
}
[Serializable]
public class SpawnerStats
{
    public EnemyStats newStats;

    public Vector2 target;
    public float spawnRate;
    public float spawnRange;
}
public class NextSpawn
{
    public bool isSpawning = false;
    public GameObject spawn = null;
    public Vector3 location = new Vector3 (0,0,0);
}

public class EnemySpawner : EnemyController {
    
    public List<Spawns> spawns = new List<Spawns>();
    public SpawnerStats stats = new SpawnerStats();
    [HideInInspector]
    public NextSpawn nextSpawn = new NextSpawn();

    protected LayerMask raycastTargets;
    private float spawnNext;
    private float elapsedTime;
    public GameObject enemy;
    //public List<GameObject> enemies = new List<GameObject>();
    private GameObject player;
    private LineRenderer bar;



    private void Start()
    {
        health = maxHealth;
        player = GameObject.Find("Player");
        raycastTargets = LayerMask.GetMask("Platform", "Player");

        bar = GetComponent<LineRenderer>();
        bar.startWidth = 0.375f;
        bar.material.color = Color.cyan;
        bar.positionCount = 2;
        bar.enabled = false;

        anim = GetComponent<Animator>();
        anim.Play(null, 0, UnityEngine.Random.Range(0.0f,1.0f));
    }

    void Update ()
    {
        if (!dead)
        {
            float deltaTime = Time.time - elapsedTime;
            Vector2 rayCast_dir = new Vector2(-(transform.position.x - player.transform.position.x), -(transform.position.y - player.transform.position.y));
            RaycastHit2D playerCheck = Physics2D.Raycast(transform.position, rayCast_dir, stats.spawnRange, raycastTargets);
            if (playerCheck != false)
            {
                if (deltaTime > stats.spawnRate && playerCheck.transform.gameObject.layer == 9 && !nextSpawn.isSpawning)
                {
                    nextSpawn.location = new Vector2(transform.position.x, transform.position.y + 2f);
                    nextSpawn.spawn = enemy;
                    anim.SetBool("Spawn", true);
                    nextSpawn.isSpawning = true;
                }
            }

            for (int i = 0; i < spawns.Count; i++)
            {
                if (maxHealth * spawns[i].healthLeft > health && !spawns[i].spawned)
                {
                    int spread = spawns[i].enemies * 2;
                    for (int ii = 0; ii < spawns[i].enemies; ii++)
                    {
                        Vector2 place = new Vector2((transform.position.x + spread / 2) - spread / (ii + 1), transform.position.y + 2f);
                        if (spawns[i].specSpawn == null)
                        {
                            //nextSpawn.location = place;
                            //nextSpawn.spawn = enemy;
                            //anim.SetBool("Spawn (test)", true);
                            LastSpawn(Instantiate(enemy, place, transform.rotation), true);
                        }
                        else
                        {
                            //nextSpawn.location = place;
                            //nextSpawn.spawn = spawns[i].specSpawn;
                            //anim.SetBool("Spawn (test)", true);
                            LastSpawn(Instantiate(spawns[i].specSpawn, place, transform.rotation), true);
                        }
                    }
                    spawns[i].spawned = true;
                }

            }

            //Update Line
            if (playerCheck != false)
            {
                if (!nextSpawn.isSpawning && playerCheck.transform.gameObject.layer == 9)
                {
                    bar.enabled = true;
                    Vector3[] barPositions =
                        { new Vector3(transform.position.x - 0.7f, transform.position.y - 1.6875f,-0.001f),
            new Vector3((transform.position.x - 0.7f) + (deltaTime/stats.spawnRate)*1.4f, transform.position.y -1.6875f,-0.001f) };
                    bar.SetPositions(barPositions);
                }
                else
                {
                    bar.enabled = false;
                }
            }
        }
    }

    public void DrawBar()
    {
        nextSpawn.isSpawning = false;
        elapsedTime = Time.time;
    }
    public void SpawnEnemy()
    {
        anim.SetBool("Spawn", false);
        LastSpawn(Instantiate(nextSpawn.spawn, nextSpawn.location, transform.rotation), false);
    }
    protected void LastSpawn(GameObject lastSpawn, bool lootable)
    {
        //anim.SetTrigger("Spawn");
        EnemyMovement spawn = lastSpawn.GetComponent<EnemyMovement>();
        //Vector2 maxLeft = new Vector2(0, 0);
        //Vector2 maxRight = new Vector2(0, 0);
        //spawn.GetComponent<EnemyMovement>().pathway.Add(maxLeft);
        //spawn.GetComponent<EnemyMovement>().pathway.Add(maxRight);

        lastSpawn.GetComponent<EnemyController>().lootable = lootable;
        spawn.rb.velocity = new Vector3(0, 12, 0);
        spawn.stats = stats.newStats;
        spawn.player = GameObject.Find("Player");
        spawn.pathway.Add(stats.target);
        spawn.pathway.Add(transform.position);
        spawn.total = 2;
    }
}
