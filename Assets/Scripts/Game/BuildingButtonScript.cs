using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class BuildingButtonScript : MonoBehaviour
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
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("building_buy_me");
        if (buildings.Length > 0)
        {
            moveDown(buildings[0]);
            buildings[0].tag = "buildingToBuy";
            pressNow = 0;
        }
    }

    int pressNow = 0;
    public void ButtonPress()
    {
        RemoveZone();
        GameObject UnitButton = GameObject.FindGameObjectsWithTag("UnitButton")[0];
        UnitButton.GetComponent<UnitButtonScript>().Bought();
        GameObject ColonistButton = GameObject.FindGameObjectsWithTag("ColonistButton")[0];
        ColonistButton.GetComponent<ColonistButtonScript>().Bought();

        GameObject[] selected = GameObject.FindGameObjectsWithTag("selected");
        if (selected.Length>0)
        {
            selected[0].tag = "character";
        }

        GameObject[] building_buy_me = GameObject.FindGameObjectsWithTag("building_buy_me");
        if (building_buy_me.Length > 0)
        {
            building_buy_me[0].tag = "buildingToBuy";
            moveDown(building_buy_me[0]);
        }

        pressNow += 1;
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("buildingToBuy");

        Debug.Log(pressNow);
        bool reset = true;
        GameObject buy_me = gameObject;
        foreach (GameObject building in buildings)
        {
            if (building.GetComponent<whoAmI>().place == pressNow )
            {
                buy_me = building;
                reset = false;
                break;
            }
        }
        if (reset)
        {
            pressNow = 0;
        } else
        {
            buy_me.tag = "building_buy_me";
            moveUp(buy_me);
        }
    }
    
    void moveUp(GameObject building)
    {
        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
        building.GetComponent<whoAmI>().MyColorDetails.GetComponent<Image>().color = my_events.turn;
        moveDown(building);
        building.GetComponent<whoAmI>().playAnimIn();
        building.tag = "building_buy_me";
        SeeZone();
    }
    void moveDown(GameObject building)
    {
        building.GetComponent<whoAmI>().undoAnim();
    }
}
