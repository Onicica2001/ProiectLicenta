using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishType : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public static bool locked;
    public int id;
    public Sprite Image;
    public string fishName;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        this.GetComponentInChildren<Text>().text = fishName;
    }
}
