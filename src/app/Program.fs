open System.IO // Provides stuff for IO operations like reading/writing files

// Common functions to be used contained in a class
type Functions =
    /// This function will read the lines of a file and return a specific line
    static member GetLine(file: string) (line: int) =
        let file_seq = File.ReadLines(file) // Create sequence from lines in file
        file_seq |> Seq.cast<string> |> Seq.item(line) // Get element

    /// This function will read a file and return contents after trimming newlines
    static member Read(file: string) =
        File.ReadAllText(file).TrimEnd('\r', '\n')

    static member Split(line: string) (delimiter: string) (element: int) =
        let line_seq = line.Split delimiter // Split line into sequence using the the delimiter
        line_seq |> Seq.cast<string> |> Seq.item(element) // Get the specified element

    static member Trim(line: string) (rem: char) =
        line.TrimStart(rem).TrimEnd(rem)

// Actual information gathering functions contained in a class
type Info =
    static member CPU =
        let line = Functions.GetLine "/proc/cpuinfo" 4 // 5th line
        let line = Functions.Split line ":" 1
        line.TrimStart(' ')

    /// This function will output the distro by parsing `/etc/os-release`
    static member Distro =
        let line = Functions.GetLine "/etc/os-release" 2 // Third line
        let line = Functions.Split line "=" 1 // Split line based on "=", grab the second element
        Functions.Trim line '"'

    /// This function will return the contents of an environmental variable and trim any newlines from it
    static member Env(var: string) =
        System.Environment.GetEnvironmentVariable(var).TrimEnd('\r', '\n')

    static member Music =
        // Currently uses rsmpc for music info until I find a find an mpd module for F# or something
        let n=
            new System.Diagnostics.Process(
                StartInfo=System.Diagnostics.ProcessStartInfo(
                    RedirectStandardOutput=true,RedirectStandardError=true,UseShellExecute=false, FileName= @"rsmpc",Arguments="current"
                )
            )
        n.Start() |> ignore // |>ignore used to silence dotnet warning
        let status = ((n.StandardOutput).ReadToEnd()).TrimEnd('\r', '\n') // Assign the value of n's standard output to status
        n.Close()
        status // Return status

// Gather info and store into variables
let cpu      = Info.CPU
let distro   = Info.Distro
let hostname = Functions.Read "/etc/hostname"
let kernel   = Functions.Read "/proc/sys/kernel/osrelease"
let shell    = Info.Env "SHELL"
let user     = Info.Env "USER"
let music    = Info.Music

// Print output
printfn 
  "CPU:      %s
Distro:   %s
Hostname: %s
Kernel:   %s
Shell:    %s
User:     %s
Music:    %s" 
  cpu distro hostname kernel shell user music
