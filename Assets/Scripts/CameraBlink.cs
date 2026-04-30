using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CameraBlink : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField]
    private float blinkInterval = 5f;
    [SerializeField]
    private float blinkDuration = 0.5f;
    [SerializeField]
    private float blinkDurationIncrement = 0.2f;
    [SerializeField]
    private float fadeTime = 0.1f;
    private float currentBlinkDuration;
    [SerializeField]
    private Image blinkOverlay;

    [Header("Blink UI Controller")]
    [SerializeField] private TextMeshProUGUI blinkIndicatorText;
    [SerializeField] private int blinkBars = 5; // Number of “I” bars
    private float timeSinceLastBlink = 0f;

    private Coroutine blinkCoroutine;
    private bool isBlinking = false;

    private void Start()
    {
        currentBlinkDuration = blinkDuration;
        if (blinkIndicatorText != null)
            blinkIndicatorText.text = "IIIII";
        if (blinkOverlay != null)
        {
            blinkOverlay.color = new Color(0, 0, 0, 0);
            blinkCoroutine = StartCoroutine(BlinkRoutine());
        }
        else
        {
            Debug.LogError("CameraBlink: No blinkOverlay assigned.");
        }
    }

    private void Update()
    {
        timeSinceLastBlink += Time.deltaTime;
        float t = Mathf.Clamp01(timeSinceLastBlink / blinkInterval);
        int barsToShow = Mathf.CeilToInt(Mathf.Lerp(blinkBars, 0, t));
        blinkIndicatorText.text = new string('I', barsToShow).PadRight(blinkBars, ' ');
        if (Input.GetKeyDown(KeyCode.Q) && !isBlinking)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            currentBlinkDuration = blinkDuration; // Reset blink duration on Q press
            blinkCoroutine = StartCoroutine(BlinkNowAndRestart());
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);
            yield return StartCoroutine(BlinkOnce());
            currentBlinkDuration += blinkDurationIncrement;
        }
    }

    private IEnumerator BlinkNowAndRestart()
    {
        yield return StartCoroutine(BlinkOnce());
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkOnce()
    {
        isBlinking = true;
        // Fade in
        yield return StartCoroutine(FadeBlink(0f, 1f, fadeTime));
        // Hold at full black
        float holdTime = Mathf.Max(0f, currentBlinkDuration - 2f * fadeTime);
        if (holdTime > 0f)
            yield return new WaitForSeconds(holdTime);
        // Fade out
        yield return StartCoroutine(FadeBlink(1f, 0f, fadeTime));
        timeSinceLastBlink = 0f;
        isBlinking = false;
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
