using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCannonEnemyController : MonoBehaviour
{
    //Initialize variables to set the boundaries for the AI's path as well as its velocity
    public float walkSpeed = 2.0F;
    public float wallDown = -4.0F;
    public float wallUp = 4.0F;
    float walkingDirection = 1.0F;

    public static int bigCannonEnemyPointValue = 10;

    public GameObject basePlayer;
    public GameObject beam;
    public GameObject ui;

    Vector3 walkAmount;

    private Animator animator;

    private bool enemyDestroyed = false;
    private bool firingMaLaser = false;

    public float firingTimer;
    public float laserTime = 5.0F;
    public float cooldown = 0F;
    public float cooldownTimer = 2F;

    bool invincible;
    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        basePlayer = GameObject.FindWithTag("Player");
        ui = GameObject.FindWithTag("UI Component");
        LoadCannon();
        invincible = false;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (firingMaLaser == false)
        {
            WalkTheDog();
        }

        else if (firingMaLaser == true)
        {
            walkSpeed = 0F;
            cooldown += 1F * Time.deltaTime;
            if (cooldown >= cooldownTimer)
            {
                firingMaLaser = false;
                walkSpeed = 2F;
            }
        }

        firingTimer += 1.0F * Time.deltaTime;
        if (firingTimer >= 4)
        {
            animator.SetTrigger("warn");
        }
        if (firingTimer >= laserTime)
        {
            ShootinTime();
        }

        //Check if the enemy has been destroyed and, if it has, stop it from moving so that the explosion animation is stationary
        if (enemyDestroyed == true)
        {
            walkSpeed = 0F;
        }
    }

    void OnTriggerEnter2D(Collider2D laser)
    {
        if (!invincible)
        {
            if (laser.gameObject.tag == "Player Laser")
            {
                invincible = true;
                Destroy(laser.gameObject);
                walkSpeed = 0.0F;
                source.PlayOneShot(explosionSound);
                animator.SetTrigger("enemyDestroy");
                Destroy(gameObject, 0.5F);
                enemyDestroyed = true;  //Set the variable to true so that smoke cloud won't keep moving
                if (enemyDestroyed == true)
                {
                    UIManager.count += bigCannonEnemyPointValue;
                }
            }
        }
    }

    void ShootinTime()
    {
        firingMaLaser = true;
        Vector2 spawnPos = transform.position;
        spawnPos.x += .35F;
        source.PlayOneShot(shootSound);
        Instantiate(beam, spawnPos, transform.rotation);
        LoadCannon();
        cooldown = 0F;
        cooldownTimer = 2F;
    }

    void LoadCannon()
    {
        firingTimer = 0;
    }

    void WalkTheDog()
    {
        //Set the AI off
        walkAmount.y = walkingDirection * walkSpeed * Time.deltaTime;
        if (walkingDirection > 0.0F && transform.position.y >= wallUp)
        {
            walkingDirection = -1.0F;
        }
        else if (walkingDirection < 0.0F && transform.position.y <= wallDown)
        {
            walkingDirection = 1.0F;
        }
        transform.Translate(walkAmount);
    }
}
