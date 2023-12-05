using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public void ChangeScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    public void ChangeSceneToStart(string Scene)
    {
        SceneManager.LoadScene(Scene);
        AudioMNG.Instance.ChangeBGM(Resources.Load<AudioClip>("Audio/Start"));
    }
}
