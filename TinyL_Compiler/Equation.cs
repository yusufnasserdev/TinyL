using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {
    partial class Parser {
        private Node? Equation() {
            if (StreamIndex >= TokenStream.Count) {
                return null;
            }
            Node equation = new Node("Equation");
            // Parentheses
            if (TokenStream[StreamIndex].token_type == Token_Class.LParenthesis) {
                equation.Children.Add(match(Token_Class.LParenthesis, equation));
                Node? subEquation = Equation();
                if (subEquation == null) {
                    return null;
                }
                equation.Children.AddRange(subEquation.Children);
                equation.Children.Add(match(Token_Class.RParenthesis, equation));
                if (!Term_after_operator(equation)) {
                    return null;
                }
            } else {
                if (!Terms_before_operator_or_ends_term(equation)) {
                    return null;
                }
            }
            return equation;
        }
        private bool Terms_before_operator_or_ends_term(Node equation) {
            while (true) {
                Node? term = Term();
                if (term == null) {
                    return false;
                }
                equation.Children.Add(term);
                if (StreamIndex >= TokenStream.Count) {
                    return false;
                }
                Token_Class? matchedToken = MatchOperatorIfPossible(equation);
                if (matchedToken == null) {
                    break;
                }
                if (StreamIndex >= TokenStream.Count) {
                    return false;
                }
                if (TokenStream[StreamIndex].token_type == Token_Class.LParenthesis) {
                    Node? subEquation = Equation();
                    if (subEquation == null) {
                        return false;
                    }
                    equation.Children.AddRange(subEquation.Children);
                    break;
                }
            }
            return true;
        }
        bool Term_after_operator(Node equation) {
            while (true) {
                if (StreamIndex >= TokenStream.Count) {
                    return false;
                }
                Token_Class? matchedToken = MatchOperatorIfPossible(equation);
                if (matchedToken == null) {
                    break;
                }
                if (StreamIndex >= TokenStream.Count) {
                    return false;
                }
                if (TokenStream[StreamIndex].token_type == Token_Class.LParenthesis) {
                    Node? subEquation = Equation();
                    if (subEquation == null) {
                        return false;
                    }
                    equation.Children.AddRange(subEquation.Children);
                    break;
                }
                Node? term = Term();
                if (term == null) {
                    return false;
                }
                equation.Children.Add(term);
            }
            return true;
        }
        Token_Class? MatchOperatorIfPossible(Node equation) {
            if (TokenStream[StreamIndex].token_type == Token_Class.PlusOp) {
                equation.Children.Add(match(Token_Class.PlusOp, equation));
                return Token_Class.PlusOp;
            }
            if (TokenStream[StreamIndex].token_type == Token_Class.MinusOp) {
                equation.Children.Add(match(Token_Class.MinusOp, equation));
                return Token_Class.MinusOp;
            }
            if (TokenStream[StreamIndex].token_type == Token_Class.MultiplyOp) {
                equation.Children.Add(match(Token_Class.MultiplyOp, equation));
                return Token_Class.MultiplyOp;
            }
            if (TokenStream[StreamIndex].token_type == Token_Class.DivideOp) {
                equation.Children.Add(match(Token_Class.DivideOp, equation));
                return Token_Class.DivideOp;
            }
            return null;
        }
    }
}
