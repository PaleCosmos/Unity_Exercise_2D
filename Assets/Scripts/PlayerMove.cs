using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public float maxSpeed;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // right
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -maxSpeed) // left
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        if (Input.GetButtonUp("Horizontal"))
        {
            anim.SetBool("isWalking", false);
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        if(Input.GetButtonDown("Horizontal"))
        {
            anim.SetBool("isWalking", true);
            spriteRenderer.flipX = System.Math.Abs(Input.GetAxisRaw("Horizontal") - -1) < 0.001;
        }
    }

    void FixedUpdate()
    {
        
    }
}
