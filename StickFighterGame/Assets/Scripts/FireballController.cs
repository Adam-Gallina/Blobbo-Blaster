using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : BulletController {

    Rigidbody2D rb;
    float unSafe;
    [HideInInspector]
    public GameObject marker;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        unSafe = Time.time + 1f;
        destroyable = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == target)
        {
            if (target == 9)
            {
                GameObject.Find("Player").GetComponent<Player>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.layer == 8 && destroyable)
        {
            Destroy(this.gameObject);
            Destroy(marker);
        }
    }

    void Update ()
    {
        if (Time.time > unSafe) { destroyable = true; }

        float rotation = 0;
        if (rb.velocity.y == 0)
        {
            rotation = -90 * Mathf.Sign(rb.velocity.x);
        }
        else if (rb.velocity.y > 0)
        {
            rotation = 0 - (Mathf.Atan(rb.velocity.x / rb.velocity.y) * 180 / Mathf.PI);
        }
        else if (rb.velocity.y < 0)
        {
            rotation = 180 - (Mathf.Atan(rb.velocity.x / rb.velocity.y) * 180 / Mathf.PI);
        }
        rotation += 180;

        Quaternion trans = transform.rotation;
        transform.eulerAngles = new Vector3(trans.x, trans.y, rotation);

        if(Vector3.Distance(transform.position, marker.transform.position) < 15)
        {
            marker.GetComponent<Animator>().SetBool("InRange", true);
        }
	}
}
