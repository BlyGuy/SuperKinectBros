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
        //Switch to the right hand-sprite
        if (!handState && handClosed)
        {
            handState = true;
            mSpriteRenderer.sprite = mHandClosedSprite;
        } else if (handState && !handClosed) {
            handState = false;
            mSpriteRenderer.sprite = mHandOpenSprite;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            //collision.GetComponent<Button>().animator.Play("Highlighted", 0);
            if(handClosed)
                collision.gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
}
