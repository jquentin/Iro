using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;



public class CircularQueue<Type>:Queue<Type>
{
	
	public int maxSize { get; private set; }
	
	public CircularQueue(int size):base(size)
	{
		maxSize = size;
	}
	
	public void EnqueueWithLimit(Type element)
	{
		Enqueue(element);
		if (Count > maxSize)
			Dequeue();
	}
}

public static class CollectionUtils 
{

	public static List<List<T>> Combinations<T>(this List<T> elements, int k)
	{
		if (k > elements.Count)
			return new List<List<T>>();
		else if (k == elements.Count)
			return new List<List<T>>(){new List<T>(elements)};
		else if (k == 1)
			return elements.Select(t => new List<T>(){t}).ToList();
		
		List<List<T>> res = new List<List<T>>();
		for(int i = 0 ; i <= elements.Count - k ; i++)
		{
			List<List<T>> subCombs = Combinations(elements.GetRange(i + 1, elements.Count - (i + 1)), k - 1);
			subCombs.ForEach(list => list.Insert(0, elements[i]));
			res.AddRange(subCombs);
		}
		return res;
	}

	public static string ListToString<T>(this List<T> elements)
	{
		if (elements == null)
			return "null";
		string res = "{ ";
		foreach(T t in elements)
		{
			res += t.ToString() + " ";
		}
		res += "}";
		return res;
	}

}
