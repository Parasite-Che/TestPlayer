using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVProController : MonoBehaviour
{
    [SerializeField]
    private DisplayUGUI displayUGUI;

    private static AVProController instance;

    public static AVProController Instance {  get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        displayUGUI = GetComponent<DisplayUGUI>();
    }

    public DisplayUGUI DisplayUGUI() { return displayUGUI; }

    public void PlayVideo()
    {
        displayUGUI?.CurrentMediaPlayer.Play();
    }

    public void PauseVideo()
    {
        displayUGUI?.CurrentMediaPlayer.Pause();
    }

}
