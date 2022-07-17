using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReload : MonoBehaviour
{
    private GameManager_Level level;
    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<GameManager_Level>();
    }

    public void B_ReloadCheckPoint()
    {
        level.ResetToLastCheckpoint();
    }

    public void B_RestartScene()
    {
        SceneManager.LoadScene("Scene_LevelMain");
    }
}
