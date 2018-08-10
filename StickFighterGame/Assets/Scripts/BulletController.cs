using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int target;
    public float damage;
    protected bool destroyable = true;


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
        }
    }

}
