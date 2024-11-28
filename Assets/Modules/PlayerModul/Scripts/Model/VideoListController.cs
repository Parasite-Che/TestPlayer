using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoListController : MonoBehaviour
{
    private MediaURLs mediaURLs;

    [SerializeField]
    private List<Transform> videoList;

    private void Awake()
    {
        videoList = GetChilds(transform);
        Init();
    }

    private void Init()
    {
        mediaURLs = JsonController<MediaURLs>.LoadFromResources("MediaURLs.json");
        for (int i = 0; i < mediaURLs.URLs.Length; i++)
        {
            videoList[i].GetChild(1).GetComponent<TMP_Text>().text = mediaURLs.URLs[i].Name;
            StartCoroutine(PictureLoader(videoList[i].GetChild(0).GetChild(0).gameObject));
            StartCoroutine(ServerManager.LoadFileFromServer(
                mediaURLs.URLs[i].ImgURLs, 
                i + ".jpg", 
                "/Previews/",
                EndLoading(videoList[i], "/Previews/" + i + ".jpg", null)));
        }
    }

    private IEnumerator PictureLoader(GameObject loadingIcon)
    {
        while (loadingIcon.activeInHierarchy)
        {
            loadingIcon.transform.Rotate(0, 0, -45);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator EndLoading(Transform videoButton, string endOfPath, IEnumerator enumerator)
    {
        videoButton.GetComponent<Image>().sprite = FromResToObj.SpriteFromRes(endOfPath, 512, 256);
        videoButton.GetChild(0).gameObject.SetActive(false);
        yield return enumerator;
    }

    private List<Transform> GetChilds(Transform parent)
    {
        List<Transform> ret = new List<Transform>();
        foreach (Transform child in parent) ret.Add(child);
        return ret;
    }

}

struct MediaURLs
{

    public (string Name, string ImgURLs, string VideoURLs)[] URLs { get; set; }

    //public MediaURLs(MediaURLs _URLs)
    //{
    //    for (int i = 0; i < _URLs.URLs.Length; i++ )
    //    {
    //        URLs[i].Name = _URLs.URLs[i].Name;
    //        URLs[i].ImgURLs = _URLs.URLs[i].ImgURLs;
    //        URLs[i].VideoURLs = _URLs.URLs[i].VideoURLs;
    //    }
    //}
}

