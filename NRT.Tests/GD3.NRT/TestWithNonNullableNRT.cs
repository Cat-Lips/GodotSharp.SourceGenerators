using Godot;

namespace NRT.Tests
{
    [SceneTree]
    public partial class TestWithNonNullableNRT : Node
    {
        public string InstantiateValue1 = "";
        public string InstantiateValue2 = default!;
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
        private void OnInstantiateTest(string value1 = "", string value2 = default!)
        {
            InstantiateValue1 = value1;
            InstantiateValue2 = value2;
        }

        private void OnNotifyTestWithActionChanged()
            => NotifyActionValue = NotifyTestWithAction;

        private TestWithNonNullableNRT()
        {
            InitNotifyTest("");
            InitNotifyTestWithAction("");
        }
    }
}
