using UnityEngine;
/// <summary>
/// Handles damage Detection in the game
/// Uses Damage Check to check if the condition is valid
/// Initialise Damage Checker to get different damage mode
/// </summary>
//[RequireComponent(typeof(CollisionPath))] // Ensures there is a Collider2D component on the GameObject
public class DamageArea : MonoBehaviour
{
    private Collider2D collision;
    //private LayerMask playerLayer = LayerManager.PlayerLayerMask;
    [SerializeField] private float damageAmount = 10.0f;
    [SerializeField] private DamageAreaType damageAreaType = DamageAreaType.SingleTargetMultipleHit;

    private StatsPawn instigator; // namely the player him self;



    private DamageChecker damageChecker;

    // Use this for initialization
    void Start()
    {
        collision = GetComponent<Collider2D>();
        Initialize(gameObject);
    }
    private void Update()
    {
        if(damageChecker!=null)
        {
            damageChecker.Update(Time.deltaTime);
        }
    }


    public void Initialize(GameObject instigator)
    {
        if (instigator.TryGetComponent<StatsPawn>(out var statsPawn))
        {
            this.instigator = statsPawn;
        }
        else
        {
            Debug.LogWarning("StatsPawn component not found on instigator.");
        }

        InitializeDamageAreaType(damageAreaType);
        //Invoke("DestroyDamageArea", 5f);
    }


    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     TryToApplyDamage(other);
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        TryToApplyDamage(other);
    }

    private void TryToApplyDamage(Collider2D other)
    {
        //if (instigator == null) return;
        var tar = other.gameObject;

        var o = other.GetComponent<StatsPawn>();
        if(damageChecker?.AddDamagedObjects(tar) ?? false)
        if (o == null) return;
        o.TakDamage(damageAmount);
        damageChecker.HitCounter();
    }

    private void DestroyDamageArea()
    {
        Debug.LogWarning("DestroyDamageArea Need to find a smarter ways");
        Destroy(gameObject);
    }

    #region -Debug-
    /// <summary>
    /// Debug Section
    /// </summary>
    private void OnDrawGizmos()
    {
        // Ensure the collider is retrieved in the case that it's null
        if (!collision)
        {
            collision = GetComponent<Collider2D>();
        }

        if (collision)
        {
            Gizmos.color = Color.red; // Set the color of the Gizmo

            // Check if the collider is a CircleCollider2D and cast it
            if (collision is CircleCollider2D circleCollider)
            {
                // Take local scale into account to get the actual world size of the collider
                float radius = circleCollider.radius * transform.lossyScale.x; // Use lossyScale for world scale

                // The center of the CircleCollider2D is in local space, convert it to world space
                Vector3 colliderWorldCenter = transform.position + (Vector3)circleCollider.offset;

                // Draw the Gizmo with the same radius as the collider
                Gizmos.DrawWireSphere(colliderWorldCenter, radius);
            }
        }
    }
    #endregion 
    //*******************************************//
    //***************Delegations*****************//
    //*******************************************//
    /// <summary>
    /// Initialise the damage type
    /// </summary>
    #region -Initialisation-
    private void InitializeDamageAreaType(DamageAreaType damageAreaType)
    {
        switch(damageAreaType)
        {
            case DamageAreaType.SingleTargetOneHit:
                SingleTargetOneHit();
                break;
            case DamageAreaType.SingleTargetMultipleHit:
                SingleTargetMultipleHit();
                break;
            case DamageAreaType.MultipleTargetsOneHit:
                MultipleTargetsOneHit();
                break;
            case DamageAreaType.MultipleTargetsMultipleHit:
                MultipleTargetsMultipleHit();
                break;
            default:
                SingleTargetOneHit();
                break;
        }

    }

    private int manyStrikes = -1;
    private int manyTargets = 100;

    private void SingleTargetOneHit()
    {
        damageChecker = new DamageChecker(1, 1, float.MaxValue/2);
    }

    private void SingleTargetMultipleHit()
    {
        damageChecker = new DamageChecker(1, manyStrikes);
    }

    private void MultipleTargetsOneHit()
    {
        damageChecker = new DamageChecker(manyTargets, -1, float.MaxValue/2);

    }

    private void MultipleTargetsMultipleHit()
    {
        damageChecker = new DamageChecker(manyTargets, manyStrikes);

    }


    //Damage OnTriggerEnter();
    private bool CheckTagsAndLayer(GameObject target)
    {
        // Correct layer && tags
        if (AllowedLayers(target.layer))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check tags, not in use
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    private bool AllowedTags(string tag)
    {
        //return tagsToDamage.Contains(tag);
        return true;
    }
  
    private bool AllowedLayers(LayerMask layer)
    {
        return true;
    }

    private bool IsSelf(GameObject target)
    {
        return true;
    }
    #endregion

}


public enum DamageAreaType
{
    SingleTargetOneHit,
    SingleTargetMultipleHit,
    MultipleTargetsOneHit,
    MultipleTargetsMultipleHit
}