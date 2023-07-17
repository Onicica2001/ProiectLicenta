using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public static bool locked;
    public int id;
    public Sprite Image;
    public string correctBin;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
