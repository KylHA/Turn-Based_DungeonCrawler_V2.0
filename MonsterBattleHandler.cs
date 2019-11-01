using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattleHandler : MonoBehaviour
{
    MonsterController monster_controller;
    [SerializeField] bool reached_enemy;
    public bool monsterf, MonsterAttack, MonsterAttackEnded, marching_Player;
    bool empty;
    Monster_Base monster_base;
    public float timer;
    
    private State state;
    
    private enum State
    {
        Attack,
        Marching,
        Turning,
        Busy,
    }

    void Start()
    {
        empty=false;//for Monster_attack_States
        monster_controller = GetComponent<MonsterController>();
        monster_base=GetComponent<Monster_Base>();
        reached_enemy=false;
        state=State.Busy;
        monsterf=false;
        timer=0;
        marching_Player=false;
    }
    void FixedUpdate()
    {
        Monster_attack_States();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MonsterSpawnP")
        {
            monsterf=true;
            reached_enemy=false;
        }
        
        if(collision.gameObject.tag == "Player" && marching_Player)
        {
            reached_enemy=true;
            marching_Player=false;
        }
    }

    public void Monster_Attack(Action onAttackComplete)
    {
        MonsterAttackEnded=false;
        state=State.Marching;
        MonsterAttack=true;
        onAttackComplete();
    }

    void Monster_attack_States()
    {
        if(MonsterAttack)
        {
            switch (state)
            { 
                case State.Attack:
                    monster_base.SetMonsterAttack();
                    timer+=Time.deltaTime;
                    if(timer>=1.7f)
                    {
                        timer=0;
                        state=State.Turning;
                        reached_enemy=false;
                    }
                    break;

                case State.Turning:
                    monster_controller.Backwards_Movement();
                    if(monsterf)
                    {
                        state=State.Busy;
                    }
                    break;

                case State.Busy:

                    if(monsterf)
                    {   
                        monster_base.SetMonsterWait();
                        MonsterAttackEnded=true;
                        MonsterAttack=false;
                        monsterf=false;
                    }
                    break;

                case State.Marching:
                     monster_controller.Forward_Movement();
                     marching_Player=true;
                    if(reached_enemy)
                        state=State.Attack;
                    break;

            }
        }
    }
}
