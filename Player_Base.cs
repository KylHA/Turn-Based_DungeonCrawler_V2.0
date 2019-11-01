using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script Controls the Player Based Animations

public class Player_Base : MonoBehaviour
{
    Animator player_anim;
    SpriteRenderer sprRend;

    void Start()
    {
        player_anim=GetComponent<Animator>();
        sprRend=GetComponent<SpriteRenderer>();
    }

    public void SetPlayerRun()
    {
        player_anim.Play("PlayerRun");
    }

    public void SetPlayerWait()
    {
        player_anim.Play("PlayerWait");
        sprRend.flipX = false;
    }
    
    public void SetPlayerAttack()
    {
        player_anim.Play("PlayerFullAttack");
    }
}
