using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }


    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CurrentLevel = 0;
    }
}
