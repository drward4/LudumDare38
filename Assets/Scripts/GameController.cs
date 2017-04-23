using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public GameObject GameOverPanel;
    public GameObject MainMenu;
    public GameObject PausePanel;

    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
    }


	void Start ()
    {
        Time.timeScale = 0f;
        this.MainMenu.SetActive(true);
    }


    public void BeginGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;
        this.MainMenu.SetActive(false);
    }


    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        this.PausePanel.SetActive(true);
    }


    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;
        this.PausePanel.SetActive(false);
    }




    public void EndGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        this.GameOverPanel.SetActive(true);
    }


    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }


    public void RestartLevel()
    {
        Debug.Log("restart");
        SceneManager.LoadScene(0);
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
        _Instance.EndGame();
        HideMessage();
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !this.GameOverPanel.activeSelf && !this.MainMenu.activeSelf)
        {
            if (this.PausePanel.activeSelf)
            {
                Debug.Log("resumio");
                this.ResumeGame();
            }
            else
            {
                this.PauseGame();
            }
        }
    }
}
