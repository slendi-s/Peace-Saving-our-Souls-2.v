﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public string SceneName = "MainMenu";
    private void OnMouseUp()
    {
        
        SceneManager.LoadScene(0);
        
    }
}
