using Bugporter.API.Features.ReportBub;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugporter.API.Features.ReportBub.GitHub
{
    public class CreateGitHubIssueCommand
    {
        private readonly GitHubClient _gitHubClient;
        private readonly GitHubRepositoryOptions _gitHubRepositoryOptions;
        private readonly ILogger<CreateGitHubIssueCommand> _logger;

        public CreateGitHubIssueCommand(ILogger<CreateGitHubIssueCommand> logger, GitHubClient gitHubClient, IOptions<GitHubRepositoryOptions> gitHubRepositoryOptions)
        {
            _logger = logger;
            _gitHubClient = gitHubClient;
            _gitHubRepositoryOptions = gitHubRepositoryOptions.Value;
        }

        public async Task<ReportedBug> Execute(NewBug newBug)
        {
            _logger.LogInformation("Create GitHub issue");

            NewIssue newIssue = new(newBug.Summary)
            {
                Body = newBug.Description
            };

            Issue createdIssue = await _gitHubClient.Issue.Create(
                _gitHubRepositoryOptions.Owner,
                _gitHubRepositoryOptions.Name,
                newIssue);

            _logger.LogInformation("Successfuly created GitHub issue {Id}", createdIssue.Number);

            return new(createdIssue.Number.ToString(), createdIssue.Title, createdIssue.Body);
        }
    }
}
