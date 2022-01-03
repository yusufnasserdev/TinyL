using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler {
    partial class Parser {
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
