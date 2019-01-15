properties { 
  $ProductVersion = "4.1"
  $TargetFramework = "net-4.0"
  $BuildNumber = "0"
} 

$baseDir  = resolve-path .
$buildDir = "$baseDir\build" 
$toolsDir = "$baseDir\tools"
$outputDir = "$buildDir\output"
$artifactsDir = "$buildDir\artifacts"
$releaseDir = "$buildDir\release"
$nunitexec = "$toolsDir\NUnit-2.6.0.12051\nunit-console-x86.exe"
$zipExec = "$toolsDir\zip\7za.exe"
$nugetExec = "$toolsDir\nuget\nuget.exe"
include $toolsDir\psake\buildutils.ps1

task default -depends DoRelease

task DoRelease -depends GenerateAssemblyInfo, Test, CreateDeployPackages, CreateWebDeployPackages, ZipAndCopyToArtifacts, CreateNugetPackages{
	
}

task Clean{
	if(Test-Path $buildDir){
		Delete-Directory $buildDir	
	}
}

task InitEnvironment{
	if($script:isEnvironmentInitialized -ne $true){
		if ($TargetFramework -eq "net-4.0"){
			$netfxInstallroot ="" 
			$netfxInstallroot =	Get-RegistryValue 'HKLM:\SOFTWARE\Microsoft\.NETFramework\' 'InstallRoot' 
			$netfxCurrent = $netfxInstallroot + "v4.0.30319"
			$script:msBuild = $netfxCurrent + "\msbuild.exe"
			
			echo ".Net 4.0 build requested - $script:msBuild" 

			$script:msBuildTargetFramework ="/p:TargetFrameworkVersion=v4.0 /ToolsVersion:4.0"
			$script:nunitTargetFramework = "/framework=4.0";
			$script:isEnvironmentInitialized = $true
		}
	}
}

task Init -depends InitEnvironment, Clean, DetectOperatingSystemArchitecture {   	
	echo "Creating build directory at the follwing path $buildDir"
	Delete-Directory $buildDir
	Create-Directory $buildDir
	Delete-Directory $releaseDir
	Create-Directory $releaseDir
	Delete-Directory $artifactsDir
	Create-Directory $artifactsDir
	
	$script:Version = $ProductVersion + "." + $BuildNumber
	
	$currentDirectory = Resolve-Path .
	
	echo "Current Directory: $currentDirectory" 
 }

task GenerateAssemblyInfo{
	$assemblyInfoDirs = Get-ChildItem -path "$baseDir" -recurse -include "*.csproj" | % {
		$propDir = $_.DirectoryName + "\Properties"
		Create-Directory $propDir
		
		$version = "$ProductVersion.$BuildNumber.0"
		
		$nuspecfile = Get-ChildItem -Path $_.DirectoryName -Filter "*.nuspec"
		if($nuspecfile -ne $null){ 
			[xml]$content = Get-Content $nuspecfile.fullname
			$version = $content.package.metadata.version + ".0"
		}
		
		Generate-Assembly-Info `
		-file "$propDir\AssemblyInfo.cs" `
		-title "$name $version" `
		-description "" `
		-company "NobelProcera" `
		-product "$name $version" `
		-version $version `
		-copyright "NobelProcera" `
	}
}

task CompileMain -depends Init { 
	Delete-Directory $outputDir
	Create-Directory $outputDir

	Write-Host "Compiling version: $Version"

	$solutions = Get-ChildItem -path "$baseDir" -recurse -include *.sln -Exclude *nobuild.sln
	
	$solutions | % {
		$solutionFile = $_.FullName
		$solutionName = $_.BaseName
		$solutionDir = $_.Directorys
		$targetDir = "$outputDir\$solutionName\"
		
		Create-Directory $targetDir
		
		exec { &$script:msBuild $solutionFile /p:OutDir="$targetDir\" /p:Configuration=Release }
	}
}

task Test -depends CompileMain {	
	if(Test-Path $buildDir\TestReports){
		Delete-Directory $buildDir\TestReports
	}
	
	Create-Directory $buildDir\TestReports
	
	$testAssemblies = @()
	$testAssemblies += Get-ChildItem -path "$outputDir" -recurse -include *.Test.dll
	
	if($testAssemblies.count -eq 0){
		Write-Host "No test assemblies found!"
	} else{
		exec {&$nunitexec $testAssemblies $script:nunitTargetFramework /xml="$buildDir\TestReports\TestResults.xml" /noshadow /nologo } 
	}
	
}
 
task CreateDeployPackages{
	dir $outputDir -recurse -include *.deploy | %{
		$nuspec = $_.FullName
		$srcDir = $_.DirectoryName

		[xml]$nuspecXml = Get-Content $nuspec
		$name = $nuspecXml.package.metadata.id
		$version = "$ProductVersion.$BuildNumber"
		$targetDir = "$releaseDir\$name-$version"
		
		Delete-Directory $targetDir
		Create-Directory $targetDir 
				
		foreach ($file in $nuspecXml.package.files.file){
			$src = $file.GetAttribute("src")
			$target = $file.GetAttribute("target")
			$target = $target.Replace("lib\net40", "")
			$exclude = $file.GetAttribute("exclude").split(";")
			if($target.EndsWith("\"))
			{
				Write-Host "create target: $targetDir\$target"
				Create-Directory "$targetDir\$target"
			}
			Copy-Item  "$srcDir\$src" "$targetDir\$target" -recurse -force -exclude $exclude
		}
		
		Write-Host "Done: $name"
	}
}

task CreateWebDeployPackages{
	Get-ChildItem -path "$baseDir" -recurse -include _PublishedWebsites | % {
		$source = Get-ChildItem -path $_ -recurse | Select-Object -First 1
		$name = $source.BaseName
		$packageDir = "$releaseDir\$name-$Version"
		$exclude = ("Web.Debug.config","Web.Release.config")
		Create-Directory $packageDir
		foreach ($file in Get-ChildItem -path $source.FullName -exclude $exclude){
			Copy-Item $file $packageDir -recurse
		}
		
		Write-Host "Done: $name"
	}
}

task ZipAndCopyToArtifacts{
	dir $releaseDir | Where-Object { $_.PSIsContainer } | % {
		$packageName = $_.Name
		
		echo "creating archive for $packageName"
		
		$archive = "$artifactsDir\$packageName.zip"
		$packageDir = $_.FullName
		exec { &$zipExec a -tzip $archive $packageDir\** }
	}
}

task CreateNugetPackages{
	dir $outputDir -recurse -include *.nuspec | % {
		$nuspecfile = $_.FullName
		$packageName = $_.Name
		Write-Host "Start: $packageName"
		[xml]$content = Get-Content $nuspecfile
		
		if($content.package.metadata.version.contains("0-BC")){
			echo "Will not generate package"
			return;
		}
		
		$packageVersion = $content.package.metadata.version
        exec { &$nugetExec pack $nuspecfile -OutputDirectory $artifactsDir -Version $packageVersion }
		Write-Host "Done: $packageName"
	}
}

task DetectOperatingSystemArchitecture {
	if (IsWow64 -eq $true)
	{
		$script:architecture = "x64"
	}
    echo "Machine Architecture is $script:architecture"
 }
