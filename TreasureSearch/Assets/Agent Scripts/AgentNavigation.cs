using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Vector3 _desiredDestination;
    private Vector3 _randomPosition;
    private MazeGenerator _mazeGenerator;
    public string playerTag = "Player";
    public float followDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _mazeGenerator = FindObjectOfType<MazeGenerator>();
        _randomPosition = GetRandomPosition();
        _desiredDestination = _randomPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            RespawnAgent();
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);

        if (playerObject != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

            if (distanceToPlayer < followDistance)
            {
                _navMeshAgent.destination = playerObject.transform.position;
            }
            else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                //Altera o destino apenas quando o agente atinge o destino
                _desiredDestination = GetRandomPosition();
                _navMeshAgent.destination = _desiredDestination;
            }
        }
    }

    private void RespawnAgent()
    {
        _randomPosition = GetRandomPosition();
        transform.position = _randomPosition;
    }

    private Vector3 GetRandomPosition()
    {
        int xIndex = Random.Range(0, _mazeGenerator._mazeWidth);
        int zIndex = Random.Range(0, _mazeGenerator._mazeDepth);

        Vector3 randomPosition = _mazeGenerator._mazeGrid[xIndex, zIndex].transform.position;

        return randomPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RespawnAgent();
        }
    }
}
