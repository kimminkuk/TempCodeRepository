using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Player Init Variables
    public Vector2 inputVec;
    public float speed = 5f;
    public Scanner scanner;
    public Hand[] hands;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() 
    {
        //1. 힘을 준다
        //rigid.AddForce(inputVec * 10f, ForceMode2D.Impulse);

        //2. 속도 제어
        //rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, 5f);

        //3. 위치 이동
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    //프레임이 종료 되기 전 실행되는 함수
    void LateUpdate() 
    {
        anim.SetFloat("Speed", inputVec.magnitude); //inputVec의 크기만 전


        if (inputVec.x != 0) {
            spriteRenderer.flipX = inputVec.x < 0; //좌측이면, true flipX가 켜진거임 그리고 우측이면 false
        }
    }
}
