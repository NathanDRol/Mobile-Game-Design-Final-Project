using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayState : State
{
    private GameFSM _stateMachine;
    private GameContoller _controller;
    private TileSpawner _spawner;
    private GameHUDController _hudController;
    private AudioClips _audioClips;

    public bool IsInPlay = false;

    public GamePlayState(GameFSM stateMachine, GameContoller controller, GameHUDController hudController, TileSpawner spawner, AudioClips audioClips)
    {
        _stateMachine = stateMachine;
        _controller = controller;
        _spawner = spawner;
        _hudController = hudController;
        _audioClips = audioClips;
    }

    public override void Enter()
    {
        base.Enter();
        IsInPlay = true;
        _hudController.DisableAllDisplays();
        _hudController.DisplayPlayerHUD();

        AudioSource audioSource = MusicController.PlayMusic2D(_audioClips._gameMusic, .43f);

        _controller.StartTimer();

        _spawner.ResetSpawner();
        _spawner.StartSpawning();
        Debug.Log("STATE: Game Play");
        Debug.Log("Listen for Player Inputs");
        Debug.Log("Display Player HUD");
    }


    public override void Exit()
    {
        base.Exit();
        IsInPlay = false;

        _controller.StopTimer();
        _hudController.HidePlayerHUD();
        _spawner.ClearAllTiles();
        MusicController.StopMusic();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();

        // check for lose condition
        if (_controller._activeMissCount >= 3)
        {
            Debug.Log("Game Over!");
            // Lose State, reload Level, change back to SetupState, etc.
            _stateMachine.ChangeState(_stateMachine.EndedState);
        }
    }
}
