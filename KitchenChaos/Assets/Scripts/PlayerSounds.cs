using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    [SerializeField] private float footstepRate = 0.1f;
    private float footstepTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            footstepTimer = footstepRate;

            if (player.Velocity > 0f)
            {
                SoundManager.Instance.PlayFootstepSound(player.transform.position, 1f);
            }
        }
    }
}
