using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Random = System.Random;

[System.Serializable]
public class GameData
{
    public float[] position = new float[3];
    public float[] col = new float[4];
    public int buildingID;
    public int characterID;
    public bool characterCanWalk;
    public string economicZone;

    public void setValue(GameData gameData)
    {
        position = gameData.position;
        col = gameData.col;
        buildingID = gameData.buildingID;
        characterID = gameData.characterID;
        economicZone = gameData.economicZone;
    }
}

[System.Serializable]
public class QueueData
{
    public float[] queue;
    public float sizeWidth;
    public float sizeHeight;

    public void setValue(QueueData queueData)
    {
        sizeWidth = queueData.sizeWidth;
        sizeHeight = queueData.sizeHeight;
        queue = queueData.queue;
    }
}


public class GameDataManager : MonoBehaviour
{
    // Create a field for the save file.
    public string saveFile;

    public string[] readFile()
    {
        saveFile = Application.persistentDataPath + "/gamedata.txt";
        Debug.Log(saveFile);
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            string[] fileJson = fileContents.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.


            Debug.Log(fileJson.Length);
            return fileJson;
        }

        return null;
    }

    public void writeFile(float sizeWidth, float sizeHeight)
    {

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        string toSave = null;
        for (int i = 0; i < boxes.Length; i++)
        {
            GameData gameData = new GameData();
            gameData.position = new float[] { boxes[i].transform.position.x, boxes[i].transform.position.y, boxes[i].transform.position.z };
            gameData.col = new float[] { boxes[i].GetComponent<Renderer>().material.color.r, boxes[i].GetComponent<Renderer>().material.color.g, boxes[i].GetComponent<Renderer>().material.color.b, boxes[i].GetComponent<Renderer>().material.color.a };

            if (boxes[i].GetComponent<box>().building != null) 
            { 
                gameData.buildingID = boxes[i].GetComponent<box>().building.GetComponent<BuildingScript>().id; 
            }

            else if (boxes[i].GetComponent<box>().character != null) 
            { 
                gameData.characterID = boxes[i].GetComponent<box>().character.GetComponent<character>().id;
                gameData.characterCanWalk = boxes[i].GetComponent<box>().character.GetComponent<character>().CanWalk;
            }

            gameData.economicZone = boxes[i].GetComponent<box>().economic_zone;
            toSave += JsonUtility.ToJson(gameData) + "|";
        }
        QueueData queueData = new QueueData();

        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
        float[] queue = my_events.get_queue();

        queueData.queue = queue;
        queueData.sizeWidth = sizeWidth;
        queueData.sizeHeight = sizeHeight;
        toSave += JsonUtility.ToJson(queueData);
        saveFile = Application.persistentDataPath + "/gamedata.txt";
        Debug.Log(saveFile);
        // Serialize the object into JSON and save string.

        // Write JSON to file.
        File.WriteAllText(saveFile, toSave);
    }
}


public class levelGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    Random rnd = new Random();
    float distance = 0.5f;
    public int factionsCount = 2;
    public float minDistanceBetweenFactions = 3;
    public float sizeHeight = 10.0f;
    public float sizeWidth = 10.0f;
    public GameObject box;
    public GameObject gap;
    public GameObject mainHouse;
    public GameObject forest;
    public GameObject animals;
    int randInt;
    public bool camSet = true;
    public bool loadLevel;
    public GameObject[] unitList;
    public GameObject[] buildingList;
    Color newColor;


    void CreateObject(Vector3 pos)
    {
        randInt = rnd.Next(0, 2);

        if (randInt == 1)
        {
            Instantiate(box, pos, transform.rotation);
        }

        else
        {
            Instantiate(gap, pos, transform.rotation);
        }
    }

    public void LoadLevel()
    {
        GameDataManager gameDataManager = new GameDataManager();

        string[] fileJson = gameDataManager.readFile();

        QueueData ResultQueue = new QueueData();
        ResultQueue.setValue(JsonUtility.FromJson<QueueData>(fileJson[fileJson.Length - 1]));

        for (int i = 0; i < fileJson.Length - 1; i++)
        {
            GameData gameData = JsonUtility.FromJson<GameData>(fileJson[i]);

            GameObject oneOfBox = Instantiate(box, new Vector3(gameData.position[0], gameData.position[1], gameData.position[2]), box.transform.rotation);
            oneOfBox.GetComponent<Renderer>().material.color = new Color(gameData.col[0], gameData.col[1], gameData.col[2], gameData.col[3]);
            
            
            if (gameData.economicZone != "")
            {
                oneOfBox.GetComponent<box>().economic_zone = gameData.economicZone;
                if (gameData.economicZone == "forest") 
                {
                    Instantiate(forest, new Vector3(gameData.position[0], gameData.position[1], gameData.position[2]), box.transform.rotation);
                }
                else if (gameData.economicZone == "animals")
                {
                    Instantiate(animals, new Vector3(gameData.position[0], gameData.position[1], gameData.position[2]), box.transform.rotation);
                }
            }

            if (gameData.characterID != 0)
            {
                foreach (GameObject character in unitList)
                {
                    if (character.GetComponent<character>().id == gameData.characterID)
                    {
                        GameObject spawnCharacter = Instantiate(character, new Vector3(gameData.position[0], gameData.position[1], gameData.position[2]), box.transform.rotation);
                        spawnCharacter.GetComponent<character>().SetColor(new Color(gameData.col[0], gameData.col[1], gameData.col[2], gameData.col[3]));
                        spawnCharacter.GetComponent<character>().block = oneOfBox;
                        if (!gameData.characterCanWalk)
                        {
                            spawnCharacter.GetComponent<character>().SetNotWalk();
                        }
                        break;
                    }
                }
            }
            else if (gameData.buildingID != 0)
            {
                foreach (GameObject building in buildingList)
                {
                    if (building.GetComponent<BuildingScript>().id == gameData.buildingID)
                    {
                        GameObject spawnBuilding = Instantiate(building, new Vector3(gameData.position[0], gameData.position[1], gameData.position[2]), box.transform.rotation);
                        spawnBuilding.GetComponent<BuildingScript>().SetColor(new Color(gameData.col[0], gameData.col[1], gameData.col[2], gameData.col[3]));
                        spawnBuilding.GetComponent<BuildingScript>().block = oneOfBox;
                        break;
                    }
                }
            }
        }

        sizeWidth = ResultQueue.sizeWidth;
        sizeHeight = ResultQueue.sizeHeight;

        float sizeWidthTemp = sizeWidth * distance;
        float sizeHeightTemp = sizeHeight * distance;

        Camera.main.GetComponent<CamEvents>().sizeWidthTemp = sizeWidthTemp;
        Camera.main.GetComponent<CamEvents>().sizeHeightTemp = sizeHeightTemp;

        float max = Math.Max(sizeWidth, sizeHeight);
        Camera.main.transform.position = new Vector3(sizeWidthTemp / 2, sizeHeightTemp / 2, -10);
        if (camSet)
        {
            Camera.main.GetComponent<Camera>().orthographicSize = max * (0.33f - (max / 10 * 0.006f));
            Camera.main.GetComponent<CamEvents>().zoomMax = max * (0.33f - (max / 10 * 0.006f));
        }

        my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
        my_events.load_game(ResultQueue.queue);
    }


    public void SaveLevel()
    {
        GameDataManager gameDataManager = new GameDataManager();
        gameDataManager.writeFile(sizeWidth, sizeHeight);
        Debug.Log("save ready");
    }


    void Start()
    {
        loadLevel = TempClass.get_loadLevel();
        Debug.Log(loadLevel);

        if (loadLevel)
        {
            LoadLevel();
        }
        else
        {
            sizeWidth = TempClass.get_width();
            sizeHeight = TempClass.get_height();
            factionsCount = TempClass.get_factionCount();

            float sizeWidthTemp = sizeWidth * distance;
            float sizeHeightTemp = sizeHeight * distance;

            Camera.main.GetComponent<CamEvents>().sizeWidthTemp = sizeWidthTemp;
            Camera.main.GetComponent<CamEvents>().sizeHeightTemp = sizeHeightTemp;

            float max = Math.Max(sizeWidth, sizeHeight);
            Camera.main.transform.position = new Vector3(sizeWidthTemp / 2, sizeHeightTemp / 2, -10);
            if (camSet)
            {
                Camera.main.GetComponent<Camera>().orthographicSize = max * (0.33f - (max / 10 * 0.006f));
                Camera.main.GetComponent<CamEvents>().zoomMax = max * (0.33f - (max / 10 * 0.006f));
            }

            while (sizeHeightTemp != 0 - distance)
            {
                while (sizeWidthTemp != 0 - distance)
                {
                    CreateObject(new Vector3(sizeWidthTemp, sizeHeightTemp, 0));
                    sizeWidthTemp -= distance;
                }
                sizeHeightTemp -= distance;
                sizeWidthTemp = sizeWidth * distance;
            }


            GameObject[] gaps = GameObject.FindGameObjectsWithTag("gap");
            for (int i = 0; i < gaps.Length; i++)
            {
                gaps[i].GetComponent<gap>().Correct(distance);
            }

            GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].GetComponent<box>().Correct(distance);
            }



            boxes = GameObject.FindGameObjectsWithTag("box");
            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i].tag == "box")
                {
                    boxes[i].GetComponent<box>().Correct(distance);
                }
            }


            boxes = GameObject.FindGameObjectsWithTag("box");

            GameObject[] factions = new GameObject[factionsCount];
            int toMany = 0;

            List<GameObject> prohibitedBlocks = new List<GameObject>();
            for (int i = 0; i < factionsCount; i++)
            {
                randInt = rnd.Next(0, boxes.Length);

                newColor = new Color(rnd.Next(0, 11) / 10f, rnd.Next(0, 11) / 10f, rnd.Next(0, 11) / 10f);

                bool allNotGood = false;
                foreach (GameObject factionBlock in factions)
                {
                    Debug.Log(factionBlock);
                    if (factionBlock == null)
                    {
                        break;
                    }

                    else if (prohibitedBlocks.Contains(boxes[randInt]))
                    {
                        allNotGood = true;
                        i--;
                        toMany++;
                        break;
                    }

                    else if (((toMany < 100) | (boxes[randInt] == factionBlock)) & ((newColor == new Color(1, 1, 1)) | (newColor == factionBlock.GetComponent<Renderer>().material.color) | (boxes[randInt].transform.position == factionBlock.transform.position) | (Vector3.Distance(boxes[randInt].transform.position, factionBlock.transform.position) < minDistanceBetweenFactions)))
                    {
                        Debug.Log(newColor);
                        Debug.Log(boxes[i]);
                        Debug.Log(Vector3.Distance(boxes[randInt].transform.position, factionBlock.transform.position));

                        Debug.Log(factionBlock.GetComponent<Renderer>().material.color);
                        Debug.Log(factionBlock);

                        allNotGood = true;
                        i--;
                        toMany++;
                        prohibitedBlocks.Add(boxes[randInt]);
                        break;
                    }
                }

                if (allNotGood)
                {
                    continue;
                }
                else { factions[i] = boxes[randInt]; prohibitedBlocks.Clear(); }

                boxes[randInt].GetComponent<Renderer>().material.color = newColor;

                GameObject house = Instantiate(mainHouse, boxes[randInt].transform.position, boxes[randInt].transform.rotation);
                boxes[randInt].GetComponent<box>().building = house;
                boxes[randInt].GetComponent<box>().power = 1;
                house.GetComponent<BuildingScript>().SetColor(newColor);
                toMany = 0;
                int blocks = 1;
                foreach (GameObject box in boxes)
                {
                    if ((Vector3.Distance(box.transform.position, boxes[randInt].transform.position) < 0.8f) & (box != boxes[randInt]))
                    {
                        box.GetComponent<Renderer>().material.color = newColor;
                        blocks += 1;
                        Debug.Log(box.transform.position);
                    }
                }

                if (blocks < 9)
                {
                    foreach (GameObject box1 in boxes)
                    {
                        foreach (GameObject box2 in boxes)
                        {
                            if ((Vector3.Distance(box1.transform.position, box2.transform.position) < 0.8f) & (box2.GetComponent<Renderer>().material.color == newColor) & (box1.GetComponent<Renderer>().material.color != newColor))
                            {
                                box1.GetComponent<Renderer>().material.color = newColor;
                                blocks += 1;
                                Debug.Log(box1.transform.position);
                                break;
                            }
                        }
                        if (blocks >= 9)
                        {
                            break;
                        }
                    }
                }
            }

            boxes = GameObject.FindGameObjectsWithTag("box");
            foreach (GameObject box in boxes)
            {
                if (box.GetComponent<box>().building == null)
                {
                    bool availableToZone = false;
                    foreach (GameObject faction in factions)
                    {
                        if ((Vector3.Distance(box.transform.position, faction.transform.position) > 1) & (box.GetComponent<Renderer>().material.color == new Color(1, 1, 1, 1)))
                        {
                            availableToZone = true;
                        }
                        else
                        {
                            availableToZone = false;
                            break;
                        }
                    }
                    if (availableToZone)
                    {
                        int chance = rnd.Next(1, 101);
                        if (chance >= 100)
                        {
                            box.GetComponent<box>().economic_zone = "animals";
                            Instantiate(animals, box.transform.position, box.transform.rotation).transform.parent = box.transform;
                        }
                        else if (chance >= 92)
                        {
                            box.GetComponent<box>().economic_zone = "forest";
                            Instantiate(forest, box.transform.position, box.transform.rotation).transform.parent = box.transform;
                        }
                    }
                }
            }

            my_events my_events = GameObject.FindGameObjectsWithTag("my_events")[0].GetComponent<my_events>();
            my_events.start_game(factions, newColor);
        }
    }
}