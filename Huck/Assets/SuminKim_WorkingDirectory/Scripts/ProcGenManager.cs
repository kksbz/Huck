using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
<<<<<<< HEAD
=======
using UnityEngine.AI;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
using System.Linq;
using UnityEngine.XR;
using Unity.VisualScripting.FullSerializer;


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif  // UNITY_EDITOR

<<<<<<< HEAD
public enum EGenerationStage
{
    Beginning = 1,

    BuildTextureMap,
    BuildDetailMap,
    BuildLowResolutionBiomeMap,
    BuildHighResolutionBiomeMap,
    HeightMapGeneration,
    TerrainPainting,
    ObjectPlacement,
    DetailPainting,

    Complete,
    NumStage = Complete
}

=======
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
public class ProcGenManager : MonoBehaviour
{
    [SerializeField] ProcGenConfigSO config;
    [SerializeField] Terrain targetTerrain;
<<<<<<< HEAD
    [Header("Debugging")]
    [SerializeField] bool DEBUG_TurnOffObjectPlacers = false;
    Dictionary<TextureConfig, int> BiomeTextureToTerrainLayerIndex = new Dictionary<TextureConfig, int>();
    Dictionary<TerrainDetailConfig, int> BiomeTerrainDetailToDetailLayerIndex = new Dictionary<TerrainDetailConfig, int>();
    byte[,] BiomeMap_LowResolution;
    float[,] BiomeStrengths_LowResolution;

    byte[,] BiomeMap;
    float[,] BiomeStrengths;

    float[,] SlopeMap;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator AsyncRegenerateWorld(System.Action<EGenerationStage, string> reportStatusFn = null)
    {
        // cache the map resolution
=======
    // 디버깅 멤버
    [Header("Debugging")]
    [SerializeField] bool DEBUG_TurnOffObjectPlacers = false;
    [SerializeField] bool DEBUG_TurnOffBakeNavMeshes = false;

    // 네비게이션
    [SerializeField] NavMeshSurface[] navMeshSurfaces = default;
    Dictionary<TextureConfig, int> BiomeTextureToTerrainLayerIndex = new Dictionary<TextureConfig, int>();
    Dictionary<TerrainDetailConfig, int> BiomeTerrainDetailToDetailLayerIndex = new Dictionary<TerrainDetailConfig, int>();

    // 바이옴 맵
    byte[,] BiomeMap;
    // 바이옴 강세
    float[,] BiomeStrengths;
    // 기울기 맵
    float[,] SlopeMap;

    public Terrain MainTerrain
    {
        get { return targetTerrain; }
    }

    void Awake()
    {
        GameManager.Instance.terrain = targetTerrain;
    }
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if(Application.isPlaying)
            StartCoroutine(AsyncRegenerateWorld(LoadingManager.Instance.OnStatusReported));
        else
        {
            /* To do? */
        }
            
#else
        StartCoroutine(AsyncRegenerateWorld(LoadingManager.Instance.OnStatusReported));
#endif
    }

    // 월드 비동기 재 생성 함수
    public IEnumerator AsyncRegenerateWorld(System.Action<EGenerationStage, string> reportStatusFn = null)
    {
        // 맵 해상도 캐싱
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        int mapResolution = targetTerrain.terrainData.heightmapResolution;
        int alphaMapResolution = targetTerrain.terrainData.alphamapResolution;
        int detailMapResolution = targetTerrain.terrainData.detailResolution;
        int maxDetailsPerPatch = targetTerrain.terrainData.detailResolutionPerPatch;

<<<<<<< HEAD
        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.Beginning, "Beginning Generation");
        yield return new WaitForSeconds(1f);


        // clear out any previously spawn objects
=======
        // { 지형 생성 프로세스 시작
        // 지형 생성 프로세스 중 어떤 스텝인지를 UI에 표시하는 함수를 매개변수로 받고
        // 매 단계마다 실행하여 현재 어떤 스텝인인지 알림
        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.Beginning, "Beginning Terrain Generation");
        yield return new WaitForSeconds(1f);

        // 이전에 스폰했었던 오브젝트 클리어
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        for (int childIndex = transform.childCount - 1; childIndex >= 0; --childIndex)
        {
#if UNITY_EDITOR
            if(Application.isPlaying)
                Destroy(transform.GetChild(childIndex).gameObject);
            else
                Undo.DestroyObjectImmediate(transform.GetChild(childIndex).gameObject);
#else
            Destroy(transform.GetChild(childIndex).gameObject);
#endif  // UNITY_EDITOR
        }

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.BuildTextureMap, "Building texture map");
        yield return new WaitForSeconds(1f);

<<<<<<< HEAD
        // Generate the texture mapping
=======
        // 텍스처 맵 생성
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        Perform_GenerateTextureMapping();

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.BuildDetailMap, "Building detail map");
        yield return new WaitForSeconds(1f);

<<<<<<< HEAD
        // Generate the texture mapping
        Perform_GenerateTerrainDetailMapping();

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.BuildLowResolutionBiomeMap, "Build low res biome map");
        yield return new WaitForSeconds(1f);

        // Generate the low resolution biome map 
        Perform_BiomeGeneration_LowResolution((int)config.biomeMapResolution);

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.BuildHighResolutionBiomeMap, "Build high res biome map");
        yield return new WaitForSeconds(1f);

        // Generate the high resolution biome map
        Perform_BiomeGeneration_HighResolution((int)config.biomeMapResolution, mapResolution);

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.HeightMapGeneration, "Modifying heights");
        yield return new WaitForSeconds(1f);

        // update the terrain heights
        Perform_HeightMapModification(mapResolution, alphaMapResolution);

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.TerrainPainting, "Painting the terrain");
        yield return new WaitForSeconds(1f);

        // paint the terrain
        Perform_TerrainPainting(mapResolution, alphaMapResolution);

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.ObjectPlacement, "Placing objects");
        yield return new WaitForSeconds(1f);

        // place the object
=======
        // 디테일 페인트 맵 생성
        Perform_GenerateTerrainDetailMapping();

        // 바이옴 생성 및 보스 성 오브젝트 생성 검증 루프
        while(true)
        {
            if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.BuildBiomeMap, "Build biome map");
            yield return new WaitForSeconds(1f);

            // 바이옴 맵 생성
            Perform_BiomeGeneration(mapResolution);

            if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.HeightMapGeneration, "Modifying heights");
            yield return new WaitForSeconds(1f);

            // 지형 높이맵 설정
            Perform_HeightMapModification(mapResolution, alphaMapResolution);

            // Height Modifier로 높이맵과 보스 성을 만드는데 여기서 보스성을 찾을 수 있다면 루프 탈출
            if(IsValidHeightModifying())
                break;
            else
            {
                if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.HeightMapGeneration, "Validation Failed Return Build biome map");
                yield return new WaitForSeconds(1f);
            }
        }

        // 월드맵 텍스처 생성 및 UI에 설정
        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.SetupWorldMap, "Setup world map"); 
                yield return new WaitForSeconds(1f);
        Perform_SetupWorldMap(config, mapResolution);

        // 지형을 텍스처 레이어로 색칠
        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.TerrainPainting, "Painting the terrain");
        yield return new WaitForSeconds(1f);

        // 지형 칠하기 단계
        Perform_TerrainPainting(mapResolution, alphaMapResolution);

        if (reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.NavMeshBaking, "NavMesh Baking");
        yield return new WaitForSeconds(1f);

        // 네비메쉬 굽기 단계
        Perform_NavMeshBaking();

        if (reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.ObjectPlacement, "Placing objects");
        yield return new WaitForSeconds(1f);

        // 오브젝트 배치 단계
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        Perform_ObjectPlacement(mapResolution, alphaMapResolution);

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.DetailPainting, "Detail Painting");
        yield return new WaitForSeconds(1f);

<<<<<<< HEAD
        // paint the details
        Perform_DetailPainting(mapResolution, alphaMapResolution, detailMapResolution, maxDetailsPerPatch);

        if(reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.Complete, "Generation complete");
=======
        // 디테일(잔디, 풀 등) 칠하기
        Perform_DetailPainting(mapResolution, alphaMapResolution, detailMapResolution, maxDetailsPerPatch);

        if (reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.PostProcessOnLoading, "PostProcessOnLoading");
        yield return new WaitForSeconds(1f);

        // 로딩 후처리 작업 단계
        Perform_PostProcessOnLoading();

        if (reportStatusFn != null) reportStatusFn.Invoke(EGenerationStage.Complete, "Terrain Generation complete");
        yield return new WaitForSeconds(1f);

>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    }

    void Perform_GenerateTextureMapping()
    {
        BiomeTextureToTerrainLayerIndex.Clear();

<<<<<<< HEAD
        // build up list of all textures
=======
        // 모든 바이옴의 텍스처 페인터로부터 텍스처 리스트 가져오기
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        List<TextureConfig> allTextures = new List<TextureConfig>();
        foreach (var biomeMetadata in config.Biomes)
        {
            List<TextureConfig> biomeTextures = biomeMetadata.Biome.RetrieveTextures();

            if (biomeTextures == null || biomeTextures.Count == 0)
                continue;

            allTextures.AddRange(biomeTextures);
        }

<<<<<<< HEAD
        if (config.PaintingPostProcessingModifier != null)
        {
            // extract all textures from every painter
=======
        // 후처리 텍스처 페인터로부터 텍스터 리스트 가져오기
        if (config.PaintingPostProcessingModifier != null)
        {
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            BaseTexturePainter[] allPainters = config.PaintingPostProcessingModifier.GetComponents<BaseTexturePainter>();
            foreach (var painter in allPainters)
            {
                var painterTextures = painter.RetrieveTextures();

                if (painterTextures == null || painterTextures.Count == 0)
                    continue;

                allTextures.AddRange(painterTextures);
            }
        }

<<<<<<< HEAD
        // filter out any duplicate entries
        allTextures = allTextures.Distinct().ToList();

        // iterate over the texture configs
=======
        // 중복 제거
        allTextures = allTextures.Distinct().ToList();

        // 가져온 텍스처들을 dictionary에 index와 같이 설정
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        int layerIndex = 0;
        foreach(var textureConfig in allTextures)
        {
            BiomeTextureToTerrainLayerIndex[textureConfig] = layerIndex;
            layerIndex++;
        }
    }

    void Perform_GenerateTerrainDetailMapping()
    {
        BiomeTerrainDetailToDetailLayerIndex.Clear();

<<<<<<< HEAD
        // build up list of all terrain details
=======
        // 모든 바이옴의 디테일 페인터로부터 디테일 리스트 가져오기
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        List<TerrainDetailConfig> allTerrainDetails = new List<TerrainDetailConfig>();
        foreach (var biomeMetadata in config.Biomes)
        {
            List<TerrainDetailConfig> biomeTerrainDetails = biomeMetadata.Biome.RetrieveTerrainDetails();

            if (biomeTerrainDetails == null || biomeTerrainDetails.Count == 0)
                continue;

            allTerrainDetails.AddRange(biomeTerrainDetails);
        }

<<<<<<< HEAD
        if (config.DetailPaintingPostProcessingModifier != null)
        {
            // extract all terrain details from every painter
=======
        // 후처리 디테일 페인터로부터 디테일 리스트 가져오기
        if (config.DetailPaintingPostProcessingModifier != null)
        {
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            BaseDetailPainter[] allPainters = config.DetailPaintingPostProcessingModifier.GetComponents<BaseDetailPainter>();
            foreach (var painter in allPainters)
            {
                var terrainDetails = painter.RetrieveTerrainDetails();

                if (terrainDetails == null || terrainDetails.Count == 0)
                    continue;

                allTerrainDetails.AddRange(terrainDetails);
            }
        }

<<<<<<< HEAD
        // filter out any duplicate entries
        allTerrainDetails = allTerrainDetails.Distinct().ToList();

        // iterate over the terrain detail configs
=======
        // 중복 제거
        allTerrainDetails = allTerrainDetails.Distinct().ToList();

        // 가져온 디테일을 dictionaty에 index와 같이 설정
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        int layerIndex = 0;
        foreach (var terrainDetail in allTerrainDetails)
        {
            BiomeTerrainDetailToDetailLayerIndex[terrainDetail] = layerIndex;
            ++layerIndex;
        }
    }
#if UNITY_EDITOR
    public void RegenerateTexture()
    {
        Perform_LayerSetup();
    }

    public void RegenerateDetailPrototypes()
    {
        Perform_DetailPrototypeSetup();
    }

    void Perform_LayerSetup()
    {
        // delete any exisiting layers
        if (targetTerrain.terrainData.terrainLayers != null || targetTerrain.terrainData.terrainLayers.Length > 0)
        {
            Undo.RecordObject(targetTerrain, "Clearing previous layers");

            // build up  list of asset path for each layer
            List<string> layersToDelete = new List<string>();
            foreach(var layer in targetTerrain.terrainData.terrainLayers)
            {
                if (layer == null)
                    continue;

                layersToDelete.Add(AssetDatabase.GetAssetPath(layer.GetInstanceID()));
            }

            // remove all links to layers
            targetTerrain.terrainData.terrainLayers = null;
            
            // delete each layer
            foreach(var layerFile in layersToDelete)
            {
                if (string.IsNullOrEmpty(layerFile))
                    continue;

                AssetDatabase.DeleteAsset(layerFile);
            }

            Undo.FlushUndoRecordObjects();
        }

        string scenePath = System.IO.Path.GetDirectoryName(SceneManager.GetActiveScene().path);

        Perform_GenerateTextureMapping();



        // generate all of the layers
        int numLayers = BiomeTextureToTerrainLayerIndex.Count;
        List<TerrainLayer> newLayers = new List<TerrainLayer>(numLayers);

        // preallocate the layers
        for(int layerIndex = 0; layerIndex < numLayers; layerIndex++)
        {
            newLayers.Add(new TerrainLayer());
        }

        // iterate over the texture map
        foreach(var textureMappingEntry in BiomeTextureToTerrainLayerIndex)
        {
            var textureConfig = textureMappingEntry.Key;
            var textureLayerIndex = textureMappingEntry.Value;
            var textureLayer = newLayers[textureLayerIndex];

            // configure the terrain layer textures
            textureLayer.diffuseTexture = textureConfig.Diffuse;
            textureLayer.normalMapTexture = textureConfig.NormalMap;

            // save as asset
            string layerPath = System.IO.Path.Combine(scenePath, "Layer_" + textureLayerIndex);
            AssetDatabase.CreateAsset(textureLayer, $"{layerPath}.asset");
        }

        Undo.RecordObject(targetTerrain.terrainData, "Updating terrain layers");
        targetTerrain.terrainData.terrainLayers = newLayers.ToArray();
    }

    void Perform_DetailPrototypeSetup()
    {
        Perform_GenerateTerrainDetailMapping();

        // build the list of detail prototypes
        var detailPrototypes = new DetailPrototype[BiomeTerrainDetailToDetailLayerIndex.Count];
        foreach(var kvp in BiomeTerrainDetailToDetailLayerIndex)
        {
            TerrainDetailConfig detailData = kvp.Key;
            int layerIndex = kvp.Value;

            DetailPrototype newDetail = new DetailPrototype();

            // is this a mesh?
            if (detailData.detailPrefab)
            {
                newDetail.prototype = detailData.detailPrefab;
                newDetail.renderMode = DetailRenderMode.VertexLit;
                newDetail.usePrototypeMesh = true;
                newDetail.useInstancing = true;
            }
            else
            {
                newDetail.prototypeTexture = detailData.billboardTexture;
                newDetail.renderMode = DetailRenderMode.GrassBillboard;
                newDetail.usePrototypeMesh = false;
                newDetail.useInstancing = false;
                newDetail.healthyColor = detailData.healthyColour;
                newDetail.dryColor = detailData.dryColour;
            }

            // transfer the common data
            newDetail.minWidth = detailData.minWidth;
            newDetail.maxWidth = detailData.maxWidth;
            newDetail.minHeight = detailData.minHeight;
            newDetail.maxHeight = detailData.maxHeight;
            newDetail.noiseSeed = detailData.noiseSeed;
            newDetail.noiseSpread = detailData.noiseSpread;
            newDetail.holeEdgePadding = detailData.holeEdgePadding;

            // check the prototype
            string errorMessage;
            if (!newDetail.Validate(out errorMessage))
            {
                throw new System.InvalidOperationException(errorMessage);
            }

            detailPrototypes[layerIndex] = newDetail;
        }

        // update the detail prototypes
        Undo.RecordObject(targetTerrain.terrainData, "Updating Detail Prototypes");
        targetTerrain.terrainData.detailPrototypes = detailPrototypes;
        targetTerrain.terrainData.RefreshPrototypes();
    }

#endif  // UNITY_EDITOR

<<<<<<< HEAD
    void Perform_BiomeGeneration_LowResolution(int mapResolution)
    {
        // allocate the Biome map and strength map
        BiomeMap_LowResolution = new byte[mapResolution, mapResolution];
        BiomeStrengths_LowResolution = new float[mapResolution, mapResolution];

        // setup space for the seed points
        int numSeedPoints = Mathf.FloorToInt(mapResolution * mapResolution * config.BiomeSeedPointDensity);
        List<byte> biomeToSpawn = new List<byte>(numSeedPoints);

        // populate the biomes to spawn based on weightings
        float totalBiomeWeighting = config.TotalWeighting;
        for(int biomeIndex = 0; biomeIndex < config.NumBiomes; biomeIndex++)
        {
            int numEntries = Mathf.RoundToInt(numSeedPoints * config.Biomes[biomeIndex].Weighting / totalBiomeWeighting);
            Debug.Log("Will Spawn " + numEntries + " SeedPoints for " + config.Biomes[biomeIndex].Biome.Name);

            for(int entryIndex = 0; entryIndex < numEntries; ++entryIndex)
            {
                biomeToSpawn.Add((byte)biomeIndex);
            }
        }

        // spawn the individual biomes
        while(biomeToSpawn.Count > 0)
        {
            // pick a random seed point
            int seedPointIndex = Random.Range(0, biomeToSpawn.Count);

            // extract the biome index
            byte biomeIndex = biomeToSpawn[seedPointIndex];

            // remove seed point
            biomeToSpawn.RemoveAt(seedPointIndex);

            Perform_SpawnIndividualBiome(biomeIndex, mapResolution);
        }
#if UNITY_EDITOR
        // save out the biome map
        Texture2D biomeMap = new Texture2D(mapResolution, mapResolution, TextureFormat.RGB24, false);
        for(int y = 0; y < mapResolution; y++)
        {
            for(int x = 0; x < mapResolution; x++)
            {
                float hue = ((float)BiomeMap_LowResolution[x, y] / (float)config.NumBiomes);
                biomeMap.SetPixel(x, y, Color.HSVToRGB(hue, 0.75f, 0.75f));
            }
        }
        biomeMap.Apply();

        System.IO.File.WriteAllBytes("BiomeMap_LowResolution.png", biomeMap.EncodeToPNG());
#endif  // UNITY_EDITOR
    }

    byte CalculateHighResBiomeIndex(int lowResMapSize, int lowResX, int lowResY, float fractionX, float fractionY)
    {
        float A = BiomeMap_LowResolution[lowResX,     lowResY];
        float B = (lowResX + 1) < lowResMapSize ? BiomeMap_LowResolution[lowResX + 1, lowResY] : A;
        float C = (lowResY + 1) < lowResMapSize ? BiomeMap_LowResolution[lowResX,     lowResY + 1] : A;
        float D = 0;
        if ((lowResX + 1) >= lowResMapSize)
            D = C;
        else if ((lowResY + 1) >= lowResMapSize)
            D = B;
        else
            D = BiomeMap_LowResolution[lowResX + 1, lowResY + 1];

        // perform bilinear filtering
        float filteredindex =   A * (1 - fractionX) * (1 - fractionY) + B * fractionX * (1 - fractionY) * 
                        C * fractionY * (1 - fractionX) + D * fractionX * fractionY;

        // build an array of the possible biomes based on the values used to interpolate
        float[] candidateBiome = new float[] { A, B, C, D };

        // find the neighbouring biome closest to the interpolated biome
        float bestBiome = -1f;
        float bestDelta = float.MaxValue;

        for(int biomeIndex = 0; biomeIndex < candidateBiome.Length; biomeIndex++)
        {
            float delta = Mathf.Abs(filteredindex - candidateBiome[biomeIndex]);

            if(delta < bestDelta)
            {
                bestDelta = delta;
                bestBiome = candidateBiome[biomeIndex];
            }
        }

        return (byte)Mathf.RoundToInt(bestBiome);
    }

    void Perform_BiomeGeneration_HighResolution(int lowResMapSize, int highResMapSize)
    {
        // allocate the biome map and strength map
        BiomeMap = new byte[highResMapSize, highResMapSize];
        BiomeStrengths = new float[highResMapSize, highResMapSize];

        // calculate map scale
        float mapScale = (float)lowResMapSize / highResMapSize;

        // calculate the high res map
        for(int y = 0; y < highResMapSize; y++)
        {
            int lowResY = Mathf.FloorToInt(y * mapScale);
            float yFraction = y * mapScale - lowResY;

            for(int x = 0; x < highResMapSize; x++)
            {
                int lowResX = Mathf.FloorToInt(x * mapScale);
                float xFraction = x * mapScale - lowResX;

                BiomeMap[x, y] = CalculateHighResBiomeIndex(lowResMapSize, lowResX, lowResY, xFraction, yFraction);

                // BiomeMap[x, y] = BiomeMap_LowResolution[lowResX, lowResY];
            }
        }
#if UNITY_EDITOR
        // save out the biome map
        Texture2D biomeMap = new Texture2D(highResMapSize, highResMapSize, TextureFormat.RGB24, false);
        for (int y = 0; y < highResMapSize; y++)
        {
            for (int x = 0; x < highResMapSize; x++)
            {
                float hue = ((float)BiomeMap[x, y] / (float)config.NumBiomes);
                biomeMap.SetPixel(x, y, Color.HSVToRGB(hue, 0.75f, 0.75f));
            }
        }
        biomeMap.Apply();

        System.IO.File.WriteAllBytes("BiomeMap_HighResolution.png", biomeMap.EncodeToPNG());
#endif  // UNITY_EDITOR
    }
    Vector2Int[] NeighbourOffsets = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
    };

    void Perform_SpawnIndividualBiome(byte biomeIndex, int mapResolution)
    {
        // cache biome config
        BiomeConfigSO biomeConfig = config.Biomes[biomeIndex].Biome;

        // pick spawn location
        Vector2Int spawnLocation = new Vector2Int(Random.Range(0, mapResolution), Random.Range(0, mapResolution));

        //pick the starting intensity
        float startIntensity = Random.Range(biomeConfig.MinIntensity, biomeConfig.MaxIntensity);

        // setup working list
        Queue<Vector2Int> workingList = new Queue<Vector2Int>();
        workingList.Enqueue(spawnLocation);

        // setup the visit map and target intensity map
        bool[,] visited = new bool[mapResolution, mapResolution];
        float[,] targetIntensity = new float[mapResolution, mapResolution];

        // set the starting intensity
        targetIntensity[spawnLocation.x, spawnLocation.y] = startIntensity;

        // let the oozing begin
        while(workingList.Count > 0) 
        {
            Vector2Int workingLocation = workingList.Dequeue();

            // set the biome
            BiomeMap_LowResolution[workingLocation.x, workingLocation.y] = biomeIndex;
            visited[workingLocation.x, workingLocation.y] = true;
            BiomeStrengths_LowResolution[workingLocation.x, workingLocation.y] = targetIntensity[workingLocation.x, workingLocation.y];

            // traverse neighbours
            for(int neighbourIndex = 0; neighbourIndex < NeighbourOffsets.Length; neighbourIndex++)
            {
                Vector2Int neighbourLocation = workingLocation + NeighbourOffsets[neighbourIndex];

                // skip if invalid
                if (neighbourLocation.x < 0 || neighbourLocation.y < 0 || neighbourLocation.x >= mapResolution || neighbourLocation.y >= mapResolution)
                    continue;
                // skip if visited
                if (visited[neighbourLocation.x, neighbourLocation.y])
                    continue;

                // flag as visited
                visited[neighbourLocation.x, neighbourLocation.y] = true;

                // work out and store neighbour strength
                float decayAmount = Random.Range(biomeConfig.MinDecayRate, biomeConfig.MaxDecayRate) * NeighbourOffsets[neighbourIndex].magnitude;
                float neighbourStrength = targetIntensity[workingLocation.x, workingLocation.y] - decayAmount;
                targetIntensity[neighbourLocation.x, neighbourLocation.y] = neighbourStrength;


                // if the strength is too low - stop
                if(neighbourStrength <= 0)
                {
                    continue;
                }

                workingList.Enqueue(neighbourLocation);
            }
        }
    }   // Perform_SpawnIndividualBiome()

    void Perform_HeightMapModification(int mapResolution, int alphaMapResolution)
    {
        float[,] heightMap = targetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);
        // execute any initial height modifiers
=======
    void Perform_BiomeGeneration(int mapResolution)
    {
        // 바이옴 맵, 바이옴 강세 맵 할당
        BiomeMap = new byte[mapResolution, mapResolution];
        BiomeStrengths = new float[mapResolution, mapResolution];

        // 바이옴 생성기 실행
        if(config.BiomeGenerators != null)
        {
            BaseBiomeMapGenerator[] generators = config.BiomeGenerators.GetComponents<BaseBiomeMapGenerator>();

            foreach(var generator in generators)
            {
                generator.Execute(config, mapResolution, BiomeMap, BiomeStrengths);
            }
        }
    }
    void Perform_HeightMapModification(int mapResolution, int alphaMapResolution)
    {
        float[,] heightMap = targetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);

        // 높이맵 선처리 작업
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        if(config.InitialHeightModifier != null)
        {
            BaseHeightMapModifier[] modifiers = config.InitialHeightModifier.GetComponents<BaseHeightMapModifier>();

            foreach(var modifier in modifiers)
            {
                modifier.Execute(config, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale);
            }
<<<<<<< HEAD

        }

        // run heightmap generation for each biome
        for(int biomeIndex = 0; biomeIndex < config.NumBiomes; biomeIndex++)
=======
        }

        // 바이옴마다 설정되어있는 높이맵 수정자 실행
        for (int biomeIndex = 0; biomeIndex < config.NumBiomes; biomeIndex++)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        {
            var biome = config.Biomes[biomeIndex].Biome;

            if (biome.HeightModifier == null || biome.HeightModifier == default)
                continue;


            BaseHeightMapModifier[] modifiers = biome.HeightModifier.GetComponents<BaseHeightMapModifier>();

            foreach (var modifier in modifiers)
            {
                modifier.Execute(config, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale, BiomeMap, biomeIndex, biome);
            }
        }

<<<<<<< HEAD
        // execute any post processing height modifiers
=======
        // 높이맵 후처리 작업
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        if (config.HeightPostProcessingModifier != null)
        {
            BaseHeightMapModifier[] modifiers = config.HeightPostProcessingModifier.GetComponents<BaseHeightMapModifier>();

            foreach (var modifier in modifiers)
            {
<<<<<<< HEAD
                modifier.Execute(config, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale);
            }
        }

        targetTerrain.terrainData.SetHeights(0, 0, heightMap);

        // generate the slope map
=======
                modifier.Execute(config, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale, BiomeMap);
            }
        }

        // 지형에 높이맵 설정
        targetTerrain.terrainData.SetHeights(0, 0, heightMap);

        // 기울기 맵 설정
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        SlopeMap = new float[alphaMapResolution, alphaMapResolution];
        for (int y = 0; y < alphaMapResolution; y++)
        {
            for (int x = 0; x < alphaMapResolution; x++)
            {
                SlopeMap[x, y] = targetTerrain.terrainData.GetInterpolatedNormal((float)x / alphaMapResolution, (float)y / alphaMapResolution).y;
            }
        }
    }

    public int GetLayerForTexture(TextureConfig textureConfig)
    {
        return BiomeTextureToTerrainLayerIndex[textureConfig];
    }
    public int GetDetailLayerForTerrainDetail(TerrainDetailConfig detailConfig)
    {
        return BiomeTerrainDetailToDetailLayerIndex[detailConfig];
    }

    void Perform_TerrainPainting(int mapResolution, int alphaMapResolution)
    {
<<<<<<< HEAD
=======
        // 높이맵, 알파맵 캐싱
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        float[,] heightMap = targetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);
        float[,,] alphaMaps =  targetTerrain.terrainData.GetAlphamaps(0, 0, alphaMapResolution, alphaMapResolution);


<<<<<<< HEAD
        // zero out all layers
=======
        // 알파맵 초기화
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        for (int y = 0; y < alphaMapResolution; y++)
        {
            for (int x = 0; x < alphaMapResolution; x++)
            {
                for (int layerIndex = 0; layerIndex < targetTerrain.terrainData.alphamapLayers; layerIndex++)
                {
                    alphaMaps[x, y, layerIndex] = 0;
                }
            }
        }

<<<<<<< HEAD
        // run terrain painting for each biome
=======
        // 바이옴마다 지형 페인터 실행
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        for (int biomeIndex = 0; biomeIndex < config.NumBiomes; biomeIndex++)
        {
            var biome = config.Biomes[biomeIndex].Biome;

            if (biome.TerrainPainter == null || biome.TerrainPainter == default)
                continue;


            BaseTexturePainter[] modifiers = biome.TerrainPainter.GetComponents<BaseTexturePainter>();

            foreach (var modifier in modifiers)
            {
                modifier.Execute(this, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale, SlopeMap, alphaMaps, alphaMapResolution,  BiomeMap, biomeIndex, biome);
            }
        }

<<<<<<< HEAD
        // run texture post processing 
=======
        // 후처리 페인터 작업
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        if(config.PaintingPostProcessingModifier != null)
        {
            BaseTexturePainter[] modifiers = config.PaintingPostProcessingModifier.GetComponents<BaseTexturePainter>();

            foreach (var modifier in modifiers)
            {
                modifier.Execute(this, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale, SlopeMap, alphaMaps, alphaMapResolution);
            }
        }
<<<<<<< HEAD
        



=======

        // 지형 알파맵 설정
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        targetTerrain.terrainData.SetAlphamaps(0, 0, alphaMaps);
    } // Perform_TerrainPainting

    void Perform_ObjectPlacement(int mapResolution, int alphaMapResolution)
    {
<<<<<<< HEAD
        if(DEBUG_TurnOffObjectPlacers)
            return;

=======
        // 디버깅 변수로 해당 함수의 실행여부 결정
        if(DEBUG_TurnOffObjectPlacers)
            return;

        // 높이맵, 알파맵 캐싱
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        float[,] heightMap = targetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);
        float[,,] alphaMaps = targetTerrain.terrainData.GetAlphamaps(0, 0, alphaMapResolution, alphaMapResolution);


<<<<<<< HEAD
        // run object placement for each biome
=======
        // 바이옴마다 오브젝트 배치자 실행
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        for (int biomeIndex = 0; biomeIndex < config.NumBiomes; biomeIndex++)
        {
            var biome = config.Biomes[biomeIndex].Biome;

            if (biome.ObjectPlacer == null || biome.ObjectPlacer == default)
                continue;

            BaseObjectPlacer[] modifiers = biome.ObjectPlacer.GetComponents<BaseObjectPlacer>();

            foreach (var modifier in modifiers)
            {
                modifier.Execute(config, transform, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale, SlopeMap, alphaMaps, alphaMapResolution, BiomeMap, biomeIndex, biome);
            }
        }
    }

<<<<<<< HEAD
=======
    void Perform_NavMeshBaking()
    {
        // 디버깅 변수로 해당 함수의 실행여부 결정
        if(DEBUG_TurnOffBakeNavMeshes)
            return;
            
        Debug.Log("NavMesh Bake");
        
        // 모든 지정된 네비메쉬 베이크
        for(int i = 0; i < navMeshSurfaces.Length; i++)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }

>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    void Perform_DetailPainting(int mapResolution, int alphaMapResolution, int detailMapResolution, int maxDetailsPerPatch)
    {
        float[,] heightMap = targetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);
        float[,,] alphaMaps = targetTerrain.terrainData.GetAlphamaps(0, 0, alphaMapResolution, alphaMapResolution);

        // create a new empty set of layers
        int numDetailLayers = targetTerrain.terrainData.detailPrototypes.Length;
        List<int[,]> detailLayerMaps = new List<int[,]>(numDetailLayers);
        for (int layerIndex = 0; layerIndex < numDetailLayers; ++layerIndex)
        {
            detailLayerMaps.Add(new int[detailMapResolution, detailMapResolution]);
        }

        // run terrain detail painting for each biome
        for (int biomeIndex = 0; biomeIndex < config.NumBiomes; ++biomeIndex)
        {
            var biome = config.Biomes[biomeIndex].Biome;
            if (biome.DetailPainter == null)
                continue;

            BaseDetailPainter[] modifiers = biome.DetailPainter.GetComponents<BaseDetailPainter>();

            foreach (var modifier in modifiers)
            {
                modifier.Execute(this, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale, SlopeMap, alphaMaps, alphaMapResolution, detailLayerMaps, detailMapResolution, maxDetailsPerPatch, BiomeMap, biomeIndex, biome);
            }
        }

        // run detail painting post processing
        if (config.DetailPaintingPostProcessingModifier != null)
        {
            BaseDetailPainter[] modifiers = config.DetailPaintingPostProcessingModifier.GetComponents<BaseDetailPainter>();

            foreach (var modifier in modifiers)
            {
                modifier.Execute(this, mapResolution, heightMap, targetTerrain.terrainData.heightmapScale, SlopeMap, alphaMaps, alphaMapResolution, detailLayerMaps, detailMapResolution, maxDetailsPerPatch);
            }
        }

        // apply the detail layers
        for (int layerIndex = 0; layerIndex < numDetailLayers; ++layerIndex)
        {
            targetTerrain.terrainData.SetDetailLayer(0, 0, layerIndex, detailLayerMaps[layerIndex]);
        }
    }

<<<<<<< HEAD
=======
    void Perform_PostProcessOnLoading()
    {
        GameObject[] PostPrcObjects = GameObject.FindGameObjectsWithTag(GData.POSTPROCESS_ON_LOADING);
        Debug.Log($"PostPrcObjects Count : {PostPrcObjects.Length}");
        List<PostProcessOnLoading> postProcessOnLoadings = new List<PostProcessOnLoading>();
        foreach (var postPrcObj in PostPrcObjects)
        {
            PostProcessOnLoading ppol = postPrcObj.GetComponent<PostProcessOnLoading>();
            if (ppol == null)
                continue;

            postProcessOnLoadings.Add(ppol);
        }

        foreach(var postProcessor in postProcessOnLoadings)
        {
            if(postProcessor != null)
            {
                postProcessor.Execute();
            }
        }
        
    }

    void Perform_SetupWorldMap(ProcGenConfigSO config, int mapResolution)
    {
        float[,] heightMap = targetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);
        Vector3 heightMapScale = targetTerrain.terrainData.heightmapScale;
        Color heightColor_HighMountain = Color.white;
        Color heightColor_Water = Color.cyan;

        // 바이옴 맵 텍스쳐 할당
        Texture2D biomeMapTexture = new Texture2D(mapResolution, mapResolution, TextureFormat.RGB24, false);
        for (int y = 0; y < mapResolution; y++)
        {
            for (int x = 0; x < mapResolution; x++)
            {
                // 바이옴맵 기반으로 바이옴의 맵 컬러값을 가져옴
                Color workingColor = config.Biomes[(int)BiomeMap[x, y]].Biome.mapColor;
                // 높이맵으로 높이값을 계산하여 특정 높이는 다른 색으로 설정
                if(heightMap[x,y] * heightMapScale.y >= 110f)
                    workingColor = heightColor_HighMountain;
                else if(heightMap[x,y] * heightMapScale.y <= 15f)
                    workingColor = heightColor_Water;

                // 탐색 좌표에 색 설정
                biomeMapTexture.SetPixel(y, x, workingColor);
            }
        }
        biomeMapTexture.Apply();

        UIManager.Instance.worldMapTexture = biomeMapTexture;
#if UNITY_EDITOR
        System.IO.File.WriteAllBytes("BiomeMap_WorldMap.png", biomeMapTexture.EncodeToPNG());
#endif  // UNITY_EDITOR
    }

    // 보스성 검증 불리언 함수
    bool IsValidHeightModifying()
    {
        bool bIsSuccessed = true;
#if UNITY_EDITOR
        if(Application.isPlaying)
        {
            // 보스성을 찾고
            GameObject bossCastle = gameObject.FindChildObj("BossCastle(Clone)");
            // 없는 경우
            if(bossCastle == default)
            {
                Debug.Log("Can't found Boss Castle");
                bIsSuccessed = false;
            }
            else
            {
                // 있는 경우
                Debug.Log("find Boss Castle");
            }    
        }
        else
        {
            /* To do? */
        } 
#else  
        GameObject bossCastle = gameObject.FindChildObj("BossCastle(Clone)");
        if(bossCastle == default)
        {
            Debug.Log("Can't found Boss Castle");
            bIsSuccessed = false;
        }
        else
            Debug.Log("find Boss Castle");
#endif 
        return bIsSuccessed;
    }

>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
}
