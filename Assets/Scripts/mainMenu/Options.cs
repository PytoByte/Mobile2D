using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    // Update is called once per frame
    public string saveFile;

    void Start()
    {
        Application.targetFrameRate = 30;
        saveFile = Application.persistentDataPath + "/gamedata.txt";

        if (!File.Exists(saveFile))
        {
            GameObject.Find("LOAD").GetComponent<Button>().interactable = false;
        } else
        {
            GameObject.Find("LOAD").GetComponent<Button>().interactable = true;
        }
    }

    public TextMeshProUGUI width_text;
    public TextMeshProUGUI height_text;
    public TextMeshProUGUI factionCount_text;

    public float value_width;
    public double convert_value_width;

    public float value_height;
    public double convert_value_height;

    public float value_factionCount;
    public double MaxFactionCount;
    public double convert_factionCount;

    public bool loadLevel = false;

    public bool contUpdate = true;

    void Update()
    {
        if (contUpdate)
        {
            //width
            value_width = GameObject.Find("WIDTHLEVEL").GetComponent<Slider>().value;
            convert_value_width = 15 + (Math.Round(35 * value_width, 0));
            width_text.text = "Ширина уровня: " + (convert_value_width * 2).ToString();

            //height
            value_height = GameObject.Find("HEIGHTLEVEL").GetComponent<Slider>().value;
            convert_value_height = 15 + (Math.Round(35 * value_height, 0));
            height_text.text = "Высота уровня: " + (convert_value_height * 2).ToString();

            //faction
            value_factionCount = GameObject.Find("FACTIONSCOUNT").GetComponent<Slider>().value;
            MaxFactionCount = 2 + Math.Round((convert_value_width * convert_value_height) / 300);
            convert_factionCount = 2 + (Math.Round((MaxFactionCount - 2) * value_factionCount, 0));
            factionCount_text.text = "Количество фракций: " + convert_factionCount.ToString();

            //write in tempclass
            TempClass.set((float)convert_value_width, (float)convert_value_height, Convert.ToInt32(convert_factionCount), loadLevel);
        } else
        {
            TempClass.set((float)convert_value_width, (float)convert_value_height, Convert.ToInt32(convert_factionCount), true);
            SceneManager.LoadScene("Game");
        }
    }

    public void ButtonEvent()
    {
        SceneManager.LoadScene("Game");
    }

    public void ButtonLoadEvent()
    {
        contUpdate = false;
    }
}
