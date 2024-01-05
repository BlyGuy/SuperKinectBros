using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public Sprite mHandOpenSprite;
    public Sprite mHandClosedSprite;
    public bool handClosed = false;
    private bool handState = false;
    private bool isClosingFist = false;
    
    private SpriteRenderer mSpriteRenderer;

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //isClosingFist = false;
        //Switch to the right hand-sprite
        if (!handState && handClosed)
        {
            isClosingFist = true;
            mSpriteRenderer.sprite = mHandClosedSprite;
        } else if (handState && !handClosed) {
            mSpriteRenderer.sprite = mHandOpenSprite;
        }
        handState = handClosed;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            try { collision.GetComponent<Button>().animator.Play("Highlighted", 0); }
            catch { }
            if (handClosed && isClosingFist)
            {
                try { collision.GetComponent<Button>().animator.Play("Normal", 0); }
                catch { }
                collision.gameObject.GetComponent<Button>().onClick.Invoke();
            }
            isClosingFist = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try { collision.GetComponent<Button>().animator.Play("Normal", 0); }
        catch { }
    }
}
