param(
    [Parameter(Mandatory=$false)][bool]   $RestorePackages  = $false,
    [Parameter(Mandatory=$false)][string] $Configuration    = "Release",
    [Parameter(Mandatory=$false)][string] $VersionSuffix    = "",
    [Parameter(Mandatory=$false)][string] $OutputPath       = "",
    [Parameter(Mandatory=$false)][bool]   $PatchVersion     = $false,
    [Parameter(Mandatory=$false)][bool]   $RunTests         = $true
)

$ErrorActionPreference = "Stop"

$solutionPath  = Split-Path $MyInvocation.MyCommand.Definition
$framework     = "netcoreapp1.1"
$dotnetVersion = "1.0.1"

if ($OutputPath -eq "") {
    $OutputPath = "$(Convert-Path "$PSScriptRoot")\artifacts"
}

$env:DOTNET_INSTALL_DIR = "$(Convert-Path "$PSScriptRoot")\.dotnetcli"

if ($env:CI -ne $null -Or $env:TF_BUILD -ne $null) {
    $RestorePackages = $true
    $PatchVersion = $true
}

if (!(Test-Path $env:DOTNET_INSTALL_DIR)) {
    mkdir $env:DOTNET_INSTALL_DIR | Out-Null
    $installScript = Join-Path $env:DOTNET_INSTALL_DIR "install.ps1"
    Invoke-WebRequest "https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0/scripts/obtain/dotnet-install.ps1" -OutFile $installScript
    & $installScript -Version "$dotnetVersion" -InstallDir "$env:DOTNET_INSTALL_DIR" -NoPath
}

$env:PATH = "$env:DOTNET_INSTALL_DIR;$env:PATH"
$dotnet   = "$env:DOTNET_INSTALL_DIR\dotnet"

function DotNetRestore {
    param([string]$Project)
    & $dotnet restore $Project --verbosity minimal
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet restore failed with exit code $LASTEXITCODE"
    }
}

function DotNetBuild {
    param([string]$Project, [string]$Configuration, [string]$VersionSuffix)
    if ($VersionSuffix) {
        & $dotnet build $Project --output $OutputPath --framework $framework --configuration $Configuration --version-suffix "$VersionSuffix"
    } else {
        & $dotnet build $Project --output $OutputPath --framework $framework --configuration $Configuration
    }
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet build failed with exit code $LASTEXITCODE"
    }
}

function DotNetTest {
    param([string]$Project)
    & $dotnet test $Project --output $OutputPath --framework $framework --no-build
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet test failed with exit code $LASTEXITCODE"
    }
}

if ($PatchVersion -eq $true) {

    $gitRevision = (git rev-parse HEAD | Out-String).Trim()
    $gitBranch   = (git rev-parse --abbrev-ref HEAD | Out-String).Trim()
    $timestamp   = [DateTime]::UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK")

    $assemblyVersion = Get-Content ".\AssemblyVersion.cs" -Raw
    $assemblyVersionWithMetadata = "{0}using System.Reflection;`r`n`r`n[assembly: AssemblyMetadata(""CommitHash"", ""{1}"")]`r`n[assembly: AssemblyMetadata(""CommitBranch"", ""{2}"")]`r`n[assembly: AssemblyMetadata(""BuildTimestamp"", ""{3}"")]" -f $assemblyVersion, $gitRevision, $gitBranch, $timestamp

    Set-Content ".\AssemblyVersion.cs" $assemblyVersionWithMetadata -Encoding utf8
}

$projects = @(
    (Join-Path $solutionPath "src\ProjectEuler\ProjectEuler.csproj"),
    (Join-Path $solutionPath "tests\ProjectEuler.Tests\ProjectEuler.Tests.csproj")
)

$testProjects = @(
    (Join-Path $solutionPath "tests\ProjectEuler.Tests\ProjectEuler.Tests.csproj")
)

if ($RestorePackages -eq $true) {
    Write-Host "Restoring NuGet packages for $($projects.Count) projects..." -ForegroundColor Green
    ForEach ($project in $projects) {
        DotNetRestore $project
    }
}

Write-Host "Building $($projects.Count) projects..." -ForegroundColor Green
ForEach ($project in $projects) {
    DotNetBuild $project $Configuration $PrereleaseSuffix
}

if ($RunTests -eq $true) {
    Write-Host "Testing $($testProjects.Count) project(s)..." -ForegroundColor Green
    ForEach ($project in $testProjects) {
        DotNetTest $project
    }
}

if ($PatchVersion -eq $true) {
    Set-Content ".\AssemblyVersion.cs" $assemblyVersion -Encoding utf8
}
