using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;

public class TimeManager : MonoBehaviour {

	DateTime currentDate;
	DateTime oldDate;
	void Start()
	{		

	}

	void OnApplicationQuit()
	{
		//Savee the current system time as a string in the player prefs class
		PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());

	}

	void OnApplicationPause(bool isPause){
		if (isPause) {
			PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());
		} else {
			UpdateGold ();
		}
	}

	int OfflineTime(){
		//Store the current time when it starts
		currentDate = System.DateTime.Now;

		//Grab the old time from the player prefs as a long
		long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString",currentDate.ToBinary().ToString()));

		//Convert the old time from binary to a DataTime variable
		DateTime oldDate = DateTime.FromBinary(temp);

		//Use the Subtract method and store the result as a timespan variable
		TimeSpan difference = currentDate.Subtract(oldDate);

		return difference.Minutes;
	}

	void UpdateGold(){
		int min = OfflineTime ();
		if (min > 0) {
			MessageBox.Show ("Offline", "$" + min * PlayerPrefs.GetInt ("valueOffline", 4));
			MessageBox.confim=()=>{
				int gold = PlayerPrefs.GetInt ("gold", 0) + min * PlayerPrefs.GetInt ("valueOffline", 4);
				PlayerPrefs.SetInt ("gold", gold);
				UIManager.Instance.goldT.text = gold.ToString ();
				Upgrading.Instance.CheckGold ();
				UpgradingOffline.Instance.CheckGold ();
			};
		}
	}
}
