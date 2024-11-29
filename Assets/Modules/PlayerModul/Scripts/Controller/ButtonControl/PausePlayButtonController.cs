using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class PausePlayButtonController : MonoBehaviour
{
    [SerializeField]
    private Sprite pauseIcon;
    [SerializeField]
    private Sprite playIcon;
    
    [SerializeField]
    private bool isPlaying = false;


    public void Init()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonLogic);

        AVProController.Instance.DisplayUGUI().CurrentMediaPlayer.Events.AddListener(HandleEvent);
    }

    private void HandleEvent(MediaPlayer mp, MediaPlayerEvent.EventType eventType, ErrorCode code)
    {
        Debug.Log("MediaPlayer " + mp.name + " generated event: " + eventType.ToString());
        if (eventType == MediaPlayerEvent.EventType.Error)
        {
            Debug.LogError("Error: " + code);
        }
        else if (eventType == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            ButtonLogic();
        }
        else if (eventType == MediaPlayerEvent.EventType.Paused)
        {
            Pause();
        }
    }

    private void ButtonLogic()
    {
        if (!isPlaying)   // Play
        {
            Play();
        }
        else            // Pause
        {
            Pause();
        }
    }

    private void Play()
    {
        gameObject.GetComponent<Image>().sprite = pauseIcon;
        AVProController.Instance.PlayVideo();
        isPlaying = true;
    }

    private void Pause()
    {
        gameObject.GetComponent<Image>().sprite = playIcon;
        AVProController.Instance.PauseVideo();
        isPlaying = false;
    }
}
