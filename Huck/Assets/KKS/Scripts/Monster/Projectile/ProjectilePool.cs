using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projecttilePrefab = default;
    private Queue<GameObject> projecttilePool = new Queue<GameObject>();
    private static ProjectilePool instance = default;
    public static ProjectilePool Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SetupProjecttilePool();
    } // Start

    private void OnDisable()
    {
        // ��Ȱ��ȭ�� �� arrowPool�� ����� ����ü ����
        foreach (var _projecttile in projecttilePool)
        {
            Destroy(_projecttile);
        }
    } // OnDisable

    //! ProjecttilePool ä��� �Լ�
    private void SetupProjecttilePool()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject projecttile = Instantiate(projecttilePrefab, Vector3.zero, Quaternion.identity);
            projecttilePool.Enqueue(projecttile);
            projecttile.SetActive(false);
        }
    } // SetupProjecttilePool

    //! ArrowPool���� ȭ�� �ѹ� �������� �Լ�
    public GameObject GetProjecttile()
    {
        GameObject projecttile = default;
        if (projecttilePool.Count > 0)
        {
            projecttile = projecttilePool.Dequeue();
        }
        else
        {
            // ProjecttilePool�� ����ü�� �������� ���� �� ���� ����
            projecttile = Instantiate(projecttilePrefab, Vector3.zero, Quaternion.identity);
            projecttile.SetActive(false);
        }
        return projecttile;
    } // GetArrow

    //! �߻��� ����ü ȸ���ϴ� �Լ�
    public void EnqueueProjecttile(GameObject _projecttile)
    {
        projecttilePool.Enqueue(_projecttile);
        _projecttile.SetActive(false);
    } // ReturnArrow
} // ProjectilePool