using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum State
    {
        WaitingToStart, Countdown, Playing, GameOver
    }

    private State state;
    private float countdownTimer = 3f;
    private float gameTimer = 10f;
    private float gameMaxTime = 3 * 60f;
    private bool paused = false;

    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.Countdown;
            OnStateChanged?.Invoke(this, new EventArgs());
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch(state)
        {
            case State.WaitingToStart:
                break;
            case State.Countdown:
                countdownTimer -= Time.deltaTime;
                if (countdownTimer < 0f)
                {
                    state = State.Playing;
                    gameTimer = gameMaxTime;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                gameTimer -= Time.deltaTime;
                if (gameTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
            default:
                break;

        }
    }

    public bool IsGamePlaying()
    {
        return state == State.Playing;
    }

    public bool IsCountdownActive()
    {
        return state == State.Countdown;
    }

    public float GetCountdownTimer()
    {
        return countdownTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGameTimerNormalized()
    {
        return (1 - (gameTimer / gameMaxTime));
    }

    public void TogglePauseGame()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
