using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class both controls the general game logic for the spaceship combat part of the game, as well as representing the player's spaceship
/// </summary>

public class GameController : MonoBehaviour {
    public CameraController cameraController;
    public GameObject Player;
    public float timeBetweenCycles; //the time between energy cycles
    public GameObject[] WeaponDisplays = new GameObject[4];
    public Image[] WeaponCooldownDisplays = new Image[4];
    public float[] weaponCooldowns = new float[4];

    bool gameState = false; //false = platformer, true = turn based
    bool press = false; //if player is pressing the switch key, this is to maintain pressdown functionality
    bool[] weaponPressed = new bool[4];

    //the player's weapons
    ShipWeapon[] weapons = new ShipWeapon[4];

    //parameters of the player's ship
    public static int damage;
    float cooldown;
    public int health;
    int maxHealth;

	// Use this for initialization
	void Start () {
        WeaponDisplays[0].GetComponent<Button>().interactable = false;
        WeaponDisplays[1].GetComponent<Button>().interactable = false;
        WeaponDisplays[2].GetComponent<Button>().interactable = false;
        WeaponDisplays[3].GetComponent<Button>().interactable = false;

        //initialize weapons
        weapons[0] = new PlasmaCannon();
        weapons[1] = new EmptyWeapon();
        weapons[2] = new SlowCannon();
        weapons[3] = new EmptyWeapon();

        WeaponDisplays[0].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[0].name + "]";
        weaponPressed[0] = false;

        WeaponDisplays[1].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[1].name + "]";
        weaponPressed[1] = false;

        WeaponDisplays[2].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[2].name + "]";
        weaponPressed[2] = false;

        WeaponDisplays[3].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[3].name + "]";
        weaponPressed[3] = false;


	}
	
	// Update is called once per frame
	void Update () {
        if(((Time.time - weaponCooldowns[0]) / weapons[0].maxcd)<=1)
        {
            WeaponCooldownDisplays[0].rectTransform.localScale = new Vector3(((Time.time - weaponCooldowns[0]) / weapons[0].maxcd), 1, 1);
        }

        if (((Time.time - weaponCooldowns[1]) / weapons[1].maxcd) <= 1)
        {
            WeaponCooldownDisplays[1].rectTransform.localScale = new Vector3(((Time.time - weaponCooldowns[1]) / weapons[1].maxcd), 1, 1);
        }

        if (((Time.time - weaponCooldowns[2]) / weapons[2].maxcd) <= 1)
        {
            WeaponCooldownDisplays[2].rectTransform.localScale = new Vector3(((Time.time - weaponCooldowns[2]) / weapons[2].maxcd), 1, 1);
        }

        if (((Time.time - weaponCooldowns[3]) / weapons[3].maxcd) <= 1)
        {
            WeaponCooldownDisplays[3].rectTransform.localScale = new Vector3(((Time.time - weaponCooldowns[3]) / weapons[3].maxcd), 1, 1);
        }


        //switch perspective between platformer and RTS
		if(Input.GetAxis("Switch")>0&&!press)
        {
            press = true;
            if(!gameState)
            {
                WeaponDisplays[0].GetComponent<Button>().interactable = true;
                WeaponDisplays[1].GetComponent<Button>().interactable = true;
                WeaponDisplays[2].GetComponent<Button>().interactable = true;
                WeaponDisplays[3].GetComponent<Button>().interactable = true;
                Player.GetComponent<PlayerController>().stunned = true;
                Player.GetComponent<PlayerController>().attackStunned = true;
                Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                cameraController.smoothSpeed += 1;
                cameraController.SetZoom(-55000);
                gameState = true;
            }
            else if(gameState)
            {
                WeaponDisplays[0].GetComponent<Button>().interactable = false;
                WeaponDisplays[1].GetComponent<Button>().interactable = false;
                WeaponDisplays[2].GetComponent<Button>().interactable = false;
                WeaponDisplays[3].GetComponent<Button>().interactable = false;
                Player.GetComponent<PlayerController>().stunned = false;
                Player.GetComponent<PlayerController>().attackStunned = false;
                cameraController.SetZoom(-55);
                gameState = false;
                cameraController.smoothSpeed -= 1;

                WeaponDisplays[0].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[0].name + "]";
                weaponPressed[0] = false;

                WeaponDisplays[1].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[1].name + "]";
                weaponPressed[1] = false;

                WeaponDisplays[2].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[2].name + "]";
                weaponPressed[2] = false;

                WeaponDisplays[3].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[3].name + "]";
                weaponPressed[3] = false;

            }

        }
        if(Input.GetAxis("Switch")==0)
        {
            press = false;
        }

        //allow player to fire weapon after selecting the appropriate button
        if(Input.GetMouseButtonDown(0))
        {
            if(weaponPressed[0]&&Time.time-weaponCooldowns[0]>weapons[0].maxcd && !EventSystem.current.IsPointerOverGameObject())
            {
                Rigidbody2D proj = Instantiate(weapons[0].projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                Vector3 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,56000f)));
                Vector2 direction = (new Vector2(mousePos.x,mousePos.y) - proj.position);

                proj.velocity = direction.normalized * 5000;

                weaponCooldowns[0] = Time.time;
            }

            if (weaponPressed[1] && Time.time - weaponCooldowns[1] > weapons[1].maxcd&& !EventSystem.current.IsPointerOverGameObject())
            {
                Rigidbody2D proj = Instantiate(weapons[1].projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                Vector3 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 56000f)));
                Vector2 direction = (new Vector2(mousePos.x, mousePos.y) - proj.position);

                proj.velocity = direction.normalized * 5000;

                weaponCooldowns[1] = Time.time;
            }

            if (weaponPressed[2] && Time.time - weaponCooldowns[2] > weapons[2].maxcd&& !EventSystem.current.IsPointerOverGameObject())
            {
                Rigidbody2D proj = Instantiate(weapons[2].projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                Vector3 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 56000f)));
                Vector2 direction = (new Vector2(mousePos.x, mousePos.y) - proj.position);

                proj.velocity = direction.normalized * 5000;

                weaponCooldowns[2] = Time.time;
            }

            if (weaponPressed[3] && Time.time - weaponCooldowns[3] > weapons[3].maxcd&& !EventSystem.current.IsPointerOverGameObject())
            {
                Rigidbody2D proj = Instantiate(weapons[3].projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                Vector3 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 56000f)));
                Vector2 direction = (new Vector2(mousePos.x, mousePos.y) - proj.position);

                proj.velocity = direction.normalized * 5000;

                weaponCooldowns[3] = Time.time;
            }
        }

        //constants for the buttons

        string x = "";

        string selectedMessage = "[click anywhere to fire]\n[click button again to cancel]";
        Color defaultColor = new Color(0.0990566f, 1f, 0.2509106f, 0.71f);
        Color selectedColor = new Color(0.2747864f, 0.7079113f, 0.8962264f, 0.6705883f);
        //make the buttons display the appropriate text
        if(weaponPressed[0])
        {
            WeaponDisplays[0].transform.GetChild(1).GetComponent<Text>().text = selectedMessage;
            WeaponDisplays[0].GetComponent<Image>().color = selectedColor;

        }
        else if(!weaponPressed[0])
        {
            WeaponDisplays[0].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[0].name + "]";
            WeaponDisplays[0].GetComponent<Image>().color = defaultColor;
        }

        if (weaponPressed[1])
        {
            WeaponDisplays[1].transform.GetChild(1).GetComponent<Text>().text = selectedMessage;
            WeaponDisplays[1].GetComponent<Image>().color = selectedColor;
        }
        else if (!weaponPressed[1])
        {
            WeaponDisplays[1].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[1].name + "]";
            WeaponDisplays[1].GetComponent<Image>().color = defaultColor;
        }

        if (weaponPressed[2])
        {
            WeaponDisplays[2].transform.GetChild(1).GetComponent<Text>().text = selectedMessage;
            WeaponDisplays[2].GetComponent<Image>().color = selectedColor;
        }
        else if (!weaponPressed[2])
        {
            WeaponDisplays[2].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[2].name + "]";
            WeaponDisplays[2].GetComponent<Image>().color = defaultColor;
        }

        if (weaponPressed[3])
        {
            WeaponDisplays[3].transform.GetChild(1).GetComponent<Text>().text = selectedMessage;
            WeaponDisplays[3].GetComponent<Image>().color = selectedColor;
        }
        else if (!weaponPressed[3])
        {
            WeaponDisplays[3].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[3].name + "]";
            WeaponDisplays[3].GetComponent<Image>().color = defaultColor;
        }


	}

    //callbacks that are executed when the buttons are pressed

    public void SelectTarget1()
    {
        if(!weaponPressed[0]&& weapons[0].name != "weapon slot empty")
        {
            //WeaponDisplays[0].transform.GetChild(1).GetComponent<Text>().text = "[click anywhere to fire]\n[click button again to cancel]";
            weaponPressed[0] = true;
            weaponPressed[1] = false;
            weaponPressed[2] = false;
            weaponPressed[3] = false;

        }
        else if(weaponPressed[0])
        {
            //WeaponDisplays[0].transform.GetChild(1).GetComponent<Text>().text = "["+weapons[0].name+"]\n[hotkey: 1]";
            weaponPressed[0] = false;
        }
    }

    public void SelectTarget2()
    {
        if (!weaponPressed[1]&& weapons[1].name != "weapon slot empty")
        {
            //WeaponDisplays[1].transform.GetChild(1).GetComponent<Text>().text = "[click anywhere to fire]\n[click button again to cancel]";
            weaponPressed[1] = true;
            weaponPressed[0] = false;
            weaponPressed[2] = false;
            weaponPressed[3] = false;

        }
        else if (weaponPressed[1])
        {
            //WeaponDisplays[1].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[1].name + "]\n[hotkey: 1]";
            weaponPressed[1] = false;
        }
    }

    public void SelectTarget3()
    {
        if (!weaponPressed[2]&&weapons[2].name!="weapon slot empty")
        {
            //WeaponDisplays[2].transform.GetChild(1).GetComponent<Text>().text = "[click anywhere to fire]\n[click button again to cancel]";
            weaponPressed[2] = true;
            weaponPressed[1] = false;
            weaponPressed[0] = false;
            weaponPressed[3] = false;

        }
        else if (weaponPressed[2])
        {
            //WeaponDisplays[2].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[1].name + "]\n[hotkey: 1]";
            weaponPressed[2] = false;
        }
    }

    public void SelectTarget4()
    {
        if (!weaponPressed[3]&& weapons[3].name != "weapon slot empty")
        {
            //WeaponDisplays[3].transform.GetChild(1).GetComponent<Text>().text = "[click anywhere to fire]\n[click button again to cancel]";
            weaponPressed[3] = true;
            weaponPressed[1] = false;
            weaponPressed[2] = false;
            weaponPressed[0] = false;

        }
        else if (weaponPressed[3])
        {
            //WeaponDisplays[3].transform.GetChild(1).GetComponent<Text>().text = "[" + weapons[1].name + "]\n[hotkey: 1]";
            weaponPressed[3] = false;
        }
    }


}

public abstract class ShipWeapon
{
    public int damage;
    public string name;
    public float maxcd;
    public GameObject projectile;

    public ShipWeapon()
    {
        //projectile = Resources.Load<GameObject>("Laser");
    }

    public abstract void HitEffect(EnemySpaceship target);
}

public class EmptyWeapon : ShipWeapon
{
    public EmptyWeapon()
    {
        name = "weapon slot empty";
        damage = 0;
        maxcd = 0;
    }
    public override void HitEffect(EnemySpaceship target)
    {
        
    }
}

public class PlasmaCannon : ShipWeapon
{
    public PlasmaCannon()
    {
        projectile = Resources.Load<GameObject>("Laser");
        name = "Plasma Cannon";
        damage = 10;
        maxcd = 2;
    }

    public override void HitEffect(EnemySpaceship target)
    {
        projectile = Resources.Load<GameObject>("Laser");
        target.health = target.health - damage;
    }
}

public class SlowCannon : ShipWeapon
{
    public SlowCannon()
    {
        projectile = Resources.Load<GameObject>("Laser");
        name = "Slow Cannon";
        damage = 70;
        maxcd = 5;
    }

    public override void HitEffect(EnemySpaceship target)
    {
        projectile = Resources.Load<GameObject>("Laser");
        target.health = target.health - damage;
    }
}






