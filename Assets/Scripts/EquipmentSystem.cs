using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField]GameObject weapon;
    [SerializeField]GameObject dealer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDealDamage()
    {
        dealer.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        dealer.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}
