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

    Const BufferSize = 1024
    Public Sub DoUploadFile(ByVal obj() As Object)
        Dim port As Integer = CInt(obj(0))
        Dim file As String = obj(1).ToString()


        Dim tcpClnt As New System.Net.Sockets.TcpClient
        Dim ip As String = obj(2).ToString()
        Dim bw As BinaryWriter
        Try
            tcpClnt.Connect(ip, port)
            bw = New BinaryWriter(tcpClnt.GetStream())
        Catch ex As Exception
#If DEBUG Then
            MsgBox(ex.ToString)
#End If
        End Try
        'Dim fs As FileStream
        'f() 's = New FileStream(file, FileMode.Open)
        ' Dim objReader As New BinaryReader(fs)
        Dim fs As New FileStream(file, FileMode.Open, FileAccess.Read)
        Dim packets = Convert.ToInt32(Math.Ceiling(fs.Length / BufferSize))
        Dim remainingBytes = CInt(fs.Length)
        bw.Write(remainingBytes)
        'Thread.Sleep(100)
        Dim fi As New FileInfo(file)
        bw.Write(fi.Name)
        'Thread.Sleep(100)
        For i = 0 To packets - 1
            Dim currentPacketLength As Integer
            If remainingBytes > BufferSize Then
                currentPacketLength = BufferSize
                remainingBytes -= currentPacketLength
            Else
                currentPacketLength = remainingBytes
                remainingBytes = 0
            End If

            Dim sendingBuffer = New Byte(currentPacketLength - 1) {}
            fs.Read(sendingBuffer, 0, currentPacketLength)
            bw.Write(sendingBuffer, 0, sendingBuffer.Length)
            'Thread.Sleep(100)
            tcpClnt.Close()
        Next

    End Sub

End Class
