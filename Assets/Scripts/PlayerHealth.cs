using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerHP = 100;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private TextMeshProUGUI playerHealth;
    

    public void PlayerTakeDamage(int damagereceived)
    {
        playerHP -= damagereceived;

        if (playerHP <= 0)
        {
            _animator.SetBool("Die", true);

            GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            
            Debug.Log(playerHP);
        }
        playerHealth.text = playerHP.ToString();
    }
}
