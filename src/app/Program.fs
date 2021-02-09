open System.IO // Provides stuff for IO operations like reading/writing files

// Functions
let GetLine (file: string) (line: int) =
    let file_seq = File.ReadLines(file) // Create sequence from lines in file
    file_seq |> Seq.cast<string> |> Seq.item(line) // Get element

let Distro =
    let line = GetLine "/etc/os-release" 2
    let line_seq = line.Split "=" // Split line into sequence with '=' as the delimiter
    line_seq |> Seq.cast<string> |> Seq.item(1) // Get the second element

let Env (var: string) =
    System.Environment.GetEnvironmentVariable(var).TrimEnd('\r', '\n')

let Read (file: string) =
    File.ReadAllText(file).TrimEnd('\r', '\n')

// Gather info and store into variables
let distro   = Distro
let hostname = Read "/etc/hostname"
let kernel   = Read "/proc/sys/kernel/osrelease"
let shell    = Env "SHELL"
let user     = Env "USER"

// Print output
printfn "Distro:   %s\nHostname: %s\nKernel:   %s\nShell:    %s\nUser:     %s" distro hostname kernel shell user
