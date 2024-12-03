namespace Analyzer
{
    partial class Form1
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
            inputTextBox = new TextBox();
            inputButton = new Button();
            analyzeButton = new Button();
            semanticsButton = new Button();
            clearInputButton = new Button();
            analyzeOutputTextBox = new TextBox();
            semanticsOutputTextBox = new TextBox();
            SuspendLayout();
            // 
            // inputTextBox
            // 
            inputTextBox.BackColor = Color.White;
            inputTextBox.Font = new Font("Cascadia Mono", 9.216F);
            inputTextBox.Location = new Point(12, 21);
            inputTextBox.Name = "inputTextBox";
            inputTextBox.PlaceholderText = "Введите строку...";
            inputTextBox.ScrollBars = ScrollBars.Horizontal;
            inputTextBox.Size = new Size(717, 26);
            inputTextBox.TabIndex = 0;
            // 
            // inputButton
            // 
            inputButton.BackColor = Color.White;
            inputButton.Font = new Font("Cascadia Mono", 9.216F);
            inputButton.Location = new Point(12, 56);
            inputButton.Name = "inputButton";
            inputButton.Size = new Size(235, 36);
            inputButton.TabIndex = 1;
            inputButton.Text = "Ввод";
            inputButton.UseVisualStyleBackColor = false;
            inputButton.Click += inputButton_Click;
            // 
            // analyzeButton
            // 
            analyzeButton.BackColor = Color.White;
            analyzeButton.Font = new Font("Cascadia Mono", 9.216F);
            analyzeButton.Location = new Point(253, 56);
            analyzeButton.Name = "analyzeButton";
            analyzeButton.Size = new Size(235, 36);
            analyzeButton.TabIndex = 2;
            analyzeButton.Text = "Анализ";
            analyzeButton.UseVisualStyleBackColor = false;
            analyzeButton.Click += analyzeButton_Click;
            // 
            // semanticsButton
            // 
            semanticsButton.BackColor = Color.White;
            semanticsButton.Enabled = false;
            semanticsButton.Font = new Font("Cascadia Mono", 9.216F);
            semanticsButton.Location = new Point(494, 56);
            semanticsButton.Name = "semanticsButton";
            semanticsButton.Size = new Size(235, 36);
            semanticsButton.TabIndex = 3;
            semanticsButton.Text = "Семантика";
            semanticsButton.UseVisualStyleBackColor = false;
            semanticsButton.Click += semanticsButton_Click;
            // 
            // clearInputButton
            // 
            clearInputButton.BackColor = Color.White;
            clearInputButton.Font = new Font("Cascadia Mono", 9.216F);
            clearInputButton.Location = new Point(735, 21);
            clearInputButton.Name = "clearInputButton";
            clearInputButton.Size = new Size(53, 29);
            clearInputButton.TabIndex = 4;
            clearInputButton.Text = "❌";
            clearInputButton.UseVisualStyleBackColor = false;
            clearInputButton.Click += clearInputButton_Click;
            // 
            // analyzeOutputTextBox
            // 
            analyzeOutputTextBox.BackColor = Color.White;
            analyzeOutputTextBox.Font = new Font("Cascadia Mono", 9.216F, FontStyle.Regular, GraphicsUnit.Point, 0);
            analyzeOutputTextBox.Location = new Point(12, 127);
            analyzeOutputTextBox.Multiline = true;
            analyzeOutputTextBox.Name = "analyzeOutputTextBox";
            analyzeOutputTextBox.PlaceholderText = "Здесь будут отражаться результаты синтаксического и семантического анализа...";
            analyzeOutputTextBox.ReadOnly = true;
            analyzeOutputTextBox.ScrollBars = ScrollBars.Vertical;
            analyzeOutputTextBox.Size = new Size(776, 127);
            analyzeOutputTextBox.TabIndex = 5;
            // 
            // semanticsOutputTextBox
            // 
            semanticsOutputTextBox.BackColor = Color.White;
            semanticsOutputTextBox.Font = new Font("Cascadia Mono", 9.216F);
            semanticsOutputTextBox.Location = new Point(12, 260);
            semanticsOutputTextBox.Multiline = true;
            semanticsOutputTextBox.Name = "semanticsOutputTextBox";
            semanticsOutputTextBox.PlaceholderText = "Здесь будет отображаться семантика...";
            semanticsOutputTextBox.ReadOnly = true;
            semanticsOutputTextBox.ScrollBars = ScrollBars.Vertical;
            semanticsOutputTextBox.Size = new Size(776, 127);
            semanticsOutputTextBox.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 420);
            Controls.Add(semanticsOutputTextBox);
            Controls.Add(analyzeOutputTextBox);
            Controls.Add(clearInputButton);
            Controls.Add(semanticsButton);
            Controls.Add(analyzeButton);
            Controls.Add(inputButton);
            Controls.Add(inputTextBox);
            Name = "Form1";
            Text = "Попов Ярослав. Вариант 7. Анализатор.";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox inputTextBox;
        private Button inputButton;
        private Button analyzeButton;
        private Button semanticsButton;
        private Button clearInputButton;
        private TextBox analyzeOutputTextBox;
        private TextBox semanticsOutputTextBox;
    }
}
