using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Common
{
	public delegate void Confim();
	public class MessageBox 
	{
		static GameObject Messagebox;
		static int Result = -1;
		public static Confim confim;
		public static string TitleStr;
		public static string ContentStr;

		public static void Show(string content)
		{          
			ContentStr = content;
			Messagebox = (GameObject)Resources.Load("PopBG");
			Messagebox= GameObject.Instantiate(Messagebox,GameObject.Find("Canvas").transform) as GameObject;
			Messagebox.transform.DOScale (1, 0.3f);
			Messagebox.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			Messagebox.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			Messagebox.GetComponent<RectTransform>().offsetMax = Vector2.zero;      
			//Time.timeScale = 0;      
		}
		public static void Show(string title,string content)
		{
			TitleStr = title;
			ContentStr = content;
			Messagebox = (GameObject)Resources.Load("PopBG");
			Messagebox = GameObject.Instantiate(Messagebox, GameObject.Find("Canvas").transform) as GameObject;
			Messagebox.transform.DOScale (1, 0.3f);
			Messagebox.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			Messagebox.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			Messagebox.GetComponent<RectTransform>().offsetMax = Vector2.zero;    
			//Time.timeScale = 0;     
		}
		public static void Sure()
		{
			if (confim!= null)
			{
				confim();
				GameObject.Destroy(Messagebox);
				TitleStr = "标题";
				ContentStr = null;
				//Time.timeScale = 1;
			}
		}     
		public static void Double()
		{
//			Result = 2;
//			GameObject.Destroy(Messagebox);
//			TitleStr = "标题";
//			ContentStr = null;
			//Time.timeScale = 1;
		}
	}
}