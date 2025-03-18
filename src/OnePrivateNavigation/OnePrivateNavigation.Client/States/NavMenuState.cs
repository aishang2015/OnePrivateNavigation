namespace OnePrivateNavigation.Client.States
{
    public class NavMenuState
    {
        // 定义一个bool变量
        public bool IsNavMenuOpen { get; private set; } = true;

        // 添加一个bool事件
        public event Action<bool>? OnNavMenuToggleEvent;

        public void ToggleNavMenu(bool isOpen)
        {
            IsNavMenuOpen = isOpen;
            OnNavMenuToggleEvent?.Invoke(isOpen);
        }
    }
}
