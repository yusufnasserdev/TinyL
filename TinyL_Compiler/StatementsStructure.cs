using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {
    // Statements Structure
    partial class Parser {
        bool LookaheadFunctionCall() {
            return (StreamIndex + 1 < TokenStream.Count &&
                TokenStream[StreamIndex].token_type == Token_Class.Identifier &&
                TokenStream[StreamIndex + 1].token_type == Token_Class.LParenthesis);
        }
        Node? FunctionCall() {
            Node functionCall = new Node("FunctionCall");
            Node? identifier = Identifier();
            Node? lParent = match(Token_Class.LParenthesis, functionCall);
            if (identifier == null || lParent == null) {
                return null;
            }
            functionCall.Children.Add(identifier);
            functionCall.Children.Add(lParent);
            if (StreamIndex < TokenStream.Count &&
                TokenStream[StreamIndex].token_type != Token_Class.RParenthesis) {
                while (true) {
                    Node? argument = Identifier();
                    if (argument == null) {
                        return null;
                    }
                    functionCall.Children.Add(argument);
                    if (StreamIndex < TokenStream.Count &&
                        TokenStream[StreamIndex].token_type != Token_Class.Comma) {
                        break;
                    }
                    Node? comma = match(Token_Class.Comma, functionCall);
                    functionCall.Children.Add(comma);
                }
            }
            Node? rParent = match(Token_Class.RParenthesis, functionCall);
            if (rParent == null) {
                return null;
            }
            functionCall.Children.Add(rParent);
            return functionCall;
        }
        bool LookaheadReadStatement() {
            return (StreamIndex < TokenStream.Count && TokenStream[StreamIndex].token_type == Token_Class.Read);
        }
        Node? ReadStatement() {
            Node read = new Node("Read");
            Node? matchedRead = match(Token_Class.Read, read);
            Node? identifier = Identifier();
            Node? matchSemicolon = match(Token_Class.Semicolon, read);
            if (matchedRead == null || identifier == null || matchSemicolon == null) {
                return null;
            }
            read.Children.Add(matchedRead);
            read.Children.Add(identifier);
            read.Children.Add(matchSemicolon);
            return read;
        }
        bool LookaheadWriteStatement() {
            return (StreamIndex < TokenStream.Count && TokenStream[StreamIndex].token_type == Token_Class.Write);
        }
        Node? WriteStatement() {
            Node write = new Node("Write");
            Node? matchedWrite = match(Token_Class.Write, write);
            Node? writable;
            if (StreamIndex < TokenStream.Count && TokenStream[StreamIndex].token_type == Token_Class.Endl) {
                writable = match(Token_Class.Endl, write);
            } else {
                writable = Expression();
            }
            Node? matchSemicolon = match(Token_Class.Semicolon, write);
            if (matchedWrite == null || writable == null || matchSemicolon == null) {
                return null;
            }
            write.Children.Add(matchedWrite);
            write.Children.Add(writable);
            write.Children.Add(matchSemicolon);
            return write;
        }
        bool LookaheadReturn() {
            return (StreamIndex < TokenStream.Count && TokenStream[StreamIndex].token_type == Token_Class.Return);
        }
        Node? ReturnStatement() {
            Node? ret = new Node("Return");
            Node? matchedReturn = match(Token_Class.Return, ret);
            Node? expression = Expression();
            Node? matchSemicolon = match(Token_Class.Semicolon, ret);
            if (matchedReturn == null || expression == null || matchSemicolon == null) {
                return null;
            }
            ret.Children.Add(matchedReturn);
            ret.Children.Add(expression);
            ret.Children.Add(matchSemicolon);
            return ret;
        }
        private Node? Statements(List<Token_Class> tokens) {
            Node statements = new Node("Statements");
            while (StreamIndex < TokenStream.Count &&
                !tokens.Contains(TokenStream[StreamIndex].token_type)) {
                int i = 0;
                for (; i < lookaheads.Length; ++i) {
                    if (lookaheads[i]()) { break; }
                }
                if (i == lookaheads.Length) {
                    return null;
                }
                Node? node = lookedStatements[i]();
                if (node == null) {
                    return null;
                }
                if (node.Name == "AssignmentStatement" || node.Name == "FunctionCall") {
                    Node? semicolon = match(Token_Class.Semicolon, node);
                    if (semicolon == null) { return null; }
                    node.Children.Add(semicolon);
                }
                statements.Children.Add(node);
            }
            return statements;
        }
    }
}
