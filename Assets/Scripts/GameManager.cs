using System;
using UnityEngine;

public enum GameState { Paused, Running, TimelineControlled }

public class GameManager : MonoBehaviour
{
    public static GameState State { get; private set; } = GameState.Paused;
    public static bool isPaused { get {  return State == GameState.Paused; } }

    public static Action onPause, onResume, onTimeline;

    private void Awake()
    {
        PlayerData.LoadData();

        Screen.SetResolution(PlayerData.resolution.width, PlayerData.resolution.height, FullScreenMode.FullScreenWindow, PlayerData.resolution.refreshRateRatio);
    }

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

    public static void SaveGame()
    {
        PlayerData.SaveData();
    }

    public static void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
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

    // FOR MOBILE
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            PlayerData.SaveData();
        }
    }

    // FOR PC
    private void OnApplicationQuit()
    {
        PlayerData.SaveData();
    }
}

public static class PlayerData
{
    public static bool showCalibrationAtStart { get; set; }
    public static float volumeMultiplier { get; set; }


    // Casual settings
    public static Resolution resolution { get; set; }
    public static int graphicsMode { get; set; }
    public static float sfxLevel { get; set; }
    public static float musicLevel { get; set; }

    public static void LoadData()
    {
        showCalibrationAtStart = PlayerPrefs.GetInt("Show Calibration", 1) == 1;
        volumeMultiplier = PlayerPrefs.GetFloat("Volume Multiplier", 1f);

        Resolution res = new Resolution();
        res.width = PlayerPrefs.GetInt("Screen X", Screen.currentResolution.width);
        res.height = PlayerPrefs.GetInt("Screen Y", Screen.currentResolution.height);
        RefreshRate refreshRate = new RefreshRate();
        refreshRate.numerator = (uint)PlayerPrefs.GetInt("Refresh Rate Numerator", (int)Screen.currentResolution.refreshRateRatio.numerator);
        refreshRate.denominator = (uint)PlayerPrefs.GetInt("Refresh Rate Denominator", (int)Screen.currentResolution.refreshRateRatio.numerator);
        res.refreshRateRatio = refreshRate;
        resolution = res;

        graphicsMode = PlayerPrefs.GetInt("Graphics Mode", 1);
        sfxLevel = PlayerPrefs.GetFloat("SFX Level", 1f);
        musicLevel = PlayerPrefs.GetFloat("Music Level", 1f);
    }

    public static void SaveData()
    {
        PlayerPrefs.SetInt("Show Calibration", showCalibrationAtStart ? 1 : 0);
        PlayerPrefs.SetFloat("Volume Multiplier", volumeMultiplier);

        PlayerPrefs.SetInt("Screen X", resolution.width);
        PlayerPrefs.SetInt("Screen Y", resolution.height);
        PlayerPrefs.SetInt("Refresh Rate Numerator", (int)resolution.refreshRateRatio.numerator);
        PlayerPrefs.SetInt("Refresh Rate Denominator", (int)resolution.refreshRateRatio.denominator);

        PlayerPrefs.SetInt("Graphics Mode", graphicsMode);
        PlayerPrefs.SetFloat("SFX Level", sfxLevel);
        PlayerPrefs.SetFloat("Music Level", musicLevel);
    }
}