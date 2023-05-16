using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cintaDireccional : MonoBehaviour
{
    [System.Serializable]
    public struct DirectionSprite
    {
        //muestra las posibles direcciones de la interseccion y sus sprites
        public Vector2 direction;
        public Sprite sprite;
    }

    public DirectionSprite[] directionSprites;
    public float speed = 1.0f;
    public string boxTag = "Caja";

    private Vector2 currentDirection;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Establecer la dirección inicial y el sprite
        if (directionSprites.Length > 0)
        {
            currentDirection = directionSprites[0].direction;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = directionSprites[0].sprite;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(boxTag))
        {
            other.transform.position += (Vector3)currentDirection * speed * Time.deltaTime;
            CenterBoxOnConveyorBelt(other.transform);
        }
    }

    private void OnMouseDown()
    {
        // Cambiar la dirección de la cinta transportadora y el sprite
        int currentIndex = System.Array.FindIndex(directionSprites, ds => ds.direction == currentDirection);
        int nextIndex = (currentIndex + 1) % directionSprites.Length;
        currentDirection = directionSprites[nextIndex].direction;
        spriteRenderer.sprite = directionSprites[nextIndex].sprite;
    }

    private void CenterBoxOnConveyorBelt(Transform boxTransform)
    {
        // Ajustar la posición en el eje X o Y para mantener la caja centrada en la cinta transportadora
        Vector2 conveyorCenter = new Vector2(transform.position.x, transform.position.y);
        Vector2 boxCenter = new Vector2(boxTransform.position.x, boxTransform.position.y);

        // Ajustar la posición en el eje X si la dirección de la cinta transportadora es en el eje Y
        if (Mathf.Abs(currentDirection.y) > 0)
        {
            boxCenter.x = conveyorCenter.x;
        }
        // Ajustar la posición en el eje Y si la dirección de la cinta transportadora es en el eje X
        else if (Mathf.Abs(currentDirection.x) > 0)
        {
            boxCenter.y = conveyorCenter.y;
        }

        boxTransform.position = boxCenter;
    }
}
