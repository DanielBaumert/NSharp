


using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NSharp.Lex;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

Assembly asm = Assembly.GetExecutingAssembly();
Type[] exportedTypes = asm.GetExportedTypes().Where(x => x != typeof(Program)).ToArray();
BenchmarkSwitcher.FromTypes(exportedTypes).Run();

[MemoryDiagnoser]
public class LexerBenchmark
{
    string content;

    [GlobalSetup]
    public void Init()
    {
        content = File.ReadAllText(@"J:\doc\Programmieren\CNative\src\NSharp\LexerLevel2 - Copy.cs");
    }

    [Benchmark]
    [Arguments(100)]
    [Arguments(200)]
    [Arguments(2000)]
    public void LexerTwoLevel(int j)
    {
        for (int i = 0; i < j; i++)
        {
            Lexer.Analyse(content);
        }
    }

    [Benchmark]
    [Arguments(100)]
    [Arguments(200)]
    [Arguments(2000)]
    public void LexerSingleLevel(int j)
    {
        for (int i = 0; i < j; i++)
        {
            Queue<LexerToken> level2Queue = new Queue<LexerToken>();
            NSharp.Lexer.Level.Lexer.Analyse(content, level2Queue);
        }
    }
}

[MemoryDiagnoser]
public unsafe class SwitchVsDelegetePtr
{

    public const int PlusId = 0;
    public const int MinusId = 1;
    public const int MalId = 2;

    public delegate*<int, int, int>* _delegates;

    [GlobalSetup]
    public void Init()
    {
        _delegates = (delegate*<int, int, int>*)NativeMemory.Alloc((nuint)(sizeof(delegate*<int, int, int>*) * 3));

        *(_delegates + PlusId) = &Plus;
        *(_delegates + MinusId) = &Minus;
        *(_delegates + MalId) = &Mal;
    }

    [GlobalCleanup]
    public void CleanUp()
    {
        NativeMemory.Free(_delegates);
    }


    [Benchmark]
    [Arguments(MinusId, 5, 5)]
    public int Switch(int op, int a, int b)
    {
        switch (op)
        {
            case PlusId:
                return Plus(a, b);
            case MinusId:
                return Minus(a, b);
            case MalId:
                return Mal(a, b);
        }

        return 0;
    }


    [Benchmark]
    [Arguments(MinusId, 5, 5)]
    public int Delegate(int op, int a, int b)
    {
        return (*(_delegates + op))(a, b);
    }

    public static int Plus(int a, int b)
    {
        return a + b;
    }

    public static int Minus(int a, int b)
    {
        return a + b;
    }

    public static int Mal(int a, int b)
    {
        return a + b;
    }
}