using CustomGeneratorTests;

[MyClass]
public partial class MyTestClass<T1>
{
    [MyMethod]
    public IEnumerable<(decimal? D, T2 T)?> MyTestMethod<T2>(ref float? _, out IEnumerable<int?> __)
    {
        // If these compile, test passes
        MyClassAttributeGeneratedThisMethod();
        MyMethodAttributeGeneratedThisMethod();

        __ = null;
        return null;
    }
}
