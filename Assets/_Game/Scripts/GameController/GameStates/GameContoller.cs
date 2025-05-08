using System.Collections;
using UnityEngine;


public class GameContoller : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private float _tapLimitDuration = 120;

    [Header("Dependencies")]
    [SerializeField] private Unit _playerUnitPrefab;
    [SerializeField] private Transform _playerUnitSpawnLocation;
    [SerializeField] private TileSpawner _tileSpawner;
    [SerializeField] private InputBroadcaster _input;
    [SerializeField] private GameHUDController _hudController;
    [SerializeField] private AudioClips _audioClips;
    public float ElapsedTime { get; private set; }
    private bool _isTimerRunning = false;

    public float TapLimitDuration => _tapLimitDuration;

    public Unit PlayerUnitPrefab => _playerUnitPrefab;
    public Transform PlayerUnitSpawnLocation => _playerUnitSpawnLocation;
    public TileSpawner TileSpawner => _tileSpawner;
    public InputBroadcaster Input => _input;
    public GameHUDController HUDController => _hudController;

    public int _score = 0;
    public int _activeMissCount = 0;
    public int _totalMissCount = 0;

    private Coroutine _missDecayCoroutine;
    private float _missDecayTime = 3f;

    private void Start()
    {
        SaveManager.Instance.Load();
    }

    private void Update()
    {
        if (_isTimerRunning)
        {
            ElapsedTime += Time.deltaTime;
        }
    }
    public void RegisterTileMiss()
    {
        _totalMissCount++;
        _activeMissCount = Mathf.Min(_activeMissCount + 1, 3);

        AudioSource audioSource = AudioController.PlayClip2D(_audioClips._miss, .5f);
        audioSource.pitch = UnityEngine.Random.Range(.75f, 1.25f);

        _hudController.UpdateHealthIcons(_activeMissCount);

        // restart timer
        if (_missDecayCoroutine != null)
        {
            StopCoroutine( _missDecayCoroutine );
        }
        _missDecayCoroutine = StartCoroutine(MissDecayTimer());
    }

    private IEnumerator MissDecayTimer()
    {
        yield return new WaitForSeconds(_missDecayTime);

        _activeMissCount = Mathf.Max(_activeMissCount - 1, 0);
        _hudController.UpdateHealthIcons(_activeMissCount);
        
        if (_activeMissCount > 0)
        {
            _missDecayCoroutine = StartCoroutine(MissDecayTimer());
        }else
        {
            _missDecayCoroutine = null;
        }
    }

    public void StartTimer()
    {
        ElapsedTime = 0f;
        _isTimerRunning = true;
    }

    public void StopTimer()
    {
        _isTimerRunning = false;
    }

    public void ResetGame()
    {
        _score = 0;
        _activeMissCount = 0;
        _totalMissCount = 0;
        ElapsedTime = 0f;

        if (_missDecayCoroutine != null)
        {
            StopCoroutine(_missDecayCoroutine);
            _missDecayCoroutine = null;
        }

        _hudController.UpdateHealthIcons(_activeMissCount);

        _tileSpawner.ResetSpawner();
    }
}
