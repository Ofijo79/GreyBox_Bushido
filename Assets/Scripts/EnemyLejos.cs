using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLejos : MonoBehaviour
{
    [SerializeField] Transform gunPosition;

    [SerializeField] int bulletType = 0;

    public GameObject flechaPrefab;
    public float flechaForce = 20f;
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(flechaPrefab, gunPosition.position, gunPosition.rotation);
    }
}
