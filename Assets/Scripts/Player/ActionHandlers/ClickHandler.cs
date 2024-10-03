using System;
using Camera;
using Events;
using UnityEngine;
using Utils.Singleton;


namespace Player.ActionHandlers
{
    public class ClickHandler : DontDestroyMonoBehaviourSingleton<ClickHandler>
    {
        [SerializeField] private float clickToDragDuration;

        public event Action<Vector3> PointerDownEvent;
        public event Action<Vector3> ClickEvent;
        public event Action<Vector3> PointerUpEvent;
        public event Action<Vector3> DragStartEvent;
        public event Action<Vector3> DragEndEvent;
        public event Action<Vector3> OnScroll;

        private Vector3 _pointerDownPosition;

        private bool _isClick;
        private bool _isDrag;
        private float _clickHoldDuration;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isClick = true;
                _clickHoldDuration = .0f;

                _pointerDownPosition = CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
                
                PointerDownEvent?.Invoke(_pointerDownPosition);
                
                _pointerDownPosition = new Vector3(_pointerDownPosition.x, _pointerDownPosition.y, .0f);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var pointerUpPosition = CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
                    
                if (_isDrag)
                {
                    DragEndEvent?.Invoke(pointerUpPosition);

                    _isDrag = false;
                }
                else
                {
                    ClickEvent?.Invoke(pointerUpPosition);
                }
                
                PointerUpEvent?.Invoke(pointerUpPosition);

                _isClick = false;
            }
            
            if (Input.mouseScrollDelta.y != 0)
            {
                Vector3 scrollDirection = new Vector3(0, Input.mouseScrollDelta.y, 0);
                OnScroll?.Invoke(scrollDirection);
            }
        }
        
        private void LateUpdate()
        {
            if (!_isClick)
                return;

            _clickHoldDuration += Time.deltaTime;
            if (_clickHoldDuration >= clickToDragDuration)
            {
                DragStartEvent?.Invoke(_pointerDownPosition);

                _isClick = false;
                _isDrag = true;
            }
        }
        
        public void SubscribeToDragEventHandlers(Action<Vector3> dragStartEvent, Action<Vector3> dragEndEvent)
        {
            DragStartEvent += dragStartEvent;
            DragEndEvent += dragEndEvent;
        }

        public void UnsubscribeToDragEventHandlers(Action<Vector3> dragStartEvent, Action<Vector3> dragEndEvent)
        {
            DragStartEvent -= dragStartEvent;
            DragEndEvent -= dragEndEvent;
        }

        public void SubscribeToScrollEvent(Action<Vector3> scrollEvent)
        {
            OnScroll += scrollEvent;
        }
        
        public void UnsubscribeToScrollEvent(Action<Vector3> scrollEvent)
        {
            OnScroll -= scrollEvent;
        }
    }
}