using UnityEngine.Events;
using UnityEngine;
using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    public bool isMove { get; private set; } = true;
    private bool isSprinting => Sprinting && Input.GetKey(sprintKey);
    private bool isJumping => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    [Header("Look Settings")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperlookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerlookLimit = 80.0f;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    int _clickCount = 0;
    public UnityEvent ShowPanel;
    public UnityEvent EndGame;
    public Transform position;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 6.0f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float jumpForce = 4.0f;
    [SerializeField] private bool Sprinting = true;
    [SerializeField] private bool Jumping = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool canUseFootsteps = true;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;


    [Header("Headbob")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobValue = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobValue = 0.05f;
    private float defaultYpos = 0;
    private float timer;


    [Header("Footsteps")]
    [SerializeField] float baseStepSpeed = 0.5f;
    [SerializeField] float sprintStepMultiplier = 0.6f;
    [SerializeField] AudioSource stepSource = default;
    [SerializeField] AudioClip[] steps = default;
    [SerializeField] AudioClip jump = default;
    float footstepTimer = 0;
    float GetCurrentOffset => isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;


    CharacterController characterController;
    public GameObject flashLight;
    bool flashlightOn;

    private Vector3 moveDir;
    private Vector2 currInput;

    float _mouseMovementX = 0;

    [Header("Gun Stuff")]
    public ParticleSystem muzzleFlash;
    public static PlayerMovement instance;
    public CharacterController charCon;
    public Transform firePoint, adsPoint, gunHolder, camTrans, camTarget;
    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currentGun;
    Vector3 gunStartPos;
    public float adsSpeed = 2f;
    float startFOV, targetFOV;
    public float zoomSpeed = 1f;
    Animator anim;
    public static bool _canShoot = true;
    public static bool _playFire = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        defaultYpos = virtualCamera.transform.localPosition.y;
        flashLight.SetActive(false);
        instance = this;
    }

    private void Start()
    {
        startFOV = virtualCamera.m_Lens.FieldOfView;
        targetFOV = startFOV;
        anim = FindObjectOfType<Gun>().GetComponent<Animator>();
    }

    void Update()
    {
        if (ToggleablePanel.AnyVisible != true)
        {
            isMove = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (isMove)
        {
            MouseLook();
            Move();


            if (Jumping)
                Jump();

            if (canUseHeadbob)
                HeadBob();

            if (canUseFootsteps)
                Footsteps();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlightOn == false)
            {
                flashLight.SetActive(true);
                flashlightOn = true;
                FindObjectOfType<AudioManager>().Play("FlashlightOn");
            }
            else if (flashlightOn == true)
            {
                flashLight.SetActive(false);
                flashlightOn = false;
                FindObjectOfType<AudioManager>().Play("FlashlightOff");
            }
        }

        if (activeGun != null && _canShoot == true)
        {
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
                {
                    if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                    {
                        firePoint.LookAt(hit.point);
                    }
                }
                else
                {
                    firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
                }

                FireShot();
            }
        }

        //repeating shots
        if (activeGun != null && Input.GetMouseButton(0) && activeGun.canAutoFire)
        {
            if (activeGun.fireCounter <= 0)
            {
                FireShot();
                _playFire = true;
            }
            else
            {
                _playFire = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchGun();
        }

        if (Input.GetMouseButtonDown(1))
        {
            PlayerMovement.instance.ZoomIn(activeGun.zoomAmount);
        }

        if (Input.GetMouseButton(1))
        {
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
        }
        else
        {
            gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPos, adsSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(1))
        {
            PlayerMovement.instance.ZoomOut();
        }
    }

    void FixedUpdate()
    {
        if (ToggleablePanel.AnyVisible)
        {
            isMove = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void LateUpdate()
    {
        transform.position = camTarget.position;
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    public void ZoomIn(float newZoom) => targetFOV = newZoom;

    public void ZoomOut() => targetFOV = startFOV;

    public void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {

            activeGun.currentAmmo--;

            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);

            activeGun.fireCounter = activeGun.fireRate;

            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

            muzzleFlash.Play();
            FindObjectOfType<AudioManager>().Play("GunShot");
        }
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);

        currentGun++;

        if (currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        firePoint.position = activeGun.firepoint.position;
    }

    public void AddGun(string gunToAdd)
    {
        bool gunUnlocked = false;

        if (unlockableGuns.Count > 0)
        {
            for (int i = 0; i < unlockableGuns.Count; i++)
            {
                if (unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;

                    allGuns.Add(unlockableGuns[i]);

                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                }
            }

        }

        if (gunUnlocked)
        {
            currentGun = allGuns.Count - 2;
            SwitchGun();
        }
    }

    private void MouseLook()
    {
        _mouseMovementX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        _mouseMovementX = Mathf.Clamp(_mouseMovementX, -upperlookLimit, lowerlookLimit);
        virtualCamera.transform.localRotation = Quaternion.Euler(_mouseMovementX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

    }
    private void Jump()
    {
        if (isJumping)
        {
            moveDir.y = jumpForce;
            //stepSource.PlayOneShot(jump);

        }
    }
    private void HeadBob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDir.x) > 0.1f || Mathf.Abs(moveDir.z) > 0.1f)
        {
            timer += Time.deltaTime * (isSprinting ? sprintBobSpeed : walkBobSpeed);
            virtualCamera.transform.localPosition = new Vector3(
                virtualCamera.transform.localPosition.x, defaultYpos + Mathf.Sin(timer) * (isSprinting ? sprintBobValue : walkBobValue), virtualCamera.transform.localPosition.z);
        }
    }

    private void Footsteps()
    {
        if (!characterController.isGrounded) return;
        if (currInput == Vector2.zero) return;
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(virtualCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
                switch (hit.collider.tag)
                {
                    case "Grass":
                        stepSource.PlayOneShot(steps[Random.Range(0, steps.Length - 1)]);
                        break;
                }
            footstepTimer = GetCurrentOffset;
        }
    }
    private void Move()
    {
        if (characterController.isGrounded)
            currInput = new Vector2((isSprinting ? sprintSpeed : moveSpeed) * Input.GetAxis("Vertical"), (isSprinting ? sprintSpeed : moveSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDir.y;
        if (characterController.isGrounded)
            moveDir = (transform.TransformDirection(Vector3.forward) * currInput.x) + (transform.TransformDirection(Vector3.right) * currInput.y);
        moveDir.y = moveDirectionY;

        if (!characterController.isGrounded)
            moveDir.y -= gravity * Time.deltaTime;
        characterController.Move(moveDir * Time.deltaTime);
    }
}


