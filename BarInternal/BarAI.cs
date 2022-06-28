using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using System.Linq;

public class BarAI : MonoBehaviour
{
    
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private float minWaitTime, maxWaitTime;
    private Animator patronAnimator;
    private CapsuleCollider capsuleCollider;
    private protected int rand;
   

    // Start is called before the first frame update
    private void Start()
    {
        patronAnimator = GetComponent<Animator>();

        capsuleCollider = GetComponent<CapsuleCollider>();
        SetRigidBodyState(true);
        SetColliderState(false);
        capsuleCollider.enabled = true;
        

        rand = Random.Range(0, moveSpots.Length) ;
    }

    // Update is called once per frame
    private void Update()
    {
        if (navMesh.enabled == true && navMesh.isStopped == false)
        {
            if (transform.CompareTag("bartender")) return;
            //navMesh.SetDestination(moveSpots[rand].position);
            patronAnimator.SetFloat("walk", navMesh.velocity.magnitude);

            if (navMesh.remainingDistance < .2f)
            {
                patronAnimator.SetFloat("walk", navMesh.velocity.magnitude);
                StartCoroutine(StopAndDrink());

            }
        }
    }

    
    private IEnumerator StopAndDrink()
    {

        navMesh.isStopped = true;
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        navMesh.isStopped = false;
        rand = Random.Range(0, moveSpots.Length);
     
    }
    

   

    private void SetRigidBodyState(bool state)
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidBodies)
        {
            rigidbody.isKinematic = state;
           
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    private void SetColliderState(bool state)
    {

        Collider[] ragdollColliders = GetComponentsInChildren<Collider>();

        foreach (Collider ragdollCollider in ragdollColliders)
        {
            ragdollCollider.enabled = state;
        }
        GetComponent<Collider>().enabled = !state;
        capsuleCollider.enabled = !state;
    }
}
