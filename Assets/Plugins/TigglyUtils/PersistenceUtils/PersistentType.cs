using UnityEngine;
using System;
using System.Collections;

public abstract class PersistentType<Type> 
{

	public Type initialValue;
	public string key;

	protected abstract void Serialize(Type value);

	protected abstract Type Deserialize();

	public PersistentType (string key, Type initialValue)
	{
		this.key = key;
		this.initialValue = initialValue;
	}

	public Type value
	{
		get
		{
			return Deserialize();
		}
		set
		{
			SetValue(value);
		}
	}

	public void SetValue(Type value)
	{
		Serialize(value);
	}

	public static implicit operator Type(PersistentType<Type> persistentType)
	{
		return persistentType.value;
	}

}
