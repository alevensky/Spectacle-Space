using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public bool alive;

    public Sprite defaultPlayer;
    public Sprite fireRateUpgrade;
    public Sprite fireModeUpgradeBeam;
    public Sprite absorbingShieldUpgrade;

    public AudioClip collectSound;
    public AudioClip collectAwaySound;
    public AudioClip shootSoundBase;
    public AudioClip shootFastSound;
    public AudioClip shootBigSound;
    public AudioClip explosionSound;

    private AudioClip currentShootSound;

    private AudioSource source;

    private float currCountdownValue;

    private SpriteRenderer spriteRenderer;

    //Provide way to control speed of player
    public float speed;

    //Grab the Rigidbody2D object for use in script
    private Rigidbody2D rigid;

    //Assign the laser object to the variable baseLaser
    public GameObject baseLaser;

    public GameObject upgradedLaser;

    public GameObject fireModeUpgrade;

    private GameObject thingToFire;

    //Assign a public variable to the speed of the base player projectile
    public float baseLaserSpeed = 8F;

    //Variables that control the rate of fire for the player
    public float fireRate = .5F;
    public bool canFire = true;
    public float fireTimer = 0.0F;
    public float projectileSpeed = 12F;

    private Animator animator;
    private bool invincible;
    private float invincibleTimer;
    private float invincibleEnd;

    private float moveVertical;
    private float moveHorizontal;

    //Initialize
    private void Start()
    {
        spriteRenderer =  GetComponent<SpriteRenderer>();
        //Get Rigidbody2D component
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Initialize count to 0
        alive = true;
        speed = 6;
        invincible = false;
        source = GetComponent<AudioSource>();
    }

    //Function to fire a projectile on mouse click/hold based on assigned fire-rate
    //Also checks what projectile to use based on the animation playing
    private void Update()
    {
        if (invincible)
        {
            invincibleTimer += 1F * Time.deltaTime;
            if (invincibleTimer >= invincibleEnd)
            {
                invincible = false;
            }
        }

        if (FireSpeedDetector.fireSpeedAnimPlaying == true)
        {
            thingToFire = upgradedLaser;
            fireRate = .5F;
            currentShootSound = shootFastSound;
        }
    
        else if (FireModeDetector.fireModeAnimPlaying == true)
        {
            thingToFire = fireModeUpgrade;
            fireRate = .1F;
            currentShootSound = shootBigSound;
        }

        else
        {
            thingToFire = baseLaser;
            fireRate = .5F;
            currentShootSound = shootSoundBase;
        }

        if ((Input.GetKey(KeyBindScript.keys["Fire"])) && (canFire && Time.time > fireTimer))
        {
            fireTimer = Time.time + fireRate;
            Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = target - myPos;
            direction.Normalize();
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);
            GameObject projectile = (GameObject)Instantiate(thingToFire, myPos, rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            source.PlayOneShot(currentShootSound);
            canFire = true;
        } 
    }

    //Script to actually move the player - using FixedUpdate since script is collision-based
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyBindScript.keys["Up"]))
        {
            moveVertical = 1F;
        }
        else if (Input.GetKeyUp(KeyBindScript.keys["Up"]))
        {
            moveVertical = 0F;
        }
        else if (Input.GetKey(KeyBindScript.keys["Down"]))
        {
            moveVertical = -1F;
        }
        else if (Input.GetKeyUp(KeyBindScript.keys["Down"]))
        {
            moveVertical = 0F;
        }
        else if (Input.GetKey(KeyBindScript.keys["Left"]))
        {
            moveHorizontal = -1F;
        }
        else if (Input.GetKeyUp(KeyBindScript.keys["Left"]))
        {
            moveHorizontal = 0F;
        }
        else if (Input.GetKey(KeyBindScript.keys["Right"])) 
        {
            moveHorizontal = 1F;
        }
        else if (Input.GetKeyUp(KeyBindScript.keys["Right"]))
        {
            moveHorizontal = 0F;
        }

        rigid.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);
        moveVertical = 0F;
        moveHorizontal = 0F;
    }

    //Detect when player collides with pickup
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            source.PlayOneShot(collectSound);
            //Remove pickup once player collides with it
            Destroy(other.gameObject);

            //Play the collectible pickup animation
            animator.SetTrigger("pickupCollectible");
            ChangeSprite();
        }

        if ((other.gameObject.tag == "Enemy Laser") || (other.gameObject.tag == "Enemy"))
        {
            if (invincible)
            {
                return;
            }
            if (AbsorbingShieldDetector.absorbingShieldAnimPlaying == true)
            {
                animator.SetTrigger("animEnd");
                AbsorbingShieldDetector.absorbingShieldAnimPlaying = false;
                invincible = true;
                invincibleEnd = 1.5F;
            }
            else
            {
                speed = 0.0F;
                animator.SetTrigger("playerDeath");
                Destroy(gameObject, 0.5F);  //Delays the object's destruction by a float
                alive = false;
                source.PlayOneShot(explosionSound);
                Time.timeScale = 0;
                FireModeDetector.fireModeAnimPlaying = false;
                FireSpeedDetector.fireSpeedAnimPlaying = false;
                AbsorbingShieldDetector.absorbingShieldAnimPlaying = false;
            }
        }
    }

    //Function gets a random number and sets the sprite in the renderer based on the random number
    void ChangeSprite()
    {
        float changeToThis = Random.Range(0.0F, 3.0F);
        if (changeToThis < 1.0F)
        {
            animator.SetTrigger("fireSpeed");
            StartCoroutine(StartCountdown());
        }
        else if (changeToThis < 2.0F)
        {
            animator.SetTrigger("fireMode");
            StartCoroutine(StartCountdown());
        }
        else if (changeToThis < 3.0F)
        {
            animator.SetTrigger("absorbingShield");
            StartCoroutine(StartCountdown());
        }
    }
    //This function counts down in seconds and then triggers the animEnd trigger to end the current animation
    public IEnumerator StartCountdown(float countdownValue = 20)
    {
        currCountdownValue = countdownValue;
        Debug.Log("Timer at start of Coroutine is: " + currCountdownValue);
        while (currCountdownValue > 0)
        {
            yield return new WaitForSeconds(1.0F);
            currCountdownValue--;
        }
        Debug.Log("Timer when animation ends is: " + currCountdownValue);
        animator.SetTrigger("animEnd");
        source.PlayOneShot(collectAwaySound);
    }

    public void PlayerDeath ()
    {
        speed = 0.0F;
        animator.SetTrigger("playerDeath");
        Destroy(gameObject, 0.5F);  //Delays the object's destruction by a float
        alive = false;
        source.PlayOneShot(explosionSound);
        Time.timeScale = 0;
        FireModeDetector.fireModeAnimPlaying = false;
        FireSpeedDetector.fireSpeedAnimPlaying = false;
    }
}