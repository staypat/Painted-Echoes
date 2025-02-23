using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI victoryText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowVictoryMessage()
    {
        StartCoroutine(ShowVictoryText());
    }

    IEnumerator ShowVictoryText()
    {
        victoryText.enabled = true;
        yield return new WaitForSeconds(3f);
        victoryText.enabled = false;
    }
}
