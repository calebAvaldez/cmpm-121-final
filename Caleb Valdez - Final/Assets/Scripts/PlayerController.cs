using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public Text countText;
    public Text birdCountText;

    public CharacterController controller;

    public GameObject piece1;
    public GameObject piece2;
    public GameObject piece3;
    public GameObject piece4;

    //public GameObject indicator;

    private Vector3 moveDirection;
    public float gravityScale;
    private int count;
    public int birdCount;
    //private bool menuOpen;

    public AudioClip pickup;
    AudioSource audioSource;

    private AudioSource soundscape1;
    private AudioSource soundscape2;
    private AudioSource soundscape3;
    private AudioSource soundscape4;

    private GameObject player;
    private GameObject soundscape;

    private Dialog dialog;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        moveSpeed = 7;
        count = 0;
        birdCount = 0;
        SetCountText();
        SetBirdCountText();

        piece1.SetActive(false);
        piece2.SetActive(false);
        piece3.SetActive(false);
        piece4.SetActive(false);

        //indicator.SetActive(false);

        player = GameObject.Find("Player");

        dialog = player.GetComponent<Dialog>();

        soundscape = GameObject.Find("Sounds");

        soundscape1 = soundscape.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        soundscape2 = soundscape.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        soundscape3 = soundscape.transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        soundscape4 = soundscape.transform.GetChild(3).gameObject.GetComponent<AudioSource>();

    }

    private void FixedUpdate()
    {
        //if (menuOpen == false)
       // {
        if (controller.isGrounded)
        {
            moveDirection.y = 0.0f;

            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) 
                + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;
        }


        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);
        controller.Move(moveDirection * Time.deltaTime);
       // }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            if (other.gameObject.transform.parent.tag == "Collectable" && other.gameObject.transform.parent.gameObject.activeInHierarchy == true)
            {
                Debug.Log(other.gameObject.transform.parent.tag);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    other.gameObject.transform.parent.gameObject.SetActive(false);
                    count++;
                    SetCountText();
                }
            }

            if (other.gameObject.transform.parent.tag == "Songkeeper")
            {
                if (Input.GetKeyDown(KeyCode.E) && count > 0 && dialog.isTalking == false)
                {
                    count--;
                    birdCount++;
                    SetCountText();
                    SetBirdCountText();

                    if (birdCount == 1)
                    {
                        piece1.SetActive(true);
                        soundscape1.Play();
                        Debug.Log("Playing Leaves.");
                    }
                    else if (birdCount == 2)
                    {
                        piece2.SetActive(true);
                        soundscape2.Play();
                        Debug.Log("Playing Wind.");
                    }
                    else if (birdCount == 3)
                    {
                        piece3.SetActive(true);
                        soundscape3.Play();
                        Debug.Log("Playing Birdsong.");
                    }
                    else if (birdCount == 4)
                    {
                        piece4.SetActive(true);
                        soundscape4.Play();
                        Debug.Log("Playing Music.");
                    }
                }
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
       /* if (other.gameObject.CompareTag("Collectable"))
        {

            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            //audioSource.PlayOneShot(pickup, 0.7f);

        } */

        if (other.gameObject.CompareTag("Interactable"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        }

        /*if (other.gameObject.CompareTag("Songkeeper") && count > 0)
        {
            count--;
            birdCount++;
            SetCountText();
            SetBirdCountText();

            if (birdCount == 1)
            {
                piece1.SetActive(true);
            } else if (birdCount == 2)
            {
                piece2.SetActive(true);
            } else if (birdCount ==3 )
            {
                piece3.SetActive(true);
            } else if (birdCount == 4)
            {
                piece4.SetActive(true);
            }

        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            //indicator.SetActive(false);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void SetCountText()
    {
        countText.text = "You: " + count.ToString();
    }

    void SetBirdCountText()
    {
        birdCountText.text = "Songkeeper: " + birdCount.ToString();
    }
}
