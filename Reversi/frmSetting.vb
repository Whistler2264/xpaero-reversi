Public Class frmSetting

    Private Sub rdbLocal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbLocal.CheckedChanged
        grpbLocal.Enabled = rdbLocal.Checked
        grpbRemote.Enabled = Not rdbLocal.Checked
    End Sub

    Private Sub rdbRemote_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbRemote.CheckedChanged
        grpbRemote.Enabled = rdbRemote.Checked
        grpbLocal.Enabled = Not rdbRemote.Checked
    End Sub

    

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'cette procédure défini les options de jeu en fonction des choix utilisateurs et lance l'initialisation du jeu
        'dans le gameEngine
        Dim ipEP As System.Net.IPEndPoint = Nothing
        Dim typeGame As clsGameEngine.gameType

        If rdbLocal.Checked = True Then

            If rdbEasy.Checked = True Then
                typeGame = clsGameEngine.gameType.localEasy
            Else
                typeGame = clsGameEngine.gameType.localHard
            End If
        Else

            If rdbCon.Checked = True Then
                ipEP = New System.Net.IPEndPoint(System.Net.IPAddress.Parse(txtIp.Text), 5200)
                typeGame = clsGameEngine.gameType.remoteCon
            Else
                typeGame = clsGameEngine.gameType.remoteHost
            End If


        End If

        frmGame.gameEngine.initGame(typeGame, ipEP)

        frmGame.newGame = True

        Me.Close()

    End Sub

   

    Private Sub rdbHost_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbHost.CheckedChanged
        Me.txtIp.Enabled = Not rdbHost.Enabled
    End Sub

    Private Sub rdbCon_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbCon.CheckedChanged
        Me.txtIp.Enabled = rdbCon.Enabled
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()

    End Sub

    Private Sub grpbLocal_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpbLocal.Enter

    End Sub

    Private Sub frmSetting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private Sub frmSetting_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

End Class