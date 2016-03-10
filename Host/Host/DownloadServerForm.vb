Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports System.Text


Public Class DownloadServerForm

    Dim DLF As String = ""
    Private Sub DownloadServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        While True
            Dim bfd As New FolderBrowserDialog
            Dim result As DialogResult = bfd.ShowDialog()
            If result = Windows.Forms.DialogResult.OK Then
                DLF = bfd.SelectedPath
                If Not TextBox1.Text = "" Then
                    ChangePanelColor(Color.Green)
                    BackgroundWorker1.RunWorkerAsync()
                End If
                Exit While
            ElseIf result = Windows.Forms.DialogResult.Cancel Then
                Me.Hide()
                Exit While
            End If
        End While
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim port As Integer = CInt(TextBox1.Text)
            Dim l As New TcpListener(port)
            l.Start()
            InfoBrief.PortDownloadServer = CInt(TextBox1.Text)
            TextBox1.Text = "" & CInt(TextBox1.Text)
            While True
                Dim client As TcpClient = l.AcceptTcpClient()
                Dim t As New Thread(AddressOf DownloadFilesServer)
                t.Start(client)
            End While
        Catch ex As Exception
            MsgBox("Downloadserver are crashed!", MsgBoxStyle.Critical, "ERROR")
#If DEBUG Then
            MsgBox(ex.ToString)
#End If
            Me.Invoke(New ChangePanelColor_d(AddressOf ChangePanelColor), Color.Red)
        End Try
    End Sub

    Const BufferSize = 1024
    Public Async Function DownloadFilesServer(ByVal client As TcpClient) As Task
        Dim filename As String = ""
        Try
            'Dim networkStream As NetworkStream = client.GetStream()
            'Dim bytes(client.ReceiveBufferSize) As Byte
            'networkStream.Read(bytes, 0, CInt(client.ReceiveBufferSize))
            'Dim clientdata As String = Encoding.ASCII.GetString(bytes)
            'Dim fi As New FileInfo(filename)
            'IO.File.WriteAllBytes(filename, bytes)
            'client.Close()
            Dim br As New BinaryReader(client.GetStream)
            Dim fileLength As Integer = 0
            fileLength = br.ReadInt32()
            Dim filen As String = ""
            filen = br.ReadString()
            Dim lvi As ListViewItem = Me.Invoke(New AddListBoxItem_d(AddressOf AddListBoxItem), filen)
            filename = DLF & "\" & filen
            If File.Exists(filename) Then
                Dim i As Integer = i
                While True
                    If File.Exists(filename & "." & i) Then
                    Else
                        filename = filename & "." & i
                        Exit While
                    End If
                End While
            Else
            End If

            Dim packets = Convert.ToInt32(Math.Ceiling(fileLength / BufferSize))
            Dim remainingBytes = fileLength
            Dim fs As New FileStream(filename, FileMode.Create, FileAccess.Write)

            For i = 0 To packets - 1
                Dim currentPacketLength As Integer
                If remainingBytes > BufferSize Then
                    currentPacketLength = BufferSize
                    remainingBytes -= currentPacketLength
                Else
                    currentPacketLength = remainingBytes
                    remainingBytes = 0
                End If
                Dim receivingBuffer = br.ReadBytes(currentPacketLength)
                fs.Write(receivingBuffer, 0, receivingBuffer.Length)
            Next

            Me.Invoke(New ChangeListBoxItem_d(AddressOf ChangeListBoxItem), lvi)
            'client.Close()
        Catch e As Exception
            MsgBox(e.ToString)
        End Try
    End Function

    Public Delegate Function GetSaveFileDialog_d() As Object()
    Public Function GetSaveFileDialog() As Object()
        Dim filenamedia As New SaveFileDialog()
        Dim obj(2) As Object
        obj(0) = filenamedia.ShowDialog()
        obj(1) = filenamedia.FileName
        Return obj
    End Function

    Public Delegate Function AddListBoxItem_d(ByVal filename As String) As ListViewItem
    Public Function AddListBoxItem(ByVal filename As String)
        'Dim f As New IO.FileInfo(filename)
        Dim listviewitem As New ListViewItem(filename)
        listviewitem.SubItems.Add("downloading ...")
        Return ListView1.Items.Add(listviewitem)
    End Function

    Public Delegate Sub ChangeListBoxItem_d(ByVal filename As ListViewItem)
    Public Sub ChangeListBoxItem(ByVal filename As ListViewItem)
        filename.SubItems(1).Text = "finish"
    End Sub

    Public Delegate Sub ChangePanelColor_d(ByVal color As Color)
    Public Sub ChangePanelColor(ByVal color As Color)
        Panel1.BackColor = color
    End Sub

End Class