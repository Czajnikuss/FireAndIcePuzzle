using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalionApps;

public class test : MonoBehaviour
{
    public int width, hight;
    public float cellSize;
    
    private Grid<GridObject> firstGrid;

//Debug
    Vector3 mousePos;
    GridObject clickedObject;
    
    void Start()
    {
        firstGrid = new Grid<GridObject>(width, hight, cellSize, transform.position, (Grid<GridObject> g, int x, int y) => new GridObject(x, y, g));
        firstGrid.OnGridValueChanged += firstGrid.Grid_OnValueChanged;
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePos = UtilsClass.GetMouseWorldPosition();
            clickedObject = firstGrid.GetGridObject(mousePos);
            if(clickedObject != null)
            {
                clickedObject.AddValue(5);
            }
        }    
    }
    
    public class GridObject
    {
        public int value, x, y;
        public Grid<GridObject> usedGrid;

        public GridObject(int x, int y, Grid<GridObject> usedGrid)
        {
            this.x = x;
            this.y = y;
            this.usedGrid = usedGrid;

        }

        public void AddValue(int toAdd)
        {
            value += toAdd;
            usedGrid.TriggerOnGridObjectChange(x, y);
            Debug.Log("AddValueCalled");
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    
}
