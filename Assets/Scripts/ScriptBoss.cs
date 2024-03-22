using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBoss : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        StartChasingPlayer();
    }

    public void StartChasingPlayer()
    {
       navMeshAgent.destination = playerTransform.position;
    }
}
