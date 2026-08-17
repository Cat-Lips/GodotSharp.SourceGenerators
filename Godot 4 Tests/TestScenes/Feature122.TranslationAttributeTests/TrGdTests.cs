using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class TrGdTests : Node, ITest
{
    private static string ResPath => TscnFilePath.GetBaseDir();
    private static Translation TrLoad(string path) => GD.Load<Translation>($"{ResPath}/{path}");

    [TR("tr.gd/gd.with.keys.csv")]
    private static partial class TrKeys;

    [TR("tr.gd/gd.with.plurals.csv")]
    private static partial class TrPlurals;

    [TR("tr.gd/gd.with.context.csv")]
    private static partial class TrContext;

    [TR("tr.gd/gd.xtras.csv", sep: ';')]
    private static partial class TrXtras;

    void ITest.ReadyTests()
    {
        TestTr();
        TestXtras();

        static void TestTr()
        {
            TestTrWithKeys();
            TestTrWithPlurals();
            TestTrWithContext();

            static void TestTrWithKeys()
            {
                var sut = TranslationServer.GetOrAddDomain(nameof(TestTrWithKeys));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.keys.en.translation"));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.keys.es.translation"));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.keys.ja.translation"));

                sut.SetLocaleOverride(TrKeys.Loc.En);
                sut.Translate(TrKeys.Key.Greet).Should().Be((StringName)"Hello, friend!");
                sut.Translate(TrKeys.Key.Ask).Should().Be((StringName)"How are you?");
                sut.Translate(TrKeys.Key.Bye).Should().Be((StringName)"Goodbye");
                sut.Translate(TrKeys.Key.Quote).Should().Be((StringName)@"""Hello"" said the man.");

                sut.SetLocaleOverride(TrKeys.Loc.Es);
                sut.Translate(TrKeys.Key.Greet).Should().Be((StringName)"Hola, amigo!");
                sut.Translate(TrKeys.Key.Ask).Should().Be((StringName)"Cómo está?");
                sut.Translate(TrKeys.Key.Bye).Should().Be((StringName)"Adiós");
                sut.Translate(TrKeys.Key.Quote).Should().Be((StringName)@"""Hola"" dijo el hombre.");

                sut.SetLocaleOverride(TrKeys.Loc.Ja);
                sut.Translate(TrKeys.Key.Greet).Should().Be((StringName)"こんにちは");
                sut.Translate(TrKeys.Key.Ask).Should().Be((StringName)"元気ですか");
                sut.Translate(TrKeys.Key.Bye).Should().Be((StringName)"さようなら");
                sut.Translate(TrKeys.Key.Quote).Should().Be((StringName)@"「こんにちは」男は言いました");
            }

            static void TestTrWithPlurals()
            {
                var sut = TranslationServer.GetOrAddDomain(nameof(TestTrWithPlurals));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.plurals.fr.translation"));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.plurals.ru.translation"));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.plurals.ja.translation"));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.plurals.zh.translation"));

                sut.SetLocaleOverride(TrPlurals.Loc.En);
                StringName enSingle = "There is %d apple";
                StringName enPlural = "There are %d apples";
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 0).Should().Be(enPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 1).Should().Be(enSingle);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 2).Should().Be(enPlural);

                sut.SetLocaleOverride(TrPlurals.Loc.Fr);
                StringName frSingle = "Il y a %d pomme";
                StringName frPlural = "Il y a %d pommes"; // *** ?pluralrule override (n != 1) *** //
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 0).Should().Be(frPlural); // (frSingle without override)
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 1).Should().Be(frSingle);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 2).Should().Be(frPlural);

                sut.SetLocaleOverride(TrPlurals.Loc.Ru);
                StringName ruSingle = "Есть %d яблоко"; // numbers ending in 1 (except 11)
                StringName ruPaucal = "Есть %d яблока"; // numbers ending in 2, 3, or 4 (except 12–14)
                StringName ruPlural = "Есть %d яблок";  // all other numbers (including 0, fractions, others)
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 0).Should().Be(ruPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 1).Should().Be(ruSingle); // Single
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 2).Should().Be(ruPaucal); // Paucal
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 4).Should().Be(ruPaucal); // Paucal
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 5).Should().Be(ruPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 10).Should().Be(ruPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 11).Should().Be(ruPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 12).Should().Be(ruPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 14).Should().Be(ruPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 15).Should().Be(ruPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 20).Should().Be(ruPlural);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 21).Should().Be(ruSingle); // Single
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 22).Should().Be(ruPaucal); // Paucal
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 24).Should().Be(ruPaucal); // Paucal
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 25).Should().Be(ruPlural);

                StringName jaAll = "リンゴが%d個あります";
                sut.SetLocaleOverride(TrPlurals.Loc.Ja);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 0).Should().Be(jaAll);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 1).Should().Be(jaAll);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 2).Should().Be(jaAll);

                StringName zhAll = "那里有%d个苹果";
                sut.SetLocaleOverride(TrPlurals.Loc.Zh);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 0).Should().Be(zhAll);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 1).Should().Be(zhAll);
                sut.TranslatePlural(TrPlurals.Key.ThereIsApple, TrPlurals.Key.ThereAreApples, 2).Should().Be(zhAll);
            }

            static void TestTrWithContext()
            {
                var sut = TranslationServer.GetOrAddDomain(nameof(TestTrWithContext));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.context.fr.translation"));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.context.ru.translation"));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.context.ja.translation"));
                sut.AddTranslation(TrLoad("tr.gd/gd.with.context.zh.translation"));

                sut.SetLocaleOverride(TrContext.Loc.En);
                sut.Translate(TrContext.Key.Message.Letter, TrContext.Key.Message.Context).Should().Be((StringName)"Letter");
                sut.Translate(TrContext.Key.Alphabet.Letter, TrContext.Key.Alphabet.Context).Should().Be((StringName)"Letter");

                sut.SetLocaleOverride(TrContext.Loc.Fr);
                sut.Translate(TrContext.Key.Message.Letter, TrContext.Key.Message.Context).Should().Be((StringName)"Courrier");
                sut.Translate(TrContext.Key.Alphabet.Letter, TrContext.Key.Alphabet.Context).Should().Be((StringName)"Lettre");

                sut.SetLocaleOverride(TrContext.Loc.Ru);
                sut.Translate(TrContext.Key.Message.Letter, TrContext.Key.Message.Context).Should().Be((StringName)"Письмо");
                sut.Translate(TrContext.Key.Alphabet.Letter, TrContext.Key.Alphabet.Context).Should().Be((StringName)"Буква");

                sut.SetLocaleOverride(TrContext.Loc.Ja);
                sut.Translate(TrContext.Key.Message.Letter, TrContext.Key.Message.Context).Should().Be((StringName)"手紙");
                sut.Translate(TrContext.Key.Alphabet.Letter, TrContext.Key.Alphabet.Context).Should().Be((StringName)"字母");

                sut.SetLocaleOverride(TrContext.Loc.Zh);
                sut.Translate(TrContext.Key.Message.Letter, TrContext.Key.Message.Context).Should().Be((StringName)"信件");
                sut.Translate(TrContext.Key.Alphabet.Letter, TrContext.Key.Alphabet.Context).Should().Be((StringName)"字母");
            }
        }

        static void TestXtras()
        {
            TestXtrasWithKeys();
            TestXtrasWithPlurals();
            TestXtrasWithContext();
            TestAdditionalFeatures();

            void TestXtrasWithKeys()
            {
                TrKeys.Domain = TranslationServer.GetOrAddDomain(nameof(TestXtrasWithKeys));
                TrKeys.Domain.AddTranslation(TrLoad("tr.gd/gd.with.keys.en.translation"));
                TrKeys.Domain.AddTranslation(TrLoad("tr.gd/gd.with.keys.es.translation"));
                TrKeys.Domain.AddTranslation(TrLoad("tr.gd/gd.with.keys.ja.translation"));

                TrKeys.Locale = TrKeys.Loc.En;
                TrKeys.Locale.Should().Be(TrKeys.Loc.En);
                TrKeys.TrGreet().Should().Be((StringName)"Hello, friend!");
                TrKeys.TrAsk().Should().Be((StringName)"How are you?");
                TrKeys.TrBye().Should().Be((StringName)"Goodbye");
                TrKeys.TrQuote().Should().Be((StringName)@"""Hello"" said the man.");

                TrKeys.Locale = TrKeys.Loc.Es;
                TrKeys.Locale.Should().Be(TrKeys.Loc.Es);
                TrKeys.TrGreet().Should().Be((StringName)"Hola, amigo!");
                TrKeys.TrAsk().Should().Be((StringName)"Cómo está?");
                TrKeys.TrBye().Should().Be((StringName)"Adiós");
                TrKeys.TrQuote().Should().Be((StringName)@"""Hola"" dijo el hombre.");

                TrKeys.Locale = TrKeys.Loc.Ja;
                TrKeys.Locale.Should().Be(TrKeys.Loc.Ja);
                TrKeys.TrGreet().Should().Be((StringName)"こんにちは");
                TrKeys.TrAsk().Should().Be((StringName)"元気ですか");
                TrKeys.TrBye().Should().Be((StringName)"さようなら");
                TrKeys.TrQuote().Should().Be((StringName)@"「こんにちは」男は言いました");
            }

            void TestXtrasWithPlurals()
            {
                TrPlurals.Domain = TranslationServer.GetOrAddDomain(nameof(TestXtrasWithPlurals));
                TrPlurals.Domain.AddTranslation(TrLoad("tr.gd/gd.with.plurals.fr.translation"));
                TrPlurals.Domain.AddTranslation(TrLoad("tr.gd/gd.with.plurals.ru.translation"));
                TrPlurals.Domain.AddTranslation(TrLoad("tr.gd/gd.with.plurals.ja.translation"));
                TrPlurals.Domain.AddTranslation(TrLoad("tr.gd/gd.with.plurals.zh.translation"));

                TrPlurals.Locale = TrPlurals.Loc.En;
                TrPlurals.TrThereIsApple(0).Should().Be((StringName)"There are 0 apples");
                TrPlurals.TrThereIsApple(1).Should().Be((StringName)"There is 1 apple");
                TrPlurals.TrThereIsApple(2).Should().Be((StringName)"There are 2 apples");
                TrPlurals.TrThereAreApples(0).Should().Be((StringName)"There are 0 apples");
                TrPlurals.TrThereAreApples(1).Should().Be((StringName)"There is 1 apple");
                TrPlurals.TrThereAreApples(2).Should().Be((StringName)"There are 2 apples");

                TrPlurals.Locale = TrPlurals.Loc.Fr; // *** ?pluralrule override (n != 1) *** //
                TrPlurals.TrThereIsApple(0).Should().Be((StringName)"Il y a 0 pommes"); // (pomme without override)
                TrPlurals.TrThereIsApple(1).Should().Be((StringName)"Il y a 1 pomme");
                TrPlurals.TrThereIsApple(2).Should().Be((StringName)"Il y a 2 pommes");
                TrPlurals.TrThereAreApples(0).Should().Be((StringName)"Il y a 0 pommes"); // (pomme without override)
                TrPlurals.TrThereAreApples(1).Should().Be((StringName)"Il y a 1 pomme");
                TrPlurals.TrThereAreApples(2).Should().Be((StringName)"Il y a 2 pommes");

                TrPlurals.Locale = TrPlurals.Loc.Ru;
                TrPlurals.TrThereIsApple(0).Should().Be((StringName)"Есть 0 яблок");
                TrPlurals.TrThereIsApple(1).Should().Be((StringName)"Есть 1 яблоко"); // Single
                TrPlurals.TrThereIsApple(2).Should().Be((StringName)"Есть 2 яблока"); // Paucal
                TrPlurals.TrThereIsApple(4).Should().Be((StringName)"Есть 4 яблока"); // Paucal
                TrPlurals.TrThereIsApple(5).Should().Be((StringName)"Есть 5 яблок");
                TrPlurals.TrThereIsApple(10).Should().Be((StringName)"Есть 10 яблок");
                TrPlurals.TrThereIsApple(11).Should().Be((StringName)"Есть 11 яблок");
                TrPlurals.TrThereIsApple(12).Should().Be((StringName)"Есть 12 яблок");
                TrPlurals.TrThereIsApple(14).Should().Be((StringName)"Есть 14 яблок");
                TrPlurals.TrThereIsApple(15).Should().Be((StringName)"Есть 15 яблок");
                TrPlurals.TrThereIsApple(20).Should().Be((StringName)"Есть 20 яблок");
                TrPlurals.TrThereIsApple(21).Should().Be((StringName)"Есть 21 яблоко"); // Single
                TrPlurals.TrThereIsApple(22).Should().Be((StringName)"Есть 22 яблока"); // Paucal
                TrPlurals.TrThereIsApple(24).Should().Be((StringName)"Есть 24 яблока"); // Paucal
                TrPlurals.TrThereIsApple(25).Should().Be((StringName)"Есть 25 яблок");
                TrPlurals.TrThereAreApples(0).Should().Be((StringName)"Есть 0 яблок");
                TrPlurals.TrThereAreApples(1).Should().Be((StringName)"Есть 1 яблоко"); // Single
                TrPlurals.TrThereAreApples(2).Should().Be((StringName)"Есть 2 яблока"); // Paucal
                TrPlurals.TrThereAreApples(4).Should().Be((StringName)"Есть 4 яблока"); // Paucal
                TrPlurals.TrThereAreApples(5).Should().Be((StringName)"Есть 5 яблок");
                TrPlurals.TrThereAreApples(10).Should().Be((StringName)"Есть 10 яблок");
                TrPlurals.TrThereAreApples(11).Should().Be((StringName)"Есть 11 яблок");
                TrPlurals.TrThereAreApples(12).Should().Be((StringName)"Есть 12 яблок");
                TrPlurals.TrThereAreApples(14).Should().Be((StringName)"Есть 14 яблок");
                TrPlurals.TrThereAreApples(15).Should().Be((StringName)"Есть 15 яблок");
                TrPlurals.TrThereAreApples(20).Should().Be((StringName)"Есть 20 яблок");
                TrPlurals.TrThereAreApples(21).Should().Be((StringName)"Есть 21 яблоко"); // Single
                TrPlurals.TrThereAreApples(22).Should().Be((StringName)"Есть 22 яблока"); // Paucal
                TrPlurals.TrThereAreApples(24).Should().Be((StringName)"Есть 24 яблока"); // Paucal
                TrPlurals.TrThereAreApples(25).Should().Be((StringName)"Есть 25 яблок");

                TrPlurals.Locale = TrPlurals.Loc.Ja;
                TrPlurals.TrThereIsApple(0).Should().Be((StringName)"リンゴが0個あります");
                TrPlurals.TrThereIsApple(1).Should().Be((StringName)"リンゴが1個あります");
                TrPlurals.TrThereIsApple(2).Should().Be((StringName)"リンゴが2個あります");
                TrPlurals.TrThereAreApples(0).Should().Be((StringName)"リンゴが0個あります");
                TrPlurals.TrThereAreApples(1).Should().Be((StringName)"リンゴが1個あります");
                TrPlurals.TrThereAreApples(2).Should().Be((StringName)"リンゴが2個あります");

                TrPlurals.Locale = TrPlurals.Loc.Zh;
                TrPlurals.TrThereIsApple(0).Should().Be((StringName)"那里有0个苹果");
                TrPlurals.TrThereIsApple(1).Should().Be((StringName)"那里有1个苹果");
                TrPlurals.TrThereIsApple(2).Should().Be((StringName)"那里有2个苹果");
                TrPlurals.TrThereAreApples(0).Should().Be((StringName)"那里有0个苹果");
                TrPlurals.TrThereAreApples(1).Should().Be((StringName)"那里有1个苹果");
                TrPlurals.TrThereAreApples(2).Should().Be((StringName)"那里有2个苹果");
            }

            void TestXtrasWithContext()
            {
                TrContext.Domain = TranslationServer.GetOrAddDomain(nameof(TestXtrasWithContext));
                TrContext.Domain.AddTranslation(TrLoad("tr.gd/gd.with.context.fr.translation"));
                TrContext.Domain.AddTranslation(TrLoad("tr.gd/gd.with.context.ru.translation"));
                TrContext.Domain.AddTranslation(TrLoad("tr.gd/gd.with.context.ja.translation"));
                TrContext.Domain.AddTranslation(TrLoad("tr.gd/gd.with.context.zh.translation"));

                TrContext.Locale = TrContext.Loc.En;
                TrContext.TrMessageLetter().Should().Be((StringName)"Letter");
                TrContext.TrAlphabetLetter().Should().Be((StringName)"Letter");

                TrContext.Locale = TrContext.Loc.Fr;
                TrContext.TrMessageLetter().Should().Be((StringName)"Courrier");
                TrContext.TrAlphabetLetter().Should().Be((StringName)"Lettre");

                TrContext.Locale = TrContext.Loc.Ru;
                TrContext.TrMessageLetter().Should().Be((StringName)"Письмо");
                TrContext.TrAlphabetLetter().Should().Be((StringName)"Буква");

                TrContext.Locale = TrContext.Loc.Ja;
                TrContext.TrMessageLetter().Should().Be((StringName)"手紙");
                TrContext.TrAlphabetLetter().Should().Be((StringName)"字母");

                TrContext.Locale = TrContext.Loc.Zh;
                TrContext.TrMessageLetter().Should().Be((StringName)"信件");
                TrContext.TrAlphabetLetter().Should().Be((StringName)"字母");
            }

            void TestAdditionalFeatures()
            {
                TrXtras.Domain = TranslationServer.GetOrAddDomain(nameof(TestAdditionalFeatures));
                TrXtras.Domain.AddTranslation(TrLoad("tr.gd/gd.xtras.fr.translation"));
                TrXtras.Domain.AddTranslation(TrLoad("tr.gd/gd.xtras.ar.translation"));

                TrXtras.Locale = TrXtras.Loc.En;
                TrXtras.TrThereIsEgg(0).Should().Be((StringName)"There are 0 eggs");
                TrXtras.TrThereIsEgg(1).Should().Be((StringName)"There is 1 egg");
                TrXtras.TrThereIsEgg(2).Should().Be((StringName)"There are 2 eggs");
                TrXtras.TrThereAreEggs(0).Should().Be((StringName)"There are 0 eggs");
                TrXtras.TrThereAreEggs(1).Should().Be((StringName)"There is 1 egg");
                TrXtras.TrThereAreEggs(2).Should().Be((StringName)"There are 2 eggs");

                TrXtras.TrThereIsApple(0).Should().Be((StringName)"There are 0 apples");
                TrXtras.TrThereIsApple(1).Should().Be((StringName)"There is 1 apple");
                TrXtras.TrThereIsApple(2).Should().Be((StringName)"There are 2 apples");
                TrXtras.TrThereAreApples(0).Should().Be((StringName)"There are 0 apples");
                TrXtras.TrThereAreApples(1).Should().Be((StringName)"There is 1 apple");
                TrXtras.TrThereAreApples(2).Should().Be((StringName)"There are 2 apples");

                TrXtras.Locale = TrXtras.Loc.Fr; // *** no ?pluralrule override *** //
                TrXtras.TrThereIsEgg(0).Should().Be((StringName)"Il y a 0 œuf");
                TrXtras.TrThereIsEgg(1).Should().Be((StringName)"Il y a 1 œuf");
                TrXtras.TrThereIsEgg(2).Should().Be((StringName)"Il y a 2 œufs");
                TrXtras.TrThereAreEggs(0).Should().Be((StringName)"Il y a 0 œuf");
                TrXtras.TrThereAreEggs(1).Should().Be((StringName)"Il y a 1 œuf");
                TrXtras.TrThereAreEggs(2).Should().Be((StringName)"Il y a 2 œufs");

                TrXtras.TrThereIsApple(0).Should().Be((StringName)"Il y a 0 pomme");
                TrXtras.TrThereIsApple(1).Should().Be((StringName)"Il y a 1 pomme");
                TrXtras.TrThereIsApple(2).Should().Be((StringName)"Il y a 2 pommes");
                TrXtras.TrThereAreApples(0).Should().Be((StringName)"Il y a 0 pomme");
                TrXtras.TrThereAreApples(1).Should().Be((StringName)"Il y a 1 pomme");
                TrXtras.TrThereAreApples(2).Should().Be((StringName)"Il y a 2 pommes");

                var arZero = "٠";
                var arOne = "١";
                var arTwo = "٢";

                var arFew3 = "٣";
                var arFew4 = "٤";
                var arFew5 = "٥";
                var arFew10 = "١٠";
                var arFew103 = "١٠٣";
                var arFew110 = "١١٠";

                var arMany11 = "١١";
                var arMany12 = "١٢";
                var arMany20 = "٢٠";
                var arMany99 = "٩٩";
                var arMany111 = "١١١";
                var arMany199 = "١٩٩";

                var arOther100 = "١٠٠";
                var arOther101 = "١٠١";
                var arOther102 = "١٠٢";
                var arOther200 = "٢٠٠";
                var arOther201 = "٢٠١";
                var arOther202 = "٢٠٢";

                var arEggsZero = "لا يوجد {0} بيض";
                var arEggsOne = "يوجد {0} بيضة";
                var arEggsTwo = "يوجد {0} بيضتان";
                var arEggsFew = "يوجد {0} بيضات";
                var arEggsMany = "يوجد {0} بيض";
                var arEggsOther = "يوجد {0} من البيض";

                TrXtras.Locale = TrXtras.Loc.Ar;

                TrXtras.TrThereIsEgg(0).Should().Be((StringName)string.Format(arEggsZero, arZero));
                TrXtras.TrThereIsEgg(1).Should().Be((StringName)string.Format(arEggsOne, arOne));
                TrXtras.TrThereIsEgg(2).Should().Be((StringName)string.Format(arEggsTwo, arTwo));

                TrXtras.TrThereIsEgg(3).Should().Be((StringName)string.Format(arEggsFew, arFew3));
                TrXtras.TrThereIsEgg(4).Should().Be((StringName)string.Format(arEggsFew, arFew4));
                TrXtras.TrThereIsEgg(5).Should().Be((StringName)string.Format(arEggsFew, arFew5));
                TrXtras.TrThereIsEgg(10).Should().Be((StringName)string.Format(arEggsFew, arFew10));
                TrXtras.TrThereIsEgg(103).Should().Be((StringName)string.Format(arEggsFew, arFew103));
                TrXtras.TrThereIsEgg(110).Should().Be((StringName)string.Format(arEggsFew, arFew110));

                TrXtras.TrThereIsEgg(11).Should().Be((StringName)string.Format(arEggsMany, arMany11));
                TrXtras.TrThereIsEgg(12).Should().Be((StringName)string.Format(arEggsMany, arMany12));
                TrXtras.TrThereIsEgg(20).Should().Be((StringName)string.Format(arEggsMany, arMany20));
                TrXtras.TrThereIsEgg(99).Should().Be((StringName)string.Format(arEggsMany, arMany99));
                TrXtras.TrThereIsEgg(111).Should().Be((StringName)string.Format(arEggsMany, arMany111));
                TrXtras.TrThereIsEgg(199).Should().Be((StringName)string.Format(arEggsMany, arMany199));

                TrXtras.TrThereIsEgg(100).Should().Be((StringName)string.Format(arEggsOther, arOther100));
                TrXtras.TrThereIsEgg(101).Should().Be((StringName)string.Format(arEggsOther, arOther101));
                TrXtras.TrThereIsEgg(102).Should().Be((StringName)string.Format(arEggsOther, arOther102));
                TrXtras.TrThereIsEgg(200).Should().Be((StringName)string.Format(arEggsOther, arOther200));
                TrXtras.TrThereIsEgg(201).Should().Be((StringName)string.Format(arEggsOther, arOther201));
                TrXtras.TrThereIsEgg(202).Should().Be((StringName)string.Format(arEggsOther, arOther202));

                TrXtras.TrThereIsEgg(0, FormatNumber: false).Should().Be((StringName)string.Format(arEggsZero, 0));
                TrXtras.TrThereIsEgg(1, FormatNumber: false).Should().Be((StringName)string.Format(arEggsOne, 1));
                TrXtras.TrThereIsEgg(2, FormatNumber: false).Should().Be((StringName)string.Format(arEggsTwo, 2));

                TrXtras.TrThereIsEgg(3, FormatNumber: false).Should().Be((StringName)string.Format(arEggsFew, 3));
                TrXtras.TrThereIsEgg(4, FormatNumber: false).Should().Be((StringName)string.Format(arEggsFew, 4));
                TrXtras.TrThereIsEgg(5, FormatNumber: false).Should().Be((StringName)string.Format(arEggsFew, 5));
                TrXtras.TrThereIsEgg(10, FormatNumber: false).Should().Be((StringName)string.Format(arEggsFew, 10));
                TrXtras.TrThereIsEgg(103, FormatNumber: false).Should().Be((StringName)string.Format(arEggsFew, 103));
                TrXtras.TrThereIsEgg(110, FormatNumber: false).Should().Be((StringName)string.Format(arEggsFew, 110));

                TrXtras.TrThereIsEgg(11, FormatNumber: false).Should().Be((StringName)string.Format(arEggsMany, 11));
                TrXtras.TrThereIsEgg(12, FormatNumber: false).Should().Be((StringName)string.Format(arEggsMany, 12));
                TrXtras.TrThereIsEgg(20, FormatNumber: false).Should().Be((StringName)string.Format(arEggsMany, 20));
                TrXtras.TrThereIsEgg(99, FormatNumber: false).Should().Be((StringName)string.Format(arEggsMany, 99));
                TrXtras.TrThereIsEgg(111, FormatNumber: false).Should().Be((StringName)string.Format(arEggsMany, 111));
                TrXtras.TrThereIsEgg(199, FormatNumber: false).Should().Be((StringName)string.Format(arEggsMany, 199));

                TrXtras.TrThereIsEgg(100, FormatNumber: false).Should().Be((StringName)string.Format(arEggsOther, 100));
                TrXtras.TrThereIsEgg(101, FormatNumber: false).Should().Be((StringName)string.Format(arEggsOther, 101));
                TrXtras.TrThereIsEgg(102, FormatNumber: false).Should().Be((StringName)string.Format(arEggsOther, 102));
                TrXtras.TrThereIsEgg(200, FormatNumber: false).Should().Be((StringName)string.Format(arEggsOther, 200));
                TrXtras.TrThereIsEgg(201, FormatNumber: false).Should().Be((StringName)string.Format(arEggsOther, 201));
                TrXtras.TrThereIsEgg(202, FormatNumber: false).Should().Be((StringName)string.Format(arEggsOther, 202));

                TrXtras.TrThereIsEgg(0, FormatPlural: false).Should().Be((StringName)arEggsZero);
                TrXtras.TrThereIsEgg(1, FormatPlural: false).Should().Be((StringName)arEggsOne);
                TrXtras.TrThereIsEgg(2, FormatPlural: false).Should().Be((StringName)arEggsTwo);

                TrXtras.TrThereIsEgg(3, FormatPlural: false).Should().Be((StringName)arEggsFew);
                TrXtras.TrThereIsEgg(4, FormatPlural: false).Should().Be((StringName)arEggsFew);
                TrXtras.TrThereIsEgg(5, FormatPlural: false).Should().Be((StringName)arEggsFew);
                TrXtras.TrThereIsEgg(10, FormatPlural: false).Should().Be((StringName)arEggsFew);
                TrXtras.TrThereIsEgg(103, FormatPlural: false).Should().Be((StringName)arEggsFew);
                TrXtras.TrThereIsEgg(110, FormatPlural: false).Should().Be((StringName)arEggsFew);

                TrXtras.TrThereIsEgg(11, FormatPlural: false).Should().Be((StringName)arEggsMany);
                TrXtras.TrThereIsEgg(12, FormatPlural: false).Should().Be((StringName)arEggsMany);
                TrXtras.TrThereIsEgg(20, FormatPlural: false).Should().Be((StringName)arEggsMany);
                TrXtras.TrThereIsEgg(99, FormatPlural: false).Should().Be((StringName)arEggsMany);
                TrXtras.TrThereIsEgg(111, FormatPlural: false).Should().Be((StringName)arEggsMany);
                TrXtras.TrThereIsEgg(199, FormatPlural: false).Should().Be((StringName)arEggsMany);

                TrXtras.TrThereIsEgg(100, FormatPlural: false).Should().Be((StringName)arEggsOther);
                TrXtras.TrThereIsEgg(101, FormatPlural: false).Should().Be((StringName)arEggsOther);
                TrXtras.TrThereIsEgg(102, FormatPlural: false).Should().Be((StringName)arEggsOther);
                TrXtras.TrThereIsEgg(200, FormatPlural: false).Should().Be((StringName)arEggsOther);
                TrXtras.TrThereIsEgg(201, FormatPlural: false).Should().Be((StringName)arEggsOther);
                TrXtras.TrThereIsEgg(202, FormatPlural: false).Should().Be((StringName)arEggsOther);
            }
        }
    }
}
