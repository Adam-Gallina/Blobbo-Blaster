using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour {

    private Vector3 trans;

    private void Update()
    {
        Vector3 player_trans = GameObject.Find("Player").transform.position;
        //trans = new Vector3(transform.position.x - player_trans.x, transform.position.y);
        trans = player_trans - transform.position;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if(collision.tag == "Platform")
        {
            transform.Translate(new Vector3(Mathf.Sign(trans.x)/10, Mathf.Sign(trans.y)/10, 0));
        }*/
    }
}
