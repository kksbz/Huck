using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class RangeMove : MonoBehaviour
{
    public GameObject camera_1p = default;
    void Start()
    {
        
=======
public class RangeMove : MonoBehaviour, IDamageable
{
    private GameObject camera_1p = default;
    private InHand playerInHand = default;
    private PlayerStat playerStat = default;
    private ResourceObjectSO BRO = default;

    void Start()
    {
        camera_1p = Camera.main.gameObject;
        playerInHand = transform.parent.GetComponent<InHand>();
        playerStat = transform.parent.GetComponent<PlayerStat>();
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    }

    void Update()
    {
<<<<<<< HEAD
        gameObject.transform.position = camera_1p.transform.position;
        gameObject.transform.rotation = camera_1p.transform.rotation;
    }
=======
        // Follow camera
        gameObject.transform.position = camera_1p.transform.position;
        gameObject.transform.rotation = camera_1p.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemData item_ = playerInHand.inventorySlotItem[playerInHand.selectedQuitSlot].itemData;

        DamageMessage dm = new DamageMessage(transform.parent.gameObject, playerStat.damage, item_);
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(dm);
        }
    }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
}
