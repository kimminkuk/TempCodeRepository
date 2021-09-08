using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Townt_Projectile : Projectile
{
    [Tooltip("Position we want to hit")]
    //public Vector3 targetPos;
    public GameObject hitEffect;
    public int team;
    private Vector3 startPos;
    private Vector3 targetPos2;
    private Vector3 CastPos;
    private Animator Ani;
    [Header("Skill Casting Object")]
    public GameObject Casting;
    private float waitTime = 0.21f;
    private bool OnOff_E_MakeTownt = true;
    private float GA = 9.8f;
    private float PosY = 0;
    private float GA_Time = 1f;
    private int GA_cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        Ani = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        lifetimeSeconds = lifetime * 2;
        startPos = transform.position; 
    }
    public override void InitSet(Vector3 this_target, int this_team, float this_projectileSpeed, int this_Damage)
    {
        TeamSite_Projectile = this_team;
        targetPos = this_target;
        speed = this_projectileSpeed;
        baseAttack = this_Damage;
        this.targetPos2 = this_target;
        this.targetPos2.y += + 6f;

        transform.position = this.targetPos2;

        CastPos = this.targetPos2;
        CastPos.y -= 1f;
        GameObject CastObject = Instantiate(Casting, CastPos, Quaternion.identity);
        CastObject.GetComponent<Skill_9_Log_Casting>().Casting(1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        lifetimeSeconds -= Time.deltaTime;
        waitTime -= Time.deltaTime;

        if (lifetimeSeconds <= 0)
        {
            Destroy(this.gameObject);
        }

        if(waitTime <= 0) 
        {
            if(OnOff_E_MakeTownt)
            {
                E_MakeTownt(); //Make Townt Projectile... Only One Time
            }

            // 1) Change RigidBody2D dynamic to kinematic.
            // 2) Gravitational Acceleration will be applied. (9.80665 m / s*s      )
            // 0.16, 1.60, 
            myRigidbody.AddForce(new Vector3(0, -4f + PosY, 0), ForceMode2D.Force);

            //Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPos,speed * Time.deltaTime);
            //nextPos = new Vector3(nextPos.x, nextPos.y - PosY, transform.position.z);
            //transform.position = nextPos;


            PosY = (9.8 * GA_Time) / 60f; //0.16-> 0.65 -> 1.47 -> 2.61 -> 4.08 -> 5.88
            //if ok? -> 2gh cal
            GA_Time = Math.Pow(++GA_cnt, 2); //1->4->9->16->25... 
        }

        // // 1) Only MoveTowards
        // // transform.position = this.targetPos2;
        // Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime *1.5f);
        // transform.position = nextPos;

        // 2) MoveTowards + Lerp
        // transform.position = this.targetPos2;
        // Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        // float baseY = Mathf.Lerp(transform.position.y, targetPos.y, 0.001f); //7*0.1 -> 0.7
        // nextPos = new Vector3(nextPos.x, nextPos.y - baseY, transform.position.z);
        // transform.position = nextPos;

        // 3) AddForce
        //myRigidbody.AddForce(new Vector3(0, -4f, 0), ForceMode2D.Impulse);
        //myRigidbody.AddForce(new Vector3(0, -4f, 0), ForceMode2D.Force);
        
        // 
        // 4) velocity?
        // myRigidbody.velocity = new Vector3(0, -1, 0) * Time.deltaTime;
    }

    public void E_MakeTownt()
    {
        Ani.SetTrigger("TowntMake");
        OnOff_E_MakeTownt = false;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (TeamSite_Projectile == A_Team)
        {
            if (other.gameObject.CompareTag("B_Team"))
            {
                GameObject efftct = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(efftct, 0.2f);
                Destroy(this.gameObject);
            }

            Collider2D[] hitOrge = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, Orge_MASK);
            foreach (Collider2D enemy in hitOrge)
            {
                enemy.GetComponent<Orge>().TakeDamage_Bteam(baseAttack, B_Team);
            }

            Collider2D[] hitLog = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, Log_MASK);
            foreach (Collider2D enemy in hitLog)
            {
                enemy.GetComponent<Log>().TakeDamage(baseAttack, B_Team);
            }
        }
        else if (TeamSite_Projectile == B_Team)
        {
            Debug.Log("OnTriggerEnter2D Townt Call: " + baseAttack);
            if (other.gameObject.CompareTag("A_Team"))
            {
                GameObject efftct = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(efftct, 0.2f);
                Destroy(this.gameObject);
            }

            Collider2D[] hitLog = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, Log_MASK);
            foreach (Collider2D enemy in hitLog)
            {
                enemy.GetComponent<Log>().TakeDamage(baseAttack, A_Team);
            }

            Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, Orge_MASK);
            foreach (Collider2D enemy in hitEnemy)
            {
                Debug.Log("Townt Damage: " + baseAttack);
                enemy.GetComponent<NewGladiator>().TakeDamage_Ateam(baseAttack, A_Team);
            }
        }
    }
}
