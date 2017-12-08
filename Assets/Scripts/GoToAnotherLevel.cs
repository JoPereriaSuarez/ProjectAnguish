using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class GoToAnotherLevel : MonoBehaviour
{
    public string levelName;

    private void Start()
    {
        tag = "NextLevelTrigger";
    }

    public void GoToLevel()
    {
        GameController.instance.player.pauseController = true;
        StartCoroutine(WaitToLoadLevel());
    }

    IEnumerator WaitToLoadLevel()
    {
        yield return new WaitForSecondsRealtime(2F);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
