using UnityEngine;

public class CarCameraFollow : MonoBehaviour
{
    public Transform target; // Машина, за которой следует камера
    public float cameraHeight = 10f; // Высота камеры (по оси Y)
    public float smoothTime = 0.3f; // Время сглаживания
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Проверяем, что камера ортографическая
        Camera cam = GetComponent<Camera>();
        if (cam != null && !cam.orthographic)
        {
            Debug.LogWarning("Камера должна быть ортографической!");
            cam.orthographic = true;
        }
    }

    void FixedUpdate() // Используем FixedUpdate для синхронизации с физикой
    {
        if (target == null) return;

        // Желаемая позиция камеры: X и Z берём от машины, Y фиксируем
        Vector3 desiredPosition = new Vector3(target.position.x, cameraHeight, target.position.z);

        // Плавное следование
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}