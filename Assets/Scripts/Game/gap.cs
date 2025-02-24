using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gap : MonoBehaviour
{
    public GameObject box;

    public void Correct(float distance)
    {
        float spawn = 0;

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        for (int i = 0; i < boxes.Length; i++)
        {
            if (Vector3.Distance(boxes[i].transform.position, transform.position) == distance)
            {
                spawn++;
                if (spawn >= 3)
                {
                    Instantiate(box, transform.position, transform.rotation);
                    break;
                }
            }
        }
        Destroy(gameObject);
    }
}
