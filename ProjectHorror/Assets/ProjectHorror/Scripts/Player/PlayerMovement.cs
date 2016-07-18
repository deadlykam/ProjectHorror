using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public Camera cam; // The main camera
    public Transform light; // The torch light
    public float rayRange = 0; // Range of the ray
    public float speed_Walk = 1f; // Walking speed
    public float speed_Run = 1f; // Running speed
    public float speed_Turn = 1f; // Turning speed
    public float ver_Min = 0; // Minimum value of vertical rotation of the player/pivot
    public float ver_Max = 0; // Maximum value of vertical rotation of the player/pivot
    public float jump_Force = 0; // Force required for jump

    public LayerMask interactive; // Interactive layer.

    // public Text message; // <-- Will require later for showing messages on screen

    private GameObject pivot; // The pivot game object which contains player models
    private Player_Info pi; // Player_Info instance required for other calculations

    public float Debug_v_Rot = 0; // <-- For debug purpose only.

    // Use this for initialization
    void Start () {
        //Cursor.visible = false;

        // Initializing the pivot game object
        pivot = transform.Find("Models").gameObject.transform.Find("Pivot").gameObject;

        // Initializing the Player_Info instance
        pi = GetComponent<Player_Info>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Turn();
        Jump();
        ToggleTorch();
        CastRay();
        QuitGame();

	}

    /// <summary>
    /// This method is used to find interactive objects. Upon collision will give message
    /// about the object and when pressed the action key will take that particular action.
    /// **Not Finished Under Construction**
    /// </summary>
    void CastRay()
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayRange, Color.green);

        if (Physics.Raycast(ray, out hit, rayRange, interactive))
        {
            if (hit.transform.CompareTag("Switch"))
            {
                // message.text = "Press to open door."; // <-- Will require later for showing messages on screen

                if (Input.GetButtonUp("Action"))
                {
                    // hit.transform.GetComponent<SwitchControl>().Action();
                }
            }

            // TODO: This part is for later use. Just copied pasted it from another project as snippet.
            /*
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactive_Ground"))
            {
                
                //Debug.Log("Left Diagonal hit " + hit.transform.gameObject.name);
                if (hit.transform.CompareTag("Rocket"))
                {
                    phc.message.text = hit.transform.GetComponent<RocketControl>().message;

                    if (Input.GetButtonUp("Action"))
                    {
                        hit.transform.GetComponent<RocketControl>().setLaunch();
                }
            }*/
        }
        else
        {
            // message.text = ""; // <-- Will require later for showing messages on screen
        }
    }
    
    /// <summary>
    /// This method turns the torch on and off.
    /// </summary>
    void ToggleTorch()
    {
        if (Input.GetButtonUp("ToggleFlash"))
        {
            // Condition for turning off the torch light
            // when activeSelf is true and vise versa.
            if (light.gameObject.activeSelf)
            {
                light.gameObject.SetActive(false);
            }
            else
            {
                light.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// This method is responsible for quitting the game.
    /// </summary>
    void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Game quitting!");
            Application.Quit();
        }
    }

    /// <summary>
    /// Makes the player jump.
    /// </summary>
    void Jump()
    {
        if (Input.GetButtonUp("Jump"))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jump_Force);
            //print("Jump!");
        }
    }

    /// <summary>
    /// This method is responsible for moving the player around.
    /// </summary>
    void Move() {
        float forward = 0; // Initializing for further calculations

        // Condition for player running.
        if (Input.GetButton("L_Shift") && pi.isRun) {
            forward = Input.GetAxis("Vertical") * speed_Run * Time.deltaTime; // Calculating the forward value using speed_Run value.
            pi.lowerStamina(); // Lowering the stamina
        }
        else {
            forward = Input.GetAxis("Vertical") * speed_Walk * Time.deltaTime; // Calculating the forward value using speed_Walk value.
        }
        
        float sideward = Input.GetAxis("Horizontal") * speed_Walk * Time.deltaTime; // Calculating the sideward value using speed_Walk

        transform.Translate(sideward, 0, forward); // Making the player move forward, sideward or both.
    }

    /// <summary>
    /// This method is responsible for turning the player.
    /// </summary>
    void Turn() {

        // Getting the horizontal value from the mouse x.
        float horizantal = Input.GetAxis("Mouse X") * speed_Turn;

        // Getting the vertical value from the mouse y.
        float vertical = Input.GetAxis("Mouse Y") * -speed_Turn;

        // Rotating around the y axis for horizontal turning.
        transform.Rotate(0, horizantal, 0);

        // Rotating around the x axis for vertical turning.
        // Vertical turning requires limit of rotation.

        Vector3 v_Rot = pivot.transform.localEulerAngles; // Taking the euler angles from the pivot model.
        v_Rot.x = Mathf.Clamp(v_Rot.x + vertical, ver_Min, ver_Max); // Clamping the x axis with the given Min and Max values.
        //v_Rot.x = v_Rot.x + vertical;
        Debug_v_Rot = v_Rot.x; // Showing the x axis value for debug purposes only!
        pivot.transform.localEulerAngles = v_Rot; // Rotating the pivot x axis after calculation.
    }
}
