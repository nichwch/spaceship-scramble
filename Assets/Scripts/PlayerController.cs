using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    Rigidbody2D myRB;
    Animator myAnim;
    SpriteRenderer myRend;

    public float maxSpeed;
    public float jumpPower;
    public float weaponCooldown;
    float timeSinceLastAttack = 0;

    public float groundCheckRadius;
    public LayerMask groundLayer;
    public GameObject groundCheck;
    public GameObject playerBullet;
    public Image healthBar;


    public bool stunned;//if player is stunned
    public bool attackStunned;//if player is attack stunned (prevented from attacking)
    bool grounded;//if player is touching ground
    bool doubleJump = false; //if player can double jump;
    bool jumpReleased = false; //if space bar is released;
    bool damaged = false; //if player was recently damaged (which will make him temporarily immune to damage)

    public int health;
    public static int attackDamage = 20; //amount of damage player deals with attacks
	// Use this for initialization
	void Start () {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myRend = GetComponent<SpriteRenderer>();
        health = 100;
        stunned = false;
        attackStunned = false;
	}
	
	// Update is called once per frame
	void Update () {
        float movement = Input.GetAxis("Horizontal");
        //check if player is dead
        if (health <= 0)
        {
            
        }
        else
        {
            //update healthbar
            healthBar.rectTransform.localScale = new Vector3(((float)health / 100), 1, 1);
        }
        //actions that cannot be performed while stunned
        if(!stunned)
        {
            myRB.velocity = new Vector2(movement * maxSpeed, myRB.velocity.y);
            if (Mathf.Abs(myRB.velocity.x) > 0.8f)
            {
                myAnim.SetInteger("isMoving", 1);
            }
            else
            {
                myAnim.SetInteger("isMoving", 0);
            }
            if(movement>0)
            {
                myRend.flipX = false;
            }
            if (movement<0)
            {
                myRend.flipX = true;
            }

            grounded = Physics2D.OverlapCircle(groundCheck.GetComponent<Transform>().position, groundCheckRadius, groundLayer);
            if(grounded)
            {
                doubleJump = true;
            }

            if(grounded&&Input.GetAxis("Jump")>0)
            {
                myRB.velocity = new Vector2(myRB.velocity.x, jumpPower);
                jumpReleased = false;
            }
            if (Input.GetAxis("Jump") < 0.1)
            {
                jumpReleased = true;
            }
            else if(!grounded&&doubleJump&&jumpReleased&&Input.GetAxis("Jump") > 0)
            {
                myRB.velocity = new Vector2(myRB.velocity.x, jumpPower);
                doubleJump = false;
            }
        }
        //actions that cannot be performed while attack stunned;
        if(!attackStunned&&Input.GetAxis("Fire1") > 0&&Time.fixedTime>timeSinceLastAttack+weaponCooldown)
        {
            timeSinceLastAttack = Time.fixedTime;
            if(!myRend.flipX)
            {
                Rigidbody2D bulletClone = Instantiate(playerBullet, transform.position + new Vector3(1f, 0.9f, 0), transform.rotation).GetComponent<Rigidbody2D>();
                bulletClone.velocity = new Vector2(50, 0);
            }
            if (myRend.flipX)
            {
                Rigidbody2D bulletClone = Instantiate(playerBullet, transform.position + new Vector3(-1f, 0.9f, 0), transform.rotation).GetComponent<Rigidbody2D>();
                bulletClone.velocity = new Vector2(-50, 0);
            }
        }


	}

    public void takeDamage()
    {
        StartCoroutine(FlashPlayer());
    }

    IEnumerator FlashPlayer()
    {
        for (int i = 0; i < 5; i++)
        {
            myRend.color = new Color(255, 0, 0);
            yield return new WaitForSeconds(0.1f);
            myRend.color = new Color(100, 100, 100);
            yield return new WaitForSeconds(0.1f);
        }
        damaged = false;
        yield break;

    }
}
