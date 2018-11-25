using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
public class ToyDependantAtlas : MonoBehaviour {

	private UISprite _sprite;
	private UISprite sprite
	{
		get
		{
			if (_sprite == null)
				_sprite = GetComponent<UISprite>();
			return _sprite;
		}
	}

	private UIAtlas atlas
	{
		get
		{
			return sprite.atlas;
		}
		set
		{
			sprite.atlas = value;
		}
	}
	
	private Object oldParentFolder;
	public Object parentFolder;
	
	private string oldName;
	public string name;

#if UNITY_EDITOR

	[ContextMenu("Update atlas")]
	public void UpdateAtlas () 
	{
		string path = AssetDatabase.GetAssetPath(parentFolder) + "/" + TigglyConstants.tigglyApp.mainToys + "/" + name + ".prefab";
		atlas = AssetDatabase.LoadAssetAtPath<UIAtlas>(path);
	}

	void OnEnable()
	{
		if (!Application.isPlaying)
		{
			UpdateAtlas();
			TigglyConstants.OnAppCodeChanged += UpdateAtlas;

		}
	}

	void Update()
	{
		if (!Application.isPlaying && (oldParentFolder != parentFolder || oldName != name))
		{
			UpdateAtlas();
			oldParentFolder = parentFolder;
			oldName = name;
		}
	}

#endif

}


//[InitializeOnLoad]
//class InitializeAtlases
//{
//
//}
