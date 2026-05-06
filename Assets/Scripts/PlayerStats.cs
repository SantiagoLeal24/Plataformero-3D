using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Estadisticas")]
    public int saludMaxima = 100;
    public int saludActual;

    private bool estaMuerto = false;

    void Start()
    {
        saludActual = saludMaxima;
    }

    public void TakeDamage(int cantidad)
    {
        if (estaMuerto) return;

        saludActual -= cantidad;

        Debug.Log($"Recibiste daño salud restante {saludActual}");

        if (saludActual <= 0)
        {
            saludActual = 0;

            Morir();
            
        }      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZonaMuerte"))
        {
            TakeDamage(saludMaxima);
        }
    }

    private void Morir()
    {
        estaMuerto = true;
        Debug.Log("Yaa, esta muerto");

        GetComponent<playerMovement>().enabled = false;

        Invoke("ReiniciarEscena", 1f);
    }

    private void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
      
}
