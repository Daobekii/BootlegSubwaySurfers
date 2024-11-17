using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    private float buffDuration = 0f;
    private bool isBuffActive = false;

    void Update()
    {
        if (isBuffActive)
        {
            buffDuration -= Time.deltaTime;

            if (buffDuration <= 0)
            {
                DeactivateBuff();
            }
        }
    }

    public void ActivateBuff(float duration)
    {
        PlayerMovement.jumpHeight = 2.0f;
        buffDuration = duration;
        isBuffActive = true;
    }

    private void DeactivateBuff()
    {
        PlayerMovement.jumpHeight = 1.0f;
        isBuffActive = false;
    }

    // Diese Methoden für den StatsManager bereitstellen
    public bool IsBuffActive()
    {
        return isBuffActive;
    }

    public float GetRemainingBuffTime()
    {
        return Mathf.Max(buffDuration, 0);
    }
}
