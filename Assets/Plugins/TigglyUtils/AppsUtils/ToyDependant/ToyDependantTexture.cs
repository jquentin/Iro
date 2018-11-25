using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(UITexture))]
public class ToyDependantTexture : MonoBehaviour {

	private UITexture _NGUITexture;
	private UITexture NGUITexture
	{
		get
		{
			if (_NGUITexture == null)
				_NGUITexture = GetComponent<UITexture>();
			return _NGUITexture;
		}
	}

	private UnityEngine.Object oldParentFolder;
	public UnityEngine.Object parentFolder;

	public string iconName;

	[Serializable]
	class ToyTextureCombination
	{
		public TigglyConstants.Toys toy;
		public Texture texture;
	}

	[SerializeField]
	private List<ToyTextureCombination> textures;

	void Start () 
	{
		Texture t = null;
		foreach(ToyTextureCombination comb in textures)
			if (comb.toy == TigglyConstants.instance.mainToys)
				t = comb.texture;
		if (NGUITexture != null)
			NGUITexture.mainTexture = t;
	}

#if UNITY_EDITOR

	[ContextMenu("Update texture")]
	public void UpdateTexture () 
	{
		string path = AssetDatabase.GetAssetPath(parentFolder) + "/" + TigglyConstants.tigglyApp.mainToys + "/" + iconName + ".png";

//		path = AssetDatabase.LoadAssetAtPath<Texture>(path);
	}

	void OnEnable()
	{
		if (!Application.isPlaying)
		{
			UpdateTexture();
			TigglyConstants.OnAppCodeChanged += UpdateTexture;

		}
	}

//	void Update()
//	{
//		if (!Application.isPlaying && (oldParentFolder != parentFolder || oldName != name))
//		{
//			UpdateTexture();
//			oldParentFolder = parentFolder;
//			oldName = name;
//		}
//	}

#endif

}
