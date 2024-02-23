using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLejos : MonoBehaviour
{
    public GameObject flechaPrefab;
    public Transform firePoint;
    public float flechaSpeed = 20f;
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        GameObject flecha = Instantiate(flechaPrefab, firePoint.position, firePoint.rotation);
        //RigidBody rbody = flecha.GetComponent<Rigidbody>();
        //rbody.AddForce(firePoint.forward * flechaSpeed, ForceMode.Impulse);
    }
}
