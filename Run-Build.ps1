properties { 
  $base_dir  = resolve-path .
  $build_dir = "$base_dir\build" 
  $buildartifacts_dir = "$build_dir\" 
  $sln_file = "$base_dir\FerroJson.sln"
  $testAssemblies = (Get-ChildItem "$base_dir" -Recurse -Include *Tests.dll -Name | Select-String "bin")
} 

task default -depends Test

task Clean { 
  remove-item -force -recurse $buildartifacts_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean { 
    new-item $buildartifacts_dir -itemType directory   
} 

task Compile -depends Init { 
  msbuild $sln_file /p:Configuration=Release /p:OutDir=""$buildartifacts_dir\signed ""  /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=""$base_dir\FerroJson.snk  ""
  msbuild $sln_file /p:Configuration=Release /p:OutDir=""$buildartifacts_dir\unsigned ""
  msbuild $sln_file /p:Configuration=Debug /p:OutDir=""$buildartifacts_dir\debug ""
} 

task Test -depends Compile {
 foreach($test_asm_name in $testAssemblies) {
  $file_name =  [System.IO.Path]::GetFileName($test_asm_name.ToString())
   nunit-console.exe $test_asm_name /nodots
 }
 if ($lastExitCode -ne 0) {
    throw "Error: Tests failed."
 }
}