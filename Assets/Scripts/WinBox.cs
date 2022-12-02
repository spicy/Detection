using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("CAR").SendMessage("Finished");   //change it 
    }


}
