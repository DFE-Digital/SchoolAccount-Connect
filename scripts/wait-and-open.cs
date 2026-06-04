using System.Diagnostics;

const string project = "schoolaccount";
const string service = "connect";
const string pattern = "Application started";
const string url = "https://127.0.0.1:7034";
var timeout = TimeSpan.FromSeconds(120);

var matched = new TaskCompletionSource<bool>(); // true = found, false = stream ended

using var proc = new Process();
proc.StartInfo = new ProcessStartInfo
{
    FileName = "docker",
    Arguments = $"compose -p {project} logs --tail 0 -f {service}",
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
};

proc.EnableRaisingEvents = true;
proc.OutputDataReceived += (_, e) => Handle(e.Data);
proc.ErrorDataReceived += (_, e) => Handle(e.Data);
proc.Exited += (_, _) => matched.TrySetResult(false); // docker quit first

proc.Start();
proc.BeginOutputReadLine();
proc.BeginErrorReadLine();

var found = matched.Task.Wait(timeout) && matched.Task.Result;

if (found)
    OpenBrowser(url);
else if (!matched.Task.IsCompleted)
    Console.Error.WriteLine($"Timed out after {timeout.TotalSeconds:0}s waiting for \"{pattern}\".");
else
    Console.Error.WriteLine($"Log stream ended before \"{pattern}\" appeared - check the project/service names.");

try
{
    proc.Kill(entireProcessTree: true);
}
catch (InvalidOperationException)
{ /* already gone */
}

proc.WaitForExit();
return found ? 0 : 1;

void Handle(string? line)
{
    if (line is null)
        return;
    Console.WriteLine(line);
    if (line.Contains(pattern, StringComparison.Ordinal))
        matched.TrySetResult(true);
}

static void OpenBrowser(string url)
{
    if (OperatingSystem.IsWindows())
        Process.Start(new ProcessStartInfo("cmd", $"/c start \"\" \"{url}\"") { CreateNoWindow = true });
    else if (OperatingSystem.IsMacOS())
        Process.Start("open", url);
    else
        Process.Start("xdg-open", url);
}
