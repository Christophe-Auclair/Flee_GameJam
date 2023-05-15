using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SuperNova : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private bool spawned;

    private void Start()
    {
        Spawn();
    }

    public async void Spawn()
    {
        await Task.Delay(3000);
        spawned = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (spawned)
        {
            float newX = transform.position.x + speed;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}
