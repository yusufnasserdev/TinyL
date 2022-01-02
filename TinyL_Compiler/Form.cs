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
            TinyL_Compiler.ClearCompiledCode();
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
            DataTable errorTable = new DataTable("Errors");
            errorTable.Columns.Add("Error Type");
            errorTable.Columns.Add("Token");
            foreach (string error in Errors.ErrorsList)
            {
                errorTable.Rows.Add("Unrecognized Token", error);
            }
            errorsDataGridView.DataSource = errorTable;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            clearContent();
        }

        private void clearContent()
        {
            TinyL_Compiler.ClearCompiledCode();
            errorsDataGridView.DataSource = null;
            lexemesGridView.DataSource = null;
            sourceCodeTextView.Text = "";
        }
    }
}