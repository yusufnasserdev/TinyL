using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {
    partial class Parser {
        private Node? Term() {
            //Node term = new Node("Identifier");
            //term.Children.Add(match(Token_Class.Identifier, term));
            //return term;
            // TODO:
            if (StreamIndex >= TokenStream.Count) {
                return null;
            }
            Node? term = new Node("Term");
            if (TokenStream[StreamIndex].token_type == Token_Class.Number) {
                Node number = new Node("Number");
                number.Children.Add(match(Token_Class.Number, term));
                term.Children.Add(number);
            } else if (LookaheadFunctionCall()) {
                Node? functionCall = FunctionCall();
                if (functionCall == null) {
                    return null;
                }
                term.Children.Add(functionCall);
            } else if (TokenStream[StreamIndex].token_type == Token_Class.Identifier) {
                term.Children.Add(Identifier());
            } else {
                return null;
            }
            return term;
        }
        private Node? DeclarationStatements() {
            Node? declarationStatement = new Node("DeclarationStatement");
            Node? datatype = DataType();
            if (datatype == null) { return null; }
            declarationStatement.Children.Add(datatype);
            while (true) {
                if (StreamIndex >= TokenStream.Count) {
                    return null;
                }
                if (LookaheadAssignmentStatement()) {
                    Node? assignmentStatement = AssignmentStatement();
                    if (assignmentStatement == null) {
                        return null;
                    }
                    declarationStatement.Children.Add(assignmentStatement);
                } else if (TokenStream[StreamIndex].token_type == Token_Class.Identifier) {
                    Node? identifier = Identifier();
                    if (identifier == null) {
                        return null;
                    }
                    declarationStatement.Children.Add(identifier);
                } else {
                    return null;
                }
                if (StreamIndex >= TokenStream.Count) {
                    return null;
                }
                if (TokenStream[StreamIndex].token_type != Token_Class.Comma) {
                    break;
                }
                declarationStatement.Children.Add(match(Token_Class.Comma, declarationStatement));
            }
            Node? semiColon = match(Token_Class.Semicolon, declarationStatement);
            if (semiColon == null) { return null; }
            declarationStatement.Children.Add(semiColon);
            return declarationStatement;
        }
        private bool LookaheadAssignmentStatement() {
            return (StreamIndex + 1 < TokenStream.Count &&
                TokenStream[StreamIndex].token_type == Token_Class.Identifier &&
                TokenStream[StreamIndex + 1].token_type == Token_Class.AssignOp);
        }
        private Node? AssignmentStatement() {
            Node? assignmentStatement = new Node("AssignmentStatement");
            Node? identifier = Identifier();
            Node? assignmentOperator = match(Token_Class.AssignOp, assignmentStatement);
            Node? expression = Expression();
            if (identifier == null || assignmentOperator == null || expression == null) {
                return null;
            }
            assignmentStatement.Children.Add(identifier);
            assignmentStatement.Children.Add(assignmentOperator);
            assignmentStatement.Children.Add(expression);
            return assignmentStatement;
        }

        private Node? Expression() {
            if (StreamIndex >= TokenStream.Count) {
                return null;
            }
            Node expression = new Node("Expression");
            if (TokenStream[StreamIndex].token_type == Token_Class.StringLiteral) {
                Node stringLiteral = new Node("StringLiteral");
                stringLiteral.Children.Add(match(Token_Class.StringLiteral, stringLiteral));
                expression.Children.Add(stringLiteral);
            } else {
                Node? equation = Equation();
                if (equation == null) {
                    return null;
                }
                if (equation.Children.Count == 1) {
                    expression.Children.Add(equation.Children[0]);
                } else {
                    expression.Children.Add(equation);
                }
            }
            return expression;
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
        private Node? DataType() {
            if (StreamIndex >= TokenStream.Count) {
                return null;
            }
            Node? datatype = new Node("DataType");
            Node? type = null;
            if (TokenStream[StreamIndex].token_type == Token_Class.Int) {
                type = match(Token_Class.Int, datatype);
            } else if (TokenStream[StreamIndex].token_type == Token_Class.String) {
                type = match(Token_Class.String, datatype);
            } else if (TokenStream[StreamIndex].token_type == Token_Class.Float) {
                type = match(Token_Class.Float, datatype);
            }
            if (type == null) {
                return null;
            }
            datatype.Children.Add(type);
            return datatype;
        }
        private bool LookaheadDatatype() {
            if (StreamIndex >= TokenStream.Count) {
                return false;
            }
            switch (TokenStream[StreamIndex].token_type) {
                case Token_Class.Int:
                case Token_Class.String:
                case Token_Class.Float:
                    return true;
            }
            return false;
        }
    }
}
