using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenu = 0, Game = 1, Loading = 2
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene((int)Scene.Loading);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(((int)targetScene));
    }
}
