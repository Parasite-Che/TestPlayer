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
        videoListController.Init();

        pausePlayButtonController.Init();

        pausePlayButtonController.gameObject.GetComponent<Button>().onClick.AddListener(videoListController.FirstClick);
    }

    void Update() 
    {
        // This is bad decision, unity has a new input system with beter realization
        // But here the implementing this system is unnecessary
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
