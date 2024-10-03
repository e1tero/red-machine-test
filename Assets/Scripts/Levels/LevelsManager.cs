using Events;
using UnityEngine;
using Utils.Scenes;
using Utils.Singleton;

namespace Levels
{
    public class LevelsManager : DontDestroyMonoBehaviour
    {
        [SerializeField] private bool _isTestLevelFirst;
        private const string LevelNamePattern = "Level{0}";
        private const string TestLevelName = "TestLevel";

        private int _currentLevelIndex;

        private void Start()
        {
            LoadInitialLevel();
            EventsController.Subscribe<EventModels.Game.TargetColorNodesFilled>(this, OnTargetColorNodesFilled);
        }

        private void LoadInitialLevel()
        {
            string initialLevelName = _isTestLevelFirst ? TestLevelName : GetLevelName(_currentLevelIndex);
            ScenesChanger.GotoScene(initialLevelName);
        }

        private string GetLevelName(int levelIndex)
        {
            return string.Format(LevelNamePattern, levelIndex);
        }

        private void OnTargetColorNodesFilled(EventModels.Game.TargetColorNodesFilled e)
        {
            LoadNextLevel();
        }

        private void LoadNextLevel()
        {
            if (_isTestLevelFirst)
            {
                ScenesChanger.GotoScene(TestLevelName);
            }
            else
            {
                _currentLevelIndex++;
                ScenesChanger.GotoScene(GetLevelName(_currentLevelIndex));
            }
        }
    }
}