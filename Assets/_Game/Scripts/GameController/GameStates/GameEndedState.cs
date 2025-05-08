using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameEndedState : State
{
    private GameFSM _stateMachine;
    private GameContoller _controller;
    private GameHUDController _hudController;
    private TileSpawner _spawner;

    public GameEndedState(GameFSM stateMachine, GameContoller controller, GameHUDController hudController, TileSpawner spawner)
    {
        _stateMachine = stateMachine;
        _controller = controller;
        _hudController = hudController;
        _spawner = spawner;
    }

    public override void Enter()
    {
        base.Enter();

        // display the game over screen
        _spawner.StopSpawning();
        _controller.StopTimer();

        int finalScore = _controller._score;
        int savedScore = SaveManager.Instance.ActiveSaveData.Score;
        bool isNewHighScore = finalScore > savedScore;

        if (isNewHighScore)
        {
            SaveManager.Instance.ActiveSaveData.Score = finalScore;
            SaveManager.Instance.Save();
            Debug.Log("New High Score Saved: " + finalScore);
        }

        _hudController.DisplayFinalStats(isNewHighScore);
        _hudController.DisplayGameOverMenu();
        

        Debug.Log("STATE: Game Ended");
        Debug.Log("Hide Player HUD");
        Debug.Log("Display GameOver HUD");
    }

    public override void Exit()
    {
        base.Exit();

        _hudController.HideGameOverMenu();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();
    }
}
