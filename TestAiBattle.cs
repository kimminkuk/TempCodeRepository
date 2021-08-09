using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAiBattle : MonoBehaviour {
    public IntValue CheckTeam;
    private int A_Team = 1;
    private int B_Team = 2;
    private int Team_State;

    private Transform testTarget;

    private LayerMask Layer;
    private void Start() {
        //if Orge, Log... class
        
        if(CheckTeam.RuntimeValue == A_Team) 
        {
            this.GameObject.tag = "A_Team";
            Team_State = A_Team;
            testTarget = GameObject.FindGameObjectWithTag("B_Team").GetComponent<Transform>();
        } 
        else if ( CheckTeam.RuntimeValue == B_Team )
        {
            this.GameObject.tag = "B_Team";
            Team_State = B_Team;
            testTarget = GameObject.FindGameObjectWithTag("A_Team").GetComponent<Transform>();
        }
    }



    //Case 1 -> this is CheckCharacter.cs case ?
    public GameObject respawnPrefab;
    public GameObject[] respawns;
    void Start()
    {
        if (respawns == null)
            respawns = GameObject.FindGameObjectsWithTag("Respawn");

        foreach (GameObject respawn in respawns)
        {
            Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
        }
    }

    //Case 2 -> this case is One <--> One intersection case..
    //I want Many Intersection case
    //1) layer sort -> log or orge or human ....
    //2) Team sort -> A or B Team..?
    // This Algorithm needs to be tested.
    private void TestDamageCnt()
    {
    
        Collider2D[] hits = Physics2D.OverlapCircleAll(tarnsform.position, 10f, Layer);
        foreach( Collider2D enemy in hitEnemies)
        {
            //A_Team <-> B_Team
            if(Team_State == A_Team) 
            {
                switch(Layer)
                {
                    case Log:
                        enemy.GetComponent<Log>().TakeDamage(baseAttack, B_Team);
                        break;
                    case Orge:
                        enemy.GetComponent<Orge>().TakeDamage(baseAttack, B_Team);
                        break;
                    case Human:
                        enemy.GetComponent<Huamn>().TakeDamage(baseAttack, B_Team);
                        break;
                    default:
                        break;
                }
                enemy.GetComponent<Log>().TakeDamage(baseAttack, B_Team);
                //enemy.GetComponent<Log1>().TakeDamage(baseAttack, B_Team);
                //enemy.GetComponent<Log2().TakeDamage(baseAttack, B_Team);
                //enemy.GetComponent<Log3().TakeDamage(baseAttack, B_Team);
                //....
                //Many Class wiil be added...
            }
            else if (Team_State == B_Team)
            {
                switch(Layer)
                {
                    case Log:
                        enemy.GetComponent<Log>().TakeDamage(baseAttack, A_Team);
                        break;
                    case Orge:
                        enemy.GetComponent<Orge>().TakeDamage(baseAttack, A_Team);
                        break;
                    case Human:
                        enemy.GetComponent<Huamn>().TakeDamage(baseAttack, A_Team);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private virtual void TakeDamage(int damage, int this_team)
    {
        if(this_team == this.Team_State)
        {
            health -= damage;
            healthBar.SetHealth(health);
            DamagePopupOpen(damage);
            // Play hurt animation
            if (health <= 0)
            {
                Die();
            }
        }
    }
}