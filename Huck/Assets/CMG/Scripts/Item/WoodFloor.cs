using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodFloor : Item
{
    protected override void ItemUse(ItemSlot itemslot_)
    {
        itemslot_.itemAmount--;
    }
}