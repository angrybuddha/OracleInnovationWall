/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: FPS gun logic.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace CircularGravityForce
{
    public class Gun : MonoBehaviour
    {
        #region Enumes

        //Gun Types
        public enum GunType
        {
            GravityGunForce,
            LauncherForce,
			GravityGunTorque,
			LauncherTorque,
        }

        #endregion

        #region Properties

        //Gun force power
		[SerializeField]
		private float cgfForcePower = 30f;
		public float CgfForcePower
		{
			get { return cgfForcePower; }
			set { cgfForcePower = value; }
		}

        //CGF gameobject
        [SerializeField]
		private GameObject cgfGameObject;
		public GameObject CgfGameObject
        {
			get { return cgfGameObject; }
			set { cgfGameObject = value; }
        }

        //Gun Lazer
		[SerializeField]
		private GameObject gunLazerObject;
		public GameObject GunLazerObject
		{
			get { return gunLazerObject; }
			set { gunLazerObject = value; }
		}

        //Spawn Items
        [SerializeField]
        private Spawn spawnPosBullet;
        public Spawn SpawnPosBullet
        {
            get { return spawnPosBullet; }
            set { spawnPosBullet = value; }
        }
        [SerializeField]
        private Spawn spawnNegBullet;
        public Spawn SpawnNegBullet
        {
            get { return spawnNegBullet; }
            set { spawnNegBullet = value; }
        }
		[SerializeField]
		private Spawn spawnPosBulletTor;
		public Spawn SpawnPosBulletTor
		{
			get { return spawnPosBulletTor; }
			set { spawnPosBulletTor = value; }
		}
		[SerializeField]
		private Spawn spawnNegBulletTor;
		public Spawn SpawnNegBulletTor
		{
			get { return spawnNegBulletTor; }
			set { spawnNegBulletTor = value; }
		}

        //Gun mode
        [SerializeField]
        private GunType gunType;
        public GunType _gunType
        {
            get { return gunType; }
            set { gunType = value; }
        }

        private Animator animator;
		private CGF_SizeByRaycast sizeByRaycast;
		private GameObject gunLazer;

        #endregion

        #region Unity Functions

        // Use this for initialization
        void Start()
        {
            animator = this.GetComponent<Animator>();
			sizeByRaycast = cgfGameObject.GetComponent<CGF_SizeByRaycast> ();
			gunLazer = Instantiate(GunLazerObject) as GameObject;
        }

        // Update is called once per frame
        void Update()
        {
			SyncGunSelection();
        }

		void LateUpdate()
		{
			gunLazer.transform.position = sizeByRaycast.HitPoint;
		}

        #endregion

        #region Functions

        void SyncGunSelection()
		{
			CircularGravity cgf = cgfGameObject.GetComponent<CircularGravity> ();

			switch (_gunType)
			{
			case GunType.GravityGunForce:
				cgf._forceType = CircularGravity.ForceType.Force;
				cgf._forceMode = ForceMode.Force;
				cgf._shape = CircularGravity.Shape.RayCast;
				sizeByRaycast.enabled = true;
				
				if (Input.GetButton("Fire1"))
				{
					cgf.ForcePower = cgfForcePower;
				}
				else if (Input.GetButton("Fire2"))
				{
					cgf.ForcePower = -cgfForcePower;
				}
				else
				{
					cgf.ForcePower = 0f;
				}
				animator.SetBool("isShooting", cgf.ForcePower != 0f);
				break;
			case GunType.LauncherForce:

				cgf._forceType = CircularGravity.ForceType.Force;
				cgf._forceMode = ForceMode.Impulse;
				cgf._shape = CircularGravity.Shape.RayCast;
				sizeByRaycast.enabled = false;
				sizeByRaycast.HitPoint = Vector3.zero;
				cgf.Size = 1f;
				cgf.ForcePower = 25f;
				
				if (Input.GetButtonDown("Fire1"))
				{
					spawnPosBullet.Spawning();
					spawnPosBullet.LastSpawned.GetComponent<CircularGravity>().ForcePower = cgfForcePower;
                    if (SceneSettings.Instance != null)
                        spawnPosBullet.LastSpawned.GetComponent<CircularGravity>()._drawGravityProperties.DrawGravityForce = SceneSettings.Instance.ToggleCGF;
					animator.SetBool("isShooting", true);
				}
				else if (Input.GetButtonDown("Fire2"))
				{
					spawnNegBullet.Spawning();
					spawnNegBullet.LastSpawned.GetComponent<CircularGravity>().ForcePower = -cgfForcePower;
                    if (SceneSettings.Instance != null)
                        spawnNegBullet.LastSpawned.GetComponent<CircularGravity>()._drawGravityProperties.DrawGravityForce = SceneSettings.Instance.ToggleCGF;
					animator.SetBool("isShooting", true);
				}
				else
				{
					animator.SetBool("isShooting", false);
				}
				break;
			case GunType.GravityGunTorque:
				cgf._forceType = CircularGravity.ForceType.Torque;
				cgf._forceMode = ForceMode.Force;
				cgf._shape = CircularGravity.Shape.RayCast;
				sizeByRaycast.enabled = true;
				
				if (Input.GetButton("Fire1"))
				{
					cgf.ForcePower = cgfForcePower;
				}
				else if (Input.GetButton("Fire2"))
				{
					cgf.ForcePower = -cgfForcePower;
				}
				else
				{
					cgf.ForcePower = 0f;
				}
				animator.SetBool("isShooting", cgf.ForcePower != 0f);
				break;
			case GunType.LauncherTorque:
				cgf._forceType = CircularGravity.ForceType.Force;
				cgf._forceMode = ForceMode.Impulse;
				sizeByRaycast.enabled = false;
				sizeByRaycast.HitPoint = Vector3.zero;
				cgf.Size = 1f;
				cgf.ForcePower = 30f;

				if (Input.GetButtonDown("Fire1"))
				{
					spawnPosBulletTor.Spawning();
					spawnPosBulletTor.LastSpawned.GetComponent<CircularGravity>().ForcePower = cgfForcePower;
                    if (SceneSettings.Instance != null)
                        spawnPosBulletTor.LastSpawned.GetComponent<CircularGravity>()._drawGravityProperties.DrawGravityForce = SceneSettings.Instance.ToggleCGF;
					animator.SetBool("isShooting", true);
				}
				else if (Input.GetButtonDown("Fire2"))
				{
					spawnNegBulletTor.Spawning();
					spawnNegBulletTor.LastSpawned.GetComponent<CircularGravity>().ForcePower = -cgfForcePower;
                    if (SceneSettings.Instance != null)
                        spawnNegBulletTor.LastSpawned.GetComponent<CircularGravity>()._drawGravityProperties.DrawGravityForce = SceneSettings.Instance.ToggleCGF;
					animator.SetBool("isShooting", true);
				}
				else
				{
					animator.SetBool("isShooting", false);
				}

				break;
			}
        }

        #endregion
    }
}
