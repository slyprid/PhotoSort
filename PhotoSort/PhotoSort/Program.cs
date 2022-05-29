// =================================================================

const bool verbose = true;
const string sourcePath = "c:\\sample";
const string targetPath = "c:\\photos";

// Index source files
var files = new List<FileInfo>();
files.AddRange(GetFiles(sourcePath));
Console.WriteLine(string.Empty);

// Check and remove duplicates
var duplicates = GetDuplicates(files).ToList();
files = files.Except(duplicates).ToList();
Console.WriteLine(string.Empty);

Console.WriteLine($"Duplicates Found: {duplicates.Count()}");
Console.WriteLine($"Total Files: {files.Count}");

// =================================================================

#region Methods / Functions

IEnumerable<FileInfo> GetFiles(string path)
{
    Console.Write($"Indexing {path} - ");
    if(path == sourcePath) Console.WriteLine(string.Empty);

    var ret = Directory.EnumerateFiles(path).Select(file => new FileInfo(file)).ToList();

    foreach (var dir in Directory.EnumerateDirectories(path))
    {
        ret.AddRange(GetFiles(dir));
    }

    Console.WriteLine($" Files Found: [{ret.Count}]");

    return ret;
}

IEnumerable<FileInfo> GetDuplicates(IEnumerable<FileInfo> input)
{
    var ret = new List<FileInfo>();
    var duplicateGroups = input.GroupBy(file => file.Name).Where(group => group.Count() > 1);

    foreach(var group in duplicateGroups)
    {
        if(verbose) Console.WriteLine($"Files with name {group.Key}");
        foreach (var file in group.Skip(1))
        {
            if (verbose) Console.WriteLine($"   {file.FullName}");
            ret.Add(file);
        }
    }

    return ret;
}

#endregion