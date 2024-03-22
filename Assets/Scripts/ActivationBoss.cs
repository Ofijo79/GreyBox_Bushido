using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationBoss : MonoBehaviour
{
    public ScriptBoss enemyScriptObject;
    // Start is called before the first frame update
    void Start()
    {
        enemyScriptObject = GameObject.Find("Oni").GetComponent<ScriptBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            enemyScriptObject.enabled = true;
        }
    }
}
