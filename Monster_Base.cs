using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Base : MonoBehaviour
{
    Animator monster_anim;
    SpriteRenderer sprRend;

    void Start()
    {
        monster_anim=GetComponent<Animator>();
        sprRend=GetComponent<SpriteRenderer>();
        monster_anim.Play("MonsterWait");
        sprRend.flipX = true;
    }
    void Awake()
    {
        
    }

    // Update is called once per frame
    public void SetMonsterRun()
    {
        monster_anim.Play("MonsterRun");
    }

    public void SetMonsterWait()
    {
        monster_anim.Play("MonsterWait");
        sprRend.flipX = true;
    }
    
    public void SetMonsterAttack()
    {
        monster_anim.Play("MonsterAttack");
        sprRend.flipX = true;
    }
}
