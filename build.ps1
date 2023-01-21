param(
  [string]$configuration = 'Release',
  [string]$path = $PSScriptRoot
)

$solution_filepath = "$path/src/AdminApp.Extensions.EmailSending.Abstractions/AdminApp.Extensions.EmailSending.Abstractions.sln"
Write-Output "Building $solution_filepath..."
& dotnet build $solution_filepath -c $configuration --no-incremental

$solution_filepath = "$path/src/AdminApp.Extensions.EmailSending/AdminApp.Extensions.EmailSending.sln"
Write-Output "Building $solution_filepath..."
& dotnet build $solution_filepath -c $configuration --no-incremental

$solution_filepath = "$path/src/AdminApp.Extensions.EmailSending.Mailgun/AdminApp.Extensions.EmailSending.Mailgun.sln"
Write-Output "Building $solution_filepath..."
& dotnet build $solution_filepath -c $configuration --no-incremental

$solution_filepath = "$path/src/AdminApp.Extensions.EmailSending.SendGrid/AdminApp.Extensions.EmailSending.SendGrid.sln"
Write-Output "Building $solution_filepath..."
& dotnet build $solution_filepath -c $configuration --no-incremental
