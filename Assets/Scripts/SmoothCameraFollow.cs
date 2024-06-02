using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target; // El objetivo a seguir (la bola)
    public float smoothSpeed = 0.125f; // La velocidad de suavizado
    public Vector3 offset; // Desplazamiento de la cámara respecto al objetivo

    void LateUpdate()
    {
        if (target != null)
        {
            // Posición deseada basada en la posición del objetivo y el desplazamiento
            Vector3 desiredPosition = target.position + offset;
            // Posición suavizada entre la posición actual y la posición deseada
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Aplicar la posición suavizada a la cámara
            transform.position = smoothedPosition;

            // Mantener la rotación fija hacia abajo
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
