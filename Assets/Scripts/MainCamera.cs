using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float smothTime = 0.5f;

    Transform target;
    float topLX, topLY, botRX, botRY;
    Vector2 velocity;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //NORMAL-------------------------
        //transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);

        //CON LIMITES-------------------
        //transform.position = new Vector3(Mathf.Clamp(target.position.x,topLX,botRX), Mathf.Clamp(target.position.y, botRY, topLY),-10);

        //CON SUAVIZADO-----------------
        float posX = Mathf.Round(Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, smothTime) * 100) / 100; //tecnica que asegura redondear
        float posY = Mathf.Round(Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, smothTime) * 100) / 100;
        transform.position = new Vector3(posX, posY, -10);
    }

    public void SetBound(GameObject map)
    {
        Tiled2Unity.TiledMap config = map.GetComponent<Tiled2Unity.TiledMap>();
        float cameraSize = Camera.main.orthographicSize;

        topLX = map.transform.position.x + cameraSize;
        topLY = map.transform.position.y - cameraSize;
        botRX = map.transform.position.x + config.NumTilesWide - cameraSize;
        botRY = map.transform.position.y - config.NumTilesHigh + cameraSize;
    }

    //public void FastMove()
    //{
    //    transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
    //}
}
