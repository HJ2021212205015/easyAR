using UnityEngine;
using UnityEngine.Events;

namespace ezAR.Core
{
    /// <summary>
    /// 应用模式枚举
    /// </summary>
    public enum AppMode
    {
        AR,         // AR追踪模式
        Frozen,     // 冻结模式
        Drawing     // 涂鸦模式
    }

    /// <summary>
    /// 应用模式管理器（单例）
    /// 管理AR/冻结/涂鸦三种模式的切换
    /// </summary>
    public class AppModeManager : MonoBehaviour
    {
        public static AppModeManager Instance { get; private set; }

        [Header("Current State")]
        [SerializeField] private AppMode currentMode = AppMode.AR;

        public AppMode CurrentMode => currentMode;
        public bool IsFrozen => currentMode != AppMode.AR;

        /// <summary>
        /// 模式切换事件
        /// </summary>
        public event UnityAction<AppMode, AppMode> OnModeChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// 切换到指定模式
        /// </summary>
        public void SetMode(AppMode newMode)
        {
            if (currentMode == newMode) return;

            AppMode oldMode = currentMode;
            currentMode = newMode;

            OnModeChanged?.Invoke(oldMode, newMode);

            Debug.Log($"[AppModeManager] Mode changed: {oldMode} -> {newMode}");
        }

        /// <summary>
        /// 切换AR/冻结模式
        /// </summary>
        public void ToggleFreeze()
        {
            if (currentMode == AppMode.AR)
            {
                SetMode(AppMode.Frozen);
            }
            else
            {
                SetMode(AppMode.AR);
            }
        }

        /// <summary>
        /// 进入涂鸦模式（需要先冻结）
        /// </summary>
        public void EnterDrawingMode()
        {
            if (currentMode == AppMode.Frozen)
            {
                SetMode(AppMode.Drawing);
            }
            else
            {
                Debug.LogWarning("[AppModeManager] Must be in Frozen mode before entering Drawing mode");
            }
        }

        /// <summary>
        /// 退出涂鸦模式
        /// </summary>
        public void ExitDrawingMode()
        {
            if (currentMode == AppMode.Drawing)
            {
                SetMode(AppMode.Frozen);
            }
        }

        /// <summary>
        /// 返回AR模式
        /// </summary>
        public void ReturnToARMode()
        {
            SetMode(AppMode.AR);
        }
    }
}
