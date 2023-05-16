using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningShipCredits : MonoBehaviour
{
    private Animator anim;
    public Image shipImage;
    public SpriteRenderer shipeSprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = shipeSprite.GetComponent<Animator>();
        anim.SetBool("EngineOn", true);
    }

    private void FixedUpdate()
    {
        shipImage.sprite = shipeSprite.sprite;
        transform.Rotate(0, 0, 0.3f);
    }
}
