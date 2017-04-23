using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController _Instance;

    public SoulController Soul;
    public TeddyController Teddy;
    public GameObject Key;
    public GameObject Hammer;
    public GameObject Window;
    public GameObject ToyChestTop;

    public GameObject ControlsPanel;
    public Text ControlsLabel;
    public GameObject MessagePanel;
    public Text MessageLabel;
    public GameObject KeyIcon;
    public GameObject HammerIcon;

    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
    }


	void Start ()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    
    }


    public static void BeginPossesion(Possessable possessable)
    {
        possessable.BeginPossession(_Instance.Soul.transform.position);
        RenderSettings.fog = false;
        _Instance.Soul.gameObject.SetActive(false);
    }


    public static void EndPossession(Possessable possessable)
    {
        GameController.HideMessage();
        possessable.EndPossession();
        RenderSettings.fog = true;
        _Instance.Soul.transform.position = possessable.transform.position + Vector3.up * 1f;
        _Instance.Soul.gameObject.SetActive(true);
    }


    public static void ShowControls(string text)
    {
        _Instance.ControlsPanel.SetActive(true);
        _Instance.ControlsLabel.text = text;
    }


    public static void ShowMessage(string text)
    {
        _Instance.MessagePanel.SetActive(true);
        _Instance.MessageLabel.text = text;
    }

    public static void HideMessage()
    {
        _Instance.MessagePanel.SetActive(false);
    }


    public static void ResetGame()
    {
        // Just reload scene?
    }


    public static void PickUpKey()
    {
        _Instance.Key.SetActive(false);
        _Instance.KeyIcon.SetActive(true);
        HideMessage();
    }


    public static void PickUpHammer()
    {
        _Instance.Hammer.SetActive(false);
        _Instance.HammerIcon.SetActive(true);
        HideMessage();
    }


    public static bool IsChestOpen()
    {
        return !_Instance.ToyChestTop.activeSelf;
    }


    public static void OpenToyChest()
    {
        _Instance.ToyChestTop.SetActive(false);
        HideMessage();
    }



    public static void SmashWindow()
    {
        HideMessage();
    }

}
