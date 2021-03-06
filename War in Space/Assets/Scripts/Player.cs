﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    float moveSpeed = 50f;// sets speed for player movement
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
    public Joystick joystick;
    float xMin;
    float xMax;
    float yMin;// variables for position of player
    float yMax;
    
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
        FindObjectOfType<Level>().LoadGameOver();// loads game over 
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
        // keyboard controls
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        // controls for virtual joystick
        deltaX = joystick.Horizontal * Time.deltaTime * moveSpeed;
        deltaY = joystick.Vertical * Time.deltaTime *  moveSpeed;
        // 
        var newXpos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXpos, newYpos);


      


    }// Move()
}