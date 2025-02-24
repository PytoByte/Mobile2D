using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    bool call = false;
    float step = 0;
    float stepCount = 10;

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    public void CallPause()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (1000 * Screen.height), transform.position.z);
        call = true;
        Camera.main.GetComponent<CamEvents>().pause = true;
    }

    public void ClosePause()
    {
        step = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y + (1000 * Screen.height), transform.position.z);
        Color myColor = gameObject.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = new Color(myColor.r, myColor.g, myColor.b, 0);

        foreach (Transform t in transform)
        {
            if (t.gameObject.name != "pauseText (TMP)")
            {
                Color tColor = t.gameObject.GetComponent<Image>().color;
                t.gameObject.GetComponent<Image>().color = new Color(tColor.r, tColor.g, tColor.b, 0);

                Transform tText = t.Find("text");
                if (tText != null)
                {
                    tColor = tText.gameObject.GetComponent<Text>().color;
                    tText.gameObject.GetComponent<Text>().color = new Color(tColor.r, tColor.g, tColor.b, 0);
                }
            }

            else
            {
                Color tColor = t.gameObject.GetComponent<TextMeshProUGUI>().color;
                t.gameObject.GetComponent<TextMeshProUGUI>().color = new Color(tColor.r, tColor.g, tColor.b, 0);
            }
        }
        Camera.main.GetComponent<CamEvents>().pause = false;
    }

    void Update()
    {
        if (call)
        {
            step++;

            Color myColor = gameObject.GetComponent<Image>().color;
            gameObject.GetComponent<Image>().color = new Color(myColor.r, myColor.g, myColor.b, 0.8f * (step / stepCount));

            foreach (Transform t in transform)
            {
                if (t.gameObject.name != "pauseText (TMP)")
                {
                    Color tColor = t.gameObject.GetComponent<Image>().color;
                    t.gameObject.GetComponent<Image>().color = new Color(tColor.r, tColor.g, tColor.b, 1 * (step / stepCount));

                    Transform tText = t.Find("Text");
                    if (tText != null)
                    {
                        tColor = tText.gameObject.GetComponent<Text>().color;
                        tText.gameObject.GetComponent<Text>().color = new Color(tColor.r, tColor.g, tColor.b, 1 * (step / stepCount));
                    }
                } 
                
                else
                {
                    Color tColor = t.gameObject.GetComponent<TextMeshProUGUI>().color;
                    t.gameObject.GetComponent<TextMeshProUGUI>().color = new Color(tColor.r, tColor.g, tColor.b, 1 * (step / stepCount));
                }
            }
            if (step == stepCount)
            {
                call = false;
            }
        }
    }
}
