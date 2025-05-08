using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameContoller))]
public class GameFSM : StateMachineMB
{
    private GameContoller _controller;
    [SerializeField] private AudioClips _audioClips;
    [SerializeField] private TileSpawner _spawner;

    // state variables here
    public GameSetupState SetupState { get; private set; }
    public GamePlayState PlayState { get; private set; }

    public GameEndedState EndedState { get; private set; }

    public MainMenuState MenuState { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<GameContoller>();
        //_spawner = GetComponent<TileSpawner

        GameHUDController hudController = _controller.HUDController;


        // state instantiation here
        SetupState = new GameSetupState(this, _controller);
        MenuState = new MainMenuState(this, _controller, hudController, _audioClips);
        PlayState = new GamePlayState(this, _controller, hudController, _spawner, _audioClips);
        EndedState = new GameEndedState(this, _controller, hudController, _spawner);

        hudController.SetGamePlayState(PlayState);
    }

    private void Start()
    {
        ChangeState (SetupState);
    }
}