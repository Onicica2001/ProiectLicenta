using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI score;
    private float scoreCount;
    // Start is called before the first frame update
    void Start()
    {
        scoreCount = PlayerPrefs.GetFloat("score", 0);
        if (scoreCount == 0)
        {
            PlayerPrefs.Save();
        }
        score.text = scoreCount.ToString();
        //Pentru a reseta scorul comenteaza tot si ruleaza linia:
        //PlayerPrefs.DeleteKey("score");
    }

    // Update is called once per frame
    void Update()
    {
        scoreCount = PlayerPrefs.GetFloat("score", 0);
        if (score.text != scoreCount.ToString())
        {
            score.text = scoreCount.ToString();
        }
    }
}
