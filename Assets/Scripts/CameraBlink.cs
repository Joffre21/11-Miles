using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraBlink : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField]
    private float blinkInterval = 5f;
    [SerializeField]
    private float blinkDuration = 0.15f;
    [SerializeField]
    private Image blinkOverlay;

    private void Start()
    {
        if (blinkOverlay != null)
        {
            blinkOverlay.color = new Color(0, 0, 0, 0);
            StartCoroutine(BlinkRoutine());
        }
        else
        {
            Debug.LogError("CameraBlink: No blinkOverlay assigned.");
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);
            // Fade in
            yield return StartCoroutine(FadeBlink(0f, 1f, blinkDuration * 0.5f));
            // Fade out
            yield return StartCoroutine(FadeBlink(1f, 0f, blinkDuration * 0.5f));
        }
    }

    private IEnumerator FadeBlink(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            blinkOverlay.color = new Color(0, 0, 0, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        blinkOverlay.color = new Color(0, 0, 0, to);
    }
}
