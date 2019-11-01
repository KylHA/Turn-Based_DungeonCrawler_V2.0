using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script Controls the Player Movement And Encounter with enemy Factors
public class PlayerController : MonoBehaviour
{
    BoxCollider2D m_Collider;
    UnitStats unitstats;
    private Rigidbody2D rb2d;
    Vector2 movement_vector;
    public SpriteRenderer sprRend;
    public bool collider_triggered;
    Player_Base player_base;
    public bool can_Attack;
    public float speed;
    void Awake()
    {
        player_base=GetComponent<Player_Base>();
        m_Collider=GameObject.FindGameObjectWithTag("encounter_Spawn").GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        Start_Varibles();
    }

     void FixedUpdate()
    {
        Encounter();
        
    }

    void Update()
    {
        //m_Collider=GameObject.FindGameObjectWithTag("encounter_Spawn").GetComponent<BoxCollider2D>();
    }

    void Start_Varibles()
    {
        unitstats = GetComponent<UnitStats>();
        rb2d = GetComponent<Rigidbody2D>();
        movement_vector=new Vector2(1f,0);
        sprRend=GetComponent<SpriteRenderer>();
        collider_triggered=false;
        m_Collider=GameObject.FindGameObjectWithTag("encounter_Spawn").GetComponent<BoxCollider2D>();
        can_Attack=false;
        speed = 5.5f;
    }

    public void Forward_Movement()
    {
        rb2d.AddRelativeForce (movement_vector * speed);
        player_base.SetPlayerRun();
        sprRend.flipX = false;
    }

    public void Backwards_Movement()
    {
        rb2d.AddRelativeForce (movement_vector * speed * -1f);
        player_base.SetPlayerRun();
        sprRend.flipX = true;
        m_Collider.enabled = true;
    }

    void Encounter()
    {
        if(!collider_triggered)
            Forward_Movement();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "encounter_Spawn")
        {
            Debug.Log("Encounter Started");
            collider_triggered=true;            
            m_Collider.enabled=false;
            player_base.SetPlayerWait();
        }   
    }
}
