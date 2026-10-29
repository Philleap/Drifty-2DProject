using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public ApplePicker gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basket"))
        {
            Debug.Log("Race finished!");
            gameManager.FinishRace();
        }
    }
}