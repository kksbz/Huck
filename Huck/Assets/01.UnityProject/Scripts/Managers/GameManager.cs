using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using Unity.VisualScripting;
using UnityEngine;
=======
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

public class GameManager : Singleton<GameManager>
{
    public GameObject playerObj = default;
<<<<<<< HEAD
    //! { [�豤��] �÷��̾� ������Ʈ ���۽� ĳ��
    private void Awake()
    {
        playerObj = GFunc.GetRootObj(GData.PLAYER_MASK);
    }
    // } [�豤��] �÷��̾� ������Ʈ ���۽� ĳ��
=======

    public GameObject procGenManager = default;
    public BuildSystem buildSystem = default;

    public TimeController timeController = default;

    public Terrain terrain = default;

    //
    private bool isMidBossClear = false;
    public bool IsMidBossClear
    {
        get { return isMidBossClear; }
        set { isMidBossClear = value; }
    }
    //
    public AudioSource bgmAudio = default;
    public AudioClip bgmSound = default;
    public AudioClip bossBgmSound = default;
    //
    public delegate void EventHandler();
    public EventHandler onPlayerDead;
    //

    //! { [KKS] 몬스터 스폰관련 변수
    [Header("스폰할 몬스터 Prefab")]
    public List<GameObject> nomalMonsterPrefab = default;
    public GameObject nameedMonsterPrefab = default;
    public GameObject bossMonsterPrefab = default;
    public Transform bossPos = default; // 보스 소환 위치
    public bool isExistenceBoss = false; // 보스 존재 유무
    public int count = 0; // Day체크
    //! } [KKS] 몬스터 스폰관련 변수

    //
    public BossRoomLightControl bossRoomLightController = default;
    //

    //! 게임매니져 초기화 함수
    protected override void Init()
    {
        procGenManager = GFunc.GetRootObj("ProcGenManager");
        IsMidBossClear = false;
        bgmAudio = GetComponent<AudioSource>();
        onPlayerDead = new EventHandler(() => Debug.Log("Player Dead"));
    }
    //! { [KKS] 몬스터 소환 함수
    public void SpawnMonster()
    {
        // 보스방 입장시 몬스터소환을 하지않음
        if (isExistenceBoss == false)
        {
            // 소환할 노멀몬스터의 타입 정할 변수 (Day가 지날때 마다 count가 1씩 증가함)
            int nomalTypeCount = count;
            if (count >= nomalMonsterPrefab.Count)
            {
                // count의 값이 노멀프리팹List의 크기보다 같거나 크면 고정 
                nomalTypeCount = nomalMonsterPrefab.Count - 1;
            }
            Debug.Log($"count 길이 : {nomalTypeCount}, {nomalMonsterPrefab.Count}");

            // 스폰할 몬스터의 수 정함
            int spawnNumber = 3 + count;
            if (spawnNumber > 10)
            {
                spawnNumber = 10;
            }

            List<Vector3> spawnPointList = new List<Vector3>();
            GetRandomPosition getRandomPosition = new GetRandomPosition();
            while (spawnPointList.Count < spawnNumber)
            {
                // 중복되지않는 원범위 랜덤좌표를 스폰할 수만큼 가져옴
                Vector3 point = getRandomPosition.GetRandomCirclePos(playerObj.transform.position, 30, 20);
                if (!spawnPointList.Contains(point))
                {
                    spawnPointList.Add(point);
                }
            }

            for (int i = 0; i < spawnNumber; i++)
            {
                // 플레이어를 바라보면서 소환되게 회전축 설정
                Vector3 dirToTarget = default;
                if (count == 4 || count == 7)
                {
                    if (count == 4)
                    {
                        // 5일차 밤에는 중간보스 1마리만 소환
                        Vector3 point = spawnPointList[i];
                        dirToTarget = (playerObj.transform.position - point).normalized;
                        GameObject nameedMonster = Instantiate(nameedMonsterPrefab, point, Quaternion.LookRotation(dirToTarget));
                        i = spawnNumber;
                    }
                    else
                    {
                        // 8일차 밤에는 중간보스 3마리 소환
                        Vector3 point = spawnPointList[i];
                        dirToTarget = (playerObj.transform.position - point).normalized;
                        GameObject nameedMonster = Instantiate(nameedMonsterPrefab, point, Quaternion.LookRotation(dirToTarget));
                        // 9일차 밤부터 일반몬스터와 같이 소환될수있게 처리
                        if (i >= 2)
                        {
                            nomalMonsterPrefab.Add(nameedMonsterPrefab);
                            i = spawnNumber;
                        }
                    }
                }
                else
                {
                    // 몬스터 소환
                    int randomIndex = Random.Range(0, nomalTypeCount + 1);
                    dirToTarget = (playerObj.transform.position - spawnPointList[i]).normalized;
                    GameObject nomalMonster = Instantiate(nomalMonsterPrefab[randomIndex], spawnPointList[i], Quaternion.LookRotation(dirToTarget));
                }
            }
        }
        count += 1;
    } // SpawnMonster

    public void BossSpwan()
    {
        // 보스몬스터가 없을 경우에만 소환
        if (isExistenceBoss == false)
        {
            GameObject boss = Instantiate(bossMonsterPrefab, bossPos.position, bossPos.rotation);
            isExistenceBoss = true;
            Debug.Log("보스소환!!");
        }
    } // BossSpwan
    //! } [KKS] 몬스터 소환 함수

    public void StartBGM()
    {
        bgmAudio.clip = bgmSound;
        bgmAudio.Play();
    } // StartBGM
    public void StartBossBGM()
    {
        bossRoomLightController.onStartBattle();
        bgmAudio.clip = bossBgmSound;
        bgmAudio.Play();
    } // StartBGM

    public void StartEnding()
    {
        LoadingManager.Instance.EndingStart();
    }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
}
