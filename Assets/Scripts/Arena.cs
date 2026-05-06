using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [Header("Configuración de Enemigos")]
    public GameObject enemigoPrefab;
    public int cantidadEnemigos = 5;
    public float alturaSpawnEnemigo = 15f;
    public float radioSpawnEnemigo = 5f;

    [Header("Aislamiento Vertical")]
    public Transform[] plataformasVecinas;
    public float elevacionMuros = 10f; 
    public float velocidadMovimiento = 2f; 

    private int enemigosVivos;
    private bool activada = false;
    private Vector3[] posicionesOriginales;

    void Start()
    {
        
        posicionesOriginales = new Vector3[plataformasVecinas.Length];
        for (int i = 0; i < plataformasVecinas.Length; i++)
        {
            posicionesOriginales[i] = plataformasVecinas[i].position;
        }
    }

    private void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player") && !activada)
        {
            activada = true;
            StartCoroutine(MoverPlataformas(true)); 
            StartCoroutine(LluviaDeEnemigos());
        }
    }

    
    private IEnumerator MoverPlataformas(bool subir)
    {
        float tiempoTranscurrido = 0;

        while (tiempoTranscurrido < velocidadMovimiento)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / velocidadMovimiento;

            for (int i = 0; i < plataformasVecinas.Length; i++)
            {
                Vector3 inicio = subir ? posicionesOriginales[i] : posicionesOriginales[i] + Vector3.up * elevacionMuros;
                Vector3 destino = subir ? posicionesOriginales[i] + Vector3.up * elevacionMuros : posicionesOriginales[i];

                
                plataformasVecinas[i].position = Vector3.Lerp(inicio, destino, porcentaje);
            }

            yield return null; 
        }
    }

    private IEnumerator LluviaDeEnemigos()
    {
        for (int i = 0; i < cantidadEnemigos; i++)
        {
            Vector2 circuloAleatorio = Random.insideUnitCircle * radioSpawnEnemigo;
            Vector3 posicionSpawn = transform.position + new Vector3(circuloAleatorio.x, alturaSpawnEnemigo, circuloAleatorio.y);

            GameObject nuevoEnemigo = Instantiate(enemigoPrefab, posicionSpawn, Quaternion.identity);

            enemigosVivos++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void EnemigoDerrotado()
    {
        enemigosVivos--;
        if (enemigosVivos <= 0)
        {
            StartCoroutine(MoverPlataformas(false)); // Bajar
            Debug.Log("Arena Desbloqueada");
        }
    }
}

