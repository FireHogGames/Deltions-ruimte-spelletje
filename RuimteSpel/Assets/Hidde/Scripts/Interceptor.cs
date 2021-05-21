﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : MonoBehaviour
{
    public Transform target, rotateOrigin;

    public float forwardValue;
    public Vector3 trackOffset;

    public float hitRange, backwardEvadeRange, rightEvadeRange, forwardEvadeRange;

    public float speed;
    public float weaponDamage, fireRate;
    public float rotationSpeed;

    private bool canMove = true;
    private int evadePhase = 0;

    private Vector3 dir;
    Quaternion lookRotation;
    Vector3 rotation;

    private float nextTimeToFire = 0f;

    private void Update()
    {
        if(Vector3.Distance(transform.position, target.position) < hitRange && evadePhase == 0)
        {
            evadePhase = 1;
            if(Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }else if(Vector3.Distance(transform.position, target.position) > backwardEvadeRange && evadePhase == 1)
        {
            evadePhase = 2;
        }else if(Vector3.Distance(transform.position, target.position) > rightEvadeRange && evadePhase == 2)
        {
            evadePhase = 3;
        }
        else if (Vector3.Distance(transform.position, target.position) > forwardEvadeRange && evadePhase == 3)
        {
            evadePhase = 0;
        }



        switch (evadePhase)
        {
            default:
                dir = target.position - transform.position;
                lookRotation = Quaternion.LookRotation(dir);
                rotation = Quaternion.Lerp(rotateOrigin.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
                rotateOrigin.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

                Vector3 flyDirection = dir += trackOffset;

                transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
                break;
            case 1:
                dir = -target.forward * forwardValue;
                lookRotation = Quaternion.LookRotation(dir);
                rotation = Quaternion.Lerp(rotateOrigin.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
                rotateOrigin.rotation = Quaternion.Euler(rotation.x , rotation.y, rotation.z);

                transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
                break;
            case 2:
                dir = target.right * forwardValue;
                lookRotation = Quaternion.LookRotation(dir);
                rotation = Quaternion.Lerp(rotateOrigin.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
                rotateOrigin.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

                transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
                break;
            case 3:
                Vector3 forward = target.forward * forwardEvadeRange;
                dir = target.position + forward;
                lookRotation = Quaternion.LookRotation(dir);
                rotation = Quaternion.Lerp(rotateOrigin.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
                rotateOrigin.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

                transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
                break;
        }

        
    }

    public void SetMovementState(bool state)
    {
        canMove = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, backwardEvadeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rightEvadeRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, forwardEvadeRange);
    }
    private void Shoot()
    {
        Debug.Log("Shoot");
    }
}