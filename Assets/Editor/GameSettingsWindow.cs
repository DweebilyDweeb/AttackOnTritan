using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameSettingsWindow : EditorWindow {

	//Update Timer
	float updateInterval = 1.0f;
	float updateTimer = 0.0f;

	//Monsters
	bool allowMonsterCollision = false;

	[MenuItem("Window/Game Settings")]
	static void ShowWindow() {
		//Get existing open window or if none, make a new one.
		GameSettingsWindow window = (GameSettingsWindow)EditorWindow.GetWindow(typeof(GameSettingsWindow), false, "Game Settings");
		window.Show();
	}

	void OnGUI() {
		//Set our label.
		OnGUIMonsterSettings();

		//groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		//myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		//EditorGUILayout.EndToggleGroup ();

		GUILayout.Label("Tell me if you guys wanna add more stuff. I'll teach you. -Terry", EditorStyles.boldLabel);
	}

	void Update() {
		//Only update every few seconds.
		if (updateTimer > 0.0f) {
			updateTimer -= Time.deltaTime;
			return;
		}
		updateTimer = updateInterval;

		UpdateMonsterSettings();
		//GUILayout.Label("Monsters", EditorStyles.boldLabel);
		//myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		//monsterSettingsGroup = EditorGUILayout.BeginToggleGroup ("Monster Settings", monsterSettingsGroup);
		//EditorGUILayout.EndToggleGroup ();
	}

	//Monster
	void OnGUIMonsterSettings() {
		GUILayout.Label("Monsters", EditorStyles.boldLabel);
		allowMonsterCollision = EditorGUILayout.Toggle ("Enable Monster Collision", allowMonsterCollision);
	}

	void UpdateMonsterSettings() {
		//Monsters
		if (allowMonsterCollision) {
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Monster"), LayerMask.NameToLayer("Monster"), false);
		} else {
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Monster"), LayerMask.NameToLayer("Monster"), true);
		}
	}

	void OnGUIGraphicalSettings() {
		GUILayout.Label("Graphical Settings", EditorStyles.boldLabel);

	}

}