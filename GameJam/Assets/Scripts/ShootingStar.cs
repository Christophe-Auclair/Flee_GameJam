using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    [SerializeField]
    RectTransform rect;

    private void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        float newX = transform.position.x + 1;
        float newY = transform.position.y - 1;

        //rect.anchoredPosition = new Vector3(newX, newY, 0f);
        transform.position = (new Vector3(newX, newY, 1f));
    }
}
