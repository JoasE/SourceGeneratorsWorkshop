using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;

namespace AotMapper.Generator
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
