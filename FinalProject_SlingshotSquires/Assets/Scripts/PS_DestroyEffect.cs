using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_DestroyEffect : MonoBehaviour
{
    public float destroyTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyMe());
    }

    // Update is called once per frame
    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
