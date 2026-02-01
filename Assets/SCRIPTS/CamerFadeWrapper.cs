using UnityEngine;
using System.Collections;

public class CameraFadeWrapper : MonoBehaviour
{
    public Player player;
    public ScreenFader fader;

    private bool isTransitioning;

    void Start()
    {
        player.onBigRequested = () =>
        {
            if (!isTransitioning)
                StartCoroutine(Transition());
        };
    }

    IEnumerator Transition()
    {
        isTransitioning = true;

        yield return fader.FadeOut();   // fade to black

        player.ToggleBigGuy();          // switch camera WHILE black

        yield return fader.FadeIn();    // reveal

        isTransitioning = false;
    }
}
