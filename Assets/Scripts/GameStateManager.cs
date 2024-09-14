using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int turn = 0;
    public int playerScore = 0;
    public int nextGoodieBagScore = 2;

    private CameraController _cameraController;

    void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();

        _cameraController.DebugSomeShit();
    }

    void Update()
    {

    }
    
    public void EndTurn()
    {
        turn++;
        updateScore();
        levelUpTiles();
        tryGiveGoodieBag();
        Debug.Log("End of turn " + turn);
    }
    
    public void updateScore()
    {
        playerScore += 1;
        //TODO Implement

        //On Z button press, giveGoodieBag();
        
    }
    
    public void levelUpTiles()
    {
        //TODO Implement
    }
    
    public void tryGiveGoodieBag()
    {
        if (playerScore >= nextGoodieBagScore)
        {
            //giveGoodieBag(); // Maybe pass score as param
            
            // Calculate next goodie bag score
            nextGoodieBagScore = (nextGoodieBagScore + 2) * 2;
        }
    }

}