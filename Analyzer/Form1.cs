namespace Analyzer
{
    public partial class Form1 : Form
    {
        private Parser parser;
        private string Input;
        public Form1()
        {
            InitializeComponent();
        }

        private void inputButton_Click(object sender, EventArgs e)
        {
            inputButtonFunctionality();
        }

        private void clearInputButton_Click(object sender, EventArgs e)
        {
            inputTextBox.Text = string.Empty;
            Input = string.Empty;
            analyzeOutputTextBox.Text = string.Empty;
            semanticsOutputTextBox.Text = string.Empty;
            semanticsButton.Enabled = false;
            parser = new Parser(Input);
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
            inputButtonFunctionality();

            ParseResult parseResult = parser.Parse();
            if (parseResult is ParseSuccess)
            {
                semanticsButton.Enabled = true;
                analyzeOutputTextBox.Text = analyzeOutputTextBox.Text + "Строка принадлежит заданному языку.";
            }

            if (parseResult is ParseError)
            {
                semanticsButton.Enabled = false;
                analyzeOutputTextBox.Text = (parseResult as ParseError).Message;
                inputTextBox.Select(parseResult.Position, 0);
                inputTextBox.Focus();
            }
        }

        private void semanticsButton_Click(object sender, EventArgs e)
        {
            parser = new Parser(Input);
            ParseResult parseResult = parser.Parse();
            semanticsOutputTextBox.Text = (parseResult as ParseSuccess).Message;
        }

        private void inputButtonFunctionality()
        {
            Input = inputTextBox.Text;
            if (Input != string.Empty)
            {
                analyzeOutputTextBox.Text = $"Вы ввели: {Input}\r\n";
            }
            else
            {
                analyzeOutputTextBox.Text = string.Empty;
            }
            semanticsOutputTextBox.Text = string.Empty;
            semanticsButton.Enabled = false;
            parser = new Parser(Input);
        }
    }
}
