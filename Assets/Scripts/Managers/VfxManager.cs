using UnityEngine;

namespace VomitCats
{
    public class VfxManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _poofVfxPrefab;
        [SerializeField] private GameObject _fireworkVfxPrefab;
        #endregion

        #region FIELDS PRIVATE
        private static VfxManager _instance;
        #endregion

        #region PROPERTIES
        public static VfxManager Instance => _instance;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }
        #endregion

        #region METHODS PRIVATE
        #endregion

        #region METHODS PUBLIC
        public void PlayVFX(Vector3 position, string name)
        {
            switch (name)
            {
                case "Poof":
                    Instantiate(_poofVfxPrefab, position, transform.rotation);
                    break;
                case "Firework":
                    Instantiate(_fireworkVfxPrefab, position, transform.rotation);
                    break;
            }
        }
        #endregion
    }
}
