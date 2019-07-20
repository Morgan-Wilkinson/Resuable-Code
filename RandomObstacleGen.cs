using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomObstacleGen : MonoBehaviour
{
    public GameObject[] spawnees;
    public Transform spawnPosition;

    int randomInt;
	
	// Update is called once per frame
	void Update ()
    {
        if (FindObjectOfType<GameManager>().gameHasEnded != true)
        {
            SpawnRandom();
        }
        
    }

    void SpawnRandom ()
    {
        randomInt = Random.Range(0, spawnees.Length);
        Instantiate(spawnees[randomInt], spawnPosition.position, spawnPosition.rotation);
    }

}
