using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController
{
    private Player _player;

    private bool _isLanded;
    private bool _isGrounded;
    private bool _isDashing;


    private float _dashTimer;
    protected float _direction;
    protected float _dashDirection;
    public float Direction
    {
        get => _direction;
        set
        {
            if (value > 0)
            {
                value = 1;
            }
            else if (value < 0)
            {
                value = -1;
            }

            if (_direction != value)
            {
                Flip(value);
            }

            _direction = value;
        }
    }

    public PlayerController(Player player)
    {
        _player = player;
    }

    public void Initialize()
    {
        _isLanded = false;
        _isGrounded = true;
        _isDashing = false;

        _dashDirection = 1;
        _dashTimer = 0f;

        _dashTimer = _player.Stat.DashDelay;
    }

    public void Update()
    {

        if (_player.Stat.IsKnockedBack) return;

        if (_player.Attack.CurrentAttackkState == Define.AttackState.ATTACK)
        {
            Stop();

            return;
        }

        _isGrounded = Physics.CheckSphere(_player.GroundCheckPoint.position, _player.GroundCheckRadius, _player.GroundLayer);
        _player.Ani.SetBool("isGrounded", _isGrounded);

        if (_isGrounded)
        {
            if (!_isLanded)
            {
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_JumpLanding", _player.transform);
                _isLanded = true;
            }
        }
        else
        {
            _isLanded = false;
        }

        if (_dashTimer >= _player.Stat.DashDelay)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Dash();
            }
        }
        else
        {
            _dashTimer += Time.deltaTime;
        }


        if (!_isDashing)
        {
            Move();
            Jump();
        }
    }

    private void Move()
    {
        

        _direction = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Direction = -1f;
            _dashDirection = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Direction = 1f;
            _dashDirection = 1f;
        }

        if (!CheckMovePath())
        {
            _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);
            if (_direction == 0)
            {
                _player.Ani.SetFloat("Speed", 0);
            }
            return;
        }


        Vector2 tempVelocity = new Vector2(_direction * _player.Stat.SprintSpeed, _player.Rb.velocity.y);

        _player.Ani.SetFloat("Speed", Mathf.Abs(tempVelocity.x));


        _player.Rb.velocity = tempVelocity;
    }


    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _isGrounded)
        {
            _player.Ani.SetTrigger("isJumping");
            _player.Rb.velocity = new Vector2(_player.Rb.velocity.x, _player.Stat.JumpForce);
            _isGrounded = false;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _player.Rb.velocity = new Vector3(_player.Rb.velocity.x, _player.Rb.velocity.y / 2, _player.Rb.velocity.z);
        }

        if (Mathf.Abs(_player.Rb.velocity.y) >= 0.1f)
        {
            _player.Ani.SetFloat("VerticalSpeed", _player.Rb.velocity.y);
        }
    }

    private void Dash()
    {
        _player.Rb.velocity = Vector2.zero;

        _isDashing = true;
        _player.GetComponent<Collider>().enabled = false; 
        Vector3 dashPosition = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(_player.transform.position, Vector3.right * _dashDirection, out hit, _player.Stat.DashDistance, _player.WallLayer)) // 벽이 있다면 벽과의 충돌 위치 바로 앞에서 멈추게 설정
        {            
            dashPosition = hit.point - (Vector3.right * _dashDirection) * 0f;  // 곱하는 수 만큼 벽에서 떨어짐
        }
        else  // 벽이 없으면 대쉬 거리만큼 앞으로 이동
        {      
            dashPosition = _player.transform.position + (Vector3.right * _dashDirection) * _player.Stat.DashDistance;
        }


        _player.Rb.DOMove(dashPosition, _player.Stat.DashDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _isDashing = false;
            _player.GetComponent<Collider>().enabled = true;
           
        });

        _player.Ani.SetTrigger("Dash");

        _dashTimer = 0;
    }

    private void Stop()
    {
        _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);
    }

    private void Flip(float value)
    {
        Vector3 tempScale = _player.PlayerModel.localScale;

        if (value * tempScale.x < 0)
        {
            tempScale.x *= -1;
        }

        _player.PlayerModel.localScale = tempScale;
    }
    private bool CheckMovePath()
    {
        // 레이캐스트로 장애물 감지
        RaycastHit hit;
        if (Physics.Raycast(_player.transform.position, Vector2.right * _dashDirection, out hit, 0.5f, _player.BlockLayer))
        {
            // 장애물이 레이캐스트 범위 안에 있음
            //Debug.Log("장애물 감지: " + hit.collider.name);
            return false;
        }

        // 장애물이 없음
        return true;
    }
}
