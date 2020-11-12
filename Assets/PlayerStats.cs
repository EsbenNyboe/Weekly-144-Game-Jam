using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float startHealth;
    float health;

    public Image healthBarFill;
    PlayerMovement movement;

    public static bool dead;

    private void Start()
    {
        TryGetComponent(out movement);

        health = startHealth;
        healthBarFill.fillAmount = health / startHealth;
        dead = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBarFill.fillAmount = health / startHealth;
        //Soundbank.instance.playerDamaged.PlayDefault();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        movement.enabled = false;
        dead = true;
        Soundbank.instance.playerKilled.PlayDefault();
        GameManager.instance.Lose();

        health = startHealth;
        healthBarFill.fillAmount = health / startHealth;
    }
}
