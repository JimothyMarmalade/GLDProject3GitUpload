using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject PlayPanel;
    public GameObject InstructionsPanel;
    public GameObject CreditsPanel;
    public Button Level2Button;

    // Start is called before the first frame update
    void Start()
    {
        HidePlayPanel();
        HideInstructionsPanel();
        HideCreditsPanel();

        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Level2Available)
        {
            Level2Button.interactable = true;
        }

    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Level2Available)
        {
            Level2Button.interactable = true;
        }
    }

    public void StartLevel(int index)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CurrentLevel = index;
    }

    public void HidePlayPanel()
    {
        PlayPanel.SetActive(false);
    }

    public void ShowPlayPanel()
    {
        PlayPanel.SetActive(true);
    }

    public void HideInstructionsPanel()
    {
        InstructionsPanel.SetActive(false);
    }

    public void ShowInstructionsPanel()
    {
        InstructionsPanel.SetActive(true);
    }

    public void HideCreditsPanel()
    {
        CreditsPanel.SetActive(false);
    }

    public void ShowCreditsPanel()
    {
        CreditsPanel.SetActive(true);
    }
}
