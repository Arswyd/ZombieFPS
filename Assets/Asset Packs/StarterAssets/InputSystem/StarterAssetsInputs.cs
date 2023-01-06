using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool fire;
		public bool zoom;
		public bool switchPistol;
		public bool switchShotgun;
		public bool switchSniper;
		public bool flashlight;
		public float scroll;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnFire(InputValue value)
		{
			FireInput(value.isPressed);
		}

		public void OnZoom(InputValue value)
		{
			ZoomInput(value.isPressed);
		}

		public void OnSwitchPistol(InputValue value)
		{
			SwitchPistolInput(value.isPressed);
		}

		public void OnSwitchShotgun(InputValue value)
		{
			SwitchShotgunInput(value.isPressed);
		}

		public void OnSwitchSniper(InputValue value)
		{
			SwitchSniperInput(value.isPressed);
		}

		public void OnFlashlight(InputValue value)
		{
			FlashlightInput(value.isPressed);
		}

		public void OnScroll(InputValue value)
		{
			ScrollInput(value.Get<float>());
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void FireInput(bool newFireState)
		{
			fire = newFireState;
		}
				
		public void ZoomInput(bool newZoomState)
		{
			zoom = newZoomState;
		}

		public void SwitchPistolInput(bool newSwitchPistolState)
		{
			switchPistol = newSwitchPistolState;
		}

		public void SwitchShotgunInput(bool newSwitchShotgunState)
		{
			switchShotgun = newSwitchShotgunState;
		}
				
		public void SwitchSniperInput(bool newSwitchSniperState)
		{
			switchSniper = newSwitchSniperState;
		}

		public void FlashlightInput(bool newFlashightState)
		{
			flashlight = newFlashightState;
		}

		public void ScrollInput(float newScrollDirection)
		{
			scroll = newScrollDirection;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}