using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Model
{
    public class GameSessionModel : MonoBehaviour
    {
        [SerializeField] private PlayerDataModel _playerData;
        [SerializeField] private string _defaultCheckpoint;
        public PlayerDataModel PlayerData => _playerData;
        public QuickInventoryModel QuickInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }
        
        private PlayerDataModel _save;
        private readonly CompositeDisposable _trash = new();

        private List<string> _checkpoints = new();
        private void Awake()
        {
            var existSession = GetExistSession();
            if (existSession != null)
            {
                existSession.StartSession(_defaultCheckpoint);
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckpoint);
            }
        }

        private void StartSession(string defaultCheckpoint)
        {
            SetChecked(_defaultCheckpoint);
            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckpoint = _checkpoints.Last();
            foreach (var checkPoint in checkpoints)
            {
                if (checkPoint.Id == lastCheckpoint)
                {
                    checkPoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_playerData);
            _trash.Retain(QuickInventory);
            
            PerksModel = new PerksModel(_playerData);
            _trash.Retain(PerksModel);
            
            StatsModel = new StatsModel(_playerData);
            _trash.Retain(StatsModel);

            _playerData.Hp.Value = (int)StatsModel.GetValue(StatId.Hp);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

        private GameSessionModel GetExistSession()
        {
            var sessions = FindObjectsOfType<GameSessionModel>();
            foreach (var gameSessionModel in sessions)
            {
                if (gameSessionModel != this)
                {
                    return gameSessionModel;
                }
            }

            return null;
        }

        public void Save()
        {
            _save = _playerData.Clone();
        }

        public void LoadLastSave()
        {
            _playerData = _save.Clone();
             
            _trash.Dispose();
            InitModels();
        }
        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                _checkpoints.Add(id);
                Save();
            }
        }

       
        private void OnDestroy()
        {
            _trash.Dispose();
        }

        private readonly List<string> _removedItems = new List<string>();
        public bool RestoreState(string itemId)
        {
            return _removedItems.Contains(itemId);
        }

        public void StoreState(string itemId)
        {
            if (!_removedItems.Contains(itemId))
                _removedItems.Add(itemId);
        }
    }
}