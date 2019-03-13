using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences0;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences3;
    public string[] sentences4;
    public string[] currentSentences;
    private int index;
    public float typingSpeed = 0.2f;

    public GameObject continueText;

    public AudioClip[] chirp;
    private AudioSource chirpSource;
    private AudioClip chirpClip;

    //public GameObject 

    private GameObject indicator;
    private GameObject player;

    private PlayerController playerController;

    public bool isTalking = false;
    public bool isSinging = false;

    public Animator anim;

    private void Start()
    {
        currentSentences = sentences0;
        continueText.SetActive(false);
        //StartCoroutine(Type());

        indicator = GameObject.FindWithTag("SongKeeperIndicator");
        indicator.SetActive(false);

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        chirpSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (textDisplay.text == currentSentences[index])
        {
            continueText.SetActive(true);
            isSinging = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextSentence();
            }
        }

        if (indicator.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.E) && textDisplay.text == "" &&
                index == 0)
            {
                isSinging = true;
                StartCoroutine(Type());
                
            }
        }

        if (textDisplay.text != "")
        {
            isTalking = true;
            playerController.moveSpeed = 0;
        } else
        {
            isTalking = false;
            playerController.moveSpeed = 10;
        }

        anim.SetBool("isSinging", isSinging);
    }

    IEnumerator Type()
    {
        PlayRandomSound();

        if (playerController.birdCount == 1)
        {
            currentSentences = sentences1;
        }
        else if (playerController.birdCount == 2)
        {
            currentSentences = sentences2;
        }
        else if (playerController.birdCount == 3)
        {
            currentSentences = sentences3;
        }
        else if (playerController.birdCount == 4)
        {
            currentSentences = sentences4;
        }
        foreach (char letter in currentSentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueText.SetActive(false);
        isSinging = true;

        if (index < currentSentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else
        {
            textDisplay.text = "";
            index = 0;
            isSinging = false;
        }
    }

    public void PlayRandomSound()
    {
        int index = Random.Range(0, chirp.Length);
        chirpClip = chirp[index];
        //chirpSource.clip = chirpClip;
        chirpSource.pitch = (Random.Range(0.8f, 1.2f));
        chirpSource.PlayOneShot(chirpClip, .2f);
    }


}
