' 82.65.107.76
Imports System.Net.Sockets
Imports System.net
Imports System.io
Imports System.Text

Public Class clsRemoteReader
    'classe interne utilis�e uniquement pour le remote engine servant � �tablir la connexion 
    'et recevoir les donn�es.

    Public mainSocket As Socket = Nothing  'socket utilis� dans les �changes
    Public Const bufferSize As Integer = 1024 'taille du buffer m�moire
    Public buffer(1024) As Byte 'buffer m�moire pour la rec�ption de donn�es
    Public sb As New StringBuilder 'stringBuilder o� sont stock�e les donn�es pour analyse

End Class 'clsRemoteReader

Public Class remoteEngine

#Region "d�clarations"
    Private _HostEndPoint As IPEndPoint = New IPEndPoint(CType(Dns.GetHostByName(Dns.GetHostName).AddressList.GetValue(0), IPAddress), 5200) 'endpoint de l'host
    Private _remoteEndPoint As IPEndPoint = Nothing 'endpoint de la personne voulant �tablir une connexion
    Private _reader As New clsRemoteReader 'objet de connexion, de reception de donn�es ..
    Private _port As Integer = 5200  'port de connexion


#End Region

    Public Event dataReceived(ByVal data As String) '�v�nement utilis� dans le gameEngine pour traiter
    'les donn�es re�ues
    Public Event connected() '�v�nement utilis� pour savoir lorsque la connexion est �tablie

    Public ReadOnly Property HostEndPoint() As IPEndPoint
        'propri�t� d'acc�s a l'IPEndPoint de l'hote, c'est � dire au point 
        'de terminaison r�seau correspondant a l'h�te local

        Get
            Return _HostEndPoint
        End Get


    End Property 'hostEndPoint

    Public Property RemoteEndPoint() As IPEndPoint
        'propri�t� d'acc�s a l'IPEndPoint du client, c'est � dire au point 
        'de terminaison r�seau correspondant au client internet
        Get
            Return _remoteEndPoint
        End Get
        Set(ByVal value As IPEndPoint)
            _remoteEndPoint = value
        End Set
    End Property 'remoteEndPoint

    'Public Function getIpWeb() As IPAddress
    '    'function qui r�cup�re l'ip internet d'un ordinateur
    '    'en se connectant a un site qui ne renvoie QUE cette IP via un script

    '    Dim web As New WebClient 'le webclient utilis� pour r�cup�rer l'information
    '    Dim reader As New StreamReader(web.OpenRead("http://www.whatismyip.com/automation/n09230945.asp"))
    '    Dim incData As New StringBuilder

    '    While reader.Peek > 0
    '        incData.Append(reader.ReadLine)
    '    End While
    '    reader.Close()

    '    Return IPAddress.Parse(incData.ToString)

    'End Function 'getIpWeb

    Public Sub connectTo(ByVal remoteEP As IPEndPoint)
        'proc�dure qui lance la connexion asynchrone, pour un client

        If _reader.mainSocket IsNot Nothing Then
            If _reader.mainSocket.Connected Then
                'si une connexion est d�j� �tablie via l'objet de connexion, on quitte
                MessageBox.Show("D�j� connect� !")
                Exit Sub
            End If
        End If

        'sinon, on lance la connexion asynchrone sur le socket de l'objet de connexion
        _reader.mainSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        _reader.mainSocket.BeginConnect(remoteEP, New AsyncCallback(AddressOf connectCallback), _reader.mainSocket)

    End Sub ' connectTo

    Public Sub waitConnection()
        'proc�dure qui lance une attente de connexion asynchrone c�t� h�te

        If _reader.mainSocket IsNot Nothing Then
            If _reader.mainSocket.Connected Then
                'si une connexion est d�j� �tablie via l'objet de connexion, on quitte
                MessageBox.Show("D�j� connect� !")
                Exit Sub
            End If
        End If

        'sinon on se met en attente de connexion sur le socket de l'objet de connexion
        _reader.mainSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        _reader.mainSocket.Bind(HostEndPoint)
        _reader.mainSocket.Listen(1)

        _reader.mainSocket.BeginAccept(New AsyncCallback(AddressOf acceptCallBack), _reader.mainSocket)


    End Sub 'waitConnection

    Private Sub acceptCallBack(ByVal obj As IAsyncResult)
        'proc�dure qui termine la connexion de mani�re asynchrone, lorsque le PC local
        'est en attente de connexion
        Try
            _reader.mainSocket = CType(obj.AsyncState, Socket).EndAccept(obj)
            RemoteEndPoint = CType(_reader.mainSocket.RemoteEndPoint, IPEndPoint)

        Catch ex As Exception
            MessageBox.Show("La connexion du client a �chou�e.")
            _reader.mainSocket = Nothing
            Exit Sub
        End Try

        RaiseEvent connected()

        _reader.mainSocket.BeginReceive(_reader.buffer, 0, clsRemoteReader.bufferSize, 0, New AsyncCallback(AddressOf readCallBack), _reader)

    End Sub 'acceptCallBack

    Private Sub connectCallback(ByVal obj As IAsyncResult)
        'proc�dure qui termine la connexion de mani�re asynchrone, lorsque le PC local
        'est en demande de connexion

        Try
            _reader.mainSocket = CType(obj.AsyncState, Socket)
            'on r�cup�re le socket pass� en param�tre auparavant, et on �tablit la connexion
            _reader.mainSocket.EndConnect(obj)
        Catch ex As Exception
            MessageBox.Show("La connexion � l'h�te a �chou�e.")
            _reader.mainSocket = Nothing
            Exit Sub
        End Try

        RaiseEvent connected()

        _reader.mainSocket.BeginReceive(_reader.buffer, 0, clsRemoteReader.bufferSize, 0, New AsyncCallback(AddressOf readCallBack), _reader)


    End Sub 'connectCallBack

    Private Sub readCallBack(ByVal obj As IAsyncResult)
        Try

            Dim state As clsRemoteReader = CType(obj.AsyncState, clsRemoteReader)
            Dim bytesInc As Integer = state.mainSocket.EndReceive(obj)

            Dim data As String = String.Empty
            Dim tempData As String = String.Empty
            Dim index As Integer

            If bytesInc > 0 Then


                tempData = Encoding.UTF8.GetString(state.buffer, 0, bytesInc)

                Array.Clear(state.buffer, 0, state.buffer.Length)

                index = tempData.IndexOf("<end>")

                If index > -1 Then
                    data = state.sb.ToString() + Mid(tempData, 1, index)

                    state.sb.Remove(0, state.sb.Length)

                    If tempData.Length > index + 5 Then
                        state.sb.Append(Mid(tempData, index + 5, tempData.Length))
                    End If

                    RaiseEvent dataReceived(data)

                    state.mainSocket.BeginReceive(state.buffer, 0, clsRemoteReader.bufferSize, 0, New AsyncCallback(AddressOf readCallBack), state)
                Else
                    state.sb.Append(data)

                    state.mainSocket.BeginReceive(state.buffer, 0, clsRemoteReader.bufferSize, 0, New AsyncCallback(AddressOf readCallBack), state)
                End If


            End If
        Catch ex As Exception

            MessageBox.Show("Erreur lors de la r�ception de donn�es")

        End Try

    End Sub 'readCallBack

    Public Sub send(ByVal data As String)
        If _reader.mainSocket.Connected Then
            Dim bytes() As Byte = Encoding.UTF8.GetBytes(data)
            _reader.mainSocket.BeginSend(bytes, 0, bytes.Length, 0, New AsyncCallback(AddressOf sendCallBack), _reader.mainSocket)
        End If
    End Sub

    Private Sub sendCallBack(ByVal obj As IAsyncResult)
        Dim handler As Socket = CType(obj.AsyncState, Socket)
        handler.EndSend(obj)
    End Sub

End Class
