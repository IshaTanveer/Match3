using UnityEngine;

public class backgroundTile : MonoBehaviour
{
    enum fruitsEnum
    {
        Apple, Banana, Bluebarry, Coconut, Orange, Watermelon
    }
    [SerializeField] private GameObject[] fruits;
    void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        int randomFruit = Random.Range(0, fruits.Length);
        GameObject fruit = Instantiate(fruits[randomFruit], transform.position, Quaternion.identity);
        fruit.transform.parent = this.transform;
        fruit.name = this.gameObject.name;
    }
}
