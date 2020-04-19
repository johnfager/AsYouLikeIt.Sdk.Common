
namespace Sdk.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class FileSplitter
    {


        #region methods

        /// <summary>
        /// Splits a large text file into smaller batches using the same file name with an "_X" appended where X is the index of output files.
        /// </summary>
        /// <param name="textFilePath">Fully qualified file path.</param>
        /// <param name="hasHeaders">Instructs the header from the file to be added to each child file.</param>
        /// <param name="maxRows">How many rows max can be in the child files.</param>
        /// <param name="overwrite">Whether to overwrite the existing files.</param>
        /// <returns>The output file names.</returns>
        public static IEnumerable<string> SplitTextFile(string textFilePath, bool hasHeaders, int maxRows, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(textFilePath))
            {
                throw new ArgumentNullException(nameof(textFilePath));
            }

            var fi = new FileInfo(textFilePath);
            if (!fi.Exists)
            {
                throw new FileNotFoundException(textFilePath);
            }

            int fileIndex = 0;
            int currentOutputRow = -1;
            string headerRow = null;

            var list = new List<string>();

            using (var sr = new StreamReader(fi.FullName))
            {
                StreamWriter sw;
                var outputFileName = fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length);

                var fiOut = new FileInfo(GetFileIndexFullName(fi.FullName, fi.Extension, fileIndex));
                if (fiOut.Exists)
                {
                    if (overwrite)
                    {
                        fiOut.Delete();
                    }
                    else
                    {
                        throw new IOException($"File '{fiOut.FullName}' already exists and overwrite is 'false'.");
                    }
                }
                sw = new StreamWriter(fiOut.FullName);
                do
                {

                    if (hasHeaders && fileIndex == 0 && currentOutputRow == -1)
                    {
                        headerRow = sr.ReadLine();
                        currentOutputRow = 0;
                    }
                    else
                    {
                        // if the maxRows has been hit
                        currentOutputRow++;

                        // if there are headers, place them in the file
                        if (currentOutputRow == 1 && hasHeaders)
                        {
                            sw.WriteLine(headerRow);
                        }

                        sw.WriteLine(sr.ReadLine());

                        // if the current row is at the max rows, close and change writers
                        if (currentOutputRow == maxRows)
                        {
                            // complete the old stream
                            sw.Dispose();
                            list.Add( outputFileName);
                            // redo the stream and counters
                            fileIndex++;
                            currentOutputRow = 0;
                            fiOut = new FileInfo(GetFileIndexFullName(fi.FullName, fi.Extension, fileIndex));
                            if (fiOut.Exists)
                            {
                                if (overwrite)
                                {
                                    fiOut.Delete();
                                }
                                else
                                {
                                    throw new InvalidOperationException($"File '{fiOut.FullName}' already exists and overwrite is 'false'.");
                                }
                            }
                            sw = new StreamWriter(fiOut.FullName);
                        }
                    }

                } while (sr.Peek() != -1);

                sw.Dispose();
                list.Add( outputFileName);
            }
            return list;
        }

        #endregion

        #region helpers

        private static string GetFileIndexFullName(string origFileName, string extension, int fileIndex)
        {
            var outputFileName = origFileName.Substring(0, origFileName.Length - extension.Length);
            return $"{outputFileName}_{fileIndex}{extension}";
        }

        #endregion

    }
}
