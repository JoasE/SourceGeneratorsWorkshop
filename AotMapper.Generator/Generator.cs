using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

            var stringBuilder = new StringBuilder(@"namespace AotMapper
{
    public partial class Mapper
    {
        public partial TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class, new()
        {
            return new TDestination();
        }
");

            var mapperClassSymbol = context.Compilation.GetTypeByMetadataName("AotMapper.Mapper");
            var mapMethodSymbol = mapperClassSymbol.GetMembers("Map").First();

            var allMethodInvocations = context.Compilation.SyntaxTrees.SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>());
            var allMethodInvocationSymbols = allMethodInvocations.Select(invocationExpression =>
            {
                var model = context.Compilation.GetSemanticModel(invocationExpression.SyntaxTree);
                var symbol = model.GetSymbolInfo(invocationExpression.Expression);

                return symbol.Symbol as IMethodSymbol;
            });

            var allMapInvocations = allMethodInvocationSymbols.Where(methodSymbol => methodSymbol?.OriginalDefinition?.Equals(mapMethodSymbol, SymbolEqualityComparer.Default) == true);

            foreach (var mapInvocation in allMapInvocations)
            {
                stringBuilder.Append(@"public void Map(");
                stringBuilder.Append(mapInvocation.TypeArguments.First());
                stringBuilder.Append(@" s, ");
                stringBuilder.Append(mapInvocation.TypeArguments.Last());
                stringBuilder.Append(@" d){}");
            }

            stringBuilder.Append(@"}}");

            context.AddSource("Mapper.implementation.cs", stringBuilder.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(postContext => postContext.AddSource("Mapper.definition.cs", @"namespace AotMapper
{
    public partial class Mapper
    {
        public partial TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class, new();
    }
}
"));
        }
    }
}
