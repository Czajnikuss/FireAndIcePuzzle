using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SpriteNames 
    {
        empty,
        fireFilled,
        iceFilled
        
    }
public enum AfiliateElement
{
    nothing,
    fire,
    ice
}
[System.Serializable]
public class SpriteGridObject 
{
    private int x, y;
    private Grid<SpriteGridObject> grid;
    public Sprite[] sprites;
    public Vector3 scaleOfCell;
    
    

    public SpriteNames thisSpriteName;
    SpriteRenderer spriteRenderer;
    GameObject tempObject;
    public AfiliateElement afiliateElement;
    
    public SpriteGridObject(Grid<SpriteGridObject> grid, int x, int y, Sprite[] sprites, GameObject gridCellPrefab, Vector3 scaleOfCell)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        this.sprites = sprites;
        this.scaleOfCell = scaleOfCell;
        

        tempObject = new GameObject (x + "," + y, typeof(SpriteRenderer));
        grid.AddObjectToHelperArray(x, y, tempObject);
        Transform transform = tempObject.GetComponent<Transform>();
        transform.position = grid.GetWorldPosition(x,y) + new Vector3(grid.cellSize, grid.cellSize) * 0.5f;
        
        spriteRenderer = tempObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        thisSpriteName = SpriteNames.empty;
        
        transform.localScale = scaleOfCell;
    }


    
   
    public void SetSpriteName(int index)
    {
        spriteRenderer.sprite = sprites[index];
        thisSpriteName = (SpriteNames)index;
        //cell afifliaton setting    
            if(thisSpriteName == SpriteNames.empty) afiliateElement = AfiliateElement.nothing;
            //fire afiliation
            else if(thisSpriteName==SpriteNames.fireFilled)
            afiliateElement = AfiliateElement.fire;
            //ice afiliation
            else if(thisSpriteName==SpriteNames.iceFilled)
            afiliateElement = AfiliateElement.ice;
        
        grid.gridArray[x,y] = this;
        grid.TriggerOnGridObjectChange(x, y);
    }

    public override string ToString()
    {
        return thisSpriteName.ToString();
    }
    
   
    
#region "Empty"
    
    
#endregion

#region "Fire"
    
    public void FireExitOK()
    {

    }
    public void FireDown()
    {
        
        SetSpriteName(0);
    }


#endregion
#region "Ice"

    public void IceExitOK()
    {

    }
    public void IceMelt()
    {
        SetSpriteName(0);
    }


#endregion

}
