using UnityEngine;

namespace NonStandard.GameUi.Particles {
	public class FloatyTextManager : MonoBehaviour {
		public FloatyText prefab_floatyText;
		public FloatyText CreateDefaultPrefab(GameObject go) {
			Rigidbody rb = go.AddComponent<Rigidbody>();
			rb.useGravity = false;
			TMPro.TextMeshPro txt = go.AddComponent<TMPro.TextMeshPro>();
			txt.text = "new text";
			FloatyText floaty = go.AddComponent<FloatyText>();
			return floaty;
        }
		public static FloatyText Create(Vector3 position, string text, Camera cam = null) {
			FloatyTextManager ftm = Global.GetComponent<FloatyTextManager>();
			if (ftm.prefab_floatyText == null) {
				ftm.prefab_floatyText = ftm.CreateDefaultPrefab(new GameObject("floaty"));
				ftm.prefab_floatyText.gameObject.SetActive(false);
			}
			FloatyText ft = Instantiate(ftm.prefab_floatyText).GetComponent<FloatyText>();
			ft.gameObject.SetActive(true);
			ft.cam = cam;
			ft.name = text;
			ft.transform.position = position;
			return ft;
		}
	}
}