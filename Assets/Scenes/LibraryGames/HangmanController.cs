using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HangmanController : MonoBehaviour
{
    [SerializeField] GameObject wordContainer;
    [SerializeField] GameObject keyboardContainer;
    [SerializeField] GameObject letterContainer;
    [SerializeField] GameObject[] hangmanStages;
    [SerializeField] GameObject letterButton;
    [SerializeField] TextAsset possibleWords;
    [SerializeField] GameObject domain;
    [SerializeField] GameObject gameOver;
    private GameObject player;

    private string word;
    private int incorrectGuesses, correctGuesses;
    private static int ASCII_CODE_FOR_A = 65, ASCII_CODE_FOR_Z = 90, LAST_POS_AUTHORS = 22, LAST_POS_ANIMALS = 100, NUMBER_OF_POINTS = 5;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.SetActive(false);
        }
        InitialiseButtons();
        InitialiseGame();
    }

    private void InitialiseButtons()
    {
        for(int i = ASCII_CODE_FOR_A; i <= ASCII_CODE_FOR_Z; i++)
        {
            CreateButton(i);
        }
    }

    public void InitialiseGame()
    {
        incorrectGuesses = 0;
        correctGuesses = 0;
        gameOver.SetActive(false);
        foreach (Button child in keyboardContainer.GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }
        foreach(Transform child in wordContainer.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        foreach(GameObject stage in hangmanStages)
        {
            stage.SetActive(false);
        }
        domain.GetComponentInChildren<TextMeshProUGUI>().text = "";

        word = GenerateWord().ToUpper();
        foreach(char letter in word)
        {
            var temp = Instantiate(letterContainer, wordContainer.transform);
        }
    }

    private void CreateButton(int i)
    {
        GameObject temp = Instantiate(letterButton, keyboardContainer.transform);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = ((char)i).ToString();
        temp.GetComponent<Button>().onClick.AddListener(delegate { CheckLetter(((char)i).ToString()); });
    }

    private string GenerateWord()
    {
        string[] wordList = possibleWords.text.Split("\n");
        int pos = Random.Range(0, wordList.Length - 1);
        string line = wordList[pos];
        if (pos < LAST_POS_AUTHORS)
        {
            domain.GetComponentInChildren<TextMeshProUGUI>().text = "Autori";
        }
        else
        {
            if (pos < LAST_POS_ANIMALS)
            {
                domain.GetComponentInChildren<TextMeshProUGUI>().text = "Animale";
            }
        }
        return line.Substring(0, line.Length - 1);
    }

    private void CheckLetter(string inputLetter)
    {
        bool letterInWord = false;
        for(int i = 0; i < word.Length; i++)
        {
            if (inputLetter == word[i].ToString())
            {
                letterInWord = true;
                correctGuesses++;
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].text = inputLetter;
            }
        }
        if (!letterInWord)
        {
            incorrectGuesses++;
            hangmanStages[incorrectGuesses - 1].SetActive(true);
        }
        CheckOutcome();
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Library");
        player.SetActive(true);
    }

    private void CheckOutcome()
    {
        if (correctGuesses == word.Length)
        {
            for(int i = 0; i < word.Length; i++)
            {
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.green;
            }
            float score = PlayerPrefs.GetFloat("score");
            Debug.Log(score);
            score = score + NUMBER_OF_POINTS;
            Debug.Log(score);
            PlayerPrefs.SetFloat("score", score);
            PlayerPrefs.Save();
            Invoke("GoBack", 3f);
        }
        if (incorrectGuesses == hangmanStages.Length)
        {
            for (int i = 0; i < word.Length; i++)
            {
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.red;
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].text = word[i].ToString();
            }
            gameOver.SetActive(true);
        }
    }

}
