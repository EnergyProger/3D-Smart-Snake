using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Camera camleft; // левая камера
    public Camera camright; // левая камера
    Texture2D tex;
    Color col;
    float vis1_L, vis2_L, vis3_L, vis1_R, vis2_R, vis3_R;
    // инициализация количество обновлений (Update),
    // когда направление движения не к яблоку
    int repeat = 100;
    public Transform target;
    // скорость перемещения - 6 единиц в секунду по умолчанию
    float speed = 6;
    // скорость вращения 3 градуса в секунду по умолчанию
    float rotationSpeed = 3;
    // коэффициент, определяющий направление поворота (krt=1 или krt=-1)
    float krt;
    // локальная переменная для хранения ссылки на компонент CharacterController
    private CharacterController _controller;

    public void Start()
    {
        tex = new Texture2D(camleft.targetTexture.width, camleft.targetTexture.height, TextureFormat.RGB24, false);

        // получаем компонент CharacterController и 
        // записываем его в локальную переменную
        _controller = GetComponent<CharacterController>();
        // создаем хвост
        // current - текущая цель элемента хвоста, начинаем с головы
        Transform current = transform;
        for (int i = 0; i < 3; i++)
        {
            // создаем примитив куб и добавляем ему компонент Tail
            Tail tail = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Tail>();
            // помещаем "хвост" за "хозяином"
            tail.transform.position = current.transform.position - current.transform.forward * 2;
            // ориентация хвоста как ориентация хозяина
            tail.transform.rotation = transform.rotation;
            // элемент хвоста должен следовать за хозяином, поэтому передаем ему ссылку на его
            tail.target = current.transform;
            // дистанция между элементами хвоста - 2 единицы
            tail.targetDistance = 2;
            // удаляем с хвоста коллайдер, так как он не нужен
            //Destroy(tail.collider);
            Destroy(tail.GetComponent<Collider>());
            // следующим хозяином будет новосозданный элемент хвоста
            current = tail.transform;
        }
    }

    public void Update()
    {
        // делаем RenderTexture левой камеры активным
        // при этом RenderTexture записывается в буфер
        RenderTexture.active = camleft.targetTexture;
        //Из буфера читаем RenderTexture.active в Texture2D
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        // обновляем изменения 
        tex.Apply();
        //Достаем цвета пикселей из Texture2D
        col = tex.GetPixel(tex.width / 2, 8 * tex.height / 10);
        // Определяем цветовые признаки пикселей
        vis1_L = col.r / col.g;
        col = tex.GetPixel(9 * tex.width / 10, 8 * tex.height / 10);
        vis2_L = col.r / col.g;
        col = tex.GetPixel(2 * tex.width / 10, 8 * tex.height / 10);
        vis3_L = col.r / col.g;

        // делаем RenderTexture правой камеры активным
        RenderTexture.active = camright.targetTexture;
        //Из буфера читаем RenderTexture.active в Texture2D
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        // обновляем изменения 
        tex.Apply();
        //Достаем цвета пикселей из Texture2D
        col = tex.GetPixel(tex.width / 2, 8 * tex.height / 10);
        // Определяем цветовые признаки пикселей
        vis1_R = col.r / col.g;
        col = tex.GetPixel(9 * tex.width / 10, 8 * tex.height / 10);
        vis2_R = col.r / col.g;
        col = tex.GetPixel(2 * tex.width / 10, 8 * tex.height / 10);
        vis3_R = col.r / col.g;

        krt = 0; // коэффициент определяет направление движения (движение прямо)
        if (vis1_L > 10 || vis1_R > 10)
        { repeat = 0; krt = 1; } // поворот влево - в случае преграды справа и по центру
        if (vis3_L > 10 || vis3_R > 10) krt = -1; // поворот вправо

        // для 5 обновлений (Update) отключается движение к яблоку
        if (krt == 0 && repeat > 5) transform.LookAt(target);
        else
        {
            Debug.Log("SOS!!!"); // сообщение в консоль Unity
            transform.Rotate(0, rotationSpeed * Time.deltaTime * krt, 0);
            transform.LookAt(transform.forward);
        }
        _controller.Move(transform.forward * speed * Time.deltaTime /*vertical*/);
    }

    GameObject food;
    // В эту функцию будут передаваться все объекты, с которыми
    // CharacterController вступает в столкновения
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.name == "Food")
        {
            // прибавляем очки еды к общему числу очков
            Game.points += 10;

            //Врезались в еду, "съедаем" ее и создаем новую в пределах поля. 
            //На самом деле перемещаем еду в Random положение
            food = GameObject.Find("Food");
            //Destroy(food);
            var pos = new Vector3(Random.Range(-40, 41), 0, Random.Range(-40, 41));
            food.transform.position = pos;
        }
        else
        {
            //врезались не в еду
            Application.LoadLevel("GameOver");
        }
    }
}
