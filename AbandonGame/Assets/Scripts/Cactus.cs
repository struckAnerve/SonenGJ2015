﻿using UnityEngine;
using System.Collections;

public class Cactus : SpawnableSuperclass
{
    Rigidbody rb;
    float MaxWindForce = 7f;

    public GameObject cactusBase;
    public GameObject cactusTop;
    public GameObject cactusWhole;
    // Use this for initialization
    new void Start()
    {
        base.Start();
        rb = thisSpawnableObj.GetComponentInChildren<Rigidbody>();
        rb.centerOfMass = Vector3.down * 2.3f;

        rb.constraints = RigidbodyConstraints.FreezePosition;
    }
    new void Update()
    {
        base.Update();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddTorque(Vector3.back * Random.Range(5, MaxWindForce));
    }
    void OnCollisionEnter(Collision col)
    {
        CarController cc = col.gameObject.GetComponent<CarController>();

        if (cc != null) //CRASH!
        {

            Vector3 colVel = col.rigidbody.velocity;
            Vector3 dampForce = colVel.normalized * -500000;
            float colSpeed = colVel.magnitude;
            if (colSpeed < 10) // not fast enough!
            {
                rb.constraints = RigidbodyConstraints.FreezePosition;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.AddExplosionForce(colSpeed * 25, col.transform.position + Vector3.up * 4, 10, 0.3f);

                rb.centerOfMass = Vector3.zero;
                col.rigidbody.AddForce(dampForce);
                cactusBase.SetActive(true);
                cactusTop.SetActive(true);
                cactusWhole.SetActive(false);

                //rb.AddTorque(Vector3.forward * 2);
            }
        }
    }
    void OnCollisionExit(Collision col)
    {

        rb.constraints = RigidbodyConstraints.None;

    }
}