using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaEnter : MonoBehaviour
{
    private bool isBossSpwan = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GData.PLAYER_MASK && isBossSpwan == false)
        {
            GameManager.Instance.BossSpwan();
            isBossSpwan = true;
        }
    } // OnTriggerEnter
}
