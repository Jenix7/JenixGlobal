using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions; //Te avisa

public class Warp : MonoBehaviour
{
    public GameObject target;
    public GameObject targetMap;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        Assert.IsNotNull(targetMap);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position = target.transform.GetChild(0).transform.position;

            Camera.main.transform.GetComponent<CameraFollow>().SetBound(targetMap);
        }
    }
   
}
