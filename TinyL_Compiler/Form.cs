using System.Data;
namespace TinyL_Compiler
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            string sourceCode = sourceCodeTextView.Text.ToString();
            TinyL_Compiler.Start_Compiling(sourceCode);
            DataTable lexemesTable = new DataTable("Lexemes Table");
            lexemesTable.Columns.Add("Lexeme");
            lexemesTable.Columns.Add("Token");
            foreach (Token token in  TinyL_Compiler.TokenStream)
            {
                lexemesTable.Rows.Add(token.lex, token.token_type.ToString());
            }
            lexemesGridView.DataSource = lexemesTable;
            //DataTable errorList = new DataTable("Error List");
            //errorList.Columns.Add("Error");
            foreach (string error in TinyL_Compiler.ErrorList)
            {
                errorsListView.Items.Add(error);
                //errorList.Rows.Add(error);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            TinyL_Compiler.ClearCompiledCode();
            errorsListView.Items.Clear();
            lexemesGridView.DataSource = null;
            sourceCodeTextView.Text = "";
        }

    }
}