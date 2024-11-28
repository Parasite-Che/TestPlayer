using RenderHeads.Media.AVProVideo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoPlayButton : MonoBehaviour
{
    public string VideoURL;
    public string VideoName;
    public TMP_Text CurentVideoNameObj;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayVideoURL);
    }

    public void PlayVideoURL()
    {
        if (AVProController.Instance.DisplayUGUI().CurrentMediaPlayer.MediaPath.Path != VideoURL)
        {
            AVProController.Instance.DisplayUGUI().CurrentMediaPlayer.OpenMedia(
            new MediaPath(VideoURL, MediaPathType.AbsolutePathOrURL),
            autoPlay: false);
            CurentVideoNameObj.text = VideoName;
        }
    }
}
