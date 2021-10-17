using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;


public class Enemy_monsterFSM : MonoBehaviour
{
    public enum States
    {
        Idle,
        Follow,
        Back,
        Ready,
        Attack,
    }

    public GameObject Hitbox;

    public float speed = 5f;
    public float AttackDelay = 3f;
    private float attackDelayRemaining = 0f;

    SpriteRenderer sprite;

    Animator animator;

    StateMachine<States, StateDriverUnity> fsm;

    Transform player;

    Vector2 home;


    private void Awake()
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);
    }

    private void Start()
    {
        home = transform.position;

        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fsm.ChangeState(States.Idle);

    }

    private void Update()
    {
        fsm.Driver.Update.Invoke();

        if (attackDelayRemaining > 0)
            attackDelayRemaining -= Time.deltaTime;
    }

    void Idle_Enter()
    {
        Debug.Log("Idle_Enter");

        animator.Play("Idle_L");
    }


    void Idle_Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= 4)
        {
            fsm.ChangeState(States.Follow);

        }
    }

    void Follow_Enter()
    {
        Debug.Log("Follow_Enter");
        animator.Play("Walk_L");
    }


    void Follow_Update()
    {

        if (Vector2.Distance(player.position, transform.position) > 4f)
        {
            fsm.ChangeState(States.Back);
        }
        else if (Vector2.Distance(player.position, transform.position) > 1f)
            transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * speed);
        else
        {
            fsm.ChangeState(States.Ready);
        }
        DirectionEnemy(player.position.x, transform.position.x);
    }

    void Back_Enter()
    {
        Debug.Log("Back_Enter");
        animator.Play("Walk_L");
    }

    void Back_Update()
    {
        if (Vector2.Distance(home, transform.position) < 0.1f)
        {
            fsm.ChangeState(States.Idle);
        }
        else if (Vector2.Distance(transform.position, player.position) < 4f)
        {
            fsm.ChangeState(States.Follow);
        }
        else
        {
            DirectionEnemy(home.x, transform.position.x);
            transform.position = Vector2.MoveTowards(transform.position, home, speed * Time.deltaTime);
        }

    }



    void Ready_Enter()
    {
        Debug.Log("Ready_Enter");
        animator.Play("Idle_L");

    }

    void Ready_Update()
    {
        DirectionEnemy(player.position.x, transform.position.x);

        if (attackDelayRemaining <= 0)
            fsm.ChangeState(States.Attack);

        if (Vector2.Distance(player.position, transform.position) > 1f)
        {
            fsm.ChangeState(States.Follow);
        }
    }


    IEnumerator Attack_Enter()
    {
        Debug.Log("Attack_Enter");

        var duration = PlayAnimAndGetDuration("Attack_L");

        yield return new WaitForSeconds(duration);

        fsm.ChangeState(States.Ready);
    }

    void Attack_Exit()
    {
        Debug.Log("Attack_Exit");
        attackDelayRemaining = AttackDelay;
    }


    public void AttackHitBoxCheck()
    {
        if (sprite.flipX == false)
            Hitbox.transform.localPosition = new Vector2(Hitbox.transform.localPosition.x * -1, Hitbox.transform.localPosition.y);
        else
            Hitbox.transform.localPosition = new Vector2(Mathf.Abs(Hitbox.transform.localPosition.x), Hitbox.transform.localPosition.y);


        var cols = Physics2D.OverlapBoxAll(Hitbox.transform.position, Vector2.zero, 0);
        foreach (var col in cols)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("damage!!!");
            }
        }
    }

    void DirectionEnemy(float targetX, float enemyX)
    {
        if (targetX < enemyX)
            sprite.flipX = false;
        else
            sprite.flipX = true;
    }

    float PlayAnimAndGetDuration(string animName)
    {
        animator.Play(animName);
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }



}
