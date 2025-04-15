using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleText : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;

    void Start()
    {
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
    }
}
