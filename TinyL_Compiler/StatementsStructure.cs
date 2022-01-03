using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {
    // Statements Structure
    partial class Parser {
        private Node? Term() {
            Node term = new Node("Identifier");
            term.Children.Add(match(Token_Class.Identifier, term));
            return term;
        }
        private Node? Identifier(Node? parent = null) {
            Node identifier = new Node("Identifier");
            Node? identifierName = match(Token_Class.Identifier, parent == null ? identifier : parent);
            if (identifierName == null) {
                return null;
            }
            identifier.Children.Add(identifierName);
            return identifier;
        }
        private Node? Statements() {
            Node statements = new Node("Statements");
            while (StreamIndex < TokenStream.Count && TokenStream[StreamIndex].token_type == Token_Class.If) {
                statements.Children.Add(IfStatement());
            }
            Node retNode = new Node("Return");
            statements.Children.Add(retNode);
            return statements;
        }
    }
}
