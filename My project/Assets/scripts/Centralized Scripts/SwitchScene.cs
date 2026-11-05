using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void Switch(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void onPress()
    {
        Switch(sceneIndex);
    }
}
