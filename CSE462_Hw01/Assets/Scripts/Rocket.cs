using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    ConstantForce gravity;
    public float rotationFactor = 120f;
    public float flyFactor = 80f;
    public float flyForwardFactor = 110f;

    private int sceneIndex;
    
    bool upFlag = false;
    bool downFlag = false;
    bool leftFlag = false;
    bool rightFlag = false;
    bool forwardFlag = false;
    bool backFlag = false;
    bool isGrounded = false;
    bool finishFlag = false;
    bool getFlag = false;

    GameObject aj;
    GameObject claire;
    GameObject sos;
    GameObject nextButton;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gravity = rigidbody.GetComponent<ConstantForce>();
        Physics.gravity = new Vector3(0.0f, -9.8f, 0.0f);
        aj = GameObject.Find("aj");
        claire = GameObject.Find("claire");
        sos = GameObject.Find("Sos");
        nextButton = GameObject.Find("NextLevelButton");
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        print(nextButton.GetComponent<TextMesh>().text);
        nextButton.active = false;
        //GameObject aj = GameObject.Find("aj");
        //GameObject claire = GameObject.Find("claire");
        //print("x: " + aj.transform.position.x);
        //print("y: " + aj.transform.position.y);
        //print("x: " + aj.transform.position.z);
        //aj.transform.position = new Vector3(-0.081f, 0.47f, 0.436f);
        ////GameObject launchpad = GameObject.Find("LaunchPad");
        ////launchpad.transform.position = new Vector3(3, 0, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        float rotationThisFrame = rotationFactor * Time.deltaTime;
        float flyThisFrame = flyFactor * Time.deltaTime;
        

        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * flyThisFrame);
            print("space");
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rigidbody.AddRelativeForce(Vector3.down * flyThisFrame);
            print("ctrl");
        }
        RotationOfObject();
    }

    public void RotationOfObject()
    {
        rigidbody.freezeRotation = true;
        float rotationThisFrame = rotationFactor * Time.deltaTime;
        float flyForwardThisFrame = flyForwardFactor * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddRelativeForce(Vector3.forward * flyForwardThisFrame);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody.AddRelativeForce(Vector3.back * flyForwardThisFrame);
        }

        rigidbody.freezeRotation = false;
    }

    public void Movement()
    {
        rigidbody.freezeRotation = true;
        float rotationThisFrame = rotationFactor * Time.deltaTime;
        float flyForwardThisFrame = flyForwardFactor * Time.deltaTime;
        float flyThisFrame = flyFactor * Time.deltaTime;

        if (upFlag)
        {
            rigidbody.AddRelativeForce(Vector3.up * flyThisFrame);
            isGrounded = false;
        }

        if (downFlag)
        {
            rigidbody.AddRelativeForce(Vector3.down * flyThisFrame);
        }

        if (leftFlag)
        {
            transform.Rotate(Vector3.down * rotationThisFrame);
        }

        if(rightFlag)
        {
            transform.Rotate(Vector3.up * rotationThisFrame);
        }

        if (!isGrounded)
        {
            if (forwardFlag)
            {
                rigidbody.AddRelativeForce(Vector3.forward * flyForwardThisFrame);
            }

            if (backFlag)
            {
                rigidbody.AddRelativeForce(Vector3.back * flyForwardThisFrame);
            }
        }

        rigidbody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Finish":
                putMinions();
                break;
            case "MinionsField":
                getMinions();
                break;
            case "Map":
                isGrounded = true;              
                break;
        }
    }

    private void getMinions()
    {
        if(!getFlag)
        {
            aj = GameObject.Find("aj");
            aj.active = false;
            claire = GameObject.Find("claire");
            claire.active = false;
            getFlag = true;
        }

        
    }

    private void putMinions()
    {
        if(!finishFlag && getFlag)
        {
            aj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.3f);
            aj.active = true;
            claire.transform.position = new Vector3(transform.position.x + 1.8f, transform.position.y, transform.position.z + 0.3f);
            claire.active = true;
            finishFlag = true;
            sos.GetComponent<TextMesh>().text = "    :)";
            nextButton.active = true;
        }
    }

    public void nextLevelButton()
    {
        SceneManager.LoadScene(1);
    }

    public void previousLevelButton()
    {
        SceneManager.LoadScene(0);
    }

    public void cancelUpFlag()
    {
        upFlag = false;
    }

    public void activateUpFlag()
    {
        upFlag = true;
    }

    public void cancelDownFlag()
    {
        downFlag = false;
    }

    public void activateDownFlag()
    {
        downFlag = true;
    }

    public void activateLeftFlag()
    {
        leftFlag = true;
    }

    public void cancelLeftFlag()
    {
        leftFlag = false;
    }

    public void activateRightFlag()
    {
        rightFlag = true;
    }

    public void cancelRightFlag()
    {
        rightFlag = false;
    }

    public void activateForwardFlag()
    {
        forwardFlag = true;
    }

    public void cancelForwardFlag()
    {
        forwardFlag = false;
    }

    public void activateBackFlag()
    {
        backFlag = true;
    }

    public void cancelBackFlag()
    {
        backFlag = false;
    }
}

