using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadPixels : MonoBehaviour
{
    //public Material apple;
    public Camera cam; // наша камера
    Texture2D tex;
    Color col;
    float vis;

    void Start()
    {
        tex = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);
    }

    void Update()
    {
        // делаем RenderTexture камеры cam активным
        // при этом RenderTexture записывается в буфер
        RenderTexture.active = cam.targetTexture;
        //Из буфера читаем RenderTexture.active в Texture2D
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        // обновляем изменения 
        tex.Apply();
        //Достаем цвет центрального пикселя из Texture2D
        col = tex.GetPixel(tex.width / 2, tex.height / 2);
        // Определяем цветовой признак пикселя
        //vis = (col.r + col.g + col.b) / 3;
        //vis = col.r;
        vis = col.r / col.g;
        //Debug.Log(apple.color);
        if (vis > 10)
        {
            Debug.Log("SOS!!!"); // сообщение в консоль Unity
                                 //UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
        //Debug.Log("vis" + vis); 
        //Debug.Log("tex_height" + tex.height/2); 
        //Debug.Log("Render_height" + RenderTexture.active.height/2);
    }
}
