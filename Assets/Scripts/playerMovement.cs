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

    [Header("Gravedad y Salto")]
    public float gravedad = -9.81f;
    private Vector3 velocidadCaida;
    public float fuerzaSalto = 2f;
    public float multiplicadorImpulsoFrontal = 1.5f;

    [Header("Chequeo de Suelo")]
    public Transform chequeoSuelo;
    public float distanciaSuelo = 0.4f;
    public LayerMask mascaraSuelo;
    private bool enSuelo;

    [Header("Wall Run")]
    public LayerMask mascaraPared;
    public float distanciaRaycatPared = 0.6f;
    public float gravedadWallRun = -1f; //gravedad reducida
    public bool tocandoParedIzquierda;
    private bool tocandoParedDerecha;
    private bool enWallRun;


    void Update()
    {
        ChequearSuelo();
        Movimiento();
        Salto();
        aplicarGravedad();
        ChequearParedes();
    }    

    private void ChequearSuelo()
    {
        enSuelo = Physics.CheckSphere(chequeoSuelo.position, distanciaSuelo, mascaraSuelo);

        if (enSuelo && velocidadCaida.y  < 0)
        {
            velocidadCaida.y = -2f;
        }
    }

    private void ChequearParedes()
    {
        tocandoParedDerecha = Physics.Raycast(transform.position, transform.right, distanciaRaycatPared, mascaraPared);
        tocandoParedIzquierda = Physics.Raycast(transform.position, -transform.right, distanciaRaycatPared, mascaraPared);
        enWallRun = (tocandoParedDerecha || tocandoParedIzquierda) && !enSuelo;

    }

    private void Movimiento()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        if (direccion.magnitude >= 0.1f)
        {
            float anguloObjetivo = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloObjetivo, ref velocidadGiroActual, tiempoSuavizadoGiro);
            transform.rotation = Quaternion.Euler(0f, angulo, 0f);

            Vector3 direccionMovimiento = Quaternion.Euler(0f, anguloObjetivo, 0f) * Vector3.forward;

            // Si estamos en el aire aplicamos multiplicador para salto mas largo
            float velocidadActual = enSuelo ? velocidad : velocidad * multiplicadorImpulsoFrontal;

            controller.Move(direccionMovimiento.normalized * velocidadActual * Time.deltaTime);
        }
    }

    private void Salto()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo)
            {
                velocidadCaida.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
            }
            else if (enWallRun)
            {
                velocidadCaida.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
            }

        }
    }

    private void aplicarGravedad()
    {
        if (enWallRun)
        {
            velocidadCaida.y = gravedadWallRun;
        }
        else
        {
            velocidadCaida.y += gravedad * Time.deltaTime;
        }
            
        controller.Move(velocidadCaida * Time.deltaTime);
    }
}
