using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//이벤트 핸들러 사용 using

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Camera camera;
    public GameObject Player;
    public List<Rect> coliders;
    public Animator Animator_Player;
    public SpriteRenderer SpriteRenderer_Player;
    public enum playerMovement { Idle, Walking, Attack }
    public playerMovement PlayerStatus = playerMovement.Idle;
    public Vector3 movement = Vector3.zero;

    #region
    //이벤트 핸들러 enum값
    public enum eEventHandle { Click, Drag }
    public eEventHandle m_ePrevEvent;

    //이번 글에서 사용하진 않았다. 자세한 설명은 블로그 아래
    private RectTransform m_BackGround;

    //조이스틱과 조이스틱 백그라운드 오브젝트
    public GameObject m_JoyStickBackGround;
    public GameObject m_JoyStick;

    //겟컴포넌트 하기 귀찮아서 따로 빼놓았다.
    private RectTransform m_TransJoyStickBackGround;
    private RectTransform m_TransJoyStick;
    #endregion

    //포지션값을 따로 저장 하기 위함.
    public Vector2 m_VecJoystickValue { get; private set; }
    public Vector3 m_VecJoyRotValue { get; private set; }

    //조이스틱의 범위계산을 위한 반지름 값.
    private float m_fRadius;

    private void Awake()
    {
        foreach (var sp in GameObject.FindGameObjectsWithTag("COLIDER_INIT"))
        {
            coliders.Add(GetWorldSapceRect(sp.GetComponent<RectTransform>()));
        }

        Animator_Player = Player.GetComponent<Animator>();
        SpriteRenderer_Player = Player.GetComponent<SpriteRenderer>();
        Init();//초기화
    }

    void FixedUpdate()
    {
        if (PlayerStatus == playerMovement.Walking)
        {
            Player.transform.Translate(movement.x, 0, 0);
            camera.transform.Translate(movement.x, 0, 0);

            foreach (var sp in coliders)
            {
                if (sp.Contains(Player.transform.position))
                {

                    Player.transform.Translate(-movement.x, 0, 0);
                    camera.transform.Translate(-movement.x, 0, 0);
                    break;
                }
            }

            Player.transform.Translate(0, movement.y, 0);
            camera.transform.Translate(0, movement.y, 0);

            foreach (var sp in coliders)
            {
                if (sp.Contains(Player.transform.position))
                {

                    Player.transform.Translate(0, -movement.y, 0);
                    camera.transform.Translate(0, -movement.y, 0);
                    break;
                }
            }

        }
    }


    void Update()
    {
        switch (PlayerStatus)
        {
            case playerMovement.Walking:
                SpriteRenderer_Player.flipX = movement.x < 0;
                Animator_Player.SetBool("isWalking", true);
                break;
            case playerMovement.Idle:
                Animator_Player.SetBool("isWalking", false);
                break;
        }
    }


    //이벤트들
    #region event
    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CallJoyStick(eventData);
        SetHandleState(eEventHandle.Click);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_JoyStickBackGround.SetActive(false);

        if (m_ePrevEvent == eEventHandle.Drag)
            return;

        SetHandleState(eEventHandle.Click);
    }

    public void OnDrag(PointerEventData eventData)
    {
        JoyStickMove(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        JoyStickMoveEnd(eventData);
    }
    #endregion

    private void Init()
    {
        m_TransJoyStickBackGround = m_JoyStickBackGround.GetComponent<RectTransform>();
        m_TransJoyStick = m_JoyStick.GetComponent<RectTransform>();
        m_fRadius = m_TransJoyStickBackGround.rect.width * 0.5f; //조이스틱의 행동반경 계산

        m_JoyStick.SetActive(true);
        m_JoyStickBackGround.SetActive(false);
    }

    private void JoyStickMoveEnd(PointerEventData eventData)
    {
        m_TransJoyStick.position = eventData.position;
        m_JoyStickBackGround.SetActive(false);

        SetHandleState(eEventHandle.Click);
        movement = Vector3.zero;
        PlayerStatus = playerMovement.Idle;
    }

    private void CallJoyStick(PointerEventData eventData)
    {
        m_JoyStickBackGround.transform.position = eventData.position;
        m_JoyStick.transform.position = eventData.position;
        m_JoyStickBackGround.SetActive(true);
    }

    private void JoyStickMove(PointerEventData eventData)
    {
        m_VecJoystickValue = eventData.position - (Vector2)m_TransJoyStickBackGround.position;

        m_VecJoystickValue = Vector2.ClampMagnitude(m_VecJoystickValue, m_fRadius);
        m_TransJoyStick.localPosition = m_VecJoystickValue;

        m_VecJoyRotValue = new Vector3(m_TransJoyStick.localPosition.x, m_TransJoyStick.localPosition.y, 0f);
        movement = m_VecJoyRotValue / 4000;
        SetHandleState(eEventHandle.Drag);
        PlayerStatus = playerMovement.Walking;
    }

    private void SetHandleState(eEventHandle _handle)
    {
        m_ePrevEvent = _handle;
    }

    Rect GetWorldSapceRect(RectTransform rt)
    {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = rt.TransformVector(r.size);
        return r;
    }
}