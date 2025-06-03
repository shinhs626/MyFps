using System.Collections;
using TMPro;
using UnityEngine;

namespace MyFps
{
    public class HiddenWallExitOpen : Interactive
    {
        #region Variables
        public TextMeshProUGUI sequence;

        [SerializeField]
        private string sequenceText = "You need more Eye picture";

        public GameObject fakePicture;
        public GameObject realPicture;

        public Animator hiddenAnim;
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            if (PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.LEFT_EYE) && PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.RIGHT_EYE))
            {
                OpenHiddenWall();
            }
            else
            {
                StartCoroutine(EyeText());
            }
        }

        IEnumerator EyeText()
        {
            unInteractive = true;
            sequence.text = "";

            yield return new WaitForSeconds(0.3f);

            sequence.text = sequenceText;

            yield return new WaitForSeconds(1.7f);

            unInteractive = false;
            sequence.text = "";
        }

        private void OpenHiddenWall()
        {
            fakePicture.SetActive(false);
            realPicture.SetActive(true);
            //Debug.Log("문이 열리네요~");

            hiddenAnim.SetTrigger("ExitOpen");
        }
        #endregion
    }

}
