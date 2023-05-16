using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingStar : MonoBehaviour
{
    RectTransform rect;
    public Image starImage;
    public SpriteRenderer starSprite;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        GameObject image = GameObject.Find("ShootingStarSprite");
        starSprite = image.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        starImage.sprite = starSprite.sprite;

        float newX = rect.anchoredPosition.x + 10;
        float newY = rect.anchoredPosition.y - 10;

        rect.anchoredPosition = new Vector2(newX, newY);
    }
}
