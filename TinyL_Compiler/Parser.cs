using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {

    public class Node {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string Name) {
            this.Name = Name;
        }
    }

    public partial class Parser {
        private int StreamIndex;
        private List<Token> TokenStream;
        public Node? Root;
        private List<string> functionsIdentifiers;
        private static readonly List<Token_Class> ifElseIfBreaker = new List<Token_Class>() {
            Token_Class.ElseIf, Token_Class.Else, Token_Class.End
        };
        private static readonly List<Token_Class> elseBreaker = new List<Token_Class> {
            Token_Class.End
        };
        private static readonly List<Token_Class> functionBreaker = new List<Token_Class> {
            Token_Class.RBrace
        };
        private static readonly List<Token_Class> repeatBreaker = new List<Token_Class> {
            Token_Class.Until
        };
        private delegate bool Lookahead();
        private delegate Node? Statement();
        private Lookahead[] lookaheads;
        private Statement[] lookedStatements;
        public Parser(List<Token> TokenStream) {
            functionsIdentifiers = new List<string>();
            this.TokenStream = TokenStream;
            StreamIndex = 0;
            Root = null;
            lookaheads = new Lookahead[] {
                LookaheadRepeat,
                LookaheadIf,
                LookaheadDatatype,
                LookaheadAssignmentStatement,
                LookaheadFunctionCall,
                LookaheadReadStatement,
                LookaheadWriteStatement,
                LookaheadReturn
            };
            lookedStatements = new Statement[] {
                RepeatStatement,
                IfStatement,
                DeclarationStatements,
                AssignmentStatement,
                FunctionCall,
                ReadStatement,
                WriteStatement,
                ReturnStatement
            };
        }
        public Node? StartParsing() {
            // TODO:
            //return RepeatStatement();
            Root = Program();
            Dictionary<string, int> repetation = new Dictionary<string, int>();
            bool validNames = true;
            foreach (string functionIdentifier in functionsIdentifiers) {
                if (!repetation.ContainsKey(functionIdentifier)) {
                    repetation.Add(functionIdentifier, 1);
                } else {
                    validNames = false;
                    Errors.ErrorsList.Add("Redeclaration of the function: " + functionIdentifier);
                }
            }
            if (functionsIdentifiers.Count == 0 || functionsIdentifiers.Last() != "main") {
                Errors.ErrorsList.Add("`main` function is not found at the end of the file.");
                validNames = false;
            }
            if (validNames) {
                return Root;
            }
            return null;
        }
        private Node? Program() {
            Node? program = new Node("Program");
            Node? functions = Functions();
            if (functions != null) {
                program.Children.Add(functions);
                return program;
            }
            return null;
        }
        public Node? match(Token_Class ExpectedToken, Node caller) {

            if (StreamIndex < TokenStream.Count) {
                if (ExpectedToken == TokenStream[StreamIndex].token_type) {
                    Node newNode = new Node(TokenStream[StreamIndex].lex);
                    StreamIndex++;
                    if (caller.Name == "FunctionDeclaration" && ExpectedToken == Token_Class.Identifier) {
                        functionsIdentifiers.Add(newNode.Name);
                    }
                    return newNode;

                } else {
                    Errors.ErrorsList.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[StreamIndex].token_type.ToString() +
                        "  found\r\n");
                    StreamIndex++;
                    return null;
                }
            } else {
                Errors.ErrorsList.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                StreamIndex++;
                return null;
            }
        }
        public static TreeNode PrintParseTree(Node root) {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode? treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        private static TreeNode? PrintTree(Node root) {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children) {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
