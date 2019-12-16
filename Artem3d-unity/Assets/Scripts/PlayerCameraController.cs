using UnityEngine;
using System.Collections;

public class PlayerCameraController : MonoBehaviour
{
    public GameObject MenuCamera;
    // ціль, за якою рухається камера
    public Transform target;
    // дистанція(регулюється динамічно під час гри)
    public float distance = 5.0f;
    // чутливість мишки по осях
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    // обмеження верхньої і нижньої зони огляду
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    // мінімальний та максимальний показник зума камери
    public float distanceMin = .5f;
    public float distanceMax = 15f;
    // тіло камери(застосовується для того, щоб камера не застрягала в інших об'єктах
    private Rigidbody rigidbody;
    // // всі елементи, які потрібно сховати, коли ми в меню
    public GameObject[] nonUI;
    // позиція камери
    float x = 0.0f;
    float y = 0.0f;

    // викликається один раз при старті
    void Start()
    {
        // приховує вказівник мишки
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // змушує камеру не змінювати кут повороту
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }
    // LateUpdate виконується останньою після всіх функцій Update
    void LateUpdate()
    {
        if (target)
        {
            // отримуємо інформацію з мишки за допомогою InputManager
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            // обмежуємо змінну у потрібних нам рамках
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // обчислюємо поворот камери
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            // дистанція зміниться, якщо користувач скористається колесом мишки
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            // обчислюємо дистанцію від камери до об'єкта
            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance -= hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            // обчислюємо позицію камери
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
        // функція виклику внутрішньоігрового меню
        if (Input.GetAxis("Cancel") == 1)
        {
            Cursor.lockState = CursorLockMode.None;
            MenuCamera.SetActive(true);
            HideAllObjects();
        }
    }
    // функція, яка обмежує оглядовість вверх та вниз
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    // ховає всі вибрані ігрові об'єкти
    public void HideAllObjects()
    {
        foreach (var obj in nonUI)
        {
            obj.SetActive(false);
        }
    }
}