using UnityEngine;
using UnityPipeline.Microsoft.CodeAnalysis.CSharp.Syntax;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject backgroundTile;
    public int width;
    public int height;
    private backgroundTile[,] allTiles;

    //Variables required for fruit generation
    enum fruitsEnum
    {
        Apple, Banana, Bluebarry, Coconut, Orange, Watermelon
    }
    [SerializeField] private GameObject[] fruits;
    public GameObject[,] AllFruits;
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
        int randomFruit;
        //if match i found go again
        do{
        randomFruit = Random.Range(0, fruits.Length);
        }
        while(CheckForMatch(i, j, fruits[randomFruit]));

        GameObject fruit = Instantiate(fruits[randomFruit], tempPosition, Quaternion.identity);
        if (fruit.CompareTag("Untagged"))
        {
            Debug.Log("Fruit" + fruit.name + "is untagged. Please set a tag on it");
        }
        fruit.transform.parent = this.transform;
        fruit.name = "(" + i + "," + j + ")";
        AllFruits[i, j] = fruit;
    }
    private bool CheckForMatch(int i, int j, GameObject fruit)
    {
        if(j >= 2)
        {
            GameObject fruitBelow = AllFruits[i, j-1];
            GameObject fruitBelow2 = AllFruits[i, j-2];
            if(SameFruit(fruit, fruitBelow) && SameFruit(fruit, fruitBelow2)) return true;
        }
        if(i >= 2)
        {
            GameObject leftFruit = AllFruits[i-1, j];
            GameObject leftFruit2 = AllFruits[i-2, j];
            if(SameFruit(fruit, leftFruit) && SameFruit(fruit, leftFruit2)) return true;
        }
        return false;
    }
    public bool SameFruit(GameObject fruitOne, GameObject fruitTwo)
    {
        if(fruitOne == null || fruitTwo == null) return false;
        else if(fruitOne.CompareTag("Untagged") || fruitTwo.CompareTag("Untagged")) return false;
        return fruitOne.CompareTag(fruitTwo.tag);
    }
}
