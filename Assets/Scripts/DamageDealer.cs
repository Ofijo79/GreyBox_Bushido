using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();

        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canDealDamage)
        {
            RaycastHit hit;

            int layerMask = 6;

            if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if(hit.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    enemy.TakeDamage(weaponDamage);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        enemy.TakeDamage(weaponDamage);
        hasDealtDamage.Clear();
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
