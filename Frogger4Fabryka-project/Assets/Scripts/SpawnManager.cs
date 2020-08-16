using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> objToSpawn;
    public float minSpawnDelay;
    public float maxSpawnDelay;

    private int index = 0;
    private bool firstSpawn = true;
    private int turtleCounter = 0;

    private GameObject instantiatedObj;
    private GameManager gameManager;

    // Apply level modifiers
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if(gameObject.name == "Vehicle Spawn Manager")
        {
            minSpawnDelay /= GameValues.vehicleSpawnDelayModifier;
            maxSpawnDelay /= GameValues.vehicleSpawnDelayModifier;
        }
        else if(gameObject.name == "Water Spawn Manager")
        {
            minSpawnDelay *= GameValues.platformSpawnDelayModifier;
            maxSpawnDelay *= GameValues.platformSpawnDelayModifier;
        }
        StartCoroutine(SpawnObj());
    }

    // Spawns objects with random delay
    IEnumerator SpawnObj()
    {
        while (true)
        {
            // Spawn only when game is active
            while (gameManager.isGameActive)
            {
                // Spawn from every spawn point
                foreach (Transform spawnPoint in transform)
                {
                    // If game starts spawn objects immediatly 
                    if (!firstSpawn)
                    {
                        yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
                    }
                    instantiatedObj = Instantiate(objToSpawn[index], spawnPoint.position, spawnPoint.rotation);

                    // Every fourth turtle platform (on current row) is submerging
                    if (instantiatedObj.CompareTag("Turtle Platform"))
                    {
                        turtleCounter++;
                        if (turtleCounter == 7 || turtleCounter == 8)
                        {
                            instantiatedObj.GetComponent<GoUnderWater>().enabled = true;
                        }
                        if (turtleCounter == 8)
                        {
                            turtleCounter = 0;
                        }
                    }

                    // objects spawn in the middle for the first time
                    if (firstSpawn)
                    {
                        if (spawnPoint.position.x < 0)
                        {
                            spawnPoint.position -= new Vector3(5, 0, 0);
                        }
                        else if (spawnPoint.position.x > 0)
                        {
                            spawnPoint.position += new Vector3(5, 0, 0);
                        }
                    }

                    // After spawning every object from the list start again from zero
                    index++;
                    if (index == objToSpawn.Count)
                    {
                        index = 0;
                        firstSpawn = false;
                    }
                }
            }
            yield return null;
            firstSpawn = true;
        }
    }
}
