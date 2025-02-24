using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class my_events : MonoBehaviour
{
    public GameObject PauseBackground;

    public TextMeshProUGUI food;
    public TextMeshProUGUI wood;

    public TextMeshProUGUI foodRewardText;
    public TextMeshProUGUI woodRewardText;

    public Color turn;
    public List<Color> queue = new List<Color>();
    List<int> queueFood = new List<int>();
    List<int> queueWood = new List<int>();
    int ind = 0;
    public int startPackFood = 10;
    public int startPackWood = 0;
    public int rewardFoodForBlock = 1;
    public int rewardWoodForBlock = 0;
    public int rewardForForest = 1;
    public int rewardForAnimals = 1;
    public int sanctionForUnit = 1;

    int rewardFood = 0;
    int rewardWood = 0;


    void Start()
    {
        Application.targetFrameRate = 60;
    }


    public float[] get_queue()
    {
        float[] result = new float[(queue.Count * 6)+1];
        for (int i=0; i<queue.Count; i+=1)
        {
            result[i+ (i * 5)] = queue[i].r;
            result[i+ (i * 5) + 1] = queue[i].g;
            result[i+ (i * 5) + 2] = queue[i].b;
            result[i+ (i * 5) + 3] = queue[i].a;
            result[i+ (i * 5) + 4] = (float)queueFood[i];
            result[i+ (i * 5) + 5] = (float)queueWood[i];
        }
        result[queue.Count * 6] = (float)ind;

        return result;
    }


    public void load_game(float[] receivedQueue)
    {
        Debug.Log("Screen size:");
        Debug.Log("==============");
        Debug.Log("width:");
        Debug.Log(Screen.width);
        Debug.Log("height:");
        Debug.Log(Screen.height);
        Debug.Log("==============");

        if (Screen.height >= 1280)
        {
            GameObject.FindGameObjectsWithTag("animation helper")[0].transform.position = new Vector3((Screen.width * 6) / 12, ((Screen.height * 2.5f) / 15) - 1000 * (Screen.height / 1280), 0);
        }
        else
        {
            GameObject.FindGameObjectsWithTag("animation helper")[0].transform.position = new Vector3((Screen.width * 6) / 12, ((Screen.height * 2.5f) / 15) - 1000 * (Screen.height * 0.00078125f), 0);
        }

        PauseBackground.transform.position = new Vector3(PauseBackground.transform.position.x, PauseBackground.transform.position.y + (1000 * Screen.height), PauseBackground.transform.position.z);

        GameObject PauseButton = GameObject.FindGameObjectsWithTag("PauseButton")[0];
        PauseButton.transform.position = new Vector3((Screen.width * 10.5f) / 12, (Screen.height * 14.1f) / 15, PauseButton.transform.position.z);
        GameObject UnitButton = GameObject.FindGameObjectsWithTag("UnitButton")[0];
        UnitButton.transform.position = new Vector3((Screen.width * 7) / 12, Screen.height / 15, UnitButton.transform.position.z);
        GameObject ColonistButton = GameObject.FindGameObjectsWithTag("ColonistButton")[0];
        ColonistButton.transform.position = new Vector3((Screen.width * 4) / 12, Screen.height / 15, ColonistButton.transform.position.z);
        GameObject BuildingButton = GameObject.FindGameObjectsWithTag("BuildingButton")[0];
        BuildingButton.transform.position = new Vector3((Screen.width * 1) / 12, Screen.height / 15, BuildingButton.transform.position.z);
        GameObject NextTurnButton = GameObject.FindGameObjectsWithTag("NextTurnButton")[0];
        NextTurnButton.transform.position = new Vector3((Screen.width * 10.8f) / 12, Screen.height / 15, NextTurnButton.transform.position.z);
        GameObject foodGameO = GameObject.FindGameObjectsWithTag("food")[0];
        foodGameO.transform.position = new Vector3((Screen.width * 2.9f) / 12, (Screen.height * 14.4f) / 15, food.transform.position.z);
        GameObject woodGameO = GameObject.FindGameObjectsWithTag("wood")[0];
        woodGameO.transform.position = new Vector3((Screen.width * 6.1f) / 12, (Screen.height * 14.4f) / 15, food.transform.position.z);

        for (int i = 0; i < receivedQueue.Length-1; i += 6)
        {
            queue.Add( new Color(receivedQueue[i], receivedQueue[i + 1], receivedQueue[i + 2], receivedQueue[i + 3]) );
            queueFood.Add( (int)receivedQueue[i + 4] );
            queueWood.Add( (int)receivedQueue[i + 5] );
        }
        
        ind = (int)receivedQueue[receivedQueue.Length - 1];
        Debug.Log(ind);
        turn = queue[ind];

        food.text = queueFood[ind].ToString();
        wood.text = queueWood[ind].ToString();

        GameObject but = GameObject.FindGameObjectsWithTag("NextTurnButton")[0];
        but.GetComponent<Image>().color = turn;
    }


    public void destroy_faction(Color color)
    {
        int destInt = queue.IndexOf(color, 0);
        queue.RemoveAt(destInt);
        queueFood.RemoveAt(destInt);
        queueWood.RemoveAt(destInt);
    }

    public void start_game(GameObject[] factions, Color col)
    {

        Debug.Log("Screen size:");
        Debug.Log("==============");
        Debug.Log("width:");
        Debug.Log(Screen.width);
        Debug.Log("height:");
        Debug.Log(Screen.height);
        Debug.Log("==============");

        if (Screen.height >= 1280)
        {
            GameObject.FindGameObjectsWithTag("animation helper")[0].transform.position = new Vector3((Screen.width * 6) / 12, ((Screen.height * 2.5f) / 15) - 1000 * (Screen.height / 1280), 0);
        }
        else
        {
            GameObject.FindGameObjectsWithTag("animation helper")[0].transform.position = new Vector3((Screen.width * 6) / 12, ((Screen.height * 2.5f) / 15) - 1000 * (Screen.height * 0.00078125f), 0);
        }

        PauseBackground.transform.position = new Vector3(PauseBackground.transform.position.x, PauseBackground.transform.position.y + (1000 * Screen.height), PauseBackground.transform.position.z);

        GameObject PauseButton = GameObject.FindGameObjectsWithTag("PauseButton")[0];
        PauseButton.transform.position = new Vector3((Screen.width * 10.5f) / 12, (Screen.height * 14.1f) / 15, PauseButton.transform.position.z);
        GameObject UnitButton = GameObject.FindGameObjectsWithTag("UnitButton")[0];
        UnitButton.transform.position = new Vector3((Screen.width * 7) / 12, Screen.height / 15, UnitButton.transform.position.z);
        GameObject ColonistButton = GameObject.FindGameObjectsWithTag("ColonistButton")[0];
        ColonistButton.transform.position = new Vector3((Screen.width * 4) / 12, Screen.height / 15, ColonistButton.transform.position.z);
        GameObject BuildingButton = GameObject.FindGameObjectsWithTag("BuildingButton")[0];
        BuildingButton.transform.position = new Vector3((Screen.width * 1) / 12, Screen.height / 15, BuildingButton.transform.position.z);
        GameObject NextTurnButton = GameObject.FindGameObjectsWithTag("NextTurnButton")[0];
        NextTurnButton.transform.position = new Vector3((Screen.width * 10.8f) / 12, Screen.height / 15, NextTurnButton.transform.position.z);
        GameObject foodGameO = GameObject.FindGameObjectsWithTag("food")[0];
        foodGameO.transform.position = new Vector3((Screen.width * 2.9f) / 12, (Screen.height * 14.4f) / 15, food.transform.position.z);
        GameObject woodGameO = GameObject.FindGameObjectsWithTag("wood")[0];
        woodGameO.transform.position = new Vector3((Screen.width * 6.1f) / 12, (Screen.height * 14.4f) / 15, food.transform.position.z);

        turn = col;
        foreach (GameObject faction in factions)
        {
            queue.Add(faction.GetComponent<Renderer>().material.color);
            queueFood.Add(startPackFood);
            queueWood.Add(startPackWood);
        }
        food.text = startPackFood.ToString();
        wood.text = startPackWood.ToString();

        GameObject but = GameObject.FindGameObjectsWithTag("NextTurnButton")[0];
        but.GetComponent<Image>().color = factions[0].GetComponent<Renderer>().material.color;
        turn = factions[0].GetComponent<Renderer>().material.color;
    }

    public void next_turn()
    {
        // Если осталась одна фракция, выйти в главное меню
        if (queue.Count<=1)
        {
            SceneManager.LoadScene("mainMenu");
        }
        else
        {
            // Установить все магазины в изначальное положение
            GameObject.FindGameObjectsWithTag("UnitButton")[0].GetComponent<UnitButtonScript>().Bought();
            GameObject.FindGameObjectsWithTag("ColonistButton")[0].GetComponent<ColonistButtonScript>().Bought();
            GameObject.FindGameObjectsWithTag("BuildingButton")[0].GetComponent<BuildingButtonScript>().Bought();

            GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
            GameObject[] characters = GameObject.FindGameObjectsWithTag("character");
            GameObject[] buildings = GameObject.FindGameObjectsWithTag("building");
            GameObject[] selected = GameObject.FindGameObjectsWithTag("selected");

            // Снять все выделения
            if (selected.Length > 0)
            {
                selected[0].GetComponent<character>().RemoveZone();
                selected[0].tag = "character";
            }

            // Если фракция не последняя в очереди
            if (ind + 1 < queue.Count)
            {
                queueFood[ind] = Convert.ToInt32(food.text) + rewardFood;
                queueWood[ind] = Convert.ToInt32(wood.text) + rewardWood;

                // Манипуляции с персонажами
                foreach (GameObject character in characters)
                {
                    // Для фракции наступающего хода
                    if (character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color == queue[ind + 1])
                    {
                        // Если не смогли оплатить долг, уничножить армию
                        if (queueFood[ind + 1] < 0)
                        {
                            character.GetComponent<character>().tag = "Destroy";
                            Destroy(character);
                        }
                        else { character.GetComponent<character>().SetWalk(); Debug.Log("Ты ходишь"); }
                    }

                    // Для ходившей фракции, запретить всем юнитам ходить
                    else if (character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color == queue[ind])
                    {
                        character.GetComponent<character>().SetNotWalk();
                        Debug.Log("Ты не ходишь");
                    }
                }

                // Смена цвета, банка валют под другую фракцию
                turn = queue[ind + 1];
                food.text = queueFood[ind + 1].ToString();
                wood.text = queueWood[ind + 1].ToString();
                GameObject but = GameObject.FindGameObjectsWithTag("NextTurnButton")[0];
                but.GetComponent<Image>().color = turn;
                ind += 1;
            }

            else // Если фракция последняя в очереди
            {
                queueFood[ind] = Convert.ToInt32(food.text) + rewardFood;
                queueWood[ind] = Convert.ToInt32(wood.text) + rewardWood;

                foreach (GameObject character in characters)
                {
                    if (character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color == queue[0])
                    {
                        if (queueFood[0] < 0)
                        {
                            character.GetComponent<character>().tag = "Destroy";
                            Destroy(character);
                        }
                        else { character.GetComponent<character>().SetWalk(); Debug.Log("Ты ходишь"); }
                    }

                    else if (character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color == queue[ind])
                    {
                        Debug.Log("Ты не ходишь");
                        Debug.Log(character.transform.position);
                        character.GetComponent<character>().SetNotWalk();
                    }
                }

                ind = 0;
                turn = queue[0];
                food.text = queueFood[ind].ToString();
                wood.text = queueWood[ind].ToString();
                GameObject but = GameObject.FindGameObjectsWithTag("NextTurnButton")[0];
                but.GetComponent<Image>().color = turn;
            }

            foreach (GameObject building in buildings) // Перенастроить защиту зданий
            {
                building.GetComponent<BuildingScript>().DefendAgain();
            }

            foreach (GameObject character in characters) // Для следующей очереди фракции, разрешить ходить юнитам
            {
                if (character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color == queue[ind])
                {
                    character.GetComponent<character>().SetWalk();
                }
            }
        }
    }


    public void plusFood(int count)
    {
        int newValue = queueFood[ind] + count;
        Debug.Log(newValue);
        queueFood[ind] = newValue;
        food.text = newValue.ToString();
    }

    public bool checkFoodMinus(int count)
    {
        int newValue = queueFood[ind] - count;
        if (newValue >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void minusFood(int count)
    {
        int newValue = queueFood[ind] - count;
        if (newValue >= 0)
        {
            queueFood[ind] = newValue;
            food.text = newValue.ToString();
        }
    }

    public void plusWood(int count)
    {
        int newValue = queueWood[ind] + count;
        Debug.Log(newValue);
        queueWood[ind] = newValue;
        wood.text = newValue.ToString();
    }

    public bool checkWoodMinus(int count)
    {
        int newValue = queueWood[ind] - count;
        if (newValue >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void minusWood(int count)
    {
        int newValue = queueWood[ind] - count;
        if (newValue >= 0)
        {
            queueWood[ind] = newValue;
            wood.text = newValue.ToString();
        }
    }


    public void Update()
    {
        rewardWood = 0;
        rewardFood = 0;

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        foreach (GameObject box in boxes)
        {
            if (box.GetComponent<Renderer>().material.color == turn)
            {
                rewardFood += rewardFoodForBlock;
                rewardWood += rewardWoodForBlock;
                if (box.GetComponent<box>().economic_zone == "forest")
                {
                    rewardWood += rewardForForest;
                }
                if (box.GetComponent<box>().economic_zone == "animals")
                {
                    rewardFood += rewardForAnimals;
                }
            }
        }

        GameObject[] characters = GameObject.FindGameObjectsWithTag("character");
        foreach (GameObject character in characters)
        {
            if (character.GetComponent<character>().colorful_details.GetComponent<Renderer>().material.color == turn)
            {
                rewardFood -= sanctionForUnit * character.GetComponent<character>().power + ( 2 * (character.GetComponent<character>().power-1) * sanctionForUnit);
            }
        }

        GameObject[] selected = GameObject.FindGameObjectsWithTag("selected");
        foreach (GameObject select in selected)
        {
            rewardFood -= sanctionForUnit * select.GetComponent<character>().power + (2 * (select.GetComponent<character>().power - 1) * sanctionForUnit);
        }

        if (rewardFood>0)
        {
            foodRewardText.text = "+" + rewardFood.ToString();
        } else
        {
            foodRewardText.text = rewardFood.ToString();
        }

        if (rewardWood > 0)
        {
            woodRewardText.text = "+" + rewardWood.ToString();
        }
        else
        {
            woodRewardText.text = rewardWood.ToString();
        }

        
    }
}
