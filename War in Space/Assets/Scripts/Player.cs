using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    float moveSpeed = 10f;// sets speed for player movement
    [SerializeField]
    float padding = 1f;// sets padding for player movement
    [SerializeField]
    int health = 200;

    [Header("Projectile")]
    [SerializeField]
    GameObject laserPrefab;
    [SerializeField]
    float projectileSpeed = 10f;// sets speed for laser
    [SerializeField]
    float projectileFiringPeriod = 0.1f;
    [SerializeField]
    [Range(0, 1)]
    float deathSoundVolume = 0.7f;
    [SerializeField]
    AudioClip deathSound;
    [SerializeField]
    [Range(0, 1)]
    float shootSoundVolume = 0.25f;
    [SerializeField]
    AudioClip shootSound;

    Coroutine firingCoroutine;// allows continuous firing

    float xMin;
    float xMax;
    float yMin;// variables for position of player
    float yMax;
    private Vector2 touchOrigin = -Vector2.one;
    // Use this for initialization
    void Start()
    {
        SetUpMoveBoundaries();

    }// Start ()

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();

    }// Update ()

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }// OnTriggerEnter2D

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();// if health hits zero call Die method
        }// if
    }// ProcessHit

    private void Die()
    {
        FindObjectOfType <Level>().LoadGameOver();// loads game over 
        Destroy(gameObject);// destroys object 
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);// plays explosion sound
    }

    public int GetHealth()
    {
        return health;
    }
    private void Fire()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }// if
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }// if
    }// Fire()

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                  laserPrefab,
                  transform.position,
                  Quaternion.identity) as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }// while

    }// FireContinuously()

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        // stops player from leaving canvas
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }// SetUpMoveBoundaries()

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        // 
        var newXpos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXpos, newYpos);
        //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
        //elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

        //Check if Input has registered more than zero touches
        if (Input.touchCount > 0)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

            //Check if the phase of that touch equals Began
            if (myTouch.phase == TouchPhase.Began)
            {
                //If so, set touchOrigin to the position of that touch
                touchOrigin = myTouch.position;
            }

            //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                //Set touchEnd to equal the position of this touch
                Vector2 touchEnd = myTouch.position;

                //Calculate the difference between the beginning and end of the touch on the x axis.
                float x = touchEnd.x - touchOrigin.x;

                //Calculate the difference between the beginning and end of the touch on the y axis.
                float y = touchEnd.y - touchOrigin.y;

                //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
                touchOrigin.x = -1;

                //Check if the difference along the x axis is greater than the difference along the y axis.
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    //If x is greater than zero, set deltaX to 1, otherwise set it to -1
                    deltaX = x > 0 ? 1 : -1;
                else
                    //If y is greater than zero, set deltaY to 1, otherwise set it to -1
                    deltaY = y > 0 ? 1 : -1;
            }
        }

      
      
    
}// Move()
}