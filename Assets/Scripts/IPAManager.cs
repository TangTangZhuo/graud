﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;


public class IPAManager : MonoBehaviour
{

	public GameObject waiting;
	public Material goldNet;
	public Text accumulation;

	bool onClicking = false;
	private static IPAManager instance;

	public static IPAManager Instance {
		get{ return instance; }
	}

	void Awake ()
	{
		instance = this;
	}

	void Start(){
		accumulation.text = PlayerPrefs.GetInt ("accumulation", 0).ToString();
		UpdateIAPState ();
	}

	public void OnPurchaseFinish (Product product)
	{

		if (product != null) {
			Debug.Log ("success:" + product.definition.id);
			if (product.definition.id == "fishingpass") {
				PlayerPrefs.SetInt ("fishingpass", 1);
			}
			if (product.definition.id == "golden_net") {
				PlayerPrefs.SetInt ("golden_net", 1);
				SubmarineController.Instance.UpdateGoldMutiple ();
				transform.Find ("goldNet").gameObject.SetActive (false);
			}
		}
	}

	public void OnPurchaseFailed (Product product, PurchaseFailureReason reason)
	{
		if (product != null) {
			Debug.Log ("fiailed:" + product.definition.id + "reason:" + reason);
		}
	}

	void UpdateIAPState(){
		if (PlayerPrefs.GetInt ("fishingpass", 0) == 1) {
			Transform fishingPass = transform.Find ("fishingPass");
			Destroy (fishingPass.GetComponent<IAPButton> ());
			Button passBtn = fishingPass.GetComponent<Button> ();
			passBtn.onClick.RemoveAllListeners();
			passBtn.onClick.AddListener (OnCollectClick);
		}
		if (PlayerPrefs.GetInt ("fishingpass", 0) == 0) {
			transform.Find ("fishingPass").GetComponent<Button> ().onClick.AddListener (OnFishVipClick);
		}
		if (PlayerPrefs.GetInt ("golden_net", 0) == 1) {
			transform.Find ("goldNet").gameObject.SetActive (false);
		}
	}

	void OnCollectClick(){
		int gold = 0;
		gold = PlayerPrefs.GetInt ("gold", 0) + PlayerPrefs.GetInt ("accumulation", 0);
		PlayerPrefs.SetInt ("gold", gold);
		UIManager.Instance.goldT.text = gold.ToString ();
		Upgrading.Instance.CheckGold();
		UpgradingOffline.Instance.CheckGold();
		PlayerPrefs.SetInt ("accumulation", 0);
		accumulation.text = "0";
	}



	public void OnFishVipClick ()
	{
	}

	public void OnGoldNetClick ()
	{
	}

	public void OnRestoreClick ()
	{

	}
		


	//	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e){
	//		if (string.Equals (e.purchasedProduct.definition.id, "fishingpass", StringComparison.Ordinal)) {
	//			print ("fishingpass");
	//		}
	//		if (string.Equals (e.purchasedProduct.definition.id, "golden_net", StringComparison.Ordinal)) {
	//			print ("golden_net");
	//		}
	//		waiting.SetActive (false);
	//		return PurchaseProcessingResult.Complete;
	//	}
	//
	//	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	//	{
	//		extensions.GetExtension<IAppleExtensions> ().RestoreTransactions (result => {
	//			if (result) {
	//				Debug.Log("restoration process succeeded");
	//			} else {
	//				Debug.Log("Restoration failed");
	//			}
	//		});
	//	}
}

