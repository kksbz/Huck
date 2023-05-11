using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEditor.Presets;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class BuildSystem : MonoBehaviour
{
    private List<GameObject> buildObjs;
    private List<Material> buildMats;

    private float hitDistance = 20.0f;
    private RaycastHit hit;
    private Ray ray;

    private Transform cameraTrans;
    private GameObject prevObj;
    private PrevObjInfo prevInfo;
=======
using UnityEngine;
using UnityEngine.AI;

public class BuildSystem : MonoBehaviour
{
    //using LayerMaskName
    private const string BUILD_TEMP_LAYER = "BuildThings";
    private const string BUILD_FLOOR_LAYER = "BuilFloor";
    private const string BUILD_WALL_LAYER = "BuildWall";
    private const string BUILD_OBJ_LAYER = "BuildObj";
    private const string BUILD_ITEM_LAYER = "BuildItem";
    private const string BUILD_LAYER = GData.BUILD_MASK;

    private List<GameObject> BuildLoadObjs;
    private List<Material> BuildLoadMats;

    private const float HIT_DISTANCE = 10.0f;
    private RaycastHit hit;
    private Ray ray;

    private GameObject prevObj;
    public bool IsBuildAct;
    private PrevObjInfo prevInfo;
    private PrevObjInfo prevDefaultInfo;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    private Vector3 prevPos;
    private Vector3 prevRot;
    private float prevYAngle;

    private buildType prevType;
    private buildTypeMat prevMat;
<<<<<<< HEAD
    private int layermask;
    private bool IsBuildTime;

    private float gridSize = 0.1f;
    private bool debugMode = false;

    void Awake()
    {
        buildObjs = new List<GameObject>();
        buildMats = new List<Material>();
=======
    private string prevName;
    private int buildObjNums;
    private int layerMask;
    public bool IsBuildTime;
    private bool IsResetCall;

    private float gridSize = 0.1f;
    public bool IsDefaultLayer = false;

    private List<GameObject> BuildingList;

    void Awake()
    {
        BuildLoadObjs = new List<GameObject>();
        BuildLoadMats = new List<Material>();
        BuildingList = new List<GameObject>();
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

        GameObject[] loadObjs = Resources.LoadAll<GameObject>("PBS/BuildPreFab/prevBuild");
        Material[] loadMats = Resources.LoadAll<Material>("PBS/BuildPreFab/Materials");

        for (int i = 0; i < loadObjs.GetLength(0); i++)
        {
<<<<<<< HEAD
            buildObjs.Add(loadObjs[i]);
=======
            BuildLoadObjs.Add(loadObjs[i]);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        }

        for (int i = 0; i < loadMats.GetLength(0); i++)
        {
<<<<<<< HEAD
            buildMats.Add(loadMats[i]);
        }

        cameraTrans = Camera.main.transform;

        //초기값
        IsBuildTime = false;
        prevType = buildType.floor;
        prevMat = buildTypeMat.green;

        prevRot = Vector3.zero;
        prevYAngle = 0.0f;
        // layermask = (-1) - (1 << LayerMask.NameToLayer("buildThings") | 1 << LayerMask.NameToLayer("Player")); //해당 레이어들만 제외
        layermask = (-1) - (1 << LayerMask.NameToLayer("buildThings")); //해당 레이어만 제외

        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {

    }

    void Update()
    {
        ControlKey();
        if (IsBuildTime) { RaycastUpdate(); }
=======
            BuildLoadMats.Add(loadMats[i]);
        }

        //reset
        IsBuildTime = false;
        IsResetCall = false;
        IsBuildAct = false;
        prevType = buildType.Foundation;
        prevMat = buildTypeMat.Green;

        prevRot = Vector3.zero;
        prevYAngle = 0.0f;
        buildObjNums = 0;

        //raycast rayerSet
        layerMask = (-1) - (1 << LayerMask.NameToLayer(BUILD_TEMP_LAYER));
        // DefaultLayerMask = (1 << LayerMask.NameToLayer(BUILD_TEMP_LAYER) | 1 << LayerMask.NameToLayer("Default"));
    }
    private void Start()
    {
        GameManager.Instance.playerObj.GetComponent<InHand>().buildSystem = this;
    }
    void Update()
    {
        ControlKey();

        float X_Angle = Quaternion.Angle(Camera.main.transform.rotation, Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0));

        if (IsBuildTime && X_Angle < 45.0f)
        {
            C_prevObj(prevType);
            if (!IsResetCall) RaycastUpdate();
        }
        else if (IsBuildTime && X_Angle >= 45.0f)
        {
            D_prevObj();
        }
    }

    public void CallingPrev()
    {
        D_prevObj();
    }

    public void CallingPrev(string btype)
    {
        IsResetCall = true;
        prevName = btype;
        switch (btype)
        {
            case "WoodBeam":
                prevType = buildType.Beam;
                break;
            case "WoodCut":
                prevType = buildType.Cut;
                break;
            case "WoodDoor":
                prevType = buildType.Door;
                break;
            case "WoodWindowWall":
                prevType = buildType.Windowswall;
                break;
            case "WoodWall":
                prevType = buildType.Wall;
                break;
            case "WoodFloor":
                prevType = buildType.Floor;
                break;
            case "WoodFoundation":
                prevType = buildType.Foundation;
                break;
            case "WoodRoof":
                prevType = buildType.Roof;
                break;
            case "WoodStairs":
                prevType = buildType.Stairs;
                break;
            case "Anvil":
                prevType = buildType.Anvil;
                break;
            case "Stove":
                prevType = buildType.Stove;
                break;
            case "Workbench":
                prevType = buildType.Workbench;
                break;
        }

        D_prevObj();
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    }

    private void ControlKey()
    {
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!debugMode) debugMode = true;
            else if (debugMode) debugMode = false;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (IsBuildTime == true)
            {
                D_prevObj();
                IsBuildTime = false;
            }
            else if (IsBuildTime == false)
            {
                IsBuildTime = true;
                C_prevObj(prevType, 1);
            }
        }

        if (IsBuildTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                D_prevObj();
                prevType++;
                if ((int)prevType > 7) prevType = 0;
                C_prevObj(prevType, 1);
            }

=======
        if (!IsBuildTime)
        {
            //문열기
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray DoorRAY = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hitD;

                if (Physics.Raycast(DoorRAY, out hitD, HIT_DISTANCE))
                {
                    if (hitD.transform.name == "DoorCollider")
                    {
                        DoorInfo temp = hitD.transform.gameObject.GetComponent<DoorInfo>();

                        temp.IsTrigger();
                    }
                }
            }
        }
        else if (IsBuildTime)
        {
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            if (Input.GetKeyDown(KeyCode.E))
            {
                prevYAngle += 45.0f;
                if (prevYAngle > 360.0f) prevYAngle = 0.0f;
            }
<<<<<<< HEAD

            if (Input.GetMouseButtonDown(1))
            {
                //설치
                BuildObj();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.point != null && hit.point != default)
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("build"))
                    {
                        Destroy(hit.transform.parent.gameObject);
                        if (prevInfo != null || prevInfo != default) prevInfo.deleteObjTime();
                    }
                }
            }
        }
    }


    private void RayFloor(RaycastHit hit2)
    {
        switch(hit2.transform.name)
        {
            //case "Center":
            //    break;
            //case "LeftTop":
            //    break;
            //case "LeftTop":
            //    break;
            //case "LeftTop":
            //    break;
            //case "LeftTop":
            //    break;
            //case "LeftTop":
            //    break;
            //case "LeftTop":
            //    break;
            //case "LeftTop":
            //    break;
=======
            if (Input.GetMouseButtonDown(1))
            {
                BuildObj();
            }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        }
    }

    private void RaycastUpdate()
    {
        //정가운데 화면 레이 쏘기
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
<<<<<<< HEAD
        if (Physics.Raycast(ray, out hit, hitDistance, layermask))
        {
            if (hit.point != null)
            {
                //if (Physics.Raycast(ray, out hit, hitDistance, LayerMask.NameToLayer("buildFloor")))
                //{
                //    InverseTransformPoint
                //    if (hit.point.y < hit.transform )
                //    RayFloor(hit);
                //}
                //else
                //{
                    prevUpdate(hit);   //자리 세팅
                    if (debugMode) Debug.DrawLine(ray.origin, hit.point, Color.green);
=======

        if (prevType == buildType.Floor || prevType == buildType.Foundation)
        {
            layerMask = (-1) - (1 << LayerMask.NameToLayer(BUILD_TEMP_LAYER) |
                                1 << LayerMask.NameToLayer(BUILD_WALL_LAYER) |
                                1 << LayerMask.NameToLayer(BUILD_OBJ_LAYER));
        }
        else if (prevType == buildType.Wall || prevType == buildType.Windowswall || prevType == buildType.Door || prevType == buildType.Cut || prevType == buildType.Beam)
        {
            layerMask = (-1) - (1 << LayerMask.NameToLayer(BUILD_TEMP_LAYER) |
                                1 << LayerMask.NameToLayer(BUILD_FLOOR_LAYER) |
                                1 << LayerMask.NameToLayer(BUILD_OBJ_LAYER));
        }
        else if (prevType == buildType.Stairs || prevType == buildType.Roof)
        {
            layerMask = (-1) - (1 << LayerMask.NameToLayer(BUILD_TEMP_LAYER) |
                                1 << LayerMask.NameToLayer(BUILD_WALL_LAYER) |
                                1 << LayerMask.NameToLayer(BUILD_FLOOR_LAYER));
        }
        else if (prevType == buildType.Anvil || prevType == buildType.Stove || prevType == buildType.Workbench)
        {
            layerMask = (-1) - (1 << LayerMask.NameToLayer(BUILD_TEMP_LAYER) |
                    1 << LayerMask.NameToLayer(BUILD_WALL_LAYER) |
                    1 << LayerMask.NameToLayer(BUILD_FLOOR_LAYER) |
                    1 << LayerMask.NameToLayer(BUILD_OBJ_LAYER) |
                    1 << LayerMask.NameToLayer(BUILD_ITEM_LAYER));
        }
        else
        {
            layerMask = (-1) - (1 << LayerMask.NameToLayer(BUILD_TEMP_LAYER) |
                    1 << LayerMask.NameToLayer(BUILD_WALL_LAYER) |
                    1 << LayerMask.NameToLayer(BUILD_FLOOR_LAYER) |
                    1 << LayerMask.NameToLayer(BUILD_OBJ_LAYER));
        }

        if (Physics.Raycast(ray, out hit, HIT_DISTANCE, layerMask))
        {
            if (hit.point != null)
            {
                prevUpdate(hit);   //자리 세팅

                //기본 부분
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default") ||
                    hit.transform.gameObject.layer == LayerMask.NameToLayer(BUILD_LAYER) ||
                    hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    IsDefaultLayer = true;
                }
                else
                {
                    IsDefaultLayer = false;
                }

                if (IsDefaultLayer)
                {
                    if (prevType == buildType.Foundation && hit.transform.gameObject.layer != LayerMask.NameToLayer("Terrain"))
                    {
                        IsBuildAct = false;
                        prevMat = buildTypeMat.Red;
                        SetPrevMat(prevType, prevMat);
                        prevObj.transform.position = hit.point;
                    }
                    else
                    {
                        if (prevDefaultInfo != null || prevDefaultInfo != default)
                        {
                            if (prevDefaultInfo.isBuildAble == false)
                            {
                                prevMat = buildTypeMat.Red;
                                IsBuildAct = false;
                                prevObj.transform.position = hit.point;
                            }
                            else
                            {
                                prevMat = buildTypeMat.Green;
                                IsBuildAct = true;
                            }
                            SetPrevMat(prevType, prevMat);
                        }
                    }
                }
                else if (!IsDefaultLayer)   //벽에 붙는 부분
                {
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
                    if (prevInfo != null || prevInfo != default)
                    {
                        if (prevInfo.isBuildAble == false)
                        {
<<<<<<< HEAD
                            prevMat = buildTypeMat.red;
                        }
                        else
                        {
                            prevMat = buildTypeMat.green;
                        }
                        SetPrevMat(prevType, 1, prevMat);
                    }
                //}
=======
                            prevMat = buildTypeMat.Red;
                            IsBuildAct = false;
                        }
                        else
                        {
                            prevMat = buildTypeMat.Green;
                            IsBuildAct = true;
                        }
                        SetPrevMat(prevType, prevMat);
                    }
                }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            }
        }
        else
        {
<<<<<<< HEAD
            if (prevMat != buildTypeMat.red)
            {
                prevMat = buildTypeMat.red;
                SetPrevMat(prevType, 1, prevMat);
            }
            prevObj.transform.position = ray.direction * hitDistance;
            if (debugMode) Debug.DrawLine(ray.origin, ray.direction * hitDistance, Color.red);
=======
            if (prevMat != buildTypeMat.Red)
            {
                IsBuildAct = false;
                prevMat = buildTypeMat.Red;
                SetPrevMat(prevType, prevMat);
            }
            prevObj.transform.position = ray.direction * HIT_DISTANCE;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        }
    }

    private void prevUpdate(RaycastHit hit2)
    {
        if (prevObj != null || prevObj != default)
        {
<<<<<<< HEAD
            //Legacy
            // prevPos = hit2.point;
            // prevPos -= Vector3.one * offset;
            // prevPos /= gridSize;
            // prevPos = new Vector3(Mathf.Round(prevPos.x), Mathf.Round(prevPos.y), Mathf.Round(prevPos.z));
            // prevPos *= gridSize;
            // prevPos += Vector3.one * offset;
            // prevObj.transform.position = prevPos;

            prevPos = hit2.point;
            prevPos /= gridSize;
            prevPos = new Vector3(Mathf.Round(prevPos.x),Mathf.Round(prevPos.y),Mathf.Round(prevPos.z));
            prevPos *= gridSize;
            prevObj.transform.position = prevPos;

            prevRot = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y + prevYAngle, 0);
            prevObj.transform.rotation = Quaternion.Euler(prevRot);

            if (prevInfo != null || prevInfo != default)
            {
                prevInfo.setMid(prevPos);
=======
            if (hit2.transform.gameObject.GetComponent<CheckTrigger>() != null)
            {
                if (hit2.transform.gameObject.GetComponent<CheckTrigger>().IsOnCollider == true)
                {
                    if (hit2.transform.gameObject.layer == LayerMask.NameToLayer(BUILD_FLOOR_LAYER) ||
                        hit2.transform.gameObject.layer == LayerMask.NameToLayer(BUILD_WALL_LAYER) ||
                        hit2.transform.gameObject.layer == LayerMask.NameToLayer(BUILD_OBJ_LAYER))
                    {
                        if (prevType == buildType.Foundation)
                        {
                            prevPos = hit2.transform.position + new Vector3(0, -0.5f, 0);
                            prevObj.transform.position = prevPos;
                            prevRot = new Vector3(0, hit2.transform.rotation.eulerAngles.y + prevYAngle, 0);
                            prevObj.transform.rotation = Quaternion.Euler(prevRot);
                        }
                        else if (prevType == buildType.Wall || prevType == buildType.Windowswall || prevType == buildType.Door)
                        {
                            string temp = hit2.transform.name;
                            if (temp.Equals("LeftBot") || temp.Equals("LeftTop") || temp.Equals("RightTop") || temp.Equals("RightBot")) { /* Do nothing */ }
                            else
                            {
                                prevPos = hit2.transform.position;
                                prevObj.transform.position = prevPos;
                                prevRot = new Vector3(0, hit2.transform.rotation.eulerAngles.y + prevYAngle, 0);
                                prevObj.transform.rotation = Quaternion.Euler(prevRot);
                            }
                        }
                        else
                        {
                            prevPos = hit2.transform.position;
                            prevObj.transform.position = prevPos;
                            prevRot = new Vector3(0, hit2.transform.rotation.eulerAngles.y + prevYAngle, 0);
                            prevObj.transform.rotation = Quaternion.Euler(prevRot);
                        }
                    }
                    else
                    {
                        prevPos = hit2.point;
                        prevPos /= gridSize;
                        prevPos = new Vector3(Mathf.Round(prevPos.x), Mathf.Round(prevPos.y), Mathf.Round(prevPos.z));
                        prevPos *= gridSize;
                        prevObj.transform.position = prevPos;

                        prevRot = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y + prevYAngle, 0);
                        prevObj.transform.rotation = Quaternion.Euler(prevRot);
                    }
                }
            }
            else
            {
                prevPos = hit2.point;
                prevPos /= gridSize;
                prevPos = new Vector3(Mathf.Round(prevPos.x), Mathf.Round(prevPos.y), Mathf.Round(prevPos.z));
                prevPos *= gridSize;
                prevObj.transform.position = prevPos;

                prevRot = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y + prevYAngle, 0);
                prevObj.transform.rotation = Quaternion.Euler(prevRot);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            }
        }
    }

    private void D_prevObj()
    {
        if (prevObj != null || prevObj != default)
            Destroy(prevObj);
    }

<<<<<<< HEAD
    private void C_prevObj(buildType buildtype, int type)
    {
        if (type == 0)
        {
            prevObj = Instantiate(buildObjs[(int)buildtype]);
            prevObj.layer = LayerMask.NameToLayer("build");
            SetPrevMat(buildtype, 0, buildTypeMat.none);
            prevObj.GetComponent<MeshCollider>().convex = true;
            prevObj.GetComponent<MeshCollider>().isTrigger = false;
        }
        else if (type == 1)
        {
            prevObj = Instantiate(buildObjs[(int)buildtype]);
            prevObj.name = "prevObj";
            SetLayer();
            prevObj.transform.GetChild(0).GetComponent<MeshCollider>().convex = true;
            prevObj.transform.GetChild(0).GetComponent<MeshCollider>().isTrigger = true;
            SetPrevMat(buildtype, 1, buildTypeMat.green);
            prevInfo = prevObj.FindChildObj("BuildCollider").GetComponent<PrevObjInfo>();
            //prevInfo.SetType(buildtype, prevPos);
            prevYAngle = 0.0f;
=======
    private void C_prevObj(buildType buildtype)
    {
        if (prevObj == null || prevObj == default)
        {
            prevObj = Instantiate(BuildLoadObjs[(int)buildtype]);
            prevObj.transform.parent = this.transform;
            prevObj.name = "prevObj";

            SetLayer();
            SetTrigger(buildtype);
            SetPrevMat(buildtype, buildTypeMat.Green);
            prevInfo = prevObj.FindChildObj("BuildCollider").GetComponent<PrevObjInfo>();
            prevDefaultInfo = prevObj.FindChildObj("BuildDefaultCollider").GetComponent<PrevObjInfo>();
            prevInfo.SetLayerType(buildtype);
            prevDefaultInfo.SetLayerType(buildtype);
            prevYAngle = 0.0f;

            IsResetCall = false;
            IsBuildAct = false;
        }
    }

    private void SetTrigger(buildType buildtemp)
    {
        switch (buildtemp)
        {
            case buildType.Foundation:
                for (int i = 0; i < 5; i++)
                {
                    prevObj.transform.GetChild(i).GetComponent<MeshCollider>().convex = true;
                    prevObj.transform.GetChild(i).GetComponent<MeshCollider>().isTrigger = true;
                }
                break;
            case buildType.Anvil:
            case buildType.Stove:
            case buildType.Workbench:
                prevObj.transform.GetComponent<MeshCollider>().convex = true;
                prevObj.transform.GetComponent<MeshCollider>().isTrigger = true;
                break;
            case buildType.Door:
                prevObj.transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;
                prevObj.transform.GetChild(0).GetChild(0).GetComponent<MeshCollider>().isTrigger = true;
                break;
            default:
                prevObj.transform.GetChild(0).GetComponent<MeshCollider>().convex = true;
                prevObj.transform.GetChild(0).GetComponent<MeshCollider>().isTrigger = true;
                break;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        }
    }

    private void SetLayer()
    {
<<<<<<< HEAD
        prevObj.layer = LayerMask.NameToLayer("buildThings");
=======
        prevObj.layer = LayerMask.NameToLayer(BUILD_TEMP_LAYER);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

        if (prevObj.transform.childCount > 0)
        {
            Transform[] allChildren = prevObj.GetComponentsInChildren<Transform>();

<<<<<<< HEAD
            foreach(Transform child in allChildren)
            {
                child.gameObject.layer = LayerMask.NameToLayer("buildThings");
=======
            foreach (Transform child in allChildren)
            {
                child.gameObject.layer = LayerMask.NameToLayer(BUILD_TEMP_LAYER);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            }
        }
    }

    private void BuildObj()
    {
<<<<<<< HEAD
        if (prevInfo != null && prevInfo != default && prevInfo.isBuildAble == true)
        {
            GameObject buildObj = Instantiate(buildObjs[(int)prevType],prevPos,Quaternion.Euler(prevRot));
            //buildObj.layer = LayerMask.NameToLayer("build");
            buildObj.layer = 0;
            buildObj.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(GData.BUILD_MASK);
            buildObj.FindChildObj("BuildPart").SetActive(false);
        }
    }

    public void SetPrevMat(buildType buildtemp,int type, buildTypeMat mat)
    {
        if (prevObj != null || prevObj != default)
        {
            if (type == 0)
            {
                switch (buildtemp)
                {
                    case buildType.door:
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.none];
                            prevObj.FindChildObj("Door").GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.none];
                        break;
                    case buildType.windowswall:
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.none];
                            prevObj.FindChildObj("Glass").GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.GlassNone];
                        break;
                    default:
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.none];
                        break;
                }
            }
            else if (type == 1)
            {
                switch (buildtemp)
                {
                    case buildType.door:
                        if (mat == buildTypeMat.green)
                        {
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.green];
                            prevObj.FindChildObj("Door").GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.green];
                        }
                        else if (mat == buildTypeMat.red)
                        {
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.red];
                            prevObj.FindChildObj("Door").GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.red];
                        }
                        break;
                    case buildType.windowswall:
                        if (mat == buildTypeMat.green)
                        {
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.green];
                            prevObj.FindChildObj("Glass").GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.GlassGreen];
                        }
                        else if (mat == buildTypeMat.red)
                        {
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.red];
                            prevObj.FindChildObj("Glass").GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.red];
                        }
                        break;
                    default:
                        if (mat == buildTypeMat.green)
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.green];
                        else if (mat == buildTypeMat.red)
                            prevObj.transform.GetChild(0).GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.red];
                        break;
                }
=======
        GameObject buildObj = default;

        if (!IsDefaultLayer)
        {
            if (prevInfo != null && prevInfo != default && IsBuildAct == true)
            {
                buildObj = Instantiate(BuildLoadObjs[(int)prevType], prevPos, Quaternion.Euler(prevRot));
                buildObj.transform.parent = this.transform;

                switch (prevType)
                {
                    case buildType.Anvil:
                        buildObj.tag = "Anvil";
                        break;
                    case buildType.Stove:
                        buildObj.tag = "Stove";
                        break;
                    case buildType.Workbench:
                        buildObj.tag = "Workbench";
                    break;
                    default:
                        buildObj.transform.GetChild(0).tag = "Gather";
                        break;
                }

                switch (prevType)
                {
                    case buildType.Anvil:
                    case buildType.Stove:
                    case buildType.Workbench:
                        buildObj.AddComponent<NavMeshObstacle>();

                        buildObj.layer = LayerMask.NameToLayer(BUILD_ITEM_LAYER);
                        if (buildObj.transform.childCount > 0)
                        {
                            Transform[] allChildren = buildObj.GetComponentsInChildren<Transform>();
                            foreach (Transform child in allChildren)
                            {
                                child.gameObject.layer = LayerMask.NameToLayer(BUILD_ITEM_LAYER);
                            }
                        }

                        break;
                    default:
                        buildObj.transform.GetChild(0).gameObject.AddComponent<NavMeshObstacle>();

                        buildObj.layer = 0;
                        buildObj.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(GData.BUILD_MASK);
                        break;
                }

                switch (prevType)
                {
                    case buildType.Foundation:
                        for (int i = 0; i < 5; i++)
                        {
                            buildObj.transform.GetChild(i).GetComponent<MeshCollider>().isTrigger = false;
                        }
                        break;
                    case buildType.Door: /* Do nothing */
                        break;
                    case buildType.Anvil:
                    case buildType.Stove:
                    case buildType.Workbench:
                        buildObj.transform.GetComponent<MeshCollider>().isTrigger = false;
                        break;
                    default:
                        buildObj.transform.GetChild(0).GetComponent<MeshCollider>().isTrigger = false;
                        break;
                }
            }
        }
        else if (IsDefaultLayer)
        {
            if (prevDefaultInfo != null && prevDefaultInfo != default && IsBuildAct == true)
            {
                buildObj = Instantiate(BuildLoadObjs[(int)prevType], prevPos, Quaternion.Euler(prevRot));
                buildObj.transform.parent = this.transform;

                switch (prevType)
                {
                    case buildType.Anvil:
                        buildObj.tag = "Anvil";
                        break;
                    case buildType.Stove:
                        buildObj.tag = "Stove";
                        break;
                    case buildType.Workbench:
                        buildObj.tag = "Workbench";
                    break;
                    default:
                        buildObj.transform.GetChild(0).tag = "Gather";
                        break;
                }

                switch (prevType)
                {
                    case buildType.Anvil:
                    case buildType.Stove:
                    case buildType.Workbench:
                        buildObj.AddComponent<NavMeshObstacle>();
                        break;
                    default:
                        buildObj.transform.GetChild(0).gameObject.AddComponent<NavMeshObstacle>();
                        break;
                }

                switch (prevType)
                {
                    case buildType.Anvil:
                    case buildType.Stove:
                    case buildType.Workbench:
                        buildObj.layer = LayerMask.NameToLayer(BUILD_ITEM_LAYER);

                        if (buildObj.transform.childCount > 0)
                        {
                            Transform[] allChildren = buildObj.GetComponentsInChildren<Transform>();

                            foreach (Transform child in allChildren)
                            {
                                child.gameObject.layer = LayerMask.NameToLayer(BUILD_ITEM_LAYER);
                            }
                        }

                        break;
                    default:
                        buildObj.layer = 0;
                        buildObj.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(GData.BUILD_MASK);
                        break;
                }

                switch (prevType)
                {
                    case buildType.Foundation:
                        for (int i = 0; i < 5; i++)
                        {
                            buildObj.transform.GetChild(i).GetComponent<MeshCollider>().isTrigger = false;
                        }
                        break;
                    case buildType.Door: /* Do nothing */
                        break;
                    case buildType.Anvil:
                    case buildType.Stove:
                    case buildType.Workbench:
                        buildObj.transform.GetComponent<MeshCollider>().isTrigger = false;
                        break;
                    default:
                        buildObj.transform.GetChild(0).GetComponent<MeshCollider>().isTrigger = false;
                        break;
                }
            }
        }

        if (buildObj != null || buildObj != default)
        {
            buildObj.name = prevName + buildObjNums;
            buildObjNums++;
            BuildingList.Add(buildObj);
        }
    }

    public void SetPrevMat(buildType buildtemp, buildTypeMat mat)
    {
        if (prevObj != null || prevObj != default)
        {
            switch (buildtemp)
            {
                case buildType.Door:
                    if (mat == buildTypeMat.Green)
                    {
                        prevObj.transform.GetChild(0).GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Green];
                        prevObj.FindChildObj("Door").GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Green];
                    }
                    else if (mat == buildTypeMat.Red)
                    {
                        prevObj.transform.GetChild(0).GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Red];
                        prevObj.FindChildObj("Door").GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Red];
                    }
                    break;
                case buildType.Windowswall:
                    if (mat == buildTypeMat.Green)
                    {
                        prevObj.transform.GetChild(0).GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Green];
                        prevObj.FindChildObj("Glass").GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.GlassGreen];
                    }
                    else if (mat == buildTypeMat.Red)
                    {
                        prevObj.transform.GetChild(0).GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Red];
                        prevObj.FindChildObj("Glass").GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Red];
                    }
                    break;
                case buildType.Foundation:
                    if (mat == buildTypeMat.Green)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            prevObj.transform.GetChild(i).GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Green];
                        }
                    }
                    else if (mat == buildTypeMat.Red)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            prevObj.transform.GetChild(i).GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Red];
                        }
                    }
                    break;
                case buildType.Anvil:
                    if (mat == buildTypeMat.Green)
                        prevObj.GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.VikingGreen];
                    else if (mat == buildTypeMat.Red)
                        prevObj.GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.VikingRed];
                    break;
                case buildType.Workbench:
                case buildType.Stove:
                    if (mat == buildTypeMat.Green)
                        prevObj.GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Green];
                    else if (mat == buildTypeMat.Red)
                        prevObj.GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Red];
                    break;
                default:
                    if (mat == buildTypeMat.Green)
                        prevObj.transform.GetChild(0).GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Green];
                    else if (mat == buildTypeMat.Red)
                        prevObj.transform.GetChild(0).GetComponent<Renderer>().material = BuildLoadMats[(int)buildTypeMat.Red];
                    break;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            }
        }
    }

<<<<<<< HEAD
    public Vector3 SetPrevOffset(buildType buildtemp)
    {
        Vector3 Result = default;

        switch (buildtemp)
        {
            case buildType.beam:
                Result = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case buildType.cut:
                Result = new Vector3(12.5f, 0.0f, 0.0f);
                break;
            case buildType.door:
                Result = new Vector3(12.5f, 0.0f, 0.0f);
                break;
            case buildType.floor:
                //Result = new Vector3(14.0f, 1.0f, -14.0f);
                Result = new Vector3(12.5f, 1.0f, 12.5f);
                break;
            case buildType.roof:
                Result = new Vector3(12.5f, 0.0f, 12.5f);
                break;
            case buildType.stairs:
                Result = new Vector3(14.0f, 0.0f, 0.0f);
                break;
            case buildType.wall:
                Result = new Vector3(12.5f, 0.0f, 0.0f);
                break;
            case buildType.windowswall:
                Result = new Vector3(12.5f, 0.0f, 0.0f);
                break;
        }
        return Result;
=======
    public void FindBuildObj(GameObject Obj)
    {
        if(Obj.tag == "Anvil" || Obj.tag == "Stove" || Obj.tag == "Workbench")
        {
            FindAndDestory(Obj.name);
        }
        else
        {
            FindAndDestory(Obj.transform.parent.name);
        }
    }

    public void FindAndDestory(string ObjName)
    {
        for (int i = 0; i < BuildingList.Count; i++)
        {
            if (BuildingList[i].name.Equals(ObjName))
            {
                Destroy(BuildingList[i]);
                BuildingList.RemoveAt(i);
                break;
            }
        }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    }
}

public enum buildType
{
<<<<<<< HEAD
    none = -1, beam, cut, door, floor, roof, stairs, wall, windowswall
=======
    None = -1, Beam, Cut, Door, Floor, Roof, Stairs, Wall, Windowswall, Foundation, Anvil, Stove, Workbench
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
}

public enum buildTypeMat
{
<<<<<<< HEAD
    none, green, red, GlassNone, GlassGreen, GlassRed
=======
    None, Green, Red, GlassNone, GlassGreen, GlassRed, VikingNone, VikingGreen, VikingRed
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
}