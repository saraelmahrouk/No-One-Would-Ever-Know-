using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    private static FadeManager instance;
    private Image fadeImage;
    public float fadeDuration = 2f;

    void Awake()
    {
        // Only one FadeManager ever exists
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Build the canvas
        Canvas c = gameObject.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        c.sortingOrder = 999;
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(transform, false);
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);

        RectTransform rt = fadeImage.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    public static void FadeToScene(int sceneIndex)
    {
        instance.StartCoroutine(instance.FadeOut(sceneIndex));
    }

    IEnumerator FadeIn()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, 1f - Mathf.Clamp01(timer / fadeDuration));
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    IEnumerator FadeOut(int sceneIndex)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(timer / fadeDuration));
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }
}