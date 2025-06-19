using UnityEngine;
using System.Collections;

namespace MyFps
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        #region Variables
        public GameObject damageEffect;
        public AudioSource hurt01;
        public AudioSource hurt02;
        public AudioSource hurt03;

        //체력
        private float currentHealth;

        [SerializeField]
        //private float maxHealth = 20;

        private bool isDeath = false;

        //죽음 처리
        public SceneFader fader;
        [SerializeField] private string loadToScene = "GameOverScene";
        #endregion

        #region Unity Event Method
        private void Start()
        {

            currentHealth = PlayerDataManager.Instance.PlayerHealth;
        }
        #endregion

        #region Custom Method
        //플레이어 데미지
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            //Debug.Log($"currentHealth: {currentHealth}");

            PlayerDataManager.Instance.PlayerHealth = currentHealth;

            //데미지 연출 Sfx, Vfx
            StartCoroutine(DamageEffect());

            if (currentHealth <= 0 && isDeath == false)
            {
                Die();
            }
        }
        IEnumerator DamageEffect()
        {
            damageEffect.SetActive(true);

            CinemachineShake.Instance.Shake(2f, 1f, 0.6f);

            PlayRandomHurt();
            yield return new WaitForSeconds(1f);
            damageEffect.SetActive(false);
        }
        private void Die()
        {
            isDeath = true;

            //죽음 처리
            fader.FadeTo(loadToScene);
        }
        private void PlayRandomHurt()
        {
            int rand = Random.Range(0, 3);

            switch (rand)
            {
                case 0:
                    hurt01.Play();
                    break;
                case 1:
                    hurt02.Play();
                    break;
                case 2:
                    hurt03.Play();
                    break;
            }
        }
        #endregion
    }

}
