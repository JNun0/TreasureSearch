using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class PlayerConditions : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private Vector3 _initialPosition;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _initialPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Agent"))
        {
            //Debug.Log("Colisão");

            ResetAgent();
        }
        else if (collision.gameObject.CompareTag("Treasure"))
        {

            ResetGame();
        }
    }

    private void ResetAgent()
    {
        transform.position = _initialPosition;
    }

    private void ResetGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}
