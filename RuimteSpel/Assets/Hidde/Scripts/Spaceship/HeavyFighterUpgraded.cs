﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyFighterUpgraded : MonoBehaviour
{
    public Transform[] spawnPoints;

    public Transform indicator;

    public float torque = 5f;
    public float thrust = 10f;
    public float rotationSpeed = 2f;
    private Rigidbody rb;
    public Transform target;

    public GameObject missle;

    public float hitRange;
    public LayerMask mask;

    public Vector3 offset;

    private bool canShoot;

    private float nextTimeToFire;

    private void Start()
    {
        target = GameObject.Find("PlayerShip").transform;

        rb = GetComponent<Rigidbody>();
        canShoot = true;
    }

    private void Update()
    {
        if (indicator != null)
        {
            indicator.LookAt(target);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, hitRange, mask))
        {
            ShootMissle();
        }

        Vector3 lookLocation = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(lookLocation);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.fixedDeltaTime * rotationSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    private void FixedUpdate()
    {
        Vector3 targetLocation = target.position - transform.position + offset;


        float distance = targetLocation.magnitude;

        Vector3 translation = Vector3.forward * Mathf.Clamp((distance - 10f) / 50f, 0f, 1f) * thrust;

        rb.AddRelativeForce(translation * Time.fixedDeltaTime);
    }   

    private void ShootMissle()
    {
        if (canShoot)
        {
            canShoot = false;
            int missleIndex = Random.Range(0, spawnPoints.Length);

            Instantiate(missle, spawnPoints[missleIndex].position, Quaternion.LookRotation(transform.forward));

            StartCoroutine(SetShootState());
        }
    }

    private IEnumerator SetShootState()
    {
        yield return new WaitForSeconds(5f);

        canShoot = true;
    }
}
