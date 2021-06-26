using System;
using System.IO;
using System.Runtime.InteropServices;
using ImageMagick;

namespace PhotoRepack
{
    class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="inputDir">The input directory images will be read from.</param>
        /// <param name="outputDir">The output directory images will be written to.</param>
        static void Main(string? inputDir, string? outputDir)
        {
            if (string.IsNullOrEmpty(inputDir)) {
                Console.Error.WriteLine("--input-dir is required");
                return;
            }
            if (string.IsNullOrEmpty(outputDir)) {
                Console.Error.WriteLine("--output-dir is required");
                return;
            }

            // Normalise path
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                // Patch user path
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/";
                inputDir = inputDir.Replace("~/", userProfile);
                inputDir = inputDir.Replace("~\\", userProfile);
                outputDir = outputDir.Replace("~/", userProfile);
                outputDir = outputDir.Replace("~\\", userProfile);
            }
            inputDir = Path.GetFullPath(inputDir);
            outputDir = Path.GetFullPath(outputDir);

            // Fuck-up guard, input is output
            if (inputDir == outputDir) {
                Console.Error.WriteLine("--input-dir is same as --output-dir, overwriting not allowed");
                return;
            }

            // Fuck-up guard, output in input
            if (outputDir.Contains(inputDir)) {
                Console.Error.WriteLine("--output-dir is inside --input-dir, aborting to prevent recursion");
                return;
            }

            // Fuck-up guard, input in output
            if (inputDir.Contains(outputDir)) {
                Console.Error.WriteLine("--input-dir is inside --output-dir, overwriting not allowed");
                return;
            }

            // Lets get to work

            // Find files in input
            var files = Directory.GetFiles(inputDir, "**", new EnumerationOptions() {
                RecurseSubdirectories = true,
            });

            var optimizer = new ImageOptimizer();
            optimizer.OptimalCompression = true;
            foreach (var file in files)
            {
                Console.Out.WriteLine($"Processing {file}");
                try {
                    var dest = file.Replace(inputDir, outputDir);
                    var destParent = Directory.GetParent(dest);
                    if (destParent is not null) {
                        destParent.Create();
                    }
                    File.Copy(file, dest);
                    optimizer.LosslessCompress(dest);
                }
                catch (Exception e) {
                    Console.Error.WriteLine("An error occurred");
                    Console.Error.WriteLine(e);
                }
            }
        }
    }
}
