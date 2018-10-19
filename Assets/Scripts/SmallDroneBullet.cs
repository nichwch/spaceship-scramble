using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDroneBullet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().health = collision.gameObject.GetComponent<PlayerController>().health - DroneController.damage;
            Destroy(gameObject);
            collision.gameObject.GetComponent<PlayerController>().takeDamage();
        }
    }
}
