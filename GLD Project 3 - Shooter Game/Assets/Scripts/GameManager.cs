using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            //Check if instance already exists
            if (_instance == null)
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));

            if (_instance == null)
                Debug.LogError("Game Manager is NULL");

            return _instance;
        }
    }


    public int CurrentLevel;
    public bool Level2Available;



    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Destroying Duplicate GameManager Singleton");
        }
        else
        {
            _instance = this;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    protected void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }


}
