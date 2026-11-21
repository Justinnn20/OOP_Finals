using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace UMakCoop.Services
{
    /// <summary>A JSON object that remembers the order its keys were added in, so the saved file reads naturally.</summary>
    internal class JsonObject
    {
        private readonly List<string> _keys = new List<string>();
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public IList<string> Keys { get { return _keys; } }

        public JsonObject Add(string key, object value)
        {
            if (!_values.ContainsKey(key)) _keys.Add(key);
            _values[key] = value;
            return this;
        }

        public object this[string key]
        {
            get
            {
                object value;
                if (!_values.TryGetValue(key, out value)) throw new FormatException("Missing field \"" + key + "\".");
                return value;
            }
        }

        public string Str(string key) { return (string)this[key]; }
        public decimal Dec(string key) { return (decimal)this[key]; }
        public int Int(string key) { return (int)(decimal)this[key]; }
        public bool Bool(string key) { return (bool)this[key]; }
        public List<object> List(string key) { return (List<object>)this[key]; }
    }

    /// <summary>
    /// A tiny JSON writer and reader (no extra references needed, works on .NET Framework 4.8 and .NET 6/8).
    /// Values are: JsonObject, List&lt;object&gt;, string, decimal (all numbers), bool, null.
    /// </summary>
    internal static class JsonLite
    {
        // ---------------------------------------------------------------- write

        public static string Write(object value)
        {
            var sb = new StringBuilder();
            WriteValue(sb, value, 0);
            return sb.ToString();
        }

        private static void WriteValue(StringBuilder sb, object value, int depth)
        {
            if (value == null) { sb.Append("null"); return; }

            var text = value as string;
            if (text != null) { WriteString(sb, text); return; }

            if (value is bool) { sb.Append((bool)value ? "true" : "false"); return; }

            if (value is int) { sb.Append(((int)value).ToString(CultureInfo.InvariantCulture)); return; }
            if (value is decimal) { sb.Append(((decimal)value).ToString(CultureInfo.InvariantCulture)); return; }

            var obj = value as JsonObject;
            if (obj != null)
            {
                sb.Append("{");
                for (int i = 0; i < obj.Keys.Count; i++)
                {
                    sb.Append(i == 0 ? "\n" : ",\n").Append(' ', (depth + 1) * 2);
                    WriteString(sb, obj.Keys[i]);
                    sb.Append(": ");
                    WriteValue(sb, obj[obj.Keys[i]], depth + 1);
                }
                sb.Append("\n").Append(' ', depth * 2).Append("}");
                return;
            }

            var list = value as List<object>;
            if (list != null)
            {
                if (list.Count == 0) { sb.Append("[]"); return; }
                sb.Append("[");
                for (int i = 0; i < list.Count; i++)
                {
                    sb.Append(i == 0 ? "\n" : ",\n").Append(' ', (depth + 1) * 2);
                    WriteValue(sb, list[i], depth + 1);
                }
                sb.Append("\n").Append(' ', depth * 2).Append("]");
                return;
            }

            throw new ArgumentException("Cannot write " + value.GetType().Name + " as JSON.");
        }

        private static void WriteString(StringBuilder sb, string s)
        {
            sb.Append('"');
            foreach (char c in s)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < ' ') sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
        }

        // ---------------------------------------------------------------- read

        public static object Parse(string json)
        {
            int pos = 0;
            object value = ReadValue(json, ref pos);
            SkipSpace(json, ref pos);
            if (pos != json.Length) throw new FormatException("Unexpected text after the JSON data at position " + pos + ".");
            return value;
        }

        private static void SkipSpace(string s, ref int pos)
        {
            while (pos < s.Length && char.IsWhiteSpace(s[pos])) pos++;
        }

        private static object ReadValue(string s, ref int pos)
        {
            SkipSpace(s, ref pos);
            if (pos >= s.Length) throw new FormatException("The JSON data ended too early.");

            char c = s[pos];
            if (c == '{') return ReadObject(s, ref pos);
            if (c == '[') return ReadArray(s, ref pos);
            if (c == '"') return ReadString(s, ref pos);
            if (Match(s, ref pos, "true")) return true;
            if (Match(s, ref pos, "false")) return false;
            if (Match(s, ref pos, "null")) return null;
            return ReadNumber(s, ref pos);
        }

        private static bool Match(string s, ref int pos, string word)
        {
            if (string.CompareOrdinal(s, pos, word, 0, word.Length) != 0) return false;
            pos += word.Length;
            return true;
        }

        private static JsonObject ReadObject(string s, ref int pos)
        {
            var obj = new JsonObject();
            pos++;   // {
            SkipSpace(s, ref pos);
            if (pos < s.Length && s[pos] == '}') { pos++; return obj; }

            while (true)
            {
                SkipSpace(s, ref pos);
                if (pos >= s.Length || s[pos] != '"') throw new FormatException("Expected a field name at position " + pos + ".");
                string key = ReadString(s, ref pos);
                SkipSpace(s, ref pos);
                if (pos >= s.Length || s[pos] != ':') throw new FormatException("Expected ':' at position " + pos + ".");
                pos++;
                obj.Add(key, ReadValue(s, ref pos));
                SkipSpace(s, ref pos);
                if (pos < s.Length && s[pos] == ',') { pos++; continue; }
                if (pos < s.Length && s[pos] == '}') { pos++; return obj; }
                throw new FormatException("Expected ',' or '}' at position " + pos + ".");
            }
        }

        private static List<object> ReadArray(string s, ref int pos)
        {
            var list = new List<object>();
            pos++;   // [
            SkipSpace(s, ref pos);
            if (pos < s.Length && s[pos] == ']') { pos++; return list; }

            while (true)
            {
                list.Add(ReadValue(s, ref pos));
                SkipSpace(s, ref pos);
                if (pos < s.Length && s[pos] == ',') { pos++; continue; }
                if (pos < s.Length && s[pos] == ']') { pos++; return list; }
                throw new FormatException("Expected ',' or ']' at position " + pos + ".");
            }
        }

        private static string ReadString(string s, ref int pos)
        {
            var sb = new StringBuilder();
            pos++;   // opening quote
            while (pos < s.Length)
            {
                char c = s[pos++];
                if (c == '"') return sb.ToString();
                if (c != '\\') { sb.Append(c); continue; }

                if (pos >= s.Length) break;
                char e = s[pos++];
                switch (e)
                {
                    case '"': sb.Append('"'); break;
                    case '\\': sb.Append('\\'); break;
                    case '/': sb.Append('/'); break;
                    case 'b': sb.Append('\b'); break;
                    case 'f': sb.Append('\f'); break;
                    case 'n': sb.Append('\n'); break;
                    case 'r': sb.Append('\r'); break;
                    case 't': sb.Append('\t'); break;
                    case 'u':
                        if (pos + 4 > s.Length) throw new FormatException("Bad \\u escape.");
                        sb.Append((char)Convert.ToInt32(s.Substring(pos, 4), 16));
                        pos += 4;
                        break;
                    default: throw new FormatException("Bad escape \\" + e + ".");
                }
            }
            throw new FormatException("A text value was never closed.");
        }

        private static decimal ReadNumber(string s, ref int pos)
        {
            int start = pos;
            while (pos < s.Length && "+-0123456789.eE".IndexOf(s[pos]) >= 0) pos++;
            decimal value;
            if (pos == start || !decimal.TryParse(s.Substring(start, pos - start),
                    NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                throw new FormatException("Unexpected character at position " + start + ".");
            return value;
        }
    }
}
