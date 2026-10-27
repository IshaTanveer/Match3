using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Vector2 firstTouchPosition; 
    private Vector2 finalTouchPosition;
    // 45 to 135 = up, 135 degree to 135 = left, 45 to -45 = right, -45 to -135 = down
    //Swapping integers
    public float swipeAngle = 0;
    public int column, row;
    private GameObject otherfruit;
    private Board board;
    int targetx, targety;
    Vector2 tempPosition;
    //Match Var
    [Header("Match Varaibles")]
    public bool isMatched = false;
    [SerializeField] private GameObject[] fruits;
    [SerializeField] float moveSpeed;
    [SerializeField] public Color particleColor;
    private void Start() {
        board = FindFirstObjectByType<Board>();
        //targetx = (int)transform.position.x;
        //targety = (int)transform.position.y;

        //column = targetx;
        //row = targety;
    }
    void Update()
    {  
        targetx = column;
        targety = row;
        //transform.position = new Vector2(targetx, targety); using lerp instead of directly moving
        tempPosition = new Vector2(targetx, targety);
        if(Mathf.Abs(targetx - transform.position.x) > .1 || Mathf.Abs(targety - transform.position.y) > .1)
        {
            transform.position = Vector2.Lerp(transform.position, tempPosition, moveSpeed * Time.deltaTime);
            //move towards target 
        }
        else
        {
            transform.position = tempPosition;
            //directly set the position
        }
    }

    private void OnMouseDown() {
        if(board.isResolving) return;
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
    }
    private void OnMouseUp() {
        if(board.isResolving) return;
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > .1 || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > .1)
        {
            CalculateAngle();
        }
    }
    void CalculateAngle()
    {
        //returns a value in radians, so multiply it with 180 and then divide it with pi
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, 
            finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        otherfruit = null; 
        MoveFruits(); 
        if(otherfruit == null) return;
        board.StartCheck(this);
    }
    public void SwapBack()
    {
        ChangeSwipeAngle();
        MoveFruits();
    }

    private void ChangeSwipeAngle()
    {
        if (swipeAngle > -45 && swipeAngle <= 45) swipeAngle = 180;
        else if (swipeAngle > 45 && swipeAngle <= 135) swipeAngle = -90;
        else if (swipeAngle > 135 || swipeAngle <= -135) swipeAngle = 0;
        else if (swipeAngle > -135 && swipeAngle <= -45) swipeAngle = 90;
    }

    void MoveFruits()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1) //right swipe
        {
            otherfruit = board.AllFruits[column + 1, row];
            if(otherfruit == null) return;
            otherfruit.GetComponent<Fruit>().column -=1;
            column += 1;
            board.AllFruits[column, row] = this.gameObject;
            board.AllFruits[column - 1, row] = otherfruit;
        }
        else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) //upwards swipe
        {
            otherfruit = board.AllFruits[column, row + 1];
            if(otherfruit == null) return;
            otherfruit.GetComponent<Fruit>().row -=1;
            row += 1;
            board.AllFruits[column, row] = this.gameObject;
            board.AllFruits[column, row - 1] = otherfruit;
        }
        else if((swipeAngle > 135 || swipeAngle <= - 135) && column > 0) //left swipe
        {
            otherfruit = board.AllFruits[column - 1, row];
            if(otherfruit == null) return;
            otherfruit.GetComponent<Fruit>().column +=1;
            column -= 1;
            board.AllFruits[column, row] = this.gameObject;
            board.AllFruits[column + 1, row] = otherfruit;
        }
        else if(swipeAngle > -135 && swipeAngle <= -45 && row > 0) //downward swipe
        {
            otherfruit = board.AllFruits[column, row - 1];
            if(otherfruit == null) return;
            otherfruit.GetComponent<Fruit>().row +=1;
            row -= 1;
            board.AllFruits[column, row] = this.gameObject;
            board.AllFruits[column, row + 1] = otherfruit;
        }
    }
    /*public void FindMatches()
    {
        if(column > 0 && column < board.width - 1)
        {
            GameObject NextObject = board.AllFruits[column + 1, row];
            GameObject PrevObject = board.AllFruits[column - 1, row];
            if(NextObject != null && PrevObject != null)
            {
                if (board.SameFruit(NextObject, this.gameObject) && board.SameFruit(PrevObject, this.gameObject))
                {
                    MakeIsMatchedTrue(NextObject, PrevObject);
                }
            }
        }
        if(row > 0 && row < board.height - 1)
        {
            GameObject TopObject = board.AllFruits[column, row + 1];
            GameObject BottomObject = board.AllFruits[column, row - 1];
            if(TopObject != null && BottomObject != null)
            {
                if (board.SameFruit(TopObject, this.gameObject) && board.SameFruit(BottomObject, this.gameObject))
                {
                    MakeIsMatchedTrue(TopObject, BottomObject);
                }
            }
        }
    }*/
}
