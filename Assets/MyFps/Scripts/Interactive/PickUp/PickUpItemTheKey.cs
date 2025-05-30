using UnityEngine;

namespace MyFps
{
    public class PickUpItemTheKey : PickUpItem
    {
        #region Variables
        [SerializeField]
        private PuzzleKey puzzleKey;
        #endregion

        #region Custom Method
        protected override bool OnPickUp()
        {
            PlayerDataManager.Instance.GainPuzzleKey(PuzzleKey.ROOM01_KEY);

            //������ ����
            Destroy(this.gameObject);
            return true;
        }
        #endregion
    }

}
