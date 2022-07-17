using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.Play("Title Screen BGM");
    }

    public void LoadStage(string stageName)
    {
        SceneManager.LoadScene(stageName);
    }
}
