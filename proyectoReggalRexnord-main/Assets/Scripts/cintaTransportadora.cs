using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cintaTransportadora : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector2 direction = Vector2.right;
    public string boxTag = "Caja";

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(boxTag))
        {
            other.transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Ajustar la posición en el eje X o Y para mantener la caja centrada en la cinta transportadora
            Vector2 conveyorCenter = new Vector2(transform.position.x, transform.position.y);
            Vector2 boxCenter = new Vector2(other.transform.position.x, other.transform.position.y);

            // Ajustar la posición en el eje X si la dirección de la cinta transportadora es en el eje Y
            if (Mathf.Abs(direction.y) > 0)
            {
                boxCenter.x = conveyorCenter.x;
            }
            // Ajustar la posición en el eje Y si la dirección de la cinta transportadora es en el eje X
            else if (Mathf.Abs(direction.x) > 0)
            {
                boxCenter.y = conveyorCenter.y;
            }

            other.transform.position = boxCenter;
        }
    }
}
