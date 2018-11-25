using UnityEngine;
using System;
using System.Collections;

public static class JSONUtils
{

	public static string GetImageFromNode (SimpleJSON.JSONNode node)
	{
		string resImage = "";
		SimpleJSON.JSONNode condImgNode = node["conditional-image"];
		if (condImgNode != null)
		{
			SimpleJSON.JSONArray array = condImgNode.AsArray;
			// if quality level dependant image
			if (array != null)
			{
				foreach(SimpleJSON.JSONNode n in array)
				{
					string image = n["specific-image"].Value;

					bool isDefault = n["is-default"].AsBool;
					if (isDefault && string.IsNullOrEmpty(resImage))
						resImage = image;

					DeviceUtils.QualityLevel level = DeviceUtils.QualityLevel.Any;
					if (n["quality-level"] != null)
					{
						level = (DeviceUtils.QualityLevel)Enum.Parse(typeof(DeviceUtils.QualityLevel), n["quality-level"].Value);
					}
					string language = string.Empty;
					if (n["language"] != null)
					{
						language = n["language"].Value;
					}

					if ((string.IsNullOrEmpty(language) && string.IsNullOrEmpty(resImage) || language == Localization.language) 
						&& (level == DeviceUtils.QualityLevel.Any && string.IsNullOrEmpty(resImage) || level == DeviceUtils.qualityLevel))
						resImage = image;
				}
			}
		}
		// if simple image
		if (string.IsNullOrEmpty(resImage))
			resImage = node["image"].Value;
		Debug.Log(resImage.ToString());
		return resImage;
	}

}
