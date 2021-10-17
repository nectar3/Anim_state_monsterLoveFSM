using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform player;
    public float speed;

    public Vector2 home;

    public float atkCoolTime = 4;
    public float atkDelay;

    public Transform boxpos;
    public Vector2 boxSize;

    Animator animator;

    void Start()
    {

        home = transform.position;
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (atkDelay >= 0)
        {
            atkDelay -= Time.deltaTime;
        }
    }


    public void DirectionEnemy(float target, float baseobj)
    {
        if (target < baseobj)
            animator.SetFloat("Direction", -1);
        else
            animator.SetFloat("Direction", 1);
    }



    public void Attack()
    {
        if (animator.GetFloat("Direction") == -1)
        {
            if (boxpos.localPosition.x > 0)
                boxpos.localPosition = new Vector2(boxpos.localPosition.x * -1, boxpos.localPosition.y);
        }
        else
        {
            if (boxpos.localPosition.x < 0)
                boxpos.localPosition = new Vector2(Mathf.Abs(boxpos.localPosition.x), boxpos.localPosition.y);
        }

        Debug.Log(boxSize);
        Collider2D[] collids = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        foreach (Collider2D col in collids)
        {
            if (col.tag == "Player")
            {
                Debug.Log("damage");
            }
        }

    }


}
