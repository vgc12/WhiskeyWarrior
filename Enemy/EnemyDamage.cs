using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using FIMSpace.FProceduralAnimation;

public class EnemyDamage : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Component aiScript;
    public bool isDead = false;
    private bool collected = false;
    public CapsuleCollider capsuleCollider;
    private bool spawnMoney = false;
    [SerializeField] Rigidbody centerRigidbody;
    [SerializeField] private GameObject moneyObject;
    private PlayerPrefSaving prefSaving;
    [SerializeField] private int moneyToAdd;
    private Animator moneyAnimator;
    [SerializeField] private GameObject UiObject;
    [SerializeField] private RagdollAnimator ragdoll;
    private bool transitionToRagdoll;
    //public Animator animator;
    [SerializeField] private Transform rightHand;






    // Start is called before the first frame update
    void Start()
    {
        moneyAnimator = moneyObject.GetComponent<Animator>();
        prefSaving = FindObjectOfType<PlayerPrefSaving>();
        currentHealth = maxHealth;
        //SetRigidBodyState(true);
        //SetColliderState(false);
        
    }

    private void Update()
    {
        
        UiObject.SetActive(false);
        Debug.Log(prefSaving.GetPlayerMoney());
        if (isDead && spawnMoney && !collected)
        {
           

            moneyObject.SetActive(true);
            moneyObject.transform.position = transform.position;
            Collider[] detectPlayer = Physics.OverlapSphere(moneyObject.transform.position, 7f);
            foreach(Collider col in detectPlayer)
            {
         
                if (col.CompareTag("Player"))
                {
                    UiObject.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        UiObject.SetActive(false);
                        prefSaving.SetPlayerMoney(prefSaving.GetPlayerMoney() + moneyToAdd);
                        moneyObject.SetActive(false);
                        collected = true;
                    }
                }
            }
        }
        
        


    }


    
 
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(moneyObject.transform.position, 7f);
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            spawnMoney = true;
            Die();
        }
    }
    void Die()
    {
        
        GetComponent<Animator>().enabled = false;
        ragdoll.User_EnableFreeRagdoll(1);
        //SetRigidBodyState(false);
        //SetColliderState(true);
        Destroy(aiScript);
        Debug.Log("enemy died");
        
        

    }

   
    /*
    void SetRigidBodyState(bool state)
    {
        
        

        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();

       
        foreach(Rigidbody rigidbody in rigidBodies)
        {
            rigidbody.isKinematic = state;
            if (state == false)
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        if (state == false)
        {

            float bruh = Random.Range(200, 400);
            Debug.Log(bruh);
            centerRigidbody.AddForce(bruh * (centerRigidbody.transform.forward * -1), ForceMode.Impulse);
            GetComponent<NavMeshAgent>().enabled = false;
            
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void SetColliderState(bool state)
    {
        
        Collider[] ragdollColliders = GetComponentsInChildren<Collider>();

        foreach (Collider ragdollCollider in ragdollColliders)
        {
            ragdollCollider.enabled = state;
        }
        GetComponent<Collider>().enabled = !state;
        capsuleCollider.enabled = !state;
    }
    */
}
