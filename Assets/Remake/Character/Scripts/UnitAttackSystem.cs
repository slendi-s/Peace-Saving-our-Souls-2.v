﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackSystem : MonoBehaviour
{
    public PlayerComponents playerComponents;
    public PlayerStatements playerStatements;
    public PlayerStats playerStats;



    public static UnitAttackSystem Instance { get; private set; }
    private void Awake()
    {
        CreateSingleton();
        
    }
    private bool CreateSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }

    public void Attack(PlayerStatements _pStatements, PlayerStats _pStats, PlayerComponents _pComponents)
    {
        _pStatements.isAttack = true;
        _pStats.countattack += 1;
        if (_pStats.countattack==1 || _pStats.countattack == 2 || _pStats.countattack == 3)
        {
            _pStats.attackComboCD = 1.5f;
            var attackCombo = string.Format("AttackCombo{0}", _pStats.countattack);
            _pComponents.animatorCharacter.SetTrigger(attackCombo);
            PrepareAttack(_pStats);
            AttackUse(_pStats,_pComponents);
            
        }
    } 
    public void AttackComboCD(PlayerStatements _pStatements, PlayerStats _pStats)
    {
        _pStats.attackComboCD -= Time.deltaTime;
        if (_pStats.attackComboCD <= 0)
        {
            _pStatements.isAttack = false;
            _pStats.countattack = 0;
        }

    }

    public void AttackUse(PlayerStats _pStats, PlayerComponents _pComponents)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_pComponents.attackPoint.position,_pStats.attackRange,_pComponents.enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerStats>().currentHealth -= _pStats.attackDamage;
            if (enemy.GetComponent<PlayerStatements>().isDead == false)
                enemy.GetComponent<PlayerComponents>().animatorCharacter.SetTrigger("Hurt");
            
            if (enemy.GetComponent<PlayerStats>().currentHealth <= 0 )
            {
                GameController.Instance.score += 1;
                enemy.GetComponent<PlayerStatements>().isDead = true;
                enemy.GetComponent<PlayerComponents>().animatorCharacter.SetBool("isDead",true);
                StartCoroutine(Timer(enemy));
            }
        
        }
    }
    IEnumerator PrepareAttack(PlayerStats _pStats)
    {
        yield return new WaitForSeconds(_pStats.prepareAttack);
    }
    IEnumerator Timer(Collider2D enemy)
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        
        yield return new WaitForSeconds(1.5f);
        Destroy(enemy.gameObject);
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    private void OnDrawGizmosSelected(PlayerStatements _pStatements,PlayerStats _pStats,PlayerComponents _pComponents)
    {
        if (_pComponents.attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_pComponents.attackPoint.position, _pStats.attackRange);
    }

    public void TakeDamage(PlayerStats _pStats,int damage)
    {
        _pStats.currentHealth -= damage;
        
    }


}
