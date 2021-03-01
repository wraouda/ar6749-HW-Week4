using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int score = 0;

    private const string FILE_DEATHS = "/Logs/totalDeaths.txt";

    string FILE_PATH_TOTAL_DEATHS; 
    
    const string PREF_KEY_TOTAL_DEATHS = "TotalDeathsKey";

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public int targetscore = 3;

    public Text text;

    public int currentLevel = 0;

    private int totalDeaths = -1;

    private int deaths = 0;

    public int Deaths
    {
        get { return deaths; }
        set
        {
            deaths = value;
            if (deaths > 0)
            {
                TotalDeaths ++;
            }
        }
        
    }

    public int lives = 1;

    public int TotalDeaths
    {
        get
        {
            if (totalDeaths < 0)
            {
                //totalDeaths = PlayerPrefs.GetInt(PREF_KEY_TOTAL_DEATHS, 0);
                string fileContents = File.ReadAllText(FILE_PATH_TOTAL_DEATHS);
                totalDeaths = Int32.Parse(fileContents);

            }   
            return totalDeaths;
        }
        set
        {
            totalDeaths = value;
            //PlayerPrefs.SetInt(PREF_KEY_TOTAL_DEATHS, totalDeaths);
            File.WriteAllText(FILE_PATH_TOTAL_DEATHS, totalDeaths + "");
        }
    }

    private List<int> totalDeathsList;

    private bool isGame = true;
    

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FILE_PATH_TOTAL_DEATHS = Application.dataPath + FILE_DEATHS;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Level: " + currentLevel +
                    "\nScore: " + Score +
                    "\nTarget: " + targetscore +
                    "\nTotal deaths: " + TotalDeaths +
                    "\nDeaths: " + Deaths +
                    "\n Lives: " + lives;
        
        if (score == targetscore) // activates the yellow prize function
        {
            currentLevel++; // increase the level
            GetComponent<AudioSource>().Play(); // play cheer sound
            targetscore += targetscore + targetscore/2; // increase target score
        }

        if (!isGame) // if we're not in the game, show death counts
        {
            string DeathString = "Total Deaths \n\n"; // Show the text

            for (var i = 0; i < totalDeathsList.Count; i++) // create the death for loop 
            {
                DeathString += totalDeathsList[i] + "\n"; // display the list of deaths
            }

            text.text = DeathString; // replaces the previous text with the DeathString text
        }

        if (lives == 0)
        {
            SceneManager.LoadScene(3);
            isGame = false;
            lives = 1; // this is used so that the game doesn't constantly keep sending you to this scene
            UpdateDeathScores();
        }
    }

    void UpdateDeathScores()
    {
        if (totalDeathsList == null) // if there are no deaths yet
        {
            totalDeathsList = new List<int>(); // create the list
            
            string fileContents = File.ReadAllText(FILE_PATH_TOTAL_DEATHS); // find the file
            
            string[] fileDeaths = fileContents.Split(','); // add a comma between each number

            print(fileDeaths.Length); // display length of the file in the console

            for (var i = 0; i < fileContents.Length - 1; i++)
            {
                totalDeathsList.Add(Int32.Parse(fileDeaths[i])); // add the death list total
                
            }

        }
    }
}
