using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;
    Vector2[] bounds = new Vector2[4];

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (CheckPosition())
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        }
    }

    public void SetBound(GameObject map)
    {
        Tiled2Unity.TiledMap til = map.GetComponent<Tiled2Unity.TiledMap>();

        Vector2 p1 = map.transform.position;
        Vector2 p2 = new Vector2(p1.x + til.NumTilesWide, p1.y);
        Vector2 p3 = new Vector2(p1.x, p1.y - til.NumTilesHigh);
        Vector2 p4 = new Vector2(p3.x + til.NumTilesWide, p3.y);

        bounds[0] = p1;
        bounds[1] = p2;
        bounds[2] = p3;
        bounds[3] = p4;
    }

    private bool CheckPosition()
    {
        float cameraSize = Camera.main.orthographicSize;
        
        foreach (Vector2 vec in bounds)
        {
            if (Vector2.Distance(target.transform.position, vec) < 5)
            {  return false;}
        }

        Debug.Log("232323");
        return true;
        
    }
}

//Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);