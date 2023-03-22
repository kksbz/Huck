using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData = null;
    public int itemCount = 3;


    public virtual void OnUseItem()
    {

    }
}