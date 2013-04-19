param($installPath, $toolsPath, $package, $project)

$defaultNamespace = $project.Properties.Item("DefaultNamespace").Value

$projectFolder = [System.IO.Path]::GetDirectoryName($project.FileName)
$path = Join-Path -Path $projectFolder -ChildPath "Instrumentation"
$files = [System.IO.Directory]::GetFiles($path)
foreach ($file in $files)
{
  $content = [System.IO.File]::ReadAllText($file)
  $content = $content.Replace("namespace NeedfulThings.PerformanceCounters", "namespace $defaultNamespace.Instrumentation")
  [System.IO.File]::WriteAllText($file, $content, [System.Text.Encoding]::UTF8)
}