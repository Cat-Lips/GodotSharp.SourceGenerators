using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[TR]
public static partial class TR;

[SceneTree]
public partial class TranslationAttributeTests : Node, ITest
{
    [TR("Resources/tr", xtras: false)]
    private static partial class _TR;

    void ITest.ReadyTests()
    {
        var a = 5;
        var b = 3;
        var (English, French) = ("!Stop!", "!Deconstructing!");
        var Order = (English: "I want to eat 5 hamburgers + 3 shakes", French: "Je veux manger 5 hamburgers + 3 des shakes");
        var AllFood = (English: "I want to eat 5 \"hamburgers\" and 3 'shakes'", French: "Je veux manger 5 \"hamburgers\" et 3 des 'shakes'");
        var Reversed = (English: "I want to eat 3 \"hamburgers\", and 5 'shakes'", French: "Je veux manger 3 \"hamburgers\", et 5 des 'shakes'");

        TestRaw(TranslationAttributeTestScene.New());
        TestInner(TranslationAttributeTestScene.New());
        TestOuter(TranslationAttributeTestScene.New());

        void TestRaw(TranslationAttributeTestScene sut)
        {
            string locale;
            string Because() => $"[{nameof(TestRaw)}.{locale}]";

            UpdateUI(locale = "en");
            sut.Label1.Text.Should().Be("English", Because());
            sut.Label2.Text.Should().Be("Francias", Because());
            sut.Label3.Text.Should().Be("Hello", Because());
            sut.Label4.Text.Should().Be(Order.English, Because());
            sut.Label5.Text.Should().Be(AllFood.English, Because());
            sut.Label6.Text.Should().Be(Reversed.English, Because());

            UpdateUI(locale = "fr");
            sut.Label1.Text.Should().Be("English", Because());
            sut.Label2.Text.Should().Be("Francias", Because());
            sut.Label3.Text.Should().Be("Bonjour", Because());
            sut.Label4.Text.Should().Be(Order.French, Because());
            sut.Label5.Text.Should().Be(AllFood.French, Because());
            sut.Label6.Text.Should().Be(Reversed.French, Because());

            void UpdateUI(string locale)
            {
                TranslationServer.SetLocale(locale);

                sut.Label1.Text = Tr("ENGLISH");
                sut.Label2.Text = Tr("FRENCH");
                sut.Label3.Text = Tr("HELLO");
                sut.Label4.Text = GetOrder();
                sut.Label5.Text = GetAllFood();
                sut.Label6.Text = GetReversed();

                string GetOrder() => string.Format(Tr("BURGERS"), a) + string.Format(Tr("SHAKES"), b);
                string GetAllFood() => string.Format(Tr("ALL_FOOD"), a, b);
                string GetReversed() => string.Format(Tr("REVERSED"), a, b);
            }
        }

        void TestInner(TranslationAttributeTestScene sut)
        {
            string locale;
            string Because() => $"[{nameof(TestInner)}.{locale}]";

            UpdateUI(locale = _TR.Loc.En);
            sut.Label1.Text.Should().Be("English", Because());
            sut.Label2.Text.Should().Be("Francias", Because());
            sut.Label3.Text.Should().Be("Hello", Because());
            sut.Label4.Text.Should().Be(Order.English, Because());
            sut.Label5.Text.Should().Be(AllFood.English, Because());
            sut.Label6.Text.Should().Be(Reversed.English, Because());

            UpdateUI(locale = _TR.Loc.Fr);
            sut.Label1.Text.Should().Be("English", Because());
            sut.Label2.Text.Should().Be("Francias", Because());
            sut.Label3.Text.Should().Be("Bonjour", Because());
            sut.Label4.Text.Should().Be(Order.French, Because());
            sut.Label5.Text.Should().Be(AllFood.French, Because());
            sut.Label6.Text.Should().Be(Reversed.French, Because());

            void UpdateUI(string locale)
            {
                TranslationServer.SetLocale(locale);

                sut.Label1.Text = Tr(_TR.Key.English);
                sut.Label2.Text = Tr(_TR.Key.French);
                sut.Label3.Text = Tr(_TR.Key.Hello);
                sut.Label4.Text = GetOrder();
                sut.Label5.Text = GetAllFood();
                sut.Label6.Text = GetReversed();

                string GetOrder() => string.Format(Tr(_TR.Key.Burgers), a) + string.Format(Tr(_TR.Key.Shakes), b);
                string GetAllFood() => string.Format(Tr(_TR.Key.AllFood), a, b);
                string GetReversed() => string.Format(Tr(_TR.Key.Reversed), a, b);
            }
        }

        void TestOuter(TranslationAttributeTestScene sut)
        {
            string locale;
            string Because() => $"[{nameof(TestOuter)}.{locale}]";

            UpdateUI(locale = TR.Loc.En);
            sut.Label1.Text.Should().Be("English", Because());
            sut.Label2.Text.Should().Be("Francias", Because());
            sut.Label3.Text.Should().Be("Hello", Because());
            sut.Label4.Text.Should().Be(Order.English, Because());
            sut.Label5.Text.Should().Be(AllFood.English, Because());
            sut.Label6.Text.Should().Be(Reversed.English, Because());

            UpdateUI(locale = TR.Loc.Fr);
            sut.Label1.Text.Should().Be("English", Because());
            sut.Label2.Text.Should().Be("Francias", Because());
            sut.Label3.Text.Should().Be("Bonjour", Because());
            sut.Label4.Text.Should().Be(Order.French, Because());
            sut.Label5.Text.Should().Be(AllFood.French, Because());
            sut.Label6.Text.Should().Be(Reversed.French, Because());

            void UpdateUI(string locale)
            {
                TranslationServer.SetLocale(locale);

                sut.Label1.Text = this.TrEnglish();
                sut.Label2.Text = this.TrFrench();
                sut.Label3.Text = this.TrHello();
                sut.Label4.Text = GetOrder();
                sut.Label5.Text = GetAllFood();
                sut.Label6.Text = GetReversed();

                string GetOrder() => this.TrBurgers(a) + this.TrShakes(b);
                string GetAllFood() => this.TrAllFood(a, b);
                string GetReversed() => this.TrReversed(a, b);
            }
        }
    }
}
