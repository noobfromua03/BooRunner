using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClockSample
{

	public class Clock : MonoBehaviour
	{
		public Transform handHours;
		public Transform handMinutes;
		public Transform handSeconds;

		private void Start()
		{
			//we are only updating everything once a second since this is sufficient for our clock
			InvokeRepeating(nameof(UpdateHands), 0, 1);
		}

		void UpdateHands()
		{
			//get the current time and convert it to hand rotation
			float handRotationHours		= System.DateTime.Now.Hour		* 30;	//360/12 = 30
			float handRotationMinutes	= System.DateTime.Now.Minute	* 6;	//360/60 = 6
			float handRotationSeconds	= System.DateTime.Now.Second	* 6;	//360/60 = 6

			//create vectors that we can assign to the transforms
			Vector3 hoursVec	= new Vector3(0, 0, handRotationHours);
			Vector3 minutesVec	= new Vector3(0, 0, handRotationMinutes);
			Vector3 secondsVec	= new Vector3(0, 0, handRotationSeconds);

			//assign the rotation to the hand Transforms
			if (handHours)
			{
				handHours.localEulerAngles = hoursVec;
			}

			if (handMinutes)
			{
				handMinutes.localEulerAngles = minutesVec;
			}

			if (handSeconds)
			{
				handSeconds.localEulerAngles = secondsVec;
			}
		}

		private void OnDestroy()
		{
			CancelInvoke();
		}
	}

}