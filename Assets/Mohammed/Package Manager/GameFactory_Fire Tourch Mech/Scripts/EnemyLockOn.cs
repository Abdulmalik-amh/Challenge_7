using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyLockOn : MonoBehaviour
{
    public Transform currentTarget;
    Animator anim;

    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform enemyTarget_Locator;

    [Tooltip("StateDrivenMethod for Switching Cameras")]
    [SerializeField] Animator cinemachineAnimator;

    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float noticeZone = 10;
    [SerializeField] float lookAtSmoothing = 2;
    [Tooltip("Angle_Degree")] [SerializeField] float maxNoticeAngle = 60;
    [SerializeField] float crossHair_Scale = 0.1f;

    
    Transform cam;
    bool enemyLocked;
    bool attacking;
    float currentYOffset;
    Vector3 pos;

    [SerializeField] CameraFollow camFollow;
    [SerializeField] Transform lockOnCanvas;
    DefMovement defMovement;

    [Header("Target Camera Settings")]
    [SerializeField] GameObject targetCam;
    [SerializeField] float Cam_R;
    [SerializeField] float Cam_L;
    private CinemachineVirtualCamera targetCamV;
    bool isRight = true;

    [SerializeField, Range(0f, 2f)]
    private float heightPercentage = 0.5f;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI cursorLocationText; // Reference to the TMP Text element

    void Start()
    {
        defMovement = GetComponent<DefMovement>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        lockOnCanvas.gameObject.SetActive(false);
        targetCamV = targetCam.GetComponent<CinemachineVirtualCamera>();
    }

    public void HandleAlEnemylLockOn()
    {
        camFollow.lockedTarget = enemyLocked;
        defMovement.lockMovement = enemyLocked;
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (currentTarget)
            {
                //If there is already a target, Reset.
                ResetTarget();
                return;
            }

            if (currentTarget = ScanNearBy()) FoundTarget(); else ResetTarget();
        }

        if (enemyLocked)
        {
            if (!TargetOnRange())
            {
                ResetTarget();
            }
            else
            {
                LookAtTarget();
                DisplayCursorLocation();
            }
            anim.SetBool("EnemyLocked", true);
        }
        else
        {
            anim.SetBool("EnemyLocked", false);
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isRight)
            {
                targetCamV.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = Cam_L;
                isRight = false;
            }
            else if (!isRight)
            {
                targetCamV.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = Cam_R;
                isRight = true;
            }
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        
    }

    public void DisplayCursorLocation()
    {
        if (currentTarget == null)
        {
            return;
        }

        // Calculate the cursor location relative to the target
        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(currentTarget.position + Vector3.up * currentYOffset);
        Vector3 cursorDirection = Input.mousePosition - targetScreenPosition;

        // Determine the cursor location (Right, Left, Top, Bottom)
        string cursorLocation = "";

        if (Mathf.Abs(cursorDirection.x) > Mathf.Abs(cursorDirection.y))
        {
            cursorLocation = (cursorDirection.x > 0) ? "Right" : "Left";
        }
        else
        {
            cursorLocation = (cursorDirection.y > 0) ? "Top" : "Bottom";
        }

        // Display the cursor location in the TMP Text element
        cursorLocationText.text = "Cursor Location: " + cursorLocation;
    }

    public int GetCursorLocation()
    {
        if (currentTarget == null)
        {
            return -1; // or any default value indicating no target
        }

        // Calculate the cursor location relative to the target
        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(currentTarget.position + Vector3.up * currentYOffset);
        Vector3 cursorDirection = Input.mousePosition - targetScreenPosition;

        // Determine the cursor location (Right: 0, Left: 1, Top: 2, Bottom: 3)
        int cursorLocation = -1;

        if (Mathf.Abs(cursorDirection.x) > Mathf.Abs(cursorDirection.y))
        {
            cursorLocation = (cursorDirection.x > 0) ? 0 : 1;
        }
        else
        {
            cursorLocation = (cursorDirection.y > 0) ? 2 : 3;
        }

        return cursorLocation;
    }


    void FoundTarget()
    {
        lockOnCanvas.gameObject.SetActive(true);
        anim.SetLayerWeight(1, 1);
        anim.SetLayerWeight(2, 1);
        cinemachineAnimator.Play("TargetCamera");
        anim.Play("mixamo_com d3");
        enemyLocked = true;

        // Assuming targetCamV is the CinemachineVirtualCamera component
        if (currentTarget != null)
        {
            // Calculate the distance between the player and the target
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            // You can adjust the virtual camera's FieldOfView based on the distance
            float newFieldOfView = Mathf.Lerp(targetCamV.m_Lens.FieldOfView, CalculateNewFOV(distanceToTarget), Time.deltaTime * 5f);
            targetCamV.m_Lens.FieldOfView = newFieldOfView;
        }
    }

    // Custom method to calculate the new FOV based on distance
    public float CalculateNewFOV(float distance)
    {
        // Define your own logic here to determine the desired FOV based on the distance
        // This is just an example; adjust the formula as needed for your game
        float minFOV = 30f;
        float maxFOV = 60f;
        float maxDistance = 10f;

        // Use a linear interpolation to smoothly adjust FOV
        return Mathf.Lerp(minFOV, maxFOV, Mathf.InverseLerp(0f, maxDistance, distance));
    }


    void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false);
        currentTarget = null;
        enemyLocked = false;
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
        cinemachineAnimator.Play("FollowCamera");
    }


    private Transform ScanNearBy()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        if (nearbyTargets.Length <= 0) return null;

        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Vector3 dir = nearbyTargets[i].transform.position - cam.position;
            dir.y = 0;
            float _angle = Vector3.Angle(cam.forward, dir);

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform;
                closestAngle = _angle;
            }
        }

        if (!closestTarget) return null;

        float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float yOffset = h * heightPercentage;

        currentYOffset = yOffset;

        if (zeroVert_Look && currentYOffset > 1.6f && currentYOffset < 1.6f * 3)
            currentYOffset = 1.6f;

        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);

        if (Blocked(tarPos)) return null;

        return closestTarget;
    }

    bool Blocked(Vector3 t){
        RaycastHit hit;
        if(Physics.Linecast(transform.position + Vector3.up * 0.5f, t, out hit)){
            if(!hit.transform.CompareTag("Player")) return true;
        }
        return false;
    }

    bool TargetOnRange(){
        float dis = (transform.position - pos).magnitude;
        if(dis/2 > noticeZone) return false; else return true;
    }


    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ResetTarget();
            return;
        }

        if (!attacking)
        {
            // Only update the rotation when the player cannot deal damage
            pos = currentTarget.position + new Vector3(0, currentYOffset, 0);
            lockOnCanvas.position = pos;
            lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * crossHair_Scale);

            enemyTarget_Locator.position = pos;
            Vector3 dir = currentTarget.position - transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);   
    }

    public void FreezeTargetLock()
    {
        attacking = true;
    }

    public void UnfreezeTargetLock()
    {
        attacking = false;
    }

    public bool IsTargeting()
    {
        return enemyLocked;
    }
}
