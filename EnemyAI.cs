using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {
    public int enemyLives = 2;
    bool enemyRespawn = false;
    public float spawnTime = 2f;
    // What to chase?
    public Transform target;
    public Transform spawnPoint;
    public GameObject enemyUnit;
    //hero collider
    //  public Collider2D enemy_collider;
    // How many times each second we will update our path
    public float updateRate = 2f;
    //animator
    private Animator animator;
    // Caching
    private Seeker seeker;
	private Rigidbody2D rb;
    private SpriteRenderer sprite;
    //The calculated path
    public Path path;
   //

    //anims
    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
    //The AI's speed per second
    public float speed = 300f;
	public ForceMode2D fMode;
	
	[HideInInspector]
	public bool pathIsEnded = false;
	
	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	
	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;
    //
    void Start () {
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        if (target == null) {
            State = CharState.Idle;
            Debug.LogError ("No Player found? PANIC!");
			return;
		}
		
		// Start a new path to the target position, return the result to the OnPathComplete method
   //TODO: 	если не работает адекватно раскоментировать эту строку!!!! : seeker.StartPath (transform.position, target.position, OnPathComplete);
		
		StartCoroutine (UpdatePath ());
	}
    //
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check collision name
        Debug.Log("collision name = " + other.gameObject.name);

        {
            if (other.gameObject.tag == "Weapon")
            {

                enemyLives--;
                Debug.Log("Lives left " + enemyLives);
            }
            //lives--;
            //Debug.Log("Lives left "+lives);

        }
    }
    private void RespawnCheck()
    {
        if (enemyLives <= 0)
        {
            enemyRespawn = true;

        }
        else
        {
            enemyRespawn = false;
        }
        if (enemyRespawn)
        {
           
            Debug.Log("ENEMY IS DEAD - CONGRATS!");
            enemyLives = 2;
            StartCoroutine(Respawn_Now());
            //  transform.position = spawnPoint.position;
        }
    }
    public IEnumerator Respawn_Now()
    {
        
        sprite.enabled = false;
        enemyUnit.GetComponent<trigger_enemy_AI>().enabled = false;
        enemyUnit.GetComponent<EnemyAI>().enabled = false;
        enemyUnit.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(spawnTime);
        enemyUnit.transform.position = spawnPoint.position;
        sprite.enabled = true;
        enemyUnit.GetComponent<BoxCollider2D>().enabled = true;
        enemyUnit.GetComponent<trigger_enemy_AI>().enabled = true;
        enemyUnit.GetComponent<EnemyAI>().enabled = true;
    }
    //}
    //
    IEnumerator UpdatePath () {
		if (target == null) {
			//TODO: Insert a player search here.
			yield return false;
        }
        if (target == null) target = FindObjectOfType<Person>().transform;

        // Start a new path to the target position, return the result to the OnPathComplete method
        seeker.StartPath (transform.position, target.position, OnPathComplete);
		
		yield return new WaitForSeconds ( 1f/updateRate );
		StartCoroutine (UpdatePath());
	}
	
	public void OnPathComplete (Path p) {
		Debug.Log ("We got a path. Did it have an error? " + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
    private void Update()
    {
        RespawnCheck();
    }

    void FixedUpdate () {
		if (target == null) {
			//TODO: Insert a player search here.
			return;
		}

       
      
        if (path == null)
			return;
		
		if (currentWaypoint >= path.vectorPath.Count) {
			if (pathIsEnded)
				return;
			
			Debug.Log ("End of path reached.");
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;
       
        //Direction to the next waypoint
        Vector3 dir = ( path.vectorPath[currentWaypoint] - transform.position ).normalized;
        sprite.flipX = dir.x < 0.0F;
        dir *= speed * Time.fixedDeltaTime;
		//Move the AI
		rb.AddForce (dir, fMode);
        
        float dist = Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]);
		if (dist < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
    //    OnTriggerEnter2D(enemy_collider);
          State = CharState.Run;
        
    }
    public enum CharState
    {
        Idle,
        Run,
    }
}

