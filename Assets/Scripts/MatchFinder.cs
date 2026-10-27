using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    Board board;
    [SerializeField] public List<GameObject> currentMatches = new List<GameObject>();
    void Start()
    {
        board = FindFirstObjectByType<Board>();
    }
    public bool FindAllMatches()
    {
        bool foundMatch = false;
        //yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentFruit = board.AllFruits[i, j];
                if(i > 0 && i < board.width - 1)
                {
                    foundMatch = FindSingleMatch(foundMatch, i - 1, i + 1, j, j, currentFruit);
                }
                if (j > 0 && j < board.height - 1)
                {
                    foundMatch = FindSingleMatch(foundMatch, i, i, j - 1, j + 1, currentFruit);
                }
            }
        }
        return foundMatch;
    }

    private bool FindSingleMatch(bool foundMatch, int lefti, int righti, int leftj,int rightj, GameObject currentFruit)
    {
        GameObject prevFruit = board.AllFruits[lefti, leftj];
        GameObject nextFruit = board.AllFruits[righti, rightj];
        if (board.SameFruit(prevFruit, currentFruit) && board.SameFruit(nextFruit, currentFruit))
        {
            MakeIsMatchedTrue(prevFruit, nextFruit, currentFruit);
            foundMatch = true;
        }

        return foundMatch;
    }

    private void MakeIsMatchedTrue(GameObject prevFruit, GameObject nextFruit, GameObject currentFruit)
    {
        if(!currentMatches.Contains(prevFruit)) currentMatches.Add(prevFruit);
        prevFruit.GetComponent<Fruit>().isMatched = true;
        if(!currentMatches.Contains(nextFruit)) currentMatches.Add(nextFruit);
        nextFruit.GetComponent<Fruit>().isMatched = true;
        if(!currentMatches.Contains(currentFruit)) currentMatches.Add(currentFruit);
        currentFruit.GetComponent<Fruit>().isMatched = true;
    }
}
