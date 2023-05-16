using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxAnim : MonoBehaviour
{
    public GameObject Box; //el objeto a animar

    void Start()
    {
        Time.timeScale = 1f;//me aseguro de regresar el tiempo a 1 para que corra el tiempo y no se quede en pausa

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            ActivarAnimacion();//se activa la animacion si entro en escena
        }
    }

    public void ActivarAnimacion()
    {
        LeanTween.moveX(Box, 2070, 4f).setLoopClamp();//una animacion en ciclo 
    }
}
