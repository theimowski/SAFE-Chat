#r "paket: groupref build //"
#load "./.fake/build.fsx/intellisense.fsx"

#if !FAKE
#r "netstandard"
#r "Facades/netstandard" // https://github.com/ionide/ionide-vscode-fsharp/issues/839#issuecomment-396296095
#endif

open Fake.Core

open SAFE.Build

let SAFE = SAFEBuild (fun x ->
    { x with
        JsDeps = Yarn
    } )

Target.create "Clean" (fun _ ->
    SAFE.CleanBuildDirs ()
)

Target.create "InstallClient" (fun _ ->
    SAFE.RestoreClient ()
)

Target.create "Build" (fun _ ->
    SAFE.BuildServer ()
    SAFE.BuildClient ()
)

Target.create "Run" (fun _ ->
    [ SAFE.RunServer; SAFE.RunClient; SAFE.RunBrowser ]
    |> SAFE.RunInParallel
)

open Fake.Core.TargetOperators

"Clean"
    ==> "InstallClient"
    ==> "Build"

"InstallClient"
    ==> "Run"

Target.runOrDefault "Build"
