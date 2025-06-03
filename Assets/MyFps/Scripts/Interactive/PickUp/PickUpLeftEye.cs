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

        //숨겨진 벽
        public GameObject fakeWall;
        public GameObject hiddenWall;
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
            //Debug.Log("픽업 Left Eye");
            PlayerDataManager.Instance.GainPuzzleKey(puzzleKey);

            //숨겨진 벽 체크
            if(PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.LEFT_EYE) && PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.RIGHT_EYE))
            {
                fakeWall.SetActive(false);
                hiddenWall.SetActive(true);
            }

            //인터렉티브 기능 제거
            unInteractive = true;

            //연출
            sequenceText.text = "";

            puzzleUI.SetActive(true);
            puzzleImage.sprite = puzzleSprite;

            yield return new WaitForSeconds(0.3f);

            sequenceText.text = sequence;

            yield return new WaitForSeconds(1.7f);

            //값 초기화
            puzzleUI.SetActive(false);
            sequenceText.text = "";

            Destroy(gameObject);

        }
        #endregion
    }

}
