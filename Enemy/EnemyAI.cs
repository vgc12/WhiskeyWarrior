using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent enemyAgent;

    public Animator enemyAnimator;

    public Transform player;

    public LayerMask playerMask, groundMask;

    public Animator playerHitAnimations;

    [SerializeField] private int punchDamage;

    [SerializeField] private int kickDamage;

    private CombatHelper combatHelper;


    public float timeBetweenAttacks;
    bool readyToAttack = true;


    //states
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    private bool move;
    private float animLength;

    private void Awake()
    {

        player = FindObjectOfType<Player>().transform;
        enemyAgent = GetComponent<NavMeshAgent>();
        animLength = enemyAnimator.runtimeAnimatorController.animationClips.First(x => x.name == "punchRight").length;
        combatHelper = GetComponent<CombatHelper>();

    }




    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);


        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInSightRange && playerInAttackRange)
        {
            Attack();
        }
        if (enemyAgent.velocity.magnitude < .05f)
        {
            move = false;
            enemyAnimator.SetBool("punchIdle", true);
        }

        if (enemyAgent.velocity.magnitude > .05f && !playerInAttackRange)
        {
            move = true;
            enemyAnimator.SetBool("punchIdle", false);
        }




    }





    private void Attack()
    {


        Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(playerPosition);

        move = false;


        if (readyToAttack)
        {
            var rand = Random.Range(0, 4);


            if (rand == 2 || rand == 3)
                animLength = enemyAnimator.runtimeAnimatorController.animationClips.First(x => x.name == "drunk kick right").length;
            else
                animLength = enemyAnimator.runtimeAnimatorController.animationClips.First(x => x.name == "punchRight").length;

            enemyAnimator.SetInteger("AttackIndex", rand);
            enemyAnimator.SetTrigger("Attack");

            StartCoroutine(TimeBeforeHit());

            readyToAttack = false;

            StartCoroutine(ResetAttack());
        }
    }

    private void Chase()
    {

        enemyAnimator.SetBool("Move", move);
        enemyAgent.SetDestination(player.position);


    }



    IEnumerator TimeBeforeHit()
    {
        enemyAgent.isStopped = true;

        yield return new WaitForSeconds(animLength);

        enemyAgent.isStopped = false;
    }

    IEnumerator ResetAttack()
    {

        yield return new WaitForSeconds(timeBetweenAttacks);


        readyToAttack = true;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }


    private void AssignAnimLengthPunch() => animLength = enemyAnimator.runtimeAnimatorController.animationClips[2].length;
    private void AssignAnimLengthKick() => animLength = enemyAnimator.runtimeAnimatorController.animationClips[3].length;
    private void RightHitAnim()
    {
        if (playerInAttackRange)
        {
            playerHitAnimations.SetTrigger("punchedFromRight");
            FindObjectOfType<PlayerHealth>().TakeDamage(punchDamage);
            combatHelper.PlayPunch();
        }
        else
            combatHelper.PlayMiss();
     }

    private void LeftHitAnim() 
    { 
        if (playerInAttackRange)
        {
            playerHitAnimations.SetTrigger("punchedFromLeft");
            FindObjectOfType<PlayerHealth>().TakeDamage(punchDamage);
            combatHelper.PlayPunch();
        }
        else
        {
            combatHelper.PlayMiss();
        }
    }

    private void LeftKickAnim() 
    {
        if (playerInAttackRange)
        {
            playerHitAnimations.SetTrigger("kickedFromLeft");
            FindObjectOfType<PlayerHealth>().TakeDamage(kickDamage);
            combatHelper.PlayPunch();
        }
        else
            combatHelper.PlayMiss();
    }

    private void RightKickAnim() 
    {
        if (playerInAttackRange)
        {
            playerHitAnimations.SetTrigger("kickedFromRight");
            FindObjectOfType<PlayerHealth>().TakeDamage(kickDamage);
            combatHelper.PlayPunch();
        }
        else
            combatHelper.PlayPunch();
    }


}
