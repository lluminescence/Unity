using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    public GameObject[] block;
    public Sprite buttonDown;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MarkBox")
        {
            GetComponent<SpriteRenderer>().sprite = buttonDown;
            GetComponent<CircleCollider2D>().enabled = false;
            foreach (GameObject obj in block)
            {
                Destroy(obj);
            }
        }
    }
}