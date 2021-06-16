using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackEnemyController : MonoBehaviour
{
    //Initialize variables to set the boundaries for the AI's path as well as its velocity
    public float walkSpeed = 2.0F;
    public float wallLeft = 0.0F;
    public float wallRight = 5.0F;
    float walkingDirection = 1.0F;

    public static int blackEnemyPointValue = 10;

    public float fireRate = 1.0F;
    public bool canFire = true;
    public float fireTimer = 0.0F;

    public GameObject basePlayer;
    public GameObject ui;

    public Rigidbody2D enemyLaser;
    public Transform launcher;
    public float projectileSpeed = 2F;

    private bool enemyDestroyed = false;

    bool invincible;
    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioSource source;

    private float coolTimer;
    private float coolTime = 1F;

    //Initialize the variable as a Vector3
    Vector3 walkAmount;

    private Animator animator;

    //Initiate the animator component on start
    private void Start()
    {
        animator = GetComponent<Animator>();
        basePlayer = GameObject.FindWithTag("Player");
        ui = GameObject.FindWithTag("UI Component");
        invincible = false;
        source = GetComponent<AudioSource>();
    }

    //Function that will walk the AI back and forth across the screen repeatedly
    private void Update()
    {

        if (coolTimer < coolTime)
        {
            this.GetComponent<Collider2D>().enabled = false;
            coolTimer += 1F * Time.deltaTime;
        }

        else if (coolTimer >= coolTime)
        {
            this.GetComponent<Collider2D>().enabled = true;
            Enemy1Fire();
        }


        //Set the AI off
        walkAmount.x = walkingDirection * walkSpeed * Time.deltaTime;
        if (walkingDirection > 0.0F && transform.position.x >= wallRight)
        {
            walkingDirection = -1.0F;
        }
        else if (walkingDirection < 0.0F && transform.position.x <= wallLeft)
        {
            walkingDirection = 1.0F;
        }
        transform.Translate(walkAmount);
        if (enemyDestroyed == true)
        {
            walkSpeed = 0.0F;
        }

    }

    //This function will allow the enemy to fire using variables defined above to control the rate of the projectiles
    private void Enemy1Fire ()
    {
        if (canFire && Time.time > fireTimer)
        {
            fireTimer = Time.time + fireRate;
            Instantiate(enemyLaser, launcher.position, launcher.rotation);
            canFire = true;
            source.PlayOneShot(shootSound);
        }
    }

    //This function detects if the enemy has been hit by the player's laser and destroys it if it has
    void OnTriggerEnter2D(Collider2D laser)
    { 
        if (!invincible)
        {
            if (laser.gameObject.tag == "Player Laser")
            {
                Destroy(laser.gameObject);
                invincible = true;
                canFire = false;
                walkSpeed = 0.0F;
                animator.SetTrigger("enemyDestroy");
                source.PlayOneShot(explosionSound);
                Destroy(gameObject, 0.5F);
                enemyDestroyed = true;  //Set the variable to true so that smoke cloud won't keep moving
                UIManager.count += blackEnemyPointValue;
            }
        }
    }
}