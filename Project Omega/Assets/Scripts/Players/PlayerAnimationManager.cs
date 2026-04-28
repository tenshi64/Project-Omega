using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void PlayAnimationWithTransition(string _animationName, float _transitionTime, int _layer = 0, int _startTime = 0)
    {
        if(!IsInState(_animationName, _layer))
        {
            animator.CrossFadeInFixedTime(_animationName, _transitionTime, _layer, _startTime);
        }
    }

    public void PlayAnimation(string _animationName, int _layer = 0, int _startTime = 0)
    {
        animator.Play(_animationName, _layer, _startTime);
    }

    public bool IsInState(string _animationName, int _layer = 0)
    {
        return animator.GetNextAnimatorStateInfo(_layer).IsName(_animationName);
    }
}
