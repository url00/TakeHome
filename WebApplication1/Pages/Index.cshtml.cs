using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CliWrap;
using CliWrap.Buffered;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Dictionary<string, string> CommandCheckingResults = new Dictionary<string, string>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            var fullCommandLine = new List<string>
            {
                "node -v",
                "npm -v",
                "python --version",
                "yarn -v",
                "ng -v",
                "chrome -v",
            };

            foreach (var commandLine in fullCommandLine)
            {
                try
                {
                    var command = commandLine.Split(" ").First();
                    var args = string.Join(" ", commandLine.Split(" ").Skip(1).ToList());
                    var result = await Cli.Wrap(command)
                        .WithArguments(args)
                        .ExecuteBufferedAsync();

                    if (result.ExitCode != 0)
                    {
                        CommandCheckingResults[command] = "Error checking.";
                    }
                    else
                    {
                        CommandCheckingResults[command] = result.StandardOutput;
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
