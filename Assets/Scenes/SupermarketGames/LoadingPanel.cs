using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public GameObject loadingPanel;
    public float timer = 5f;
    public TextMeshProUGUI timerSeconds;
    public static GameObject player;
    void Start()
    {
        loadingPanel.SetActive(true);
        timerSeconds = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerSeconds.text = timer.ToString("f0");
        if (timer <= 0)
        {
            loadingPanel.SetActive(false);
            SceneManager.LoadScene("ManiaCumparaturilor");
        }
    }

}
