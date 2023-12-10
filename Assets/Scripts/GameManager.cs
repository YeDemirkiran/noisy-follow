using System;
using UnityEngine;

public enum GameState { Paused, Running, TimelineControlled }

public class GameManager : MonoBehaviour
{
    public static GameState State { get; private set; } = GameState.Running;
    public static bool isPaused { get {  return State == GameState.Paused; } }

    public static Action onPause, onResume, onTimeline;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public static void Pause()
    {
        OnPause();
        Debug.Log("Game Paused");
    }

    public static void Resume()
    {
        OnResume();
        Debug.Log("Game Resumed");
    }

    static void OnPause()
    {
        State = GameState.Paused;

        Time.timeScale = 0f;

        onPause?.Invoke();
    }

    static void OnResume()
    {
        State = GameState.Running;

        Time.timeScale = 1f;

        onResume?.Invoke();
    }
}