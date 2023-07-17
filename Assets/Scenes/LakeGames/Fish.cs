using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public static bool locked;
    public int id;
    public Sprite ImageFaceLeft, ImageFaceRight;
    public string correctFish;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
