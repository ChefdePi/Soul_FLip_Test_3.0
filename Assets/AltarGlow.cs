using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AltarGlow : MonoBehaviour
{
    public Light2D altarLight;
    public float glowSpeed = 0.5f;     // Breathing speed
    public float minRadius = 1.8f;     // Smallest outer radius
    public float maxRadius = 2.2f;     // Largest outer radius

    void Update()
    {
        if (altarLight != null)
        {
            // Normalized value between 0 and 1 based on sine wave
            float t = (Mathf.Sin(Time.time * glowSpeed) + 1f) * 0.5f;

            // Lerp between min and max
            altarLight.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, t);
        }
    }
}