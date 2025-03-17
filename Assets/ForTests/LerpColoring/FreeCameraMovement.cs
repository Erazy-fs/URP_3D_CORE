using UnityEngine;

public class FreeCameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;   // Скорость перемещения
    public float lookSpeed = 2f;   // Скорость вращения камеры
    public float upDownSpeed = 2f; // Скорость движения вверх/вниз

    private float rotationX = 0f;  // Для вращения камеры по оси X (вверх/вниз)
    private float rotationY = 0f;  // Для вращения камеры по оси Y (влево/вправ)

    private Transform playerTransform;
    
    private float initialRotationX;
    private float initialRotationY;

    void Start()
    {
        playerTransform = transform; // Получаем ссылку на трансформ камеры

        // Сохраняем начальные углы вращения из инспектора
        initialRotationX = playerTransform.eulerAngles.x;
        initialRotationY = playerTransform.eulerAngles.y;

        // Убираем изменения в начальной ориентации камеры
        rotationX = initialRotationX;
        rotationY = initialRotationY;
    }

    void Update()
    {
        // Получаем ввод с клавиатуры
        float moveForwardBack = Input.GetAxis("Vertical"); // W / S
        float moveLeftRight = Input.GetAxis("Horizontal"); // A / D
        float moveUpDown = 0;

        if (Input.GetKey(KeyCode.Space)) // движение вверх
        {
            moveUpDown = 1;
        }
        else if (Input.GetKey(KeyCode.LeftControl)) // движение вниз
        {
            moveUpDown = -1;
        }

        // Перемещаем камеру в зависимости от ее текущего направления
        Vector3 move = playerTransform.forward * moveForwardBack + playerTransform.right * moveLeftRight + playerTransform.up * moveUpDown;
        playerTransform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // Вращаем камеру с помощью мыши
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Ограничиваем вращение по вертикали

        rotationY += Input.GetAxis("Mouse X") * lookSpeed;

        // Применяем сохраненные начальные углы и текущие изменения вращения
        playerTransform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
