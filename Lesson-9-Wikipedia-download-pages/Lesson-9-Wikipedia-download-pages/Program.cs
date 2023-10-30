using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Security.Principal;
//using HtmlAgilityPack; // You'll need to install the HtmlAgilityPack NuGet package

char[] englishLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

List<string> lettersCombinations = new List<string>();

string targetDirectory = "C:\\Lesson-9";

try
{
    // Create a new DirectoryInfo object
    DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);

    // Get the directory's current access control list
    DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

    // Add a rule to allow write access
    FileSystemAccessRule accessRule = new FileSystemAccessRule(
        new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
        FileSystemRights.Write,
        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
        PropagationFlags.None,
        AccessControlType.Allow);

    // Add the access rule to the directory's access control list
    dirSecurity.AddAccessRule(accessRule);

    // Apply the modified access control list to the directory
    dirInfo.SetAccessControl(dirSecurity);

    Console.WriteLine("Permissions added successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

foreach (char c in englishLetters)
{
    for (int i = 0; i < englishLetters.Length; i++)
    {
        string combinedString = $".{c}{englishLetters[i]}";

        string baseUrl = "https://en.wikipedia.org/wiki/"; // base URL of the website to download.
        string baseUrlWithCombination = "https://en.wikipedia.org/wiki/" + combinedString; // base URL of the website to download.
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(baseUrlWithCombination);
                // 'htmlContent' now contains the HTML content of the web page.
                if (response.IsSuccessStatusCode)
                {
                    string htmlContent = await client.GetStringAsync(baseUrlWithCombination);
                    string folderPath = CreateDirectory(combinedString);
                    // You can save the HTML content to a file if you want, like this:
                    System.IO.File.WriteAllText(folderPath + "\\page-html", htmlContent);
                    Console.WriteLine($"Web page {baseUrlWithCombination} downloaded successfully.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}

static string CreateDirectory (string foldernName) 
{
    // Specify the base disk path where you want to create the folder.
    string baseDiskPath = "C:\\Lesson-9"; // Change this to your desired base path.

    // Combine the base path and customer name to create the full path.
    string folderPath = Path.Combine(baseDiskPath, foldernName);

    try
    {
        // Check if the folder already exists. If not, create it.
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Console.WriteLine($"Folder '{foldernName}' created successfully at '{baseDiskPath}'.");
            return folderPath;
        }
        else
        {
            Console.WriteLine($"Folder '{foldernName}' already exists at '{baseDiskPath}'.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
    return null;
}







