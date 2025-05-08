// Add the required libraries
#r "Newtonsoft.Json"
#r "Microsoft.Azure.Workflows.Scripting"
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Workflows.Scripting;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Text;

/// <summary>
/// Executes the inline csharp code.
/// </summary>
/// <param name="context">The workflow context.</param>
/// <remarks> This is the entry-point to your code. The function signature should remain unchanged.</remarks>
public static async Task<string> Run(WorkflowContext context, ILogger log)
{
  var composeOutputs = (await context.GetActionResults("Compose_archiveContent").ConfigureAwait(false)).Outputs;

  Stream outputStream = new MemoryStream();

  using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
  {
    foreach (var item in composeOutputs.Children())
    {        
      if (string.IsNullOrEmpty(item["filename"]?.ToString()) || string.IsNullOrEmpty(item["fileContent"]?.ToString()))
      {
        continue;
        //return "Invalid input data.";
      }
          
      var zipEntry = zipArchive.CreateEntry(item["filename"]?.ToString(), CompressionLevel.Optimal);
          
      using (var zipEntryStream = zipEntry.Open())
      {
        // Create a new stream to write the file content to the zip entry        
        using (var binaryWriter = new BinaryWriter(zipEntryStream))
        { 
          binaryWriter.Write(Convert.FromBase64String(item["fileContent"]?.ToString()));
        }                        
      }     
    }
  }
    
  //return the zip stream in base64 encoding
  outputStream.Position = 0;
  using (BinaryReader binaryReader = new BinaryReader(outputStream))
  {
    return Convert.ToBase64String(binaryReader.ReadBytes((int)outputStream.Length));
  }  
}