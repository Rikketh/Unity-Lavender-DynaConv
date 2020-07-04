using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lavender.SDK;

namespace Rickter.Lavender.Tools.Adapters {

	public enum PostReplacementActions
	{
		Disable,
		Remove,
	};


	[ExecuteInEditMode]
	[AddComponentMenu("Custom/Rickter/Lavender/Adapters/DynamicBone to JiggleBone Adapter")]
	public class DynaConv : MonoBehaviour
	{
		public PostReplacementActions   postReplacementAction = PostReplacementActions.Disable;
		public bool                     enforceOriginalValues = false;

		public void RunConversion()
		{
			this.StartSearch();
		}

		public void StartSearch()
		{
			this.FindDynamicBoneColliders();
			this.FindDynamicBone();

			// delaying reference removals since colliders can be referenced in multiple JB components
			if (postReplacementAction == PostReplacementActions.Remove) {
				this.RemoveDynamicBoneColliders();
			}
		}

		private void FindDynamicBoneColliders()
		{
			DynamicBoneCollider[] comps = gameObject.GetComponentsInChildren<DynamicBoneCollider>(true);

			if (comps.Length == 0) {
				Debug.Log("Targeted object tree doesn't have any DynamicBoneColliders!", gameObject);
			}

			foreach (DynamicBoneCollider component in comps) {
				if (component.gameObject.GetComponent<JiggleColliderSphere>()) {
					if (enforceOriginalValues) {
						Debug.Log("Copying DynamicBoneCollider values into " + component.gameObject.name, component.gameObject);

						JiggleColliderSphere lavColliderSphere = component.gameObject.GetComponent<JiggleColliderSphere>();
						this.ReassignDynamicBoneColliderValues(lavColliderSphere, component);
					} else {
						Debug.Log(component.gameObject.name + " already has JiggleColliderSphere", component.gameObject);
					}
				} else {
					Debug.Log(component.gameObject.name + " doesn't have JiggleColliderSphere, targeting...", component.gameObject);

					JiggleColliderSphere lavColliderSphere = component.gameObject.AddComponent(typeof(JiggleColliderSphere)) as JiggleColliderSphere;
					this.ReassignDynamicBoneColliderValues(lavColliderSphere, component);
				}
			}
		}

		private void ReassignDynamicBoneColliderValues(JiggleColliderSphere lavColliderSphere, DynamicBoneCollider dCol)
		{
			lavColliderSphere.Center    = dCol.m_Center;
			lavColliderSphere.Direction = (Direction) dCol.m_Direction;
			lavColliderSphere.Bound     = (Bound) dCol.m_Bound;
			lavColliderSphere.Radius    = dCol.m_Radius;
			lavColliderSphere.Height    = dCol.m_Height;
		}

		private void FindDynamicBone()
		{
			DynamicBone[] comps = gameObject.GetComponentsInChildren<DynamicBone>(true);

			if (comps.Length == 0) {
				Debug.Log("Targeted object tree doesn't have any DynamicBones!", gameObject);
			}

			foreach (DynamicBone component in comps) {
				if (component.gameObject.GetComponent<JiggleBone>()) {
					if (enforceOriginalValues) {
						Debug.Log("Copying DynamicBone values into " + component.gameObject.name, component.gameObject);

						JiggleBone lavJiggleBone = component.gameObject.GetComponent<JiggleBone>();
						this.ReassignDynamicBoneValues(lavJiggleBone, component);
					} else {
						Debug.Log(component.gameObject.name + " already has JiggleBone", component.gameObject);
					}
				} else {
					Debug.Log(component.gameObject.name + " doesn't have JiggleBone, targeting...", component.gameObject);

					JiggleBone lavJiggleBone = component.gameObject.AddComponent(typeof(JiggleBone)) as JiggleBone;
					this.ReassignDynamicBoneValues(lavJiggleBone, component);

					switch (postReplacementAction) {
						case PostReplacementActions.Disable:
							component.enabled = false;
						break;
						case PostReplacementActions.Remove:
							DestroyImmediate(component);
						break;
					}
				}
			}
		}

		private void ReassignDynamicBoneValues(JiggleBone lavJiggleBone, DynamicBone dynBone)
		{
			lavJiggleBone.Root              = dynBone.m_Root;
			lavJiggleBone.ConstrainAxis     = (ConstrainAxis) dynBone.m_FreezeAxis;
			lavJiggleBone.Damping           = dynBone.m_Damping;
			lavJiggleBone.DampingCurve      = dynBone.m_DampingDistrib;
			lavJiggleBone.Elasticity        = dynBone.m_Elasticity;
			lavJiggleBone.ElasticityCurve   = dynBone.m_ElasticityDistrib;
			lavJiggleBone.Stiffness         = dynBone.m_Stiffness;
			lavJiggleBone.StiffnessCurve    = dynBone.m_StiffnessDistrib;
			lavJiggleBone.Inert             = dynBone.m_Inert;
			lavJiggleBone.InertCurve        = dynBone.m_InertDistrib;
			lavJiggleBone.Radius            = dynBone.m_Radius;
			lavJiggleBone.RadiusCurve       = dynBone.m_RadiusDistrib;
			lavJiggleBone.EndOffset         = dynBone.m_EndOffset;
			lavJiggleBone.Gravity           = dynBone.m_Gravity;
			lavJiggleBone.Force             = dynBone.m_Force;
			lavJiggleBone.Exclusions        = dynBone.m_Exclusions;

			List<JiggleCollider> lavColliders = new List<JiggleCollider>();

			foreach (DynamicBoneCollider dCol in dynBone.m_Colliders) {
				lavColliders.Add(dCol.gameObject.GetComponent<JiggleColliderSphere>());
			}

			lavJiggleBone.Colliders         = lavColliders;
		}

		private void RemoveDynamicBoneColliders()
		{
			DynamicBoneCollider[] comps = gameObject.GetComponentsInChildren<DynamicBoneCollider>(true);
			foreach (DynamicBoneCollider component in comps) {
				DestroyImmediate(component);
			}
		}
	}

}