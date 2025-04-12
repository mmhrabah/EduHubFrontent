// Marmoset Skyshop
// Copyright 2013 Marmoset LLC
// http://marmoset.co
// version 7.5.2018

using UnityEngine;
using System.Collections;


public class CameraMovementOS : MonoBehaviour
{
    [Space(10F)]
    [Header("::: Speed Control -------")]
    [Space(15F)]
    public float rotationSpeed = 5f;
    [Range(.2f, 10)]
    public float rotateSmoothing = 5f;
    [Space(15F)]
    public float PanSpeed = 2.0f;
    [Range(.1f, 10)]
    public float PanSmoothing = 5f;
    [Space(15F)]
    public float zoomSpeed = 50;
    [Range(1, 10)]
    public float zoomSmooth = 5;
    [Space(20F)]
    [Range(.5f, 5f)]
    public float focusSmoothing = 5f;
    [Space(20F)]
    [Header("::: Pan Limit ----------- ")]
    [Space(10F)]
    public bool allowPanning = true;
    public bool activateBoundPan = false;

    public float panBoundMinX = 5f;
    public float panBoundMaxX = 5f;

    public float panBoundMinY = 5f;
    public float panBoundMaxY = 5f;

    public float panBoundMinZ = 5f;
    public float panBoundMaxZ = 5f;
    [Space(10F)]
    [Header("::: Rotation Limit ----------")]
    [Space(10F)]
    public bool allowRotation = true;
    public float rotationBoundMinX = -360f;
    public float rotationBoundMaxX = 360f;

    public float rotationBoundMinY = 0f;
    public float rotationBoundMaxY = 90f;

    [HideInInspector]
    public float rotateSmoothingFocus = .7f;
    [Space(10F)]
    [Header("::: Zoom Limit -----------")]
    [Space(10F)]
    public float zMinLimit = 15;
    public float zMaxLimit = 30;
    [HideInInspector]
    public float zMinLimitTemp;
    [HideInInspector]
    public float zMaxLimitTemp;
    [Space(10F)]
    public float StartupPosition = 5;
    [Space(10F)]
    [Header("--- Offset ---")]
    [Space(10F)]
    [HideInInspector]
    public float zOffset = 5;
    [Space(10F)]
    public Transform pivotTransform;
    [HideInInspector]
    public float offsetRotataionY = 25;
    [HideInInspector]
    public float offsetRotataionX = 25;
    [Space(10F)]
    [HideInInspector]
    public float smoothTime = 3.0f;
    [HideInInspector]
    public AnimationCurve pitchCurve;


    [HideInInspector]
    public Rect paramInputBounds = new Rect(0, 0, 1, 1);
    [Space(20F)]
    [HideInInspector]
    public bool usePivotPoint;
    [HideInInspector]
    public Vector3 pivotPoint = new Vector3(0, 2, 0);



    [Space(20F)]
    [HideInInspector]
    public bool start = false;

    [Space(20F)]

    //	public float EndstartPostion = 5;
    [Space(20F)]
    [HideInInspector]
    public Vector3 targetLookAtOrigin;
    [HideInInspector]
    public Vector3 targetPosi;

    Quaternion targetRota;
    [HideInInspector]
    public bool focus;
    [HideInInspector]
    public bool startFocus;


    [Space(20F)]



    private Vector3 velocity = Vector3.zero;
    private Vector2 euler;

    private Quaternion targetRot;
    private Vector3 targetLookAt;
    private float targetDist;
    private Vector3 distanceVec = new Vector3(0, 0, 0);
    [HideInInspector]
    public Transform target;
    private Rect inputBounds;

#if UNITY_IPHONE || UNITY_ANDROID
	private bool firstTouch = true;
#endif

    void first()

    {
        offsetRotataionX = transform.rotation.eulerAngles.x;
        offsetRotataionY = transform.rotation.eulerAngles.y;
        if (pivotTransform != null)
        {
            targetLookAtOrigin = pivotTransform.position;
        }
        zMinLimitTemp = zMinLimit;
        zMaxLimitTemp = zMaxLimit;
        distanceVec = new Vector3(0, 0, StartupPosition);
        start = true;
    }
    public void Awake()
    {
        first();
    }

    CameraMovementOS thisScript;
    public void Start()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;


        target = pivotTransform;

        targetRot = transform.rotation;
        targetLookAt = target.position;
#if UNITY_IPHONE || UNITY_ANDROID
		firstTouch = true;
#endif



    }

    private float rotX, rotY;
    public void Update()
    {
        //NOTE: mouse coordinates have a bottom-left origin, camera top-left

        if (target)
        {

            float dx = Input.GetAxis("Mouse X");

            float dy = Input.GetAxis("Mouse Y");


#if UNITY_IPHONE || UNITY_ANDROID
			if(Input.multiTouchEnabled) {
				if(Input.touchCount > 0) {
					//touch-down detection. kekeke.
					if(!firstTouch) {
						dx += Input.GetTouch(0).deltaPosition.x * 0.01f;
						dy += Input.GetTouch(0).deltaPosition.y * 0.01f;
					}
					firstTouch = false;
				} else {
					firstTouch = true;
				}
			}
#endif
            bool click1 = Input.GetMouseButton(0) || Input.touchCount == 1;
            bool click2 = Input.GetMouseButton(1) || Input.touchCount == 2;
            bool click3 = Input.GetMouseButton(2) || Input.touchCount == 3;
            bool click4 = Input.touchCount >= 4;
            bool rotInput = click1;
            bool skyInput = click4 || click1 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
            bool panInput = click3 || click1 && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
            bool zoomInput = click2;


            if (panInput && allowPanning)
            {

                dx = dx * PanSpeed * 0.005f * targetDist;
                dy = dy * PanSpeed * 0.005f * targetDist;

                targetLookAt -= transform.up * dy + transform.right * dx;
                if (activateBoundPan)
                {
                    targetLookAt.x = Mathf.Clamp(targetLookAt.x, targetLookAtOrigin.x - panBoundMinX, targetLookAtOrigin.x + panBoundMaxX);
                    targetLookAt.y = Mathf.Clamp(targetLookAt.y, targetLookAtOrigin.y - panBoundMinY, targetLookAtOrigin.y + panBoundMaxY);
                    targetLookAt.z = Mathf.Clamp(targetLookAt.z, targetLookAtOrigin.z - panBoundMinZ, targetLookAtOrigin.z + panBoundMaxZ);

                }
            }
            else if (zoomInput)
            {
                dy = dy * zoomSpeed * 0.005f * targetDist;
                targetDist += dy;
                targetDist = Mathf.Max(0.1f, targetDist);
            }
            else if (rotInput && allowRotation)
            {
                rotX = Input.GetAxis("Mouse X") * rotationSpeed;

                rotY = Input.GetAxis("Mouse Y") * rotationSpeed;


            }
            else if (focus)
            {


                targetRot = Quaternion.Slerp(targetRot, targetRota, Time.fixedTime * rotateSmoothingFocus);
                targetLookAt = Vector3.Lerp(targetLookAt, targetPosi, Time.fixedTime * focusSmoothing);
                startFocus = true;
                if (VecEqual(targetLookAt, targetPosi))
                {
                    focus = false;

                }

            }
            //	targetDist = Mathf.Max(0.1f,targetDist);

        }
    }
    private bool currentlyMovingFocus = false;


    bool quatEq(Quaternion q1, Quaternion q2)
    {
        if (Mathf.Abs((Mathf.Abs(q1.x) - Mathf.Abs(q2.x))) < .0001f
            &&
            Mathf.Abs((Mathf.Abs(q1.y) - Mathf.Abs(q2.y))) < .0001f
            &&
            Mathf.Abs((Mathf.Abs(q1.z) - Mathf.Abs(q2.z))) < .0001f
            &&
            Mathf.Abs((Mathf.Abs(q1.w) - Mathf.Abs(q2.w))) < .0001f)
            return true;
        return false;

    }

    bool VecEqual(Vector3 v1, Vector3 v2)
    {

        if (
            Mathf.Abs(v1.x - v2.x) < .0001f
            &&
            Mathf.Abs(v1.y - v2.y) < .0001f
            &&
            Mathf.Abs(v1.z - v2.z) < .0001f
            )
            return true;
        return false;
    }

    public void FixedUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            startFocus = false;
        }
        if ((quatEq(transform.rotation, targetRot) && VecEqual(targetPosi, transform.position)) || Mathf.Abs( rotX) > 0 ||Mathf.Abs(rotY) > 0)
        {

            currentlyMovingFocus = false;
        }
        rotX = Mathf.Lerp(rotX, 0, 1f / (rotateSmoothing * 10f));

        rotY = Mathf.Lerp(rotY, 0, 1f / (rotateSmoothing * 10f));

        //LIMIT rotation:


        euler.x += rotX;
        euler.y -= rotY;
        euler.y = ClampAngle(euler.y, rotationBoundMinY, rotationBoundMaxY);
        //new update
        euler.x = ClampAngle(euler.x, rotationBoundMinX, rotationBoundMaxX);
        if (!currentlyMovingFocus)
            targetRot = Quaternion.Euler(euler.y + offsetRotataionX, euler.x + offsetRotataionY, 0);
        // transform.rotation = targetRot;

        if (targetDist < zMinLimit)
        {
            targetDist = zMinLimit;
        }

        if (targetDist > zMaxLimit)
        {
            targetDist = zMaxLimit;
        }

        targetDist -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 0.5f;
        //distance = zoomSmooth * targetDist + (1 - zoomSmooth) * distance;
        // transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRot, Time.fixedDeltaTime * rotateSmoothing);

        var smoothing = PanSmoothing;
        if (currentlyMovingFocus)
        {
            // print("ssssssss");
            smoothing = focusSmoothing;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotateSmoothing * 0.5f);
        }
        else
            transform.rotation = targetRot;



        float slope = 1f * (5 - 20) / (10 - 1);
        float ms = 20 + slope * (smoothing - 1);

        //  target.position = Vector3.LerpUnclamped(target.position, targetLookAt, Time.fixedDeltaTime * ms);
        // if (currentlyMovingFocus)
        //


        //for focus 
        target.position = Vector3.Lerp(target.position, targetLookAt, Time.fixedDeltaTime * 8.5f);

        //   else
        // target.position = Vector3.LerpUnclamped(target.position, targetLookAt, Time.fixedDeltaTime * ms);

        float zs = 20 + slope * (zoomSmooth - 1); //zoom
                                                  //zoom limit
        if (start)
        {
            transform.position = new Vector3(target.position.x, target.position.y, StartupPosition);

            StartCoroutine(flyStart());
        }
        if (startFocus)
        {

            StartCoroutine(focusAndZoom());

        }
        else
            distanceVec.z = Mathf.Lerp(distanceVec.z, Mathf.Clamp(targetDist, zMinLimit, zMaxLimit), Time.fixedDeltaTime * zs);
        transform.position = target.position - transform.rotation * distanceVec;

    }

    public void gotoTarget(Transform _target)
    {
        currentlyMovingFocus = true;
        euler.x = euler.y = 0;
        ///   targetRot=Quaternion.identity;

        //   float targetRotX = pitchCurve.Evaluate(t);
        // targetRota = Quaternion.Euler(targetRotX, offsetRotataionY, 0.0f);

        targetRota = Quaternion.Euler(offsetRotataionX, offsetRotataionY, 0.0f);


        Vector3 offset = new Vector3(0.0f, 3, 1);
        //     Debug.Log(offset);
        //  print(_target.position);
        targetPosi = _target.position;// - targetRot * offset;
                                      // Debug.Log(targetPosi);
        targetLookAtOrigin = new Vector3(targetPosi.x, targetPosi.y, targetPosi.z);
        //StartCoroutine(fly(targetLookAt, targetRot));
        focus = true;

    }


    static float ClampAngle(float angle, float min, float max)
    {
        if (angle > -.0005 && angle < .0005)
        {
            angle = 0;
        }
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        var a = Mathf.Clamp(angle, min, max);


        if (angle < 0)
        {
            //  print(a);
        }
        return a;
    }
    Vector3 startingPos;
    Quaternion startingRot;
    IEnumerator flyStart()
    {
        if (!start) yield return null;
        //  Debug.Log("flyStart" +zMaxLimit);

        zMinLimit = StartupPosition;
        distanceVec.z = Mathf.Lerp(distanceVec.z, ClampAngle(targetDist, zMinLimit, zMaxLimit), Time.deltaTime * PanSmoothing);
        yield return new WaitForSeconds(1f);
        zMinLimit = zMinLimitTemp;
        start = false;


    }
    IEnumerator focusAndZoom()
    {
        if (!startFocus) yield return null;
        //  zMinLimit = zOffset;
        // zMaxLimit = zOffset;
        var v = zMinLimit + zOffset;
        if (v > zMaxLimit)
        {
            v = zMaxLimit;
            zOffset = zMaxLimit;
        }
        distanceVec.z = Mathf.Lerp(distanceVec.z, ClampAngle(0, v, zMaxLimit), Time.deltaTime * focusSmoothing);
        yield return new WaitForSeconds(.02f);
        //   transform.position = target.position - transform.rotation * distanceVec;
        //zMinLimit = zMinLimitTemp;
        //zMaxLimit = zMaxLimitTemp;
        //startFocus = false;
        var a = ClampAngle(targetDist, zMinLimit, zMaxLimit);
        if (Mathf.Abs(distanceVec.z - a) < .001f)
        {
            startFocus = false;
        }


    }


}
