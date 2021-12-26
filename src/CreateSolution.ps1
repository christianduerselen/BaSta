dotnet new sln --name Solution
Get-ChildItem -Recurse *.csproj | ForEach { dotnet sln Solution.sln add $_.FullName }