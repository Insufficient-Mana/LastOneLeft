using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{

    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Destroy(gameObject);
            collision.GetComponent<TopviewZmbHealth>().currentHealth -= damage;
        }
    }


}
