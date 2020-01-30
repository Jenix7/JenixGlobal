using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float visionRadius;
    public float attackRadius;
    public float speed;

    //variablesAtaque
    public GameObject rockPrefab;
    public float attackSpeed = 2f;
    bool attacking;

    GameObject player;

    Vector3 initialPosition, target;

    Animator anim;
    Rigidbody2D rb2d;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        initialPosition = transform.position;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        //se queda donde esta
        target = initialPosition;

        //Raycast enemigo a jugador
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            player.transform.position - transform.position, 
            visionRadius, 
            1 << LayerMask.NameToLayer("Default"));
                                       //Poner Enemy en layer Enemy(distinta a default)
                                       //Poner attack y slash en layer Attack

        //Dibujar Raycast
        Vector3 forward = transform.TransformDirection(player.transform.position-transform.position);
        Debug.DrawRay(transform.position,forward,Color.red);

        //si encuentra player, player es el target
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                target = player.transform.position;
            }
        }

        //Calcular distancia i direccion
        float distance = Vector3.Distance(target, transform.position);
        Vector3 dir = (target - transform.position).normalized;

        //Esta en rango ataque
        if (target != initialPosition && distance < attackRadius)
        {
            anim.SetFloat("movX",dir.x);
            anim.SetFloat("movY",dir.y);
            anim.Play("Enemy_Walk",-1,0); //congle animacion de andar (cando nos toca)

            if (!attacking) StartCoroutine(Attack(attackSpeed));
        }

        //Esta en rango vision
        else
        {
            rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);

            anim.speed = 1;
            anim.SetFloat("movX", dir.x);
            anim.SetFloat("movY", dir.y);
            anim.SetBool("walking",true);
        }

        //Vuelve a su sitio
        if (target == initialPosition && distance < 0.02f)
        {
            transform.position = initialPosition;
            anim.SetBool("walking",false);
        }

        Debug.DrawLine(transform.position, target, Color.green);
    }

    //Dibuja sobre escena Radios
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    IEnumerator Attack(float seconds)
    {
        attacking = true;
        if (target != initialPosition && rockPrefab != null)
        {
            Instantiate(rockPrefab,transform.position,transform.rotation);
            yield return new WaitForSeconds(seconds);
        }
        attacking = false;
    }
}
