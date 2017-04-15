using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterBuilder : TigglyNetworkBehaviour {

	public bool isPlayable;

	public Transform bodyContainer;
	public List<GameObject> bodies;

	GameObject instantiatedBody;

	[SyncVar]
	public int selectedBodyIndex = -1;

	static int chosenColors = 0;

//	void Awake () 
//	{
//		Init();
//	}

	// Now used only in context menu and not runtime, because the network is messed if we replace the components at runtime
	[ContextMenu("Init")]
	public void Init () 
	{
		if (!isPlayable)
		{
			IroCharacterController characterController = GetComponent<IroCharacterController>();
			ControllableColorBeing colorBeing = GetComponent<ControllableColorBeing>();
			PlayerWeaponContainer weaponContainer = GetComponentInChildren<PlayerWeaponContainer>();

			AICharacterController newCharacterController = gameObject.AddComponent<AICharacterController>();
			newCharacterController.body = characterController.body;
			newCharacterController.moveSpeed = characterController.moveSpeed;
			ColorBeing newColorBeing = gameObject.AddComponent<ColorBeing>();
			newColorBeing.color = colorBeing.color;
			newColorBeing.maxHealth = colorBeing.maxHealth;
			newColorBeing.healthBar = colorBeing.healthBar;
			WeaponContainer newWeaponContainer = weaponContainer.gameObject.AddComponent<AIWeaponContainer>();
			newWeaponContainer.weaponComponents = weaponContainer.weaponComponents;
			newWeaponContainer.currentWeaponIndex = weaponContainer.currentWeaponIndex;

			GetComponent<NetworkIdentity>().localPlayerAuthority = false;
			GetComponent<NetworkIdentity>().serverOnly = true;

			DestroyOrDestroyImmediate(colorBeing.colorWheel.gameObject);
			DestroyOrDestroyImmediate(characterController);
			DestroyOrDestroyImmediate(colorBeing);
			DestroyOrDestroyImmediate(weaponContainer);
		}
	}

	void DestroyOrDestroyImmediate(Object o)
	{
		if (Application.isPlaying)
			Destroy(o);
		else
			DestroyImmediate(o, true);
	}

	void Start()
	{
		if (isLocalPlayer)
		{
			if (bodies != null && bodies.Count > 0)
				ModeDependantCall(CmdSelectBody, SelectBody);
//			ModeDependantCall(CmdSelectColor, SelectColor);
			if (offlineMode)
				SelectColor();
			else
				CmdSelectColor();
			Camera.main.transform.parent = this.transform;
			Camera.main.transform.localPosition = new Vector3(0f, 0f, Camera.main.transform.localPosition.z);
			gameObject.tag = "Player";
		}
//		else
//		{
//			SpawnBody(selectedBodyIndex);
//		}
	}

	[Command]
	void CmdSelectBody()
	{
		SelectBody();
	}
	void SelectBody()
	{
		RandomExhaustInt chosenBodies = new RandomExhaustInt(0, bodies.Count - 1, ExclusionMode.FixedRandomizedElmts, 1, "ChosenBodies", bodies.Count);
		selectedBodyIndex = chosenBodies.Pick();
//		RpcSpawnBody(selectedBodyIndex);
	}
	[Command]
	void CmdSelectColor()
	{
		SelectColor();
	}
	void SelectColor()
	{
		Color color = Color.HSVToRGB(0.25f * chosenColors, 1f, 1f);
		chosenColors = (chosenColors + 1) % 4;
		ColorBeing colorBeing = GetComponent<ColorBeing>();
		colorBeing.color = color;
	}

//	void Update()
//	{
//		if (instantiatedBody == null && selectedBodyIndex >= 0)
//			SpawnBody(selectedBodyIndex);
//	}

	void OnSelectedBodyIndex(int selectedBody)
	{
		SpawnBody(selectedBody);
	}

//	[ClientRpc]
//	void RpcSpawnBody(int bodyIndex)
//	{
//		SpawnBody(bodyIndex);
//	}

	void SpawnBody(int bodyIndex)
	{
		instantiatedBody = Instantiate(bodies[bodyIndex], bodyContainer);
		instantiatedBody.transform.localPosition = Vector3.zero;
		instantiatedBody.transform.localRotation = Quaternion.identity;
		GetComponent<ColorBeing>().UpdateColorSprites();
	}

}
