using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public enum GameMode {

    IDLE,
    PLAY,
    LEVELEND

};


public class MissionDemolition : MonoBehaviour {

    public static MissionDemolition S;      //singleton

    public GameObject[] castles;            //array of the castles

    public Text levelText;                  //text of which level you are on
    public Text scoreText;                  //text of current score

    public Vector3 castlePos;               //position to place each castle

    private int level;                      //current level
    private int numOfLevels;                //total number of levels

    private int shotsTaken;                 //amount of shots taken

    private GameObject castle;              //current castle implemented

    private GameMode mode = GameMode.IDLE;  //current state of the game

    private string showing = "Slingshot";   //CameraFollow mode

    public Button switchViewButton;
    private Text switchViewButtonText;

    void Start() {

        S = this;

        switchViewButtonText = switchViewButton.GetComponentInChildren<Text>();

        level = 0;
        numOfLevels = castles.Length;
        StartLevel();

    }

    void Update() {

        DisplayText();

        if(mode == GameMode.PLAY && Goal.goalMet == true) {
            mode = GameMode.LEVELEND;
            SwitchView("Both");
            Invoke("NextLevel", 2.0f);
        }

    }

    private void StartLevel() {

        if(castle != null) {
            Destroy(castle);
        }

        GameObject[] projectileGOs = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in projectileGOs) {
            Destroy(pTemp);
        }

        castle = Instantiate(castles[level]);
        castle.transform.position = castlePos;

        shotsTaken = 0;

        SwitchView("Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        DisplayText();

        mode = GameMode.PLAY;

    }

    private void NextLevel() {

        level++;
        if(level == numOfLevels) {
            level = 0;
        }
        StartLevel();

    }

    private void DisplayText() {

        levelText.text = "Level: " + (level + 1) + " of " + numOfLevels;
        scoreText.text = "Shots taken: " + shotsTaken;

    }

    private static void SwitchView(string viewToShow) {

        S.showing = viewToShow;

        switch(S.showing) {
            case "Slingshot":
                CameraFollow.S.pointOfInterest = null;
                break;
            case "Castle":
                CameraFollow.S.pointOfInterest = S.castle;
                break;
            case "Both":
                CameraFollow.S.pointOfInterest = GameObject.Find("ViewBoth");
                break;
            
        }

    }

    public static void ShotFired() {
        S.shotsTaken++;
    }

    //Method to handle switch view button
    public void SwitchViewButton() {

        if(showing == "Slingshot") {
            SwitchView("Castle");
            switchViewButtonText.text = "Show Both";
        } else if(showing == "Castle") {
            SwitchView("Both");
            switchViewButtonText.text = "Show Slingshot";
        } else if(showing == "Both") {
            SwitchView("Slingshot");
            switchViewButtonText.text = "Show Castle";
        }
    }

}