using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limits : MonoBehaviour
{
    //Esquinas
    Vector2 p1, p2, p3, p4;
    void Start()
    {
        Tiled2Unity.TiledMap til = GetComponent<Tiled2Unity.TiledMap>();

        p1 = transform.position;
        p2 = new Vector2(p1.x + til.NumTilesWide, p1.y);
        p3 = new Vector2(p1.x, p1.y - til.NumTilesHigh);
        p4 = new Vector2(p3.x + til.NumTilesWide, p3.y);

    }


    void Update()
    {
        
    }
}
