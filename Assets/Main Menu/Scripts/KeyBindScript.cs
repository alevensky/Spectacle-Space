using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeyBindScript : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text up, down, left, right, fire;

    private GameObject currentKey;

    private Color32 normal = new Color32(69, 229, 255, 255);
    private Color32 selected = new Color32(33, 108, 120, 255);

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if ((KeyCode) PlayerPrefs.GetInt("Up") != KeyCode.None)
        {
            keys["Up"] = (KeyCode)PlayerPrefs.GetInt("Up");
        }
        else
        {
            keys.Add("Up", KeyCode.UpArrow);
        }
        if ((KeyCode)PlayerPrefs.GetInt("Down") != KeyCode.None)
        {
            keys["Down"] = (KeyCode)PlayerPrefs.GetInt("Down");
        }
        else
        {
            keys.Add("Down", KeyCode.DownArrow);
        }
        if ((KeyCode)PlayerPrefs.GetInt("Left") != KeyCode.None)
        {
            keys["Left"] = (KeyCode)PlayerPrefs.GetInt("Left");
        }
        else
        {
            keys.Add("Left", KeyCode.LeftArrow);
        }
        if ((KeyCode)PlayerPrefs.GetInt("Right") != KeyCode.None)
        {
            keys["Right"] = (KeyCode)PlayerPrefs.GetInt("Right");
        }
        else
        {
            keys.Add("Right", KeyCode.RightArrow);
        }
        if ((KeyCode)PlayerPrefs.GetInt("Fire") != KeyCode.None)
        {
            keys["Fire"] = (KeyCode)PlayerPrefs.GetInt("Fire");
        }
        else
        {
            keys.Add("Fire", KeyCode.Mouse0);
        }
        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        fire.text = keys["Fire"].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Use these if statements to actually control the player
        //Access them from PlayerController
        /*if (Input.GetKeyDown(keys["Up"]))
        {
            //Do something
        }

        if (Input.GetKeyDown(keys["Down"]))
        {
            //Do something
        }

        if (Input.GetKeyDown(keys["Left"]))
        {
            //Do something
        }

        if (Input.GetKeyDown(keys["Right"]))
        {
            //Do something
        }

        if (Input.GetKeyDown(keys["Fire"]))
        {
            //Do something
        } */
    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
            else if (e.isMouse && e.button == 0)
            {
                keys[currentKey.name] = KeyCode.Mouse0;
                Debug.Log("Detected Key Code: " + keys[currentKey.name]);
                currentKey.transform.GetChild(0).GetComponent<Text>().text = KeyCode.Mouse0.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
            else if (e.isMouse && e.button == 1)
            {
                keys[currentKey.name] = KeyCode.Mouse1;
                Debug.Log("Detected Key Code: " + keys[currentKey.name]);
                currentKey.transform.GetChild(0).GetComponent<Text>().text = KeyCode.Mouse1.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
            else if (e.isMouse && e.button == 2)
            {
                keys[currentKey.name] = KeyCode.Mouse2;
                Debug.Log("Detected Key Code: " + keys[currentKey.name]);
                currentKey.transform.GetChild(0).GetComponent<Text>().text = KeyCode.Mouse2.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
            foreach(string key in keys.Keys)
            {
                int intRepresentation = (int)keys[key];
                PlayerPrefs.SetInt(key, intRepresentation);
            }
            PlayerPrefs.Save();
        }
    }

    public void ChangeKey(GameObject clicked)
    {

        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }

        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    public void ResetKeyBinds ()
    {
        keys["Up"] = KeyCode.UpArrow;
        keys["Down"] = KeyCode.DownArrow;
        keys["Left"] = KeyCode.LeftArrow;
        keys["Right"] = KeyCode.RightArrow;
        keys["Fire"] = KeyCode.Mouse0;

        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        fire.text = keys["Fire"].ToString();

        foreach (string key in keys.Keys)
        {
            int intRepresentation = (int)keys[key];
            PlayerPrefs.SetInt(key, intRepresentation);
        }
        PlayerPrefs.Save();
    }
}
