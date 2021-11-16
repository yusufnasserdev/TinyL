using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Class
{

    Number, Comment, Identifier, Int, Float, String, Read, Write,
    Repeat, Until, If, ElseIf, Else, Then, Return, Endl, PlusOp,
    MinusOp, MultiplyOp, DivideOp, AssignOp, LParenthesis,
    RParenthesis, OrOp, AndOp, GreaterThanOp, LessThanOp, Comma,
    NotEqualOp, EqualOp, RBrace, LBrace, Semicolon, End
}

namespace TinyL_Compiler
{
    
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        List<string> ErrorsList = new List<string>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();
        readonly Regex NumberRegex = new Regex(@"^[0-9]+(\.[0-9]*)?$", RegexOptions.Compiled);
        readonly Regex StringRegex = new Regex("^\"[^\"]*\"$", RegexOptions.Compiled);
        readonly Regex CommentRegex = new Regex(@"^/*([^*]|(\*+[^/]))*\*/$", RegexOptions.Compiled);
        readonly Regex IdentifierRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);
        
        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("write", Token_Class.Write);

            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParenthesis);
            Operators.Add(")", Token_Class.RParenthesis);
            Operators.Add("{", Token_Class.LBrace);
            Operators.Add("}", Token_Class.RBrace);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add(":=", Token_Class.AssignOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("&&", Token_Class.AndOp);
        }
        public void StartScanning(string SourceCode)
        {
            // i: Outer loop to check on lexemes.
            for (int i = 0; i < SourceCode.Length; i++)
            {
                // j: Inner loop to check on each character in a single lexeme.
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                // Identifier 
                if (char.IsLetter(CurrentChar))
                {
                    ++j;
                    while (j < SourceCode.Length) {
                        CurrentChar = SourceCode[j];
                        if (!char.IsLetter(CurrentChar)
                        && !char.IsDigit(CurrentChar))
                        {
                            break;
                        }
                        CurrentLexeme += CurrentChar.ToString();
                        ++j;
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                // Number
                else if (char.IsDigit(CurrentChar))
                {
                    ++j;
                    while (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        if (!(char.IsDigit(CurrentChar) || CurrentChar == '.'))
                        {
                            break;
                        }
                        CurrentLexeme += CurrentChar.ToString();
                        ++j;
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                // Comment or Divide
                else if (CurrentChar == '/')
                {
                    CurrentChar = SourceCode[++j];
                    if (CurrentChar == '*')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        ++j;
                        while (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                            CurrentLexeme += CurrentChar.ToString();
                            if (CurrentChar == '*' && j + 1 < SourceCode.Length && SourceCode[j + 1] == '/')
                            {
                                CurrentLexeme += SourceCode[j + 1].ToString();
                                j += 2;
                                break;
                            }
                            ++j;
                        }
                        i = j - 1;
                    }
                    FindTokenClass(CurrentLexeme);
                }
                // String
                else if(CurrentChar == '\"')
                {
                    j++;
                    while(j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        if(CurrentChar == '\"'/* && SourceCode[j - 1] != '\\'*/)
                        {
                            ++j;
                            break;
                        }
                        j++;
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                else
                {
                    j++;
                    while(j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        if (char.IsDigit(CurrentChar) || char.IsLetter(CurrentChar)
                            || CurrentChar == '\"' || CurrentChar == '/'
                            || char.IsWhiteSpace(CurrentChar)) {
                            break;
                        }
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
            }

            TinyL_Compiler.TokenStream = Tokens;
            TinyL_Compiler.ErrorList = ErrorsList;
        }
        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identifier;
                Tokens.Add(Tok);
            }
            // Is it number?
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
            //Is it a comment?
            else if (isComment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if (isOperator(Lex))
            {
                if (Operators.ContainsKey(Lex))
                {
                    Tok.token_type = Operators[Lex];
                    Tokens.Add(Tok);
                } else
                {
                    foreach (char c in Lex)
                    {
                        if (Operators.ContainsKey(c.ToString()))
                        {
                            Tokens.Add(new Token {
                                lex = c.ToString(),
                                token_type = Operators[c.ToString()]
                            });
                        } else
                        {
                            ErrorsList.Add(c.ToString());
                        }
                    }
                }
            }
            //Is it string?
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            //Is it an undefined?
            else
            {
                ErrorsList.Add(Lex);
            }
        }
        // Don't forget to add String
        bool isIdentifier(string lex)
        {
            //MessageBox.Show(IdentifierRegex.ToString());
            return IdentifierRegex.IsMatch(lex);
        }
        bool isNumber(string lex)
        {
            return NumberRegex.IsMatch(lex);
        }
        bool isComment(string lex)
        {
            return CommentRegex.IsMatch(lex);
        }
        bool isOperator(string lex)
        {
            if (Operators.ContainsKey(lex))
            {
                return true;
            }
            foreach (char c in lex)
            {
                if (Operators.ContainsKey(c.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        bool isString(string lex)
        {
            return StringRegex.IsMatch(lex);
        }
    }
}
