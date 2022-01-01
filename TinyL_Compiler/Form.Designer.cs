namespace TinyL_Compiler
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CompileButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.sourceCodeTextView = new System.Windows.Forms.TextBox();
            this.lexemesGridView = new System.Windows.Forms.DataGridView();
            this.errorsDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.lexemesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CompileButton
            // 
            this.CompileButton.Location = new System.Drawing.Point(12, 527);
            this.CompileButton.Name = "CompileButton";
            this.CompileButton.Size = new System.Drawing.Size(84, 31);
            this.CompileButton.TabIndex = 0;
            this.CompileButton.Text = "Compile";
            this.CompileButton.UseVisualStyleBackColor = true;
            this.CompileButton.Click += new System.EventHandler(this.CompileButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(103, 527);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(84, 31);
            this.ClearButton.TabIndex = 1;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // sourceCodeTextView
            // 
            this.sourceCodeTextView.Location = new System.Drawing.Point(12, 12);
            this.sourceCodeTextView.Multiline = true;
            this.sourceCodeTextView.Name = "sourceCodeTextView";
            this.sourceCodeTextView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.sourceCodeTextView.Size = new System.Drawing.Size(501, 276);
            this.sourceCodeTextView.TabIndex = 2;
            // 
            // lexemesGridView
            // 
            this.lexemesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lexemesGridView.Location = new System.Drawing.Point(519, 12);
            this.lexemesGridView.Name = "lexemesGridView";
            this.lexemesGridView.ReadOnly = true;
            this.lexemesGridView.RowHeadersWidth = 51;
            this.lexemesGridView.RowTemplate.Height = 29;
            this.lexemesGridView.Size = new System.Drawing.Size(530, 546);
            this.lexemesGridView.TabIndex = 3;
            // 
            // errorsDataGridView
            // 
            this.errorsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.errorsDataGridView.Location = new System.Drawing.Point(12, 294);
            this.errorsDataGridView.Name = "errorsDataGridView";
            this.errorsDataGridView.ReadOnly = true;
            this.errorsDataGridView.RowHeadersWidth = 51;
            this.errorsDataGridView.RowTemplate.Height = 29;
            this.errorsDataGridView.Size = new System.Drawing.Size(500, 227);
            this.errorsDataGridView.TabIndex = 6;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 570);
            this.Controls.Add(this.errorsDataGridView);
            this.Controls.Add(this.lexemesGridView);
            this.Controls.Add(this.sourceCodeTextView);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.CompileButton);
            this.Name = "Form";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.lexemesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button CompileButton;
        private Button ClearButton;
        private TextBox sourceCodeTextView;
        private DataGridView lexemesGridView;
        private DataGridView errorsDataGridView;
    }
}