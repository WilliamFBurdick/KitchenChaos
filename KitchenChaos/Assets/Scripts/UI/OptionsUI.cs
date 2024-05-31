using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button sfxButton;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button closeButton;

    // Key rebinding
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Button interactButton;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private Button pauseButton;
    // Gamepad rebinding
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [SerializeField] private Button gamepadPauseButton;

    [SerializeField] private Transform pressToRebindTransform;

    private Action onCloseOptions;

    private void Awake()
    {
        Instance = this;
        sfxButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseOptions();
        });

        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding. Move_Left);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Right);
        });
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });
        interactAltButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.InteractAlternate);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });

        gamepadInteractButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Gamepad_Interact);
        });
        gamepadInteractAltButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Gamepad_InteractAlternate);
        });
        gamepadPauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Gamepad_Pause);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
        HideRebindOverlay();
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        sfxText.text = "SFX: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f).ToString();

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    public void Show(Action onCloseOptions)
    {
        this.onCloseOptions = onCloseOptions;
        gameObject.SetActive(true);

        sfxButton.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowRebindOverlay()
    {
        pressToRebindTransform.gameObject.SetActive(true);  
    }

    private void HideRebindOverlay()
    {
        pressToRebindTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowRebindOverlay();
        GameInput.Instance.RebindBinding(binding, () => {
            HideRebindOverlay();
            UpdateVisual();
            });
    }
}
