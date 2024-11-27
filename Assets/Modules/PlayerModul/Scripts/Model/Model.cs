using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    private MediaURLs mediaURLs;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        mediaURLs = JsonController<MediaURLs>.LoadFromResources("MediaURLs.json");
        Debug.Log(mediaURLs.URLs[0].Name);

        for (int i = 0; i < mediaURLs.URLs.Length; i++)
        {
            StartCoroutine(ServerManager.LoadFileFromServer(mediaURLs.URLs[i].ImgURLs, i + ".jpg", "/Previews/"));
        }

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
