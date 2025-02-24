using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whoAmI : MonoBehaviour
{
    public GameObject Me;
    public GameObject MyColorDetails;
    public int priceFood;
    public int priceWood;
    public int place;
    bool play = false;

    float startY;

    float frames = 10;
    float frameNow = 0;
    float deltaPos = 1000;

    public void Start()
    {
        startY = (Screen.height * 2.5f) / 15 - 1000;

        if (Screen.height >= 1280)
        {
            startY = (Screen.height * 2.5f) / 15 - (1000 * (Screen.height / 1280));
        }
        else
        {
            startY = (Screen.height * 2.5f) / 15 - (1000 * (Screen.height * 0.00078125f));
        }
    }

    public void playAnimIn()
    {
        play = true;
    }

    public void undoAnim()
    {
        play = false;

        if (Screen.height >= 1280)
        {
            transform.position = new Vector3((Screen.width * 6) / 12, startY - (1000 * (Screen.height / 1280)), 0);
        }
        else
        {
            transform.position = new Vector3((Screen.width * 6) / 12, startY - (1000 * (Screen.height * 0.00078125f)), 0);
        }
    }

    void Update()
    {
        if (play)
        {
            if (frameNow > frames)
            {
                frameNow = 0;
                play = false;
            }
            else
            {
                if (Screen.height >= 1280)
                {
                    transform.position = new Vector3((Screen.width * 6) / 12, startY + ((frameNow / frames) * deltaPos * (Screen.height / 1280)), 0);
                }
                else
                {
                    transform.position = new Vector3((Screen.width * 6) / 12, startY + ((frameNow / frames) * deltaPos * (Screen.height * 0.00078125f)), 0);
                }

                frameNow += 1;
            }
        }   
    }
}
