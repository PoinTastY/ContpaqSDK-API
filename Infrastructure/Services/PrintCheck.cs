using Domain.SDK_Comercial;
using ESCPOS_NET.Emitters;
using System.IO.Ports;
using System.Text;
using Document = Domain.Entities.Document;

namespace Infrastructure.Services
{
    public static class PrintCheck
    {
        public static string payee = "COOPERATIVA DE CONSUMO AGROPECUARIO";

        /// <summary>
        /// prints the information of the document in the specified template to the serial printer
        /// </summary>
        /// <param name="document"></param>
        /// <param name="template"></param>
        /// <param name="sDKSettings"></param>
        /// <param name="amountinwords"></param>
        /// <returns>true if succeed, but no false, instead, it throws exception</returns>
        public static bool Print(Document document, string template, SDKSettings sDKSettings, string amountinwords)
        {
            byte[] templateCommands = [];
            try
            {
                SendCommands(document, sDKSettings.SerialPort, template, amountinwords);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Templates
        private static void GetDefaultCommands(Document document, SerialPort serialPort, string amountinwords)//pending
        {
            try
            {
                string amount = $"                                         {document.CTOTAL.ToString("#,##0.00")}";

                string amountInWords = amountinwords;
                amountInWords = ValidateCharsLen(amountInWords, 50);

                string date = $"                      {document.CFECHA:dd-MM-yyyy}";

                var e = new EPSON();

                serialPort.Write(e.SetLineSpacingInDots(10), 0, e.SetLineSpacingInDots(10).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(date), 0, e.PrintLine(date).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(payee), 0, e.PrintLine(payee).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLinesReverse(1), 0, e.FeedLinesReverse(1).Length);
                Thread.Sleep(100);


                serialPort.Write(e.PrintLine(amount), 0, e.PrintLine(amount).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                var wordsbytes = Encoding.ASCII.GetBytes(amountInWords);

                serialPort.Write(wordsbytes, 0, wordsbytes.Length);
                Thread.Sleep(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetCitiBanamexCommands(Document document, SerialPort serialPort, string amountinwords)//DONE
        {
            try
            {
                var payeeBytes = Encoding.ASCII.GetBytes(payee);
                string amount = $"         {document.CTOTAL.ToString("#,##0.00")}";

                string amountInWords = amountinwords;
                amountInWords = ValidateCharsLen(amountInWords, 47);

                string date = $"                                        {document.CFECHA:dd-MM-yyyy}";

                var e = new EPSON();

                serialPort.Write(e.SetLineSpacingInDots(11), 0, e.SetLineSpacingInDots(11).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(date), 0, e.PrintLine(date).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(2), 0, e.FeedLines(2).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(payee + amount), 0, e.PrintLine(payee + amount).Length);
                Thread.Sleep(100);

                //serialPort.Write(e.FeedLinesReverse(1), 0, e.FeedLinesReverse(1).Length);
                //Thread.Sleep(100);

                //serialPort.Write(e.PrintLine(amount), 0, e.PrintLine(amount).Length);
                //Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                var wordsbytes = Encoding.ASCII.GetBytes(amountInWords);

                serialPort.Write(wordsbytes, 0, wordsbytes.Length);
                Thread.Sleep(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetBanBajioCommands(Document document, SerialPort serialPort, string amountinwords)//DONE
        {
            try
            {
                string amount = $"                                         {document.CTOTAL.ToString("#,##0.00")}";

                string amountInWords = amountinwords;
                amountInWords = ValidateCharsLen(amountInWords, 42);

                string date = $"                      {document.CFECHA:dd-MM-yyyy}";

                var e = new EPSON();

                serialPort.Write(e.SetLineSpacingInDots(10), 0, e.SetLineSpacingInDots(10).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(date), 0, e.PrintLine(date).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(payee), 0, e.PrintLine(payee).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLinesReverse(1), 0, e.FeedLinesReverse(1).Length);
                Thread.Sleep(100);


                serialPort.Write(e.PrintLine(amount), 0, e.PrintLine(amount).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                var wordsbytes = Encoding.ASCII.GetBytes(amountInWords);

                serialPort.Write(wordsbytes, 0, wordsbytes.Length);
                Thread.Sleep(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetBBVACommands(Document document, SerialPort serialPort, string amountinwords)//DONE
        {
            try
            {
                var payeeBytes = Encoding.ASCII.GetBytes(payee);
                string amount = $"                                          {document.CTOTAL.ToString("#,##0.00")}";

                string amountInWords = amountinwords;
                amountInWords = ValidateCharsLen(amountInWords, 47);

                string date = $"                      {document.CFECHA:dd-MM-yyyy}";

                var e = new EPSON();

                serialPort.Write(e.SetLineSpacingInDots(11), 0, e.SetLineSpacingInDots(11).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(2), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(date), 0, e.PrintLine(date).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(payee), 0, e.PrintLine(payee).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLinesReverse(1), 0, e.FeedLinesReverse(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(amount), 0, e.PrintLine(amount).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                var wordsbytes = Encoding.ASCII.GetBytes(amountInWords);

                serialPort.Write(wordsbytes, 0, wordsbytes.Length);
                Thread.Sleep(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetHSBCCommands(Document document, SerialPort serialPort, string amountinwords)//DONE
        {
            try
            {
                var payeeBytes = Encoding.ASCII.GetBytes(payee);
                string amount = $"                                          {document.CTOTAL.ToString("#,##0.00")}";

                string amountInWords = amountinwords;
                amountInWords = ValidateCharsLen(amountInWords, 50);

                string date = $"                                       {document.CFECHA:dd-MM-yyyy}";

                var e = new EPSON();

                serialPort.Write(e.SetLineSpacingInDots(11), 0, e.SetLineSpacingInDots(11).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(date), 0, e.PrintLine(date).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(2), 0, e.FeedLines(2).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(payee), 0, e.PrintLine(payee).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLinesReverse(1), 0, e.FeedLinesReverse(1).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(amount), 0, e.PrintLine(amount).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                var wordsbytes = Encoding.ASCII.GetBytes(amountInWords);

                serialPort.Write(wordsbytes, 0, wordsbytes.Length);
                Thread.Sleep(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetBanorteCommands(Document document, SerialPort serialPort, string amountinwords)//done
        {
            try
            {
                string amount = $"          {document.CTOTAL.ToString("#,##0.00")}";

                string amountInWords = amountinwords;
                amountInWords = ValidateCharsLen(amountInWords, 50);

                string date = $"                   {document.CFECHA:dd-MM-yyyy}";

                var e = new EPSON();

                serialPort.Write(e.SetLineSpacingInDots(13), 0, e.SetLineSpacingInDots(11).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(3), 0, e.FeedLines(2).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(date + amount), 0, e.PrintLine(date + amount).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(2).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(payee), 0, e.PrintLine(payee).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                var wordsbytes = Encoding.ASCII.GetBytes(amountInWords);

                serialPort.Write(wordsbytes, 0, wordsbytes.Length);
                Thread.Sleep(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetSantanderCommands(Document document, SerialPort serialPort, string amountinwords)//done
        {
            try
            {
                string amount = $"     {document.CTOTAL.ToString("#,##0.00")}";

                string amountInWords = amountinwords;
                amountInWords = ValidateCharsLen(amountInWords, 44);

                string date = $"                          {document.CFECHA:dd-MM-yyyy}";

                var e = new EPSON();

                serialPort.Write(e.SetLineSpacingInDots(12), 0, e.SetLineSpacingInDots(11).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(2), 0, e.FeedLines(2).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(date), 0, e.PrintLine(date).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(2).Length);
                Thread.Sleep(100);

                serialPort.Write(e.PrintLine(payee + amount), 0, e.PrintLine(payee + amount).Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                var wordsbytes = Encoding.ASCII.GetBytes(amountInWords);

                serialPort.Write(wordsbytes, 0, wordsbytes.Length);
                Thread.Sleep(100);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Sends the ESC/POS comands, using the referenced printing template
        /// </summary>
        /// <param name="document"></param>
        /// <param name="port"></param>
        /// <param name="template"></param>
        /// <exception cref="Exception"></exception>
        private static void SendCommands(Document document, string port, string template, string amountinwords)
        {
            SerialPort serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)
            {
                Handshake = Handshake.RequestToSend
            };

            try
            {
                serialPort.Open();

                byte[] selectPageMode = [0x1B, 0x4C];
                byte[] setPrintDirection = [0x1B, 0x54, 0x03];
                byte[] formFeed = [0x0C];
                byte[] releasePaper = [0x1B, 0x71];

                var e = new EPSON();

                serialPort.Write(e.Initialize(), 0, e.Initialize().Length);
                Thread.Sleep(100);

                serialPort.Write(selectPageMode, 0, selectPageMode.Length);
                Thread.Sleep(100);

                serialPort.Write(setPrintDirection, 0, setPrintDirection.Length);
                Thread.Sleep(100);

                serialPort.Write(e.FeedLines(2), 0, e.FeedLines(2).Length);
                Thread.Sleep(100);

                if (template == "banamex")
                    GetCitiBanamexCommands(document, serialPort, amountinwords);
                else if (template == "banbajio")
                    GetBanBajioCommands(document, serialPort, amountinwords);
                else if (template == "bbva")
                    GetBBVACommands(document, serialPort, amountinwords);
                else if (template == "hsbc")
                    GetHSBCCommands(document, serialPort, amountinwords);
                else if (template == "banorte")
                    GetBanorteCommands(document, serialPort, amountinwords);
                else if (template == "santander")
                    GetSantanderCommands(document, serialPort, amountinwords);
                else if (template == "default")
                    GetDefaultCommands(document, serialPort, amountinwords);
                else
                    GetDefaultCommands(document, serialPort, amountinwords);// Default template lul

                serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
                Thread.Sleep(100);

                //TODO: maybe try to make it stop printing when text or paper is done

                serialPort.Write(releasePaper, 0, releasePaper.Length);
                Thread.Sleep(100);

                serialPort.Write(formFeed, 0, formFeed.Length);
                Thread.Sleep(100);

                serialPort.Close();
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception($"No se encontro el Puerto {port};  (" + ex.Message + ") NO SE ACTUALIZARA ESTADO A IMPRESO");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " NO SE ACTUALIZARA ESTADO A IMPRESO");
            }
            finally
            {
                if (serialPort.IsOpen)
                    serialPort.Close();
            }
        }

        /// <summary>
        /// function to validate the horizontal size of the string, if it exeeds the provided len, jumps a line
        /// </summary>
        /// <param name="text"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private static string ValidateCharsLen(string text, int len)
        {
            if (text.Length <= len)
            {
                return text;
            }

            // Buscar el último espacio antes o en la posición len
            int spaceIndex = text.LastIndexOf(' ', len);

            // Si no hay espacio, buscar el siguiente espacio después de len
            if (spaceIndex == -1)
            {
                spaceIndex = text.IndexOf(' ', len);
            }

            // Si aún no hay espacio, cortar en len
            if (spaceIndex == -1)
            {
                spaceIndex = len;
            }

            // Dividir el texto en dos partes
            string firstPart = text.Substring(0, spaceIndex);
            string secondPart = text.Substring(spaceIndex).TrimStart(); // Eliminar cualquier espacio al inicio de la segunda parte

            return firstPart + "\n" + ValidateCharsLen(secondPart, len);
        }
    }
}
