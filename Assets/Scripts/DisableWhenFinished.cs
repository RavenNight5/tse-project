using UnityEngine;
using UnityEngine.Video;

public class DisableWhenFinished : MonoBehaviour
{
    public GameObject menu;

    void Start()
    {
        GetComponent<VideoPlayer>().loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        menu.SetActive(true);
        gameObject.SetActive(false);
    }
}