using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDragon : MonoBehaviour
{
    public int HP = 1000;

    public Animator animator;

    [SerializeField] public Slider dragonHealthBar;


    private void Update()
    {
        dragonHealthBar.value = HP;
    }
    public void TakeDamage(int damageAmount)
    {
        HP  -= damageAmount;

        if(HP <=0)
        {
            //Play death animation
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            //play get hit animation
            animator.SetTrigger("damage");
            
            Debug.Log(HP);
        }
    }

    
}
