using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Vector2 playerPosition;

    [SerializeField]
    private int hp = 5;
    [SerializeField]
    private float jumpForce = 0.5f;
    [SerializeField]
    private float horSpeed = 6.0f;
    [SerializeField]
    private int playerLayer = 9;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject bootProjectile;
    [SerializeField]
    private Text bootsBulletsText;

    private float maxSpeed = 10.0f;

    //Player mechanics
    private bool jump = false;
    private bool left = false;
    private bool right = false;
    
    private int maxChargeBoots = 10; //TODO: Max charge is going to change along the game? If not, add readonly 
    private int currentChargeBoots = 10;
    
    private Rigidbody2D rigidBody;
    private float onTopPlatformDistance = 0.6f; //TODO: Review after new sprites

    private bool consume = true;
    private int maxHP;
    private float waitTime;
    private float slowRatio;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.freezeRotation = true;
        maxHP = hp;
        waitTime = 0;
        slowRatio = 1;
    }

    private void Update()
    {
        if (!consume && Time.time > waitTime)
            consume = true;

        if (slowRatio != 1 && Time.time > waitTime)
            slowRatio = 1;

        //TODO: Check if player at a platform or in the air
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && currentChargeBoots > 0)
        {
            jump = true;
            if(consume)
                currentChargeBoots--;
            //Debug.Log("Saltei curr: " + currentChargeBoots);
        }
       
    }

    void FixedUpdate()
    {
       // Debug.Log("Boots: " + currentChargeBoots);
       //Player is mid air
        if (jump && !onTopOfPlatform())
        {
            rigidBody.velocity = new Vector2(0, 0);
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            Instantiate(bootProjectile, spawnPoint.position, spawnPoint.rotation);
            jump = false;

        }
        //Player on top of platform - cannot shoot boots
        else if(jump && onTopOfPlatform())
        {            
            rigidBody.velocity = new Vector2(0, 0);
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jump = false;   
        }

        float hor = Input.GetAxis("Horizontal");

        rigidBody.velocity = new Vector2(hor * horSpeed, rigidBody.velocity.y) / slowRatio;

        if(rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }


        playerPosition = transform.position;
    }
    
    bool onTopOfPlatform()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, onTopPlatformDistance, playerLayer);

        if ( hit.collider != null) {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if (hit.collider.tag == "platformNormal")
                rechargeBoots();

            return true;
                
        }

        return false;
           
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            playerHit();
            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile")
        {
            playerHit();
        }

        if (other.tag == "EnemyHead")
        {
            print("TOLADA");
            stumpKillEnemy(other.transform.parent.gameObject);
        }
        if (other.tag == "PowerUp")
            Destroy(other.gameObject);
    }

    public void playerHit()
    {
        rigidBody.AddForce(new Vector2(-5f, 3f), ForceMode2D.Impulse);
        hp--;
        if (hp <= 0)
        {
            playerDie();
        }
    }

    public void stumpKillEnemy(GameObject enemy)
    {
        Destroy(enemy);
        rigidBody.velocity = new Vector2(0, 0);
        rigidBody.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
        rechargeBoots();
    }

    public void rechargeBoots()
    {
        currentChargeBoots = maxChargeBoots;
        bootsBulletsText.text = currentChargeBoots.ToString();
    }

    public void playerDie()
    {
        //gameover
        Destroy(gameObject);
    }

    public void increaseHP()
    {
        if(hp < maxHP)
        {
            hp++;
        }
    }

    public void setConsume()
    {
        consume = true;
    }

    public void unsetConsume(float time)
    {
        consume = false;
        waitTime = Time.time + time;
    }

    public void slowSpeed(float time, float pSlowRatio)
    {
        slowRatio = pSlowRatio;
        waitTime = Time.time + time;
    }
}
