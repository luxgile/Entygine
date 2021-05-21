using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Entygine.SourceGen
{
    [Generator]
    public class EntityIteratorsGen : ISourceGenerator
    {
        public struct IteratorArguments : IEquatable<IteratorArguments>, IComparable<IteratorArguments>
        {
            public bool shared;
            public bool write;
            public bool optional;

            public int CompareTo(IteratorArguments other)
            {
                int comparison = 0;
                if (shared && !other.shared)
                    comparison += 100;
                else if (!shared && other.shared)
                    comparison -= 100;

                if (write && !other.write)
                    comparison += 10;
                else if (!write && other.write)
                    comparison -= 10;

                if (optional && !other.optional)
                    comparison += 1;
                else if (!optional && other.optional)
                    comparison -= 1;

                return comparison;
            }

            public override bool Equals(object obj)
            {
                return obj is IteratorArguments arguments &&
                       shared == arguments.shared &&
                       write == arguments.write &&
                       optional == arguments.optional;
            }

            public bool Equals(IteratorArguments other)
            {
                return shared == other.shared &&
                       write == other.write &&
                       optional == other.optional;
            }

            public override int GetHashCode()
            {
                int hashCode = -237574455;
                hashCode = hashCode * -1521134295 + shared.GetHashCode();
                hashCode = hashCode * -1521134295 + write.GetHashCode();
                hashCode = hashCode * -1521134295 + optional.GetHashCode();
                return hashCode;
            }
        }

        private EntityIteratorsFinder finder = new EntityIteratorsFinder();
        private HashSet<List<IteratorArguments>> hash = new HashSet<List<IteratorArguments>>();

        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System.Runtime.CompilerServices;\n");
            sb.Append("using System.Collections.Generic;\n");
            sb.Append("namespace Entygine.Ecs\n");
            sb.Append("{\n");
            sb.Append("public class EntityIterator : IIteratorPhase1, IIteratorPhase2, IIteratorPhase3\n");
            sb.Append("{\n");
            sb.Append(IteratorMethods + "\n");
            try
            {
                for (int i = 0; i < finder.ids.Count; i++)
                {
                    Stuff current = finder.ids[i];
                    Compilation compilation = context.Compilation;

                    if (!context.Compilation.ContainsSyntaxTree(current.tree))
                        compilation = context.Compilation.AddSyntaxTrees(current.tree);

                    SemanticModel model = compilation.GetSemanticModel(current.tree);

                    TypeInfo typeKind = model.GetTypeInfo(current.invocation);
                    if (typeKind.Type != null)
                    {
                        if (typeKind.Type.Name == "EntityIterator")
                        {
                            ParameterSyntax[] parametersSyntax = GetLambdaParameters(current.invocation);
                            List<IteratorArguments> arguments = new List<IteratorArguments>();
                            for (int t = 0; t < parametersSyntax.Length; t++)
                                arguments.Add(CreateArgument(parametersSyntax[t], model));

                            //arguments.Sort();
                            if (!hash.Contains(arguments, new ArgumentsEqualityComparer()))
                            {
                                sb.Append($"//From file: {current.tree.FilePath}\n");
                                PrintDelegate(sb, arguments.ToArray());
                                PrintIterator(sb, arguments.ToArray());
                                hash.Add(arguments);
                            }
                        }
                    }
                }
            }
            catch (Exception e) { sb.Append($"\n /* {e} */"); }
            sb.Append("}\n");
            sb.Append("}\n");
            context.AddSource("Iterators.gen.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        public class ArgumentsEqualityComparer : IEqualityComparer<List<IteratorArguments>>
        {
            public bool Equals(List<IteratorArguments> x, List<IteratorArguments> y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(List<IteratorArguments> obj)
            {
                return obj.GetHashCode();
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => finder);
        }

        private IteratorArguments CreateArgument(ParameterSyntax parameter, SemanticModel model)
        {
            bool isWrite = false;
            if (parameter.ChildTokens()?.ToArray()[0].IsKind(SyntaxKind.RefKeyword) ?? false)
                isWrite = true;

            bool isShared = false;
            bool isOptional = false;
            if (parameter.ChildNodes()?.ToArray()[0] is IdentifierNameSyntax idSyntax)
                isShared = model.GetTypeInfo(idSyntax).Type.AllInterfaces.Any((x) => x.Name == "ISharedComponent");
            else if (parameter.ChildNodes()?.ToArray()[0] is NullableTypeSyntax nullableSyntax && nullableSyntax.ChildNodes()?.ToArray()[0] is IdentifierNameSyntax idSyntax2)
            {
                isOptional = true;
                isShared = model.GetTypeInfo(idSyntax2).Type.AllInterfaces.Any((x) => x.Name == "ISharedComponent");
            }

            return new IteratorArguments()
            {
                write = isWrite,
                shared = isShared,
                optional = isOptional,
            };
        }

        private ParameterSyntax[] GetLambdaParameters(IdentifierNameSyntax iteratorVariable)
        {
            InvocationExpressionSyntax parentInvocation = null;
            SyntaxNode currentParent = iteratorVariable.Parent;
            while (currentParent != null && !(currentParent is ExpressionStatementSyntax))
            {
                if (currentParent is InvocationExpressionSyntax invocation)
                    parentInvocation = invocation;

                currentParent = currentParent.Parent;
            }

            if (parentInvocation == null)
                return null;

            for (int i = 0; i < parentInvocation.ArgumentList.Arguments.Count; i++)
            {
                var childs = parentInvocation.ArgumentList.Arguments[i].ChildNodes().ToArray();
                for (int i2 = 0; i < childs.Length; i++)
                {
                    if (childs[i2] is ParenthesizedLambdaExpressionSyntax lambda)
                        return lambda.ParameterList.Parameters.ToArray();
                }
            }

            return null;
        }

        private void PrintDelegate(StringBuilder sb, IteratorArguments[] arguments)
        {
            sb.Append($"public delegate void ");
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i != 0)
                    sb.Append("_");

                var argument = arguments[i];
                sb.Append(argument.shared ? "S" : "");
                sb.Append(argument.write ? "W" : "R");
                sb.Append(argument.optional ? "O" : "");
            }
            sb.Append("<");
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append($"C{i}");
            }
            sb.Append(">");
            sb.Append("(");
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var argument = arguments[i];
                sb.Append(argument.write ? "ref " : "in ");
                sb.Append($"C{i}");
                sb.Append(argument.optional ? "?" : "");
                sb.Append($" c{i}");
            }
            sb.Append(")");
            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                sb.Append(" where ");
                sb.Append($"C{i} : struct, {(argument.shared ? "ISharedComponent" : "IComponent")}");
            }
            sb.Append(";\n");
        }

        private void PrintIterator(StringBuilder sb, IteratorArguments[] arguments)
        {

            sb.Append($"public IIteratorPhase2 Iterate<");
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append($"C{i}");
            }
            sb.Append(">(");
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i != 0)
                    sb.Append("_");

                var argument = arguments[i];
                sb.Append(argument.shared ? "S" : "");
                sb.Append(argument.write ? "W" : "R");
                sb.Append(argument.optional ? "O" : "");
            }
            sb.Append("<");
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append($"C{i}");
            }
            sb.Append("> iterator)");
            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                sb.Append(" where ");
                sb.Append($"C{i} : struct, {(argument.shared ? "ISharedComponent" : "IComponent")}");
            }

            sb.Append("\n{\n");
            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                sb.Append($"TypeId id{i} = TypeManager.GetIdFromType(typeof(C{i}));\n");
                sb.Append($"AddType({(argument.optional ? "anyTypes" : "withTypes")}, id{i});\n");
            }
            sb.Append("BakeSettings();\n");
            sb.Append("iteration = IteratorUtils.ForEachChunk(world, settings, Version, (chunk) => \n{\n");
            for (int i = 0; i < arguments.Length; i++)
            {
                IteratorArguments argument = arguments[i];
                if (argument.optional)
                    sb.Append($"bool flag{i} = chunk.TryGetComponents(id{i}, out ComponentArray collection{i});\n");
                else
                    sb.Append($"chunk.TryGetComponents(id{i}, out ComponentArray collection{i});\n");
            }
            sb.Append("for (int i = 0; i < chunk.Count; i++)\n");
            sb.Append("{\n");
            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                if (argument.optional)
                    sb.Append($"C{i}? c{i} = flag{i} ? collection{i}.Get<C{i}>(i) : null;\n");
                else
                    sb.Append($"ref C{i} c{i} = ref collection{i}.GetRef<C{i}>(i);\n");
            }
            sb.Append("iterator(");
            for (int i = 0; i < arguments.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var argument = arguments[i];
                sb.Append(argument.write ? "ref" : "in");
                sb.Append($" c{i}");
            }
            sb.Append(");\n");
            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                if (argument.optional && argument.write)
                    sb.Append($"if(flag{i}) collection{i}[i] = c{i};\n");
            }
            sb.Append("}\n");
            sb.Append("});\n");
            sb.Append("return this;\n");
            sb.Append("}\n\n");
        }
        /*
        public delegate void W<C0>(ref C0 write0) where C0 : struct, IComponent;

        public IIteratorPhase2 Iterate<C0>(GeneratedIterators.W<C0> iterator) where C0 : struct, IComponent
        {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            BakeSettings();
            SetDelegate((chunk) =>
            {
                chunk.TryGetComponents(id0, out ComponentArray collection0);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(ref Unsafe.Unbox<C0>(collection0[e]));
                }
            });

            return this;
        }
        */

        private const string IteratorMethods = @"
        private List<TypeId> anyTypes = new();
        private List<TypeId> withTypes = new();
        private List<TypeId> noneTypes = new();

        private EntityWorld world;
        private QuerySettings settings;
        private IteratorAction iteration;

        public uint Version { get; private set; }

        public EntityIterator()
        {
            settings = new();
        }

        public void SetWorld(EntityWorld world) => this.world = world;

        private void AddType(List<TypeId> list, TypeId type)
        {
            if (!list.Contains(type))
                list.Add(type);
        }

        public IIteratorPhase3 SetVersion(uint version)
        {
            Version = version;
            return this;
        }

        IIteratorPhase1 IIteratorPhase1.Any(params TypeId[] types) => Any(types);
        public IIteratorPhase1 Any(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(anyTypes, type);

            return this;
        }

        IIteratorPhase1 IIteratorPhase1.None(params TypeId[] types) => None(types);
        public IIteratorPhase1 None(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(noneTypes, type);
            return this;
        }

        IIteratorPhase1 IIteratorPhase1.With(params TypeId[] types) => With(types);
        public EntityIterator With(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(withTypes, type);
            return this;
        }

        private void BakeSettings()
        {
            settings.With(withTypes.ToArray());
            settings.Any(anyTypes.ToArray());
            settings.None(noneTypes.ToArray());
        }

        public void Synchronous()
        {
            iteration();
        }
";
    }

    public class EntityIteratorsFinder : ISyntaxReceiver
    {
        public List<Stuff> ids = new List<Stuff>();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is IdentifierNameSyntax invocationExpressionSyntax && invocationExpressionSyntax.Parent is MemberAccessExpressionSyntax)
            {
                ids.Add(new Stuff() { tree = syntaxNode.SyntaxTree, invocation = invocationExpressionSyntax });
            }
        }
    }

    public class Stuff
    {
        public SyntaxTree tree;
        public IdentifierNameSyntax invocation;
    }
}
