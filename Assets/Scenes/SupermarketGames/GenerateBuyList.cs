using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateBuyList : MonoBehaviour
{
    public List<string> spawnPoolItems;
    public int noOfItemsToBuy = 5;
    public List<string> buyList;
    public TextMeshProUGUI buyListText;
    // Start is called before the first frame update
    void Start()
    {
        buyListText.text = "";
        for(int i = 0; i < noOfItemsToBuy; i++)
        {
            buyList.Add(GenerateItem());
            buyListText.text += "- " + buyList[i] + "\n";
        }
    }

    public void WriteBuyList()
    {
        for (int i = 0; i < buyList.Count; i++)
        {
            buyListText.text += "- " + buyList[i] + "\n";
        }
    }

    private string GenerateItem()
    {
        int pos = Random.Range(0, spawnPoolItems.Count);
        string line = spawnPoolItems[pos];
        spawnPoolItems.Remove(line);
        return line;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
