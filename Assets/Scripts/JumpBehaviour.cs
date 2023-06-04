using UnityEngine;

/// <summary>
/// Represents bunny behaviour according to animation
/// </summary>
public class JumpBehaviour : StateMachineBehaviour
{
    SpriteRenderer BunnySprite;
    BunnyScript Bunny;
    float jumpTime;
    float startX;
    float endX;
    float startY;
    float endY;
    const float JUMP_START = 0.2f;
    const float JUMP_END = 0.7f;
    const float JUMP_HEIGHT = 1.5f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Bunny == null)
        {
            Bunny = animator.GetComponent<BunnyScript>();
        }
        if (BunnySprite == null)
        {
            BunnySprite = animator.GetComponent<SpriteRenderer>();
        }
        if (Bunny.JumpDistance > 0)
        {
            BunnySprite.flipX = true;
        }
        else
        {
            BunnySprite.flipX = false;
        }
        startY = Bunny.transform.position.y;
        startX = Bunny.transform.position.x;
        endX = startX + Bunny.JumpDistance;
        endY = startY + Bunny.JumpDeltaY;
        jumpTime = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        jumpTime += Time.deltaTime;
        if (jumpTime / stateInfo.length > JUMP_END)
        {
            Bunny.transform.position = new Vector3(endX, endY, Bunny.transform.position.z);
        } else if (jumpTime / stateInfo.length > JUMP_START)
        {
            float progress = (jumpTime / stateInfo.length - JUMP_START) / (JUMP_END - JUMP_START);
            Bunny.transform.position = new Vector3(Mathf.Lerp(startX, endX, progress), Mathf.Lerp(startY, endY, progress) + JUMP_HEIGHT * Mathf.Sin(progress * Mathf.PI), Bunny.transform.position.z);
        }
        else
        {
            Bunny.transform.position = new Vector3(startX, startY, Bunny.transform.position.z);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(Bunny.transform.position.x - Bunny.TargetLocation) < Mathf.Abs(Bunny.JumpDistance) * 0.9f)
        {
            animator.SetBool("Jumping", false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
