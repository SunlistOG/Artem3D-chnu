using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject ScrollbarObj;
    public GameObject ButtonExample;
    public GameObject text;
    private Text volume;
    public AudioListener[] audioListeners;
    public GameObject cameraPrev;
    public GameObject cameraNew;
    private GameObject backButton;
    private Scrollbar scrollbar;
    private Vector2 screenResolution = new Vector2();
    private void Start()
    {
        screenResolution = new Vector2(GetComponent<RectTransform>().offsetMax.x, GetComponent<RectTransform>().offsetMax.y);
        RectTransform scrollbarTransform = ScrollbarObj.GetComponent<RectTransform>();
        scrollbar = ScrollbarObj.GetComponent<Scrollbar>();
        scrollbarTransform.localPosition = new Vector3(0, 0, 0);
        scrollbarTransform.sizeDelta = new Vector2(screenResolution.x / 3.4f, screenResolution.y / 7.7f);

        backButton = Instantiate(ButtonExample, GetComponent<Transform>());
        backButton.GetComponent<Button>().onClick.AddListener(new UnityAction(GoBack));
        backButton.GetComponent<RectTransform>().localPosition = new Vector3(0, 0 - screenResolution.x / 8.5f, 0);
        backButton.GetComponent<RectTransform>().sizeDelta = new Vector2(screenResolution.x / 3.4f, screenResolution.y / 7.7f);
        backButton.GetComponentsInChildren<Text>().FirstOrDefault().text = "Back";

        volume = text.GetComponent<Text>();
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(screenResolution.x / 3.4f, screenResolution.y / 7.7f);
        text.GetComponent<RectTransform>().localPosition = new Vector2(screenResolution.x / 3f, scrollbarTransform.localPosition.y);

        volume.text = ((int)scrollbar.value).ToString() + "%";
        scrollbar.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }
    void ChangeVolume()
    {
        AudioListener.volume = (scrollbar.value); // можливо треба *100
        volume.text = ((int)(scrollbar.value * 100)).ToString() + "%";
        Debug.Log(AudioListener.volume);
    }
    void GoBack()
    {
        cameraNew.SetActive(true);
        cameraPrev.SetActive(false);
    }
}
