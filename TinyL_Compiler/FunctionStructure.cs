using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {
    // Function Struction
    partial class Parser {
        private Node? Parameter() {
            Node parameter = new Node("Parameter");
            Node? datatype = DataType();
            if (datatype != null) {
                parameter.Children.Add(datatype);
                Node? identifier = Identifier();
                if (identifier != null) {
                    parameter.Children.Add(identifier);
                    return parameter;
                }
            }
            return null;
        }
        private Node? Parameters() {
            Node parameters = new Node("Parameters");
            if (LookaheadDatatype()) {
                Node? parameter = Parameter();
                if (parameter == null) {
                    return null;
                }
                parameters.Children.Add(parameter);
                while (StreamIndex < TokenStream.Count &&
                    TokenStream[StreamIndex].token_type == Token_Class.Comma) {
                    Node? comma = match(Token_Class.Comma, parameters);
                    parameters.Children.Add(comma);
                    parameter = Parameter();
                    if (parameter == null) {
                        return null;
                    }
                    parameters.Children.Add(parameter);
                }
            }
            return parameters;
        }
        private Node? FunctionDeclaration() {
            Node functionDeclaration = new Node("FunctionDeclaration");
            Node? datatype = DataType();
            Node? identifier = Identifier(functionDeclaration);
            Node? leftParenthesis = match(Token_Class.LParenthesis, functionDeclaration);
            Node? parameters = Parameters();
            Node? rightParenthesis = match(Token_Class.RParenthesis, functionDeclaration);
            if (datatype == null || identifier == null ||
                leftParenthesis == null || parameters == null || rightParenthesis == null) {
                return null;
            }
            functionDeclaration.Children.Add(datatype);
            functionDeclaration.Children.Add(identifier);
            functionDeclaration.Children.Add(leftParenthesis);
            functionDeclaration.Children.Add(parameters);
            functionDeclaration.Children.Add(rightParenthesis);
            return functionDeclaration;
        }
        private Node? FunctionBody() {
            Node functionBody = new Node("FunctionBody");
            Node? lBrace = match(Token_Class.LBrace, functionBody);
            Node? statements = Statements(functionBreaker);
            Node? rBrace = match(Token_Class.RBrace, functionBody);
            if (lBrace == null || statements == null || rBrace == null) {
                return null;
            }
            if (statements.Children.Count == 0 ||
                statements.Children.Last().Name != "Return") {
                Errors.ErrorsList.Add("Expected return at the end of the function!");
                return null;
            }
            functionBody.Children.Add(lBrace);
            functionBody.Children.Add(statements);
            functionBody.Children.Add(rBrace);
            return functionBody;
        }
        private Node? Function() {
            Node? declaration = FunctionDeclaration();
            Node? body = FunctionBody();
            if (declaration != null && body != null) {
                Node functionNode = new Node("Function");
                functionNode.Children.Add(declaration);
                functionNode.Children.Add(body);
                return functionNode;
            }
            return null;
        }
        private Node? Functions() {
            Node functions = new Node("Functions");
            Node? function = Function();
            if (function == null) {
                return null;
            }
            while (function != null) {
                functions.Children.Add(function);
                if (!(TokenStream.Count - StreamIndex > 2 && LookaheadDatatype()
                    && TokenStream[StreamIndex + 1].token_type == Token_Class.Identifier
                    && TokenStream[StreamIndex + 2].token_type == Token_Class.LParenthesis)) {
                    break;
                }
                function = Function();
            }
            return functions;
        }
    }
}
