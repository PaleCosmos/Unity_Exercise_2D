using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSystems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Start");
    }

    // Update is called once per frame
    public void StartButtonPressed()
    {
        Debug.Log("Start Button Pressed");
        SceneManager.LoadScene("Main");
    }
}
