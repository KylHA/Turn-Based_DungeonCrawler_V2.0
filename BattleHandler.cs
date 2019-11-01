using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour 
{
    Player_Base player_base;
    PlayerController p_control;
    [SerializeField] bool reached_enemy;
    public bool encounter, PlayerAttack, PlayerAttackEnded;
    private Action onMarchComplete;
    public float timer;
    private State state;
    
    private enum State
    {
        Attack,
        Marching,
        Turning,
        Busy,
    }

    void StartVaribles()
    {
        player_base=GetComponent<Player_Base>();
        p_control=GetComponent<PlayerController>();
        reached_enemy=false;
        state=State.Busy;
        encounter=false;
        timer=0;
    }

    void Start()
    {
        StartVaribles();
        Setup();
    }

    void FixedUpdate()
    {
        Player_attack_States();
    }

    void Setup()
    {
        //if(isPlayer(Obj))
        //{
        //   player_base.SetPlayerWait();
        //}
        //else
        //{
        //    //Monter_base.SetMonsterWait();
        //}
    }

     public void Player_Attack(Action onAttackComplete)
    {
        PlayerAttack=true;
        PlayerAttackEnded=false;
        state=State.Marching;
        onAttackComplete();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Enemy"&& PlayerAttack)
        {
            reached_enemy=true;
            encounter=false;
        }
        if(collision.gameObject.tag=="encounter_Spawn")
        {
            encounter=true;
        }
    }

    public void Player_attack_States()
    {   
        if(PlayerAttack)
        {
            switch (state)
            {
                case State.Attack:
                    timer+=Time.deltaTime;
                    player_base.SetPlayerAttack();
                    if(timer>=1.7f)
                    {
                        timer=0;
                        state=State.Turning;
                        reached_enemy=false;
                    }
                    break;

                case State.Turning:
                    p_control.Backwards_Movement();
                    if(encounter)
                        state=State.Busy;
                    break;

                case State.Busy:
                    if(encounter)
                    {   PlayerAttack=false;
                        PlayerAttackEnded=true;
                        player_base.SetPlayerWait();
                    }
                    break;

                case State.Marching:
                    p_control.Forward_Movement();
                    if(reached_enemy)
                        state=State.Attack;
                    break;
            }
        }
    }
}
