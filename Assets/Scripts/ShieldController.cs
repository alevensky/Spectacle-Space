using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private Animator animator;
    //Variable that holds the ticking timer
    public float destroyTimer;
    //Variable that holds the time a projectile will stay active (in seconds)
    public int destroyTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer += 1.0F * Time.deltaTime;
        if ((destroyTimer >= destroyTime) || ShieldEnemyController.enemyDestroyed == true)
        {
            Destroy(gameObject);
            ShieldEnemyController.shieldActive = false;
        }
    }

    void OnTriggerEnter2D(Collider2D laser)
    {
        if (laser.gameObject.tag == "Player")
        {
            animator.SetTrigger("playerDeath");
            Destroy(gameObject, 0.5F);
            laser.gameObject.GetComponent<PlayerController>().alive = false;
            Time.timeScale = 0;
        }

        else if (laser.gameObject.tag == "Player Laser")
        {
            Destroy(laser.gameObject);
        }
    }
}
