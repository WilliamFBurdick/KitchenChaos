using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCountdownUI : MonoBehaviour
{
    const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator anim;
    private int previousNum;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void Update()
    {
        int countdownNum = Mathf.CeilToInt(GameManager.Instance.GetCountdownTimer());
        countdownText.text = countdownNum.ToString();

        if (previousNum != countdownNum)
        {
            previousNum = countdownNum;
            anim.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
