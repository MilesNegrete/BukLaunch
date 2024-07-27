using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PdfiumViewer;

class CoverMaker
{
    static void Main(string[] args)
    {
        string folderPath = @"C:\Users\miles\Desktop\books\Dungeon Crawler"; 
        string outputFolderPath = @"C:\Users\miles\Desktop\books\Dungeon Crawler\Covers"; 

        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }

        foreach (string pdfFilePath in Directory.GetFiles(folderPath, "*.pdf"))
        {
            try
            {
                using (var document = PdfDocument.Load(pdfFilePath))
                {
                    var page = document.Render(0, 300, 300, true);

                    string fileName = Path.GetFileNameWithoutExtension(pdfFilePath) + ".png";
                    string outputFilePath = Path.Combine(outputFolderPath, fileName);

                    page.Save(outputFilePath, ImageFormat.Png);

                    page.Dispose();

                    Console.WriteLine($"Converted {pdfFilePath} to {outputFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {pdfFilePath}: {ex.Message}");
            }
        }

        Console.WriteLine("Processing completed.");
    }
}