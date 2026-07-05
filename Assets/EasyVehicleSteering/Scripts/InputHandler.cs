using UnityEngine;

namespace EasyVehicleSteering
{
	public static class InputHandler
	{
		private static bool useSimulated = false;
		private static float simulatedHorizontal = 0f;

		private static bool useSimulatedVertical = false;
		private static float simulatedVertical = 0f;

		private static bool simulatedBrake = false;

		public static void SetSimulatedHorizontal(float value)
		{
			simulatedHorizontal = Mathf.Clamp(value, -1f, 1f);
			useSimulated = true;
		}

		public static void ClearSimulated()
		{
			simulatedHorizontal = 0f;
			useSimulated = false;
		}

		public static void SetSimulatedVertical(float value)
		{
			simulatedVertical = Mathf.Clamp(value, -1f, 1f);
			useSimulatedVertical = true;
		}

		public static void ClearSimulatedVertical()
		{
			simulatedVertical = 0f;
			useSimulatedVertical = false;
		}

		public static void SetBrake(bool value)
		{
			simulatedBrake = value;
		}

		public static float Horizontal
		{
			get
			{
#if UNITY_EDITOR || UNITY_STANDALONE
				if (!useSimulated)
					return Input.GetAxisRaw("Horizontal");
				else
					return simulatedHorizontal;
#else
				if (useSimulated)
					return simulatedHorizontal;
				else
					return Input.GetAxisRaw("Horizontal");
#endif
			}
		}

		public static float Vertical
		{
			get
			{
#if UNITY_EDITOR || UNITY_STANDALONE
				if (!useSimulatedVertical)
					return Input.GetAxisRaw("Vertical");
				else
					return simulatedVertical;
#else
				if (useSimulatedVertical)
					return simulatedVertical;
				else
					return Input.GetAxisRaw("Vertical");
#endif
			}
		}

		public static bool IsBraking
		{
			get
			{
#if UNITY_EDITOR || UNITY_STANDALONE
				if (!useSimulatedVertical)
					return Input.GetKey(KeyCode.Space);
				else
					return simulatedBrake;
#else
				if (useSimulatedVertical)
					return simulatedBrake;
				else
					return Input.GetKey(KeyCode.Space);
#endif
			}
		}
	}
}
