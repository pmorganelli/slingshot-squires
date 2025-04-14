using UnityEngine;

public class CropSlotManager : MonoBehaviour
{
    public Transform[] attackSlots; // Set in Inspector
    private bool[] slotTaken;

    void Awake()
    {
        slotTaken = new bool[attackSlots.Length];
    }

    public Transform ClaimSlot(out int index)
    {
        for (int i = 0; i < attackSlots.Length; i++)
        {
            if (!slotTaken[i])
            {
                slotTaken[i] = true;
                index = i;
                return attackSlots[i];
            }
        }

        index = -1;
        return null;
    }

    public void ReleaseSlot(int index)
    {
        if (index >= 0 && index < slotTaken.Length)
        {
            slotTaken[index] = false;
        }
    }
}
