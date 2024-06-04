using System;
using UnityEngine;

public class KatanaAnimations : MonoBehaviour
{
    public Animator playerAnimController;

    public AnimationClip[] animationsList;

    public AnimationClip CurrentAnimationClip;

    private int index;

    private void Awake()
    {
        playerAnimController = GetComponent<Animator>();
        animationsList = playerAnimController.runtimeAnimatorController.animationClips;
        CurrentAnimationClip = animationsList[0];
        index = 0;
    }

    private void Update()
    {
        if (CurrentAnimationClip != null &&
            !playerAnimController.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnimationClip.name))
        {
            playerAnimController.Play(CurrentAnimationClip.name);
        }
    }

    [ContextMenu("PlayNextAnimation")]
    public void PlayNextAnimation() => CurrentAnimationClip = animationsList[++index % animationsList.Length];

    [ContextMenu("PlayPreviousAnimation")]
    public void PlayPreviousAnimation() => CurrentAnimationClip = animationsList[--index % animationsList.Length];
}