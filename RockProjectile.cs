using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectile : Projectile
{
    public GameObject hitEffect;
    public override void OnTriggerEnter2D(Collider2D other)
    {
            int inflictChance = Random.Range(0, 9);

            if (TeamSite_Projectile == A_Team)
            {
                if (other.gameObject.CompareTag("B_Team"))
                {
                    GameObject efftct = Instantiate(hitEffect, transform.position, Quaternion.identity);
                    Destroy(efftct,0.2f);
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
                    Debug.Log("Rock Damage: " + baseAttack);
                    enemy.GetComponent<Log>().TakeDamage(baseAttack, B_Team);
                }
            }
            else if (TeamSite_Projectile == B_Team)
            {
                if (other.gameObject.CompareTag("A_Team"))
                {
                    GameObject efftct = Instantiate(hitEffect, transform.position, Quaternion.identity);
                    Destroy(efftct,0.2f);
                    Destroy(this.gameObject);
                }

                Collider2D[] hitLog = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, Log_MASK);
                foreach (Collider2D enemy in hitLog)
                {
                    enemy.GetComponent<Log>().TakeDamage(baseAttack, A_Team,inflictChance);
                }

                Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, Orge_MASK);
                foreach (Collider2D enemy in hitEnemy)
                {
                    Debug.Log("Rock Damage: " + baseAttack);
                    enemy.GetComponent<NewGladiator>().TakeDamage_Ateam(baseAttack, A_Team,inflictChance);
                }
            }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
