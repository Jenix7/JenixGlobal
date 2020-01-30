using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    public float speed = 2f;
    public GameObject initialMap;
    public GameObject slashPrefab;

    Animator anim;
    Rigidbody2D rb;
    CircleCollider2D attackCol;
    Vector2 mov;
    bool movePrevent;
    Aura aura;

    private void Awake()
    {
        Assert.IsNotNull(initialMap);
        Assert.IsNotNull(slashPrefab);
    }

    void Start()
    {
        //DECLARAR
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        aura = transform.GetChild(1).GetComponent<Aura>();

        //collider de ataque
        attackCol = transform.GetChild(0).GetComponent<CircleCollider2D>();
        attackCol.enabled = false;

        //BoundsMapa
        Camera.main.transform.GetComponent<MainCamera>().SetBound(initialMap);
    }

    void Update()
    {
        Movements();
        SwordAttack();
        SlashAttack();
        PreventMovement();
    }

    private void FixedUpdate()
    {
        //Movimiento DENTRO Rigidbody
        rb.MovePosition(rb.position + mov * speed * Time.deltaTime);
    }

    void Movements()
    {
        //Movimento FUERA Rigidbody-----------------------------------------------------------------------------------------------------------------
        //Vector3 mov = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + mov, speed * Time.deltaTime);
        //------------------------------------------------------------------------------------------------------------------------------------------

        mov = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (mov != Vector2.zero)
        {
            anim.SetFloat("movX", mov.x);
            anim.SetFloat("movY", mov.y);

            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    void SwordAttack()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool attacking = stateInfo.IsName("Player_Attack");

        if (Input.GetKeyDown("space") && !attacking)
        {
            anim.SetTrigger("attacking");
        }

        //Colider de ataque
        if (mov != Vector2.zero) { attackCol.offset = new Vector2(mov.x / 2, mov.y / 2); }
        if (attacking)
        {
            float playbackTime = stateInfo.normalizedTime; // tiempo que tarda en hacer ataque
            if (playbackTime > 0.33 && playbackTime < 0.66) attackCol.enabled = true;
            else attackCol.enabled = false;
        }
    }

    void SlashAttack()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool loading = stateInfo.IsName("Player_Slash");

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {  anim.SetTrigger("loading");

            //Empieza AURA
            aura.AuraStart();
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            anim.SetTrigger("attacking");

            if (aura.IsLoaded())
            {
                //Calcular angulo
                float angle = Mathf.Atan2(anim.GetFloat("movY"), anim.GetFloat("movX")) * Mathf.Rad2Deg;
                //Instanciar
                GameObject slashObj = Instantiate(slashPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
                //Movimiento inicial
                Slash slash = slashObj.GetComponent<Slash>();
                slash.mov.x = anim.GetFloat("movX");
                slash.mov.y = anim.GetFloat("movY");
            }
            aura.AuraStop();
            
            //Esperar un momento i reaccivar movimiento
            StartCoroutine(EnableMovementAfter(0.4f));
        }

        //Bloquea movimiento cuando loading
        if (loading) { movePrevent = true; }
        
    }

    void PreventMovement()
    {
        if (movePrevent) mov = Vector2.zero;
    }

    IEnumerator EnableMovementAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        movePrevent = false;
    }
}