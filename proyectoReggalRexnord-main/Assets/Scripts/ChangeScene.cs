using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Change(string sceneName)
    {
        //cambia la escena y regresa el tiempo a normal
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}
