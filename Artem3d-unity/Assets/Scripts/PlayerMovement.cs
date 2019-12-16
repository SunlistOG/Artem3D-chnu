using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // компонент, що відповідає за анімування персонажа
    private Animator anim;
    // швидкість ходьби персонажа
    public float WalkSpeed = 6.0f;
    // швидкість бігу персонажа
    public float RunSpeed = 20f;
    // показник швидкості, з якою персонаж притягується до землі
    private float gravity = 500.0f;
    // стан, в якому перебуває персонаж(біг, ходьба і тд)
    private State state = State.stay;
    // компонент, який відповідає за управління персонажем
    private CharacterController controller;
    // вектор напрямку руху персонажа
    private Vector3 moveDirection = Vector3.zero;
    // посилання на компонент камери, що спостерігає за персонажем
    // камера є фізичним тілом, яке не може проходити крізь інші об'єкти
    private Rigidbody cam;

    // посилання на компонент, що є фізичним тілом даного персонажа
    private void Start()
    {
        // визначаємо компонент, що відповідає за управління персонажем
        controller = GetComponent<CharacterController>();
        // визначаємо компонент, що відповідає за анімування персонажа
        anim = GetComponent<Animator>();
        // визначаємо компонент, що є камерою, яка спостерігає за персонажем
        cam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Rigidbody>();
    }

    // функція, що виконується перед усіма функціями типу Update
    void FixedUpdate()
    {
        // отримуємо ввід користувача
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // визначаємо стан в залежності від вводу користувача
        GetStateByInput();
        // визначаємо чи персонаж повинен пересуватися
        bool isWalking = (moveDirection.magnitude != 0);
        // визначаємо швидкість пересування персонажа
        float speed = ((isWalking) && ((Input.GetAxis("Run") > 0))) ? RunSpeed : WalkSpeed;
        // нормалізуємо вектор пересування персонажа
        moveDirection = moveDirection.normalized;
        // функція, що відповідає за стрибок
        if ((Input.GetAxis("Jump") > 0) && (controller.isGrounded))
        {
            StartCoroutine(Jump());
        }
        // функція, що відповідає за пересування персонажа
        if (isWalking)
        {
            transform.rotation = Quaternion.Euler(0f, Vector2.SignedAngle(Vector2.up, new Vector2(moveDirection.x * -1f, moveDirection.z)) + cam.rotation.eulerAngles.y, 0f);
            float prevHight = moveDirection.y;
            moveDirection = cam.rotation * moveDirection;
            moveDirection.y = prevHight;
        }
        // накладання гравітації на персонажа
        moveDirection.y -= gravity * Time.deltaTime / speed;
        // команда пересування для персонажа
        controller.Move(moveDirection * speed * Time.fixedDeltaTime);
    }
    // функція, що виконується на початку кожного кадру
    private void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        GetStateByInput();
    }
    // функція, що обирає стани персонажа
    private void GetStateByInput()
    {
        if ((!controller.isGrounded))
        {
            SetState(State.jump);
        }

        else if (moveDirection.magnitude != 0)
        {
            if (Input.GetAxis("Run") == 0)
            {
                SetState(State.walk);
            }
            else if (Input.GetAxis("Run") == 1)
            {
                SetState(State.run);
            }
        }
        else SetState(State.stay);
    }
    // функція, що відповідає за стрибок
    public IEnumerator Jump()
    {
        gravity = -1500f;
        yield return new WaitForSeconds(0.15f);
        gravity = 300f;
        yield return null;
    }
    // перелічення станів персонажа
    // він може стояти, ходити, бігти та стрибати
    private enum State
    {
        stay = 1,
        walk = 2,
        run = 3,
        jump = 4,
    }
    // функція, що запускає анімацію із вибраним станом
    private void SetState(State s)
    {
        switch ((int)s)
        {
            case 1: anim.Play("Artem_stay", 0); break;
            case 2: anim.Play("Artem_walk", 0); break;
            case 3: anim.Play("Artem_run", 0); break;
            case 4: anim.Play("Artem_jump", 0); break;
        }
    }
}