using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideListController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> videoList;

    private void Awake()
    {
        videoList = GetChilds(transform);
    }

    private List<Transform> GetChilds(Transform parent)
    {
        List<Transform> ret = new List<Transform>();
        foreach (Transform child in parent) ret.Add(child);
        return ret;
    }

}
