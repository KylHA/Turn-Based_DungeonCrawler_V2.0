using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    
    void Start()
    {
        offset= new Vector3(2,0,0);
    }
    void Update () 
    {
        transform.position = new Vector3 (player.position.x + offset.x, player.position.y , 0); // Camera follows the player with specified offset position
    }
}
