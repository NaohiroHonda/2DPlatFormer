using UnityEngine;
using System.Collections;

using Assets.Adds;

public class Bomb : MonoBehaviour
{
    public BombType type;
    public int bombDamage = PowerUpManager.GetBombDamage;
	public float bombRadius = PowerUpManager.GetBombRadius;			// Radius within which enemies are killed.
	public float bombForce = 100f;			// Force that enemies are thrown from the blast.
	public AudioClip boom;					// Audioclip of explosion.
	public AudioClip fuse;					// Audioclip of fuse.
	public float fuseTime = 1.5f;
	public GameObject explosion;			// Prefab of explosion effect.


	private LayBombs layBombs;				// Reference to the player's LayBombs script.
	private PickupSpawner pickupSpawner;	// Reference to the PickupSpawner script.
	private ParticleSystem explosionFX;     // Reference to the particle system of the explosion effect.

    [SerializeField]
    private EventTrigger collisionTrigger;
    [SerializeField]
    private EventTrigger exitTrigger;

    private bool isLaunched;

    void Awake ()
	{
		// Setting up references.
		explosionFX = GameObject.FindGameObjectWithTag("ExplosionFX").GetComponent<ParticleSystem>();
		pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
		if(GameObject.FindGameObjectWithTag("Player"))
			layBombs = GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
	}

	void Start ()
	{
        //collisionCollider = collisionObj.GetComponent<EventTrigger>();
        //exitTrigger = ExitObj.GetComponent<EventTrigger>();

        bombDamage = PowerUpManager.GetBombDamage;
        bombRadius = PowerUpManager.GetBombRadius;
        type = PowerUpManager.BombType;

        isLaunched = transform.root == transform;

        if (isLaunched && type != BombType.Timer)
        {
            GameObject spark = GetComponentInChildren<ParticleSystem>().gameObject;
            Destroy(spark);
        }

        // If the bomb has no parent, it has been laid by the player and should detonate.
        if (isLaunched)
        {
            //bombのScaleが0.18f
            exitTrigger.gameObject.GetComponent<CircleCollider2D>().radius = bombRadius * 5f - 1f;

            StartCoroutine(BombDetonation());
        }

	}


	IEnumerator BombDetonation()
	{
		// Play the fuse audioclip.
		AudioSource.PlayClipAtPoint(fuse, transform.position);

        if (type == BombType.Timer)
        {
            // Wait for 2 seconds.
            yield return new WaitForSeconds(fuseTime);

            // Explode the bomb.
            Explode();
        }
	}

    private void Update()
    {
        if (type == BombType.Timer || !isLaunched) return;

        if(type == BombType.Collision && collisionTrigger.Status == EventTrigger.TriggerStatus.Enter)
        {
            if (collisionTrigger.CollidedObj == null) return;
            if(collisionTrigger.CollidedObj.GetComponent<Enemy>().HP > 0) Explode();
        }
        else if(type == BombType.AI && exitTrigger.Status == EventTrigger.TriggerStatus.Exit)
        {
            if (exitTrigger.CollidedObj == null) return;
            if (exitTrigger.CollidedObj.GetComponent<Enemy>().HP > 0) Explode();
        }
    }

    public void Explode()
	{

		// Make the pickup spawner start to deliver a new pickup.
		pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

		// Find all the colliders on the Enemies layer within the bombRadius.
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemies"));

		// For each collider...
		foreach(Collider2D en in enemies)
		{
			// Check if it has a rigidbody (since there is only one per enemy, on the parent).
			Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
			if(rb != null && rb.tag == "Enemy")
			{
				// Find the Enemy script and set the enemy's health.
				rb.gameObject.GetComponent<Enemy>().HP-=bombDamage;


                if (rb.gameObject.GetComponent<Enemy>().HP <= 0) PowerUpManager.BombKill();

				// Find a vector from the bomb to the enemy.
				Vector3 deltaPos = rb.transform.position - transform.position;

				// Apply a force in this direction with a magnitude of bombForce.
				Vector3 force = deltaPos.normalized * bombForce;
				rb.AddForce(force);
			}
		}

		// Set the explosion effect's position to the bomb's position and play the particle system.
		explosionFX.transform.position = transform.position;
		explosionFX.Play();

		// Instantiate the explosion prefab.
		Instantiate(explosion,transform.position, Quaternion.identity);

		// Play the explosion sound effect.
		AudioSource.PlayClipAtPoint(boom, transform.position);

		// Destroy the bomb.
		Destroy (gameObject);
	}
}
