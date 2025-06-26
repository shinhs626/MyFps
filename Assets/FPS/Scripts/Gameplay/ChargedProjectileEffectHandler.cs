using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ChargedProjectileEffectHandler : MonoBehaviour
    {
        #region Variables
        public GameObject chargingObject;
        public MinMaxVector scale;

        private ProjectileBase projectileBase;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            projectileBase = this.GetComponent<ProjectileBase>();
            projectileBase.OnShoot += OnShoot;
        }
        #endregion

        #region Custom Method
        private void OnShoot()
        {
            chargingObject.transform.localScale = scale.GetValueRatio(projectileBase.InitialCharge);
        }
        #endregion
    }
}
