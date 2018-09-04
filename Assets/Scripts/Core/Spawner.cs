/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for spawning gameobjects, and managing them in a collection pool.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    //Creates the gameobjects/pool on start
    [SerializeField, Header("Settings:")]
    private bool createOnStart = true;
    public bool CreateOnStart
    {
        get { return createOnStart; }
        set { createOnStart = value; }
    }
    
    //Spawn point used
    [SerializeField]
    private List<Transform> spawnPoints;
    public List<Transform> SpawnPoints
    {
        get { return spawnPoints; }
        set { spawnPoints = value; }
    }

    //Prefab/gameobject used for spawning
    [SerializeField, Header("Prefab Setup:")]
    private GameObject prefab;
    public GameObject Prefab
    {
        get { return prefab; }
        set { prefab = value; }
    }

    //Max amount of gameobjects
    [SerializeField]
    private int maxCount;
    public int MaxCount
    {
        get { return maxCount; }
        set { maxCount = value; }
    }

    //Used for ignoring colliders
    [SerializeField]
    private List<Collider> ignoreColliders;
    public List<Collider> IgnoreColliders
    {
        get { return ignoreColliders; }
        set { ignoreColliders = value; }
    }

    //List of all created objects used for spawning
    [SerializeField, Header("Spawned Prefabs:")]
    private List<GameObject> spawnPool;
    public List<GameObject> SpawnPool
    {
        get { return spawnPool; }
        set { spawnPool = value; }
    }

    //Last spawned gameobject
    [SerializeField]
    private GameObject lastSpawned;
    public GameObject LastSpawned
    {
        get { return lastSpawned; }
        set { lastSpawned = value; }
    }

    //Current index
    private int spawnPoolIndex = 0;

    //Used for seeing the spawn points within the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (var item in SpawnPoints)
        {
            Gizmos.DrawWireSphere(item.position, .25f);
        }
    }

    #region Unity Functions

    // Use this for initialization
    void Start()
    {
        if (CreateOnStart)
        {
            SetupSpawnPool();
        }
    }

    #endregion

    #region Functions

    //Sets up the spawn pool by creating the gameobjects
    public void SetupSpawnPool()
    {
        if (Prefab != null)
        {
            if (SpawnPool != null)
            {
                for (int i = 0; i < SpawnPool.Count; i++)
                {
                    Destroy(SpawnPool[i]);
                }
            }

            SpawnPool = new List<GameObject>();

            for (int i = 0; i < maxCount; i++)
            {
                GameObject spawnedObject = Instantiate(Prefab) as GameObject;

                spawnedObject.transform.SetParent(this.transform, false);
                spawnedObject.SetActive(false);
                spawnedObject.name = string.Format("{0}: {1}", Prefab.name, i.ToString());
                
                SpawnPool.Add(spawnedObject);
            }
        }
    }

    //Resets all pooled gameobjects
    public void ResetAllSpawnedObjects()
    {
        spawnPoolIndex = 0;

        foreach (var item in SpawnPool)
        {
            item.SetActive(false);
        }

        LastSpawned = null;
    }

    //Spawns a gameobject
    public void Spawn()
    {
        if (spawnPoolIndex >= SpawnPool.Count)
            spawnPoolIndex = 0;

        var spawnPoint = this.transform;

        spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count-1)];

        SpawnPool[spawnPoolIndex].SetActive(true);
        SpawnPool[spawnPoolIndex].gameObject.transform.position = spawnPoint.transform.position;
        SpawnPool[spawnPoolIndex].gameObject.transform.rotation = spawnPoint.transform.rotation;

        if (SpawnPool[spawnPoolIndex].GetComponent<Rigidbody>() != null)
        {
            SpawnPool[spawnPoolIndex].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            SpawnPool[spawnPoolIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        LastSpawned = SpawnPool[spawnPoolIndex];

        IgnoreCollidersOnLastSpawn();

        spawnPoolIndex++;
    }

    //Spawns a gameobject at a point
    public void SpawnAtPoint(Vector3 point)
    {
        if (spawnPoolIndex >= SpawnPool.Count)
            spawnPoolIndex = 0;

        SpawnPool[spawnPoolIndex].SetActive(true);
        SpawnPool[spawnPoolIndex].gameObject.transform.position = point;

        if (SpawnPool[spawnPoolIndex].GetComponent<Rigidbody>() != null)
        {
            SpawnPool[spawnPoolIndex].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            SpawnPool[spawnPoolIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        LastSpawned = SpawnPool[spawnPoolIndex];

        IgnoreCollidersOnLastSpawn();

        spawnPoolIndex++;
    }

    //Used for ignoring collision
    public void IgnoreCollidersOnLastSpawn()
    {
        if (LastSpawned.GetComponent<Collider>() != null)
        {
            foreach (var collider in IgnoreColliders)
            {
                Physics.IgnoreCollision(collider, LastSpawned.GetComponent<Collider>(), true);
            }
        }
    }

    #endregion
}
