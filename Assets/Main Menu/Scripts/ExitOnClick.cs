using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitOnClick : MonoBehaviour
{
    //Make the exit button exit the game when clicked
    public void ExitGame()
    {
        Application.Quit();
    }
}
