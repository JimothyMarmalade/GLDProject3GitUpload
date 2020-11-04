using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanel : MonoBehaviour
{

    public TMP_Text WinLoseText;


    // Start is called before the first frame update
    void Start()
    {
        WinLoseText.text = "";
        this.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CurrentLevel = 0;
    }

    public void DisplayWinningText()
    {
        WinLoseText.text = "Winner!";
    }

    public void DisplayLosingText()
    {
        WinLoseText.text = "Loser!";
    }
}
