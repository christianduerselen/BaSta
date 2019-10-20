using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NDbfReader;

namespace BaSta.IO.Dbase
{
    /// <summary>
    /// Class parsing model information stored in a dBASE file.
    /// </summary>
    internal class DbaseParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbaseParser"/> class.
        /// </summary>
        /// <param name="filePath">The path to the dBASE file to parse data from.</param>
        internal DbaseParser(string filePath)
        {
            using (Table table = Table.Open(filePath))
            {
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbaseParser"/> class.
        /// </summary>
        /// <param name="stream">The stream of the dBASE file information to parse data from.</param>
        internal DbaseParser(Stream stream)
        {
            using (Table table = Table.Open(stream))
            {
                Reader reader = table.OpenReader(Encoding.ASCII);
                string[] columns = table.Columns.Select(x => x.Name).ToArray();
                while (reader.Read())
                {
                    foreach (string column in columns)
                    {
                        Debug.WriteLine($"{column}: {reader.GetValue(column)}");
                    }
                }
            }
        }
    }
}