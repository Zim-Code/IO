$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\IO\bin\Release\ZimCode.IO.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\NuGet\ZimCode.IO.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\NuGet\ZimCode.IO.compiled.nuspec

& $root\NuGet\NuGet.exe pack $root\NuGet\ZimCode.IO.compiled.nuspec

& $root\NuGet\NuGet.exe setapikey $variables.nuget_key
& $root\NuGet\NuGet.exe push $root\NuGet\*.nupkg