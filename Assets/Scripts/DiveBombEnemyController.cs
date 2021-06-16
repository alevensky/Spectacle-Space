using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveBombEnemyController : MonoBehaviour
{
    private Animator animator;
    public Transform target;
    public GameObject player;
    public static int diveEnemyPointValue = 10;
    public float trackSpeed = .5F;
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
            targetPos.x = target.position.x - transform.position.x;
            targetPos.y = target.position.y - transform.position.y;
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
        if (!invincible)
        {
            if (laser.gameObject.tag == "Player Laser")
            {
                trackSpeed = 0F;
                isDead = true;
                Destroy(laser.gameObject);
                invincible = true;
                animator.SetTrigger("enemyDestroy");
                source.PlayOneShot(explosionSound);
                Destroy(gameObject, 0.5F);
                UIManager.count += diveEnemyPointValue;
            }
        }
    }
}
