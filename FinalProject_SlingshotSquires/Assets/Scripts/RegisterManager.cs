using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterManager : MonoBehaviour
{
    public GameObject shopUI;
    public ShopManagerScript shop;
    private void OnCollisionEnter2D(Collision2D other)
    {
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
