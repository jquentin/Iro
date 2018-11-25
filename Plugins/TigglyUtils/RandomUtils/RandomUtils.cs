using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class RandomUtils {

	public static List<int> RandomDifferentValues(int nbValues, int min, int max)
	{
		if (nbValues > max - min)
		{
			Debug.LogError("Calling RandomList with an interval smaller than the number of different elements expected");
			return null;
		}
		List<int> nbs = new List<int>();
		for (int i = 0 ; i < nbValues ; i++)
		{
			bool found = false;
			int nb = -1;
			while (!found)
			{
				nb = Random.Range(min, max);
				if (!nbs.Contains(nb))
				{
					found = true;
				}
			}
			nbs.Add(nb);
		}
		return nbs;
	}
	
	public static List<Type> RandomDifferentValuesInList<Type>(int nbValues, List<Type> elements, List<Type> excluding = null)
	{
		if (excluding == null)
			excluding = new List<Type>();
		if (nbValues > elements.Count - excluding.Count)
		{
			Debug.LogError(string.Format("Calling RandomList with a list smaller than the number of different elements expected : nbValues = {0} ; elements = {1} ; excluding = {2}", nbValues, elements.ListToString(), excluding.ListToString()));
			return null;
		}
		List<Type> res = new List<Type>();
		for (int i = 0 ; i < nbValues ; i++)
		{
			bool found = false;
			Type elmt = default(Type);
			while (!found)
			{
				elmt = elements[Random.Range(0, elements.Count)];
				if (!res.Contains(elmt) && !excluding.Contains(elmt))
				{
					found = true;
				}
			}
			res.Add(elmt);
		}
		return res;
	}
	
	public static Type RandomValueInListExcluding<Type>(List<Type> elements, List<Type> excluding = null)
	{
		List<Type> chosenElementList = RandomDifferentValuesInList(1, elements, excluding);
		if (chosenElementList != null && chosenElementList.Count > 0)
			return chosenElementList[0];
		else
			return default(Type);
	}

	public static Type PickRandomElement<Type>(this List<Type> elements)
	{
		if (elements == null || elements.Count == 0)
			return default(Type);
		return elements[Random.Range(0, elements.Count)];
	}

	public static List<Type> PickRandomElements<Type>(this List<Type> elements, int nbToPick, List<Type> excluding = null)
	{
		return RandomDifferentValuesInList(nbToPick, elements, excluding);
	}
}
