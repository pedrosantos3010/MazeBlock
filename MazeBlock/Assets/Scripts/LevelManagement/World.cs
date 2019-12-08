using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Create World")]
public class World : ScriptableObject {
	[Header("Basic Information")]
	public Color worldColor;
	public int number;
	public int levelCount;
	public int levelsNeeded;

	[HideInInspector]
	public int completeLevelsCount;
	[HideInInspector]
	public bool[] levelIsComplete;

	int currentPage;
	public int CurrentPage {
		get { return currentPage; }
	}
		
	public bool UpdatePage(int direction) {
		
		if (direction == 0) {
			currentPage = 0;
			return false;
		}

		if (currentPage + direction >= 0) {
			if ((currentPage + direction) * 12 < levelCount) {
				currentPage += direction;
				return true;
			}
		}

		return false;
	}

	public void SaveAll () {
		string savetxt = "W" + number.ToString() + "L";

		for (int i = 0; i < levelCount; i++) {
			PlayerPrefs.SetInt (savetxt + i.ToString (), (levelIsComplete [i] ? 1 : 0));
		}
	}

	public void WinAll () {
		string savetxt = "W" + number.ToString() + "L";

		for (int i = 0; i < levelCount; i++) {
			PlayerPrefs.SetInt (savetxt + i.ToString (),  1);
		}
	}

	public void SaveLevel (int l_number) {
		l_number -= 1;//Converting to index
		string savetxt = "W" + number.ToString() + "L" + l_number.ToString();

		completeLevelsCount += 1;
		PlayerPrefs.SetInt (savetxt, 1);
	}

	public void Load () {
		string savetxt = "W" + number.ToString() + "L";

		completeLevelsCount = 0;
		levelIsComplete = new bool[levelCount];

		for (int i = 0; i < levelCount; i++) {
			levelIsComplete[i] = (PlayerPrefs.GetInt (savetxt + i.ToString ()) == 1 ? true : false);

			if (levelIsComplete[i])
				completeLevelsCount += 1;
		}

		GameManager.Instance.totalCompleteLevels += completeLevelsCount;
	}
}