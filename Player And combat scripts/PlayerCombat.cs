using FIMSpace.FProceduralAnimation;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class PlayerCombat : MonoBehaviour
{
    public Animator punchAnimator;
    public Animator enemyAnimator;

    private LayerMask playerMask;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private NavMeshAgent navMesh;
    private CombatHelper combatHelper;

    public Animator camAnimator;

    private PlayerPrefSaving prefSaving;
    [SerializeField] private int punchDamage;
    [SerializeField] private int kickDamage;







    private Collider enemyCol;

    private float roundHouseAnimLength;
    private float punchAnimLength;
    private bool hitEnemy;



 
    [SerializeField] private RagdollAnimator ragdoll;
    [SerializeField] private Transform rightArmTransform;
    [SerializeField] private Transform leftArmTransform;
    [SerializeField] private Transform rightLegTransform;
    [SerializeField] private Transform leftLegTransform;
    [SerializeField] private Transform rightLegOverlapSpherePos;
    [SerializeField] private Transform leftLegOverlapSpherePos;


    private Collider[] rightHandCols;
    private Collider[] leftHandCols;
    private Collider[] rightLegCols;
    private Collider[] leftLegCols;


    [SerializeField] private EnemyDamage enemyDamage;

    private readonly string punchRight = "playerRightPunch";
    private readonly string punchLeft = "playerLeftPunch";
    private readonly string kickRight = "playerRightKick";
    private readonly string kickLeft = "playerLeftKick";

 
    private readonly int animLayer = 0;
    public int buttonPressed;

    private bool readyToAttack;
    private bool temp = true;


    private void Start()
    {
        prefSaving = FindObjectOfType<PlayerPrefSaving>();
        punchDamage = prefSaving.GetPunchDamage();
        kickDamage = prefSaving.GetKickDamage();
        combatHelper = GetComponent<CombatHelper>();

        buttonPressed = -1;

        camAnimator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        playerMask = ~LayerMask.GetMask("Player");
        temp = true;

        if (enemyAnimator != null)
        {
            punchAnimLength = enemyAnimator.runtimeAnimatorController.animationClips.First(x => x.name == "enemygettingpunched").length;

            roundHouseAnimLength = enemyAnimator.runtimeAnimatorController.animationClips.First(x => x.name == "roundhousereaction").length;
        }
    }


    public void UpdatePlayerDamage(int attackDamage, int kickDamage)
    {
        prefSaving.SetPunchDamage(this.punchDamage + attackDamage);
        punchDamage = prefSaving.GetPunchDamage();
        prefSaving.SetKickDamage(this.kickDamage + kickDamage);
        this.kickDamage = prefSaving.GetKickDamage();
    }

    // player combat is being overhauled due to being too boring and not annoying enough later in the game
    // sound effects arent playing currently.
    void Update()
    {
        if (DialogueVariables.dialogueIsPlaying || enemyAnimator == null || navMesh == null) return;

        // hit detection based on limbs
        rightHandCols = Physics.OverlapSphere(rightArmTransform.position, .5f);
        leftHandCols = Physics.OverlapSphere(leftArmTransform.position, .5f);
        rightLegCols = Physics.OverlapSphere(rightLegOverlapSpherePos.position, .5f);
        leftLegCols = Physics.OverlapSphere(leftLegOverlapSpherePos.position, .5f);

        /* button pressed is an int that gets set from InputManager
        *0 left mouse button
        *1 right mouse button
        *2 Q 
        *3 e
        *4 spacebar
        *-1 no key is pressed
        */
        if (buttonPressed == 0 && readyToAttack) PlayAnimation(punchRight);
        if (buttonPressed == 1 && readyToAttack) PlayAnimation(punchLeft);
        if (buttonPressed == 2 && readyToAttack) PlayAnimation(kickLeft);
        if (buttonPressed == 3 && readyToAttack) PlayAnimation(kickRight);
        if (buttonPressed == 4 && readyToAttack) RoundHouse(leftLegCols);

        Debug.Log(isPlaying(punchAnimator, punchRight));

        if (!isPlaying(punchAnimator, punchRight) && !isPlaying(punchAnimator, punchLeft)
            && !isPlaying(punchAnimator, kickRight) && !isPlaying(punchAnimator, kickLeft))
        {
            readyToAttack = true;
        }



    }

    private void FixedUpdate()
    {
        if (isPlaying(punchAnimator, punchLeft))
        {
            DamageEnemy(leftHandCols, punchLeft, punchDamage);
            MoveRagdoll(leftArmTransform, leftHandCols, 1, 30);
        }
        else if (isPlaying(punchAnimator, punchRight))
        {
            DamageEnemy(rightHandCols, punchRight, punchDamage);
            MoveRagdoll(rightArmTransform, rightHandCols, 1, 30);
        }
        else if (isPlaying(punchAnimator, kickRight))
        {
            DamageEnemy(rightLegCols, kickRight, kickDamage);
            MoveRagdoll(rightLegTransform, rightLegCols, -1, 90);
        }
        else if (isPlaying(punchAnimator, kickLeft))
        {
            DamageEnemy(leftLegCols, kickLeft, kickDamage);
            MoveRagdoll(leftLegTransform, leftLegCols, -1, 90);
        }
    }

    private void DamageEnemy(Collider[] colliders, string animation, float damage)
    {
        foreach (Collider col in colliders)
        {
            Debug.Log(col.gameObject.layer);
            if (isPlaying(punchAnimator, animation) && col.gameObject.layer == LayerMask.NameToLayer("ragdoll") && !readyToAttack)
            {

                enemyDamage.TakeDamage(punchDamage);
                Debug.Log(enemyDamage.currentHealth);
                readyToAttack = true;
            }
        }
    }


    private void MoveRagdoll(Transform limb, Collider[] cols, int positveOrNegative, float impactAmount)
    {
        // adds force to enemy's ragdoll based on limb direction
        Vector3 direction = ragdoll.Parameters.Head.position - Camera.main.transform.position;
        direction += (limb.forward * positveOrNegative) * (impactAmount);
        foreach (Collider col in cols)
        {
            if (col.GetComponent<Rigidbody>() != null)
            {
                ragdoll.User_SetLimbImpact(col.GetComponent<Rigidbody>(), direction, 0f);
                combatHelper.PlayPunch();
                break;
            }
            hitEnemy = false;
        }
        if (hitEnemy == false)
            combatHelper.PlayMiss();
    }


    private bool isPlaying(Animator anim, string stateName)
    {
        // checks if an animator currently has a specific animation that is playing
        if (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }


    private void EnemyRoundhouseAnim()
    {
        // this is commented out as i dont know if i should have animations and ragdolls playing at the same time. they work together its just idk if this is needed yet
        /*
        if (enemyCol.CompareTag("Enemy"))
        {
            navMesh.isStopped = true;
            enemyAnimator.SetTrigger("roundHouse");
            StartCoroutine(FreezeInPlace(roundHouseAnimLength));

        }
        */
    }

    private void RoundHouse(Collider[] overlapColliders)
    {
        
        punchAnimator.Play("roundhouse");
        camAnimator.Play("Roundhousecam");

        foreach (Collider col in overlapColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                enemyCol = col;
                
                combatHelper.PlayPunch();

                hitEnemy = true;
                break;
            }
            hitEnemy = false;
        }
        if (hitEnemy == false)
        {
            combatHelper.PlayMiss();
        }


        buttonPressed = -1;
        return;

    }

    private IEnumerator FreezeInPlace(float freezeTime)
    {

        yield return new WaitForSeconds(freezeTime);

        navMesh.isStopped = false;
    }

    private void PlayAnimation(string animName)
    {

        punchAnimator.Play(animName);
        readyToAttack = false;
        buttonPressed = -1;
        //  StartCoroutine(FreezeInPlace(animTime));
        //combatHelper.PlayPunch();

    }





}
