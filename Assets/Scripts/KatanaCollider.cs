using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaCollider : MonoBehaviour
{
    public BoxCollider[] katanaCollider;
    public BoxCollider katanaColl;
    
    // Start is called before the first frame update
    void Start()
    {
        takeOutthecollider();
    }

    public void ActivateKatanaCollider()
    {
        for (int i = 0; i < katanaCollider.Length; i++)
        {
            if(katanaCollider[i] != null)
            {
                katanaCollider[i].enabled = true;
            }
            else
            {
                katanaColl.enabled = true;
            }
        }
    }

    public void takeOutthecollider()
    {
        for (int i = 0; i < katanaCollider.Length; i++)
        {
            if(katanaCollider[i] != null)
            {
                katanaCollider[i].enabled = false;
            }
        }
        katanaColl.enabled = false;
    }
}
