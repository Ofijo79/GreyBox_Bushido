using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TenguHealth : MonoBehaviour
{
    public int hp;
    public int katanaDMG;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Katana")
        {
            hp -= katanaDMG;
        }

        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
