using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timer = 0;
    private float endTime = 3 * 60f;
    public TextMeshProUGUI timerSeconds;
    public GameObject endMenu;
    public PlayerScript player;
    private static int NUMBER_OF_POINTS = 25;
    // Start is called before the first frame update
    void Start()
    {
        endMenu.SetActive(false);
        player = FindObjectOfType<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        string time = minutes + ":" + seconds;
        timerSeconds.text = time;
        if (timer >=endTime)
        {
            if (player.buyList.buyList.Count == 0)
            {
                float score = PlayerPrefs.GetFloat("score");
                Debug.Log(score);
                score = score + NUMBER_OF_POINTS - 5;
                Debug.Log(score);
                PlayerPrefs.SetFloat("score", score);
                PlayerPrefs.Save();
                endMenu.GetComponentInChildren<TextMeshProUGUI>().text = "You Won!";
            }
            else
            {
                endMenu.GetComponentInChildren<TextMeshProUGUI>().text = "Mai incearca :(";
            }
            endMenu.SetActive(true);
            Time.timeScale = 0f;
            StartCoroutine(BackToSupermarket());
        }
    }
    private IEnumerator BackToSupermarket()
    {
        yield return new WaitForSecondsRealtime(5);
        LoadingPanel.player.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Supermarket");
    }
}
