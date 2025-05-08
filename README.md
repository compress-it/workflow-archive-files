# workflow-archive-files
An example Azure LogicApp Standard workflow (Not consumption/shared) example demonstrating the archiving and compression of files into a ZIP file.
The Azure Logic app uses an inline C# script to zip several files using the native C# .Net System.IO.Compression library.

General flow:
A "List files" (Azure File Storage) action is used to return a list of files in a specified folder. The workflow iterates through each file and extracts the file contents using the "Get File Content" action.
The contents are then Composed into a Json structure, which is passed to a C# script to compress and zip the source files.

Only standard non-commercial actions are used and a separate Azure Function to perform the compression is not required. The standard "Extract Archive" action can be used to Unzip and decompress the content of the archive file. This example does not include the extraction of files. 

There are several ways to achieve the same objective, Standard Logic Apps support inline scripts and this capability is used for the compression.
