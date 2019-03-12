using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences3;
    private string[] currentSentences;
    private int index;
    public float typingSpeed = 0.2f;

    public GameObject continueText;

    void Start()
    {
        currentSentences = sentences1;
    }

    void Update()
    {
        if (textDisplay.text == currentSentences[index])
        {
            continueText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextSentence();
            }
        }
    }

    IEnumerator Type()
    {
        foreach(char letter in currentSentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueText.SetActive(false);

        if (index < currentSentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else
        {
            textDisplay.text = "";
        }
    }


}
