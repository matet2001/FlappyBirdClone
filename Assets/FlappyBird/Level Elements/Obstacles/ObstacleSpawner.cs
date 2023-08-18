using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private List<ObstacleController> obstaclePool = new List<ObstacleController>();

    [SerializeField] Transform obstacleContainer;
    [SerializeField] float queueTime = 1.25f;
    [SerializeField] float height = 1.1f;
    [SerializeField] float obstacleSpeed = 2.5f;

    private GameObject pfObstacle;
    private float timer;
    private bool shouldSpawnObstacles;

    private void Awake()
    {
        GameEventController.OnGameRestart += OnGameRestart;
        GameEventController.OnGameOver += OnGameOver;
        GameEventController.OnGameStart += OnGameStart;

        LoadObstaclePrefab();
    }
    private void LoadObstaclePrefab()
    {
        pfObstacle = Resources.Load<GameObject>("PfObstacle");

        if (!pfObstacle) Debug.Log("Loading obstacle failed");
    }
    private void Update()
    {
        SpawningObstacles();
    }
    #region Obstacle Spawning
    private void SpawningObstacles()
    {
        if (!shouldSpawnObstacles) return;
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        SpawnObstacle();
    }
    private void SpawnObstacle()
    {
        timer = queueTime;

        Vector3 position = obstacleContainer.position + new Vector3(0, UnityEngine.Random.Range(-height, height), 0);

        if (!TryToGetOutFromPool(out ObstacleController returnObstacleController, position))
            CreateNewObstacle(out returnObstacleController, position);

        StartObstacle(position, returnObstacleController);
    }
    private bool TryToGetOutFromPool(out ObstacleController returnObstacleController, Vector3 position)
    {
        returnObstacleController = null;

        foreach (ObstacleController obstacleController in obstaclePool)
        {
            if (obstacleController.gameObject.activeInHierarchy) continue;
            returnObstacleController = obstacleController;
            return true;
        }

        return false;
    }
    private void StartObstacle(Vector3 position, ObstacleController obstacleController)
    {
        obstacleController.gameObject.SetActive(true);
        obstacleController.transform.position = position;
        obstacleController.StartObstacle(obstacleSpeed);
    }
    private void CreateNewObstacle(out ObstacleController obstacleController, Vector3 position)
    {
        GameObject newObstacleGameObject = Instantiate(pfObstacle, position, Quaternion.identity, obstacleContainer);
        obstacleController = newObstacleGameObject.GetComponent<ObstacleController>();
        obstaclePool.Add(obstacleController);
    }
    #endregion
    #region Events
    private void OnGameStart()
    {
        obstaclePool.ForEach(x => x.StartObstacle(obstacleSpeed));
        shouldSpawnObstacles = true;
    }
    private void OnGameOver()
    {
        obstaclePool.ForEach(x => x.StopObstacle());
        shouldSpawnObstacles = false;
    }
    private void OnGameRestart()
    {
        obstaclePool.ForEach(x => x.gameObject.SetActive(false));
    }
    #endregion
}
