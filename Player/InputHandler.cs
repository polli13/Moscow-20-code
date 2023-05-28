using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private float vertical;
    private float horizontal;

    private static float delta;
    public static float Delta
    {
        get { return delta; }
    }

    private bool l_shift;
    private bool space;

    private bool tab;
    private bool esc;
    private bool E;

    [SerializeField]
    private bool pauseActive = false;
    private bool inventoryActive = false;

    [SerializeField]
    private bool isWalking = false;
    [SerializeField]
    private bool canRun = true;
    [SerializeField]
    private bool canControls = true;

    private PlayerController controller;
    private CameraFolllow camFollow;
    private DogAI dog;
    private MenuUI ui;

    public static InputHandler m_InputHandler;

    private void Awake()
    {
        if (m_InputHandler == null)
            m_InputHandler = this;

        controller = FindObjectOfType<PlayerController>();
        controller.Init();
        camFollow = FindObjectOfType<CameraFolllow>();
        dog = FindObjectOfType<DogAI>();

        ui = FindObjectOfType<MenuUI>();
    }

    public void FixedUpdate()
    {
        delta = Time.fixedDeltaTime;
      
        if (!canControls || pauseActive)
        {
            delta = 0;
            return;
        }

        GetInputFixedUpdate();

        controller.inputs.vertical = vertical;
        controller.inputs.horizontal = horizontal;
        controller.FixedUpdateStates(delta);

        camFollow.FixedTick(delta);
    }

    private void GetInputFixedUpdate()
    {
        vertical = Input.GetAxisRaw(StaticStrings.vertical);
        horizontal = Input.GetAxisRaw(StaticStrings.horizontal);
    }

    public void Update()
    {
        if (!canControls) return;

        GetInputUpdate();

        delta = Time.deltaTime;

        if (pauseActive) delta = 0;

        UpdateStates();
        controller.UpdateStates(delta);
    }

    private void GetInputUpdate()
    {
        l_shift = Input.GetButton(StaticStrings.LShift);
        space = Input.GetButtonDown(StaticStrings.Jump);

        tab = Input.GetKeyDown(KeyCode.Tab);
        esc = Input.GetKeyDown(KeyCode.Escape);
        E = Input.GetKeyDown(KeyCode.E);
    }

    private void UpdateStates()
    {
        if (esc)
        {
            pauseActive = !pauseActive;
            inventoryActive = false;
            ui.OpenCloseDiary(!pauseActive);
            ui.OpenCloseMenu(pauseActive);
        }

        if (tab)
        {
            if (pauseActive) return;
            inventoryActive = !inventoryActive;
            ui.OpenCloseDiary(inventoryActive);
        }

        controller.states.isWalking = isWalking && controller.CheckVelocityRun();

        //SPRINT
        if(canRun)
            controller.states.isRunning = l_shift && controller.CheckVelocityRun() && !controller.states.isInAction && controller.states.onGround;

        //Interaction
        if (E)
        {
            if (ExamineSystem.m_ExamineSystem.currentPickUp != null)
            {
                ExamineSystem.m_ExamineSystem.Examine();
            }
        }
    }

    public void PauseGame(bool _pause)
    {
        Time.timeScale = _pause ? 0 : 1;
    }
}
