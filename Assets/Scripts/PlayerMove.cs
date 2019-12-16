using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public float movePower;
    public float jumpPower;

    private Queue<Vector3> velocities;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        velocities = new Queue<Vector3>(new List<Vector3>{ rigid.velocity, rigid.velocity});
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
        Ray();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(Input.GetAxisRaw("Horizontal") < 0 )
        {
            spriteRenderer.flipX = true;
            moveVelocity = Vector3.left;
        }else if(Input.GetAxisRaw("Horizontal") > 0 )
        {
            spriteRenderer.flipX = false;
            moveVelocity = Vector3.right;
        }
        anim.SetBool("isWalking", (System.Math.Abs(Input.GetAxisRaw("Horizontal")) > 0.001));

        transform.position += moveVelocity * movePower * Time.deltaTime;

        if(System.Math.Abs(rigid.velocity.y) < 0.01 && GetVectorValue())
        {
            anim.SetBool("isJumping", false);
        }
        SetVectorValue(rigid.velocity);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
    }

    void Ray()
    {
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f);

        if(rayHit.collider != null)
        {
            if(rayHit.distance <= 0.5f && rigid.velocity.y < 0f)
            {
                anim.SetBool("isJumping", false);
            }
            Debug.Log(rayHit.distance);
        }
    }

    void SetVectorValue(Vector3 data)
    {
        velocities.Dequeue();
        velocities.Enqueue(data);
    }

    bool GetVectorValue()
    {
        bool flag = true;

        foreach(Vector3 en in velocities.ToArray())
        {
            if(System.Math.Abs(en.y) > 0.0001f)
            {
                flag = false;
                break;
            }
        }

        return flag;
    }
}
