using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMove : MonoBehaviour
{
    public GameObject camera_1p = default;
    void Start()
    {

    }

    void Update()
    {
        gameObject.transform.position = camera_1p.transform.position;
        gameObject.transform.rotation = camera_1p.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GData.ENEMY_MASK)
        {
            other.GetComponent<IDamageable>().TakeDamage(transform.parent.gameObject, 10);
        }
    }
}
