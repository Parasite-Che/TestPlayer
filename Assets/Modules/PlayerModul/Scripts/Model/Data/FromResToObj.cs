using System.IO;
using UnityEngine;


public class FromResToObj
{
    // Чтобы загрузить спрайт из папки на устройстве использовать метод ниже 
    // SpriteFromRes( "/sprites/achievements_icons/thumb1.png" /* полный путь к файлу */, 500 /* ширина спрайта */ , 500 /* Высота спрайта */); // возвращает найденный спрайт

    private static byte[] getImageByte(string imagePath)
    {
        // Чтение в файл
        FileStream files = new FileStream(imagePath, FileMode.Open);
        // Создать новый объект битового потока
        byte[] imgByte = new byte[files.Length];
        // Записать файл в соответствующий объект потока битов
        files.Read(imgByte, 0, imgByte.Length);
        // Закрыть файл
        files.Close();
        // Возвращаем значение битового потока
        return imgByte;
    }
    
    public static Sprite SpriteFromRes(string endOfPath, float imgWidth, float imgHeight)
    {
#if UNITY_EDITOR
        string path = Application.dataPath;
#else
        string path = Application.persistentDataPath;
#endif
        //Debug.Log(path);
        if (File.Exists(path + endOfPath))
        {
            Texture2D t2d = new Texture2D((int)imgWidth, (int)imgHeight);
            // Согласно Road King, читаем поток байтов и преобразуем его в картинку
            t2d.LoadImage(getImageByte(path + endOfPath));

            return (Sprite.Create(t2d, new Rect(0, 0, imgWidth, imgHeight), Vector2.zero));
        }
        else
        {
            Debug.LogError("Файл" + endOfPath + " не существует, либо неверно указан путь");
            return null;
        }
        
    }
}