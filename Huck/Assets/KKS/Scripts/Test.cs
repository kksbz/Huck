using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.Video;

public class Test : MonoBehaviour
{
    public GameObject targetObj;
    public Rigidbody rb;
    public Vector3 startPos;
    public float timer;
    public float h;
    public bool isJump = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask(GData.TERRAIN_MASK));
        if (hits.Length > 0 && isJump == true)
        {
            rb.isKinematic = true;
            isJump = false;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            isJump = true;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            Vector3 a = CalculateBestThrowSpeed(transform.position, targetObj.transform.position, 2f);
            Debug.Log($"{a}");
            rb.AddForce(a, ForceMode.Impulse);
        }
    }
    static Vector3 CalculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
    {
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;
        float v0y = y / timeToTarget + 0.5f * Physics.gravity.magnitude * timeToTarget;
        float v0xz = xz / timeToTarget;
        Vector3 result = toTargetXZ.normalized;
        result *= v0xz;
        result.y = v0y;
        return result;
=======

public class Test : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.SpawnMonster();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.Instance.count = 4;
            GameManager.Instance.SpawnMonster();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameManager.Instance.BossSpwan();
        }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    }
}
