using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite background;
    public int id;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
