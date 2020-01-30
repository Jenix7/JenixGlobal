using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [Tooltip("Velocidad de movimiento en unidades del mundo")]
    public float speed;

    GameObject player;
    Rigidbody2D rb2d;
    Vector3 target, dir;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();

        if(player != null)
        {
            target = player.transform.position;
            dir = (target - transform.position).normalized;
        }
    }

    void FixedUpdate()
    {
        if (target != Vector3.zero)
        {
            rb2d.MovePosition(transform.position + (dir*speed) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player" || collision.transform.tag == "Attack")
        {
            Destroy(gameObject);
        }
    }

    //si sale de la pantalla-------------------------------------------------------------------
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
