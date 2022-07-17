using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LaunchScene : MonoBehaviour
{
    public void LaunchSceneF(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
