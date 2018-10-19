using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpaceship : MonoBehaviour {
    int damage;
    float cooldown;
    public int health;
    int maxHealth;
    SpriteRenderer myRenderer;

    public Image healthBar;

    Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        health = 100;
	}
	
	// Update is called once per frame
	void Update () {

        if (health <= 0)
        {

        }
        else
        {
            //update healthbar
            healthBar.rectTransform.localScale = new Vector3(((float)health / 100), 1, 1);
        }
		
	}

    public void takeDamage()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 5; i++)
        {
            myRenderer.color = new Color(255, 0, 0);
            yield return new WaitForSeconds(0.1f);
            myRenderer.color = new Color(197, 0, 255);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;

    }
}
