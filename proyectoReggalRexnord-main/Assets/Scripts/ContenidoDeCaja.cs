using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContenidoDeCaja : MonoBehaviour
{
    public Sprite[] contentSprites;
  
    //contenidos de las cajas
    public enum ContentType
    {
        Screws,
        SynchronousDrives,
        GearBox,
        Controller,
        AC,
        Bearings,
        Motor,
        Belts,
    }
    //imagenes de las cajas
    public void SetSprite()
    {
        if (contentSprites.Length == System.Enum.GetValues(typeof(ContentType)).Length)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = contentSprites[(int)contentType];
            }
            else
            {
                Debug.LogError("No se encontró el componente SpriteRenderer en el objeto caja.");
            }
        }
        else
        {
            Debug.LogError("La cantidad de sprites en el array contentSprites no coincide con la cantidad de tipos de contenido.");
        }
    }

    public ContentType contentType;
}

