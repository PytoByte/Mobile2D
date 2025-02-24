using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonScript : MonoBehaviour
{
    public GameObject zone;
    public List<GameObject> availableBlocks = new List<GameObject>();
    List<GameObject> zoneBlocks = new List<GameObject>();

    // Визуально показать, где можно разместить персонажа, с помощью зоны
    public void SeeZone()
    {
        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
        //my_events.turn
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        character subject = GameObject.FindGameObjectsWithTag("buy_me")[0].GetComponent<whoAmI>().Me.GetComponent<character>();
        for (int i = 0; i < boxes.Length; i++)
        {
            if ((boxes[i].GetComponent<box>().character != null) & (my_events.turn == boxes[i].GetComponent<Renderer>().material.color))
            {
                if ((boxes[i].GetComponent<box>().character.GetComponent<character>().id == subject.id) & (!subject.highestPower))
                {
                    availableBlocks.Add(boxes[i]);
                }

                else
                {
                    zoneBlocks.Add(boxes[i]);
                }
            }



            else if (my_events.turn != boxes[i].GetComponent<Renderer>().material.color)
            {
                if (boxes[i].GetComponent<box>().character != null)
                {
                    if ((subject.power <= boxes[i].GetComponent<box>().character.GetComponent<character>().power) & (!subject.highestPower) & !(subject.CanKillSamePower & (subject.power == boxes[i].GetComponent<box>().character.GetComponent<character>().power)))
                    {
                        zoneBlocks.Add(boxes[i]);
                    }

                    else
                    {
                        bool placeZone = true;
                        for (int j = 0; j < boxes.Length; j++)
                        {
                            if ((my_events.turn == boxes[j].GetComponent<Renderer>().material.color) & (Vector3.Distance(boxes[j].transform.position, boxes[i].transform.position) < subject.movementDistanceOutColor))
                            {
                                placeZone = false;
                                break;
                            }
                        }
                        if (placeZone) { zoneBlocks.Add(boxes[i]); } else { availableBlocks.Add(boxes[i]); }
                    }
                }

                else if ((!subject.highestPower) & (subject.power <= boxes[i].GetComponent<box>().power) & !(subject.CanKillSamePower & (subject.power == boxes[i].GetComponent<box>().power)))
                {
                    zoneBlocks.Add(boxes[i]);
                }

                else
                {
                    bool placeZone = true;
                    for (int j = 0; j < boxes.Length; j++)
                    {
                        if ((my_events.turn == boxes[j].GetComponent<Renderer>().material.color) & (Vector3.Distance(boxes[j].transform.position, boxes[i].transform.position) < subject.movementDistanceOutColor))
                        {
                            placeZone = false;
                            break;
                        }
                    }
                    if (placeZone) { zoneBlocks.Add(boxes[i]); } else { availableBlocks.Add(boxes[i]); }
                }
            }
            else { availableBlocks.Add(boxes[i]); }
        }

        for (int i = 0; i < zoneBlocks.Count; i++)
        {
            Instantiate(zone, zoneBlocks[i].transform.position, zoneBlocks[i].transform.rotation);
        }
        zoneBlocks.Clear();
    }

    // Убрать зону
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

    // Вернуть магазин в изначальное состояние
    public void Bought()
    {
        RemoveZone();
        Debug.Log("execute " + pressNow.ToString());
        GameObject[] units = GameObject.FindGameObjectsWithTag("buy_me");
        if (units.Length > 0)
        {
            moveDown(units[0]);
            units[0].tag = "unitToBuy";
            pressNow = 0;
        }
    }

    // Выбор персонажа для покупки
    int pressNow = 0;
    public void ButtonPress()
    {
        RemoveZone();
        GameObject ColonistButton = GameObject.FindGameObjectsWithTag("ColonistButton")[0];
        ColonistButton.GetComponent<ColonistButtonScript>().Bought();
        GameObject BuildingButton = GameObject.FindGameObjectsWithTag("BuildingButton")[0];
        BuildingButton.GetComponent<BuildingButtonScript>().Bought();

        GameObject[] selected = GameObject.FindGameObjectsWithTag("selected");
        if (selected.Length>0)
        {
            selected[0].tag = "character";
        }

        GameObject[] unit_buy_me = GameObject.FindGameObjectsWithTag("buy_me");
        if (unit_buy_me.Length > 0)
        {
            unit_buy_me[0].tag = "unitToBuy";
            moveDown(unit_buy_me[0]);
        }

        pressNow += 1;
        GameObject[] units = GameObject.FindGameObjectsWithTag("unitToBuy");

        Debug.Log(pressNow);
        bool reset = true;
        GameObject buy_me = gameObject;
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<whoAmI>().place == pressNow )
            {
                buy_me = unit;
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
            buy_me.tag = "buy_me";
            moveUp(buy_me);
        }
    }
    
    // Показать выбранного персонажа для покупки
    void moveUp(GameObject unit)
    {
        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
        unit.GetComponent<whoAmI>().MyColorDetails.GetComponent<Image>().color = my_events.turn;
        moveDown(unit);
        unit.GetComponent<whoAmI>().playAnimIn();
        unit.tag = "buy_me";
        SeeZone();
    }

    // Скрыть персонажа
    void moveDown(GameObject unit)
    {
        unit.GetComponent<whoAmI>().undoAnim();
    }
}
