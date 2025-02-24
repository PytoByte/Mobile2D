using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorForUnit : MonoBehaviour
{
    public GameObject Parent;
    // Start is called before the first frame update
    public void OnMouseDown()
    {
        Parent.GetComponent<character>().OnMouseDown();
    }
}
