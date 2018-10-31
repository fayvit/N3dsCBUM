using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaBehaviour : StateMachineBehaviour {

    [SerializeField] private string nomeState;

    AnimatorStateInfo aInfo;

    public AnimatorStateInfo A_Info
    {
        get { return aInfo; }
    }

    public string NomeState
    {
        get { return nomeState; }
    }


    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        aInfo = animatorStateInfo;
        //Debug.Log("o nome do state é: " + nomeState + " : " + aInfo.IsName(nomeState));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animatorStateInfo.normalizedTime >= 0.5f)
        {
            animator.SetBool(nomeState, false);
        }
    }
}
