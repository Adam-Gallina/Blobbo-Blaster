using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MortarStats
{
    public float range;
    public float damage;
    public float fireRate;
}

public class MortarController : EnemyController {

    public MortarStats stats;

    private GameObject player;
    private GameObject barrel;

    public GameObject bullet;
    public GameObject marker;
    private float fireNext;

	void Start ()
    {
        player = GameObject.Find("Player");
        barrel = transform.GetChild(0).gameObject;
	}

    void Update()
    {
        Vector2 target = player.transform.position;
        if (Time.time > fireNext)
        {
            if (Dist(target, transform.position) <= stats.range)
            {
                fireNext = Time.time + stats.fireRate;
                Fire(target);
            }
        }
    }

    private void Fire(Vector2 target)
    {
        GameObject lastShot = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        lastShot.GetComponent<BulletController>().target = 9;
        lastShot.GetComponent<BulletController>().damage = stats.damage;
        if (SetTrajectory(lastShot, target, 30))
        {
            lastShot.GetComponent<FireballController>().marker = Instantiate(marker, new Vector2(target.x + Random.Range(-2.0f, 2.0f), target.y + Random.Range(5.0f, 7.0f)), transform.rotation);
        }
        else
        {
            Destroy(lastShot.gameObject);
        }
    }

    private float Dist(Vector2 target, Vector2 trans)
    {
        float diffX = target.x - trans.x;
        float diffY = target.y - trans.y;
        float dist = Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffY, 2)) * Mathf.Sign(diffX);
        return dist;
    }

    //Applies the force to the Rigidbody2D so it will land at the target position. The arch [0, 1] determines the percent of arch to provide between the minimum and maximum arch.
    public bool SetTrajectory(GameObject lastShot, Vector2 target, float force, float arch = 0.5f)
    {
        Rigidbody2D rb = lastShot.GetComponent<Rigidbody2D>();
        Mathf.Clamp(arch, 0, 1);
        var origin = rb.position;
        float x = target.x - origin.x;
        float y = target.y - origin.y;
        float gravity = -Physics2D.gravity.y;
        float b = force * force - y * gravity;
        float discriminant = b * b - gravity * gravity * (x * x + y * y);
        if (discriminant < 0)
        {
            return false;
        }
        float discriminantSquareRoot = Mathf.Sqrt(discriminant);
        float minTime = Mathf.Sqrt((b - discriminantSquareRoot) * 2) / Mathf.Abs(gravity);
        float maxTime = Mathf.Sqrt((b + discriminantSquareRoot) * 2) / Mathf.Abs(gravity);
        float time = (maxTime - minTime) * arch + minTime;
        float vx = x / time;
        float vy = y / time + time * gravity / 2;
        var trajectory = new Vector2(vx, vy);
        rb.AddForce(trajectory, ForceMode2D.Impulse);
        return true;
    }

}
