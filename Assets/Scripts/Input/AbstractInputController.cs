using System;
using UnityEngine;

namespace InputSystem
{
    public abstract class AbstractInputController : MonoBehaviour, IInputController
    {
        private Action<Vector3, Quaternion> _moveAction;
        private Action _buildAction;
        private Func<RaycastHit, bool> _raycastAction;

        private Vector2 _sensitivity = new Vector2(1f, 1f);        
        protected bool isBuilding;

        public void WithMoveListener(Action<Vector3, Quaternion> moveAction)
        {
            _moveAction = moveAction;
        }

        public void WithBuildListener(Action buildAction)
        {
            _buildAction = buildAction;
        }

        public void WithRaycastingListener(Func<RaycastHit, bool> raycastAction)
        {
            _raycastAction = raycastAction;
        }

        protected abstract bool GetDirection(out Vector3 direction);
        protected abstract bool GetEulerAnglesRotation(out Vector2 eulerAnglesRotation);
        protected abstract Vector2 GetPositionOnScreen();
        protected abstract bool IsPressed();


        public void StartBuilding()
        {
            isBuilding = true;
        }

        public void StopBuilding()
        {
            isBuilding = false;
        }

        public virtual void Update()
        {
            if (isBuilding)
            {
                UpdateBuilding();
            }
            else
            {
                UpdateMove();
            }
        }

        private void UpdateMove()
        {
            Vector3 direction;
            Vector2 eulerAnglesRotation;
            if (GetDirection(out direction) | GetEulerAnglesRotation(out eulerAnglesRotation))
            {
                Quaternion rotation = Quaternion.Euler(-eulerAnglesRotation.y * _sensitivity.y,
                    eulerAnglesRotation.x * _sensitivity.x,
                    0f);
                _moveAction.Invoke(direction, rotation);
            }
        }

        private void UpdateBuilding()
        {
            RaycastHit raycastHit;
            var position = GetPositionOnScreen();            
            var ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (Vector3.Distance(Camera.main.transform.position, raycastHit.point) < 30f)
                {
                    if (_raycastAction(raycastHit) && IsPressed())
                    {
                        StopBuilding();
                        _buildAction();
                    }
                }               
            }                  
        }
    }
}
