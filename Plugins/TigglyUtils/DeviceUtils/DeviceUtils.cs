using UnityEngine;
using System;
using System.Collections;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public static class DeviceUtils 
{

	static QualityLevel qualityLevelInEditor = QualityLevel.Low;

	public enum QualityLevel {Any, Low, High}
	public static QualityLevel qualityLevel
	{
		get
		{
			#if UNITY_IOS && !UNITY_EDITOR
			switch (Device.generation)
			{
			case DeviceGeneration.iPad1Gen:
			case DeviceGeneration.iPad2Gen:
			case DeviceGeneration.iPad3Gen:
			case DeviceGeneration.iPadMini1Gen:
			case DeviceGeneration.iPhone4:
			case DeviceGeneration.iPhone4S:
			case DeviceGeneration.iPodTouch1Gen:
			case DeviceGeneration.iPodTouch2Gen:
			case DeviceGeneration.iPodTouch3Gen:
			case DeviceGeneration.iPodTouch4Gen:
			case DeviceGeneration.iPodTouch5Gen:
			return QualityLevel.Low;
			default:
			return QualityLevel.High;
			}
			#endif
			return qualityLevelInEditor;
		}
	}
	
	private static float _aspectRatio = float.NaN;
	public static float aspectRatio
	{
		get
		{
			if (float.IsNaN(_aspectRatio))
				_aspectRatio = (float)Screen.width / (float)Screen.height;
			return _aspectRatio;
		}
	}

	public static bool isOldDevice
	{
		get
		{
			#if UNITY_EDITOR
			return true;
			#elif UNITY_IOS
			return (SystemInfo.systemMemorySize < 700 || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad3Gen);
			#else
			return (SystemInfo.systemMemorySize < 700);

			#endif
		}
	}
}
