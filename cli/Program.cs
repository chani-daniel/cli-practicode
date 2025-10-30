#region MyRegion
#region MyRegion
//using System.CommandLine;

//var bundleOption = new Option<FileInfo>("-- output", "File path and name");
//var bundleCommand = new Command("bundle", "Bundle code files to a single file");
//bundleCommand.AddOption(bundleOption);
//bundleCommand.SetHandler((output) =>
//{
//    try
//    {
//        File.Create(output.FullName);
//        Console.WriteLine("File was created");
//    }
//    catch (DirectoryNotFoundException ex)
//    {
//        Console.WriteLine("Error: File path is invalid");
//    }
//}, bundleOption);


//var rootCommand = new RootCommand("Root command for File Bundler CLI");
//rootCommand.AddCommand(bundleCommand);

//rootCommand.InvokeAsync(args);

#endregion

#region adi

//using System.CommandLine;
//using System.CommandLine.Invocation;
//using System.Text;



//var bundleCommand = new Command("bundle", "Bundle files to one file");

//// ------------------ הגדרת אפשרויות עם כינויים ------------------
//var languageOption = new Option<string>("--language", "Programming language (e.g., C#)")
//{
//    IsRequired = true
//};
//languageOption.AddAlias("-l");
//bundleCommand.AddOption(languageOption);

//var bundleOption = new Option<FileInfo>("--output", "Output bundle file");
//bundleOption.AddAlias("-o");
//bundleCommand.AddOption(bundleOption);

//var noteOption = new Option<bool>("--note", "Include source note in bundle");
//noteOption.AddAlias("-n");
//bundleCommand.AddOption(noteOption);

//var sortOption = new Option<string>("--sort", () => "name", "Sort files by 'name' or 'type'");
//sortOption.AddAlias("-s");
//bundleCommand.AddOption(sortOption);

//var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "Remove empty lines from source files");
//removeEmptyLinesOption.AddAlias("-r");
//bundleCommand.AddOption(removeEmptyLinesOption);

//var authorOption = new Option<string>("--author", "Author name to include in bundle");
//authorOption.AddAlias("-a");
//bundleCommand.AddOption(authorOption);

//// ------------------ Handler של הפקודה bundle ------------------
//bundleCommand.SetHandler((output, note, sort, removeEmptyLines, author, language) =>
//{
//    // --------- בדיקות תקינות ---------
//    if (string.IsNullOrEmpty(language))
//    {
//        Console.WriteLine("Error: Language cannot be empty.");
//        return;
//    }

//    if (output == null)
//    {
//        Console.WriteLine("Error: Output file must be specified.");
//        return;
//    }

//    try
//    {
//        var test = File.Create(output.FullName);
//        test.Close();
//        File.Delete(output.FullName);
//    }
//    catch
//    {
//        Console.WriteLine("Error: Output file path is not valid.");
//        return;
//    }

//    if (sort != "name" && sort != "type")
//    {
//        Console.WriteLine("Warning: Sort must be 'name' or 'type'. Using default 'name'.");
//        sort = "name";
//    }

//    try
//    {
//        using (var fs = File.Create(output.FullName))
//        {
//            // --------- כתיבת הערות ----------
//            if (!string.IsNullOrEmpty(author))
//                fs.Write(Encoding.UTF8.GetBytes($"// Created by: {author}\n"));

//            if (note)
//                fs.Write(Encoding.UTF8.GetBytes("// מקור הקוד: שם קובץ המקור ונתיב יחסי\n"));

//            // --------- איסוף קבצים כולל סינון תיקיות ---------
//            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories)
//                                 .Where(f => f.EndsWith(language, StringComparison.OrdinalIgnoreCase))
//                                 .Where(f => !f.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar) &&
//                                             !f.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar) &&
//                                             !f.Contains(Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar))
//                                 .ToList();

//            // --------- מיון הקבצים ---------
//            if (sort == "name")
//                files.Sort();
//            else
//                files = files.OrderBy(f => Path.GetExtension(f)).ThenBy(f => f).ToList();

//            // --------- קריאה וכתיבה ל-bundle ---------
//            foreach (var file in files)
//            {
//                var lines = File.ReadAllLines(file);

//                if (removeEmptyLines)
//                    lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

//                foreach (var line in lines)
//                    fs.Write(Encoding.UTF8.GetBytes(line + "\n"));
//            }
//        }

//        Console.WriteLine("Bundle created successfully.");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error: {ex.Message}");
//    }

//}, bundleOption, noteOption, sortOption, removeEmptyLinesOption, authorOption, languageOption);

//// ------------------ פקודת create-rsp ------------------
//var createRspCommand = new Command("create-rsp", "Create a response file for the bundle command");
//var rspFileOption = new Option<FileInfo>("--file", "Name of the response file to create") { IsRequired = true };
//createRspCommand.AddOption(rspFileOption);

//createRspCommand.SetHandler((rspFile) =>
//{
//    // --------- קלט מהמשתמש ---------
//    Console.Write("Enter language (e.g., C#): ");
//    var language = Console.ReadLine()?.Trim();
//    if (string.IsNullOrEmpty(language))
//    {
//        Console.WriteLine("Error: Language cannot be empty.");
//        return;
//    }

//    Console.Write("Enter output file name: ");
//    var output = Console.ReadLine()?.Trim();
//    if (string.IsNullOrEmpty(output))
//    {
//        Console.WriteLine("Error: Output file must be specified.");
//        return;
//    }

//    Console.Write("Include note? (true/false): ");
//    var note = Console.ReadLine()?.Trim().ToLower() == "true";

//    Console.Write("Sort by (name/type, default name): ");
//    var sort = Console.ReadLine()?.Trim();
//    if (string.IsNullOrEmpty(sort)) sort = "name";
//    if (sort != "name" && sort != "type")
//    {
//        Console.WriteLine("Warning: Sort must be 'name' or 'type'. Using default 'name'.");
//        sort = "name";
//    }

//    Console.Write("Remove empty lines? (true/false): ");
//    var removeEmptyLines = Console.ReadLine()?.Trim().ToLower() == "true";

//    Console.Write("Author name: ");
//    var author = Console.ReadLine()?.Trim();

//    // --------- בניית פקודה מלאה ---------
//    var commandParts = new List<string> { "bundle" };

//    commandParts.Add($"--language {language}");
//    commandParts.Add($"--output {output}");
//    if (note) commandParts.Add("--note");
//    commandParts.Add($"--sort {sort}");
//    if (removeEmptyLines) commandParts.Add("--remove-empty-lines");
//    if (!string.IsNullOrEmpty(author)) commandParts.Add($"--author \"{author}\"");

//    var finalCommand = string.Join(" ", commandParts);

//    // --------- שמירה לקובץ תגובה ---------
//    try
//    {
//        File.WriteAllText(rspFile.FullName, finalCommand);
//        Console.WriteLine($"Response file created successfully: {rspFile.FullName}");
//        Console.WriteLine($"Contents:\n{finalCommand}");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error writing response file: {ex.Message}");
//    }

//}, rspFileOption);

//// ------------------ הוספה ל-rootCommand ------------------
//var rootCommand = new RootCommand("Command-line utility for bundling code");
//rootCommand.AddCommand(bundleCommand);
//rootCommand.AddCommand(createRspCommand);

//rootCommand.InvokeAsync(args);



//using System.CommandLine;
//using System.CommandLine.Invocation;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Collections.Generic;

//var bundleCommand = new Command("bundle", "Bundle files to one file");

//// ------------------ הגדרת אפשרויות עם כינויים ------------------
//var languageOption = new Option<string>("--language", "Programming language (e.g., C#)")
//{
//    IsRequired = true
//};
//languageOption.AddAlias("-l");
//bundleCommand.AddOption(languageOption);

//var bundleOption = new Option<FileInfo>("--output", "Output bundle file");
//bundleOption.AddAlias("-o");
//bundleCommand.AddOption(bundleOption);

//var noteOption = new Option<bool>("--note", "Include source note in bundle");
//noteOption.AddAlias("-n");
//bundleCommand.AddOption(noteOption);

//var sortOption = new Option<string>("--sort", () => "name", "Sort files by 'name' or 'type'");
//sortOption.AddAlias("-s");
//bundleCommand.AddOption(sortOption);

//var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "Remove empty lines from source files");
//removeEmptyLinesOption.AddAlias("-r");
//bundleCommand.AddOption(removeEmptyLinesOption);

//var authorOption = new Option<string>("--author", "Author name to include in bundle");
//authorOption.AddAlias("-a");
//bundleCommand.AddOption(authorOption);

//// ------------------ Handler של הפקודה bundle ------------------
//bundleCommand.SetHandler((InvocationContext context) =>
//{
//    var output = context.ParseResult.GetValueForOption(bundleOption);
//    var note = context.ParseResult.GetValueForOption(noteOption);
//    var sort = context.ParseResult.GetValueForOption(sortOption);
//    var removeEmptyLines = context.ParseResult.GetValueForOption(removeEmptyLinesOption);
//    var author = context.ParseResult.GetValueForOption(authorOption);
//    var language = context.ParseResult.GetValueForOption(languageOption);

//    // --------- בדיקות תקינות ---------
//    if (string.IsNullOrEmpty(language))
//    {
//        Console.WriteLine("Error: Language cannot be empty.");
//        /*return*/;  // לא מחזיר Task
//    }

//    if (output == null)
//    {
//        Console.WriteLine("Error: Output file must be specified.");
//        /*return*/;  // לא מחזיר Task
//    }

//    try
//    {
//        var test = File.Create(output.FullName);
//        test.Close();
//        File.Delete(output.FullName);
//    }
//    catch
//    {
//        Console.WriteLine("Error: Output file path is not valid.");
//        /*return*/;  // לא מחזיר Task
//    }

//    if (sort != "name" && sort != "type")
//    {
//        Console.WriteLine("Warning: Sort must be 'name' or 'type'. Using default 'name'.");
//        sort = "name";
//    }

//    try
//    {
//        using (var fs = File.Create(output.FullName))
//        {
//            // --------- כתיבת הערות ----------
//            if (!string.IsNullOrEmpty(author))
//                fs.Write(Encoding.UTF8.GetBytes($"// Created by: {author}\n"));

//            if (note)
//                fs.Write(Encoding.UTF8.GetBytes("// מקור הקוד: שם קובץ המקור ונתיב יחסי\n"));

//            // --------- איסוף קבצים כולל סינון תיקיות ---------
//            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories)
//                                 .Where(f => f.EndsWith(language, StringComparison.OrdinalIgnoreCase))
//                                 .Where(f => !f.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar) &&
//                                             !f.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar) &&
//                                             !f.Contains(Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar))
//                                 .ToList();

//            // --------- מיון הקבצים ---------
//            if (sort == "name")
//                files.Sort();
//            else
//                files = files.OrderBy(f => Path.GetExtension(f)).ThenBy(f => f).ToList();

//            // --------- קריאה וכתיבה ל-bundle ---------
//            foreach (var file in files)
//            {
//                var lines = File.ReadAllLines(file);

//                if (removeEmptyLines)
//                    lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

//                foreach (var line in lines)
//                    fs.Write(Encoding.UTF8.GetBytes(line + "\n"));
//            }
//        }

//        Console.WriteLine("Bundle created successfully.");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error: {ex.Message}");
//    }

//}, bundleOption, noteOption, sortOption, removeEmptyLinesOption, authorOption, languageOption);

//// ------------------ פקודת create-rsp ------------------
//var createRspCommand = new Command("create-rsp", "Create a response file for the bundle command");
//var rspFileOption = new Option<FileInfo>("--file", "Name of the response file to create") { IsRequired = true };
//createRspCommand.AddOption(rspFileOption);

//createRspCommand.SetHandler((rspFile) =>
//{
//    // --------- קלט מהמשתמש ---------
//    Console.Write("Enter language (e.g., C#): ");
//    var language = Console.ReadLine()?.Trim();
//    if (string.IsNullOrEmpty(language))
//    {
//        Console.WriteLine("Error: Language cannot be empty.");
//        return;  // לא מחזיר Task
//    }

//    Console.Write("Enter output file name: ");
//    var output = Console.ReadLine()?.Trim();
//    if (string.IsNullOrEmpty(output))
//    {
//        Console.WriteLine("Error: Output file must be specified.");
//        return;  // לא מחזיר Task
//    }

//    Console.Write("Include note? (true/false): ");
//    var note = Console.ReadLine()?.Trim().ToLower() == "true";

//    Console.Write("Sort by (name/type, default name): ");
//    var sort = Console.ReadLine()?.Trim();
//    if (string.IsNullOrEmpty(sort)) sort = "name";
//    if (sort != "name" && sort != "type")
//    {
//        Console.WriteLine("Warning: Sort must be 'name' or 'type'. Using default 'name'.");
//        sort = "name";
//    }

//    Console.Write("Remove empty lines? (true/false): ");
//    var removeEmptyLines = Console.ReadLine()?.Trim().ToLower() == "true";

//    Console.Write("Author name: ");
//    var author = Console.ReadLine()?.Trim();

//    // --------- בניית פקודה מלאה ---------
//    var commandParts = new List<string> { "bundle" };

//    commandParts.Add($"--language {language}");
//    commandParts.Add($"--output {output}");
//    if (note) commandParts.Add("--note");
//    commandParts.Add($"--sort {sort}");
//    if (removeEmptyLines) commandParts.Add("--remove-empty-lines");
//    if (!string.IsNullOrEmpty(author)) commandParts.Add($"--author \"{author}\"");

//    var finalCommand = string.Join(" ", commandParts);

//    // --------- שמירה לקובץ תגובה ---------
//    try
//    {
//        File.WriteAllText(rspFile.FullName, finalCommand);
//        Console.WriteLine($"Response file created successfully: {rspFile.FullName}");
//        Console.WriteLine($"Contents:\n{finalCommand}");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error writing response file: {ex.Message}");
//    }

//}, rspFileOption);

//// ------------------ הוספה ל-rootCommand ------------------
//var rootCommand = new RootCommand("Command-line utility for bundling code");
//rootCommand.AddCommand(bundleCommand);
//rootCommand.AddCommand(createRspCommand);

//rootCommand.InvokeAsync(args).Wait();


#endregion

#region good
//using System;
//using System.CommandLine;
//using System.IO;
//using System.Threading.Tasks;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        // יצירת הפקודות
//        var bundleCommand = new Command("bundle", "Run the bundle command with the specified options");
//        var createRspCommand = new Command("create-rsp", "Create a response file for the bundle command");

//        // הגדרת הפקודה 'bundle'
//        bundleCommand.SetHandler(async () =>
//        {
//            string language = PromptForInput("Enter language (e.g., 'C#' or 'all' for all): ");
//            string output = PromptForInput("Enter output file path (e.g., 'bundle.zip'): ");
//            string note = PromptForInput("Enter note (optional, press Enter to skip): ");
//            string sort = PromptForInput("Enter sort order (optional, default is 'name'): ");
//            bool removeEmptyLines = PromptForBoolInput("Remove empty lines (yes/no): ");
//            string author = PromptForInput("Enter author (optional, press Enter to skip): ");

//            // כאן תוכל להוסיף את הקוד להריץ את הפקודה Bundle עם האפשרויות שנבחרו
//            Console.WriteLine("Executing bundle with the following options:");
//            Console.WriteLine($"Language: {language}");
//            Console.WriteLine($"Output: {output}");
//            Console.WriteLine($"Note: {note}");
//            Console.WriteLine($"Sort: {sort}");
//            Console.WriteLine($"Remove empty lines: {removeEmptyLines}");
//            Console.WriteLine($"Author: {author}");
//        });

//        // הגדרת הפקודה 'create-rsp'
//        createRspCommand.SetHandler(async () =>
//        {
//            Console.WriteLine("Please enter the following details to create the response file.");

//            string language = PromptForInput("Language (e.g., 'C#' or 'all' for all): ");
//            string output = PromptForInput("Output file path (e.g., 'bundle.zip'): ");
//            string note = PromptForInput("Note (optional, press Enter to skip): ");
//            string sort = PromptForInput("Sort order (optional, default is 'name'): ");
//            bool removeEmptyLines = PromptForBoolInput("Remove empty lines (yes/no): ");
//            string author = PromptForInput("Author (optional, press Enter to skip): ");

//            // יצירת קובץ תגובה
//            string rspFileName = "response.rsp";
//            using (var writer = new StreamWriter(rspFileName))
//            {
//                if (!string.IsNullOrEmpty(language))
//                    await writer.WriteLineAsync($"--language {language}");
//                if (!string.IsNullOrEmpty(output))
//                    await writer.WriteLineAsync($"--output \"{output}\"");
//                if (!string.IsNullOrEmpty(note))
//                    await writer.WriteLineAsync($"--note \"{note}\"");
//                if (!string.IsNullOrEmpty(sort))
//                    await writer.WriteLineAsync($"--sort {sort}");
//                if (removeEmptyLines)
//                    await writer.WriteLineAsync("--remove-empty-lines");
//                if (!string.IsNullOrEmpty(author))
//                    await writer.WriteLineAsync($"--author \"{author}\"");
//            }

//            Console.WriteLine($"Response file created: {rspFileName}");
//        });

//        // יצירת RootCommand והרצה
//        var rootCommand = new RootCommand("Project CLI");
//        rootCommand.AddCommand(bundleCommand);
//        rootCommand.AddCommand(createRspCommand);
//        await rootCommand.InvokeAsync(args);
//    }

//    // פונקציות עזר להקלת קלט מהמשתמש
//    static string PromptForInput(string prompt)
//    {
//        Console.WriteLine(prompt);
//        return Console.ReadLine();
//    }

//    static bool PromptForBoolInput(string prompt)
//    {
//        Console.WriteLine(prompt);
//        string input = Console.ReadLine().ToLower();
//        return input == "yes" || input == "y";
//    }
//}

#endregion
#endregion


using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // יצירת הפקודות
        var bundleCommand = new Command("bundle", "Run the bundle command with the specified options");
        var createRspCommand = new Command("create-rsp", "Create a response file for the bundle command");

        // הגדרת האופציות של הפקודה 'bundle'
        var languageOption = new Option<string>("--language", "Enter language (e.g., 'C#' or 'all' for all)");
        var outputOption = new Option<string>("--output", "Enter output file path (e.g., 'bundle.zip')");
        var noteOption = new Option<string>("--note", "Enter note (optional, press Enter to skip)");
        var sortOption = new Option<string>("--sort", "Enter sort order (optional, default is 'name')");
        var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "Remove empty lines (yes/no)");
        var authorOption = new Option<string>("--author", "Enter author (optional, press Enter to skip)");

        // הוספת האופציות לפקודה
        bundleCommand.AddOption(languageOption);
        bundleCommand.AddOption(outputOption);
        bundleCommand.AddOption(noteOption);
        bundleCommand.AddOption(sortOption);
        bundleCommand.AddOption(removeEmptyLinesOption);
        bundleCommand.AddOption(authorOption);

        // הגדרת הפקודה 'bundle'
        bundleCommand.SetHandler(async (InvocationContext context) =>
        {
            // קריאה לאופציות מתוך הפקודה
            string language = context.ParseResult.GetValueForOption(languageOption);
            string output = context.ParseResult.GetValueForOption(outputOption);
            string note = context.ParseResult.GetValueForOption(noteOption);
            string sort = context.ParseResult.GetValueForOption(sortOption);
            bool removeEmptyLines = context.ParseResult.GetValueForOption(removeEmptyLinesOption);
            string author = context.ParseResult.GetValueForOption(authorOption);

            // אם לא נבחרו ערכים אז לשים ברירת מחדל
            if (string.IsNullOrEmpty(language)) language = "all";
            if (string.IsNullOrEmpty(sort)) sort = "name";

            // טיפול בנתיב עם רווחים
            if (!string.IsNullOrEmpty(output) && output.Contains(" "))
            {
                output = $"\"{output}\"";
            }

            // קריאה לפונקציה שמבצע את האריזה
            await BundleCode(language, output, note, sort, removeEmptyLines, author);
        });

        // הגדרת הפקודה 'create-rsp'
        createRspCommand.SetHandler(async () =>
        {
            Console.WriteLine("Please enter the following details to create the response file.");

            string language = PromptForInput("Language (e.g., 'C#' or 'all' for all): ");
            string output = PromptForInput("Output file path (e.g., 'bundle.zip'): ");
            string note = PromptForInput("Note (optional, press Enter to skip): ");
            string sort = PromptForInput("Sort order (optional, default is 'name'): ");
            bool removeEmptyLines = PromptForBoolInput("Remove empty lines (yes/no): ");
            string author = PromptForInput("Author (optional, press Enter to skip): ");

            // יצירת קובץ תגובה
            string rspFileName = "response.rsp";
            using (var writer = new StreamWriter(rspFileName))
            {
                if (!string.IsNullOrEmpty(language))
                    await writer.WriteLineAsync($"--language {language}");
                if (!string.IsNullOrEmpty(output))
                    await writer.WriteLineAsync($"--output \"{output}\"");
                if (!string.IsNullOrEmpty(note))
                    await writer.WriteLineAsync($"--note \"{note}\"");
                if (!string.IsNullOrEmpty(sort))
                    await writer.WriteLineAsync($"--sort {sort}");
                if (removeEmptyLines)
                    await writer.WriteLineAsync("--remove-empty-lines");
                if (!string.IsNullOrEmpty(author))
                    await writer.WriteLineAsync($"--author \"{author}\"");
            }

            Console.WriteLine($"Response file created: {rspFileName}");
        });

        // יצירת RootCommand והרצה
        var rootCommand = new RootCommand("Project CLI");
        rootCommand.AddCommand(bundleCommand);
        rootCommand.AddCommand(createRspCommand);
        await rootCommand.InvokeAsync(args);
    }

    // פונקציות עזר להקלת קלט מהמשתמש
    static string PromptForInput(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
    }

    static bool PromptForBoolInput(string prompt)
    {
        Console.WriteLine(prompt);
        string input = Console.ReadLine().ToLower();
        return input == "yes" || input == "y";
    }

    // פונקציה לטיפול באריזת קבצים
    static async Task BundleCode(string language, string output, string note, string sort, bool removeEmptyLines, string author)
    {
        // אם השפה היא "all", כל קבצי הקוד ייכללו
        var allFiles = Directory.GetFiles(".", "*.*", SearchOption.AllDirectories)
            .Where(file => !file.Contains("bin") && !file.Contains("debug") && IsValidLanguage(file, language))
            .ToList();

        // מיון קבצים לפי שם או סוג
        if (sort == "name")
        {
            allFiles = allFiles.OrderBy(f => f).ToList();
        }
        else if (sort == "type")
        {
            allFiles = allFiles.OrderBy(f => Path.GetExtension(f)).ToList();
        }

        // פתיחת קובץ ה-bundle לכתיבה
        using (var bundleWriter = new StreamWriter(output))
        {
            if (!string.IsNullOrEmpty(author))
                await bundleWriter.WriteLineAsync($"# Author: {author}");

            if (!string.IsNullOrEmpty(note))
                await bundleWriter.WriteLineAsync($"# Note: {note}");

            // הוספת כל קובץ
            foreach (var file in allFiles)
            {
                if (removeEmptyLines)
                {
                    // קריאה לקובץ והסרת שורות ריקות
                    var content = await File.ReadAllLinesAsync(file);
                    content = content.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                    await bundleWriter.WriteLineAsync($"# Source: {file}");
                    foreach (var line in content)
                    {
                        await bundleWriter.WriteLineAsync(line);
                    }
                }
                else
                {
                    var content = await File.ReadAllTextAsync(file);
                    await bundleWriter.WriteLineAsync($"# Source: {file}");
                    await bundleWriter.WriteLineAsync(content);
                }
            }
        }

        Console.WriteLine($"Files bundled into {output}");
    }

    // פונקציה לוודא שהשפה תקנית או "all"
    static bool IsValidLanguage(string file, string language)
    {
        if (language == "all") return true;
        string extension = Path.GetExtension(file)?.ToLower();
        return (language.ToLower() == "c#" && extension == ".cs");
    }
}
