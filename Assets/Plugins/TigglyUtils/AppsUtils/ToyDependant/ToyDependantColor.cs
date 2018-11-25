using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if TMProFadeEnabled
using TMPro;
#endif

public class ToyDependantColor : MonoBehaviour {
	
	private SpriteRenderer spriteRenderer;
	private MeshRenderer meshRenderer;
	#if TMProFadeEnabled
	private TextMeshPro textMeshProRenderer;
	#endif
	private UIWidget NGUIWidget;
	private AnimatedColor animatedColorComponent;

	[Serializable]
	class ToyColorCombination
	{
		public TigglyConstants.Toys toy;
		public Color color;
	}

	[SerializeField]
	private List<ToyColorCombination> colors;

	
	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		meshRenderer = GetComponent<MeshRenderer>();
		#if TMProFadeEnabled
		textMeshProRenderer = GetComponent<TextMeshPro>();
		if (textMeshProRenderer != null)
			meshRenderer = null;
		#endif
		NGUIWidget = GetComponent<UIWidget>();
		animatedColorComponent = GetComponent<AnimatedColor>();
	}

	void Start () 
	{
		Color c = Color.black;
		foreach (ToyColorCombination comb in colors)
			if (comb.toy == TigglyConstants.instance.mainToys)
				c = comb.color;
		if (spriteRenderer != null)
			spriteRenderer.color = c;
		if (meshRenderer != null)
			meshRenderer.material.color = c;
		
		#if TMProFadeEnabled
		if (textMeshProRenderer != null)
			textMeshProRenderer.color = c;
		#endif
		if (NGUIWidget != null)
			NGUIWidget.color = c;
		if (animatedColorComponent != null)
			animatedColorComponent.color = c;
	}

}
