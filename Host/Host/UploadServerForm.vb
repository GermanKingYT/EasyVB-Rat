Imports System.Net.Sockets
Imports System.IO
Imports System.Text
Imports System.Threading

Public Class UploadServerForm

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not TextBox1.Text = "" Then
            ChangePanelColor(Color.Green)
            BackgroundWorker1.RunWorkerAsync()
        End If
    End Sub

    Public Delegate Sub ChangePanelColor_d(ByVal color As Color)
    Public Sub ChangePanelColor(ByVal color As Color)
        Panel1.BackColor = color
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim port As Integer = CInt(TextBox1.Text)
            Dim l As New TcpListener(port)
            l.Start()
            While True
                Dim client As TcpClient = l.AcceptTcpClient()
                Dim t As New Thread(AddressOf UploadFilesServer)
                t.Start(client)
            End While
        Catch ex As Exception
            MsgBox("Uploadserver are crashed!", MsgBoxStyle.Critical, "ERROR")
#If DEBUG Then
            MsgBox(ex.ToString)
#End If
            Me.Invoke(New ChangePanelColor_d(AddressOf ChangePanelColor), Color.Red)
        End Try
    End Sub

    Public Sub UploadFilesServer(ByVal client As TcpClient)
        Dim filenamedia As New OpenFileDialog()
        Dim filename As String = ""
        While True
            If filenamedia.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If File.Exists(filenamedia.FileName) Then
                    Exit While
                Else
                    MsgBox("Datei exestiert nicht!", MsgBoxStyle.Critical, "ERROR")
                End If
            End If
        End While
        filename = filenamedia.FileName
        Try
            Dim networkStream As NetworkStream = client.GetStream()
            Dim fs As FileStream
            fs = New FileStream(filename, FileMode.Open)
            Dim objReader As New BinaryReader(fs)
            Dim send() As Byte = objReader.ReadBytes(fs.Length)
            networkStream.Write(send, 0, send.Length)
            objReader.Close()
            fs.Close()
        Catch e As Exception

        End Try
    End Sub

End Class