using UnityEngine;
using System.Linq;

public class PlayerAnimEvents : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Collider[] rightArmCols;
    [SerializeField] private Collider[] leftArmCols;
    [SerializeField] private Collider[] rightLegCols;
    [SerializeField] private Collider[] leftLegCols;

    [SerializeField] private Animator camAnimator;

    // this isnt needed really, i just wanted to know how to use delegates
    public delegate void DisableAllColliders();

    private DisableAllColliders DisableAll;

    void Start()
    {
        DisableAll += DisableRightArmColliders;
        DisableAll += DisableLeftArmColliders;
        DisableAll += DisableLeftLegColliders;
        DisableAll += DisableRightLegColliders;
        DisableAll();


    }

    // disables and enables colliders for player to avoid damaging enemy without attacking. 
    // need multiple methods for anim events.

    public void DisableRightArmColliders() => rightArmCols.ToList().ForEach((Collider x) => x.enabled = false);


    public void EnableRightArmColliders() => rightArmCols.ToList().ForEach((Collider x) => x.enabled = true);

    public void EnableLeftArmColliders() => leftArmCols.ToList().ForEach((Collider x) => x.enabled = true);

    public void DisableLeftArmColliders() => leftArmCols.ToList().ForEach((Collider x) => x.enabled = false);

    public void EnableRightLegColliders() => rightLegCols.ToList().ForEach((Collider x) => x.enabled = true);

    public void DisableRightLegColliders() => rightLegCols.ToList().ForEach((Collider x) => x.enabled = false);

    public void EnableLeftLegColliders() => leftLegCols.ToList().ForEach((Collider x) => x.enabled = true);

    public void DisableLeftLegColliders() => leftLegCols.ToList().ForEach((Collider x) => x.enabled = false);

}
