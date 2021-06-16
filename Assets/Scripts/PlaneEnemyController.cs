using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneEnemyController : MonoBehaviour
{
    public Transform target;
    public GameObject missile;
    public GameObject bullet;
    public GameObject player;
    public int planeBossPointValue = 100;
    public float trackSpeed = 1F;
    public float machineGunCooldown = 2F;
    public float health = 200F;
    private float machineGunTimer;
    public float missileCooldown = 10F;
    private float missileTimer;
    private bool isShootingMachineGun;
    private bool isShootingMissile;
    private Animator animator;
    public GameObject ui;
    bool invincible;
    public float wallLeft = -7.0F;
    public float wallRight = 7.0F;
    public AudioClip explosionSound;
    public AudioSource source;
    public AudioClip shootSound;
    public AudioClip missileSound;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        animator = GetComponent<Animator>();
        ui = GameObject.FindWithTag("UI Component");
        invincible = false;
        source = GetComponent<AudioSource>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            machineGunTimer += 1.0F * Time.deltaTime;
            if (machineGunTimer >= machineGunCooldown)
            {
                FireBullets();
            }

            missileTimer += 1F * Time.deltaTime;
            if (missileTimer >= missileCooldown)
            {
                FireMissiles();
            }

            Vector2 direction = new Vector2(target.position.x - transform.position.x, 0F);
            gameObject.GetComponent<Rigidbody2D>().velocity = direction * trackSpeed;
            if (direction.x > 0F && transform.position.x >= wallRight)
            {
                trackSpeed = 0F;
            }
            else if (direction.x < 0.0F && transform.position.x <= wallLeft)
            {
                trackSpeed = 0F;
            }
            else
            {
                trackSpeed = 1F;
            }
        }
    }

    private void FireBullets()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        machineGunTimer = 0F;
        source.PlayOneShot(shootSound);
    }

    private void FireMissiles()
    {
        source.PlayOneShot(missileSound);
        Vector2 missile1InstantPos = new Vector2(transform.position.x + 0.7F, transform.position.y);
        Vector2 missile2InstantPos = new Vector2(transform.position.x - 0.7F, transform.position.y);
        Instantiate(missile, missile1InstantPos, transform.rotation);
        Instantiate(missile, missile2InstantPos, transform.rotation);
        missileTimer = 0F;
    }

    void OnTriggerEnter2D(Collider2D laser)
    {
        if (!invincible)
        {
            if (laser.gameObject.tag == "Player Laser")
            {
                Destroy(laser.gameObject);
                health -= 2F;
                Debug.Log("Plane boss's health is now: " + health);
                if (health <= 0F)
                {
                    isDead = true;
                    animator.SetTrigger("enemyDestroy");
                    source.PlayOneShot(explosionSound);
                    Destroy(gameObject, 1F);
                    UIManager.bossOngoing = false;
                    UIManager.isBossTime = false;
                    trackSpeed = 0F; // stop the enemy from moving after it's destroyed while the animation plays
                    UIManager.count += planeBossPointValue;
                    invincible = true;
                }
            }
        }
    }
}
