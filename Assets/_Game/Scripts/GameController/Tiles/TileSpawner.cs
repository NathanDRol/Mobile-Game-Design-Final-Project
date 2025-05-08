using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [Header("Tile Prefab")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float tileLifetime = 3f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxSpawnInterval;
    [SerializeField] private float maxTileLifetime;
    [SerializeField] private float spawnAcceleration = 0.05f;
    [SerializeField] private float lifetimeReduction = 0.05f;
    [SerializeField] private float minSpawnInterval = 0.3f;
    [SerializeField] private float minTileLifetime = 0.5f;
    private float _defaultSpawnInterval;
    private float _defaultTileLifetime;

    private GameContoller _gameController;
    private Coroutine _spawnRoutine;

    private List<GameObject> _activeTiles = new List<GameObject>();

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        _gameController = FindFirstObjectByType<GameContoller>();

        _defaultSpawnInterval = spawnInterval;
        _defaultTileLifetime = tileLifetime;
    }

    public void StartSpawning()
    {
        _spawnRoutine = StartCoroutine(SpawnTiles());
    }

    public void StopSpawning()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    public void ClearAllTiles()
    {
        foreach(GameObject tile in _activeTiles)
        {
            if (tile != null)
            {
                Destroy(tile);
            }
        }
        _activeTiles.Clear();
        StopSpawning();
    }

    public IEnumerator SpawnTiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject newTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);

            _activeTiles.Add(newTile);

            StartCoroutine(HandleTileLifetime(newTile, tileLifetime));

            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - spawnAcceleration);
            tileLifetime = Mathf.Max(minTileLifetime, tileLifetime - lifetimeReduction);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float tileHalfWidth = tilePrefab.transform.localScale.x / 2f;
        float tileHalfHeight = tilePrefab.transform.localScale.y / 2f;

        float minX = -cameraWidth + tileHalfWidth;
        float maxX = cameraWidth - tileHalfWidth;
        float minY = -cameraHeight + tileHalfHeight;
        float maxY = cameraHeight - tileHalfHeight;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector3(randomX, randomY, 0f);
    }

    private IEnumerator HandleTileLifetime(GameObject tile, float lifetime)
    {
        TapTile tapTile = tile.GetComponent<TapTile>();
        
        yield return new WaitForSeconds(lifetime);

        if (tile != null && tapTile != null && tapTile.gameObject != null)
        {

            tapTile.TriggerMissAnimation();

            yield return new WaitForSeconds(0.6f);

            _gameController.RegisterTileMiss();
            Destroy(tile);
        }
    }

    public void ResetSpawner()
    {
        StopSpawning();
        spawnInterval = _defaultSpawnInterval;
        tileLifetime = _defaultTileLifetime;
    }
}