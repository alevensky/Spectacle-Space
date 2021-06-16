using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GC;
    GameObject[] pauseObjects;
    GameObject[] finishObjects;
    GameObject playerController;
    public static int count;
    public Text countText;

    public static bool isBossTime;
    public string lastBossWas;
    public bool bossSpawned;
    public static bool bossOngoing;

    // Start is called before the first frame update
    void Start()
    {
        count = 950;
        playerController = GameObject.FindWithTag("Player");
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        finishObjects = GameObject.FindGameObjectsWithTag("ShowOnFinish");
        hidePaused();
        hideFinished();
        isBossTime = false;
        bossOngoing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //uses the escape key to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 1)
            {
                pauseControl();
            } else if (Time.timeScale == 0)
            {
                pauseControl();
            }
        }

        if ((count != 0) && ((count % 1000 == 0) || (count % 1010 == 0)) && (lastBossWas != "plane") && (bossOngoing == false))
        {
            isBossTime = true;
            GameController.countGreaterThanThousand = true;
            if (bossSpawned != true)
            {
                GC.GetComponent<GameController>().SpawnBoss("plane");
                bossOngoing = true;
                lastBossWas = "plane";
            }
        }
        else if ((count != 0) && (count % 1000 == 0) && (lastBossWas != "nuke") && (bossOngoing == false))
        {
            isBossTime = true;
            GameController.countGreaterThanTwo = true;
            if (bossSpawned != true)
            {
                GC.GetComponent<GameController>().SpawnBoss("nuke");
                bossOngoing = true;
                lastBossWas = "nuke";
            }
        }

        countText.text = "Score: " + count.ToString();

        //shows finish gameobjects if player is dead and timescale = 0
        if (Time.timeScale == 0 && playerController.GetComponent<PlayerController>().alive == false)
        {
            showFinished();
        }
    }

    //Reloads the level
    public void Reload()
    {
        Destroy(countText.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    //Pauses or unpauses the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        } else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //loads inputted level
    public void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    //shows objects with ShowOnFinish tag
    public void showFinished()
    {
        foreach(GameObject g in finishObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnFinish tag
    public void hideFinished()
    {
        foreach(GameObject g in finishObjects)
        {
            g.SetActive(false);
        }
    }

    public void ClearText()
    {
        Destroy(countText.gameObject);
    }
}
