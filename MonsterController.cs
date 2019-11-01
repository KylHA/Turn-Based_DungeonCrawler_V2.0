using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script Controls the Monster Movement 
public class MonsterController : MonoBehaviour
{

    UnitStats unitstats;
    private Rigidbody2D rb2d;
    Vector2 movement_vector;
    public SpriteRenderer sprRend;
    public bool collider_triggered;
    BoxCollider2D m_Collider; 
    MonsterBattleHandler battleControl;
    Monster_Base monster_base; //will be added for Monster animations
    public float speed;
     void Start()
    {
        Start_Varibles();
        
    }

     void Start_Varibles()
    {
        speed = 5.5f;
        unitstats = GetComponent<UnitStats>();
        rb2d = GetComponent<Rigidbody2D>();
        movement_vector=new Vector2(1f,0);
        sprRend=GetComponent<SpriteRenderer>();
        m_Collider=GameObject.FindGameObjectWithTag("MonsterSpawnP").GetComponent<BoxCollider2D>();
        m_Collider.enabled=false;
        collider_triggered=false;
        battleControl=GetComponent<MonsterBattleHandler>();
        monster_base=GetComponent<Monster_Base>();
        //battleControl.monsterf=true;
    }
    void Update()
    {
        m_Collider=GameObject.FindGameObjectWithTag("MonsterSpawnP").GetComponent<BoxCollider2D>();
    }

    public void Forward_Movement()
    {
        rb2d.AddRelativeForce (movement_vector * speed * -1f);
        monster_base.SetMonsterRun();
        sprRend.flipX = true;
        collider_triggered=true;
        battleControl.monsterf=false;
    }

    public void Backwards_Movement()
    {
        rb2d.AddRelativeForce (movement_vector * speed);
        monster_base.SetMonsterRun();
        sprRend.flipX = false;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collider_triggered)
        {          
            m_Collider.enabled=true;
            
            collider_triggered=false;
        }   

        if(collision.gameObject.tag == "MonsterSpawnP")
        {
            battleControl.monsterf=true;
            m_Collider.enabled=false;
        }
    }
}
