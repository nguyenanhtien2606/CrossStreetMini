using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    Rigidbody rigid;
    public float moveSpeed = 10f;
    public float gravity = 20.0f;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GameManager.global.score == 2)
        {
            moveSpeed = 30;
        }
        if (GameManager.global.score == 5)
        {
            moveSpeed = 40;
        }
        if (GameManager.global.score >= 8)
        {
            moveSpeed = 50;
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (gameObject.transform.rotation.y == 0)
        {
            rigid.velocity = new Vector3(0, 0, moveSpeed);
        }
        else
        {
            rigid.velocity = new Vector3(0, 0, -moveSpeed);
        }
        
    }
}
