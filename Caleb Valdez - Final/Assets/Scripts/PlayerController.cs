using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    public Text countText;

    public CharacterController controller;

    private Vector3 moveDirection;
    public float gravityScale;
    private int count;
    //private bool menuOpen;

    public AudioClip pickup;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        moveSpeed = 7;
        count = 0;
        SetCountText();
    }

    private void FixedUpdate()
    {
        //if (menuOpen == false)
       // {
        if (controller.isGrounded)
        {
            moveDirection.y = 0.0f;

            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
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
    }

    void SetCountText()
    {
        countText.text = count.ToString();
    }
}
