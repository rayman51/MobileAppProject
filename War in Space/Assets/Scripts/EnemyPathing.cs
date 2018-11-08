﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    WaveConfig waveConfig;
    List<Transform> waypoints;


    int waypointIndex = 0;
    // Use this for initialization
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }// Start

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;

    }// SetWaveConfig

    // Update is called once per frame
    void Update()
    {
        Move();
    }//Update

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.getMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                (transform.position,
                targetPosition,
                movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }// if
        }// if
        else
        {
            Destroy(gameObject);

        }// else
    }// Move()
}
