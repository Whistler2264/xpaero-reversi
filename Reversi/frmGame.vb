
Public Class frmGame
    Public WithEvents gameEngine As New clsGameEngine
    Public newGame As Boolean = False

    Private Sub mnuPlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Procedure through which we carry out the launch of the game: 

        'First we post the settings page.
        frmSetting.ShowDialog()

        If newGame = False Then Exit Sub

        'Then we sizes the game form to the size of the board.
        dimForm()

        newGame = False

        'And at last we launches the game if the player is offline (no need to await connection of the opponent).
        If gameEngine.typePartie = clsGameEngine.gameType.localEasy Or gameEngine.typePartie = clsGameEngine.gameType.localHard Then
            gameEngine.startIaGame()
        Else
            'Otherwise, we prepare the graphic interface for Play in remote and await the connection to begin.
            Me.StatusStrip1.Refresh()

        End If
    End Sub

    Private Sub dimForm()
        'Sizes the form according to the parameters of the game and place controls.
        Me.Size = New Size((clsGameEngine.CaseSize + clsGameEngine.MarginSize) * gameEngine.gameSize + 6, (clsGameEngine.CaseSize + clsGameEngine.MarginSize) * gameEngine.gameSize + Me.MenuStrip1.Height + Me.StatusStrip1.Height + 26)

    End Sub


    Private Sub picGame_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picGame.MouseClick
        'This procedure manages when the player plays a pawn in the game.

        If (gameEngine.MyTurn And gameEngine.CanPlay) = True Then
            Dim X As Short, Y As Short

            X = Math.Truncate(e.X / (clsGameEngine.CaseSize + clsGameEngine.MarginSize))
            Y = Math.Truncate(e.Y / (clsGameEngine.CaseSize + clsGameEngine.MarginSize))

            If X < gameEngine.gameSize And Y < gameEngine.gameSize Then
                gameEngine.play(X, Y, True)
            End If

        End If

    End Sub

    Private Sub gameUpdate(ByVal bit As Bitmap) Handles gameEngine.boardUpdated
        'Procdure that puts the interface user according to the gameEngine setup;
        'the board (in the picbox) and scores (in the status strip) are posted.
        picGame.Image = bit
        picGame.Refresh()
        Me.lblScore.Text = gameEngine.pawnsPlayer.ToString
        Me.lblSCoreAdv.Text = gameEngine.pawnsOpponent.ToString()

        Me.StatusStrip1.Refresh()

    End Sub


    Private Sub frmGame_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PictureBox.CheckForIllegalCrossThreadCalls = False
    End Sub

    Declare Unicode Function ShellAboutW Lib "shell32.dll" Alias "ShellAboutW" _
        (ByVal hWnd As IntPtr, ByVal szApp As String, ByVal szOtherStuff As String, ByVal hIcon As IntPtr) As Integer

    Private Sub AppropriateStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        ShowAboutBox()
    End Sub

    Sub ShowAboutBox()
        Dim myIcon As Icon = Me.Icon
        ShellAboutW(Me.Handle, Me.ProductName, "", myIcon.Handle)
    End Sub

    Private Sub QuitterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        gameEngine = Nothing
        Application.Exit()
    End Sub

    Private Sub NewGameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub NewGameToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewGameToolStripMenuItem1.Click
        'Procedure through which we carry out the launch of the game: 

        'First we post the settings page.
        frmSetting.ShowDialog()

        If newGame = False Then Exit Sub

        'Then we sizes the game form to the size of the board.
        dimForm()

        newGame = False

        'And at last we launches the game if the player is offline (no need to await connection of the opponent).
        If gameEngine.typePartie = clsGameEngine.gameType.localEasy Or gameEngine.typePartie = clsGameEngine.gameType.localHard Then
            gameEngine.startIaGame()
        ElseIf gameEngine.typePartie = clsGameEngine.gameType.localEasy And frmSetting.CheckBox1.Checked = True Or gameEngine.typePartie = clsGameEngine.gameType.localHard And frmSetting.CheckBox1.Checked = True Then
        Else
            'Otherwise, we prepare the graphic interface for Play in remote and await the connection to begin.
            Me.StatusStrip1.Refresh()

        End If

    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            ' Close the application when the Esc key is pressed
            Me.Close()
        ElseIf e.KeyCode = Keys.F2 Then
            'Procedure through which we carry out the launch of the game: 

            'First we post the settings page.
            frmSetting.ShowDialog()

            If newGame = False Then Exit Sub

            'Then we sizes the game form to the size of the board.
            dimForm()

            newGame = False

            'And at last we launches the game if the player is offline (no need to await connection of the opponent).
            If gameEngine.typePartie = clsGameEngine.gameType.localEasy Or gameEngine.typePartie = clsGameEngine.gameType.localHard Then
                gameEngine.startIaGame()
            Else
                'Otherwise, we prepare the graphic interface for Play in remote and await the connection to begin.
                Me.StatusStrip1.Refresh()

            End If
        End If
    End Sub
End Class
