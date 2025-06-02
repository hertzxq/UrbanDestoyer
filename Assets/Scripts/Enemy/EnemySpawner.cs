using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject policeCarPrefab; // Префаб полицейской машины
    public float spawnInterval = 5f; // Интервал спавна в секундах
    public Transform[] spawnPoints; // Точки спавна
    private float timer; // Таймер для отслеживания интервала

    void Start()
    {
        // Инициализация массива точек спавна
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
        
        // Немедленный спавн при старте
        SpawnPoliceCars();
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        // Спавн новых машин, когда таймер достигает интервала
        if (timer >= spawnInterval)
        {
            SpawnPoliceCars();
            timer = 0f; // Сброс таймера
        }
    }

    void SpawnPoliceCars()
    {
        // Спавн машины на каждой точке спавна
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Создаем новую машину на позиции и с поворотом точки спавна
            Instantiate(policeCarPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}