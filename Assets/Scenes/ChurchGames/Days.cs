using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Days : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public static bool locked;
    public int id;
    public Sprite Image;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
