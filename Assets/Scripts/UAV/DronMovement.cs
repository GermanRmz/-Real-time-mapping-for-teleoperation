using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DronMovement : MonoBehaviour {
    //[UPyPlot.UPyPlotController.UPyProbe]
    private float xVar;

    //[UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
    private float yVar;

    //[UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
    private float zVar;

    private float lastRndX = 0;
    private float lastRndY = 0;
    private float lastRndZ = 0;
    
    //private Transform DronEnab;
    Rigidbody MyDron;
    public bool Abrir, Cerrar;

    //posicion recivida drone arm ------------
    float posx;
    float posy;
    float posz;
    float posrot;
    //posicion drone virtual
    float dposx;
    float dposy;
    float dposz;
    float dposrot;

    //obtener posiciones
    public void PosGetX(float psgx)
    {
        posx = psgx/10000;
    }

    public void PosGetY(float psgy)
    {
        posy = psgy/10000;
    }

    public float z=0;

    public void PosGetZ(float psgz)
    {
        posz = psgz/10000;
    }

    public void PosGetRot(float psrot)
    {
        posrot = Math.Abs(psrot/100);
    }

    // Use this for initialization
    void Awake()
    {
        MyDron = GetComponent<Rigidbody>();
    }

    public bool DronControl;
    public void ChangeToDronControl()
    {

        //DronControl = true;
        if (DronControl == true) { DronControl = false; }

        else if (DronControl == false) { DronControl = true; }

    }
    void FixedUpdate()
    {
        //float axis = Input.GetAxis("Vertical1");

        if (DronControl == true)
        {
            MyDron.useGravity = false;

            dposx = transform.localPosition.x;
            dposy = transform.localPosition.z;
            dposz = transform.localPosition.y;
            dposrot = transform.localEulerAngles.y;

            //Debug.Log(dposx);
            //Debug.Log(posx);
            //Debug.Log("dposrotfadsfad");

            //Debug.Log(DronControl);
            MovementUpDown();
            //MovementForward();
            Rotation();
            //ClampingSpeedValues();
            //Swerwe();
            MyDron.AddRelativeForce(Vector3.up * upForce);
            MyDron.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, -tiltAmountSideways));

        }
        else if (DronControl == false)
        {
            MyDron.useGravity = false;
            //MyDron.AddRelativeForce(Vector3.up * upForce);
            //MyDron.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, -tiltAmountSideways));

        }
        
        //plot 
        xVar = Mathf.Lerp(lastRndX, dposx, Time.deltaTime * 0.5f);
        lastRndX = xVar;

        yVar = Mathf.Lerp(lastRndY, dposy, Time.deltaTime * 0.5f);
        lastRndY = yVar;

        zVar = Mathf.Lerp(lastRndZ, dposz, Time.deltaTime * 0.5f);
        lastRndZ = zVar;
        
    }


    private float movementForwardSpeed = 200.0f;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward;
    float JIV;
    public void JIUP()
    {
        JIV = 1;
    }
    public void JIDOWN()
    {
        JIV = -1;
    }
    float LeapAde;


    void MovementForward()
    {
        //transform.position = new Vector3(posx * 10,posy * 10,posz * 10);
        float vely = 0;
        
        if ((dposz < posy - 2) && posy != 0)
        {
            vely = .3f;
        }else if ((dposz > posy + 2) && posy != 0)
        {
            vely = -.3f;
        }
        else
        {
            vely = 0;
        }
        

        float MoveForwar = Input.GetAxis("Vertical") + vely;
        float MoveForward = JIV + MoveForwar + LeapAde;// + posx;
        //Debug.Log(MoveForward);
        if (MoveForward != 0)
        {
            MyDron.AddRelativeForce(Vector3.forward * MoveForward * movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, MoveForward, ref tiltVelocityForward, 3.5f);
            JIV = 0;
            LeapAde = 0;
        }
        vely = 0;
    }

    //programacionjoystick
    float JoyVER;
    public void JoyUP()
    {
        JoyVER = 1;
    }
    public void JoyDOWN()
    {
        JoyVER = -1;
    }

    

    public float upForcee;
    float upForce;
    float lastErrorZ = 0;
    public float kpZ = .2f;
    public float KiZ = 0.1f;
    public float kdZ = 0.0f;

    float deltaErrorZ = 0;
    float errorZ = 0;
    float ControlIZ=0;
    void MovementUpDown()
    {
        float velz = 0;

        errorZ = posz - dposz;

        float ControlPZ = kpZ * errorZ;
        ControlIZ += KiZ * errorZ;
        deltaErrorZ = errorZ - lastErrorZ;

        velz = ControlPZ + ControlIZ + kdZ * deltaErrorZ;
        lastErrorZ = errorZ;

        upForce=velz;
        print(dposz + " to " + posz + " diff: " + (posz-dposz));


        /*
        if (dposy < posz - 2 && posz != 0)
        {
            velz = 1;
        }else if(dposy > posz + 2 && posz != 0)
        {
            velz = -1;
        }
        else
        {
            velz = 0;
        }
        
        upForce += upForcee;// + posz;
        if (Input.GetKey(KeyCode.I) | JoyVER == 1 | velz == 1)
        {
            upForce = 190;
        }
        else if (Input.GetKey(KeyCode.K) | JoyVER == -1 | velz == -1)
        {
            upForce = -20;
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
        {
            upForce = 98.1f;
        }
        JoyVER = 0;
        velz = 0;*/
        
    }

    float JoyHOR;
    public void JoyRIGHT()
    {
        JoyHOR = 1;
    }
    public void JoyLEFT()
    {
        JoyHOR = -1;
    }


    private float wantedYRotation;
    [HideInInspector] public float currentYRotation;
    private float rotateAmoutByKey = 1.0f;
    private float rotationYVelocity;

    float lastErrorYRotation = 0;
    void Rotation()
    {
        float velrot = 0;
        float KpRot = 1f;
        float KdRot = .65f;
        float deltaErrorRot = 0;

        float mappedPosRot = 360 - posrot;
        
        float distancia1Rot = (mappedPosRot - dposrot + 360) % 360;
        float distancia2Rot = (dposrot - mappedPosRot + 360) % 360;

        if (mappedPosRot != 0)
        {
            if (distancia1Rot <= distancia2Rot)
            {
                deltaErrorRot = distancia1Rot - lastErrorYRotation;
                velrot = KpRot * distancia1Rot + KdRot * deltaErrorRot;
                lastErrorYRotation = distancia1Rot;
            }
            else if (distancia2Rot < distancia1Rot)
            {
                deltaErrorRot = distancia2Rot - lastErrorYRotation;
                velrot = -KpRot * distancia2Rot + KdRot * deltaErrorRot;
                lastErrorYRotation = distancia2Rot;
            }
            //print(dposrot + " to " + mappedPosRot+ " " + posrot + " diff: " + velrot);
        }
        else
        {
            velrot = 0;
        }

        currentYRotation = velrot+dposrot;
        JoyHOR = 0;
    }
    private Vector3 velocityToSmoothDampToZero;
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(JIV) > 0.2f && Mathf.Abs(JIH) > 0.2f)
        {
            MyDron.velocity = Vector3.ClampMagnitude(MyDron.velocity, Mathf.Lerp(MyDron.velocity.magnitude, 5.0f, Time.deltaTime * 2f));
        }
        if (Mathf.Abs(JIV) > 0.2f && Mathf.Abs(JIH) < 0.2f)
        {
            MyDron.velocity = Vector3.ClampMagnitude(MyDron.velocity, Mathf.Lerp(MyDron.velocity.magnitude, 5.0f, Time.deltaTime * 2f));
        }
        if (Mathf.Abs(JIV) < 0.2f && Mathf.Abs(JIH) > 0.2f)
        {
            MyDron.velocity = Vector3.ClampMagnitude(MyDron.velocity, Mathf.Lerp(MyDron.velocity.magnitude, 5.0f, Time.deltaTime * 2f));
        }
        if (Mathf.Abs(JIV) < 0.2f && Mathf.Abs(JIH) < 0.2f)
        {
            MyDron.velocity = Vector3.SmoothDamp(MyDron.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
    }
    

    private float sideMovementAmount = 250.0f;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;
    //drone
    float JIH;
    public void JIRIGTH()
    {
        JIH = 1;
    }
    public void JILEFT()
    {
        JIH = -1;
    }


    void Swerwe()
    {
        float velx = 0;
        
        if ((posx < dposx - 1) && posx != 0)
        {
            velx = -.5f;
        }
        else if ((posx > dposx + 1) && posx != 0)
        {
            velx = .5f;
        }
        else
        {
            velx = 0;
        }
        

        float atc = Input.GetAxis("Horizontal");
        float acac = atc + JIH + velx;// + posy;

        if (Mathf.Abs(acac) > 0.2f)
        {
            MyDron.AddRelativeForce(Vector3.right * acac * sideMovementAmount);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 15 * acac, ref tiltAmountVelocity, 5.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 5.1f);
        }
        JIH = 0;
        velx = 0;
    }
}

