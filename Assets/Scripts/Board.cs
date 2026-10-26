using System.Collections;
using UnityEngine;

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
    int MatchFound = 0;
    public bool isResolving;
    [SerializeField] int offset;
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
                Vector2 tempPosition = new Vector2(i, j);
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
        tempPosition = new Vector2(i, j + offset);
        GameObject fruit = Instantiate(fruits[randomFruit], tempPosition, Quaternion.identity);
        fruit.GetComponent<Fruit>().row = j;
        fruit.GetComponent<Fruit>().column = i;
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

    public void StartCheck(Fruit swapped)
    {
        isResolving = true;
        StartCoroutine(CheckForMatch(swapped));
    }

    IEnumerator CheckForMatch(Fruit swappedFruit)
    {
        yield return new WaitForSeconds(0.5f);
        if (!FindAllMatches())
        {
            swappedFruit.SwapBack();
        }

        while(FindAllMatches()){
            FindMatchedFruitsToDestroy();
            collapseColumns();
            //yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(FillBoard());
        }
        foreach (GameObject fruit in AllFruits)
        {
            if(fruit != null) fruit.GetComponent<Fruit>().isMatched = false;
        }  
        isResolving = false;
    }

    private bool FindAllMatches()
    {
        MatchFound = 0;
        foreach (GameObject fruit in AllFruits)
        {
            if (fruit != null) {
                fruit.GetComponent<Fruit>().FindMatches();
                if(fruit.GetComponent<Fruit>().isMatched) MatchFound++;
            }
        }
        if(MatchFound == 0) return false;
        else return true;
    }
    private void collapseColumns()
    {
        for(int i = 0; i < width; i++)
        {
            int emptyCount = 0;
            for (int j = 0; j < height; j++)
            {
                if(AllFruits[i, j] == null)
                {
                    emptyCount++;
                }
                else if (emptyCount > 0)
                {
                    GameObject fruit = AllFruits[i, j];
                    fruit.GetComponent<Fruit>().row -= emptyCount;
                    AllFruits[i, j - emptyCount] = fruit;
                    AllFruits[i, j] = null;
                }
            }
        }
    }
    void FindMatchedFruitsToDestroy()
    {
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                DestroyMatchedFruits(i, j);
            }
        }
    }

    private void DestroyMatchedFruits(int column, int row)
    {
        if(AllFruits[column, row] == null) return;
        if(AllFruits[column, row].GetComponent<Fruit>().isMatched)
        {
            Destroy(AllFruits[column, row]); //use object pooling here!!!
            AllFruits[column, row] = null;
        }
    }
    void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(AllFruits[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + offset);
                    int randomFruit = Random.Range(0, fruits.Length);
                    GameObject fruit = Instantiate(fruits[randomFruit], tempPosition, Quaternion.identity) as GameObject;
                    fruit.GetComponent<Fruit>().row = j;
                    fruit.GetComponent<Fruit>().column = i;
                    fruit.transform.parent = transform;
                    fruit.name = "(" + i + "," + j + ")";
                    AllFruits[i, j] = fruit;
                }
            }
        }
    }
    IEnumerator FillBoard()
    {
        RefillBoard();
        yield return new WaitForSeconds(0.5f);
    }
}
