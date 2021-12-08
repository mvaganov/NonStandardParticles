using NonStandard.Extension;
using System.Collections.Generic;
using UnityEngine;

namespace NonStandard.GameUi.Particles {
	/// <summary>
	/// This class takes all <see cref="ParticleSystem"/> objects in children, and makes them accessible with 
	/// <see cref="ExpectedParticles.Get(string)"/>. Intended to be used with <see cref="NonStandard.Global"/>.
	/// The particle systems managed by <see cref="ExpectedParticles"/> are intended to be one-off effects, like
	/// an explosion, or puff of smoke after an item is picked up.
	/// </summary>
	public class ExpectedParticles : MonoBehaviour {
		/// <summary>
		/// Using a <see cref="List{T}"/> here for simpler introspection, since <see cref="Dictionary{TKey, TValue}"/> doesn't
		/// show up in the Unity inspector.
		/// </summary>
		[Tooltip("The intention is for this field to auto-populate based on child ParticleSystem objects")]
		public List<ParticleSystem> managedParticles = new List<ParticleSystem>();
		public Dictionary<string, int> lookup = new Dictionary<string, int>();
		protected ParticleSystem current;
		public void AbsorbChildParticles() {
			ParticleSystem[] kids = GetComponentsInChildren<ParticleSystem>(true);
			for (int i = 0; i < kids.Length; ++i) {
				if (kids[i] != null && managedParticles.IndexOf(kids[i]) < 0) {
					ParticleSystem.MainModule mm = kids[i].main;
					mm.simulationSpace = ParticleSystemSimulationSpace.World;
					lookup[kids[i].name] = managedParticles.Count;
					managedParticles.Add(kids[i]);
				}
			}
		}
		void Start() {
			AbsorbChildParticles();
		}
		public void SetCurrent(string name) { current = Get(name); }
		public void SetCurrentPosition(Vector3 position) { current.transform.position = position; }
		public void SetCurrentPosition(Transform other) { SetCurrentPosition(other.position); }
		public void EmitCurrent(int count) { current.Emit(count); }
		public int GetId(string particleSystemName) { return lookup.TryGetValue(particleSystemName, out int v) ? v : -1; }
		public ParticleSystem Get(string particleSystemName) {
			ParticleSystem pSys = managedParticles.Find(p => p.name == particleSystemName);
			if (pSys == null) {
				Show.Warning(transform.HierarchyPath() + " could not find particle \"" + particleSystemName + "\", try: " +
					  string.Join(", ", managedParticles.ConvertAll(p => p.name)) + " (" + managedParticles.Count + ")");
			}
			return pSys;
		}
		public void Emit(int particleSystemId, Vector3 pos, int count) {
			ParticleSystem p = managedParticles[particleSystemId];
			p.transform.position = pos;
			p.Emit(count);
		}
		public void Emit(string particleSystemName, Vector3 pos, int count) {
			Emit(GetId(particleSystemName), pos, count);
		}
	}
}