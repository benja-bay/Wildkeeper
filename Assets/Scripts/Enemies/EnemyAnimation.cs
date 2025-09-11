using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    private Vector2 lastDirection = Vector2.down;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int AttackX = Animator.StringToHash("AttackX");
    private static readonly int AttackY = Animator.StringToHash("AttackY");

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Vector2 velocity = new Vector2(agent.velocity.x, agent.velocity.y);
        bool isMoving = velocity.sqrMagnitude > 0.01f;

        animator.SetBool(IsMoving, isMoving);

        if (isMoving)
        {
            lastDirection = velocity.normalized;
        }

        animator.SetFloat(MoveX, lastDirection.x);
        animator.SetFloat(MoveY, lastDirection.y);
    }
    
    public void PlayAttack()
    {
        animator.SetBool(IsMoving, false);
        animator.SetBool(IsAttacking, true);
        animator.SetFloat(AttackX, lastDirection.x);
        animator.SetFloat(AttackY, lastDirection.y);
        animator.CrossFade("Attack", 0.1f);
    }

    public void StopAttack()
    {
        animator.SetBool(IsAttacking, false);
    }
}

