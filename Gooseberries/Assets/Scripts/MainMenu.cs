using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadStage(string stageName)
    {
        Debug.Log("LOAD");
        SceneManager.LoadScene(stageName);
    }
}
