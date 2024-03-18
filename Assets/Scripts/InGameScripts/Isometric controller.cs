using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isometriccontroller : MonoBehaviour
{
    CharacterController _controller;
    Transform _camera;
    public GameObject _cameraNormal;

    float _horizontal;
    float _vertical;

    [SerializeField] float _playerSpeed = 5;

    float _turnSmoothVelocity;
    [SerializeField] float _turnSmoothTime = 0.1f;

    [SerializeField] float _jumpHeigh = 1;
    float _gravity = -9.81f;

    Animator _animator;

    Vector3 _playerGravity;
    //variables sensor
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.2f;
    [SerializeField] LayerMask _groundLayer;

    bool _isGrounded;

    public float attackRange = 1f;

    public int attackDamage = 10;

    public LayerMask enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        
        Movement();

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if(Input.GetKeyDown("e"))
        {
            Attack();
        }
        if(Input.GetKeyUp("e"))
        {
            StopAttack();
        }

        if(Input.GetKeyDown("r"))
        {
            Block();
        }
        if(Input.GetKeyUp("r"))
        {
            DontBlock();
        }
    }

    void Attack()
    {
        _animator.SetBool("Attack", true);
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy.name);
        }
    }

    void StopAttack()
    {
       _animator.SetBool("Attack", false); 
    }
    
    void Block()
    {
        _animator.SetBool("IsBlocking", true);
        _playerSpeed = 2;
    }

    void DontBlock()
    {
        _animator.SetBool("IsBlocking", false);
        _playerSpeed = 7;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Movement()
    {
        Vector3 direction = new Vector3(-_vertical, 0, _horizontal);

        _animator.SetFloat("VelX", 0);
        _animator.SetFloat("VelZ", direction.magnitude);
        
        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            _controller.Move(direction.normalized * _playerSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

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
}
