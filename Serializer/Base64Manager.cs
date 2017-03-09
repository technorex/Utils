namespace TechnoRex.Utils.Serializer
{
    public class Base64Manager
    {
        public static string EncodeBase64(string text, System.Text.Encoding encoding)
        {
            if (text == null)
            {
                return null;
            }

            byte[] textAsBytes = encoding.GetBytes(text);
            return System.Convert.ToBase64String(textAsBytes);
        }

        public static string DecodeBase64(string encodedText, System.Text.Encoding encoding)
        {
            if (encodedText == null)
            {
                return null;
            }

            byte[] textAsBytes = System.Convert.FromBase64String(encodedText);
            return encoding.GetString(textAsBytes);
        }
    }
}