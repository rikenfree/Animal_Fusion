using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    public string firstChoice;
    public string secondChoice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


}
