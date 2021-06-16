using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeBossController : MonoBehaviour
{
    public Transform target;
    public GameObject player;
    private Animator animator;
    public GameObject missile;
    public GameObject bullet;
    public float trackSpeed = 1F;

    public static int nukeBossPointValue = 200;

    public GameObject bombEnemy;
    public GameObject diveBombEnemy1;
    public GameObject diveBombEnemy2;
    public GameObject nuke;

    private float machineGunTimer;
    public float machineGunCooldown = 2F;

    public float health = 300;
    private bool addTime = true;

    public float addTimer;
    public float addCooldown = 3F;

    public float missileCooldown = 10F;
    private float missileTimer;

    private float timeTillNuke;
    public float nukeCooldown = 7.5F;
    public GameObject ui;
    public float wallDown = -7.0F;
    public float wallUp = 7.0F;
    bool invincible;
    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioSource source;
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

    void LateUpdate()
    {
        
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
                FireMissile();
            }

            timeTillNuke += 1F * Time.deltaTime;
            if (timeTillNuke >= nukeCooldown)
            {
                addTime = false;
            }
            Vector2 direction = new Vector2(0F, target.position.y - transform.position.y);
            gameObject.GetComponent<Rigidbody2D>().velocity = direction * trackSpeed;
            if (direction.y > 0F && transform.position.x >= wallUp)
            {
                trackSpeed = 0F;
            }
            else if (direction.y < 0.0F && transform.position.x <= wallDown)
            {
                trackSpeed = 0F;
            }
            else
            {
                trackSpeed = 1F;
            }

            addTimer += 1F * Time.deltaTime;

            if ((addTime == true) && (addTimer >= addCooldown))
            {
                SummonAdds();
                addTimer = 0F;
            }
            else if (addTime == false)
            {
                trackSpeed = 0F;
                FireNuke();
                trackSpeed = 1F;
                timeTillNuke = 0F;
                addTime = true;
            }
        }
    }

    private void FireBullets()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        machineGunTimer = 0F;
        source.PlayOneShot(shootSound);
    }

    private void FireMissile()
    {
        Vector2 missile1InstantPos = new Vector2(transform.position.x, transform.position.y);
        Instantiate(missile, missile1InstantPos, transform.rotation);
        missileTimer = 0F;
    }

    void FireNuke()
    {
        source.PlayOneShot(missileSound);
        Vector2 nukePos = new Vector2(transform.position.x, transform.position.y);
        Instantiate(nuke, nukePos, transform.rotation);
    }

    void SummonAdds()
    {
        float selection = Random.Range(0F, 3F);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
        if (selection <= 1F)
        {
            Instantiate(bombEnemy, enemyPos, transform.rotation);
        }
        else if (selection <= 2F)
        {
            Instantiate(diveBombEnemy1, enemyPos, transform.rotation);
        }
        else
        {
            Instantiate(diveBombEnemy2, enemyPos, transform.rotation);
        }
    }

    void OnTriggerEnter2D(Collider2D laser)
    {
        if (!invincible)
        {
            if (laser.gameObject.tag == "Player Laser")
            {
                Destroy(laser.gameObject);
                health -= 2F;
                Debug.Log("Boss's health is: " + health);
                if (health <= 0F)
                {
                    isDead = true;
                    animator.SetTrigger("enemyDestroy");
                    source.PlayOneShot(explosionSound);
                    Destroy(gameObject, 1F);
                    UIManager.bossOngoing = false;
                    UIManager.isBossTime = false;
                    trackSpeed = 0F; // stop the enemy from moving after it's destroyed while the animation plays
                    UIManager.count += nukeBossPointValue;
                    invincible = true;
                }
            }
        }
    }
}
