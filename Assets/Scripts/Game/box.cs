using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    public GameObject zone;
    public GameObject character;
    public GameObject building;
    public int power = -2;
    public string economic_zone;

    public void Correct(float distance)
    {
        int match = 0;
        int needMatch = 2;
        bool alive = false;
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        for (int i = 0; i < boxes.Length; i++)
        {
            if (Vector3.Distance(boxes[i].transform.position, transform.position) == distance)
            {
                match += 1;
                if (match == needMatch)
                {
                    alive = true;
                    break;
                }
            }
        }

        if (!alive)
        {
            gameObject.tag = "Destroyed";
            Destroy(gameObject);
        }
    }

    public int OnMouseDown()
    {
        //Debug.Log(character);

        if ( (transform.position.y > Camera.main.transform.position.y-(4*(Camera.main.GetComponent<Camera>().orthographicSize/Camera.main.GetComponent<CamEvents>().zoomMax))) | ( (GameObject.FindGameObjectsWithTag("buy_me").Length<=0) & (GameObject.FindGameObjectsWithTag("colonist_buy_me").Length<=0) & (GameObject.FindGameObjectsWithTag("building_buy_me").Length<=0) ) ) 
        {
            GameObject[] selected = GameObject.FindGameObjectsWithTag("selected");

            if ((selected.Length == 1) & (character!=null))
            {
                if (selected[0].transform.position == character.transform.position)
                {
                    selected[0].GetComponent<character>().RemoveZone();
                }
            }

            my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();

            if ( (selected.Length == 1) & (character == null) )
            {
                if (selected[0].GetComponent<character>().availableBlocks.Contains(gameObject))
                {
                    Move(selected[0]);
                } else { selected[0].GetComponent<character>().RemoveZone(); }
            }


            else if (character != null)
            {
                if (Vector3.Distance(character.transform.position,transform.position)>0.8f)
                {
                    character = null;
                }

                else if ((selected.Length == 0) & (gameObject.GetComponent<Renderer>().material.color == my_events.turn))
                {
                    if (character.GetComponent<character>().CanWalk)
                    {
                        character.tag = "selected";
                        character.GetComponent<character>().SeeZone();
                    }
                }

                else if (selected.Length == 1)
                {
                    if (selected[0].GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color != character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color)
                    {
                        if (selected[0].GetComponent<character>().availableBlocks.Contains(gameObject))
                        {
                            character.tag = "Destroyed";
                            Destroy(character);
                            character = null;
                            Move(selected[0]);
                        }
                        else { selected[0].GetComponent<character>().RemoveZone(); }
                    }
                    else if ((selected[0].GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color == character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color) & (selected[0].GetComponent<character>().availableBlocks.Contains(gameObject)))
                    {
                        if (selected[0].GetComponent<character>().availableBlocks.Contains(gameObject))
                        {
                            Move(selected[0]);
                        }
                    }
                    else { selected[0].GetComponent<character>().RemoveZone(); }
                }
            }

            GameObject UnitButton = GameObject.FindGameObjectsWithTag("UnitButton")[0];
            GameObject ColonistButton = GameObject.FindGameObjectsWithTag("ColonistButton")[0];
            GameObject buildingButton = GameObject.FindGameObjectsWithTag("BuildingButton")[0];
            if (UnitButton.GetComponent<UnitButtonScript>().availableBlocks.Contains(gameObject) | ColonistButton.GetComponent<ColonistButtonScript>().availableBlocks.Contains(gameObject) | buildingButton.GetComponent<BuildingButtonScript>().availableBlocks.Contains(gameObject))
            {
                GameObject[] unit_buy_me = GameObject.FindGameObjectsWithTag("buy_me");
                GameObject[] colonist_buy_me = GameObject.FindGameObjectsWithTag("colonist_buy_me");
                if (unit_buy_me.Length > 0)
                {
                    Debug.Log("unit_buy_me.Length > 0");

                    if (UnitButton.GetComponent<UnitButtonScript>().availableBlocks.Contains(gameObject))
                    {
                        whoAmI characteristics = unit_buy_me[0].GetComponent<whoAmI>();
                        bool canBuy = (my_events.checkFoodMinus(characteristics.priceFood) & my_events.checkWoodMinus(characteristics.priceWood));
                        if (canBuy)
                        {
                            my_events.minusFood(characteristics.priceFood);
                            my_events.minusWood(characteristics.priceWood);

                            GameObject unitToSpawn = characteristics.Me;

                            Debug.Log(character);
                            Debug.Log(transform.position);
                            Debug.Log(character != null);
                            Debug.Log(gameObject.GetComponent<Renderer>().material.color == my_events.turn);
                            if ((character != null) & (gameObject.GetComponent<Renderer>().material.color == my_events.turn))
                            {
                                Debug.Log("I AM HEREEEEEEEEEEEEEEE");
                                bool CanWalk = character.GetComponent<character>().CanWalk;

                                unitToSpawn = Instantiate(character.GetComponent<character>().upgrade, transform.position, transform.rotation);
                                unitToSpawn.GetComponent<character>().SetColor(my_events.turn);
                                unitToSpawn.GetComponent<character>().block = gameObject;

                                character.tag = "Destroyed";
                                Destroy(character);

                                if (!CanWalk)
                                {
                                    unitToSpawn.GetComponent<character>().SetNotWalk();
                                    unitToSpawn.transform.position = transform.position;
                                }

                                character = unitToSpawn;
                                unitToSpawn.tag = "character";
                                ChangeColor(gameObject.GetComponent<Renderer>().material.color, my_events.turn);
                            } 
                            else
                            {
                                unitToSpawn = Instantiate(unitToSpawn, transform.position, transform.rotation);
                                unitToSpawn.GetComponent<character>().SetColor(my_events.turn);
                                unitToSpawn.GetComponent<character>().block = gameObject;
                                if (character != null)
                                {
                                    Destroy(character);
                                }
                                if ((building != null) & (gameObject.GetComponent<Renderer>().material.color != my_events.turn))
                                {
                                    if (building.tag != "MainHouse")
                                    {
                                        building.GetComponent<BuildingScript>().Weakness();
                                    }
                                }
                                character = unitToSpawn;
                                unitToSpawn.tag = "character";
                                if (gameObject.GetComponent<Renderer>().material.color != my_events.turn)
                                {
                                    character.GetComponent<character>().SetNotWalk();
                                }
                                ChangeColor(gameObject.GetComponent<Renderer>().material.color, my_events.turn);
                            }

                            
                            UnitButton.GetComponent<UnitButtonScript>().Bought();
                        }
                        else { UnitButton.GetComponent<UnitButtonScript>().Bought(); }
                    }
                    else { UnitButton.GetComponent<UnitButtonScript>().Bought(); }
                }

                else if (character == null)
                {
                    if (colonist_buy_me.Length > 0)
                    {
                        if (my_events.turn == gameObject.GetComponent<Renderer>().material.color)
                        {
                            whoAmI characteristics = colonist_buy_me[0].GetComponent<whoAmI>();
                            bool canBuy = (my_events.checkFoodMinus(characteristics.priceFood) & my_events.checkWoodMinus(characteristics.priceWood));
                            if (gameObject.GetComponent<Renderer>().material.color == my_events.turn)
                            {
                                if (canBuy)
                                {
                                    my_events.minusFood(characteristics.priceFood);
                                    my_events.minusWood(characteristics.priceWood);
                                    GameObject colonistToSpawn = characteristics.Me;
                                    colonistToSpawn = Instantiate(colonistToSpawn, transform.position, transform.rotation);
                                    colonistToSpawn.GetComponent<character>().SetColor(my_events.turn);
                                    colonistToSpawn.GetComponent<character>().block = gameObject;
                                    character = colonistToSpawn;
                                    colonistToSpawn.tag = "character";
                                    ColonistButton.GetComponent<ColonistButtonScript>().Bought();
                                }
                                else { ColonistButton.GetComponent<ColonistButtonScript>().Bought(); }
                            }
                            else { ColonistButton.GetComponent<ColonistButtonScript>().Bought(); }
                        }
                        else { ColonistButton.GetComponent<ColonistButtonScript>().Bought(); }
                    }

                    else if (building == null)
                    {
                        GameObject[] building_buy_me = GameObject.FindGameObjectsWithTag("building_buy_me");
                        if (building_buy_me.Length > 0)
                        {
                            if (my_events.turn == gameObject.GetComponent<Renderer>().material.color)
                            {
                                whoAmI characteristics = building_buy_me[0].GetComponent<whoAmI>();
                                bool canBuy = (my_events.checkFoodMinus(characteristics.priceFood) & my_events.checkWoodMinus(characteristics.priceWood));
                                if (gameObject.GetComponent<Renderer>().material.color == my_events.turn)
                                {
                                    if (canBuy)
                                    {
                                        my_events.minusFood(characteristics.priceFood);
                                        my_events.minusWood(characteristics.priceWood);
                                        GameObject buildingToSpawn = characteristics.Me;
                                        buildingToSpawn = Instantiate(buildingToSpawn, transform.position, transform.rotation);
                                        buildingToSpawn.GetComponent<BuildingScript>().SetColor(my_events.turn);
                                        building = buildingToSpawn;
                                        buildingToSpawn.GetComponent<BuildingScript>().block = gameObject;
                                        buildingToSpawn.GetComponent<BuildingScript>().Defend();
                                        buildingButton.GetComponent<BuildingButtonScript>().Bought();
                                    }
                                    else { buildingButton.GetComponent<BuildingButtonScript>().Bought(); }
                                }
                                else { buildingButton.GetComponent<BuildingButtonScript>().Bought(); }
                            }
                            else { buildingButton.GetComponent<BuildingButtonScript>().Bought(); }
                        }
                    }
                }
            }
        }
        return 0;
    }

    void Move(GameObject selected)
    {
        if ( (selected.GetComponent<character>().DestroyAfterStep) & (selected.GetComponent<character>().CanWalk) )
        {
            selected.GetComponent<character>().SetNotWalk();
            selected.GetComponent<character>().block.GetComponent<box>().character = null;
            Color prevCol = gameObject.GetComponent<Renderer>().material.color;
            Color newCol = selected.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color;
            selected.GetComponent<character>().RemoveZone();
            ChangeColor(prevCol, newCol);
            selected.tag = "Destroyed";
            Destroy(selected);
        }
        else if (selected.GetComponent<character>().CanWalk)
        {
            if (character!=null)
            {
                bool CanWalk = character.GetComponent<character>().CanWalk;
                Destroy(character);
                character.tag = "Destroyed";
                character = Instantiate(selected.GetComponent<character>().upgrade, transform.position, transform.rotation);
                character.GetComponent<character>().block = gameObject;
                if (!CanWalk)
                {
                    character.GetComponent<character>().SetNotWalk();
                    character.transform.position = transform.position;
                }
                selected.GetComponent<character>().block.GetComponent<box>().character = null;
                Destroy(selected);
                selected.tag = "Destroyed";
                character.GetComponent<character>().RemoveZone();
                character.GetComponent<character>().SetColor(gameObject.GetComponent<Renderer>().material.color);
                Color prevCol = gameObject.GetComponent<Renderer>().material.color;
                Color newCol = character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color;
                ChangeColor(prevCol, newCol);
            }
            else
            {
                selected.GetComponent<character>().SetNotWalk();
                selected.transform.position = transform.position;
                character = selected;
                character.GetComponent<character>().block.GetComponent<box>().character = null;
                character.GetComponent<character>().block = gameObject;
                character.GetComponent<character>().RemoveZone();
                Color prevCol = gameObject.GetComponent<Renderer>().material.color;
                Color newCol = character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color;
                ChangeColor(prevCol, newCol);
            }
        }
    }

    public void ChangeColor(Color prevCol, Color newCol)
    {
        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
        if (newCol != prevCol)
        {
            gameObject.GetComponent<Renderer>().material.color = newCol;

            if (building != null)
            {
                if (building.tag == "MainHouse")
                {
                    List<GameObject> territory = new List<GameObject>();
                    bool foundBlock = false;

                    GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
                    foreach (GameObject Ibox in boxes)
                    {
                        if (Ibox.gameObject.GetComponent<Renderer>().material.color == prevCol)
                        {
                            territory.Add(Ibox);
                            if (Ibox.GetComponent<box>().building == null)
                            {
                                GameObject newbuilding = Instantiate(building, Ibox.transform.position, Ibox.transform.rotation);
                                Ibox.GetComponent<box>().power = 1;
                                Ibox.GetComponent<box>().building = newbuilding;
                                foundBlock = true;
                                break;
                            }
                        }
                    }

                    if (!foundBlock)
                    {
                        if (territory.Count==0)
                        {
                            my_events.destroy_faction(prevCol);
                        }
                        else
                        {
                            foreach (GameObject Ibox in territory)
                            {
                                Ibox.GetComponent<box>().building.GetComponent<BuildingScript>().Weakness();
                                GameObject newbuilding = Instantiate(building, Ibox.transform.position, Ibox.transform.rotation);
                                Ibox.GetComponent<box>().power = 1;
                                Ibox.GetComponent<box>().building = newbuilding;
                            }
                        }
                    }
                    power = -2;
                    building.tag = "Destroyed";
                    Destroy(building);
                    building = null;

                } else { building.GetComponent<BuildingScript>().Weakness();  }
            }
        }
    }
}
