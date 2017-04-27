using UnityEngine;
using System.Collections;

public class playerAttack : MonoBehaviour
{

    private bool attacking = false;

    private float attackTimer = 0;
    private float attackCd = 0.3f;
    private Animator animator;
    public BoxCollider2D attackTrigger;

    private Animator anim;
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if(Input.GetKeyDown("f")&& !attacking)
        {
            State = CharState.Attack;
            attacking = true;
            attackTimer = attackCd;
            attackTrigger.enabled = true;
        }

        if(attacking)
        {
            if(attackTimer>0)
            {

                attackTimer -= Time.deltaTime;

            }
            else
            {
                attacking = false;

                attackTrigger.enabled = false;

            }
        }


        anim.SetBool("Attack", attacking);
    }
    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
    public enum CharState
    {
      
        Attack = 3
    }
}
