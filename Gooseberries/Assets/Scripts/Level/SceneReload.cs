using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReload : MonoBehaviour
{
    private GameManager_Level level;
    [SerializeField] private GameObject tutorial;
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
        DestroyImmediate(FindObjectOfType<ZoneCounter>().gameObject);
        SceneManager.LoadScene("Title Screen");
    }

    public void B_Help()
    {
        if(tutorial.activeSelf == false)
        {
            tutorial.SetActive(true);
        }
        else
        {
            tutorial.SetActive(false);
        }
    }
}
