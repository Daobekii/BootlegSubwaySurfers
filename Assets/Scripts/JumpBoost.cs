using UnityEngine;

public class JumpBoost : MonoBehaviour
{
    public float buffDuration = 10f;
    private float rotationSpeed = 45f;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            BuffHandler buffHandler = FindFirstObjectByType<BuffHandler>();
            if (buffHandler != null)
            {
                buffHandler.ActivateBuff(buffDuration);
            }
            Destroy(gameObject);
        }
    }
}
