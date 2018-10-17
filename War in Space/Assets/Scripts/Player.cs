using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float moveSpeed = 10f;// sets speed for player movement
    [SerializeField] float padding = 1f;// sets padding for player movement
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;// sets speed for laser
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine;// allows continuous firing

    float xMin;
    float xMax;
    float yMin;// variables for position of player
    float yMax;

    // Use this for initialization
    void Start () {
        SetUpMoveBoundaries();
       
    }// Start ()

    // Update is called once per frame
    void Update () {
        Move();
        Fire();

    }// Update ()

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());       
        }// if
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine (firingCoroutine);
        }
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

        var newXpos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXpos, newYpos);

    }// Move()
}