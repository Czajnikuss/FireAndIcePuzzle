using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalionApps;
using TMPro;

public class MainBoard : MonoBehaviour
{
    public Grid<SpriteGridObject> mainBoardGrid;
    public int width, hight;
    public float cellSize;
    public Sprite[] sprites;
    public GameObject gridCellPrefab;
    public Vector3 scaleOfCell;

    public PuzzlePieceShower[] allShowers;
    private bool allAwaiting = true;

    private Vector3 mousePos;
    private SpriteGridObject clickedObject;
    [Space]
    [Header("UI Elements")]
    [Space]
    public TextMeshPro firePointsText;
    public TextMeshPro icePointsText;
    public TextMeshPro totalPointsText;
    public int firePoints, icePoints, totalPoints;
    public int pointsForFullFire, pointsForFullIce;
    public GameObject lostPanel;
    
    
    
    
    void Start()
    {
        lostPanel.SetActive(false);
        mainBoardGrid = new Grid<SpriteGridObject>(width, hight, cellSize, transform.position, (Grid<SpriteGridObject> g, int x, int y) => new SpriteGridObject(g, x, y, sprites, gridCellPrefab, scaleOfCell));
        mainBoardGrid.SetAllGridElementsAsChildren(this.transform);
        mainBoardGrid.OnGridValueChanged += mainBoardGrid.Grid_OnValueChanged;
    
        FirePointShow();
        IcePointShow();
    }

    // Update is called once per frame
    void Update()
    {
       /* if(Input.GetMouseButtonDown(0))
        {
            mousePos = UtilsClass.GetMouseWorldPosition();
            clickedObject = spriteGrid.GetGridObject(mousePos);
            clickedObject.ChangeSprite(1);
            
        }*/

        if(Input.GetKeyDown(KeyCode.B))
        {
            mousePos = UtilsClass.GetMouseWorldPosition();
           
            clickedObject = mainBoardGrid.GetGridObject(mousePos);
            Debug.Log(clickedObject);
            
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        
    }
    public void DropedOnMainGrid(Vector3 dropPosition, PuzzlePieceShower dropedShower)
    {
        List<bool> checkThisPuzzleList = new List<bool>();
        List<int[]> goodCoordinates = new List<int[]>();
        bool probablyBadFit = false;
        int x,y;
        
        mainBoardGrid.GetXY(dropPosition, out x, out y);
        //Debug.Log("x; "+ x + " Y: " + y);
        if(x<0 || y<0 || x>width || y>hight)
        {
            probablyBadFit = true;
            goto BadFitForSure;
        }
        for (int i = 0; i < dropedShower.width; i++)
        {
            if(x+i-2<0)
            {
                
                probablyBadFit = true;
                continue;
            }
            for (int j = 0; j < dropedShower.hight; j++)
            {
                if(y+j-2<0)
                {
                    
                    probablyBadFit = true;
                    continue;
                }

                if(dropedShower.puzzleGrid.gridArray[i,j].thisSpriteName == SpriteNames.empty)
                {
                    probablyBadFit = true;
                    continue;
                }
                
                else
                {
                   if(x+(i-2) > width || y+(j-2) > hight)
                   {
                       probablyBadFit = true;
                       continue;
                   }
                   else
                   {
                        if(mainBoardGrid.gridArray[x+(i-2),y+(j-2)].thisSpriteName == SpriteNames.empty)
                        {
                            checkThisPuzzleList.Add(true);
                            goodCoordinates.Add(new int[2]  {x+(i-2), y+(j-2)});
                        }
                        else checkThisPuzzleList.Add(false);
                   }
                }
            }
        }
        if(checkThisPuzzleList.Contains(false))
        {
            //return puzzle to holding place
            dropedShower.BadFit();
        }
        else
        {
            //put puzzle in place
            probablyBadFit = false;
            dropedShower.GoodFit();
            foreach (var item in goodCoordinates)
            {
                mainBoardGrid.gridArray[item[0], item[1]].SetSpriteName((int)dropedShower.thisPuzzleType);
                mainBoardGrid.gridArray[item[0], item[1]].playFadeInAnim();
            }
            MoveMade();
        }
        BadFitForSure:
        if(probablyBadFit) dropedShower.BadFit();
    }

    public void MoveMade()
    {
        //check if any row or column filed 
        //column
        for (int x = 0; x < width; x++)
        {
            int fireColumn=0, iceColumn=0;
            for (int y = 0; y < hight; y++)
            {
                if(mainBoardGrid.gridArray[x,y].thisSpriteName == SpriteNames.fireFilled) fireColumn++;
                
                if(mainBoardGrid.gridArray[x,y].thisSpriteName == SpriteNames.iceFilled) iceColumn++;
            }
           // Debug.Log("Column " + x + "amount fire " + fireColumn);
            if(fireColumn == hight)
            {
//TODO                //fire column points and animation
                for (int y = 0; y < hight; y++)
                {
                    if(mainBoardGrid.gridArray[x,y].thisSpriteName == SpriteNames.fireFilled)
                    {
                        firePoints += pointsForFullFire;
                        mainBoardGrid.gridArray[x,y].FireExitOK();
                        mainBoardGrid.gridArray[x,y].SetSpriteName(0);
                    }
                }
            }
            else if(iceColumn == hight)
            {
//TODO                //ice column points and animation
                for (int y = 0; y < hight; y++)
                {
                    if(mainBoardGrid.gridArray[x,y].thisSpriteName == SpriteNames.iceFilled)
                    {
                        icePoints += pointsForFullIce;
                        mainBoardGrid.gridArray[x,y].IceExitOK();
                        mainBoardGrid.gridArray[x,y].SetSpriteName(0);
                    }
                }
            }
        }
        //rows
        for (int y = 0; y < hight; y++)
        {
            int fireRow=0, iceRow=0;
            for (int x = 0; x < width; x++)
            {
                if(mainBoardGrid.gridArray[x,y].thisSpriteName == SpriteNames.fireFilled) fireRow++;
                
                if(mainBoardGrid.gridArray[x,y].thisSpriteName == SpriteNames.iceFilled) iceRow++;
            }
            //Debug.Log("Row " + y + "amount fire " + fireRow);
            if(fireRow == width)
            {
//TODO                //fire row points and animation
                for (int x = 0; x < width; x++)
                {
                    if(mainBoardGrid.gridArray[x,y].thisSpriteName == SpriteNames.fireFilled)
                    {
                        firePoints += pointsForFullFire;
                        mainBoardGrid.gridArray[x,y].FireExitOK();
                        mainBoardGrid.gridArray[x,y].SetSpriteName(0);
                    }
                }
            }
            else if(iceRow == width)
            {
//TODO                //ice row points and animation
                for (int x = 0; x < width; x++)
                {
                    if(mainBoardGrid.gridArray[x,y].thisSpriteName == SpriteNames.iceFilled)
                    {
                        icePoints += pointsForFullIce;
                        mainBoardGrid.gridArray[x,y].IceExitOK();
                        mainBoardGrid.gridArray[x,y].SetSpriteName(0);
                    }
                }
            }
        }
        FirePointShow();
        IcePointShow();
        //any remaining cells check for naiboring oposite types and act on it
        CheckElementsInteractions();
        //find out are all showers empty if so generate random puzzle
        allAwaiting = true;
        foreach (var item in allShowers)
        {
            if(!item.awaiting) allAwaiting = false;
        }
        if(allAwaiting) 
        {
           foreach (var item in allShowers)
            {
                item.GetRandomPuzzle();
            } 
        }
       //lets check are there any possible moves left
        CheckForPossibleMoves();
    }
    public void CheckForPossibleMoves()
    {
        bool movePossible = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                
                foreach (var item in allShowers)
                {
                    if(!item.awaiting)
                    {
                        List<bool> checkThisPuzzleList = new List<bool>();
                        for (int i = 0; i < item.width; i++)
                        {
                            if(x+i-2<0 || x+i-2>width)
                            {
                                continue;
                            }
                            else
                            {
                                for (int j = 0; j < item.hight; j++)
                                {
                                    if(y+j-2<0 || y+j-2>hight)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if(item.puzzleGrid.gridArray[i,j].thisSpriteName == SpriteNames.empty)
                                        {
                                            continue;
                                        }
                                        
                                        else
                                        {
                                            if(x+(i-2) >= width || y+(j-2) >= hight)
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                //Debug.Log("x: " + x + ", y: "+y+", i: "+ i + ",j: " + j);
                                                if(mainBoardGrid.gridArray[x+(i-2),y+(j-2)].thisSpriteName == SpriteNames.empty)
                                                {
                                                    checkThisPuzzleList.Add(true);
                                                }
                                                else 
                                                checkThisPuzzleList.Add(false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if(checkThisPuzzleList.Contains(false))
                        {
                            continue;
                        }
                        else
                        {
                            //we have possible next move
                            movePossible = true;
                            Debug.Log("next move");
                            goto gotNextMove;
                        }
                    }
                }
            }
        }
        if(!movePossible)
        {
//TODO            //no possible moves
            Debug.Log("no more moves");
            lostPanel.SetActive(true);
        }
    gotNextMove:;

    }

    private void CheckElementsInteractions()
    {
        List<SpriteGridObject>  iceMeltList = new List<SpriteGridObject>(), 
                                fireDownList = new List<SpriteGridObject>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                if(mainBoardGrid.gridArray[x,y].afiliateElement == AfiliateElement.fire)
                {
                    if(x-1 >= 0 && mainBoardGrid.GetGridObject(x-1,y).afiliateElement == AfiliateElement.ice)
                    iceMeltList.Add(mainBoardGrid.GetGridObject(x-1,y));

                    if(x+1 < width && mainBoardGrid.GetGridObject(x+1,y).afiliateElement == AfiliateElement.ice)
                    iceMeltList.Add(mainBoardGrid.GetGridObject(x+1,y));

                    if(y-1 >= 0 &&mainBoardGrid.GetGridObject(x,y-1).afiliateElement == AfiliateElement.ice)
                    iceMeltList.Add(mainBoardGrid.GetGridObject(x,y-1));
                    
                    if(y+1< hight &&mainBoardGrid.GetGridObject(x,y+1).afiliateElement == AfiliateElement.ice)
                    iceMeltList.Add(mainBoardGrid.GetGridObject(x,y+1));
                }
                else if(mainBoardGrid.gridArray[x,y].afiliateElement == AfiliateElement.ice)
                {
                    if(x-1 >= 0 && mainBoardGrid.GetGridObject(x-1,y).afiliateElement == AfiliateElement.fire)
                    fireDownList.Add(mainBoardGrid.GetGridObject(x-1,y));

                    if(x+1< width && mainBoardGrid.GetGridObject(x+1,y).afiliateElement == AfiliateElement.fire)
                    fireDownList.Add(mainBoardGrid.GetGridObject(x+1,y));

                    if(y-1 >= 0 &&mainBoardGrid.GetGridObject(x,y-1).afiliateElement == AfiliateElement.fire)
                    fireDownList.Add(mainBoardGrid.GetGridObject(x,y-1));
                    
                    if(y+1 < hight &&mainBoardGrid.GetGridObject(x,y+1).afiliateElement == AfiliateElement.fire)
                    fireDownList.Add(mainBoardGrid.GetGridObject(x,y+1));
                }
            }
        }
        if(fireDownList.Count>0)
        {
            foreach (var item in fireDownList)
            {
                firePoints-=pointsForFullFire;
                
                item.FireDown();
            }
            FirePointShow();
        }
        if(iceMeltList.Count>0)
        {
            foreach (var item in iceMeltList)
            {
                icePoints-=pointsForFullIce;
                item.IceMelt();
            }
            IcePointShow();
        }
    }
    private void FirePointShow()
    {
        firePointsText.text = "Fire \n " + firePoints.ToString();
        TotalPointShow();
    }
    private void IcePointShow()
    {
        icePointsText.text = "Ice \n " + icePoints.ToString();
        TotalPointShow();
    }
    private void TotalPointShow()
    {
        totalPoints = (firePoints + icePoints) - Mathf.Abs(firePoints-icePoints);
        totalPointsText.text = "Total \n " + totalPoints.ToString();
        
    }
    public void Restart()
    {
        firePoints = 0;
        icePoints = 0;
        FirePointShow();
        IcePointShow();
        foreach (var item in mainBoardGrid.gridArray)
        {
            item.SetSpriteName(0);
        }
        foreach (var item in allShowers)
        {
            item.GetRandomPuzzle();
        }
        lostPanel.SetActive(false);
    }
}
