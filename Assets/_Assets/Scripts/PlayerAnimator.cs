using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int IS_WALKING_HASH = Animator.StringToHash("IsWalking");
    private Animator animator;

    [SerializeField] private Player player;

    private void Awake()
    {
        animator= GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING_HASH, player.IsWalking());
    }
}
