# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src"

# List of projects
$projects = (
    "Zql.RabbitMq.Sdk"   
)

# Rebuild solution
Set-Location $slnPath
& dotnet restore

# Copy all nuget packages to the pack folder
foreach($project in $projects) {
    
    $projectFolder = Join-Path $srcPath $project

    # Create nuget pack
    Set-Location $projectFolder
    Remove-Item -Recurse (Join-Path $projectFolder "bin/Release")
    & dotnet msbuild /p:Configuration=Release /p:SourceLinkCreate=true
    & dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true

    # Copy nuget package
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $project + ".*.nupkg")
    Move-Item $projectPackPath $packFolder

}

# Go back to the pack folder
Set-Location $packFolder

Write-Host ""
Write-Host "Do you wish to post to microsoft nuget ?"
Write-Host ""
$user_input = Read-Host 'Please enter y or n ?'
if ($user_input -eq 'y') {
    foreach ($packfile in Get-ChildItem -Path $packFolder -Recurse -Include *.nupkg) {
        ..\tools\nuget\nuget.exe push $packfile -Source https://www.nuget.org/api/v2/package oy2d4zneq3p7lgxcfafavf6dkujw5guunbzpv3facntqte
    }
}
del *.nupkg
pause
exit