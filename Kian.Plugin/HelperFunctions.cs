namespace Kian.Core
{
    public class HelperFunctions
    {
        public static string GetValidFilePath(string filePath)
        {
            // Switch to alternate signs
            filePath = filePath.Replace(":", "꞉"); // MODIFIER LETTER COLON (U+A789)
            filePath = filePath.Replace("*", "∗"); // ASTERISK OPERATOR (U+2217)
            filePath = filePath.Replace("?", "﹖"); // SMALL QUESTION MARK (U+FE56)

            foreach (char c in System.IO.Path.GetInvalidPathChars())
                filePath = filePath.Replace(c, '_');

            return filePath;
        }

        // Credit goes to http://stackoverflow.com/a/11124118.
        // Returns the human-readable file size for an arbitrary, 64-bit file size
        // The default format is "0.# XB", e.g. "4.2 KB" or "1.4 GB"
        public static string GetBytesReadable(long i)
        {
            // Get absolute value
            long absolutei = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolutei >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (i >> 50);
            }
            else if (absolutei >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (i >> 40);
            }
            else if (absolutei >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (i >> 30);
            }
            else if (absolutei >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (i >> 20);
            }
            else if (absolutei >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (i >> 10);
            }
            else if (absolutei >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.# ") + suffix;
        }
    }
}