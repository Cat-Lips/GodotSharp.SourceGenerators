using Godot;

namespace NRT.Tests
{
    [SceneTree]
    public partial class TestWithNonNullableNRT : Node
    {
        public string InstantiateValue = "";
        public string NotifyActionValue = "";

        [Notify]
        public string NotifyTest
        {
            get => _notifyTest.Get();
            set => _notifyTest.Set(value);
        }

        [Notify]
        public string NotifyTestWithAction
        {
            get => _notifyTestWithAction.Get();
            set => _notifyTestWithAction.Set(value, OnNotifyTestWithActionChanged);
        }

        [OnInstantiate(ctor: null)]
        private void OnInstantiateTest(string value = "")
            => InstantiateValue = value;

        private void OnNotifyTestWithActionChanged()
            => NotifyActionValue = NotifyTestWithAction;

        private TestWithNonNullableNRT()
        {
            InitNotifyTest("");
            InitNotifyTestWithAction("");
        }
    }
}
