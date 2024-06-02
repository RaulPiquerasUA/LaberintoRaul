using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardController : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public float maxRotationAngle = 10.0f;

    void Update()
    {
        float tiltX = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        float tiltZ = -Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // Aplicar rotaciones
        Vector3 newRotation = transform.eulerAngles + new Vector3(tiltX, 0, tiltZ);

        // Convertir la rotación a un rango de -180 a 180 grados para una comparación más fácil
        if (newRotation.x > 180) newRotation.x -= 360;
        if (newRotation.z > 180) newRotation.z -= 360;

        // Limitar la rotación en los ejes X y Z
        newRotation.x = Mathf.Clamp(newRotation.x, -maxRotationAngle, maxRotationAngle);
        newRotation.z = Mathf.Clamp(newRotation.z, -maxRotationAngle, maxRotationAngle);

        // Asignar la nueva rotación
        transform.eulerAngles = newRotation;
    }
}

