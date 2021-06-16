using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveBombEnemy2Controller : MonoBehaviour
{
    private Animator animator;
    public Transform target;
    public GameObject player;
    public static int diveEnemy2PointValue = 10;
    public float trackSpeed = .5F;
    private int enemyHealth = 1;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;
    public GameObject ui;
    bool invincible;
    public AudioClip explosionSound;
    public AudioSource source;
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
            targetPos = target.position;
            thisPos = transform.position;
            targetPos.x = targetPos.x - thisPos.x;
            targetPos.y = targetPos.y - thisPos.y;
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Vector2 direction = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
            gameObject.GetComponent<Rigidbody2D>().velocity = direction * trackSpeed;
        }
        else if (isDead)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0F, 0F);
        }
    }

    //This function detects if the enemy has been hit by the player's laser and destroys it if it has
    void OnTriggerEnter2D(Collider2D laser)
    {
        if (laser.gameObject.tag == "Player Laser")
        {
            if (!invincible)
            {
                if (enemyHealth > 0)
                {
                    enemyHealth -= 1;
                    animator.SetTrigger("isDamaged");
                }
                else if (enemyHealth <= 0)
                {
                    isDead = true;
                    animator.SetTrigger("enemyDestroy");
                    invincible = true;
                    trackSpeed = 0F;
                    source.PlayOneShot(explosionSound);
                    Destroy(gameObject, 0.5F);
                    UIManager.count += diveEnemy2PointValue;
                }
            }
            Destroy(laser.gameObject);
            
        }
    }
}
