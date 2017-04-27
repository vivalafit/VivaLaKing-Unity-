using UnityEngine;
using System.Collections;
using Pathfinding;

public class Person : MonoBehaviour
{


    //Respawn fields
    public int lives = 1;
    bool respawn = false;
    public float spawnTime = 3f;
    //
    [SerializeField]
    private float speed = 1.5F;
    [SerializeField]
    private float jumpForce = 9.0F;
  
    //Attack fields
    private bool attackingLeft, attackingRight = false;
    private float attackTimer = 0;
    private float attackCd = 0.3f;
    //
    private bool isGrounded = false;
    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
    public BoxCollider2D attackTriggerLeft, attackTriggerRight;
    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;
    public Transform spawnPoint;
    public GameObject player, deathParticle, spawnParticle;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        attackTriggerLeft.enabled = false;
        attackTriggerRight.enabled = false;

    }
    //Collision check - FOR PLAYER
    void OnCollisionEnter2D(Collision2D col)
    {
        //Check collision name
        Debug.Log("collision name = " + col.gameObject.name);

        {
            if (col.gameObject.tag == "Enemy")
            {

                lives--;
                Debug.Log("Lives left " + lives);
            }
            //lives--;
            //Debug.Log("Lives left "+lives);

        }

    }
    //Collision check - FOR ENEMY

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {

        State = CharState.Idle;
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonUp("Jump")) Jump();
        AttackLeft();
        AttackRight();
        RespawnCheck();

    }
    // Test Respawn

    private void RespawnCheck()
    {
        if (lives <= 0)
        {
            respawn = true;

        }
        else
        {
            respawn = false;
        }
        if (respawn)
        {
            // Destroy(player.gameObject);      
            //   player.transform.position = spawnPoint.position;
            Debug.Log("You`re Dead - respawning! ");
            lives = 1;
            StartCoroutine(Respawn_Now());
            //  transform.position = spawnPoint.position;
        }
    }
    //
    public IEnumerator Respawn_Now()
    {
        Instantiate(deathParticle, player.transform.position, player.transform.rotation);
        sprite.enabled = false;
        player.GetComponent<Person>().enabled = false;

        yield return new WaitForSeconds(spawnTime);
        player.transform.position = spawnPoint.position;
        Instantiate(spawnParticle, player.transform.position, player.transform.rotation);
        sprite.enabled = true;
        player.GetComponent<Person>().enabled = true;
    }

    //
    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        sprite.flipX = direction.x < 0.0F;
        State = CharState.Run;
    }
    //
    private void Jump()
    {
        State = CharState.Jump;
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

    }
    //
    private void AttackLeft()
    {

        if (Input.GetKeyDown("f") && !attackingLeft)
        {
            sprite.flipX = true;
            State = CharState.Attack;
            attackingLeft = true;
            attackTimer = attackCd;
            attackTriggerLeft.enabled = true;
        }

        if (attackingLeft)
        {
            if (attackTimer > 0)
            {

                attackTimer -= Time.deltaTime;

            }
            else
            {
                attackingLeft = false;

                attackTriggerLeft.enabled = false;

            }
        }
        //   animator.SetBool("Attack", attacking);
    }
    private void AttackRight()
    {

        if (Input.GetKeyDown("g") && !attackingRight)
        {
            sprite.flipX = false;
            State = CharState.Attack;
            attackingRight = true;
            attackTimer = attackCd;
            attackTriggerRight.enabled = true;
        }

        if (attackingRight)
        {
            if (attackTimer > 0)
            {

                attackTimer -= Time.deltaTime;

            }
            else
            {
                attackingRight = false;

                attackTriggerRight.enabled = false;

            }
        }
    }
    //
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5F);
        isGrounded = colliders.Length > 1;
    }

}

public enum CharState
{
    Idle,
    Run,
    Jump,
    Attack
}