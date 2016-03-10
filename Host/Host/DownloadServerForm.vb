Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports System.Text


Public Class DownloadServerForm

    Private Sub DownloadServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not TextBox1.Text = "" Then
            ChangePanelColor(Color.Green)
            BackgroundWorker1.RunWorkerAsync()
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim port As Integer = CInt(TextBox1.Text)
            Dim l As New TcpListener(port)
            l.Start()
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

    Public Sub DownloadFilesServer(ByVal client As TcpClient)
        Dim filenamedia As New SaveFileDialog()
        Dim filename As String = ""
        While True
            If filenamedia.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If File.Exists(filenamedia.FileName) Then
                    MsgBox("Datei exestiert bereits!", MsgBoxStyle.Critical, "ERROR")
                Else
                    Exit While
                End If
            End If
        End While
        filename = filenamedia.FileName
        Try
            Dim lvi As ListViewItem = Me.Invoke(New AddListBoxItem_d(AddressOf AddListBoxItem), filename)
            Dim networkStream As NetworkStream = client.GetStream()
            Dim bytes(client.ReceiveBufferSize) As Byte
            networkStream.Read(bytes, 0, CInt(client.ReceiveBufferSize))
            Dim clientdata As String = Encoding.ASCII.GetString(bytes)
            Dim fi As New FileInfo(filename)
            Dim sw As StreamWriter = fi.CreateText()
            sw.Write(clientdata)
            sw.Close()
            Me.Invoke(New ChangeListBoxItem_d(AddressOf ChangeListBoxItem), lvi)
        Catch e As Exception

        End Try
    End Sub

    Public Delegate Function AddListBoxItem_d(ByVal filename As String) As ListViewItem
    Public Function AddListBoxItem(ByVal filename As String)
        Dim f As New IO.FileInfo(filename)
        Dim listviewitem As New ListViewItem(f.Name)
        listviewitem.SubItems.Add("downloading ...")
        Return ListView1.Items.Add(listviewitem)
    End Function

    Public Delegate Sub ChangeListBoxItem_d(ByVal filename As ListViewItem)
    Public Sub ChangeListBoxItem(ByVal filename As ListViewItem)
        filename.SubItems(0).Text = "finish"
    End Sub

    Public Delegate Sub ChangePanelColor_d(ByVal color As Color)
    Public Sub ChangePanelColor(ByVal color As Color)
        Panel1.BackColor = color
    End Sub

End Class