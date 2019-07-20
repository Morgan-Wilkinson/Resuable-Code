using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobGenerator : MonoBehaviour
{
    public enum State
    {
        Idle,
        Initialize,
        Setup,
        SpawnMob
    }
    public GameObject[] mobPrefabs; //an array to hold all of the prefabs of mobs we want to spawn
    public GameObject[] spawnPoints; // this array will hold a reference to akk the spawn points in the scene

    public State state;     //this is our local variable that holds our current state
    public int mobsNumber = 1;
    public float countSpawnTimer;
    public float spawnTimer = 10;
    GameObject child;

    void Awake()
    {
        state = mobGenerator.State.Initialize;
        countSpawnTimer = spawnTimer;

    }


    // Use this for initialization
    IEnumerator Start()
    {
        while (true)
        {
            switch (state)
            {
                case State.Initialize:
                    Initialize();
                    break;
                case State.Setup:
                    Setup();
                    break;
                case State.SpawnMob:
                    SpawnMob();
                    break;
            }

            yield return 0;
        }

    }

    void Update()
    {
        countSpawnTimer -= Time.deltaTime;
        if (countSpawnTimer <= 0)
        {
            countSpawnTimer = 0;
            state = mobGenerator.State.Initialize;
        }
    }

    private void Initialize()
    {
        countSpawnTimer = spawnTimer;
        if (!CheckForMobPrefabs())
            return;
        if (!CheckForSpawnPoints())
            return;
        state = mobGenerator.State.Setup;
    }

    private void Setup()
    {
        state = mobGenerator.State.SpawnMob;
    }
    private void SpawnMob()
    {
        GameObject[] gos = AvailableSpawnPoints();

        for (int cnt = 0; cnt < mobsNumber; cnt++)
        {
            GameObject go = Instantiate(mobPrefabs[Random.Range(0, mobPrefabs.Length)],
                                        gos[Random.Range(0, gos.Length)].transform.position,
                                        Quaternion.identity
                                        ) as GameObject;
            go.transform.parent = gos[cnt].transform;
            Destroy(go, spawnTimer);
            Debug.Log("AvailableSpawnPoints");
        }
        state = mobGenerator.State.Idle;
    }

    //check to see that we have at least one mob prefab to spawn
    private bool CheckForMobPrefabs()
    {
        if (mobPrefabs.Length > 0)
            return true;
        else
            return false;
    }

    //check to see that we have at least one spawnpoint to spawn mobs
    private bool CheckForSpawnPoints()
    {
        if (spawnPoints.Length > 0)
            return true;
        else
            return false;
    }

    //generate a list of available spawn points that do not have any mobs child to it
    private GameObject[] AvailableSpawnPoints()
    {
        List<GameObject> gos = new List<GameObject>();

        for (int cnt = 0; cnt < spawnPoints.Length; cnt++)
        {
            if (spawnPoints[cnt].transform.childCount == 0)
            {
                gos.Add(spawnPoints[cnt]);
            }
        }
        return gos.ToArray();
    }

}