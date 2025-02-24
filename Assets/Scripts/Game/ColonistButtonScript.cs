using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class ColonistButtonScript : MonoBehaviour
{
    public GameObject zone;

    public List<GameObject> availableBlocks = new List<GameObject>();

    public void SeeZone()
    {
        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        for (int i = 0; i < boxes.Length; i++)
        {
            if (my_events.turn != boxes[i].GetComponent<Renderer>().material.color)
            {
                Instantiate(zone, boxes[i].transform.position, boxes[i].transform.rotation);
            } else { availableBlocks.Add(boxes[i]); }
        }
    }


    public void RemoveZone()
    {
        Debug.Log("nee");
        GameObject[] zoneBlocksToDestroy = GameObject.FindGameObjectsWithTag("zone");
        for (int i = 0; i < zoneBlocksToDestroy.Length; i++)
        {
            Destroy(zoneBlocksToDestroy[i]);
        }
        availableBlocks.Clear();
    }


    public void Bought()
    {
        RemoveZone();
        Debug.Log("execute " + pressNow.ToString());
        GameObject[] colonists = GameObject.FindGameObjectsWithTag("colonist_buy_me");
        if (colonists.Length>0)
        {
            moveDown(colonists[0]);
            colonists[0].tag = "colonistToBuy";
            pressNow = 0;
        }
    }

    int pressNow = 0;
    public void ButtonPress()
    {
        RemoveZone();
        GameObject UnitButton = GameObject.FindGameObjectsWithTag("UnitButton")[0];
        UnitButton.GetComponent<UnitButtonScript>().Bought();
        GameObject BuildingButton = GameObject.FindGameObjectsWithTag("BuildingButton")[0];
        BuildingButton.GetComponent<BuildingButtonScript>().Bought();

        GameObject[] selected = GameObject.FindGameObjectsWithTag("selected");
        if (selected.Length>0)
        {
            selected[0].tag = "character";
        }

        GameObject[] colonist_buy_me = GameObject.FindGameObjectsWithTag("colonist_buy_me");
        if (colonist_buy_me.Length > 0)
        {
            colonist_buy_me[0].tag = "colonistToBuy";
            moveDown(colonist_buy_me[0]);
        }

        pressNow += 1;
        GameObject[] colonists = GameObject.FindGameObjectsWithTag("colonistToBuy");

        Debug.Log(pressNow);
        bool reset = true;
        GameObject buy_me = gameObject;
        foreach (GameObject colonist in colonists)
        {
            if (colonist.GetComponent<whoAmI>().place == pressNow )
            {
                buy_me = colonist;
                reset = false;
                break;
            }
        }
        if (reset)
        {
            Debug.Log("Reset");
            pressNow = 0;
        } else
        {
            buy_me.tag = "colonist_buy_me";
            moveUp(buy_me);
        }
    }
    
    void moveUp(GameObject colonist)
    {
        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
        colonist.GetComponent<whoAmI>().MyColorDetails.GetComponent<Image>().color = my_events.turn;
        moveDown(colonist);
        colonist.GetComponent<whoAmI>().playAnimIn();
        colonist.tag = "colonist_buy_me";
        SeeZone();
    }
    void moveDown(GameObject colonist)
    {
        colonist.GetComponent<whoAmI>().undoAnim();
    }
}
