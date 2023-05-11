using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
[CreateAssetMenu(fileName = "Biome Config", menuName = "Procedural Generation/Biome Configuration", order = -1)]
public class BiomeConfigSO : ScriptableObject
{
    public string Name;

=======
// ����Ƽ Create�� ��ũ���ͺ� ������Ʈ�� ����� �ִ� menu�� ����
[CreateAssetMenu(fileName = "Biome Config", menuName = "Procedural Generation/Biome Configuration", order = -1)]
public class BiomeConfigSO : ScriptableObject
{
    // ���̿� �̸�
    public string Name;

    // �ּ� �ִ� �е� * �������� ���̿� ũ�� �о���
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    [Range(0f, 1.0f)]
    public float MinIntensity = 0.5f;
    [Range(0f, 1.0f)]
    public float MaxIntensity = 1f;

<<<<<<< HEAD
=======
    // �ּ� �ִ� ������ * �������� ���̿� ũ�� �۾���
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    [Range(0f, 1.0f)]
    public float MaxDecayRate = 0.01f;
    [Range(0f, 1.0f)]
    public float MinDecayRate = 0.02f;

<<<<<<< HEAD
    public GameObject HeightModifier;
    public GameObject TerrainPainter;
    public GameObject ObjectPlacer;
=======
    // �� ǥ�� �÷�
    public Color mapColor = default;

    // ���� ������
    public GameObject HeightModifier;
    // �ؽ�ó���̾� ������
    public GameObject TerrainPainter;
    // ������Ʈ ��ġ��
    public GameObject ObjectPlacer;
    // ������ ����Ʈ ������
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    public GameObject DetailPainter;

    public List<TextureConfig> RetrieveTextures()
    {
        if (TerrainPainter == null)
            return null;
        
        List<TextureConfig> allTextures = new List<TextureConfig>();
        BaseTexturePainter[] allPainters = TerrainPainter.GetComponents<BaseTexturePainter>();
        foreach(var painter in allPainters)
        {
            var painterTextures = painter.RetrieveTextures();

            if(painterTextures == null || painterTextures.Count == 0)
            {
                continue;
            }
            allTextures.AddRange(painterTextures);
        }

        return allTextures;
    }
    public List<TerrainDetailConfig> RetrieveTerrainDetails()
    {
        if (DetailPainter == null)
            return null;

        // extract all terrain details from every painter
        List<TerrainDetailConfig> allTerrainDetails = new List<TerrainDetailConfig>();
        BaseDetailPainter[] allPainters = DetailPainter.GetComponents<BaseDetailPainter>();
        foreach (var painter in allPainters)
        {
            var terrainDetails = painter.RetrieveTerrainDetails();

            if (terrainDetails == null || terrainDetails.Count == 0)
                continue;

            allTerrainDetails.AddRange(terrainDetails);
        }

        return allTerrainDetails;
    }

}
