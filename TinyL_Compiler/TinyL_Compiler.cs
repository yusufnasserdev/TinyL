using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyL_Compiler
{
    public static class TinyL_Compiler
    {
        public static Scanner TinyL_Scanner = new Scanner();

        public static List<string> Lexemes = new List<string>();

        public static List<Token> TokenStream = new List<Token>();
        public static Parser parser;
        public static Node tree;

        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner

            TinyL_Scanner.StartScanning(SourceCode);

            //Parser
            parser = new Parser(TokenStream);
            tree = parser.StartParsing();
            //Sematic Analysis
        }
        public static void ClearCompiledCode()
        {
            Lexemes.Clear();
            TokenStream.Clear();
            Errors.ErrorsList.Clear();
        }
        public static void SplitLexemes(string SourceCode)
        {
            string[] Lexemes_arr = SourceCode.Split(' ');
            for (int i = 0; i < Lexemes_arr.Length; i++)
            {
                if (Lexemes_arr[i].Contains("\r\n"))
                {
                    Lexemes_arr[i] = Lexemes_arr[i].Replace("\r\n", string.Empty);
                }
                Lexemes.Add(Lexemes_arr[i]);
            }

        }
    }
}
