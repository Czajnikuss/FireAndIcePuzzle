using System;
using UnityEngine;
using TalionApps;

[Serializable]
public class Grid<TGridObject>
{
    public int hight, width;
    public float cellSize;
    public Vector3 originPoint;
    public TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    private GameObject[,] gameObjectsArray;
    public bool showDebug = false;

    public event EventHandler<OnGridValueChangedArgs> OnGridValueChanged;
    public class OnGridValueChangedArgs : EventArgs
    {
        public int x;
        public int y;
        public OnGridValueChangedArgs ( int x, int y)
        {
            this.x=x;
            this.y=y;
        }
    }


    public Grid (int width, int hight, float cellSize, Vector3 originPoint, Func<Grid<TGridObject>, int, int, TGridObject> constructGridObject)
    {
        this.hight = hight;
        this.width = width;
        this.cellSize = cellSize;
        if(originPoint == Vector3.zero)
        {
            this.originPoint = new Vector3(-(width/2f)* cellSize, -(hight/2f)*cellSize, 0);
        }
        else this.originPoint = originPoint;
        
        gridArray = new TGridObject[width, hight];
        gameObjectsArray = new GameObject[width, hight];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x,y] = constructGridObject(this, x, y);
            }
        }
        debugTextArray = new TextMesh[width, hight];
        CreateDebug();
        
    }
    public void CreateDebug()
    {
    if(showDebug)
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x,y] = UtilsClass.CreateWorldText(gridArray[x,y]?.ToString(),null,GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 10, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 10);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y +1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x +1, y), Color.white, 100f);
                
            }
        }
                
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, hight), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(0, hight), GetWorldPosition(width, hight), Color.white, 100f);
    }
    
    }
    public void UpdateDebug(int x, int y)
    {
        if(showDebug)
        {
            debugTextArray[x, y].text = gridArray[x,y].ToString();
        }
    }
    public void Grid_OnValueChanged(object sender, OnGridValueChangedArgs args)
    {
       
       UpdateDebug(args.x, args.y);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPoint;
    }
    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - originPoint).x / cellSize);
        y = Mathf.FloorToInt((worldPos - originPoint).y / cellSize);
        
    }
    public void TriggerOnGridObjectChange(int x, int y)
    {
         if(OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedArgs( x, y));
           
    }
    public void SetGridObject(int x, int y, TGridObject value)
    //
    //Sets value of grid object given x and y
    {
        if(x>=0 && y>=0 && x<=width && y<= hight)
        {
            gridArray[x,y] = value;
            debugTextArray[x, y].text = value.ToString();
            if(OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedArgs( x, y));
            
        }
    }
    public void SetGridObject(Vector3  worldPos, TGridObject value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetGridObject(x, y, value);
    }
    

    public TGridObject GetGridObject(int x, int y)
    {
        if(x>=0 && y>=0 && x<=width && y<= hight)
        {
            return gridArray[x,y];
        }
        else
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetGridObject(Vector3  worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetGridObject(x, y);
    }
    public void AddObjectToHelperArray(int x, int y, GameObject objToAdd)
    {
        gameObjectsArray[x,y] = objToAdd;
    }
    public GameObject GetObjFromHelperArray (int x, int y)
    {
        return gameObjectsArray[x, y];
    }
    public void SetAllGridElementsAsChildren(Transform parentTransform)
    {
        foreach (var item in gameObjectsArray)
        {
            item.transform.SetParent(parentTransform);
        }
    }

}
