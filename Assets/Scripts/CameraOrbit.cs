using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    public Vector3 _LocalRotation; //initializes to (0, 0, 0)
    public float _CameraDistance = 10f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 1f;
    public float OrbitSpeed = 10f;
    public float ScrollSpeed = 3f;
    public float translateSpeed = 1f;
    public Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
        newPosition = _XForm_Parent.position;
        //get current rotation from object

        //set camera's rotation to that rotation
        //must convert rotation which is in quaternion to a vector to apply in the "Lerp" function.
        _LocalRotation = new Vector3(_XForm_Parent.rotation.eulerAngles.y, _XForm_Parent.rotation.eulerAngles.x, 0);
        //set camera's rotation same as the object's rotation
        _XForm_Camera.rotation = _XForm_Parent.rotation;
        //set camera "_CameraDistance" away from it
        _XForm_Camera.position = _XForm_Parent.position - _XForm_Parent.forward * _CameraDistance;
    }

    // Late Update is called after Update.
    void LateUpdate()
    {
        
        if (Input.GetMouseButton(1))
        {
            if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                //clamp the y rotation at horizon and not flipping over at the top.
                _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0f, 90f);

            }
        }

        //actual camera rig transformation
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitSpeed);

        if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollSpeed));
        }

        //scrolling input from our mouse scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

            //control camera zoom by distance
            ScrollAmount *= (this._CameraDistance * 0.3f);

            this._CameraDistance += ScrollAmount * -1f;

            //sets limits on how far the camera can move.
            this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 100f);
        }
        
    }
}