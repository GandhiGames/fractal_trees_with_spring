using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FractalTree.Demo
{
	public class DemoSceneSwitcher : MonoBehaviour
	{
		public int numOfScenes = 2;

		private int m_CurrentSceneIndex;

		void Awake ()
		{
			DontDestroyOnLoad (gameObject);
		}

		void Update ()
		{
			if (Input.GetKeyUp (KeyCode.Return)) {
				LoadNextScene ();
			}
		}

		public void LoadNextScene ()
		{
			m_CurrentSceneIndex = (m_CurrentSceneIndex + 1) % numOfScenes;

			SceneManager.LoadScene (m_CurrentSceneIndex);
		}
	}
}