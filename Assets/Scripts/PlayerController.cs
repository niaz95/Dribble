using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    
    private Rigidbody rb;
    private int count;
    private float startTimer;
    public int speed;
    public int pickups;
    public Text countText;
    public Text winText;
    public Text timerText;
    static bool isDead;
    static bool youWin;
    static bool moveAllowed;
    
    //new input mechanism
    //use speed instead of velocity
//    public float turnSpeed = 40;
    public float height = 0.05f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
//    public float maxGroundAngle = 120;
    
//    Vector2 input;
//    float angle;
//    float groundAngle;
//    
//    Quaternion targetRotation;
//    Transform cam;
//    
    Vector3 forward;
    RaycastHit hitInfo;
    bool grounded;
    //
        
    Animator anim;
    
    void Start(){
        count = 0;      //initialise score
        startTimer = 0.0f;
        setCountText();
        rb = GetComponent<Rigidbody>();
        winText.text = "";
        isDead = false;
        moveAllowed = true;
        youWin = false;
        anim = GetComponent<Animator>();
        anim.SetBool("BallDead",isDead);
        
    }
    
    void Update(){
        
        float t = Time.time - startTimer;                        //Set timer 
        string seconds = (t%60).ToString("f2");
        timerText.text = "Time: "+ seconds + " s";
        
        if(isDead){
            moveAllowed = false;
            rb.velocity = new Vector3(0,0,0);
            startTimer = Time.time;                             //Restart timer
            anim.SetBool("BallDead",true);
            Invoke("RestartScene",1f);      //Restart scene in initial position and play dead animation
        }
        
        if(youWin){
            moveAllowed = false;
            winText.text = "You Win!";
            anim.SetBool("BallDead",true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }    
    }

    void FixedUpdate(){
        
        if(moveAllowed){
            
        CheckGround();
        CalculateForward();
        
            
        float moveHorizontal = Input.acceleration.x;        //using accelorometer input
        float moveVertical = Input.acceleration.y;
              
        Vector3 movement = new Vector3 (moveHorizontal,0.0f,moveVertical);
        //rb.AddForce(movement * speed);
            
        rb.AddForce(forward * speed);
            
//            GetInput();
//            CalculateDirection();
//            CalculateForward();
//            CalculateGroundAngle();
//            CheckGround();
//                    
//            Rotate();
//            Move();
        }      
    }
    
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Pick Up")){
            other.gameObject.SetActive(false);
            if (count<pickups){
                setCountText();
            }else{
                youWin = true;    //Winning by picking up all pickups on the map
            }
        }else if(other.gameObject.CompareTag("Hole")){ 
            isDead = true;        //Restart in initial position if ball falls in hole
            
        }else if(other.gameObject.CompareTag("Goal")){
            youWin = true;      //Winning by entering the goal post
            
        }
    }
    
    void setCountText(){
        count = count + 1;
        countText.text = "Score: " + count.ToString(); 
    }
    
    void RestartScene(){
        Scene currentLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentLevel.buildIndex);
    }
    
    //new input mechanism
    ///
//    void GetInput(){
//        input.x = Input.acceleration.x;
//        input.y = Input.acceleration.y;
//    }
//    
//    void CalculateDirection(){
//        angle = Mathf.Atan2(input.x,input.y);
//        angle = Mathf.Rad2Deg * angle;
//        //angle += cam.eulerAngles.y;
//    }
//    
//    void Rotate(){
//        targetRotation = Quaternion.Euler(0, angle, 0);
//        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
//    }
//    
//    void Move(){
//        if(groundAngle >= maxGroundAngle) return;
//        
//        rb.AddForce(forward * speed);
//        //transform.position += forward * speed * Time.deltaTime;
//    }
//    
    void CalculateForward(){
        if(!grounded){
            forward = transform.forward;
            return;
        }
        
        forward = Vector3.Cross(transform.right, hitInfo.normal);
    }
//    
//    void CalculateGroundAngle(){
//        if(!grounded){
//            groundAngle = 90;
//            return;
//        }
//        
//        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
//    }
//    
    void CheckGround(){
        if(Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground)){    
            grounded = true;
        }else{
            grounded = false;
        }
    }

                 
}
