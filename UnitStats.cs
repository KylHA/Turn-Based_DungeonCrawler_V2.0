using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    
	public float health;
    
	public float mana;
    
	public float attack;
    
	public float magic;
    
	public float defense;
    
	public float speed;
	
	public int exprience;

	public bool dead = false;

    void Start()
    {
        
    }

    void Update()
    {	Vector3 vektor=new Vector3(3.85f,-2.59f,-0.01f);

		if (dead)
		{
			this.transform.position=vektor;
			this.health=50;
			this.dead=false;
		}
    }
}
