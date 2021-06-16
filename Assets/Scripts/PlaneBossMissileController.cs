using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBossMissileController : MonoBehaviour
{
    public GameObject player;
    public float trackSpeed = .25F;
    public GameObject bombExplosion;
    public Transform target;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;
    private Animator animator;
    public AudioClip explosionSound;
    public AudioSource source;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        animator = GetComponent<Animator>();
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
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90F));
            Vector2 direction = new Vector2(target.position.x, target.position.y);
            gameObject.GetComponent<Rigidbody2D>().velocity = direction * trackSpeed;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, trackSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0F, 0F);
        }
    }

    void OnTriggerEnter2D(Collider2D laser)
    {
        if (laser.gameObject.tag == "Player Laser")
        {
            isDead = true;
            Instantiate(bombExplosion, transform.position, transform.rotation);
            trackSpeed = 0F;
            source.PlayOneShot(explosionSound);
            Destroy(gameObject, 0.5F);
        }
        else if (laser.gameObject.tag == "Player")
        {
            Instantiate(bombExplosion, transform.position, transform.rotation);
            trackSpeed = 0F;
            source.PlayOneShot(explosionSound);
            Destroy(gameObject, 0.5F);
        }
    }
}
