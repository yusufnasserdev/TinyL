using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {
    partial class Parser {
        bool LookaheadIf() {
            return (StreamIndex < TokenStream.Count && TokenStream[StreamIndex].token_type == Token_Class.If);
        }
        private Node? IfStatement() {
            Node ifKeyword = new Node("If");
            Node next = ifKeyword;
            do {
                Node? matchedIf = match(next == ifKeyword ? Token_Class.If : Token_Class.ElseIf, next);
                Node? conditions = Conditions();
                Node? matchedThen = match(Token_Class.Then, next);
                Node? statements = Statements(ifElseIfBreaker);
                if (matchedIf == null || conditions == null ||
                    matchedThen == null || statements == null) {
                    return null;
                }
                next.Children.Add(matchedIf);
                next.Children.Add(conditions);
                next.Children.Add(matchedThen);
                next.Children.Add(statements);
                if (StreamIndex >= TokenStream.Count ||
                    TokenStream[StreamIndex].token_type != Token_Class.ElseIf) {
                    break;
                }
                next = new Node("ElseIf");
                ifKeyword.Children.Add(next);
            } while (true);
            ElseStatement(ifKeyword);
            if (ifKeyword.Children.Last().Name != "end") {
                return null;
            }
            return ifKeyword;
        }
        private void ElseStatement(Node ifStatement) {
            if (StreamIndex >= TokenStream.Count) {
                return;
            }
            Token_Class tokenClass = TokenStream[StreamIndex].token_type;
            if (tokenClass == Token_Class.Else) {
                Node elseStatement = new Node("Else");
                elseStatement.Children.Add(match(tokenClass, elseStatement));
                Node? statements = Statements(elseBreaker);
                if (statements == null) {
                    return;
                }
                elseStatement.Children.Add(statements);
                ifStatement.Children.Add(elseStatement);
            }
            Node? matchedEnd = match(Token_Class.End, ifStatement);
            if (matchedEnd != null) {
                ifStatement.Children.Add(matchedEnd);
            }
        }
        private Node? Conditions() {
            Node conditions = new Node("Conditions");
            Node? condition = Condition();
            if (condition == null) {
                return null;
            }
            conditions.Children.Add(condition);
            while (StreamIndex < TokenStream.Count) {
                Node? logicalOperator = LogicalOperator();
                if (logicalOperator == null) {
                    break;
                }
                conditions.Children.Add(logicalOperator);
                condition = Condition();
                if (condition == null) {
                    return null;
                }
                conditions.Children.Add(condition);
            }
            return conditions;
        }
        private Node? LogicalOperator() {
            Node logicalOperator = new Node("LogicalOperator");
            Token_Class tokenClass = TokenStream[StreamIndex].token_type;
            if (tokenClass != Token_Class.AndOp && tokenClass != Token_Class.OrOp) {
                return null;
            }
            logicalOperator.Children.Add(match(tokenClass, logicalOperator));
            return logicalOperator;
        }
        private Node? Condition() {
            Node condition = new Node("Condition");
            Node? identifier = Identifier();
            Node? conditionOperator = ConditionOperator();
            Node? term = Term();
            if (conditionOperator == null ||
                identifier == null || term == null) {
                return null;
            }
            condition.Children.Add(identifier);
            condition.Children.Add(conditionOperator);
            condition.Children.Add(term);
            return condition;
        }

        private Node? ConditionOperator() {
            Node conditionOperator = new Node("ConditionOperator");
            Token_Class? tokenClass = LookaheadConditionOperator();
            if (tokenClass == null) {
                Errors.ErrorsList.Add("Expected condition operator.");
                return null;
            }
            conditionOperator.Children.Add(match((Token_Class)tokenClass, conditionOperator));
            return conditionOperator;
        }
        private Token_Class? LookaheadConditionOperator() {
            if (StreamIndex >= TokenStream.Count) {
                return null;
            }
            switch (TokenStream[StreamIndex].token_type) {
                case Token_Class.GreaterThanOp:
                case Token_Class.LessThanOp:
                case Token_Class.EqualOp:
                case Token_Class.NotEqualOp:
                    return TokenStream[StreamIndex].token_type;
            }
            return null;
        }
    }
}
