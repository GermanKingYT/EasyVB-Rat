Imports System.Threading
Imports System.Net.Sockets
Imports System.IO

Public Class UploadClient

    Public Sub UploadFile(ByVal port As Integer, ByVal file As String, ByVal ip As String)
        Dim obj(3) As Object
        obj(0) = port
        obj(1) = file
        obj(2) = ip
        Dim t As New Thread(AddressOf DoUploadFile)
        t.Start(obj)
    End Sub

    Public Sub DoUploadFile(ByVal obj() As Object)
        Dim port As Integer = CInt(obj(0))
        Dim file As String = obj(1).ToString()


        Dim networkStream As NetworkStream
        Dim tcpClnt As New System.Net.Sockets.TcpClient
        Dim ip As String = obj(2).ToString()
        Try
            tcpClnt.Connect(ip, port)
            networkStream = tcpClnt.GetStream()
        Catch ex As Exception
#If DEBUG Then
            MsgBox(ex.ToString)
#End If
        End Try
        Dim fs As FileStream
        fs = New FileStream(file, FileMode.Open)
        Dim objReader As New BinaryReader(fs)
        Dim send() As Byte = objReader.ReadBytes(fs.Length)
        networkStream.Write(send, 0, send.Length)
        objReader.Close()
        fs.Close()

    End Sub

End Class
