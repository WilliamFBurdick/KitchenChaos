using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYERPREFS_SFX_VOLUME = "sfxVolume";
    public static SoundManager Instance;

    [SerializeField] private AudioClipsSO audioClipRefs;
    private float volume = 1f;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYERPREFS_SFX_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;

        Player.Instance.OnPickedUpSomething += Player_OnPickedUpSomething;
        BaseCounter.OnObjectPlaced += BaseCounter_OnObjectPlaced;

        TrashCounter.OnTrashObject += TrashCounter_OnTrashObject;
    }

    private void TrashCounter_OnTrashObject(object sender, System.EventArgs e)
    {
        TrashCounter counter = sender as TrashCounter;
        PlaySound(audioClipRefs.trash, counter.transform.position);
    }

    private void BaseCounter_OnObjectPlaced(object sender, System.EventArgs e)
    {
        BaseCounter counter = sender as BaseCounter;
        PlaySound(audioClipRefs.objectDrop, counter.transform.position);
    }

    private void Player_OnPickedUpSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefs.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefs.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefs.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefs.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volumeMultiplier * volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void PlayFootstepSound(Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipRefs.footstep, position, volumeMultiplier * volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefs.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefs.warning, position);
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYERPREFS_SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() { return volume; }
}
