using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Isometriccontroller : MonoBehaviour
{
    //Movimiento y Animaciones
    CharacterController _controller;

    float _horizontal;
    float _vertical;
    public Vector3 direction;

    [SerializeField] float _playerSpeed = 5;

    [SerializeField] float _jumpHeigh = 1;
    float _gravity = -9.81f;

    Animator _animator;

    Vector3 _playerGravity;

    [SerializeField] AxisState xAxis;
    [SerializeField] AxisState yAxis;
    
    //variables sensor
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.2f;
    [SerializeField] LayerMask _groundLayer;

    bool _isGrounded;

    //Camera
    Transform _camera;
    public GameObject _cameraNormal;
    float _turnSmoothVelocity;
    [SerializeField] float _turnSmoothTime = 0.1f;
    Transform _lookAtTransform;

    //Attack
    public float attackRange = 1f;
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    int buttonQuantity;
    bool canClick;

    //Dash
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 2f;
    private bool isDashing = false;
    bool isInvulnerable = false;

    //Resistencia
    private ResistencePlayer _resistance;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _lookAtTransform = GameObject.Find("LookAt").transform;
        _resistance = GetComponent<ResistencePlayer>();
        _camera = Camera.main.transform;
        buttonQuantity = 0;
        canClick = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(_horizontal, 0, _vertical);

        
        Movement();

        Jump();

        if(Input.GetKeyDown("e"))
        {
            Combo();
        }
        /*if(Input.GetKeyUp("e"))
        {
            StopAttack();
        }*/

        if(Input.GetKey("r"))
        {
            if(direction != Vector3.zero)
            {
                WalkingBlock();
            }
            else
            {
                Block();
            }
            
        }
        if(Input.GetKeyUp("r"))
        {
            DontBlock();
        }

        if (!isDashing && Input.GetKeyDown("z"))
        {
            StartCoroutine(PerformDash());
        }
    }

    /*void Attack()
    {
        _animator.SetBool("Attack", true);
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy.name);
        }
    }*/

    void Combo()
    {
        if(canClick)
        {
            buttonQuantity++;
        }

        if(buttonQuantity == 1)
        {
            _animator.SetInteger("attack", 1);
            _resistance.Hit();
        }
    }

    public void ComboVerification()
    {
        canClick = false;

        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && buttonQuantity == 1)
        {
            _animator.SetInteger("attack", 0);
            canClick = true;
            buttonQuantity = 0;
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && buttonQuantity >= 2)
        {
            _animator.SetInteger("attack", 2);
            canClick = true;
            _resistance.Hit();
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && buttonQuantity == 2)
        {
            _animator.SetInteger("attack", 0);
            canClick = true;
            buttonQuantity = 0;
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && buttonQuantity >= 3)
        {
            _animator.SetInteger("attack", 3);
            canClick = true;
            _resistance.Hit();
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && buttonQuantity == 3)
        {
            _animator.SetInteger("attack", 0);
            canClick = true;
            buttonQuantity = 0;
        }
        else if(_resistance.actualResistance == 0f)
        {
            _animator.SetInteger("attack", 0);
        }
        else
        {
            _animator.SetInteger("attack", 0);
            canClick = true;
            buttonQuantity = 0;
        }
    }

    /*void StopAttack()
    {
       _animator.SetBool("Attack", false); 
    }*/
    
    void Block()
    {
         _animator.SetBool("IsBlocking", true);
         _animator.SetBool("WalkingBlock", false);

        _playerSpeed = 2;
    }

    void WalkingBlock()
    {
        _animator.SetBool("WalkingBlock", true);
        _animator.SetBool("IsBlocking", false);

        _playerSpeed = 2;
    }

    void DontBlock()
    {
        _animator.SetBool("IsBlocking", false);
        _animator.SetBool("WalkingBlock", false);
        _playerSpeed = 7;
    }

    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }*/

    void Movement()
    {
        /*Vector3 direction = new Vector3(-_vertical, 0, _horizontal);

        _animator.SetFloat("VelX", 0);
        _animator.SetFloat("VelZ", direction.magnitude);
        
        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            _controller.Move(direction.normalized * _playerSpeed * Time.deltaTime);
        }*/

        //Vector3 direction = new Vector3(_horizontal, 0, _vertical);
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
            direction = new Vector3(_horizontal, 0, _vertical);
        
            _animator.SetFloat("VelX", 0);
            _animator.SetFloat("VelZ", direction.magnitude);
            
            if(direction != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +  _camera.eulerAngles.y;

                Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
            }
            
            Vector3 move = new Vector3(0, _vertical, _horizontal).normalized;

            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, yAxis.Value, 0);
            _lookAtTransform.eulerAngles = new Vector3(yAxis.Value, transform.eulerAngles.y, 0);
            _resistance.MovementRes();
        //}        
    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
        _animator.SetBool("IsJumping", !_isGrounded);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -2;
        }
        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeigh * -2 * _gravity);
            //_animator.SetBool("IsJumping", true);
        }        
        _playerGravity.y += _gravity * Time.deltaTime;
        
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);
    }

    IEnumerator PerformDash()
    {
        isDashing = true;
        isInvulnerable = true;

        // Guardar la posición inicial para el dash
        Vector3 startPosition = transform.position;

        // Calcula la dirección del dash basada en la orientación del personaje
        Vector3 dashDirection = transform.forward;

        // Inicia el contador de tiempo del dash
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            // Calcula la nueva posición durante el dash
            Vector3 newPosition = startPosition + dashDirection * dashDistance * (elapsedTime / dashDuration);

            // Mueve al personaje hacia la nueva posición
            _controller.Move(newPosition - transform.position);

            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            _animator.SetBool("Dashing", true);

            // Espera hasta el siguiente frame
            yield return null;
            _animator.SetBool("Dashing", false);
        }

        // Espera el tiempo de cooldown antes de permitir otro dash
        yield return new WaitForSeconds(dashCooldown);

        isDashing = false;
        isInvulnerable = false;
    }
}
