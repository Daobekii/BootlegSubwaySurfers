using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;
    private float targetCameraY;

    void Start()
    {
        offset = transform.position - player.position;
        targetCameraY = transform.position.y;
    }

    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x, targetCameraY, offset.z + player.position.z);

        float newCameraY = player.localPosition.y + 1.7f; //1.7 ist die y-Koordinate des Spielers, wenn er auf einem Container ist
        if (newCameraY < 2.9f)
        {
            newCameraY = 3.5f;
        } else if (newCameraY > 2.9f){
            newCameraY = 5.2f;
        }
        targetCameraY = newCameraY;

        transform.position = Vector3.Lerp(transform.position, newPosition, 5 * Time.deltaTime);
        //Eventuell noch hinzufügen, dass die Kamera sich abhängig von spieler x-Achse mitbewegt, wie in Subway Surfers. Aber Zaun wird nerven. Vielleicht 0.75, anstatt 1.5?
    }
}
