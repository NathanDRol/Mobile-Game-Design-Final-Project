using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class MainMenuState : State
{
    private GameFSM _stateMachine;
    private GameContoller _controller;
    private GameHUDController _hudController;
    private AudioClips _audioClips;

    public MainMenuState(GameFSM stateMachine, GameContoller controller, GameHUDController hudController, AudioClips audioClips)
    {
        _stateMachine = stateMachine;
        _controller = controller;
        _hudController = hudController;
        _audioClips = audioClips;
    }

    public override void Enter()
    {
        base.Enter();
        _hudController.HidePlayerHUD();
        _hudController.HideGameOverMenu();
        _hudController.DisplayMainMenuHUD();
        _hudController.DisplayHighScore();

        AudioSource audioSource = MusicController.PlayMusic2D(_audioClips._mainMenuMusic, .70f);

    }

    public override void Exit()
    {
        base.Exit();
        _hudController.HideMainMenuHUD();
        MusicController.StopMusic();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();
        
       // _stateMachine.ChangeState(_stateMachine.PlayState);
    }
}
