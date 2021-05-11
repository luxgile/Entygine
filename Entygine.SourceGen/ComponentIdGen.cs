using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Text;

namespace Entygine_SourceGen
{
    [Generator]
    public class ComponentIdGen : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            ComponentReceiver receiver = (ComponentReceiver)context.SyntaxReceiver;

            StringBuilder sb = new StringBuilder($@"public enum TypesFound {{ {string.Join(", ", receiver.Types)} }}");

            context.AddSource("Example2.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ComponentReceiver());
        }

        public class ComponentReceiver : ISyntaxReceiver
        {
            public List<string> Types { get; } = new List<string>();
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (!(syntaxNode is StructDeclarationSyntax structSyntax))
                    return;

                SeparatedSyntaxList<BaseTypeSyntax> baseTypes = structSyntax.BaseList?.Types ?? default;
                if (baseTypes.Count == 0)
                    return;

                for (int i = 0; i < baseTypes.Count; i++)
                {
                    if (!(baseTypes[i] is SimpleBaseTypeSyntax syntax) || syntax.Type.ToString() != $"IComponent")
                        return;
                }

                string text = structSyntax.Identifier.Text;
                if (!Types.Contains(text))
                    Types.Add(text);
            }
        }
    }
}
