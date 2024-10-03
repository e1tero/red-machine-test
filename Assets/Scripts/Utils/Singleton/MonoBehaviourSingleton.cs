using Utils.MonoBehaviourUtils;

namespace Utils.Singleton
{
    public abstract class MonoBehaviourSingleton<T> : GameMonoBehaviour
        where T : MonoBehaviourSingleton<T>
    {
        protected static T _instance;

        public static T Instance => _instance;

        public virtual void Awake()
        {
            _instance = (T) this;

            Init();
        }

        protected abstract void Init();
    }
}