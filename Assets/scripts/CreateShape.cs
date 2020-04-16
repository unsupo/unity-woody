using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShape : MonoBehaviour
{

    public Vector3 scale = new Vector3(0.03f,0.03f,0.03f);
    public Vector2 offset = new Vector2(0.26f,0.26f);
    public Vector3 pieceOffset = new Vector3(0,-0.32f,0);

    ManageGame gameManager;
    public List<List<Vector2>> shapes;

    GameObject gg;
    // Start is called before the first frame update
    public Sprite sprite;
    Vector2 min = new Vector2(9999f,9999f), max = new Vector2(-9999f,-9999f);

    void Start(){
        initShapes();
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ManageGame>();
        NewShape();
    }

    public void NewShape(){
        gg = new GameObject("piece");
        gg.AddComponent<DragAndDrop>();
        Vector3 center = spawnShape(shapes[Random.Range(0,shapes.Count)]);
        gg.transform.parent = transform;
        gg.transform.position = transform.position-center;
        BoxCollider2D collider = gg.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(max.x-min.x+2*offset.x,max.y-min.y+2*offset.y,0);
        collider.offset = center;
        // if(!gameManager.checkIfPieceIsAllowed(gg))
        //     Debug.Log("Game Over "+gg.transform.parent.name);
    }

    // Update is called once per frame
    void Update(){

    }

    Vector3 spawnShape(List<Vector2> shape){
        min = new Vector2(9999f,9999f); 
        max = new Vector2(-9999f,-9999f);
        Vector3 center = new Vector3();
        foreach (Vector2 v in shape)
            center+=SpawnTile((int)v.x,(int)v.y).transform.position;
        return new Vector3(center.x/(float)shape.Count,center.y/(float)shape.Count,center.z/(float)shape.Count);
    }

    GameObject SpawnTile(int x, int y){
        GameObject g = new GameObject(x + ":" + y);
        // Debug.Log(name+" "+transform.position);
        g.transform.localScale = scale;
        g.transform.parent = gg.transform;
        SpriteRenderer tile = g.AddComponent<SpriteRenderer>();
        tile.sprite = sprite;

        BoxCollider2D collider = g.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(offset.x*2f,offset.y*2f,0);
        // collider.offset = new Vector3(x*offset.x,y*offset.y,0);
        collider.isTrigger = true;
        StickPieceToGrid sptg = g.AddComponent<StickPieceToGrid>();
        sptg.setXY(x,y);
        Rigidbody2D rb = g.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // Debug.Log(rect);
        g.transform.position = transform.position+new Vector3(x*offset.x,y*offset.y,0);
        // size+=new Vector3(x*offset.x,y*offset.y,0);
        min.x = min.x > g.transform.position.x ? g.transform.position.x : min.x;
        min.y = min.y > g.transform.position.y ? g.transform.position.y : min.y;
        max.x = max.x < g.transform.position.x ? g.transform.position.x : max.x;
        max.y = max.y < g.transform.position.y ? g.transform.position.y : max.y;
        return g;
        // Debug.Log("TILE:" + g.transform.position);
        // tile.color = new Color(value, value, value);
    }

    void initShapes(){
        shapes = new List<List<Vector2>>();
        // lines 5l, 4l, 3l, 2l, 1l,  
        int l = 5;
        for(int i = 1; i<=l; i++){
            List<Vector2> ll = new List<Vector2>(),lm = new List<Vector2>();
            for(int j = 0; j<i; j++){
                ll.Add(new Vector2(0,j));
                lm.Add(new Vector2(j,0));
            }
            shapes.Add(ll);
            if(i>1)
                shapes.Add(lm);
        }

        // squares 3x3 and 2x2
        int s = 3;
        for(int i = 2; i<=s; i++){
            List<Vector2> ll = new List<Vector2>();
            for(int j = 0; j<i; j++)
                for(int k = 0; k<i; k++)
                    ll.Add(new Vector2(j,k));
            shapes.Add(ll);
        }

        // L shapes 3x3 and 2x2
        int L = 3;
        for(int i = 2; i<=L; i++){
            List<Vector2> ll1 = new List<Vector2>(), ll2 = new List<Vector2>(), ll3 = new List<Vector2>(), ll4 = new List<Vector2>();
            // x x   x x    x       x
            // x       x    x x   x x
            for(int j = 0; j<i; j++){
                // 1
                for(int k = 0; k<i; k++){
                    if(j>0 && k>0) continue;
                    ll1.Add(new Vector2(j,k));
                }
                // 2
                for(int k = 0; k<i; k++){
                    if(j>0 && k<i-1) continue;
                    ll2.Add(new Vector2(j,k));
                }
                // 3
                for(int k = 0; k<i; k++){
                    if(j<i-1 && k>0) continue;
                    ll3.Add(new Vector2(j,k));
                }
                // 4
                for(int k = 0; k<i; k++){
                    if(j<i-1 && k<i-1) continue;
                    ll4.Add(new Vector2(j,k));
                }
            }
            shapes.Add(ll1);
            shapes.Add(ll2);
            shapes.Add(ll3);
            shapes.Add(ll4);
        }
    }
}
