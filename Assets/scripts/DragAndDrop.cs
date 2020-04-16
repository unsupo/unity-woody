using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragAndDrop : MonoBehaviour {

    public bool moveAllowed = true, isDragging;
    Collider2D col;

    public Vector3 moveScale = new Vector3(2.7f,2.7f,0f);
    Vector3 initpos;
    Vector2 colliderDecrease = new Vector2(.2f,.2f), originalSize;
    GameObject board;
    private Camera main;
    ManageGame gameManager;
    BoxCollider2D bpos;
    // StickPieceToGrid spg;
    Collider2D last;
    private void Start() {
        initpos = transform.position;
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ManageGame>();
        col = GetComponent<Collider2D>();    
        board = GameObject.Find("GameBoard");
        main = Camera.main;
        bpos = board.transform.GetComponent<BoxCollider2D>();
        originalSize = transform.GetComponent<BoxCollider2D>().size;
    }

    private void OnMouseDown() {
        if(moveAllowed){
            isDragging = true;   
            transform.localScale += moveScale;
            transform.GetComponent<BoxCollider2D>().size -= colliderDecrease;
        }
    }
    // TODO remove game object and instead replace with image of gameobject

    private void OnMouseUp() {
        if(moveAllowed){
            isDragging = false;
            // if(!bpos.bounds.Intersects(col.bounds)){
            // if(!bpos.bounds.Contains(col.bounds.center)){
            if(!(bpos.bounds.Contains(col.bounds.min) && bpos.bounds.Contains(col.bounds.max))){
                cantMove();
            }else {
                bool canMove = true;
                foreach (Transform child in transform)
                    if(child.GetComponent<StickPieceToGrid>().canStickToClosestGrid(child) == null){
                        cantMove();
                        canMove = false;
                        break;
                    }
                if(canMove){
                    int count = 0;
                    foreach (Transform child in transform){
                        child.GetComponent<StickPieceToGrid>().stickToClosestGrid(child);
                        count++;
                    }
                    gameManager.checkForBreaks();
                    transform.parent = board.transform.parent.transform;
                    gameManager.Moved();
                    gameManager.checkForAllowedPieces();
                    gameManager.addPoints(count);
                    moveAllowed = false;
                }
                // transform.Translate(new Vector3(last.transform.position.x-col.offset.x,last.transform.position.y-col.offset.y,transform.position.z));
            }
        }
    }
    public void cantMove(){
        transform.GetComponent<BoxCollider2D>().size = originalSize;
        transform.localScale -= moveScale;
        transform.position = Vector3.Lerp(transform.position,initpos,1);
    }

    private void Update() {
        if(isDragging && moveAllowed){
            Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            mousePos.x -= col.offset.x * moveScale.x * 1f;
            mousePos.y -= col.offset.y * moveScale.y * 1f;
            transform.Translate(new Vector3(mousePos.x,mousePos.y));
            // Debug.Log(board.transform.GetComponent<BoxCollider2D>().bounds.Intersects(col.bounds));
            // Debug.Log(board.transform.GetComponent<BoxCollider2D>().bounds.Contains(col.bounds.center));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        last = other;
        // Debug.Log(other);
        // if(!moveAllowed)
        //     Debug.Log(other);
    }
}