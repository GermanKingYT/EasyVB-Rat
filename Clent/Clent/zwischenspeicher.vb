'//////////////////////////////////////////
'//////////////////////////////////////////
'///Die Datei ist nur zum Code Anschauen///
'//////////////////////////////////////////
'//////////////////////////////////////////

Imports System.Net.Sockets
Imports System.IO
Imports System.Text


Public Class Form1zw
    Dim networkStream As NetworkStream

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim tcpClnt As New System.Net.Sockets.TcpClient
        Dim ip As String = "localhost"
        Dim port As Integer = 501
        Try
            tcpClnt.Connect(ip, port)
            networkStream = tcpClnt.GetStream()
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim networkStream As NetworkStream
        Dim fs As FileStream
        fs = New FileStream("D:\Users\Peter\AppData\Roaming\Untitled.png", FileMode.Open)
        Dim objReader As New BinaryReader(fs)
        Dim send() As Byte = objReader.ReadBytes(fs.Length)
        networkStream.Write(send, 0, send.Length)
        objReader.Close()
        fs.Close()
    End Sub
End Class


Module Module1zw

    Sub Main()
        Const portNumber As Integer = 501
        Dim tcpListener As New TcpListener(portNumber)
        Dim tcpClient As TcpClient
        tcpListener.Start()
        Console.WriteLine("Listening...")

        Try
            tcpClient = tcpListener.AcceptTcpClient()
            Console.WriteLine("Connection accepted.")
            Dim networkStream As NetworkStream = tcpClient.GetStream()
            Dim bytes(tcpClient.ReceiveBufferSize) As Byte
            networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize))
            Dim clientdata As String = Encoding.ASCII.GetString(bytes)
            Dim fi As New FileInfo("D:\Users\Peter\AppData\Untitled.png")
            Dim sw As StreamWriter = fi.CreateText()
            sw.Write(clientdata)
            sw.Close()
            Console.WriteLine("Message received")
        Catch e As Exception
            Console.WriteLine(e.ToString())
            Console.ReadLine()
        End Try

        Console.ReadLine()
        tcpClient.Close()
        tcpListener.Stop()
        Console.WriteLine("Connection closed")
        Console.ReadLine()

    End Sub


End Module