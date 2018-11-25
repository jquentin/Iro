using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UITexture))]
public class LocalizeUITexture : LocalizeComponent {

	public string pathPrefix = "";


	UITexture _uiTexure;
	UITexture uiTexure
	{
		get
		{
			if (_uiTexure == null)
				_uiTexure = GetComponent<UITexture>();
			return _uiTexure;
		}
	}

	public override string value {
		set 
		{
			uiTexure.mainTexture = Resources.Load<Texture>(pathPrefix + value);
			int width = uiTexure.width;
			int height = uiTexure.height;
			UIWidget.AspectRatioSource aspectRatioSource = uiTexure.keepAspectRatio;
			uiTexure.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			uiTexure.MakePixelPerfect();
			uiTexure.keepAspectRatio = aspectRatioSource;
			uiTexure.width = width;
			uiTexure.height = height;
		}
	}

	protected override bool fixArabic {
		get {
			return false;
		}
	}
}
