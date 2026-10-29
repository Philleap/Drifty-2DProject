using UnityEngine;
using DG.Tweening;
public class AppleTree : MonoBehaviour
{
    [Header("Inscribed")]
    // Prefab for instantiating apples
    public GameObject applePrefab;
    public GameObject goldenApplePrefab;
    // Speed at which the AppleTree moves
    public float speed = 10f;
    // Distance where AppleTree turns around
    public float leftAndRightEdge = 24f;
    // Chance that the AppleTree will change directions
    public float changeDirChance = 0.01f;
    // Seconds between Apples instantiations
    public float appleDropDelay = 1f;
    private Vector3 originalScale;

    void Start()
    {
        //  Start dropping apples
        //Invoke("DropApple", 2f);
        //DOTween.Init();
        //originalScale = transform.localScale;
    }
    void DropApple()
    {
        float roll = Random.value;
        if (roll < 0.10f)
        {
            GameObject goldenApple = Instantiate<GameObject>(goldenApplePrefab);
            goldenApple.transform.position = transform.position;
        }
        else
        {
            GameObject apple = Instantiate<GameObject>(applePrefab);
            apple.transform.position = transform.position;
        }
        transform.localScale = originalScale;
        transform
            .DOScale(originalScale * 1.1f, 1f)
            .SetEase(Ease.OutElastic)
            .OnComplete(() => transform.localScale = originalScale);
        Invoke("DropApple", appleDropDelay);
    }
        // Update is called once per frame
        void Update()
    {
        //basic movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        //changing direction
        if (pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed);   // Move right
        }
        else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed);  // Move left
        }
       // else if (Random.value < changeDirChance)
        //{
        //    speed *= -1;  // Change direction
       // }

    }
    void FixedUpdate()
    {                                                 
        // Random direction changes are now time-based due to FixedUpdate()
        if (Random.value < changeDirChance)
        {                          
            speed *= -1; // Change direction 
        }
    }
}
