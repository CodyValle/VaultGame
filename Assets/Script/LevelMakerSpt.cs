using UnityEngine;
using System.Collections;
using SimpleJSON;

public class LevelMakerSpt : MonoBehaviour
{
	void Start()
	{
		// Load the level script as a TextAsset
		TextAsset JSONAsset = Resources.Load("LevelScripts/test") as TextAsset;

		// Load the TextAsset as a JSON Object
		JSONNode N = JSON.Parse(JSONAsset.text);

		// DImension variables
		float wide = BoxPoolerSpt.Instance.getWidth();
		float height = BoxPoolerSpt.Instance.getHeight();

		// Place the boxes where they need to be
		foreach (JSONNode n in N["boxLocs"].Childs)
		{
			// Place the boxs in the specified locations
			GameObject box = BoxPoolerSpt.Instance.getBox();
			box.transform.position = new Vector2(n["x"].AsInt * wide, n["y"].AsInt * height);
		}

		// set up the start position vector
		Vector2 vec = new Vector2(N["startLoc"]["x"].AsInt * wide, N["startLoc"]["y"].AsInt * height);
		// Tell the character where to start
		GameManager.Instance.getCharacter().GetComponent<CharacterSpt>().setStart(vec);
	}
}
