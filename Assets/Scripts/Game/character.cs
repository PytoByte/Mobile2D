using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    public int id; // id персонажа
    public GameObject upgrade; // кем станет персонаж при улучшении
    public GameObject block;
    public GameObject zone;
    public GameObject colorful_details; // цветная часть персонажа
    public float movementDistanceInColor = 2.5f; // Дистанция перемещения внутри блоков фракции
    public float movementDistanceOutColor = 0.8f; // Дистанция перемещения вне блоков фракции
    public int power = 1;
    public bool DestroyAfterStep = false;
    public bool CanTakeEnemyTerritory = true;
    public bool CanKillSamePower = false;
    public bool highestPower = false; // Юнит является обладателем высшей силы из всех
    public List<GameObject> availableBlocks = new List<GameObject>(); // Куда юнит может походить
    public bool CanWalk = true; // Может ли ходить
    public Vector3 startPos; // Начальная позиция анимации
    int frames = 56; // Количество кадров в анимации
    float deltapos = 0.08f; // Смещение от начальной позиции при анимации
    int nowFrame = 0; // Проигрываемый кадр анимации
    List<GameObject> zoneBlocks = new List<GameObject>(); // Куда будут установленны блоки зоны

    void Start()
    {
        startPos = transform.position;
    }

    public void SetColor(Color colorToSet) // Установка цветных деталей
    {
        colorful_details = Instantiate(colorful_details, transform.position, transform.rotation);
        colorful_details.transform.parent = gameObject.transform;
        colorful_details.GetComponent<colorForUnit>().Parent = gameObject;
        colorful_details.GetComponent<Renderer>().material.color = colorToSet;
    }

    public void SetNotWalk() // Персонаж походил
    {
        transform.position = startPos;
        CanWalk = false;
        transform.position = block.transform.position;
    }

    public void SetWalk() // Персонаж снова может ходить
    {
        startPos = transform.position;
        CanWalk = true;
    }

    public void SeeZone()
    {
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        for (int i = 0; i < boxes.Length; i++)
        {
            if (block == boxes[i])
            {
                availableBlocks.Add(boxes[i]);
            }

            else if (!CanTakeEnemyTerritory)
            {
                if (Vector3.Distance(boxes[i].transform.position, gameObject.transform.position) > movementDistanceInColor)
                {
                    zoneBlocks.Add(boxes[i]);
                }

                else if (colorful_details.GetComponent<Renderer>().material.color != boxes[i].GetComponent<Renderer>().material.color)
                {
                    if (boxes[i].GetComponent<Renderer>().material.color == new Color(1, 1, 1, 1))
                    {
                        if (boxes[i].GetComponent<box>().character != null)
                        {
                            if ((power <= boxes[i].GetComponent<box>().character.GetComponent<character>().power) & (!highestPower) & !(CanKillSamePower & (power == boxes[i].GetComponent<box>().character.GetComponent<character>().power)))
                            {
                                zoneBlocks.Add(boxes[i]);
                            }

                            else
                            {
                                bool placeZone = true;
                                for (int j = 0; j < boxes.Length; j++)
                                {
                                    if ((colorful_details.GetComponent<Renderer>().material.color == boxes[j].GetComponent<Renderer>().material.color) & (Vector3.Distance(boxes[j].transform.position, boxes[i].transform.position) < movementDistanceOutColor))
                                    {
                                        placeZone = false;
                                        break;
                                    }
                                }
                                if (placeZone) { zoneBlocks.Add(boxes[i]); } else { availableBlocks.Add(boxes[i]); }
                            }
                        }
                        /*if (boxes[i].box.building != null)
                        {

                        }*/
                        else
                        {
                            bool placeZone = true;
                            for (int j = 0; j < boxes.Length; j++)
                            {
                                if ((colorful_details.GetComponent<Renderer>().material.color == boxes[j].GetComponent<Renderer>().material.color) & (Vector3.Distance(boxes[j].transform.position, boxes[i].transform.position) < movementDistanceOutColor))
                                {
                                    placeZone = false;
                                    break;
                                }
                            }
                            if (placeZone) { zoneBlocks.Add(boxes[i]); } else { availableBlocks.Add(boxes[i]); }
                        }
                    } else { zoneBlocks.Add(boxes[i]); }
                } else { availableBlocks.Add(boxes[i]); }
            }


            // Power > -1
            else if ((colorful_details.GetComponent<Renderer>().material.color == boxes[i].GetComponent<Renderer>().material.color) & (Vector3.Distance(boxes[i].transform.position, gameObject.transform.position) > movementDistanceInColor))
            {
                zoneBlocks.Add(boxes[i]);
            }

            else if ((boxes[i].GetComponent<box>().character!=null) & (colorful_details.GetComponent<Renderer>().material.color == boxes[i].GetComponent<Renderer>().material.color))
            {
                if ((boxes[i].GetComponent<box>().character.GetComponent<character>().id == id) & (!highestPower))
                {
                    availableBlocks.Add(boxes[i]);
                }

                else
                {
                    zoneBlocks.Add(boxes[i]);
                }
            }
            


            else if (colorful_details.GetComponent<Renderer>().material.color != boxes[i].GetComponent<Renderer>().material.color)
            {
                if (boxes[i].GetComponent<box>().character != null)
                {
                    if ( (power <= boxes[i].GetComponent<box>().character.GetComponent<character>().power) & (!highestPower) & !(CanKillSamePower & (power == boxes[i].GetComponent<box>().character.GetComponent<character>().power)))
                    {
                        zoneBlocks.Add(boxes[i]);
                    } 
                    
                    else
                    {
                        bool placeZone = true;
                        for (int j = 0; j < boxes.Length; j++)
                        {
                            if ((colorful_details.GetComponent<Renderer>().material.color == boxes[j].GetComponent<Renderer>().material.color) & (Vector3.Distance(boxes[j].transform.position, boxes[i].transform.position) < movementDistanceOutColor))
                            {
                                placeZone = false;
                                break;
                            }
                        }
                        if (placeZone) { zoneBlocks.Add(boxes[i]); } else { availableBlocks.Add(boxes[i]); }
                    }
                }

                else if ( (!highestPower) & (power <= boxes[i].GetComponent<box>().power) & !(CanKillSamePower & (power == boxes[i].GetComponent<box>().power)))
                {
                    zoneBlocks.Add(boxes[i]);
                }

                else
                {
                    bool placeZone = true;
                    for (int j = 0; j < boxes.Length; j++)
                    {
                        if ((colorful_details.GetComponent<Renderer>().material.color == boxes[j].GetComponent<Renderer>().material.color) & (Vector3.Distance(boxes[j].transform.position, boxes[i].transform.position) < movementDistanceOutColor))
                        {
                            placeZone = false;
                            break;
                        }
                    }
                    if (placeZone) { zoneBlocks.Add(boxes[i]); } else { availableBlocks.Add(boxes[i]); }
                }
            } else { availableBlocks.Add(boxes[i]); }
        }

        for (int i = 0; i < zoneBlocks.Count; i++)
        {
            Instantiate(zone, zoneBlocks[i].transform.position, zoneBlocks[i].transform.rotation);
        }
        zoneBlocks.Clear();
    }


    public void RemoveZone()
    {
        GameObject[] zoneBlocks = GameObject.FindGameObjectsWithTag("zone");
        for (int i = 0; i < zoneBlocks.Length; i++)
        {
            gameObject.tag = "character";
            Destroy(zoneBlocks[i]);
            availableBlocks.Clear();
        }
    }


    public void OnMouseDown()
    {
        if (block==null)
        {
            GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
            foreach (GameObject box in boxes)
            {
                if (box.transform.position == transform.position)
                {
                    block = box;
                    box.GetComponent<box>().character = gameObject;
                    break;
                }
            }
        }
        else if (block.GetComponent<box>().character==null)
        {
            block.GetComponent<box>().character = gameObject;
        }

        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
        GameObject[] unit_buy_me = GameObject.FindGameObjectsWithTag("buy_me");
        /*if (unit_buy_me.Length > 0)
        {
            unit_buy_me[0].transform.position = new Vector3(unit_buy_me[0].transform.position.x, -550, unit_buy_me[0].transform.position.z);
            unit_buy_me[0].tag = "unitToBuy";
            RemoveZone();
        }*/

        block.GetComponent<box>().OnMouseDown();
    }

    
    void Update() // Анимация доступности ходьбы
    {
        if (CanWalk)
        {
            if (nowFrame == frames)
            {
                nowFrame = 0;
            }
            else  if (nowFrame <= frames / 2)
            {
                float y = startPos.y + deltapos * (nowFrame / (frames * 0.5f));
                transform.position = new Vector3(startPos.x, y, startPos.z);
            } else
            {
                float y = startPos.y + deltapos - deltapos * ((nowFrame - frames * 0.5f) / (frames * 0.5f));
                transform.position = new Vector3(startPos.x, y, startPos.z);
            }
            nowFrame += 1;
        }
    }
}
