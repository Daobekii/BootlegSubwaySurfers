using UnityEngine;

public class Coin : MonoBehaviour
{
    public ScriptManager scriptManager;
    private float rotationSpeed = 90;

    void Start() {
        scriptManager = GameObject.FindGameObjectWithTag("Logic").GetComponent<ScriptManager>();
    }
    
    void Update() {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            FindFirstObjectByType<AudioManager>().PlaySound("PickUpCoin");
            scriptManager.AddCoin();
            Destroy(gameObject);
        }
    }
}
