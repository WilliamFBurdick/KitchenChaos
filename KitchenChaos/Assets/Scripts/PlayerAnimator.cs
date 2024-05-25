using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    const string IS_WALKING = "IsWalking";

    private Animator anim;
    private Player player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        anim.SetBool(IS_WALKING, player.Velocity != 0);
    }
}
