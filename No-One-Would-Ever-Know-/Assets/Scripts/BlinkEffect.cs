using System.Collections;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    public CanvasGroup blinkOverlay;

    [Header("Blink Settings")]
    public float fadeSpeed = 0.15f;
    public float openDuration = 1.2f;
    public float openTime = 0.4f;
    public float closedTime = 0.15f;

    void Start()
    {
        if (blinkOverlay == null)
        {
            Debug.LogError("Blink Overlay is not assigned.");
            return;
        }

        StartCoroutine(BlinkSequence());
    }

    IEnumerator BlinkSequence()
    {
        blinkOverlay.gameObject.SetActive(true);

        blinkOverlay.alpha = 1f;

        yield return new WaitForSeconds(0.5f);

        yield return FadeTo(0f, openDuration);

        yield return new WaitForSeconds(openTime);

        yield return FadeTo(1f, fadeSpeed);
        yield return new WaitForSeconds(closedTime);
        yield return FadeTo(0f, fadeSpeed);

        yield return new WaitForSeconds(0.3f);

        yield return FadeTo(1f, fadeSpeed);
        yield return new WaitForSeconds(closedTime);
        yield return FadeTo(0f, fadeSpeed);

        blinkOverlay.gameObject.SetActive(false);
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = blinkOverlay.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            blinkOverlay.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        blinkOverlay.alpha = targetAlpha;
    }
}