using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalionApps;

public class PuzzlePieceShower : MonoBehaviour
{
    public MainBoard mainBoard;
    public Grid<SpriteGridObject> puzzleGrid, holderGrid;
    public int width =5, hight=5;
    public float cellSize, holderCellSize;
    public Sprite[] sprites;
    public GameObject gridCellPrefab, holderCellPrefab;
    public Vector3 scaleOfCell, holderScaleOfCell;
    public SpriteNames[,] puzzleLayout = new SpriteNames[5,5];
    public SpriteNames thisPuzzleType;
    private Vector3 startingPoint;
    public bool awaiting = false;

    private void Awake() {
    //debug    
      
        
        
    //debug
    startingPoint = transform.position;    
    //set holder grid
        holderGrid = new Grid<SpriteGridObject>(width, hight, holderCellSize, transform.position + new Vector3(-(width/2f)* holderCellSize,-(hight/2f)*holderCellSize, 0), (Grid<SpriteGridObject> g, int x, int y) => new SpriteGridObject(g, x, y, sprites, holderCellPrefab, holderScaleOfCell));
        holderGrid.SetAllGridElementsAsChildren(this.transform);
        holderGrid.OnGridValueChanged += holderGrid.Grid_OnValueChanged;
        //SetHolderLayout();
        
    //set puzzleGrid
        puzzleGrid = new Grid<SpriteGridObject>(width, hight, cellSize, transform.position + new Vector3(-(width/2f)*cellSize,-(hight/2f)*cellSize, 0), (Grid<SpriteGridObject> g, int x, int y) => new SpriteGridObject(g, x, y, sprites, gridCellPrefab, scaleOfCell));
        puzzleGrid.SetAllGridElementsAsChildren(this.transform);
        puzzleGrid.OnGridValueChanged += puzzleGrid.Grid_OnValueChanged;
        //SetPuzzleLayout();
        PuzzleGridSetActive(false);
    }    
    private void Start() 
    {
        GetRandomPuzzle();
    }    
       

    public void PuzzleGridSetActive(bool active)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                puzzleGrid.GetObjFromHelperArray(x,y).SetActive(active);
            }
        }
        //Debug.Log("puzzle set to " + active);
    }
    public void HolderGridSetActive(bool active)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                holderGrid.GetObjFromHelperArray(x,y).SetActive(active);
            }
        }
       // Debug.Log("holder set to " + active);
    }
    public void SetPuzzleLayout()
    {
        
        for (int y = 0; y < hight; y++)
        {
            for (int x = 0; x < width; x++)
            {
                puzzleGrid.gridArray[x,y].SetSpriteName((int)puzzleLayout[x,y]);
                if(puzzleLayout[x,y] != SpriteNames.empty)thisPuzzleType = puzzleLayout[x,y];
            }
        }
        awaiting = false;
    }
    public void SetPuzzleLayout(SpriteNames[,] puzzleLayout)
    {
        this.puzzleLayout = puzzleLayout;
        SetPuzzleLayout();
    }
    public void SetHolderLayout()
    {
        
        for (int y = 0; y < hight; y++)
        {
            for (int x = 0; x < width; x++)
            {
                holderGrid.gridArray[x,y].SetSpriteName((int)puzzleLayout[x,y]);
                
            }
        }
    }
    public void SetHolderLayout(SpriteNames[,] puzzleLayout)
    {
        this.puzzleLayout = puzzleLayout;
        SetHolderLayout();
    }
    private void OnMouseDown() 
    {
        HolderGridSetActive(false);
        PuzzleGridSetActive(true);
    }
    public void OnMouseDrag() 
    {
        this.gameObject.transform. Translate(Input.GetAxis("Mouse X")/2f, Input.GetAxis("Mouse Y")/2f,0);
    }
    private void OnMouseUp() 
    {
        mainBoard.DropedOnMainGrid(transform.position, this);
    }
    public void BadFit()
    {
        LeanTween.move(this.gameObject, startingPoint, 0.5f).setOnComplete(()=> BackToActiveHolder());
        
    }
    private void BackToActiveHolder()
    {
        PuzzleGridSetActive(false);
        HolderGridSetActive(true);
    }
    public void GoodFit()
    {
        awaiting = true;
        transform.position = startingPoint;
        PuzzleGridSetActive(false);
        HolderGridSetActive(false);
    }
    public void GetRandomPuzzle()
    {
        HolderGridSetActive(true);
        puzzleLayout = PossiblePuzzleShapes.Instance.GeneratePattern();
        SetHolderLayout();
        SetPuzzleLayout();
        
    }
    
}
