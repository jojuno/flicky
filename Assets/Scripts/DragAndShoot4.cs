using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class DragAndShoot4 : MonoBehaviour
{

    [SerializeField]
    private Vector3 mousePressDownPos;
    [SerializeField]
    private Vector3 mouseReleasePos;

    private Rigidbody rb;

    private bool isShoot;

    private float forceMultiplier = 2000f;
    
    LineRenderer lr;
    Plane m_Plane;
    Vector3 startPos;
    Vector3 endPos;
    Camera camera;
    Vector3 hitPointOriginal;
    GameObject dragIndicatorArrow;

    private bool clicked;
    public bool Clicked
    {
        get { return clicked; }
        set { clicked = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_Plane = new Plane(Vector3.up, new Vector3(0, 0.6f, 0));
        camera = Camera.main;
        clicked = false;
    }

    void Shoot(Vector3 Force)
    {
        if (isShoot)
            return;

        GameObject board = GameObject.Find("board");

        GameMaster2 master = board.GetComponent<GameMaster2>();

        rb.AddForce(Force * forceMultiplier);

        isShoot = true;
    }

    IEnumerator SetFlicked()
    {
        yield return new WaitForSeconds(0.1f);
        GameMaster2 gm = GameObject.Find("board").GetComponent<GameMaster2>();
        gm.Flicked = true;
    }

    //tracks event while the mouse is over the object's collider
    private void OnMouseDown()
    {
        GameMaster2 gm = GameObject.Find("board").GetComponent<GameMaster2>();
        if (gm.PieceMoving)
        {
            return;
        }
        if (gm.mode == "battle" && gameObject.GetComponent<Piece>().OwnerId == gm.currentPlayerId && gm.Flicked == false)
        {

            clicked = true;
            //line doesn't draw when object is clicked sometimes
            if (lr == null)
            {
                lr = gameObject.AddComponent<LineRenderer>();
                lr.SetWidth(0.2f, 0.2f);
                lr.material = new Material(Shader.Find("Sprites/Default"));
            }
            lr.enabled = true;
            lr.positionCount = 2;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Initialise the enter variable
            float enter = 1000.0f;

            if (m_Plane.Raycast(ray, out enter))
            {
                hitPointOriginal = ray.GetPoint(enter);

                lr.SetPosition(0, hitPointOriginal);
                mousePressDownPos = hitPointOriginal;
                dragIndicatorArrow = Instantiate(gm.dragIndicatorArrow, mousePressDownPos, Quaternion.AngleAxis(90, Vector3.left));
                
            }
        } else
        {
            clicked = false;
        }
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float enter = 1000.0f;

        if (!clicked)
        {
            return;
        }

        if (m_Plane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            if (lr != null)
            {
                lr.SetPosition(1, hitPoint);
                
                Vector3 lineVector = mousePressDownPos - hitPoint;
                //takes the smaller of the two angles
                float angleRelativeToZ;
                if ((lineVector.x > 0 && lineVector.z > 0) || (lineVector.x > 0 && lineVector.z < 0))
                {
                    angleRelativeToZ = Vector3.Angle(new Vector3(lineVector.x, 0, lineVector.z), Vector3.forward) + 180;
                }
                else
                {
                    //angle starts to decrease from 180 when you're looking at the board from behind
                    angleRelativeToZ = 180 - Vector3.Angle(new Vector3(lineVector.x, 0, lineVector.z), Vector3.forward);
                }
                
                //add -90 to the z rotation to match the rotation in the initial prefab
                Vector3 newRotation = new Vector3(-90, 0, angleRelativeToZ-90);
                dragIndicatorArrow.transform.eulerAngles = newRotation;
                float length = (mousePressDownPos - hitPoint).magnitude;
                float factor = 3;
                Color c = new Color(1, 1.0f - Mathf.Log(length / factor) / 2.8f, 1.0f -Mathf.Log(length / factor) / 2.8f, 1);
                lr.SetColors(c, c);
                Renderer dragIndicatorArrowRenderer = dragIndicatorArrow.GetComponent<Renderer>();
                dragIndicatorArrowRenderer.materials[0].color = c;
            }
        }
    }

    private void OnMouseUp()
    {
        GameMaster2 gm = GameObject.Find("board").GetComponent<GameMaster2>();
        Piece piece = GetComponent<Piece>();
        if (lr == null)
        {
            return;
        }
        
        if (gm.mode == "battle" && piece.OwnerId == gm.currentPlayerId && !gm.Flicked && clicked)
        {
            //delay to check if pieces are moving
            StartCoroutine(SetFlicked());
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter = 1000.0f;

            if (m_Plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                if (lr != null)
                {
                    lr.enabled = false;
                }
                mouseReleasePos = hitPoint;
            }
            Shoot((mousePressDownPos - mouseReleasePos));
            Destroy(dragIndicatorArrow);
            //update turn num
            gm.turnNum++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity == Vector3.zero)
        {
            isShoot = false;
        }
    }
}