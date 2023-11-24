using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<EnemyData> enemies = new List<EnemyData>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    // Tambahkan variabel untuk melacak jumlah musuh dalam setiap gelombang
    public int baseWaveEnemyCount = 5; // Jumlah musuh dalam gelombang awal
    public int waveEnemyCountMultiplier = 2; // Pengganda jumlah musuh setiap gelombang

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            //spawn an enemy
            if (enemiesToSpawn.Count > 0)
            {
                //spawn an enemy
                if (enemiesToSpawn[0] != null)
                {
                    GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity);
                    enemiesToSpawn.RemoveAt(0);
                    spawnedEnemies.Add(enemy);
                    spawnTimer = spawnInterval;

                    if (spawnIndex + 1 <= spawnLocation.Length - 1)
                    {
                        spawnIndex++;
                    }
                    else
                    {
                        spawnIndex = 0;
                    }
                }
                else
                {
                    waveTimer = 0;
                }
            }
            else
            {
                waveTimer = 0; // if no enemies remain, end wave
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
        if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
        {
            currWave++;
            GenerateWave();
            CleanUpSpawnedEnemies();
            RemoveInactiveEnemies();
        }
    }

    public void CleanUpSpawnedEnemies()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null || !enemy.activeSelf);
    }


    public void CleanUpEmptyObjects()
    {
        foreach (var emptyObject in GameObject.FindGameObjectsWithTag("EmptyObject"))
        {
            Destroy(emptyObject);
        }
    }
    public void RemoveEnemy(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }

    public void RemoveInactiveEnemies()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null || !enemy.activeSelf);
    }



    public void GenerateWave()
    {
        waveValue = baseWaveEnemyCount * (int)Mathf.Pow(waveEnemyCountMultiplier, currWave);
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count;
        waveTimer = waveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        int maxIterations = 5;
        int iterationCount = 0;

        // Menghasilkan musuh berdasarkan waveValue
        while (waveValue > 0 && iterationCount < maxIterations)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }

            iterationCount++;
        }

        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}

[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab;
    public int cost;
}

#if UNITY_EDITOR
[CustomEditor(typeof(WaveSpawner))]
public class WaveSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Render the default inspector without the EnemiesToSpawn property
        DrawPropertiesExcluding(serializedObject, "enemiesToSpawn");

        // Render the EnemiesToSpawn property separately
        SerializedProperty enemiesToSpawn = serializedObject.FindProperty("enemiesToSpawn");
        EditorGUILayout.PropertyField(enemiesToSpawn, true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
