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
            context.RegisterForPostInitialization(postContext => postContext.AddSource("Mapper.cs", @"namespace AotMapper
{
    public class Mapper
    {
    }
}
"));
        }
    }
}
