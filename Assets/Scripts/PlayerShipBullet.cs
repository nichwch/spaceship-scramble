using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//actually plasma bullet not player bullet LOL
public class PlayerShipBullet : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyShip")
        {
            collision.gameObject.GetComponent<EnemySpaceship>().health = collision.gameObject.GetComponent<EnemySpaceship>().health - new PlasmaCannon().damage;
            Destroy(gameObject);
            collision.gameObject.GetComponent<EnemySpaceship>().takeDamage();
        }
    }
}
