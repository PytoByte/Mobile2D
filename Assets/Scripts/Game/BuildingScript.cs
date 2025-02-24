using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> DefendBlocks = new List<GameObject>();
    public int id;
    public GameObject block;
    public GameObject colorful_details;
    public int power;

    public void SetColor(Color colorToSet)
    {
        colorful_details = Instantiate(colorful_details, transform.position, transform.rotation);
        colorful_details.transform.parent = gameObject.transform;
        colorful_details.GetComponent<Renderer>().material.color = colorToSet;
    }

    public void Defend()
    {
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        foreach (GameObject box in boxes)
        {
            if (Vector3.Distance(box.transform.position, gameObject.transform.position) < 0.8f)
            {
                if ( (box.GetComponent<box>().power<=power) & (box.GetComponent<Renderer>().material.color == colorful_details.GetComponent<Renderer>().material.color) )
                {
                    box.GetComponent<box>().power = power;
                }
                DefendBlocks.Add(box);
            }
        }
    }

    public void DefendAgain()
    {
        foreach (GameObject box in DefendBlocks)
        {
            if ((box.GetComponent<box>().power <= power) & (box.GetComponent<Renderer>().material.color == colorful_details.GetComponent<Renderer>().material.color))
            {
                box.GetComponent<box>().power = power;
            }
        }
    }

    public void Weakness()
    {
        gameObject.tag = "Destroyed";
        foreach (GameObject box in DefendBlocks)
        {
            box.GetComponent<box>().power = -2;
        }
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("building");
        foreach (GameObject building in buildings)
        {
            building.GetComponent<BuildingScript>().DefendAgain();
        }
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        block.GetComponent<box>().OnMouseDown();
    }
}
