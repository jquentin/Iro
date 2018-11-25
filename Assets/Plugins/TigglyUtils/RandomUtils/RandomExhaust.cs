using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

/// <summary>
/// This class can be used to randomly pick elements from a list while not repeating the same element shortly.
/// It will keep a list of the last picked values, and exclude a certain number of it from the picking.
/// 
/// The list among which the elements should be picked can be modified at any moment.
/// 
/// This list can be persistent if a persistencyKey is given, so the picked elements won't repeat, even among successive sessions.
/// 
/// The Pick function will return a random element of the list, excluding a certain number of last picked elements.
/// This number of excluded elements is calculated differently based on the chosen exclusionType.
///
/// FixedExcludedElmts:
/// This mode is to avoid having the same elements being picked in a row.
/// In this mode, the exclusionNumber parameter represents the minimum distance between 2 picks of the same element (if the size of the list allows it).
/// Greater values for exclusionNumber mean less randomness in the picking and more looping.
/// If = 0, the list will pick a random element, with chances of repeating several times the same element.
/// If = 1, the list will avoid picking twice the same element in a row.
/// If = 2, it will avoid picking the last picked element or the one picked before.
/// Etc... 
/// If >= list.Count - 1, the picking will loop with no more randomness.
/// 
/// FixedRandomizedElmts:
/// This mode is to loop the elements of the list without any repetition, even distant.
/// To avoid having the same order looping over, you can set the exclusionNumber parameter to a value greater than 1.
/// The exclusionNumber parameter represents the difference between the size of the list and the number of elements that can be saved in
/// the excluded elements list. 
/// In other words, in this mode exclusionNumber is the number of elements among which the picked element is picked.
/// It is the number of elements NOT being excluded.
/// Greater values for exclusionNumber mean more randomness in the picking.
/// If = 1, while the list stays the same, once the excluded list is filled, the order will stay the same. 
/// This will perfectly loop the same order with no randomness.(similar to situation in mode FixedExcludedElmts with exclusionNumber >= list.Count - 1)
/// If = 2, then, the picked number will be chosen among the 2 oldest picked values, which will vary the pick order among time.
/// This will have similar effect to mode FixedExcludedElmts with exclusionNumber == list.Count - 2.
/// Etc...
/// Note: In this mode, values < 1 are meaningless and will be reduced to the same effect as a value of 1, because the picking needs at least one element among which to choose from.
/// 
/// As this is a generic class, Unity cannot display it in the editor.
/// But any derived non-generic class will be displayed in Unity editor.
/// A few common use-cases are implemented in this file for convenience (RandomExhaustInt, RandomExhaustAudioClip).
/// For any other use, create a similar derived non-generic class resolving the generic type with the type of your choice.
/// 
/// </summary>

[Serializable]
public enum ExclusionMode { FixedRandomizedElmts, FixedExcludedElmts }

[Serializable]
public class RandomExhaust<Type>
{
	public List<Type> list;
	public bool useFirstToPick = false;
	public Type firstToPick;
	private List<Type> lastPicked = new List<Type>();
	private int nbToExclude
	{
		get
		{
			int res;
			if (exclusionMode == ExclusionMode.FixedRandomizedElmts)
			{
				res = list.Count - exclusionNumber;
			}
			else
			{
				res = exclusionNumber;
			}
			// Avoid to return a negative number in cases where the list is smaller than the exclusionNumber in FixedRandomizedElmts mode
			// Also avoid to exclude all elements of the list, or the Pick won't be able to choose any element
			return Mathf.Max(0, Mathf.Min(list.Count - 1, res));
		}
	}
	/// <summary>
	/// Returns the list of elements to be excluded of the current Pick
	/// It will take a range of the lastPicked elements with a size based on the exclusionType and the parameters nbNonExcludedElements and nbExcludedElements.
	/// </summary>
	/// <value>The list of elements to be excluded of the current Pick.</value>
	private List<Type> excluded
	{
		get
		{
			lastPicked.Reverse();
			List<Type> res = lastPicked.GetRange(0, Math.Min(lastPicked.Count, nbToExclude));
			lastPicked.Reverse();
			return res;
		}
	}

	[SerializeField]
	private ExclusionMode exclusionMode = ExclusionMode.FixedRandomizedElmts;
	[SerializeField]
	private int exclusionNumber = 1;
	[SerializeField]
	private int maxSizeSaved;
	private bool persistent
	{
		get
		{
			return !string.IsNullOrEmpty(persistencyKey);
		}
	}
	[SerializeField]
	private string persistencyKey;
	private string persistencyPath
	{
		get
		{
			return GetPerstencyPath(persistencyKey);
		}
	}

	private static string GetPerstencyPath(string key)
	{
		return Application.persistentDataPath + "/" + key;
	}
	
	public RandomExhaust(List<Type> list, int exclusionNumber = 1, string persistencyKey = "", int maxSizeSaved = 30, bool useFirstToPick = false, Type firstToPick = default(Type))
	{
		this.list = list;
		this.exclusionMode = ExclusionMode.FixedRandomizedElmts;
		this.persistencyKey = persistencyKey;
		this.maxSizeSaved = maxSizeSaved;
		this.exclusionNumber = exclusionNumber;
		this.useFirstToPick = useFirstToPick;
		this.firstToPick = firstToPick;
		if (persistent)
		{
			LoadExcludingList();
			UpdateExcludingList();
		}
	}

	public RandomExhaust(List<Type> list, ExclusionMode exclusionMode, int exclusionNumber = 1, string persistencyKey = "", int maxSizeSaved = 30, bool useFirstToPick = false, Type firstToPick = default(Type))
	{
		this.list = list;
		this.exclusionMode = exclusionMode;
		this.persistencyKey = persistencyKey;
		this.maxSizeSaved = maxSizeSaved;
		this.exclusionNumber = exclusionNumber;
		this.useFirstToPick = useFirstToPick;
		this.firstToPick = firstToPick;
		if (persistent)
		{
			LoadExcludingList();
			UpdateExcludingList();
		}
	}

	public RandomExhaust()
	{
		this.list = null;
		this.persistencyKey = "";
		this.maxSizeSaved = 30;
		this.exclusionNumber = 1;
		this.useFirstToPick = false;
		this.firstToPick = default(Type);
		if (persistent)
		{
			LoadExcludingList();
			UpdateExcludingList();
		}
	}
	
	public void AddRange(List<Type> elements)
	{
		list.AddRange(elements);
		UpdateExcludingList();
	}
	
	public void Add(Type element)
	{
		list.Add(element);
		UpdateExcludingList();
	}
	
	
	public void Remove(Type element)
	{
		list.Remove(element);
		UpdateExcludingList();
	}
	
	public void RemoveRange(int index, int count)
	{
		list.RemoveRange(index, count);
		UpdateExcludingList();
	}
	
	public void UpdateList(List<Type> elements)
	{
		list.Clear();
		list.AddRange(elements);
		UpdateExcludingList();
	}
	
	private void SaveExcludingList()
	{
		XmlSerializer formatter = new XmlSerializer(lastPicked.GetType());
		Stream stream = new FileStream(persistencyPath, FileMode.Create, FileAccess.Write, FileShare.None);
		formatter.Serialize(stream, lastPicked);
		stream.Close();
		
	}
	private void LoadExcludingList()
	{
		XmlSerializer formatter = new XmlSerializer(lastPicked.GetType());
		if (File.Exists(persistencyPath))
		{
			Stream stream = new FileStream(persistencyPath, FileMode.Open, FileAccess.Read, FileShare.Read);
			lastPicked = (List<Type>) formatter.Deserialize(stream);
			stream.Close();
		}
		UpdateExcludingList();
		Debug.Log("Loaded lastedPicked list: " + PrintLastPicked());
	}
	
	private void UpdateExcludingList()
	{
//		if (excluded.Count > list.Count - nbNonExcludedElements)
//		{
//			excluded.RemoveRange(0, excluded.Count - (list.Count - nbNonExcludedElements));
//			if (persistent)
//				SaveExcludingList();
//		}
	}
	
	private void AddToExcludingList(Type element)
	{
		lastPicked.Add(element);
		if (lastPicked.Count > maxSizeSaved)
			lastPicked.RemoveRange(0, lastPicked.Count - maxSizeSaved);
		UpdateExcludingList();
		if (persistent)
			SaveExcludingList();
	}
	
	public Type Pick()
	{
		UpdateExcludingList();
		Type pickedElement;
		if (useFirstToPick && excluded.Count == 0)
			pickedElement = firstToPick;
		else
			pickedElement = RandomUtils.RandomValueInListExcluding(list, excluded);
		Debug.Log("Pick element: " + pickedElement.ToString() + " among: " + PrintElements() + " excluding: " + PrintExcluded());
		AddToExcludingList(pickedElement);
		return pickedElement;
	}

	//TODO in case useful, use Pick several times
//	public List<Type> PickList(int number)
//	{
//		return null;
//	}

	public override string ToString ()
	{
		return string.Format ("[RandomExhaust: list=$1 ; lastPicked=$2 ; excluded=$3", PrintElements(), PrintLastPicked(), PrintExcluded());
	}

	private string PrintList(List<Type> list)
	{
		string res = "{ ";
		for (int i = 0 ; i < list.Count ; i++)
		{
			res += list[i].ToString();
			if (i < list.Count - 1)
				res += " ; ";
		}
		res += " }";
		return res;
	}
	
	private string PrintElements()
	{
		return PrintList(list);
	}
	
	private string PrintExcluded()
	{
		return PrintList(excluded);
	}

	private string PrintLastPicked()
	{
		return PrintList(lastPicked);
	}

	public void ClearHistory()
	{
		lastPicked.Clear();
		File.Delete(persistencyPath);
	}
	
	public static void ClearPersistentHistory(string persistencyKey)
	{
		File.Delete(GetPerstencyPath(persistencyKey));
	}
	
}


[Serializable]
public class RandomExhaustInt : RandomExhaust<int> 
{
	public RandomExhaustInt(List<int> list, int exclusionNumber = 1, string persistencyKey = "", int maxSizeSaved = 30)
		: base(list, exclusionNumber, persistencyKey, maxSizeSaved) { }

	public RandomExhaustInt(List<int> list, ExclusionMode exclusionMode, int exclusionNumber = 1, string persistencyKey = "", int maxSizeSaved = 30) 
		: base(list, exclusionMode, exclusionNumber, persistencyKey, maxSizeSaved) { }
		
	public RandomExhaustInt() 
		: base () { }

	/// <summary>
	/// Initializes a new instance of the <see cref="RandomExhaustInt"/> class with a list of numbers ranging from rangeStart to rangeEnd.
	/// </summary>
	public RandomExhaustInt(int rangeStart, int rangeEnd, ExclusionMode exclusionMode, int exclusionNumber = 1, string persistencyKey = "", int maxSizeSaved = 30)
		: base (Enumerable.Range(rangeStart, rangeEnd - rangeStart + 1).ToList(), exclusionMode, exclusionNumber, persistencyKey, maxSizeSaved) { }

}


[Serializable]
public class RandomExhaustAudioClip : RandomExhaust<AudioClip> 
{

	public RandomExhaustAudioClip(List<AudioClip> list, int exclusionNumber = 1, string persistencyKey = "", int maxSizeSaved = 30)
		: base(list, exclusionNumber, persistencyKey, maxSizeSaved) { }

	public RandomExhaustAudioClip(List<AudioClip> list, ExclusionMode exclusionMode, int exclusionNumber = 1, string persistencyKey = "", int maxSizeSaved = 30) 
		: base(list, exclusionMode, exclusionNumber, persistencyKey, maxSizeSaved) { }

	public RandomExhaustAudioClip() 
		: base () { }
	
}