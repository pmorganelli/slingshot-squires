using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    
    // AudioSource for playing sound effects
    public AudioSource audioSource;
    
    // AudioClip for when opening the inventory menu
    public AudioClip openSound;
    
    // AudioClip for when closing the inventory menu
    public AudioClip closeSound;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if(!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);
            // Play open sound effect when opening the inventory
            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }
        }
        else
        {
            inventoryPanel.SetActive(false);
            // Play close sound effect when closing the inventory
            if (audioSource != null && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
            }
        }
    }
}
