using ShGames.Gameplay.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator playerAnimController;

    const string normalVelocityX = "normal_velocityX", normalVelocityY = "normal_velocityY";

    public int normalVelX = 0, normalVelY = 0;

    private void Awake()
    {
        normalVelX = Animator.StringToHash(normalVelocityX);
        normalVelY = Animator.StringToHash(normalVelocityY);

    }

    private void Start()
    {
    }

    public void PlayerVelocityHandler(Vector3 velocity)
    {
        playerAnimController.SetFloat(normalVelX,velocity.x);
        playerAnimController.SetFloat(normalVelY,velocity.y);
    }

    private void OnDestroy()
    {
    }
}
