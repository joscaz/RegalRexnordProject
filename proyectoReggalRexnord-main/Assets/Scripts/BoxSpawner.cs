using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab; //el prefab con las cajas y sus veriantes
    public float spawnInterval = 1.0f; //cada cuanto se spawnean las cajas
    public float maxScore = 100f; //el score necesario para que se acabe el juego
    public Image scoreBar; //Imagen del score que vaa aumentando
    public float maxRiskPercentage = 100; //el error necesario para que se acabe el juego
    public Image errorBar; //Imagen del error que vaa aumentando
    public GameObject Victory;//pantalla de victoria
    public GameObject Defeat;//pantalla de derrota



    private float currentScore = 0f; //el score actual
    private float currentRiskPercentage = 0; //el error actual
    private float timer = 0; //inicializador del timer
    private bool scoreSent = false;
    

    private void Update()
    {
        timer += Time.deltaTime;

        //spawnea enemigos cada "spawnInterval" segundos
        if (timer >= spawnInterval && (currentScore < maxScore && currentRiskPercentage < maxRiskPercentage))
        {
            SpawnBox();
            timer = 0;
        }
        else
        {
            if (currentScore >= maxScore && !scoreSent)
            {
                //si llega al maximo score muestra la pantalla de victoria y se para el tiempo
                Victory.SetActive(true);
                Time.timeScale = 0f;
                // Send score to server if the maximum score is reached
                StartCoroutine(SendScoreToServer(currentScore));
                scoreSent = true;
                return;
            }
            else if (currentRiskPercentage >= maxRiskPercentage && !scoreSent)
            {
                //si llega al maximo riesgo muestra la pantalla de derrota y se para el tiempo
                Defeat.SetActive(true);
                Time.timeScale = 0f;
                // Send score to server if the maximum score is reached
                StartCoroutine(SendScoreToServer(currentScore));
                scoreSent = true;
                return;
            }
        }
    }

    private IEnumerator SendScoreToServer(float score)
    {
        string token = PlayerPrefs.GetString("Token");

        ScoreData data = new ScoreData(score);
        string json = JsonUtility.ToJson(data);

        // Convert the JSON string to a byte[] array
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Post("http://localhost:8000/api/scores/register_score/", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Token " + token);

        // Send the request
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error sending score to server: " + request.error);
        }
        else
        {
            Debug.Log("Score sent to server");
        }
    }

    private void SpawnBox()
    {
        GameObject newBox = Instantiate(boxPrefab, transform.position, Quaternion.identity);
        ContenidoDeCaja ContenidoDeCaja = newBox.GetComponent<ContenidoDeCaja>();
        ContenidoDeCaja.contentType = (ContenidoDeCaja.ContentType)Random.Range(0, System.Enum.GetValues(typeof(ContenidoDeCaja.ContentType)).Length);

        // Asignar el sprite correspondiente al contenido de la caja
        ContenidoDeCaja.SetSprite();
    }


    public void UpdateScore(int score)
    {
        currentScore += score; //aumenta la variable del score
        scoreBar.fillAmount = currentScore / 100; //modifica la imagen de la barrita progresiva
    }

    public void UpdateRiskPercentage(int riskPercentage)
    {
        currentRiskPercentage += riskPercentage;//aumenta la variable del riesgo
        errorBar.fillAmount = currentRiskPercentage / 100;//modifica la imagen de la advertencia
    }

    public float GetCurrentScore()
    {
        return currentScore;
    }

    public float GetCurrentRiskPercentage()
    {
        return currentRiskPercentage;
    }
}

[System.Serializable]
class ScoreData
{
    public float score;

    public ScoreData(float score)
    {
        this.score = score;
    }
}