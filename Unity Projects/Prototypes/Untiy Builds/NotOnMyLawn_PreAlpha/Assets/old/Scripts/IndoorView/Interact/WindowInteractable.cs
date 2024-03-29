using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowInteractable : MonoBehaviour
{
    public bool isInteracting;
    public bool windowView;

    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;

    public Rigidbody2D Player;
    public bool zombieThroughTheWindow;

    public GameObject outsideGun;
    // Start is called before the first frame update
    void Start()
    {
         zombieThroughTheWindow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && windowView == true && isInteracting == false)
        {
            switchToOutside();
            isInteracting = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && isInteracting == true)
        {
            switchToInside();
            isInteracting = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            windowView = true;
        }

        if(collision.CompareTag("Zombie"))
        {
            zombieThroughTheWindow = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            windowView = false;
        }

        if (collision.CompareTag("Zombie"))
        {
            zombieThroughTheWindow = false;
        }
    }


    private void switchToOutside()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        Camera3.SetActive(false);
        
        outsideGun.SetActive(true);

        GameObject.Find("Player").GetComponent<TEMP_HorizontalMovement>().enabled = false;
        GameObject.Find("Gun").GetComponent<GunScript>().enabled = false;
        Player.velocity = new Vector2(0, 0);

    }
    private void switchToInside()
    {
        Camera1.SetActive(true);
        Camera2.SetActive(false);
        Camera3.SetActive(false);
        outsideGun.SetActive(false);

        GameObject.Find("Player").GetComponent<TEMP_HorizontalMovement>().enabled = true;
        GameObject.Find("Gun").GetComponent<GunScript>().enabled = true;
    }


}
