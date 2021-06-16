using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemyController : MonoBehaviour
{
    public Transform target;
    public GameObject player;
    public static int bombEnemyPointValue = 10;
    public float trackSpeed = .5F;
    public GameObject bombExplosion;
    public GameObject ui;
    bool invincible;
    public AudioClip explosionSound;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        ui = GameObject.FindWithTag("UI Component");
        invincible = false;
        source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //rotate to look at the player
        transform.LookAt(target.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation
        Vector2 direction = new Vector2(target.position.x, target.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, trackSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D laser)
    {
        if (!invincible)
        {
            if (laser.gameObject.tag == "Player Laser")
            {
                Instantiate(bombExplosion, transform.position, transform.rotation);
                invincible = true;
                trackSpeed = 0F;
                source.PlayOneShot(explosionSound);
                Destroy(gameObject, 0.5F);
                Destroy(laser.gameObject);
                UIManager.count += bombEnemyPointValue;
            }
        }
        
        else if (laser.gameObject.tag == "Player")
        {
            Instantiate(bombExplosion, transform.position, transform.rotation);
            trackSpeed = 0F;
            source.PlayOneShot(explosionSound);
            Destroy(gameObject, 0.5F);
            laser.gameObject.GetComponent<PlayerController>().alive = false;
            Time.timeScale = 0;
        }
    }
}
