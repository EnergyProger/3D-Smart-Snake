using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Размер меню
    public Vector2 menuSize = new Vector2(500, 300);
    // минимальная высота кнопки
    public float buttonMinHeight = 60f;
    // шрифт заголовка
    public Font captionFont;
    // шрифт кнопок
    public Font buttonFont;
    // тексты меню
    public string mainMenuText = "Main menu";
    public string startButtonText = "Start game";
    public string exitButtonText = "Exit";
    public void OnGUI()
    {
        // рассчитываем прямоугольник по центру экрана
        Rect rect = new Rect(
            Screen.width / 2f - menuSize.x / 2,
            Screen.height / 2f - menuSize.y / 2,
            menuSize.x,
            menuSize.y);
        // область меню
        GUILayout.BeginArea(rect, GUI.skin.textArea);
        {
            // создаем стиль заголовка
            GUIStyle captionStyle = new GUIStyle(GUI.skin.label);
            // устанавливаем стиль заголовка шрифт captionFont
            captionStyle.font = captionFont;
            // Рассположение текста по центру
            captionStyle.alignment = TextAnchor.MiddleCenter;
            captionStyle.fontSize = 70;
            // текст заголовка
            GUILayout.Label(mainMenuText, captionStyle);
            // создаем стиль кнопки
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            // устанавливаем стилю кнопки шрифт buttonFont
            buttonStyle.font = buttonFont;
            // отступы кнопок от краев
            buttonStyle.margin = new RectOffset(20, 20, 3, 3);
            buttonStyle.fontSize = 40;
            // FlexibleSpace - автоматически рассчитанное место для
            // заполнения пустого пространства между элементами
            GUILayout.FlexibleSpace();
            // отрисовка кнопки Start и обработка ее нажатия
            if (GUILayout.Button(startButtonText, buttonStyle, GUILayout.MinHeight(buttonMinHeight)))
            {
                // загрузка сцены с именем Level
                Application.LoadLevel("Level");
            }
            GUILayout.FlexibleSpace();
            // отрисовка кнопки Exit и обработка ее нажатия
            if (GUILayout.Button(exitButtonText, buttonStyle, GUILayout.MinHeight(buttonMinHeight)))
            {
                // выход
                Application.Quit();
            }
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndArea();
    }
}