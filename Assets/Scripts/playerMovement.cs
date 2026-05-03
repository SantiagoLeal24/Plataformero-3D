using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Referencias")]
    public CharacterController controller;
    public Transform cam; //Ref la camara principal

    [Header("Estadísticas de Movimiento")]
    public float velocidad = 6f;
    public float tiempoSuavizadoGiro = 0.1f;
    private float velocidadGiroActual;

    [Header("Gravedad")]
    public float gravedad = -9.81f;
    private Vector3 velocidadCaida;
    public Transform chequeoSuelo;
    public float distanciaSuelo = 0.4f;
    public LayerMask mascaraSuelo;
    private bool enSuelo;

    void Update()
    {
        // 1. Chequear suelo
        enSuelo = Physics.CheckSphere(chequeoSuelo.position, distanciaSuelo, mascaraSuelo);

        if (enSuelo && velocidadCaida.y < 0)
        {
            velocidadCaida.y = -2f;
        }

        // 2. Leer Input

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. Mover y rotar con Input

        if (direccion.magnitude >= 0.1f)
        {

            // Calcula el ángulo hacia donde queremos ir, sumando la rotación de la cámara
            float anguloObjetivo = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // Suaviza la transición entre el ángulo actual y el objetivo
            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloObjetivo, ref velocidadGiroActual, tiempoSuavizadoGiro);
            transform.rotation = Quaternion.Euler(0f, angulo, 0f);

            // Convierte el ángulo en una dirección hacia la que moverse
            Vector3 direccionMovimiento = Quaternion.Euler(0f, anguloObjetivo, 0f) * Vector3.forward;

            // Aplica el movimiento horizontal
            controller.Move(direccionMovimiento.normalized * velocidad * Time.deltaTime);
        
    }

        // Gravedad

        velocidadCaida.y += gravedad * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);
    }
}
