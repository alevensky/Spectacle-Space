using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    //Variable that holds the ticking timer
    public float destroyTimer;
    //Variable that holds the time a projectile will stay active (in seconds)
    public float destroyTime = 2F;

    //Increments the timer variable every second and destroys the game object when it passes the time stored in the destroyTime variable
    void Update()
    {
        destroyTimer += 1.0F * Time.deltaTime;
        if (destroyTimer >= destroyTime)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
