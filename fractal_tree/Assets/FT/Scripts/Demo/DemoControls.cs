using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FractalTree.Demo
{
	public class DemoControls : MonoBehaviour
	{
		public GameObject controls;
		public DemoForceController forceController;
		public DemoTreeCreator treeCreator;

		public InputField radiusInput;
		public InputField pushInput;
		public InputField pullInput;

		public Text stationaryButtonText;

		public Text warningLabel;

		void Start()
		{
			radiusInput.text = forceController.radius.ToString();
			pushInput.text = forceController.pushForce.ToString();
			pullInput.text = forceController.pullForce.ToString();
		}

		void Update()
		{

			if (treeCreator.activeTree == null) {
				DisableControls ();
			} else if (treeCreator.showingStationary) {
				stationaryButtonText.text = "Spring";
				DisableControls ();
			}

			if (!(treeCreator.activeTree is MovingTreeBuilder)) {
				DisableControls ();
				return;
			}

			stationaryButtonText.text = "Stationary";
			EnableControls ();
		}

		public void OnRadiusChanged(string value)
		{
			float radius;

			if (float.TryParse (value, out radius)) {
				forceController.radius = radius;
			}
		}

		public void OnPushChanged(string value)
		{
			float push;

			if (float.TryParse (value, out push)) {
				forceController.pushForce = push;
			}
		}

		public void OnPullChanged(string value)
		{
			float pull;

			if (float.TryParse (value, out pull)) {
				forceController.pullForce = pull;
			}
		}

		public void OnNextTreePressed()
		{
			treeCreator.ShowNextTree ();
		}

		public void OnChangeTreeStatePressed()
		{
			bool changedState = treeCreator.SwitchTreeState ();

			if (!changedState) {
				warningLabel.text = "Cannot convert to moving tree due to shape complexity";
			
				StartCoroutine (HideWarningText (2f));
			}
		}

		private IEnumerator HideWarningText(float seconds)
		{
			yield return new WaitForSeconds (seconds);

			warningLabel.text = "";
		}

		private void EnableControls()
		{
			controls.gameObject.SetActive (true);
		}

		private void DisableControls()
		{
			controls.gameObject.SetActive (false);
		}
	}
}