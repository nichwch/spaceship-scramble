using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<DroneController>().health = collision.gameObject.GetComponent<DroneController>().health - PlayerController.attackDamage;
            Destroy(gameObject);
            collision.gameObject.GetComponent<DroneController>().takeDamage();
        }
    }
}
