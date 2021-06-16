using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeBossBeam : MonoBehaviour
{
    [Header("Laser pieces")]
    public GameObject laserStart;
    public GameObject laserMiddle;
    public GameObject laserEnd;

    private GameObject start;
    private GameObject middle;
    private GameObject end;

    public float destroyTimer = 0F;
    public int destroyTime = 2;

    // Update is called once per frame
    void Update()
    {
        destroyTimer += 1F * Time.deltaTime;
        if (destroyTimer >= destroyTime)
        {
            GameObject.Destroy(gameObject);
        }
        if (start == null)
        {
            start = Instantiate(laserStart) as GameObject;
            start.transform.parent = this.transform;
            start.transform.localPosition = Vector2.zero;
        }

        if (middle == null)
        {
            middle = Instantiate(laserMiddle) as GameObject;
            middle.transform.parent = this.transform;
            middle.transform.localPosition = Vector2.zero;
        }

        // Define a size
        float maxLaserSize = 20f;
        float currentLaserSize = maxLaserSize;

        // Raycast to the left (negative right)
        Vector2 laserDirection = -(this.transform.right);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, laserDirection, maxLaserSize);

        if (hit.collider != null)//Hit
        {
            currentLaserSize = Vector2.Distance(hit.point, this.transform.position);
            if (end == null)
            {
                end = Instantiate(laserEnd) as GameObject;
                end.transform.parent = this.transform;
                end.transform.localPosition = Vector2.zero;
            }

            if (hit.collider.gameObject.tag == "Player")
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                Animator animator = player.GetComponent<Animator>();
                animator.SetTrigger("playerDeath");
                Destroy(player.gameObject, 0.5F);
                player.gameObject.GetComponent<PlayerController>().alive = false;
                Time.timeScale = 0;
            }
        }

        else
        {
            // Nothing hit
            if (end != null) Destroy(end);
        }
        float startSpriteWidth = start.GetComponent<Renderer>().bounds.size.x;
        float endSpriteWidth = 0f;
        if (end != null) endSpriteWidth = end.GetComponent<Renderer>().bounds.size.x;
        middle.transform.localScale = new Vector3(currentLaserSize - startSpriteWidth, middle.transform.localScale.y, middle.transform.localScale.z);
        middle.transform.localPosition = new Vector2((currentLaserSize / 2f), 0f);
        if (end != null)
        {
            end.transform.localPosition = new Vector2(currentLaserSize, 0f);
        }
    }
}
