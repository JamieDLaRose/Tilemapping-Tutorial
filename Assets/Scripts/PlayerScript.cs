using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text win;
    public Text lives;

    private int livesValue;
    private int scoreValue;

    private bool facingRight;

    public AudioSource musicSource;
    public AudioSource playerSounds;

    public AudioClip backgroundMusic;
    public AudioClip victorySound;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        livesValue = 3;
        scoreValue = 0;
        facingRight = true;
        win.text = "";
        lives.text = "";
        SetScore();
        SetLives();
        musicSource.loop = true;
        playerSounds.loop = false;
        musicSource.clip = backgroundMusic;
        playerSounds.clip = victorySound;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Player Flipping
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f && !facingRight)
            {
                Flip();
                facingRight = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0.5f && facingRight)
            {
                Flip();
                facingRight = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
            Application.Quit();

        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetScore();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                anim.SetInteger("State", 0);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                anim.SetInteger("State", 1);
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                anim.SetInteger("State", 0);
            }
        }
    }

    void SetScore()
    {
        score.text = "Score: " + scoreValue.ToString();
        if (scoreValue == 4 && livesValue > 0)
        {
            win.text = "You win! Game created by Jamie LaRose.";
            playerSounds.Play();
        }
    }

    void SetLives()
    {
        lives.text = "Lives: " + livesValue.ToString();
        if (livesValue <= 0 && scoreValue < 4)
        {
            win.text = "You lose. Game created by Jamie LaRose.";
            this.gameObject.SetActive(false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
