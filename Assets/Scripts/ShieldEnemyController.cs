using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyController : MonoBehaviour
{
    private Animator animator;
    public static int shieldEnemyPointValue = 10;
    public static bool enemyDestroyed = false;
    public GameObject basePlayer;
    private float timeTillShield = 5F;
    private float summonShield = 0F;
    public GameObject shield;
    public static bool shieldActive = false;
    public GameObject ui;
    public float walkSpeed = 2.0F;
    public float wallLeft = 0.0F;
    public float wallRight = 5.0F;
    float walkingDirection = 1.0F;
    Vector3 walkAmount;
    bool invincible;
    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioSource source;
    public AudioClip shieldOn;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        basePlayer = GameObject.FindWithTag("Player");
        ui = GameObject.FindWithTag("UI Component");
        invincible = false;
        source = GetComponent<AudioSource>();
        GameController.shieldOnField = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldActive == false)
        {
            summonShield += 1F * Time.deltaTime;
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
            //Check if the enemy has been destroyed and, if it has, stop it from moving so that the explosion animation is stationary
            if (enemyDestroyed == true)
            {
                walkSpeed = 0.0F;
            }
        }
        if ((summonShield >= timeTillShield) && (shieldActive == false))
        {
            source.PlayOneShot(shieldOn);
            shieldActive = true;
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            GameObject newShield = (GameObject)Instantiate(shield, myPos, transform.rotation);
            timeTillShield = 12.5F;
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
                animator.SetTrigger("enemyDestroy");
                source.PlayOneShot(explosionSound);
                Destroy(gameObject, 0.5F);
                GameController.shieldOnField = false;
                enemyDestroyed = true;  //Set the variable to true so that smoke cloud won't keep moving
                if (enemyDestroyed == true)
                {
                    UIManager.count += shieldEnemyPointValue;
                }
            }
        }
    }
}
