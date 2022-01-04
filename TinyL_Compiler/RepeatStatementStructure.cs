using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {
    partial class Parser {
        bool LookaheadRepeat() {
            return StreamIndex < TokenStream.Count && TokenStream[StreamIndex].token_type == Token_Class.Repeat;
        }
        Node? RepeatStatement() {
            Node repeat = new Node("Repeat");
            Node? matchedRepeat = match(Token_Class.Repeat, repeat);
            Node? statements = Statements(repeatBreaker);
            Node? matchedUntil = match(Token_Class.Until, repeat);
            Node? condition = Conditions();
            if (matchedRepeat == null || statements == null ||
                matchedUntil == null || condition == null) {
                return null;
            }
            repeat.Children.Add(matchedRepeat);
            repeat.Children.Add(statements);
            repeat.Children.Add(matchedUntil);
            repeat.Children.Add(condition);
            return repeat;
        }
    }
}
