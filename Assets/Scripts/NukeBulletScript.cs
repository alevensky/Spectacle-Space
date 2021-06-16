using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeBulletScript : MonoBehaviour
{
    public float bulletSpeed = 7F;
    float flyingDirection = -1.0F;
    Vector3 flyAmount;
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        flyAmount.x = flyingDirection * bulletSpeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = flyAmount;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }
}
