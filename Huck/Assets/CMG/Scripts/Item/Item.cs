using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData = null;
<<<<<<< HEAD
    public int itemCount = 3;
}
=======
    public int itemCount = 1;

    public void OnUse(ItemSlot itemSlot_)
    {
        ItemUse(itemSlot_);
    }

    protected virtual void ItemUse(ItemSlot itemSlot_)
    {
        if (itemSlot_.itemData.ItemUseAble)
        {
            itemSlot_.itemAmount--;
        }
    }
}
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
