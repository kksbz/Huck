using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
=======
using UnityEngine.AI;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

#if UNITY_EDITOR
using UnityEditor;
#endif  // UNITY_EDITOR

[System.Serializable]
public class PlaceableObjectConfig
{
    public bool hasHeightLimits = false;
    public float minHeightToSpawn = 0f;
    public float maxHeightToSpawn = 0f;
    public bool canGoInWater = false;
    public bool canGoAboveWater = true;
<<<<<<< HEAD
    [Range(0f, 1f)] public float weighting;
    public List<GameObject> prefabs;

    public float NormalisedWeighting { get; set;} = 0f;
=======
    public bool isEmbedded = false;
    public bool isNavAgent = false;
    [Range(0f, 1f)] public float weighting;
    public List<GameObject> prefabs;

    public float NormalisedWeighting { get; set; } = 0f;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
}

public class BaseObjectPlacer : MonoBehaviour
{
<<<<<<< HEAD
    
=======

>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    [SerializeField] protected List<PlaceableObjectConfig> objects;
    [SerializeField] protected float targetDensity = 0.1f;
    [SerializeField] protected int maxSpawnCount = 1000;
    [SerializeField] protected int maxInvalidLocationSkips = 10;
    [SerializeField] protected float maxPositionJitter = 0.1f;

    protected List<Vector3> GetAllLocationForBiome(ProcGenConfigSO globalConfig, int mapResolution, float[,] heightMap, Vector3 heightmapScale, byte[,] biomeMap, int biomeIndex)
    {
        List<Vector3> locations = new List<Vector3>(mapResolution * mapResolution / 10);

<<<<<<< HEAD
        for(int y = 0; y < mapResolution; y++)
        {
            for(int x = 0; x < mapResolution; x++)
=======
        for (int y = 0; y < mapResolution; y++)
        {
            for (int x = 0; x < mapResolution; x++)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            {
                if (biomeMap[x, y] != biomeIndex)
                    continue;

<<<<<<< HEAD
                float height = heightMap[x,y] * heightmapScale.y;

                

                locations.Add(new Vector3(y * heightmapScale.z, heightMap[x, y] * heightmapScale.y, x * heightmapScale.x));
=======
                float height = heightMap[x, y] * heightmapScale.y;

                Vector3 newLocs = new Vector3(y * heightmapScale.z, heightMap[x, y] * heightmapScale.y, x * heightmapScale.x);
                locations.Add(newLocs);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            }
        }

        return locations;
    }
<<<<<<< HEAD
    public virtual void Execute(ProcGenConfigSO globalConfig,Transform objectRoot, int mapResolution, float[,] heightMap, Vector3 heightmapScale, float[,] slopeMap, float[,,] alphaMaps, int alphaMapResolution, byte[,] biomeMap = null, int biomeIndex = -1, BiomeConfigSO biome = null)
    {
        // validate the configs
        foreach(var config in objects)
        {
            if(!config.canGoInWater && !config.canGoAboveWater)
=======
    public virtual void Execute(ProcGenConfigSO globalConfig, Transform objectRoot, int mapResolution, float[,] heightMap, Vector3 heightmapScale, float[,] slopeMap, float[,,] alphaMaps, int alphaMapResolution, byte[,] biomeMap = null, int biomeIndex = -1, BiomeConfigSO biome = null)
    {
        // validate the configs
        foreach (var config in objects)
        {
            if (!config.canGoInWater && !config.canGoAboveWater)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
                throw new System.InvalidOperationException($"Object placer forbids both in and out of water. Cannot run!");
        }
        // normalize the weighting
        float weightSum = 0f;
<<<<<<< HEAD
        foreach(var config in objects)
        {
            weightSum += config.weighting;
        }
        foreach(var config in objects)
=======
        foreach (var config in objects)
        {
            weightSum += config.weighting;
        }
        foreach (var config in objects)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        {
            config.NormalisedWeighting = config.weighting / weightSum;
        }
    }

    protected virtual void ExecuteSimpleSpawning(ProcGenConfigSO globalConfig, Transform objectRoot, List<Vector3> candidateLocations)
    {
<<<<<<< HEAD
        foreach(var spawnConfig in objects)
=======
        foreach (var spawnConfig in objects)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        {
            // pick a random prefab
            var prefab = spawnConfig.prefabs[Random.Range(0, spawnConfig.prefabs.Count)];

            // determine the spawn count
<<<<<<< HEAD
            float baseSpawnCount =  Mathf.Min(maxSpawnCount, candidateLocations.Count * targetDensity);
=======
            float baseSpawnCount = Mathf.Min(maxSpawnCount, candidateLocations.Count * targetDensity);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            int numToSpawn = Mathf.FloorToInt(spawnConfig.NormalisedWeighting * baseSpawnCount);

            int skipCount = 0;
            int numPlaced = 0;
<<<<<<< HEAD
            for(int index = 0; index < maxSpawnCount; index++)
=======
            for (int index = 0; index < numToSpawn; index++)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            {
                // pick a random location to spawn at
                int randomLocationIndex = Random.Range(0, candidateLocations.Count);
                Vector3 spawnLocation = candidateLocations[randomLocationIndex];

<<<<<<< HEAD
                // height is invalid?
                bool isValid = true;
                if(spawnLocation.y < globalConfig.waterHeight && !spawnConfig.canGoInWater)
                        isValid = false;
                if(spawnLocation.y >= globalConfig.waterHeight && !spawnConfig.canGoAboveWater)
                        isValid = false;

                // skip if outside of height limits
                if(spawnConfig.hasHeightLimits && (spawnLocation.y < spawnConfig.minHeightToSpawn || spawnLocation.y >= spawnConfig.maxHeightToSpawn))
                    isValid = false;

                // location is not valid?
                if(!isValid)
=======
                //
                if (spawnConfig.isNavAgent)
                {
                    NavMeshHit hit = default;
                    Debug.Log("물소 스폰 위치 찾는 중!");
                    if (NavMesh.SamplePosition(spawnLocation, out hit, 50f, NavMesh.AllAreas) == true)
                    {
                        spawnLocation = hit.position;
                        Debug.Log($"물소 스폰 위치 : {spawnLocation}");
                    }
                }
                //

                // height is invalid?
                bool isValid = true;
                if (spawnLocation.y < globalConfig.waterHeight && !spawnConfig.canGoInWater)
                    isValid = false;
                if (spawnLocation.y >= globalConfig.waterHeight && !spawnConfig.canGoAboveWater)
                    isValid = false;

                // skip if outside of height limits
                if (spawnConfig.hasHeightLimits && (spawnLocation.y < spawnConfig.minHeightToSpawn || spawnLocation.y >= spawnConfig.maxHeightToSpawn))
                    isValid = false;

                // location is not valid?
                if (!isValid)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
                {
                    skipCount++;
                    --index;

<<<<<<< HEAD
                    if(skipCount >= maxInvalidLocationSkips)
=======
                    if (skipCount >= maxInvalidLocationSkips)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
                        break;

                    continue;
                }
                skipCount = 0;
                numPlaced++;
                // remove the location if chosen
                candidateLocations.RemoveAt(randomLocationIndex);

<<<<<<< HEAD
                SpawnObject(prefab, spawnLocation, objectRoot);
            }
            Debug.Log($"Placed : {numPlaced} objects out of {numToSpawn}");
=======
                // ���� ������ ������Ʈ�� �������� Y��ǥ ����
                if (spawnConfig.isEmbedded)
                    spawnLocation -= new Vector3(0, Mathf.Clamp(Random.Range(-1f, 1f), 0f, 1f), 0);

                SpawnObject(prefab, spawnLocation, objectRoot);
            }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        }
    }

    protected virtual void SpawnObject(GameObject prefab, Vector3 spawnLocation, Transform objectRoot)
    {
        Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
<<<<<<< HEAD
        Vector3 positionOffset = new Vector3(Random.Range(  -maxPositionJitter, maxPositionJitter),
                                                            0,
                                                            Random.Range(-maxPositionJitter, maxPositionJitter));

        // instantiate the prefab
#if UNITY_EDITOR
        if (Application.isPlaying)
            Instantiate(prefab, spawnLocation + positionOffset, spawnRotation, objectRoot);
        else
        {
            var spawnedGO = PrefabUtility.InstantiatePrefab(prefab, objectRoot) as GameObject;
=======
        Vector3 positionOffset = new Vector3(Random.Range(-maxPositionJitter, maxPositionJitter),
                                                            0,
                                                            Random.Range(-maxPositionJitter, maxPositionJitter));
        // instantiate the prefab
        GameObject spawnedGO = default;
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            spawnedGO = Instantiate(prefab, spawnLocation + positionOffset, spawnRotation, objectRoot);
        }
        else
        {
            spawnedGO = PrefabUtility.InstantiatePrefab(prefab, objectRoot) as GameObject;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            spawnedGO.transform.position = spawnLocation + positionOffset;
            spawnedGO.transform.rotation = spawnRotation;
            Undo.RegisterCreatedObjectUndo(spawnedGO, "Placed object");
        }
#else
<<<<<<< HEAD
        Instantiate(prefab, spawnLocation + positionOffset, spawnRotation, objectRoot);
#endif // UNITY_EDITOR
=======
        spawnedGO = Instantiate(prefab, spawnLocation + positionOffset, spawnRotation, objectRoot);
        
#endif // UNITY_EDITOR
        // ���� ���� ó�� ����
        spawnedGO.tag = prefab.tag;
        if (spawnedGO.GetComponent<Item>() != null)
        {
            Rigidbody spawnedRigid = spawnedGO.GetComponent<Rigidbody>();
            spawnedRigid.useGravity = false;
            spawnedRigid.constraints = RigidbodyConstraints.FreezeAll;
        }
        if (spawnedGO.tag == GData.ANIMAL_MASK)
        {
            spawnedGO.transform.SetParent(null);
        }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    }
}
