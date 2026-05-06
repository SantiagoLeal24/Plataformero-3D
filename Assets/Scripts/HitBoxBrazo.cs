using UnityEngine;

public class HitBoxBrazo : MonoBehaviour
{
    public int DamageActual;

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Enemigo"))
        {
            Debug.Log($"Impacto en enemigo, daño causado {DamageActual}");
        }
    }
}
