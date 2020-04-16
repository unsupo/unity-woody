using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPieceToGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canMove = false;
    public int x,y;

    List<List<GameObject>> gameObjects = new List<List<GameObject>>();

    void Start(){
        gameObjects = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ManageGame>().GetGameObjects();
    }

    // Update is called once per frame
    void Update(){
        
        
    }

    public void setXY(int x, int y){
        this.x = x;
        this.y = y;
    }
    public GameObject canStickToClosestGrid(Transform child){
        GameObject minpos = null;
        float min = 999999;
        foreach(List<GameObject> gos in gameObjects){
            foreach(GameObject go in gos){
                float d = Vector3.Distance(child.position,go.transform.position);
                if(min > d){
                    min = d;
                    minpos = go;
                }
            }
        }
        if(minpos.GetComponent<GridPiece>().isPlaced())
            return null;
        return minpos;
    }
    public void stickToGrid(Transform child, GameObject gridPiece){
        child.position = new Vector3(gridPiece.transform.position.x,gridPiece.transform.position.y,10);
        gridPiece.GetComponent<GridPiece>().place(true);
    }

    public void stickToClosestGrid(Transform child){
        GameObject minpos = null;
        float min = 999999;
        foreach(List<GameObject> gos in gameObjects){
            foreach(GameObject go in gos){
                float d = Vector3.Distance(child.position,go.transform.position);
                if(min > d){
                    min = d;
                    minpos = go;
                }
            }
        }
        if(minpos.GetComponent<GridPiece>().isPlaced())
            return;
        // Debug.Log(child+","+minpos);
        child.position = new Vector3(minpos.transform.position.x,minpos.transform.position.y,10);
        minpos.GetComponent<GridPiece>().place(true);
        minpos.GetComponent<GridPiece>().setPlacedGameObject(transform.gameObject);
    }

    public void setCanMove(bool v){
        canMove = v;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Worked: "+other);
        // other.name
        // if(canMove)
        //     transform.position = new Vector3(other.transform.position.x, other.transform.position.y);
    }
}
