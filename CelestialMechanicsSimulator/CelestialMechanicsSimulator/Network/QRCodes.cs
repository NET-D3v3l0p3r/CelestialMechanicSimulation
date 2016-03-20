using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;

namespace QRCodes
{
    /// <summary>
    /// Class providing methods for generating QR BarCodes.
    /// </summary>
    public static class QRCode
    {
        private static readonly WebClient m_WebClient = new WebClient();

        private const string m_GoogleApiUrl = "http://chart.apis.google.com/chart?cht=qr&chld={2}|{3}&chs={0}x{0}&chl={1}";
        private const int m_GuidCodeSize = 31;

        private static int m_DefaultSize = 80;
        private static int m_DefaultMargin = 0;
        private static int m_DefaultGuidScale = 2;
        private static int m_DefaultManyMargin = 5;
        private static ErrorCorrectionLevel m_DefaultErrorCorrection = ErrorCorrectionLevel.Low;

        /// <summary>
        /// Gets or sets the default margin between QR BarCodes in images containing many of them.
        /// </summary>
        /// <value>
        /// The default margin between QR BarCodes in images containing many of them.
        /// </value>
        public static int DefaultManyMargin
        {
            get { return m_DefaultManyMargin; }
            set { m_DefaultManyMargin = value; }
        }

        /// <summary>
        /// Gets or sets the default GUID scale.
        /// </summary>
        /// <value>
        /// The default GUID scale.
        /// </value>
        public static int DefaultGuidScale
        {
            get { return m_DefaultGuidScale; }
            set { m_DefaultGuidScale = value; }
        }

        /// <summary>
        /// Gets or sets the default size.
        /// </summary>
        /// <value>
        /// The default size.
        /// </value>
        public static int DefaultSize
        {
            get { return m_DefaultSize; }
            set { m_DefaultSize = value; }
        }

        /// <summary>
        /// Gets or sets the default margin.
        /// </summary>
        /// <value>
        /// The default margin.
        /// </value>
        public static int DefaultMargin
        {
            get { return m_DefaultMargin; }
            set { m_DefaultMargin = value; }
        }

        /// <summary>
        /// Gets or sets the default error correction Level.
        /// </summary>
        /// <value>
        /// The default error correction level.
        /// </value>
        public static ErrorCorrectionLevel DefaultErrorCorrection
        {
            get { return m_DefaultErrorCorrection; }
            set { m_DefaultErrorCorrection = value; }
        }

        /// <summary>
        /// Generates an QR BarCode containing the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <returns>The image containing the generated QR BarCode.</returns>
        public static Image Generate(Guid data)
        {
            if (data == null) throw new ArgumentNullException("data");

            return Generate(data, m_DefaultGuidScale);
        }

        /// <summary>
        /// Generates an QR BarCode containing the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <param name="scale">The scale of the QR BarCode.</param>
        /// <returns>The image containing the generated QR BarCode.</returns>
        public static Image Generate(Guid data, int scale)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (scale < 1) throw new ArgumentOutOfRangeException("scale", scale, "Must be greater than zero.");

            return Generate(data.ToString("N"), m_GuidCodeSize * scale, 1, ErrorCorrectionLevel.Medium);
        }

        /// <summary>
        /// Generates an image containing the QR BarCodes containing the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <returns>The image containing the generated QR BarCodes.</returns>
        public static Image GenerateMany(Guid[,] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            return GenerateMany(data, m_DefaultManyMargin, m_DefaultGuidScale);
        }

        /// <summary>
        /// Generates an image containing the QR BarCodes containing the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <param name="manyMargin">The margin between the BarCodes.</param>
        /// <param name="scale">The scale of the QR BarCode.</param>
        /// <returns>The image containing the generated QR BarCodes.</returns>
        public static Image GenerateMany(Guid[,] data, int manyMargin, int scale)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (manyMargin < 0) throw new ArgumentOutOfRangeException("manyMargin", manyMargin, "Must be greater than or equal to zero.");
            if (scale < 1) throw new ArgumentOutOfRangeException("scale", scale, "Must be greater than zero.");

            var countX = data.GetLength(0);
            var countY = data.GetLength(1);
            var img = new Bitmap(m_GuidCodeSize * scale * countX + manyMargin * (countX + 1), m_GuidCodeSize * scale * countY + manyMargin * (countY + 1));

            using (var g = Graphics.FromImage(img))
            {
                g.Clear(Color.White);

                for (int x = 0; x < countX; x++)
                {
                    for (int y = 0; y < countY; y++)
                    {
                        using (var qr = QRCode.Generate(data[x, y], scale))
                        {
                            var loc = new Point(x * qr.Width + manyMargin * x + manyMargin, y * qr.Height + manyMargin * y + manyMargin);

                            g.DrawImage(qr, loc);
                            g.DrawRectangle(Pens.Black, new Rectangle(new Point(loc.X - 1, loc.Y - 1), new Size(qr.Width + 1, qr.Height + 1)));
                        }
                    }
                }
            }

            return img;
        }

        /// <summary>
        /// Generates an QR BarCode containing the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <returns>The image containing the generated QR BarCode.</returns>
        public static Image Generate(string data)
        {
            if (data == null) throw new ArgumentNullException("data");

            return Generate(data, m_DefaultSize, m_DefaultMargin, m_DefaultErrorCorrection);
        }

        /// <summary>
        /// Generates an QR BarCode containing the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <param name="size">Image size.</param>
        /// <param name="margin">The width of the white border around the data portion of the code. This is in rows, not in pixels.</param>
        /// <param name="errorCorrection">QR codes support four levels of error correction to enable recovery of missing, misread, or obscured data. Greater redundancy is achieved at the cost of being able to store less data.</param>
        /// <returns>The image containing the generated QR BarCode.</returns>
        public static Image Generate(string data, int size, int margin, ErrorCorrectionLevel errorCorrection)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (size < 1) throw new ArgumentOutOfRangeException("size", size, "Must be greater than zero.");
            if (margin < 0) throw new ArgumentOutOfRangeException("margin", margin, "Must be greater than or equal to zero.");
            if (!Enum.IsDefined(typeof(ErrorCorrectionLevel), errorCorrection)) throw new InvalidEnumArgumentException("errorCorrectionLevel", (int)errorCorrection, typeof(ErrorCorrectionLevel));

            var link = string.Format(m_GoogleApiUrl, size, HttpUtility.UrlEncode(data), errorCorrection.ToString()[0], margin);

            using (var ms = new MemoryStream(m_WebClient.DownloadData(link)))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Generates an image containing the QR BarCodes containing the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <returns>The image containing the generated QR BarCodes.</returns>
        public static Image GenerateMany(string[,] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            return GenerateMany(data, m_DefaultSize, m_DefaultManyMargin, m_DefaultMargin, m_DefaultErrorCorrection);
        }

        /// <summary>
        /// Generates an image containing the QR BarCodes containing the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <param name="size">Image size.</param>
        /// <param name="manyMargin">The margin between the BarCodes.</param>
        /// <param name="margin">The width of the white border around the data portion of the code. This is in rows, not in pixels.</param>
        /// <param name="errorCorrection">QR codes support four levels of error correction to enable recovery of missing, misread, or obscured data. Greater redundancy is achieved at the cost of being able to store less data.</param>
        /// <returns>The image containing the generated QR BarCodes.</returns>
        public static Image GenerateMany(string[,] data, int size, int manyMargin, int margin, ErrorCorrectionLevel errorCorrection)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (size < 1) throw new ArgumentOutOfRangeException("size", size, "Must be greater than zero.");
            if (margin < 0) throw new ArgumentOutOfRangeException("margin", margin, "Must be greater than or equal to zero.");
            if (manyMargin < 0) throw new ArgumentOutOfRangeException("manyMargin", manyMargin, "Must be greater than or equal to zero.");
            if (!Enum.IsDefined(typeof(ErrorCorrectionLevel), errorCorrection)) throw new InvalidEnumArgumentException("errorCorrectionLevel", (int)errorCorrection, typeof(ErrorCorrectionLevel));

            var countX = data.GetLength(0);
            var countY = data.GetLength(1);
            var img = new Bitmap(size * countX + manyMargin * (countX + 1), size * countY + manyMargin * (countY + 1));

            using (var g = Graphics.FromImage(img))
            {
                g.Clear(Color.White);

                for (int x = 0; x < countX; x++)
                {
                    for (int y = 0; y < countY; y++)
                    {
                        using (var qr = QRCode.Generate(data[x, y], size, margin, errorCorrection))
                        {
                            var loc = new Point(x * qr.Width + manyMargin * x + manyMargin, y * qr.Height + manyMargin * y + manyMargin);

                            g.DrawImage(qr, loc);
                            g.DrawRectangle(Pens.Black, new Rectangle(loc, new Size(qr.Width - 1, qr.Height - 1)));
                        }
                    }
                }
            }

            return img;
        }
    }

    /// <summary>
    /// QR codes support four levels of error correction to enable recovery of missing, misread, or obscured data.
    /// Greater redundancy is achieved at the cost of being able to store less data.
    /// </summary>
    public enum ErrorCorrectionLevel
    {
        /// <summary>
        /// [Default] Allows recovery of up to 7% data loss
        /// </summary>
        Low = 0,
        /// <summary>
        /// Allows recovery of up to 15% data loss
        /// </summary>
        Medium,
        /// <summary>
        /// Allows recovery of up to 25% data loss
        /// </summary>
        QuiteGood,
        /// <summary>
        /// Allows recovery of up to 30% data loss
        /// </summary>
        High
    }
}