using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class DataFormater
    {
        public string DataTableCenter(string? text, int colWidth)
        {
            if (String.IsNullOrWhiteSpace(text))
                return new string(' ',colWidth);

            var padding = colWidth - text.Length;
            var leftPadding = padding / 2 + text.Length;
            var rightPadding = padding + text.Length;

            return text.PadRight(leftPadding).PadLeft(rightPadding);
        }
    }
}
