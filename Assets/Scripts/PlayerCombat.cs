using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    [Header("Referencias de Brazos")]
    public HitBoxBrazo brazoDerecho;
    public HitBoxBrazo brazoIzquierdo;

    [Header("Estadisticas de Combate")]
    public int damageBase = 10;
    public int damageFuerte = 25;
    public float duracionGolpe = 0.2f;
    public float ventanaCombo = 1f;

    private int pasoCombo = 0;
    private float temporizadorCombo = 0f;
    private bool estaAtacando = false;


    void Start()
    {
        brazoDerecho.GetComponent<Collider>().enabled = false;
        brazoIzquierdo.GetComponent<Collider>().enabled = false;
    }

   
    void Update()
    {
        if (pasoCombo > 0 && !estaAtacando)
        {
            temporizadorCombo -= Time.deltaTime;

            if (temporizadorCombo <= 0)
            {
                pasoCombo = 0;
                Debug.Log("Combo perdido por inactividad");
            }
        }

        if (Input.GetMouseButtonDown(0) && !estaAtacando)
        {
            EjecutarAtaque();
        }
    }

    private void EjecutarAtaque()
    {
        estaAtacando = true;
        temporizadorCombo = ventanaCombo;

        if (pasoCombo == 0)
        {
            Debug.Log("Combo 1: Jab Derecho 1");

            StartCoroutine(DarGolpe(brazoDerecho, damageBase));
            pasoCombo++;
        }
        else if (pasoCombo == 1)
        {
            Debug.Log("Combo 2: Jab Derecho 2");

            StartCoroutine(DarGolpe(brazoDerecho, damageBase));
            pasoCombo++;
        }
        else if (pasoCombo == 2)
        {
            Debug.Log("Combo 3: Zurdazo");

            StartCoroutine(DarGolpe(brazoIzquierdo, damageFuerte));
            pasoCombo = 0;
        }
    }

    private IEnumerator DarGolpe(HitBoxBrazo brazoUsado, int cantidadDamage)
    {
        brazoUsado.DamageActual = cantidadDamage;

        brazoUsado.GetComponent<Collider>().enabled = true;

        yield return new WaitForSeconds(duracionGolpe);

        brazoUsado.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(0.1f);
        estaAtacando = false;
    }
}
