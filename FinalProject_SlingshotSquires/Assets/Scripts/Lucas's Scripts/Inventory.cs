using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public CollectableType type;
        public int             count;
        public int             maxAllowed;
        public Sprite          icon;

        public Slot()
        {
            type       = CollectableType.NONE;
            count      = 0;
            maxAllowed = 99;
        }

        public bool CanAddItem() => count < maxAllowed;

        public void AddItem(Collectable item)
        {
            type = item.type;
            icon = item.icon;
            count++;
        }
    }

    public List<Slot> slots = new List<Slot>();

    public Inventory(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
            slots.Add(new Slot());
    }

    //-----------------------------------------------------------
    // 1. For world pick-ups (unchanged)
    //-----------------------------------------------------------
    public void Add(Collectable item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.type == item.type && slot.CanAddItem())
            {
                slot.AddItem(item);
                return;
            }
        }
        foreach (Slot slot in slots)
        {
            if (slot.type == CollectableType.NONE)
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    //-----------------------------------------------------------
    // 2. For shop purchases (NEW convenience overload)
    //-----------------------------------------------------------
    public void Add(CollectableType type, Sprite icon)
    {
        foreach (Slot slot in slots)
        {
            if (slot.type == type && slot.CanAddItem())
            {
                slot.count++;
                return;
            }
        }
        foreach (Slot slot in slots)
        {
            if (slot.type == CollectableType.NONE)
            {
                slot.type  = type;
                slot.icon  = icon;
                slot.count = 1;
                return;
            }
        }
    }
}
