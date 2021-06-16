using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningEnemyController : MonoBehaviour
{
    public static int spinningEnemyPointValue = 10;

    public float fireRate = 3F;
    public bool canFire = true;
    public float fireTimer = 0.0F;

    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioSource source;

    public GameObject basePlayer;

    public GameObject enemyLaser;
    public Transform launcher;
    public float projectileSpeed = 6F;

    private bool enemyDestroyed = false;

    private Animator animator;
    public GameObject ui;

    bool invincible;

    private float coolTimer;
    private float coolTime = 1F;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        basePlayer = GameObject.FindWithTag("Player");
        ui = GameObject.FindWithTag("UI Component");
        invincible = false;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    // Function will rotate the enemy continually until the enemyDestroyed condition is met - also calls the function to make the enemy fire every frame at a specified rate
    void Update()
    {
        if (enemyDestroyed == false)
        {
            transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
            if (coolTimer < coolTime)
            {
                coolTimer += 1F * Time.deltaTime;
                this.GetComponent<Collider2D>().enabled = false;
            }

            else if (coolTimer >= coolTime)
            {
                this.GetComponent<Collider2D>().enabled = true;
                SpinningEnemyFire();
            }
        }
    }
    //This function will allow the enemy to fire using variables defined above to control the rate of the projectiles
    private void SpinningEnemyFire()
    {
        if (canFire && Time.time > fireTimer)
        {
            fireTimer = Time.time + fireRate;
            GameObject projectile = (GameObject)Instantiate(enemyLaser, launcher.position, launcher.rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up * projectileSpeed;
            source.PlayOneShot(shootSound);
            canFire = true;
        }
    }

    //This function detects if the enemy has been hit by the player's projectiles and destroys it if it has
    void OnTriggerEnter2D(Collider2D laser)
    {
        if (!invincible)
        {
            if (laser.gameObject.tag == "Player Laser")
            {
                invincible = true;
                canFire = false;
                Destroy(laser.gameObject);
                animator.SetTrigger("enemyDestroy");
                source.PlayOneShot(explosionSound);
                Destroy(gameObject, 0.5F);
                enemyDestroyed = true;  //Set the variable to true so that smoke cloud won't keep moving once enemy is destroyed
                if (enemyDestroyed == true)
                {
                    UIManager.count += spinningEnemyPointValue;
                }
            }
        }
    }
}
