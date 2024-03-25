using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBoss : MonoBehaviour
{
    public Transform playerTransform;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _animator = GetComponent<Animator>();

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // Configura el destino del NavMeshAgent al transform del personaje principal
            navMeshAgent.SetDestination(playerTransform.position);

            // Calcula la dirección hacia el personaje principal
            Vector3 direccion = (playerTransform.position - transform.position).normalized;

            // Calcula la rotación deseada para mirar hacia el personaje principal
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);

            // Aplica la rotación al enemigo
            transform.rotation = rotacionDeseada;
            _animator.SetBool("Run", true);
        }
    }
}
