using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GData
{
<<<<<<< HEAD
    public const string PLAYER_MASK = "Player";
    public const string ENEMY_MASK = "Enemy";
    public const string BUILD_MASK = "build";
    public const string GATHER_MASK = "Gather";
    public const string TERRAIN_MASK = "Terrain";
}

//! 지형의 속성을 정의하기 위한 타입
public enum TerrainType
{
    NONE = -1, 
    PLAIN_PASS,
    OCEAN_N_PASS
}       // PuzzleType
=======
    // Scene Name
    public const string SCENENAME_TITLE = "TitleScene";
    public const string SCENENAME_LOADING = "LoadingScene";
    public const string SCENENAME_PLAY = "PlayScene";

    // Layer Mask
    public const string PLAYER_MASK = "Player";
    public const string ENEMY_MASK = "Enemy";
    public const string BUILD_MASK = "Build";
    public const string GATHER_MASK = "Gather";
    public const string TERRAIN_MASK = "Terrain";
    public const string FLOOR_MASK = "Floor";
    public const string WALL_MASK = "Wall";
    public const string ANIMAL_MASK = "Animal";

    public const string POSTPROCESS_ON_LOADING = "PostProcessObject";

    // Asset Path
    public const string PREFAB_PATH = "Prefabs/";
    public const string UI_PATH = "UI/";
}

public enum EBiome
{
    BUSH,
    GRASSYPLAINS,
    RAINFOREST,
    SCRUB,
    SWAMP,
    DECAY,
    HIGHMOUNTAIN,
    MIDDLEMOUNTAIN,
    LOWMOUNTAIN
}
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
