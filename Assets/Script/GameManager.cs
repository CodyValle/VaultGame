using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
	[Header("References")]

	[Tooltip("Reference to the Player")]
	public GameObject character;
	
	// Singleton, so protected constructor
	protected GameManager() {}

	public GameObject getCharacter()
	{
		return character;
	}
}
