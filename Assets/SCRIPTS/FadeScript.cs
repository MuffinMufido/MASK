using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.8f;

    void Awake()
    {
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;
    }


    

    public IEnumerator FadeOut()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            float eased = Mathf.SmoothStep(0f, 1f, t);
            c.a = Mathf.Lerp(from, to, eased);
            fadeImage.color = c;
            yield return null;
        }
    }
}
