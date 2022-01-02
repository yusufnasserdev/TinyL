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
        public Parser(List<Token> TokenStream) {
            this.TokenStream = TokenStream;
            StreamIndex = 0;
            Root = null;
        }
        public Node? StartParsing() {
            // TODO:
            return null;
        }
        public Node? match(Token_Class ExpectedToken) {

            if (StreamIndex < TokenStream.Count) {
                if (ExpectedToken == TokenStream[StreamIndex].token_type) {
                    StreamIndex++;
                    Node newNode = new Node(ExpectedToken.ToString());

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
