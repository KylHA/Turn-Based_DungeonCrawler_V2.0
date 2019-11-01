using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject monster_Prefab;
    [SerializeField] GameObject encounter_spawner;
    [SerializeField] GameObject monster_spawn_point_spawner;
    [SerializeField] GameObject Empty_Wall;
    UnitStats m_unitstats, p_unitstats;
    GameObject instanceMonsterwall,instancePlayerwall;
    BoxCollider2D e_Collider;

    Vector3 spawner_pos;
    PlayerController p_Control;
    MonsterController m_Control;
    BattleHandler Battle_Control;
    MonsterBattleHandler M_Battle_Control;
    private State state;
    private Monster_state m_state;
    private Attack_Order_State a_order_state;
    float player_start_value,monster_start_value;
    public int turnCounter;
    public float Roller;
    
    private enum Attack_Order_State
    {
        PlayerCanAttack,
        MonsterCanAttack,
    }

    private enum Monster_state
    {
        MonsterSpawned,
        SpawnEmpty,
    }

    private enum State
    {
        WaitingForPlayer,
        Reached,
        Busy,
    }

    void Start()
    {
        Start_Varibles();
        
        state = State.WaitingForPlayer;
        m_state=Monster_state.SpawnEmpty;
        a_order_state=Attack_Order_State.MonsterCanAttack;
    }

    void Awake()
    {
        
    }

    void FixedUpdate()
    {
        if(m_state==Monster_state.SpawnEmpty)
        {
            onEncounter(p_Control.collider_triggered, () => {
                m_state=Monster_state.MonsterSpawned;
            });
            state=State.WaitingForPlayer;
        }

        Start_Attack();
    }

    void Start_Varibles()
    {
        //GameObject spawner=GameObject.FindGameObjectWithTag("encounter_Spawn");
        //spawner_pos=spawner.transform.position;
        monster_Prefab=GameObject.FindGameObjectWithTag("Enemy");
        p_Control=GameObject.Find("Test_obj").GetComponent<PlayerController>();
        m_Control=GameObject.FindGameObjectWithTag("Enemy").GetComponent<MonsterController>();
        Battle_Control=GameObject.Find("Test_obj").GetComponent<BattleHandler>();
        M_Battle_Control=GameObject.FindGameObjectWithTag("Enemy").GetComponent<MonsterBattleHandler>();
        m_unitstats=GameObject.FindGameObjectWithTag("Enemy").GetComponent<UnitStats>();
        p_unitstats=GameObject.Find("Test_obj").GetComponent<UnitStats>();
        turnCounter=1;//will be needed for continues attack cycle
        e_Collider=Empty_Wall.GetComponent<BoxCollider2D>();
    }

    void Spawn_Enemy()
    {
        Vector3 distance=new Vector3(3,0,0);
        
        monster_Prefab.transform.position=GameObject.FindGameObjectWithTag("encounter_Spawn").transform.position+distance;  
        Spawn_BackWalls();   
    }

    void Spawn_BackWalls()
    {
        Vector3 Sides=new Vector3(0.15f,0,0);
        instancePlayerwall = Instantiate (Empty_Wall, GameObject.FindGameObjectWithTag("encounter_Spawn").transform.position 
        - Sides, Quaternion.identity) as GameObject;
        instanceMonsterwall = Instantiate (Empty_Wall, GameObject.FindGameObjectWithTag("MonsterSpawnP").transform.position 
        + Sides, Quaternion.identity) as GameObject;
    }
    void onEncounter(bool create,Action onMonsterSpawn )
    {
        if(create)
        {
            Spawn_Enemy();
            onMonsterSpawn();
        }
    }

    void First_Attacker_Roller()
    {
        player_start_value = UnityEngine.Random.Range(p_unitstats.speed - 3f,p_unitstats.speed + 3f);
        monster_start_value = UnityEngine.Random.Range(m_unitstats.speed - 3f,m_unitstats.speed + 3f);
        Debug.Log("player_start_value rolled: "+player_start_value+"/ monster_start_value rolled: "+monster_start_value);
    }

    void Attack_Turns()
    {
        if(player_start_value>=monster_start_value)
            a_order_state=Attack_Order_State.PlayerCanAttack;
        else
            a_order_state=Attack_Order_State.MonsterCanAttack;
    }

    void Start_Attack()
    {
        if(m_state==Monster_state.MonsterSpawned && state==State.WaitingForPlayer)
        {
            Game_Checker();
            First_Attacker_Roller();
            Attack_Turns();
            
            if(a_order_state==Attack_Order_State.MonsterCanAttack)
            {
                p_unitstats.health -= Attack_Damage_Roller(m_unitstats);
                Debug.Log("Monster Attack Started");
                M_Battle_Control.Monster_Attack(() => {
                    state = State.WaitingForPlayer;
                });
            }

            if(a_order_state==Attack_Order_State.PlayerCanAttack)
            {
                m_unitstats.health  -= Attack_Damage_Roller(p_unitstats);
                Debug.Log("Player Attack Started");
                Battle_Control.Player_Attack(() => {
                    state = State.WaitingForPlayer;
                });
                p_Control.Backwards_Movement();//for player to turn back original position
            }
            state=State.Busy;
        }
        Turn_Ender();
    }

     void Turn_Counter()
    {
        state=State.WaitingForPlayer;
        Debug.Log("Turn: "+turnCounter);
        
        turnCounter++;
    }

    void Turn_Ender()
    {
        if(Battle_Control.PlayerAttackEnded)
        {
            Battle_Control.PlayerAttackEnded=false;
            Debug.Log("Battle_Control.PlayerAttackEnded");
            Turn_Counter();
            a_order_state=Attack_Order_State.MonsterCanAttack;
        }

        if(M_Battle_Control.MonsterAttackEnded)
        {
            M_Battle_Control.MonsterAttackEnded=false;
            Debug.Log("Battle_Control.MonsterAttackEnded");
            Turn_Counter();
            a_order_state=Attack_Order_State.PlayerCanAttack;
        }
    }

    void Dead_Control()
    {
        if(m_unitstats.health<=0)
        {
            m_unitstats.dead=true;
        }
        if(p_unitstats.health<=0)
        {
            p_unitstats.dead=true;
        }
    }

    void Game_Checker()
    {
        Dead_Control();

        if(m_unitstats.dead)
        {
            p_Control.collider_triggered=false;
            Debug.Log("collider Triggered : "+p_Control.collider_triggered);
            Destroy(instancePlayerwall);
            Destroy(instanceMonsterwall);
            state=State.Busy;
            Random_Spawn_Point_Spawner();
        }

        if(p_unitstats.dead)
            Debug.Log("Game Over Player IS DEAD");
    }

    void Random_Spawn_Point_Spawner()
    {   float handler=7;
        float spawn_creater=UnityEngine.Random.Range(handler,handler+10);
        Vector3 spawn_vector = new Vector3(spawn_creater,0.07f,0);
        Vector3 distance_spawner=new Vector3(3,0,0);
        if(handler+10<=40)
        {
        GameObject instance_encounter_spawner = Instantiate (encounter_spawner, spawn_vector, Quaternion.identity) as GameObject;
        GameObject instance_monster_spawn_spawner =Instantiate (monster_spawn_point_spawner, spawn_vector + distance_spawner, Quaternion.identity) as GameObject;
        }
        handler+=4;
    }

    float Attack_Damage_Roller(UnitStats unitstats)
    {
        Roller = UnityEngine.Random.Range(unitstats.attack-5,unitstats.attack+5);
        int crit_Roller=UnityEngine.Random.Range(1,6);

        if(crit_Roller==6)
        {
            Roller *= 3;
        }
        return Roller;
    }
}