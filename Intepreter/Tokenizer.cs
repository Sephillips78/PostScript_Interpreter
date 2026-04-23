using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class Tokenizer
    {
        public List<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            int i = 0;

            while (i < input.Length)
            {
                char c = input[i];

                // -------------------------
                // Skip whitespace
                // -------------------------
                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                // -------------------------
                // MULTI-CHAR OPERATORS (=, ==)
                // -------------------------
                if (c == '=')
                {
                    if (i + 1 < input.Length && input[i + 1] == '=')
                    {
                        tokens.Add(new Token
                        {
                            Type = Token.TokenType.Operator,
                            Value = "=="
                        });
                        i += 2;
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token
                        {
                            Type = Token.TokenType.Operator,
                            Value = "="
                        });
                        i++;
                        continue;
                    }
                }

                // -------------------------
                // STRING LITERALS: (hello)
                // -------------------------
                if (c == '(')
                {
                    i++; // skip '('
                    var value = "";

                    while (i < input.Length && input[i] != ')')
                    {
                        value += input[i];
                        i++;
                    }

                    if (i >= input.Length)
                        throw new Exception("Unterminated string literal");

                    i++; // skip ')'

                    tokens.Add(new Token
                    {
                        Type = Token.TokenType.String,
                        Value = value
                    });

                    continue;
                }

                // -------------------------
                // PROCEDURES: { ... }
                // -------------------------
                if (c == '{')
                {
                    i++; // skip '{'
                    var procTokens = new List<Token>();
                    int depth = 1;

                    var buffer = "";

                    while (i < input.Length && depth > 0)
                    {
                        char pc = input[i];

                        if (pc == '{')
                        {
                            depth++;
                            buffer += pc;
                        }
                        else if (pc == '}')
                        {
                            depth--;

                            if (depth == 0)
                            {
                                if (!string.IsNullOrWhiteSpace(buffer))
                                {
                                    procTokens.AddRange(Tokenize(buffer));
                                }

                                buffer = "";
                                break;
                            }
                            else
                            {
                                buffer += pc;
                            }
                        }
                        else
                        {
                            buffer += pc;
                        }

                        i++;
                    }

                    tokens.Add(new Token
                    {
                        Type = Token.TokenType.Procedure,
                        Value = procTokens
                    });

                    i++; // skip final '}'
                    continue;
                }

                // -------------------------
                // NORMAL TOKENS
                // -------------------------
                if (char.IsLetterOrDigit(c) || c == '/' || c == '-')
                {
                    var value = "";

                    while (i < input.Length &&
                           !char.IsWhiteSpace(input[i]) &&
                           input[i] != '(' &&
                           input[i] != ')' &&
                           input[i] != '{' &&
                           input[i] != '}' &&
                           input[i] != '=') // <-- IMPORTANT: stop at '='
                    {
                        value += input[i];
                        i++;
                    }

                    tokens.Add(ClassifyToken(value));
                    continue;
                }

                throw new Exception($"Unexpected character: {c}");
            }

            return tokens;
        }

        // -------------------------
        // Token classification
        // -------------------------
        private Token ClassifyToken(string value)
        {
            if (value == "true")
            {
                return new Token { Type = Token.TokenType.Boolean, Value = true };
            }

            if (value == "false")
            {
                return new Token { Type = Token.TokenType.Boolean, Value = false };
            }

            if (double.TryParse(value, out var num))
            {
                return new Token { Type = Token.TokenType.Number, Value = num };
            }

            if (value.StartsWith("/"))
            {
                return new Token
                {
                    Type = Token.TokenType.Name,
                    Value = value.Substring(1)
                };
            }

            return new Token
            {
                Type = Token.TokenType.Operator,
                Value = value
            };
        }
    }
}