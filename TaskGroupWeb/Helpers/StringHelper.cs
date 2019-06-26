using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskGroupWeb.Helpers
{
    public static class StringHelper
    {
        #region - Dash -

        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }

        #endregion

        #region - CryptoUrl -

        private const string actionKey = "32CC7ED0A4CEC0D7C7B6B68D81661C417279C15D3548DD3B";

        private const string actionIv = "B93FE8EAFAA9821D";

        public static TripleDESCryptoServiceProvider Des3
        {
            get { return new TripleDESCryptoServiceProvider { Mode = CipherMode.CBC }; }
        }

        public static string EncryptUrl(this object value)
        {
            if (value == null)
                return "";

            if (String.IsNullOrEmpty(value.ToString()))
                return "";

            string data = value.ToString();
            string key = actionKey;
            string iv = actionIv;

            byte[] bdata = Encoding.UTF8.GetBytes(data);
            byte[] bkey = HexToBytes(key);
            byte[] biv = HexToBytes(iv);

            MemoryStream stream = new MemoryStream();

            CryptoStream encStream = new CryptoStream(stream, Des3.CreateEncryptor(bkey, biv), CryptoStreamMode.Write);

            encStream.Write(bdata, 0, bdata.Length);

            encStream.FlushFinalBlock();

            encStream.Close();

            return BytesToHex(stream.ToArray());
        }

        public static string DecryptUrl(this object value)
        {
            if (value == null)
                return "";

            if (String.IsNullOrEmpty(value.ToString()))
                return "";

            string data = value.ToString();
            string key = actionKey;
            string iv = actionIv;

            byte[] bdata = HexToBytes(data);
            byte[] bkey = HexToBytes(key);
            byte[] biv = HexToBytes(iv);

            MemoryStream stream = new MemoryStream();

            CryptoStream encStream = new CryptoStream(stream, Des3.CreateDecryptor(bkey, biv), CryptoStreamMode.Write);
            encStream.Write(bdata, 0, bdata.Length);
            encStream.FlushFinalBlock();
            encStream.Close();

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        private static byte[] HexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                string code = hex.Substring(i * 2, 2);
                bytes[i] = byte.Parse(code, System.Globalization.NumberStyles.HexNumber);
            }

            return bytes;
        }

        private static string BytesToHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
                hex.AppendFormat("{0:X2}", bytes[i]);

            return hex.ToString();
        }

        #endregion

        #region - Nome Header -

        //public static string TratarNomeHeader(string valor, bool expurgo = false)
        //{
        //    var e2DocSession = UserContext.e2DocSession;

        //    var login = e2DocSession.Login;
        //    var access = e2DocSession.Access;

        //    switch (valor)
        //    {
        //        case "*documento*":
        //            valor = (expurgo) ? "[Documento]" : "Documento";
        //            break;

        //        case "*descricao*":
        //            valor = (expurgo) ? "[Descrição]" : "Descrição";
        //            break;

        //        case "*tamanho*":
        //            valor = (expurgo) ? "[Tamanho (kb)]" : "Tamanho (kb)";
        //            break;

        //        case "*versoes*":
        //            valor = (expurgo) ? "[" + access.ParametroListar("nome.versao").Valor + "]" : access.ParametroListar("nome.versao").Valor;
        //            break;

        //        case "*paginas*":
        //            valor = (expurgo) ? "[Páginas]" : "Páginas";
        //            break;

        //        case "*usuarioExclusao*":
        //            valor = (expurgo) ? "[Usuário Exclusão]" : "Usuário Exclusão";
        //            break;

        //        case "*dataExclusao*":
        //            valor = (expurgo) ? "[Data Exclusão]" : "Data Exclusão";
        //            break;
        //    }

        //    if (valor.ToLower().IndexOf("pst_s_indice") >= 0)
        //    {
        //        int idx = int.Parse(valor.ToLower().Replace("pst_s_indice", "").ToString());

        //        for (int i = 0; i < login.Indices.Length; i++)
        //        {
        //            if (login.Indices[i].ID == idx)
        //            {
        //                valor = login.Indices[i].sLabel;
        //                break;
        //            }
        //        }
        //    }

        //    return valor;
        //}

        #endregion

        #region - Tratar Imagem Extensao -

        public static string TratarImagemExtensao(string arquivo)
        {
            string[] nome = arquivo.ToUpper().Split('.');

            var extensao = nome[nome.Length - 1];

            switch (extensao)
            {
                case "DOC":
                case "DOCX":
                    return "word.gif";

                case "XLS":
                case "XLSX":
                    return "excel.gif";

                case "PPT":
                case "PPTX":
                    return "pp.gif";

                case "PDF":
                    return "docPDF.png";

                case "WAV":
                    return "wav.gif";

                case "MP3":
                case "WMA":
                case "AAC":
                case "OGG":
                case "AC3":
                    return "audio.png";

                case "AVI":
                case "RMVB":
                case "MKV":
                case "MPEG":
                case "MOV":
                case "MP4":
                case "WMV":
                case "SFW":
                case "FLV":
                    return "video.png";

                case "JPG":
                case "JPEG":
                case "GIF":
                case "PNG":
                case "BITMAP":
                case "TIFF":
                case "RAW":
                case "SVG":
                case "WEBP":
                case "EXIF":
                    return "imagem.png";

                case "P7S":
                    return "cert.png";

                default:
                    return "generic.png";
            }
        }

        #endregion

        #region - Data/Hora -

        public static string DataHora
        {
            get
            {
                return DateTime.Now.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToLongTimeString();
            }
        }

        #endregion

        #region - Versão -

        public static string Versao
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        #endregion

        #region - Word -

        //public static void FindAndReplace(Microsoft.Office.Interop.Word.Application doc, object findText, object replaceWithText)
        //{
        //    object matchCase = false;
        //    object matchWholeWord = true;
        //    object matchWildCards = false;
        //    object matchSoundsLike = false;
        //    object matchAllWordForms = false;
        //    object forward = true;
        //    object format = false;
        //    object matchKashida = false;
        //    object matchDiacritics = false;
        //    object matchAlefHamza = false;
        //    object matchControl = false;
        //    object read_only = false;
        //    object visible = true;
        //    object replace = 2;
        //    object wrap = 1;

        //    doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
        //        ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
        //        ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        //}

        #endregion

        #region - LongFormat -

        public static string LongToFormatLong(long valor)
        {
            return string.Format("{0:N0}", valor);
        }

        #endregion

        #region - Validar Expressão Regular -

        public static bool ValidaRegEx(string regex, string texto)
        {
            try
            {
                Regex rg = new Regex(regex);

                return rg.IsMatch(texto);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region - String Valid Binder -

        public static bool CheckStringValidBinder(string valor)
        {
            var valid = true;

            var caracteres = new string[] { "\\", "*", "|", ":", "%", "?", "'" };

            foreach (var item in caracteres)
            {
                if (valor.ToString().Contains(item))
                {
                    valid = false;
                }
            }

            return valid;
        }

        #endregion

        #region - Size Suffix -

        public static string ToFileSize(string kb)
        {
            var valor = double.Parse(kb);

            string[] suffixes = { "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            for (int i = 0; i < suffixes.Length; i++)
            {
                if (valor <= (Math.Pow(1024, i + 1)))
                {
                    return ThreeNonZeroDigits(valor /
                        Math.Pow(1024, i)) +
                        " " + suffixes[i];
                }
            }

            return ThreeNonZeroDigits(valor /
                Math.Pow(1024, suffixes.Length - 1)) +
                " " + suffixes[suffixes.Length - 1];
        }

        private static string ThreeNonZeroDigits(double value)
        {
            if (value >= 100)
            {
                // No digits after the decimal.
                return value.ToString("0,0");
            }
            else if (value >= 10)
            {
                // One digit after the decimal.
                return value.ToString("0.0");
            }
            else
            {
                // Two digits after the decimal.
                return value.ToString("0.00");
            }
        }

        #endregion

        #region - Truncate Long String -

        public static string TruncateLongString(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }

        #endregion

        #region - Meses -

        public static string[] MESES_ABREVIACAO = { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };
        public static string[] MESES = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };

        #endregion

        #region - Remove Acento -

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        #endregion
    }

    #region - SemiNumericComparer -

    public class SemiNumericComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            if (IsNumeric(s1) && IsNumeric(s2))
            {
                if (Convert.ToInt32(s1) > Convert.ToInt32(s2)) return 1;
                if (Convert.ToInt32(s1) < Convert.ToInt32(s2)) return -1;
                if (Convert.ToInt32(s1) == Convert.ToInt32(s2)) return 0;
            }

            if (IsNumeric(s1) && !IsNumeric(s2))
                return -1;

            if (!IsNumeric(s1) && IsNumeric(s2))
                return 1;

            return string.Compare(s1, s2, true);
        }

        public static bool IsNumeric(object value)
        {
            try
            {
                int i = Convert.ToInt32(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    #endregion
}
