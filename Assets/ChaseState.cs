using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;

    Transform player;

    private PlayerHealth playerHealth;

    private int playerDamageReceived = 10;
    private float nextAttackTime = 0f;
    private float attackCooldown = 1.5f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        playerHealth = player.GetComponent<PlayerHealth>();
        agent.speed = 3.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance > 10)
        {
            animator.SetBool("isChasing", false);
        }

        if(playerHealth.playerHP <= 0)
        {
            animator.SetBool("isChasing", false );
            animator.SetBool("isAttacking", false ) ;
        }
        else
        {
            if (distance < 5.5f)
            {
                animator.SetBool("isAttacking", true);
                if (Time.time > nextAttackTime)
                {

                    playerHealth.PlayerTakeDamage(playerDamageReceived);
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
        /*if (playerHealth.playerHP <= 0)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", false);
        }*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
