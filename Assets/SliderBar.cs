using UnityEngine;

public class SliderBar : MonoBehaviour
{
    [Header("Handle Movement")]
    [Tooltip("Drag the RectTransform of your handle (Meter) here.")]
    [SerializeField] private RectTransform meterHandle;

    [Tooltip("Local X position at time = 0 (start of 45s).")]
    [SerializeField] private float handleStartX = -400f;

    [Tooltip("Local X position at time = 45s.")]
    [SerializeField] private float handleEndX = 400f;

    [Tooltip("Duration in seconds over which the handle moves from startX to endX.")]
    [SerializeField] private float duration = 45f;

    private float timer = 0f;

    private void Start()
    {
        if (meterHandle != null)
        {
            Vector3 pos = meterHandle.localPosition;
            pos.x = handleStartX;
            meterHandle.localPosition = pos;
        }
    }

    private void Update()
    {
        if (meterHandle == null) return;

        // Advance the timer up to 'duration'
        if (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            Vector3 pos = meterHandle.localPosition;
            pos.x = Mathf.Lerp(handleStartX, handleEndX, t);
            meterHandle.localPosition = pos;
        }
    }
}