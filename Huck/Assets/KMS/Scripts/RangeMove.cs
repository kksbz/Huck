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
        // Follow camera
        gameObject.transform.position = camera_1p.transform.position;
        gameObject.transform.rotation = camera_1p.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GData.ENEMY_MASK)
        {
            DamageMessage dm = new DamageMessage(transform.parent.gameObject, 10f);
            other.gameObject.GetComponent<IDamageable>().TakeDamage(dm);
        }
    }
}
