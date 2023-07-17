using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    const string STRIKE_START = "<s>";
    const string STRIKE_END = "</s>";
    public List<ObjectItem> boughtList;
    public bool isWithPlayer;

    public void WriteBoughtItems(GenerateBuyList buyList)
    {
        for (int i = 0; i < boughtList.Count; i++)
        {
            buyList.buyListText.text += "- " + STRIKE_START + boughtList[i].itemName + STRIKE_END + " \n";
        }
    }

    public void StealItem(string itemName, GenerateBuyList buyList)
    {
        ObjectItem item = null;
        foreach(ObjectItem objectItem in boughtList)
        {
            if (objectItem.itemName == itemName)
            {
                item = objectItem;
            }
        }
        if (item != null)
        {
            boughtList.Remove(item);
            buyList.buyList.Add(itemName);
            buyList.buyListText.text = "";
            buyList.WriteBuyList();
            WriteBoughtItems(buyList);
        }
    }

    public void AddItem(GenerateBuyList buyList, ObjectItem item)
    {
        boughtList.Add(item);
        if (isWithPlayer)
        {
            PlayerIsBack();
        }
        buyList.buyList.Remove(item.itemName);
        buyList.buyListText.text = "";
        buyList.WriteBuyList();
        WriteBoughtItems(buyList);
    }

    public void PlayerLeft()
    {
        isWithPlayer = false;
        foreach(ObjectItem objectItem in boughtList)
        {
            objectItem.gameObject.SetActive(true);
        }
    }

    public void PlayerIsBack()
    {
        isWithPlayer = true;
        foreach (ObjectItem objectItem in boughtList)
        {
            objectItem.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!PauseMenu.isPaused)
        {
            foreach (ObjectItem objectItem in boughtList)
            {
                objectItem.gameObject.transform.position = transform.position;
            }
        }
    }
}
