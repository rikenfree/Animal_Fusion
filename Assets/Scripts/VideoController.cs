using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public static VideoController Instance;

    public VideoPlayer generateVideoPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OnGeneratePlayVideo()
    {
        generateVideoPlayer.Play();

        if (generateVideoPlayer != null)
        {
            generateVideoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        UIManager.Instance.OnClickVideo(false);
    }
}
