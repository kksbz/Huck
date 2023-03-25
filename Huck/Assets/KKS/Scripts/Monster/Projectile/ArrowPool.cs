using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab = default;
    [SerializeField] private Transform arrowPos = default;
    private Queue<GameObject> arrowPool = new Queue<GameObject>();
    private Vector3 dir = default;
    private static ArrowPool instance = default;
    public static ArrowPool Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SetupArrowPool();
    } // Start

    //! ArrowPool 채우는 함수
    private void SetupArrowPool()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
            arrowPool.Enqueue(arrow);
            arrow.SetActive(false);
        }
    } // SetupArrowPool

    //! ArrowPool에서 화살 한발 가져오는 함수
    public GameObject GetArrow()
    {
        GameObject arrow = default;
        if (arrowPool.Count > 0)
        {
            arrow = arrowPool.Dequeue();
        }
        else
        {
            // ArrowPool에 화살이 0개 이하일 때 새로 한발 생성
            arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
            arrow.SetActive(false);
        }
        return arrow;
    } // GetArrow

    //! 발사한 화살 회수하는 함수
    public void ReturnArrow(GameObject _arrow)
    {
        arrowPool.Enqueue(_arrow);
        _arrow.SetActive(false);
    } // ReturnArrow
} // ArrowPool
