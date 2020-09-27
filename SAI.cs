using System;
using System.Collections.Generic;

namespace Simple_Assembler_Interpreter
{
    class SAI
    {
        /// <summary>
        /// Доступные комманды ассемблера
        /// </summary>
        readonly static List<string> Commands = new List<string>
        { "mov", "inc", "dec", "jnz" };

        readonly static List<char> Digits = new List<char>
        {'1','2','3','4','5','6','7','8','9','0'};

        /// <summary>
        /// Проверка токена на число
        /// </summary>
        /// <param name="Token"></param>
        /// <returns>Булевое значение. Истина если токен - число, иначе ложь</returns>
        private static bool IsNum(string Token)
        {
            for (int i = Token[0] == '-' ? 1 : 0; i < Token.Length; i++)
            {
                if ((Token[i] == '.' && i == 0) || (Digits.IndexOf(Token[i]) == -1 && Token[i] != '.')) return false;
            }
            return true;
        }

        /// <summary>
        /// Функция инкрементирования виртуального регистра
        /// </summary>
        /// <param name="Tokens"></param>
        /// <param name="Registers"></param>
        private static void Inc(string[] Tokens, ref Dictionary<string, int> Registers)
        {
            if (Tokens.Length == 2 && Registers.ContainsKey(Tokens[1])) Registers[Tokens[1]]++;
        }

        /// <summary>
        /// Функция декрементирования виртуального регистра
        /// </summary>
        /// <param name="Tokens"></param>
        /// <param name="Registers"></param>
        private static void Dec(string[] Tokens, ref Dictionary<string, int> Registers)
        {
            if (Tokens.Length == 2 && Registers.ContainsKey(Tokens[1])) Registers[Tokens[1]]--;
        }

        /// <summary>
        /// Функция реализующая команду помещения значения в виртуальный регистр
        /// </summary>
        /// <param name="Tokens"></param>
        /// <param name="Registers"></param>
        private static void Mov(string[] Tokens, ref Dictionary<string, int> Registers)
        {
            if (Tokens.Length == 3 && Registers.ContainsKey(Tokens[1]) && IsNum(Tokens[2]))
            {
                Registers[Tokens[1]] = Convert.ToInt32(Tokens[2]);
            }
            else if (Tokens.Length == 3 && Registers.ContainsKey(Tokens[1]) && Registers.ContainsKey(Tokens[2]))
            {
                Registers[Tokens[1]] = Registers[Tokens[2]];
            }
            else if (Tokens.Length == 3 && !Registers.ContainsKey(Tokens[1]) && IsNum(Tokens[2]))
            {
                Registers.Add(Tokens[1], Convert.ToInt32(Tokens[2]));
            }
            else
            {
                Registers.Add(Tokens[1], Registers[Tokens[2]]);
            }
        }

        /// <summary>
        /// Функция обработки безусловных переходов
        /// </summary>
        /// <param name="Tokens"></param>
        /// <param name="Registers"></param>
        /// <param name="CommandCounter"></param>
        private static void Jnz(string[] Tokens, ref Dictionary<string, int> Registers, ref int CommandCounter)
        {
            if (Tokens.Length == 3 && IsNum(Tokens[1]) && Convert.ToInt32(Tokens[1]) != 0)
            {
                CommandCounter += Convert.ToInt32(Tokens[2]);
            }
            else if (Tokens.Length == 3 && IsNum(Tokens[1]) && Convert.ToInt32(Tokens[1]) == 0)
            {
                CommandCounter++;
            }
            else if (Tokens.Length == 3 && Registers.ContainsKey(Tokens[1]) && IsNum(Tokens[2]) && Registers[Tokens[1]] != 0)
            {
                CommandCounter += Convert.ToInt32(Tokens[2]);
            }
            else
            {
                CommandCounter++;
            }
        }

        /// <summary>
        /// Преобразование строки в массив токенов
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        private static string[] CommandToken(string Command)
        {
            List<string> Tokens = new List<string>();

            string Token = "";
            foreach (char ch in Command)
            {
                if (ch != ' ')
                {
                    Token += ch;
                }
                else
                {
                    Tokens.Add(Token);
                    Token = "";
                }
            }
            Tokens.Add(Token);

            return Tokens.ToArray();
        }

        /// <summary>
        /// Интерпретатор
        /// </summary>
        /// <param name="program"></param>
        /// <returns>Словарь. Ключи - регистры, значения по ключам - значения регистров после выполнения программы</returns>
        public static Dictionary<string, int> Interpreter(string[] program)
        {
            Dictionary<string, int> Registers = new Dictionary<string, int>();

            for (int CommandCounter = 0; CommandCounter < program.Length;)
            {
                string[] Token = CommandToken(program[CommandCounter]);

                switch (Commands.IndexOf(Token[0]))
                {
                    case 0: Mov(Token, ref Registers); CommandCounter++; break;
                    case 1: Inc(Token, ref Registers); CommandCounter++; break;
                    case 2: Dec(Token, ref Registers); CommandCounter++; break;
                    case 3: Jnz(Token, ref Registers, ref CommandCounter); break;
                }
            }
            return Registers;
        }
    }
}
