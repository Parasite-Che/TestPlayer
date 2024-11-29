using System.IO;
using UnityEngine;


public class FromResToObj
{
    // ����� ��������� ������ �� ����� �� ���������� ������������ ����� ���� 
    // SpriteFromRes( "/sprites/achievements_icons/thumb1.png" /* ������ ���� � ����� */, 500 /* ������ ������� */ , 500 /* ������ ������� */); // ���������� ��������� ������

    private static byte[] getImageByte(string imagePath)
    {
        // ������ � ����
        FileStream files = new FileStream(imagePath, FileMode.Open);
        // ������� ����� ������ �������� ������
        byte[] imgByte = new byte[files.Length];
        // �������� ���� � ��������������� ������ ������ �����
        files.Read(imgByte, 0, imgByte.Length);
        // ������� ����
        files.Close();
        // ���������� �������� �������� ������
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
            // �������� Road King, ������ ����� ������ � ����������� ��� � ��������
            t2d.LoadImage(getImageByte(path + endOfPath));

            return (Sprite.Create(t2d, new Rect(0, 0, imgWidth, imgHeight), Vector2.zero));
        }
        else
        {
            Debug.LogError("����" + endOfPath + " �� ����������, ���� ������� ������ ����");
            return null;
        }
        
    }
}