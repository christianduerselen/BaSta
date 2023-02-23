$oldSln = Get-ChildItem *.sln
$slnName = [System.IO.Path]::GetFileNameWithoutExtension($oldSln.FullName)

If ($slnName -eq "") {
	$slnName = "Solution"
}

if (-not(Test-Path -Path "$slnName.sln" -PathType Leaf)) {
	dotnet new sln --name $slnName --force
}

dotnet sln "$slnName.sln" list | ForEach {
	dotnet sln "$slnName.sln" remove $_
}

Get-ChildItem -Path "src" -Include "*.csproj" -Recurse | ForEach { dotnet sln "$slnName.sln" add $_.FullName --in-root }
Get-ChildItem -Path "test" -Include "*.csproj" -Recurse | ForEach { dotnet sln "$slnName.sln" add $_.FullName --solution-folder "Test" }