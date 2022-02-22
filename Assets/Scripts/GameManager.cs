using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject CameraFront;
    public GameObject CameraTopDown;
    public Light currLight;
    public GameObject PlayerPrefab;
    public Text livesText; //needs UnityEngine.UI

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelGameover;

    //"singleton" used to keep accessing to the game manager easy
    public static GameManager Instance { get; private set; }

    public enum State {  MENU, INIT, PLAY, GAMEOVER}
    private State currState;
    private GameObject currPlayer;

    private int currLives;
    public int Lives
    {
        get { return currLives;}
        set { currLives = value;
            livesText.text = "Lives: " + currLives;
        }
    }

    public void PlayButtonClicked()
    {
        SwitchState(State.INIT);
    }

    public void SpawnerButtonClicked()
    {
        //Spawner button only destroys current player
        //and decrement lives as the update() function
        //does the hard work of checking if the currplayer is referencing null
        //this means that i dont have to actually spawn the new character here
        //in the spawner button function after destroying the old one
        //instead i just let the update function do it
        if (currPlayer != null)
        {
            Destroy(currPlayer);
            Lives--;
        }
    }

    public void RestartButtonClicked()
    {
        SwitchState(State.MENU);
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CameraFront.SetActive(true);
        CameraTopDown.SetActive(false);
        SwitchState(State.MENU);
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                if(currPlayer != null)
                {
                    Destroy(currPlayer);
                }
                panelMenu.SetActive(true);
                panelPlay.SetActive(false);
                panelGameover.SetActive(false);
                break;

            case State.INIT:
                panelPlay.SetActive(true);
                Lives = 3;
                //Instantiate(PlayerPrefab);
                SwitchState(State.PLAY);
                break;

            case State.PLAY:
                break;

            case State.GAMEOVER:
                panelGameover.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //press 'c' to swap cameras so players get more views
        //of the game terrain
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }

        //press 'L' to turn switch light on and off
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchLight();
        }

        switch (currState)
        {
            case State.MENU:
                break;

            case State.INIT:
                break;

            case State.PLAY:

                if (currPlayer == null)
                {
                    //if we got lives, spawn a player if no player
                    if (Lives > 0)
                    {
                        currPlayer = Instantiate(PlayerPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }
                break;

            case State.GAMEOVER:
                break;
        }
    }

    void EndState()
    {
        switch (currState)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;

            case State.INIT:
                break;

            case State.PLAY:
                break;

            case State.GAMEOVER:
                panelMenu.SetActive(false);
                panelGameover.SetActive(false);
                break;
        }
    }

    void SwitchCamera()
    {
        if (CameraFront.activeSelf)
        {
            CameraFront.SetActive(false);
            CameraTopDown.SetActive(true);
        }
        else
        {
            CameraFront.SetActive(true);
            CameraTopDown.SetActive(false);
        }
    }

    void SwitchLight()
    {
        if (currLight.isActiveAndEnabled){
            currLight.enabled = false;
        }
        else
        {
            currLight.enabled = true;
        }
    }

    public void SwitchState(State newState)
    {
        EndState();
        currState = newState;
        BeginState(newState);
    }
}
