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
public class SpriteGridObject :MonoBehaviour
{
    private int x, y;
    private Grid<SpriteGridObject> grid;
    public Sprite[] sprites;
    public Vector3 scaleOfCell;
    
    

    public SpriteNames thisSpriteName;
    SpriteRenderer spriteRenderer;
    GameObject tempObject;
    public AfiliateElement afiliateElement;
   [SerializeField] private Animation cellAnimation;
    
    private ParticleSystem sparks;
    
    public SpriteGridObject(Grid<SpriteGridObject> grid, int x, int y, Sprite[] sprites, GameObject gridCellPrefab, Vector3 scaleOfCell)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        this.sprites = sprites;
        this.scaleOfCell = scaleOfCell;
        
        
        tempObject = Instantiate(gridCellPrefab);
        tempObject.name = x + "," + y;
        cellAnimation = tempObject.GetComponent<Animation>();
        GameObject sparksObject = tempObject.transform.Find("Sparks08").gameObject;
        sparks = sparksObject?.GetComponent<ParticleSystem>();
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
            if(thisSpriteName == SpriteNames.empty) 
            {
                afiliateElement = AfiliateElement.nothing;
                sparks.Stop(true);
            }
            //fire afiliation
            else if(thisSpriteName==SpriteNames.fireFilled)
            afiliateElement = AfiliateElement.fire;
            //ice afiliation
            else if(thisSpriteName==SpriteNames.iceFilled)
            afiliateElement = AfiliateElement.ice;
        
        grid.gridArray[x,y] = this;
        grid.TriggerOnGridObjectChange(x, y);
    }
    public void playFadeInAnim()
    {
        if(afiliateElement == AfiliateElement.fire)
        {
            spriteRenderer.material = AssetsStorage.Instance.fireFadeInMaterial;
            sparks.Play();
            //Debug.Log("anim set to fire");
        }
        else if(afiliateElement == AfiliateElement.ice)
        {
            spriteRenderer.material = AssetsStorage.Instance.iceFadeInMaterial;
            sparks.Play();
           //Debug.Log("Anim set to ice");
        }
//        Debug.Log("AnimPlay");       
        cellAnimation.Play();
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
