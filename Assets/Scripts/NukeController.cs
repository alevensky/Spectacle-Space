using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeController : MonoBehaviour
{
    public float missileSpeed = 15F;
    float flyingDirection = -1.0F;
    Vector3 flyAmount;
    private Rigidbody2D rigid;
    public GameObject explosion;
    public GameObject informationNuke;
    public AudioClip explosionSound;
    public AudioSource source;
    public Transform target;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        Invoke("SummonWarning", 3F);
        Invoke("SummonExplosion", 5F);
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        flyAmount.x = flyingDirection * missileSpeed;
        rigid.AddForce(flyAmount);
    }

    void SummonWarning ()
    {
        Vector2 informationPos = new Vector2(target.position.x, target.position.y + 2F);
        Instantiate(informationNuke, informationPos, target.rotation);
    }

    void SummonExplosion ()
    {
        Vector2 explosionPos = new Vector2(0F, 0F);
        Instantiate(explosion, explosionPos, transform.rotation);
        source.PlayOneShot(explosionSound);
        Destroy(gameObject);
    }
}
