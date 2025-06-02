using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyFps
{
    public class PickUpLeftEye : Interactive
    {
        #region Variables
        public TextMeshProUGUI sequenceText;

        public GameObject puzzleUI;
        public Image puzzleImage;

        public Sprite puzzleSprite;

        [SerializeField]
        private string sequence = "Pick Up Left Eye";

        [SerializeField]
        private PuzzleKey puzzleKey = PuzzleKey.LEFT_EYE;
        #endregion

        #region Unity Event Method
        protected override void DoAction()
        {
            StartCoroutine(LeftEye());
        }
        #endregion

        #region Custom Method
        IEnumerator LeftEye()
        {
            //Debug.Log("�Ⱦ� Left Eye");
            PlayerDataManager.Instance.GainPuzzleKey(puzzleKey);

            //���ͷ�Ƽ�� ��� ����
            unInteractive = true;

            //����
            sequenceText.text = "";

            puzzleUI.SetActive(true);
            puzzleImage.sprite = puzzleSprite;

            yield return new WaitForSeconds(0.3f);

            sequenceText.text = sequence;

            yield return new WaitForSeconds(1.7f);

            //�� �ʱ�ȭ
            puzzleUI.SetActive(false);
            sequenceText.text = "";

            Destroy(gameObject);

        }
        #endregion
    }

}
