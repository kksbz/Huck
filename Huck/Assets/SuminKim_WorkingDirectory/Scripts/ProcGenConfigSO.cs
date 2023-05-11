using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeConfig
{
    public BiomeConfigSO Biome;

    [Range(0f, 1.0f)] public float Weighting = 1f;
}

[CreateAssetMenu(fileName = "ProcGen Config", menuName = "Procedural Generation/ProcGen Configuration", order = -1)]
public class ProcGenConfigSO : ScriptableObject
{
<<<<<<< HEAD
    public List<BiomeConfig> Biomes;

    public enum EBiomeMapBaseResolution
    {
        Size_64x64      = 64,
        Size_128x128    = 128,
        Size_256x256    = 256,
        Size_512x512    = 512,
    }

    [Range(0f, 1.0f)]
    public float BiomeSeedPointDensity = 0.1f;

    public EBiomeMapBaseResolution biomeMapResolution = EBiomeMapBaseResolution.Size_64x64;

    public GameObject InitialHeightModifier;
    public GameObject HeightPostProcessingModifier;

    public GameObject PaintingPostProcessingModifier;
    public GameObject DetailPaintingPostProcessingModifier;

=======
    // 바이옴 리스트
    public List<BiomeConfig> Biomes;

    // 바이옴 생성기
    public GameObject BiomeGenerators;
    // 높이맵 선 처리 작업 수정자
    public GameObject InitialHeightModifier;
    // 높이맵 후 처리 작업 수정자
    public GameObject HeightPostProcessingModifier;
    // 테라인 후 처리 페인터
    public GameObject PaintingPostProcessingModifier;
    // 테라인 후처리 디테일 페인팅 수정자
    public GameObject DetailPaintingPostProcessingModifier;

    // water plane 높이
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    public float waterHeight = 15f;

    public int NumBiomes => Biomes.Count;

    public float TotalWeighting
    {
        get
        {
            float sum = 0f;

            foreach(var config in Biomes)
            {
                sum += config.Weighting;
            }
            return sum;
        }
    }
}
