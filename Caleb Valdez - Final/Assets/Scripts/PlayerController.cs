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

    public GameObject recordText;

    //public GameObject indicator;

    private Vector3 moveDirection;
    public float gravityScale;
    public int count;
    public int birdCount;
    //private bool menuOpen;

    public AudioClip pickup;
    AudioSource audioSource;

    public AudioSource soundscape1;
    public AudioSource soundscape2;
    public AudioSource soundscape3;
    public AudioSource soundscape4;

    private GameObject player;
    private GameObject soundscape;

    private Dialog dialog;

    private MicInput mic;
    private float loudness;

    private Color stoneColor1;
    private Color stoneColor2;
    private Color stoneColor3;

    private bool ring1;
    private bool ring2;
    private bool ring3;
  
    public GameObject stone1;
    public GameObject stone2;
    public GameObject stone3;
    public GameObject wall;

    public GameObject collider1;
    public GameObject collider2;
    public GameObject collider3;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        //moveSpeed = 7;
        count = 0;
        birdCount = 0;
        SetCountText();
        SetBirdCountText();

        ring1 = false;
        ring2 = false;
        ring3 = false;

        //indicator.SetActive(false);

        player = GameObject.Find("Player");
        dialog = player.GetComponent<Dialog>();

        soundscape = GameObject.Find("Sounds");

        mic = GameObject.Find("Mic Input").GetComponent<MicInput>();
        mic.StopMicrophone();
        recordText.SetActive(false);

        soundscape1 = soundscape.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        soundscape2 = soundscape.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        soundscape3 = soundscape.transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        soundscape4 = soundscape.transform.GetChild(3).gameObject.GetComponent<AudioSource>();

        stoneColor1 = stone1.GetComponent<MeshRenderer>().material.color;
        stoneColor2 = stone2.GetComponent<MeshRenderer>().material.color;
        stoneColor3 = stone3.GetComponent<MeshRenderer>().material.color;

        stoneColor1.a = 0.0f;
        stoneColor2.a = 0.0f;
        stoneColor3.a = 0.0f;
        stone1.GetComponent<MeshRenderer>().material.color = stoneColor1;
        stone2.GetComponent<MeshRenderer>().material.color = stoneColor2;
        stone3.GetComponent<MeshRenderer>().material.color = stoneColor3;
        Debug.Log("Stone Alpha: " + stone1.GetComponent<MeshRenderer>().material.color);

        stone1.SetActive(false);
        stone2.SetActive(false);
        stone3.SetActive(false);
    }

    private void FixedUpdate()
    {
        loudness = MicInput.MicLoudness;

        //Movement Code
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

        /*Toggling recording mic input on and off
        if (dialog.isTalking == false && Input.GetKeyDown(KeyCode.Space))
        {
            if (mic.isRecording == false)
            {
                mic.InitMic();
                recordText.SetActive(true);

            } else
            {
                mic.StopMicrophone();
                recordText.SetActive(false);
            }
        } else if (dialog.isTalking == true && mic.isRecording == true)
        {
            mic.StopMicrophone();
            recordText.SetActive(false);
        }*/

        //make objects appear
        if (birdCount == 2 & !collider1.activeInHierarchy)
        {
            stone1.SetActive(true);
            wall.SetActive(false);
        }

        if (mic.isRecording)
        {
            recordText.SetActive(true);
        } else
        {
            recordText.SetActive(false);
        }
        if (ring1 && MicInput.MicLoudness > 0.1f && stoneColor1.a < 1f)
        {
            stoneColor1.a += 0.1f;
            stone1.GetComponent<MeshRenderer>().material.color = stoneColor1;
            Debug.Log("Stone Alpha: " + stone1.GetComponent<MeshRenderer>().material.color);
        } else if (stoneColor1.a >= 1f && stone1.activeInHierarchy)
        {
            mic.StopMicrophone();
            collider1.SetActive(true);
            stone1.SetActive(false);
            stone2.SetActive(true);
            ring1 = false;

        }

        if (ring2 && MicInput.MicLoudness > 0.1f && stoneColor2.a < 1f)
        {
            stoneColor2.a += 0.1f;
            stone2.GetComponent<MeshRenderer>().material.color = stoneColor2;
            Debug.Log("Stone Alpha: " + stone2.GetComponent<MeshRenderer>().material.color);
        } else if (stoneColor2.a >= 1f && stone2.activeInHierarchy)
        {
            mic.StopMicrophone();
            collider2.SetActive(true);
            stone2.SetActive(false);
            stone3.SetActive(true);
            ring2 = false;
            
        }

        if (ring3 && MicInput.MicLoudness > 0.1f && stoneColor3.a < 1f)
        {
            stoneColor3.a += 0.1f;
            stone3.GetComponent<MeshRenderer>().material.color = stoneColor3;
            Debug.Log("Stone Alpha: " + stone3.GetComponent<MeshRenderer>().material.color);
        } else if (stoneColor3.a >= 1f && stone3.activeInHierarchy)
        {
            mic.StopMicrophone();
            collider3.SetActive(true);
            stone3.SetActive(false);
            ring3 = false;
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);

            if (other.gameObject.transform.parent.tag == "Collectable" && 
                other.gameObject.transform.parent.gameObject.activeInHierarchy == true)
            {
                Debug.Log(other.gameObject.transform.parent.tag);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    other.gameObject.transform.parent.gameObject.SetActive(false);
                    count++;
                    //SetCountText();
                }
            }

            if (other.gameObject.transform.parent.tag == "Songkeeper")
            {
                if (Input.GetKeyDown(KeyCode.E) && count > 0 && dialog.isTalking == false)
                {
                    //count--;
                   // birdCount++;
                    //SetCountText();
                    //SetBirdCountText();

                  /*  if (birdCount == 1)
                    {
                        piece1.SetActive(false);
                        soundscape1.Play();
                        Debug.Log("Playing Leaves.");
                        
                    }
                    else if (birdCount == 2)
                    {
                        piece2.SetActive(false);
                        soundscape2.Play();
                        Debug.Log("Playing Wind.");
                    }
                    else if (birdCount == 3)
                    {
                        piece3.SetActive(false);
                        soundscape3.Play();
                        Debug.Log("Playing Birdsong.");
                    }
                    else if (birdCount == 4)
                    {
                        piece4.SetActive(false);
                        soundscape4.Play();
                        Debug.Log("Playing Music.");
                    } */
                }
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Record Ring 1")
        {
            mic.InitMic();
            recordText.SetActive(true);
            ring1 = true;
        }

        if (other.gameObject.tag == "Record Ring 2")
        {
            mic.InitMic();
            recordText.SetActive(true);
            ring2 = true;
        }

        if (other.gameObject.tag == "Record Ring 3")
        {
            mic.InitMic();
            recordText.SetActive(true);
            ring3 = true;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            //indicator.SetActive(false);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Record Ring 1")
        {
            mic.StopMicrophone();
            recordText.SetActive(false);
            ring1 = false;
        }

        if (other.gameObject.tag == "Record Ring 2")
        {
            mic.StopMicrophone();
            recordText.SetActive(false);
            ring2 = false;
        }

        if (other.gameObject.tag == "Record Ring 3")
        {
            mic.StopMicrophone();
            recordText.SetActive(false);
            ring3 = false;
        }
    }

    void SetCountText()
    {
        countText.text = "";
    }

    void SetBirdCountText()
    {
        birdCountText.text = "";
    }
}
