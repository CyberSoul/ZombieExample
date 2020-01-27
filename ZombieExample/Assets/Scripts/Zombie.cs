using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    Animator animator;
    MovementAnimator movementAnimator;
    bool dead;

    NavMeshAgent navMeshAgent;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        movementAnimator = GetComponent<MovementAnimator>();
        navMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;

        navMeshAgent.SetDestination(player.transform.position);
        transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
        //navMeshAgent.SetDestination(TargetPosition);
    }

    public void Kill()
    {
        if (!dead)
        {
            dead = true;
            Destroy(capsuleCollider);
            Destroy(movementAnimator);
            Destroy(navMeshAgent);
            GetComponentInChildren<ParticleSystem>().Play();
            animator.SetTrigger("died");
            Destroy(gameObject, 5);//Remove object after 5 seconds.
        }
    }
}
