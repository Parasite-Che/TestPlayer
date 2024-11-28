using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Model : MonoBehaviour
{
    [SerializeField]
    private VideoListController videoListController;
    [SerializeField]
    private PausePlayButtonController pausePlayButtonController;

    private void Start()
    {
        pausePlayButtonController.gameObject.GetComponent<Button>().onClick.AddListener(videoListController.FirstClick);
    }

}
