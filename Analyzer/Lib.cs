using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Analyzer
{
    // Перечисление типов токенов
    public enum TokenType
    {
        IDENTIFIER,
        COLON,
        EQUAL,
        COMMA,
        SEMICOLON,
        RESERVED_WORD,
        STRING_LITERAL,
        INTEGER_LITERAL,
        REAL_LITERAL,
        END_OF_FILE,
        ERROR
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }
    }

    public class Lexer
    {
        private string input;
        private int position;
        private readonly Dictionary<string, TokenType> reservedWords = new Dictionary<string, TokenType>
        {
            { "CONST", TokenType.RESERVED_WORD },
            { "STRING", TokenType.RESERVED_WORD },
            { "WORD", TokenType.RESERVED_WORD },
            { "REAL", TokenType.RESERVED_WORD },
            { "INTEGER", TokenType.RESERVED_WORD },
            { "CHAR", TokenType.RESERVED_WORD }
        };

        public Lexer(string input)
        {
            this.input = input;
            position = 0;
        }

        public Token GetNextToken()
        {
            SkipWhitespace();

            if (position >= input.Length)
            {
                return new Token { Type = TokenType.END_OF_FILE, Position = position };
            }

            char currentChar = input[position];

            // Идентификатор или зарезервированное слово
            if (char.IsLetter(currentChar))
            {
                int startPos = position;
                StringBuilder sb = new StringBuilder();
                while (position < input.Length && (char.IsLetterOrDigit(input[position])))
                {
                    sb.Append(input[position]);
                    position++;
                }
                string lexeme = sb.ToString();
                TokenType type = reservedWords.ContainsKey(lexeme.ToUpper()) ? TokenType.RESERVED_WORD : TokenType.IDENTIFIER;
                return new Token { Type = type, Value = lexeme, Position = startPos };
            }

            // Числовая константа
            if (char.IsDigit(currentChar))
            {
                int startPos = position;
                StringBuilder sb = new StringBuilder();
                bool hasDecimalPoint = false;
                while (position < input.Length && (char.IsDigit(input[position]) || input[position] == '.'))
                {
                    if (input[position] == '.')
                    {
                        if (hasDecimalPoint)
                        {
                            // Ошибка: более одной десятичной точки
                            return new Token { Type = TokenType.ERROR, Value = "Некорректная числовая константа", Position = startPos };
                        }
                        hasDecimalPoint = true;
                    }
                    sb.Append(input[position]);
                    position++;
                }
                return new Token
                {
                    Type = hasDecimalPoint ? TokenType.REAL_LITERAL : TokenType.INTEGER_LITERAL,
                    Value = sb.ToString(),
                    Position = startPos
                };
            }

            // Строковая константа
            if (currentChar == '\'' || currentChar == '‘' || currentChar == '’') // Учитываем разные виды кавычек
            {
                int startPos = position;
                char quoteChar = '\'';
                if (currentChar == '‘') quoteChar = '’';
                position++;
                StringBuilder sb = new StringBuilder();
                while (position < input.Length && input[position] != quoteChar)
                {
                    sb.Append(input[position]);
                    position++;
                }
                if (position >= input.Length || input[position] != quoteChar)
                {
                    // Ошибка: незакрытая строка
                    return new Token { Type = TokenType.ERROR, Value = "Незакрытая строковая константа", Position = startPos };
                }
                position++; // Пропустить закрывающую кавычку
                return new Token { Type = TokenType.STRING_LITERAL, Value = sb.ToString(), Position = startPos };
            }

            // Символы
            switch (currentChar)
            {
                case ':':
                    position++;
                    return new Token { Type = TokenType.COLON, Value = ":", Position = position - 1 };
                case '=':
                    position++;
                    return new Token { Type = TokenType.EQUAL, Value = "=", Position = position - 1 };
                case ',':
                    position++;
                    return new Token { Type = TokenType.COMMA, Value = ",", Position = position - 1 };
                case ';':
                    position++;
                    return new Token { Type = TokenType.SEMICOLON, Value = ";", Position = position - 1 };
                default:
                    // Неизвестный символ
                    return new Token { Type = TokenType.ERROR, Value = $"Неизвестный символ '{currentChar}'", Position = position++ };
            }
        }

        private void SkipWhitespace()
        {
            while (position < input.Length && char.IsWhiteSpace(input[position]))
                position++;
        }
    }

    public class IdentifierInfo
    {
        public string Name { get; set; }
        public string Type { get; set; } // Тип (STRING, WORD, REAL, INTEGER, CHAR)
        public string Value { get; set; }
    }

    public abstract class ParseResult { }

    public class ParseSuccess : ParseResult
    {
        public string Message { get; set; }
        public ParseSuccess(string message)
        {
            Message = message;
        }
    }

    public class ParseError : ParseResult
    {
        public string Message { get; set; }
        public ParseError(string message)
        {
            Message = message;
        }
    }

    public class Parser
    {
        private Lexer lexer;
        private Token currentToken;
        private string input;
        private List<IdentifierInfo> _identifiers = new List<IdentifierInfo>();
        private HashSet<string> declaredIdentifiers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> reservedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "CONST", "STRING", "WORD", "REAL", "INTEGER", "CHAR"
        };

        public Parser(string input)
        {
            this.input = input;
            lexer = new Lexer(input);
            currentToken = lexer.GetNextToken();
        }

        // Начать разбор
        public ParseResult Parse()
        {
            try
            {
                ParseConstDeclaration();
                Console.WriteLine("Анализ успешно завершен.");
                return new ParseSuccess(GetTable());
            }
            catch (Exception ex)
            {
                return new ParseError(GetError(ex.Message, currentToken.Position));
            }
        }

        private void ParseConstDeclaration()
        {
            Expect(TokenType.RESERVED_WORD, "Ожидалось 'CONST'");
            ParseDescriptionList();
            Expect(TokenType.SEMICOLON, "Ожидалась ';'");
        }

        private void ParseDescriptionList()
        {
            Consume(TokenType.RESERVED_WORD); // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ParseDescription();
            while (currentToken.Type == TokenType.COMMA)
            {
                Consume(TokenType.COMMA);
                ParseDescription();
            }
        }

        private void ParseDescription()
        {
            IdentifierInfo identifier = new IdentifierInfo();

            if (currentToken.Type == TokenType.IDENTIFIER)
            {
                string name = currentToken.Value;
                int position = currentToken.Position;
                // Проверка длины идентификатора и зарезервированных слов
                if (name.Length > 8)
                    throw new Exception($"Идентификатор '{name}' превышает допустимую длину (8 символов)");
                if (reservedWords.Contains(name))
                    throw new Exception($"Идентификатор '{name}' не может быть зарезервированным словом");
                if (declaredIdentifiers.Contains(name))
                    throw new Exception($"Идентификатор '{name}' уже объявлен");

                identifier.Name = name;
                declaredIdentifiers.Add(name);
                Consume(TokenType.IDENTIFIER);

                // Проверка на наличие типа
                if (currentToken.Type == TokenType.COLON)
                {
                    Consume(TokenType.COLON);
                    if (currentToken.Type == TokenType.RESERVED_WORD && IsType(currentToken.Value))
                    {
                        identifier.Type = currentToken.Value.ToUpper();
                        Consume(TokenType.RESERVED_WORD);
                    }
                    else
                    {
                        throw new Exception("Ожидалось указание типа после ':'");
                    }
                }

                // Ожидается '='
                Expect(TokenType.EQUAL, "Ожидалось '='");
                Consume(TokenType.EQUAL);

                // Разбор значения
                ParseValue(identifier);
                _identifiers.Add(identifier);
            }
            else
            {
                throw new Exception("Ожидался идентификатор");
            }
        }

        private void ParseValue(IdentifierInfo identifier)
        {
            switch (currentToken.Type)
            {
                case TokenType.STRING_LITERAL:
                    {
                        string value = currentToken.Value;
                        Consume(TokenType.STRING_LITERAL);
                        if (string.IsNullOrEmpty(identifier.Type))
                        {
                            // Если тип не указан, предполагаем STRING
                            identifier.Type = "STRING";
                        }
                        if (identifier.Type == "STRING")
                        {
                            if (value.Length > 256)
                                throw new Exception($"Строковая константа превышает допустимую длину (256 символов)");
                        }
                        else if (identifier.Type == "CHAR")
                        {
                            if (value.Length != 1)
                                throw new Exception($"CHAR константа должна содержать один символ");
                        }
                        else
                        {
                            throw new Exception($"Тип '{identifier.Type}' несовместим со строковым значением");
                        }
                        identifier.Value = $"'{value}'";
                        break;
                    }
                case TokenType.INTEGER_LITERAL:
                case TokenType.REAL_LITERAL:
                    {
                        string value = currentToken.Value;
                        bool isReal = currentToken.Type == TokenType.REAL_LITERAL;
                        Consume(currentToken.Type);
                        if (string.IsNullOrEmpty(identifier.Type))
                        {
                            // Если тип не указан, предполагаем INTEGER или REAL
                            identifier.Type = isReal ? "REAL" : "INTEGER";
                        }
                        if (identifier.Type == "INTEGER")
                        {
                            if (isReal)
                                throw new Exception("Ожидалось целое число для типа INTEGER");
                            if (!int.TryParse(value, out int intValue) || intValue < -32768 || intValue > 32767)
                                throw new Exception("Значение INTEGER выходит за допустимый диапазон (-32768 до 32767)");
                        }
                        else if (identifier.Type == "WORD")
                        {
                            if (isReal)
                                throw new Exception("Ожидалось целое число для типа WORD");
                            if (!int.TryParse(value, out int intValue) || intValue < 0 || intValue > 256)
                                throw new Exception("Значение WORD выходит за допустимый диапазон (0 до 256)");
                        }
                        else if (identifier.Type == "REAL")
                        {
                            if (!double.TryParse(value.Replace('.', ','), out _))
                                throw new Exception("Некорректное вещественное число");
                        }
                        else
                        {
                            throw new Exception($"Тип '{identifier.Type}' несовместим с числовым значением");
                        }
                        identifier.Value = value;
                        break;
                    }
                default:
                    throw new Exception("Ожидалось значение константы");
            }
        }

        // Проверка, является ли строка допустимым типом
        private bool IsType(string lexeme)
        {
            return lexeme.Equals("STRING", StringComparison.OrdinalIgnoreCase) ||
                   lexeme.Equals("WORD", StringComparison.OrdinalIgnoreCase) ||
                   lexeme.Equals("REAL", StringComparison.OrdinalIgnoreCase) ||
                   lexeme.Equals("INTEGER", StringComparison.OrdinalIgnoreCase) ||
                   lexeme.Equals("CHAR", StringComparison.OrdinalIgnoreCase);
        }

        // Ожидание определенного типа токена
        private void Expect(TokenType type, string errorMessage)
        {
            if (currentToken.Type != type)
                throw new Exception(errorMessage);
        }

        // Потребление токена
        private void Consume(TokenType type)
        {
            if (currentToken.Type == type)
            {
                currentToken = lexer.GetNextToken();
                // Пропускаем пробелы и учитываем регистр
                while (currentToken.Type == TokenType.RESERVED_WORD)
                {
                    if (currentToken.Value.Equals(" ", StringComparison.OrdinalIgnoreCase))
                    {
                        currentToken = lexer.GetNextToken();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new Exception($"Ожидался '{type}', найдено '{currentToken.Type}'");
            }
        }

        // Сообщение об ошибке с указанием позиции
        private string GetError(string message, int position)
        {
            string result = $"Ошибка: {message}\r\n";
            result += input + "\r\n";
            result += new string(' ', position) + "^";

            return result;
        }

        // Вывод таблиц идентификаторов и констант
        private string GetTable()
        {
            string result = "Таблица идентификаторов и констант:\r\n";
            result += "Имя\tТип\tЗначение\r\n";
            foreach (var id in _identifiers)
            {
                result += $"{id.Name}\t{id.Type}\t{id.Value}\r\n";
            }

            return result;
        }
    }
}