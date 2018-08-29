using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Common;

public class UIMessage : MonoBehaviour {
	public Text Title;
	public Text Content;//这个是Content下的text
	public Button Sure;
	public Button Double;
	void Start()
	{
		Sure.onClick.AddListener(MessageBox.Sure);
		Double.onClick.AddListener(MessageBox.Double);
		Title.text =MessageBox.TitleStr;
		Content.text = MessageBox.ContentStr;
	}
}