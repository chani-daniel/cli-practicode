
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
