using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class RegisterManager : MonoBehaviour
{
    public GameObject shopUI;
    public ShopManagerScript shop;
    private void OnCollisionEnter2D(Collision2D other)
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
        Debug.Log("ENTERING");
        shopUI.SetActive(true);
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("EXITING");
        shop.back();
        shopUI.SetActive(false);
    }
}
