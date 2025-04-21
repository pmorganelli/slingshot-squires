using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class registerHandler : MonoBehaviour
{
    public void openStore () 
    {
        SceneManager.LoadScene("ShopScene");
    }
}
