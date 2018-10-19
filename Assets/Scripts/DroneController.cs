using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//will be parent class to other enemy classes...
//have to refactor code

public class DroneController : MonoBehaviour {
    Rigidbody2D myRB;
    public Rigidbody2D player;
    public Transform playerTransform;
    public GameObject bullet;
    public int bulletSpeed;

    SpriteRenderer myRenderer;
    Animator animator;
    public int maxSpeed;
    int movement = 0;
    Vector2 newPosition;
    Vector2 randomPosition;

    public int health;
    public static int damage = 10;

	// Use this for initialization
	void Start () {
        myRB = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if((Time.fixedTime)%1==0)
        {
            randomPosition = RandomVector();
            movement = Random.Range(0, 7);
        }
        if (Vector2.Distance(myRB.position,player.position)>7&&Vector2.Distance(myRB.position, player.position) < 38&&(int)transform.position.z == (int)playerTransform.position.z)
        {
            newPosition = Vector2.MoveTowards(myRB.position, randomPosition, 10 * Time.fixedDeltaTime);
            myRB.MovePosition(newPosition);
        }
        else
        {
            myRB.velocity = new Vector2(0, 0);
        }



        //newPosition = Vector2.MoveTowards(myRB.position, player.position, 10 * Time.deltaTime);
        //myRB.MovePosition(newPosition);

        //flip sprite towards player
        if(player.position.x>myRB.position.x)
        {
            myRenderer.flipX = true;
        }
        if (player.position.x < myRB.position.x)
        {
            myRenderer.flipX = false;
        }

        //fire weapon
        if ((Time.fixedTime) % 1 == 0&&Vector2.Distance(myRB.position, player.position) < 38&&myRB.position.y>player.position.y&&(int)transform.position.z==(int)playerTransform.position.z)
        {
            Rigidbody2D bulletClone = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            bulletClone.velocity = (player.position - bulletClone.position).normalized * bulletSpeed;
           
        }

        if(health<=0)
        {
            myRB.gravityScale = 4;
            myRB.freezeRotation = false; 
            myRB.angularVelocity = 120;
            enabled = false;
            animator.enabled = false;
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
            myRenderer.color = new Color(100, 100, 100);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;

    }

    private Vector2 RandomVector()
    {
        var x = Random.Range(player.position.x-10, player.position.x + 10);
        var y = Random.Range(player.position.y, player.position.y + 10);
        return new Vector2(x, y);
    }
}
