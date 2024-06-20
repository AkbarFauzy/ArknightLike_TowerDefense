using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<IStageObserver> _observers = new List<IStageObserver>();

    [System.Serializable]
    private struct Wave {
        public List<Spawner> spawn;
    }
    [System.Serializable]
    private struct Spawner {
        public GameObject prefab;
        public CharacterStats stats;
        public float waitingTime;
    }

    [SerializeField] private List<Wave> StageWave;
    [HideInInspector] public int EnemyNumber;
    private Enemy _latestEnemy;

    private void Start()
    {
        foreach (var wave in StageWave) {
            EnemyNumber += wave.spawn.Count;
        }

        StartCoroutine(SpawnEnemy());
    }

    public int GetEnemyNumber() {
        return EnemyNumber;
    }

    private IEnumerator SpawnEnemy() {
        foreach (Wave wave in StageWave) {
            foreach (Spawner spawn in wave.spawn) {
                yield return new WaitForSeconds(spawn.waitingTime);
                GameObject enemy = Instantiate(spawn.prefab, transform.position, Quaternion.identity);
                enemy.transform.parent = this.transform;
                enemy.GetComponentInChildren<Enemy>().SetStats(spawn.stats);
                _latestEnemy = enemy.GetComponentInChildren<Enemy>();
                NotifyObserverEnemySpawn(_latestEnemy);
            }
        }
    }

    public Enemy GetLatestEnemy() {
        return _latestEnemy;
    }

    public void AddObserver(IStageObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IStageObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyObserver(StageEvents stageEvent)
    {
        _observers.ForEach((_observers) =>
        {
            _observers.OnNotify(stageEvent);
        });
    }

    private void NotifyObserverEnemySpawn(Enemy enemy)
    {
        _observers.ForEach((_observers) =>
        {
            _observers.OnNotifyCharacterSpawn(enemy);
        });
    }
}
