using System;
using System.Collections;      // ← Add this line
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Cycle Settings")]
    [SerializeField] private float phaseDuration = 45f;
    [SerializeField] private int cyclesToWin    = 3;

    [Header("Harmony Settings")]
    [Range(0,100)] [SerializeField] private float startingHarmony   = 50f;
    [SerializeField] private float decayRatePerSecond             = 1f;
    [SerializeField] private float woodValue     = 1f;
    [SerializeField] private float stoneValue    = 2f;
    [SerializeField] private float shardValue    = 4f;
    [SerializeField] private float crystalValue  = 5f;

    private float harmony;
    private bool  lightAlive  = true;
    private bool  darkAlive   = true;
    private int   cyclesCompleted = 0;

    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Fired any time harmony changes (due to decay or a feed). 
    /// The parameter is the raw harmony in [0,100].
    /// </summary>
    public static event Action<float> OnBalanceChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        harmony           = Mathf.Clamp(startingHarmony, 0f, 100f);
        lightAlive        = true;
        darkAlive         = true;
        cyclesCompleted   = 0;

        OnBalanceChanged?.Invoke(harmony);

        StartCoroutine(RunFullCycles());
    }

    private IEnumerator RunFullCycles()
    {
        while (cyclesCompleted < cyclesToWin && lightAlive && darkAlive)
        {
            yield return RunPhase(PhaseType.Light);
            if (!lightAlive || !darkAlive) { LoseGame(); yield break; }

            yield return RunPhase(PhaseType.Dark);
            if (!lightAlive || !darkAlive) { LoseGame(); yield break; }

            cyclesCompleted++;
        }

        if (cyclesCompleted >= cyclesToWin && lightAlive && darkAlive)
            WinGame();
        else
            LoseGame();
    }

    private enum PhaseType { Light, Dark }

    private IEnumerator RunPhase(PhaseType phase)
    {
        float timer = 0f;
        while (timer < phaseDuration && lightAlive && darkAlive)
        {
            float dt = Time.deltaTime;
            timer += dt;

            // Passive decay toward 50 (the “balanced” midpoint)
            float mid = 50f;
            if      (harmony > mid) harmony = Mathf.Max(harmony - decayRatePerSecond * dt, mid);
            else if (harmony < mid) harmony = Mathf.Min(harmony + decayRatePerSecond * dt, mid);

            OnBalanceChanged?.Invoke(harmony);

            // Check collapse
            if      (harmony <= 0f)  { lightAlive = false; yield break; }
            else if (harmony >= 100f) { darkAlive  = false; yield break; }

            yield return null;
        }
    }

    public void OnHarvestWood()
    {
        if (!lightAlive || !darkAlive) return;
        AddHarmony(woodValue);
    }

    public void OnHarvestStone()
    {
        if (!lightAlive || !darkAlive) return;
        AddHarmony(stoneValue);
    }

    public void OnHarvestShard()
    {
        if (!lightAlive || !darkAlive) return;
        AddHarmony(shardValue);
    }

    public void OnHarvestCrystal()
    {
        if (!lightAlive || !darkAlive) return;
        SubtractHarmony(crystalValue);
    }

    private void AddHarmony(float amount)
    {
        harmony = Mathf.Clamp(harmony + amount, 0f, 100f);
        if (harmony >= 100f) darkAlive = false;
        OnBalanceChanged?.Invoke(harmony);
    }

    private void SubtractHarmony(float amount)
    {
        harmony = Mathf.Clamp(harmony - amount, 0f, 100f);
        if (harmony <= 0f) lightAlive = false;
        OnBalanceChanged?.Invoke(harmony);
    }

    private void WinGame()
    {
        Debug.Log("You survived all cycles! You win!");
        // TODO: Show win UI, stop game, etc.
    }

    private void LoseGame()
    {
        if (!lightAlive)
            Debug.Log("Overworld collapsed! You lose!");
        else if (!darkAlive)
            Debug.Log("Underworld collapsed! You lose!");
        else
            Debug.Log("Game Over!");
        // TODO: Show lose UI, stop game, etc.
    }

    public float GetHarmony()       => harmony;
    public int   GetCyclesCompleted() => cyclesCompleted;
}
