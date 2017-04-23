using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SoulController Soul;
    public GameObject MessagePanel;
    public Text MessageText;

    private static GameController _Instance;

	void Start ()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (_Instance == null)
        {
            _Instance = this;
        }         
    }


    public static void BeginPossesion(Possessable possessable)
    {
        possessable.BeginPossession(_Instance.Soul.transform.position);
        RenderSettings.fog = false;
        GameController.ShowMessage("Press Escape to Leave");
        _Instance.Soul.gameObject.SetActive(false);
    }


    public static void EndPossession(Possessable possessable)
    {
        possessable.EndPossession();
        RenderSettings.fog = true;
        _Instance.Soul.transform.position = possessable.transform.position + Vector3.up * 1f;
        _Instance.Soul.gameObject.SetActive(true);
        GameController.HideMessage();
    }


    public static void ShowMessage(string text)
    {
        _Instance.MessagePanel.SetActive(true);
        _Instance.MessageText.text = text;
    }


    public static void HideMessage()
    {
        _Instance.MessagePanel.SetActive(false);
    }
}
