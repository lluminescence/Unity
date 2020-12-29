using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    
    public float speed;
    public float jumpHeight;
    public float insideTimerUp = 15f;

    public bool key = false;
    public bool inWater = false;

    public Main main;
    public Transform groundCheck;
    public GameObject blueGem, greenGem;
    public Image playerCountDown;
    public Image insideCountDown;
    public Inventory inventory;
    public SoundEffector soundEffector;
    Animator anim;

    int gemCount = 0;
    int curHp;
    int maxHp = 3;
    int coins = 0;

    bool isGrounded;
    bool isHit = false;
    bool canTp = true;
    bool isClimbing = false;
    bool gemUsage = true;

    float hitTimer = 0f;
    float insideTimer = -1f;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
    }

    
    void Update()
    {
        if (inWater && !isClimbing)
        {
            anim.SetInteger("State", 4);
            isGrounded = true;
            if (Input.GetAxis("Horizontal") != 0)
                Flip();
        }
        else
        {
            CheckGround();
            if (Input.GetAxis("Horizontal") == 0 && (isGrounded) && !isClimbing)
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded && !isClimbing)  
                    anim.SetInteger("State", 2);
            }
        }

        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
            

        if (insideTimer >= 0)
        {
            insideTimer += Time.deltaTime;
            if (insideTimer >= insideTimerUp)
            {
                insideTimer = 0f;
                RecountHp(-1);
            }
            else
            {
                insideCountDown.fillAmount = 1 - (insideTimer / insideTimerUp);
            }
        }

    }



    void FixedUpdate()
    {
        
    }
    
    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing)
            anim.SetInteger("State", 3);    
    }

    public void RecountHp(int deltaHp)
    {
        curHp += deltaHp;
        if (deltaHp < 0)
        {
            isHit = true;
            StartCoroutine(OnHit());
        }
        else if (curHp > maxHp)
        {
            curHp = maxHp;
        }
        if (curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        else if (gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Key"))
        {
            Destroy(collision.gameObject);
            key = true;
            inventory.AddKey();
        }

        if (collision.gameObject.tag == ("Door"))
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTp)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTp = false;
                StartCoroutine(TPwait());
            }

            else if (key)
                collision.gameObject.GetComponent<Door>().Unlock();
        }

        if (collision.gameObject.tag == ("Coin"))
        {
            Destroy(collision.gameObject);
            coins++;
            soundEffector.PlayCoinSound();
        }

        if (collision.gameObject.tag == ("Heart"))
        {
            Destroy(collision.gameObject);
            inventory.AddHp();
        }

        if (collision.gameObject.tag == ("Mushroom"))
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
        }
        
        if (collision.gameObject.tag == ("GreenGem"))
        {
            Destroy(collision.gameObject);
            //StartCoroutine(SpeedBonus());
            inventory.AddGg();
        }

        if (collision.gameObject.tag == ("TimerButtonStart"))
        {
            insideTimer = 0f;
        }

        if (collision.gameObject.tag == ("TimerButtonStop"))
        {
            insideTimer = -1f;
            insideCountDown.fillAmount = 0f;
        }

        if (collision.gameObject.tag == ("Frontier"))
        {
            Lose();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ladder")
        {
            isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if(Input.GetAxis("Vertical") == 0)
            {
                anim.SetInteger("State", 5);
            }
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
            }
            
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 7f;
                speed *= 0.25f;
            }
        }

        if (collision.gameObject.tag == "Lava")
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= 3f)
            {
                hitTimer = 0f;
                playerCountDown.fillAmount = 1f;
                RecountHp(-1);
            }
            else
            {
                playerCountDown.fillAmount = 1 - (hitTimer / 3f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 7f)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }

        if (collision.gameObject.tag == "Lava")
        {
            hitTimer = 0f;
            playerCountDown.fillAmount = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
        if (collision.gameObject.tag == "Quicksand")
        {
            speed *= 0.25f;
            rb.mass *= 100f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Quicksand")
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }

    IEnumerator OnHit()
    {
        if (isHit)
        {
            if (GetComponent<SpriteRenderer>().color.g > 0)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
            }
        }
        else
        {
            if (GetComponent<SpriteRenderer>().color.g < 1)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);
            }
        }

        if (GetComponent<SpriteRenderer>().color.g >= 1f)
        {
            StopCoroutine(OnHit());
        }
        if (GetComponent<SpriteRenderer>().color.g <= 0f)
        {
            isHit = false;
        }

        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(1f);
        canTp = true;
    }

    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.3f);
        an.SetBool("isJump", false);
    }
    
    IEnumerator SpeedBonus()
    {
        if (gemUsage)
        {
            gemCount++;
            greenGem.SetActive(true);
            CheckGems(greenGem);

            speed *= 2;
            greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

            StartCoroutine(WaitTime());

            yield return new WaitForSeconds(4f);
            StartCoroutine(Invis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
            yield return new WaitForSeconds(1f);
            speed /= 2;

            gemCount--;
            greenGem.SetActive(false);
            CheckGems(blueGem);
        }
        else
        {
            StopCoroutine(WaitTime());
        }
    }

    IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if(spr.color.a > 0)
        {
            StartCoroutine(Invis(spr, time));
        }
    }

    public IEnumerator WaitTime()
    {
        gemUsage = false;
        yield return new WaitForSeconds(4f);
        gemUsage = true;
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetHearts()
    {
        return curHp;
    }

    public void GreenGem()
    {
        StartCoroutine(SpeedBonus());
    }
}
