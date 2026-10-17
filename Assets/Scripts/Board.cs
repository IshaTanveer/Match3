using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject backgroundTile;
    [SerializeField] int width;
    [SerializeField] int height;
    private backgroundTile[,] allTiles;

    //Variables required for fruit generation
    enum fruitsEnum
    {
        Apple, Banana, Bluebarry, Coconut, Orange, Watermelon
    }
    [SerializeField] private GameObject[] fruits;
    private GameObject[,] AllFruits;
    void Start()
    {
        AllFruits = new GameObject[width, height];
        allTiles = new backgroundTile[width, height];
        SetupAllTiles();
    }
    void SetupAllTiles()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);;
                GameObject BackgrounTileInstance = Instantiate(backgroundTile, tempPosition, Quaternion.identity) as GameObject;
                BackgrounTileInstance.transform.parent = this.transform;
                BackgrounTileInstance.name = "(" + i + "," + j + ")";
                InitializeFuits(i, j, tempPosition);
            }
        }
    }
    void InitializeFuits(int i, int j, Vector2 tempPosition)
    {
        int randomFruit = Random.Range(0, fruits.Length);
        GameObject fruit = Instantiate(fruits[randomFruit], tempPosition, Quaternion.identity);
        fruit.transform.parent = this.transform;
        fruit.name = "(" + i + "," + j + ")";
        AllFruits[i, j] = fruit;
    }
}
