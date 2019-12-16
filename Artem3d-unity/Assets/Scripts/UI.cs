using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // вигляд кнопки
    public GameObject ButtonExample;
    // вкладка опції
    public GameObject Options;
    // всі елементи, які потрібно сховати, коли ми в меню
    public GameObject[] nonUI;
    // посилання на MenuCamera
    public GameObject cameraPrev;
    // посилання на PlayerCamera
    public GameObject cameraNew;
    // розширення екрану
    private Vector2 screenResolution = new Vector2();
    // колекція з позиціями кнопок меню
    List<float> buttonPos = new List<float>();
    // колекція з функціями кнопок меню
    List<UnityAction> buttonFunctions = new List<UnityAction>();
    // колекція з текстом кнопок меню
    List<string> buttonText = new List<string>();
    private void Start()
    {
        // обчислюємо розмір екрану
        screenResolution = new Vector2(GetComponent<RectTransform>().offsetMax.x, GetComponent<RectTransform>().offsetMax.y); 
        Debug.Log(screenResolution);
        // виводимо меню( параметр false означає, що конпка Continue генеруватись не буде ) 
        Render(false);
    }

    #region SomeFunctions
    // переводить змінну в нову систему координит Canvas
    private float ScreenToCanvasPoint(float y)
    {
        return y - screenResolution.y / 2;
    }
    // відповідні функції кнопок меню
    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ShowAllObjects();
        cameraNew.SetActive(true);
        cameraPrev.SetActive(false);
    }
    public void NewGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Render(true);
        ShowAllObjects();
        nonUI[0].transform.position = new Vector3(0, 0, 0);
        nonUI[0].transform.rotation = new Quaternion();
        cameraNew.SetActive(true);
        cameraPrev.SetActive(false);
    }
    public void OpenOptions()
    {
        Options.SetActive(true);
        cameraPrev.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    // ховає всі вибрані ігрові об'єкти
    public void HideAllObjects()
    {
        foreach (var obj in nonUI)
        {
            obj.SetActive(false);
        }
    }
    // виводить всі ігрові об'єкти
    public void ShowAllObjects()
    {
        foreach (var obj in nonUI)
        {
            obj.SetActive(true);
        }
    }
    // генерує колекцію функцій кнопок
    private void ActoinsToList(bool AddContinue = false)
    {
        buttonFunctions = new List<UnityAction>();
        if (AddContinue)
        { buttonFunctions.Add(new UnityAction(Continue)); }
        buttonFunctions.Add(new UnityAction(NewGame));
        buttonFunctions.Add(new UnityAction(OpenOptions));
        buttonFunctions.Add(new UnityAction(Quit));
    }
    // генерує колекцію тексту кнопок
    private void TextToList(bool AddContinue = false)
    {
        buttonText = new List<string>();
        if (AddContinue)
        { buttonText.Add("Continue"); }
        buttonText.Add("New Game");
        buttonText.Add("Options");
        buttonText.Add("Quit");
    }
    // виводить на екран всі кнопки з колекції
    private void InstantiateButtons()
    {
        var buttons = GameObject.FindGameObjectsWithTag("MenuButton");
        foreach(var b in buttons)
        {
            Destroy(b);
        }
        int j = 0;
        foreach (var pos in buttonPos)
        {
            GameObject button = Instantiate(ButtonExample, GetComponent<Transform>());
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            Button b = button.GetComponent<Button>();
            Text t = button.GetComponentsInChildren<Text>().FirstOrDefault();
            b.onClick.AddListener(buttonFunctions[j]);
            t.text = buttonText[j];
            buttonTransform.localPosition = new Vector3(0, pos, 0);
            buttonTransform.sizeDelta = new Vector2(screenResolution.x / 3.4f, screenResolution.y / 7.7f);
            j++;
        }
    }
    // визначає чи потрібна кнопка Continue та викликає необхідні функції для рендуру кнопок
    private void Render(bool AddContinue = false)
    {
        ActoinsToList(AddContinue);
        TextToList(AddContinue);
        HideAllObjects();
        buttonPos = new List<float>();
        int ButtonsCount = AddContinue ? 4 : 3;
        for (int i = ButtonsCount; i > 0; i--)
        {
            buttonPos.Add(ScreenToCanvasPoint(screenResolution.y * i / (ButtonsCount + 1)));
        }
        InstantiateButtons();
    }
    #endregion
}
