using System;
using UnityEditor;
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


    [ContextMenu("PlayNextAnimation")]
    public void PlayNextAnimation()
    {
        if(!isActiveAndEnabled || !EditorApplication.isPlaying) return;
        var d = ++index;
        index = d % animationsList.Length;
        
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        CurrentAnimationClip = animationsList[index];
        
        if (CurrentAnimationClip != null )
        {
            playerAnimController.Play(CurrentAnimationClip.name);
        }
    }

    [ContextMenu(nameof(PlayPreviousAnimation))]
    public void PlayPreviousAnimation()
    {
        if(!isActiveAndEnabled || !EditorApplication.isPlaying) return;
        var d = --index;
        index = d % animationsList.Length;

        PlayAnimation();
    }
}