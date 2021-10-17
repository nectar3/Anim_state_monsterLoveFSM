using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 50f;
    void Start()
    {
    }
    void Update()
    {
        float moveY = 0f;
        float moveX = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {

            moveY -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {

            moveX -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX += speed * Time.deltaTime;
        }
        transform.Translate(new Vector3(moveX, moveY, 0f) * 0.1f);
    }
}
