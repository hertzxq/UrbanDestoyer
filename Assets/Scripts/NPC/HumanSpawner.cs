using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    public GameObject humanPrefab; // Префаб человека (NPC)
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
        SpawnHumans();
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        // Спавн новых людей, когда таймер достигает интервала
        if (timer >= spawnInterval)
        {
            SpawnHumans();
            timer = 0f; // Сброс таймера
        }
    }

    void SpawnHumans()
    {
        // Спавн человека на каждой точке спавна
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Создаем нового человека на позиции и с поворотом точки спавна
            Instantiate(humanPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}