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

    private void Awake()
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
        if (eventType == MediaPlayerEvent.EventType.FinishedPlaying 
            || eventType == MediaPlayerEvent.EventType.Paused 
            || eventType == MediaPlayerEvent.EventType.Started
            || eventType == MediaPlayerEvent.EventType.Unpaused)
        {
            ButtonLogic();
        }
    }


    private void ButtonLogic()
    {
        if (!isPlaying)   // Play
        {
            gameObject.GetComponent<Image>().sprite = pauseIcon;
            AVProController.Instance.PlayVideo();
        }
        else            // Pause
        {
            gameObject.GetComponent<Image>().sprite = playIcon;
            AVProController.Instance.PauseVideo();
        }
        isPlaying = !isPlaying;
    }

}
