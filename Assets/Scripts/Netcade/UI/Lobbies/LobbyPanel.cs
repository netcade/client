using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public void FillData(string gameName, string owner)
    {
        this.GetComponentInChildren<TMP_Text>().text = gameName + "\n<size=10>by <b>" + owner + "</b></size>";
    }
}
