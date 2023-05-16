using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recibidor : MonoBehaviour
{
    public ContenidoDeCaja.ContentType expectedContentType;
    public int scorePerBox = 10;
    public int riskPercentagePerMismatch = 5;
    public BoxSpawner boxSpawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ContenidoDeCaja ContenidoDeCaja = collision.GetComponent<ContenidoDeCaja>();

        if (ContenidoDeCaja != null)
        {
            if (ContenidoDeCaja.contentType == expectedContentType)
            {
                boxSpawner.UpdateScore(scorePerBox); //agrega puntuacion
                Debug.Log($"Caja correcta recibida. Puntos actuales: {boxSpawner.GetCurrentScore()}");
            }
            else
            {
                boxSpawner.UpdateRiskPercentage(riskPercentagePerMismatch);//agrega riesgo
                Debug.Log($"Caja incorrecta recibida. Porcentaje de riesgo actual: {boxSpawner.GetCurrentRiskPercentage()}%");
            }

            // Destruye la caja después de interactuar con ella
            Destroy(collision.gameObject);
        }
    }
}
