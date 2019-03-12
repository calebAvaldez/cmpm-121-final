using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    public Text countText;
    public Text birdCountText;

    public CharacterController controller;

    public GameObject piece1;
    public GameObject piece2;
    public GameObject piece3;
    public GameObject piece4;

    private Vector3 moveDirection;
    public float gravityScale;
    private int count;
    private int birdCount;
    //private bool menuOpen;

    public AudioClip pickup;
    AudioSource audioSource;

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

    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {

            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            //audioSource.PlayOneShot(pickup, 0.7f);

        }

        if (other.gameObject.CompareTag("Songkeeper") && count > 0)
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
