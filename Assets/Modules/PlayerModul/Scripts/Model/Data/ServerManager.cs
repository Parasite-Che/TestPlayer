using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class ServerManager
{
    public static bool ExistingCheck(string pathEnd)
    {
#if UNITY_EDITOR
        string path = Application.dataPath;
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
#endif
        if (File.Exists(path + pathEnd))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    #region LoadImageFromServer(string url)
    public static IEnumerator LoadFileFromServer(string url, string fileName, string directory, IEnumerator enumerator) //TMP_Text loadingText, Slider bar)
    {
        // ссылка откуда качаем
        var wwwRequest = new UnityWebRequest(url);
        wwwRequest.method = UnityWebRequest.kHttpVerbGET;
        // тут куда качаем наш файл в системе, обязательно использовать Application.persistentDataPath
#if UNITY_EDITOR
        string path = Application.dataPath;
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
#endif
        //Debug.Log(path);
        if (Directory.Exists(path + directory) == false) {
            Directory.CreateDirectory(path  + directory);
        }

        if (File.Exists(path + directory + fileName) == false)
        {
            var dh = new DownloadHandlerFile(path + directory + fileName);
            Debug.Log("------LoadFileFromServer------");
            dh.removeFileOnAbort = true; // Удалить файл при неудачном скачивании
            wwwRequest.downloadHandler = dh;
            
            wwwRequest.SendWebRequest();
            //yield return wwwRequest.SendWebRequest();
            while (wwwRequest.isDone != true)
            {
                //bar.value = Math.Abs(wwwRequest.downloadProgress);
                //loadingText.text = "Скачивание файла " + fileName + ": " + (wwwRequest.downloadProgress * 100).ToString();
                yield return null;
                //Debug.Log(wwwRequest.isDone);
            }
            
            if (wwwRequest.isNetworkError || wwwRequest.isHttpError)
            {
                Debug.Log(wwwRequest.error);
            }
            else
            {
                Debug.Log(wwwRequest.result);
            }
        }
        yield return wwwRequest;
        yield return enumerator;
    }
    #endregion

}
