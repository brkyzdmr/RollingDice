using System.Text;

namespace Brkyzdmr.Helpers
{
    public static class StringHelpers
    {
        public static string SplitPascalCase(this string input)
        {
            StringBuilder stringBuilder = new StringBuilder(input.Length);
            stringBuilder.Append(char.ToUpper(input[0]));

            for (int i = 1; i < input.Length; i++)
            {
                char c = input[i];
                if (char.IsUpper(c) && !char.IsUpper(input[i - 1]))
                {
                    stringBuilder.Append(' ');
                }
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }
    }
}