using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.NeuralVision.GoogleOCR
{
    public static class LangConverter
    {
        private static Dictionary<char, string> ConvertedLetters = new Dictionary<char, string>
    {
        {'а', "a"},
        {'б', "b"},
        {'в', "v"},
        {'г', "g"},
        {'д', "d"},
        {'е', "e"},
        {'ё', "yo"},
        {'ж', "zh"},
        {'з', "z"},
        {'и', "i"},
        {'й', "j"},
        {'к', "k"},
        {'л', "l"},
        {'м', "m"},
        {'п', "n"},
        {'н', "n"},
        {'о', "o"},
        {'р', "p"},
        {'с', "c"},
        {'т', "t"},
        {'у', "y"},
        {'ф', "f"},
        {'х', "x"},
        {'ц', "c"},
        {'ч', "4"},
        {'ш', "sh"},
        {'щ', "ch"},
        {'э', "e"},
        {'ю', "n"},
        {'я', "r"},
        {'А', "A"},
        {'Б', "B"},
        {'В', "B"},
        {'Г', "G"},
        {'Д', "D"},
        {'Е', "E"},
        {'Ё', "Yo"},
        {'Ж', "Zh"},
        {'З', "Z"},
        {'И', "I"},
        {'Й', "J"},
        {'К', "K"},
        {'Л', "L"},
        {'М', "M"},
        {'Н', "N"},
        {'О', "O"},
        {'Р', "P"},
        {'П', "P"},
        {'С', "С"},
        {'Т', "T"},
        {'У', "y"},
        {'Ф', "F"},
        {'Х', "X"},
        {'Ц', "C"},
        {'Ч', "Ch"},
        {'Ш', "Sh"},
        {'Щ', "Sch"},
        {'Ъ', "b"},
        {'Ы', "bi"},
        {'Ь', "b"},
        {'Э', "E"},
        {'Ю', "Yu"},

        {'0', "0"},
        {'1', "1"},
        {'2', "2"},
        {'3', "3"},
        {'8', "8"},


        {'!', "!"},
        {'@', "@"},
        {'#', "#"},
        {'$', "$"},
        {'%', "%"},
        {'^', "^"},
        {'*', "*"},
        {'(', "("},
        {')', ")"},
        {'-', "-"},
        {'_', "_"},
        {'=', "="},
        {'+', "+"},
        {'{', "{"},
        {'}', "}"},
        {'[', "["},
        {']', "]"},
        {'|', "|"},
        {' ', " "},
        {'/', "/"},
        {'\\', "\\"},      
        {'.', "."},
        {'?', "?"},    
        {';', ";"},
        {':', ":"},
        {'\'', "\'"},
        {'\"', "\""},
        {',', ","},
        {'<', "<"},
        {'>', ">"},

        {'V', "V"},
        {'ъ', "b"},
        {'ы', "bi"},
        {'ь', "b"},
        {'Я', "Ya"},
        {'4', "h"},
        {'5', "s"},
        {'6', "q"},
        {'7', "r"},
        {'R', "R"},
    };

        public static string ConvertToLatin(string source)
        {
            var result = new StringBuilder();
            foreach (var letter in source)
            {
                try
                {
                    result.Append(GetValue(ConvertedLetters, letter));
                }
                catch
                {
                    result.Append(letter);
                }
            }
            return result.ToString();
        }

        public static string ConvertToCyrillic(string source)
        {
            var result = new StringBuilder();
            foreach (var letter in source)
            {
                try
                {
                    var c = GetKey(ConvertedLetters, letter.ToString());
                    if (c != '\0')
                        result.Append(c);
                    else
                        result.Append(letter);
                }
                catch
                {
                    result.Append(letter);
                }
            }
            return result.ToString();
        }
        private static string GetValue(IReadOnlyDictionary<char, string> dictValues, char keyValue)
        {
            return dictValues.ContainsKey(keyValue) ? dictValues[keyValue] : keyValue.ToString();
        }

        private static char GetKey(Dictionary<char, string> dictValues, string value)
        {
            return dictValues.ContainsValue(value) ?
                dictValues.Where(x => x.Value[0].ToString() == value).FirstOrDefault().Key : value[0];
        }
    }
}
