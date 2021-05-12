using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
            StringBuilder sb = new StringBuilder();
            int componentId = 0;

            sb.Append("#region USINGS\n");
            sb.Append("using Entygine.Ecs;\n");
            foreach (KeyValuePair<string, List<string>> item in receiver.Types)
            {
                sb.Append($"using {item.Key};\n");
            }
            sb.Append("#endregion\n\n");

            sb.Append("#region COMPONENTS\n");
            foreach (KeyValuePair<string, List<string>> item in receiver.Types)
            {
                string namespaceName = item.Key;
                List<string> types = item.Value;

                sb.Append($@"namespace {namespaceName}");
                sb.Append("\n{\n");
                for (int i = 0; i < types.Count; i++)
                {
                    sb.Append("\t");
                    sb.Append($@"public partial struct {types[i]} {{ public static TypeId Identifier {{ get; }} = new ({componentId++}); }}");
                    sb.Append("\n");
                }
                sb.Append("}\n");
            }
            sb.Append("#endregion\n");

            sb.Append("#region TYPE MANAGER\n");
            sb.Append("namespace Entygine.Ecs\n{\n");
            sb.Append($"public static partial class TypeManager \n{{\n\tstatic TypeManager()\n\t{{\n");
            componentId = 0;
            foreach (KeyValuePair<string, List<string>> item in receiver.Types)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    sb.Append($"\t\tidToType.Add(new TypeId({componentId++}), typeof({item.Value[i]}));\n");
                }
            }
            sb.Append($"\t}}\n}}\n}}\n");
            sb.Append("#endregion");

            context.AddSource("__ComponentIds__.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ComponentReceiver());
        }

        public class ComponentReceiver : ISyntaxReceiver
        {
            public Dictionary<string, List<string>> Types { get; } = new Dictionary<string, List<string>>();
            public List<string> usings = new List<string>();
            private string lastNamespace;
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is NamespaceDeclarationSyntax namespaceSyntax)
                {
                    lastNamespace = namespaceSyntax.Name.ToString();
                    return;
                }

                if (!(syntaxNode is StructDeclarationSyntax structSyntax))
                    return;

                bool isPartial = structSyntax.Modifiers.Any(SyntaxKind.PartialKeyword);

                SeparatedSyntaxList<BaseTypeSyntax> baseTypes = structSyntax.BaseList?.Types ?? default;
                if (baseTypes.Count == 0)
                    return;

                for (int i = 0; i < baseTypes.Count; i++)
                {
                    if (!(baseTypes[i] is SimpleBaseTypeSyntax syntax))
                        return;

                    string typeName = syntax.Type.ToString();
                    if (typeName != "IComponent" && typeName != "ISharedComponent")
                        return;
                }

                string text = structSyntax.Identifier.Text;
                if (Types.TryGetValue(lastNamespace, out List<string> types) && !types.Contains(text))
                    types.Add(text);
                else
                    Types.Add(lastNamespace, new List<string>() { text });

                lastNamespace = null;
            }
        }
    }
}
