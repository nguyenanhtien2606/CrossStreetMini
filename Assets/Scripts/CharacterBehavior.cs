using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    Animator anim;

    public float speed = 0.5f;
    public float fastSpeed;
    public float lowSpeed = 0.1f;
    public float rotateSpeed = 30;

    Vector3 moveDir = Vector3.zero;
    public float gravity = 20.0f;
    float currentAngle;
    float targetAngle;

    public bool isDead;
    public bool isGrab;

    private Rigidbody[] rigids;
    public bool _isGrounded = true;
    private Transform _groundChecker;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;

    public GameObject bloodTexture;

    public GameObject seftGold;

    private void Start()
    {
        seftGold.SetActive(false);
        seftGold.GetComponent<Rigidbody>().isKinematic = true;
        seftGold.GetComponent<Collider>().enabled = false;

        isDead = false;
        _groundChecker = transform.GetChild(0);
        anim = GetComponent<Animator>();

        SetRigidbodyState(true);
        SetColliderState(false);

        fastSpeed = speed;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            MovementCharacter();
        }
    }

    void MovementCharacter()
    {
        moveDir.z = Input.GetAxis("Vertical");
        moveDir.x = Input.GetAxis("Horizontal");

        if (moveDir.z != 0 || moveDir.x != 0)
        {
            moveDir = new Vector3(-moveDir.z, 0, moveDir.x);

            if (_isGrounded == false)
            {
                moveDir.y -= gravity;
            }

            GetComponent<Rigidbody>().MovePosition(transform.position + moveDir * speed);

            var newRotation = Quaternion.LookRotation(moveDir);
            newRotation.x = 0.0f;
            newRotation.z = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotateSpeed);

            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            isDead = true;
            anim.enabled = false;
            SetRigidbodyState(false);
            SetColliderState(true);

            seftGold.GetComponent<Collider>().enabled = true;
            seftGold.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(seftGold, 2);
            GameManager.global.gameObject.GetComponent<AudioSource>().PlayOneShot(GameManager.global.deathSound);

            Instantiate(bloodTexture, transform.position, Quaternion.Euler(90, 0, 0));

            StartCoroutine(SpawnAndDestroy());
        }

        if (other.gameObject.tag == "Gold")
        {
            if (!isGrab)
            {
                GrabObj(true);

                other.gameObject.SetActive(false);
                StartCoroutine(other.gameObject.GetComponent<GoldBehavior>().AutoActiveSeft());
            }
        }

        if (other.gameObject.tag == "EndPoint")
        {
            if (isGrab)
            {
                GrabObj(false);
                GameManager.global.moneyParticle.Play();
                GameManager.global.score++;
                GameManager.global.scoreText.text = GameManager.global.score.ToString();
                GameManager.global.gameObject.GetComponent<AudioSource>().PlayOneShot(GameManager.global.scoreSound);
            }
        }
    }

    IEnumerator SpawnAndDestroy()
    {
        yield return new WaitForSeconds(2);
        GameManager.global.SpawnCharacter();
        Destroy(gameObject);
    }

    void GrabObj(bool grab)
    {
        if (grab)
        {
            isGrab = true;
            anim.SetBool("Grab", true);
            seftGold.SetActive(true);
            speed = lowSpeed;
        }
        else
        {
            isGrab = false;
            anim.SetBool("Grab", false);
            seftGold.SetActive(false);
            speed = fastSpeed;
        }
    }

    void SetRigidbodyState(bool state)
    {
        rigids = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigids)
        {
            rb.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }
}
