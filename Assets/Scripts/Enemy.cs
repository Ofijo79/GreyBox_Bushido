using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    [SerializeField]float Health = 50;
    float damageAmount = 10;

    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    { 
        if(other.gameObject.tag == "Katana")
        {     
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        Health -= damageAmount;
        ActualizeHealth();
        if(Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    void ActualizeHealth()
    {
        healthSlider.value = Health / maxHealth;
    }
}
