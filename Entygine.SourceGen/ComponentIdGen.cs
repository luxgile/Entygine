using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Text;

namespace Entygine_SourceGen
{
    [Generator]
    public class ComponentIdGen : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder sb = new(@"
    public static class ExampleCallGenerated
    {
        public static void CallGenerated()
        {
            
        }
    }");

            context.AddSource("Example", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
