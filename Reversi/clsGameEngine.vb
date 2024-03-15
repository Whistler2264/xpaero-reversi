Imports System.Net
Imports System.Math
Public Class clsGameEngine

#Region "declarations"
    Private gameArray(_gameSize, _gameSize) As caseType  'Keep the board in memory.
    Private _gameField As Bitmap 'The picture of the board, as drawn on the screen.
    Private _gameGraph As Graphics 'The graphics to draw the board.
    Private WithEvents remEngine As New remoteEngine 'The asynchronous remote connection object.
    Private TypeOfGame As gameType 'The type of game being played (solo, double, etc.)
    Private Const _gameSize As Short = 11 'Number of moves on the board by line / column.
    '***  /!\ *** for 11, there are 12 moves (the picture begins with 0)***  /!\ ***
    'This board is already 12 x 12.... converting to larger should be easy.
    'Plus, the game AUTOMATICALLY sizes the board to the right size grid!
    Public Const CaseSize As Short = 32  'Size of one player game.
    Public Const MarginSize As Short = 3 'Margin between two player game.
    Private _GamePieces As Short  'The number of free spaces on the board.
    Private _MyTurn As Boolean = False
    Private _CanPlay As Boolean = False
    Private sep As Char = (",")
    Private WhtPawns As Short 'Variable containing the number of White pawns in the game.
    Private BlkPawns As Short 'Same thing for the Black pawns.

    Public Enum caseType As Short
        Know = 0
        White = 1
        Black = 2
        Player = 3
        Opponent = 4
        Both = 5
    End Enum

    Public Enum gameType As Short
        remoteHost = 0
        remoteCon = 1
        localEasy = 2
        localHard = 3
    End Enum



    Public Event boardUpdated(ByVal bmp As Bitmap)
#End Region



#Region "functions and sub of the game"
    Public Sub initGame(ByVal _TypeOfGame As gameType, Optional ByVal IPEndPt As IPEndPoint = Nothing)
        Randomize()

        'Initialization the basic parameters of the game.
        _GamePieces = (_gameSize + 1) * (_gameSize + 1)
        _gameField = Nothing
        _gameGraph = Nothing
        _gameField = New Bitmap(My.Resources.Board)
        _gameGraph = Graphics.FromImage(_gameField)

        WhtPawns = 0
        BlkPawns = 0

        For x As Integer = 0 To _gameSize
            For y As Integer = 0 To _gameSize
                gameArray(x, y) = caseType.Know
            Next y
        Next x

        TypeOfGame = _TypeOfGame

        If TypeOfGame = gameType.remoteCon Then
            'Put the four initial pieces.
            gameArray(Floor(_gameSize / 2), Floor(_gameSize / 2)) = caseType.Black
            gameArray(Ceiling(_gameSize / 2), Ceiling(_gameSize / 2)) = caseType.Black
            gameArray(Floor(_gameSize / 2), Ceiling(_gameSize / 2)) = caseType.White
            gameArray(Ceiling(_gameSize / 2), Floor(_gameSize / 2)) = caseType.White
        Else
            'Put the four initial pieces.
            gameArray(Floor(_gameSize / 2), Floor(_gameSize / 2)) = caseType.White
            gameArray(Ceiling(_gameSize / 2), Ceiling(_gameSize / 2)) = caseType.White
            gameArray(Floor(_gameSize / 2), Ceiling(_gameSize / 2)) = caseType.Black
            gameArray(Ceiling(_gameSize / 2), Floor(_gameSize / 2)) = caseType.Black
        End If

        'Record in memory the legal moves.
        FindGoodMoves(True)
        'Launch the connection (or the connection expectation) if the game is in remote.
        If TypeOfGame = gameType.remoteCon Then
            remEngine.connectTo(IPEndPt)
        ElseIf TypeOfGame = gameType.remoteHost Then
            remEngine.waitConnection()
        End If
    End Sub 'initGame

    Public Sub startIaGame()
        'Procedure for launching the game.
        _MyTurn = CInt(Rnd() * 2)
        _CanPlay = True
        drawGame()
        If _MyTurn = False Then
            IA()
        End If
    End Sub 'startIaGame

    Private Sub remoteConnected() Handles remEngine.connected
        If TypeOfGame = gameType.remoteHost Then
            _MyTurn = True
        End If
        _CanPlay = True
        drawGame()
        MessageBox.Show("Your oponnent is connected, the game can begin!", "Game Starting", MessageBoxButtons.OK)
    End Sub 'remoteConnected

    Private Sub FindGoodMoves(Optional ByVal init As Boolean = False)
        'This procedure enters into the game picture the valid pieces
        'for the local player and his opponent.
        'Although very long and repetitive, this code piece is rather simple to understand:
        'one verifies the valid moves in 8 directions surrounding the current piece,
        'while putting those moves (for both players) into memory.
        If (CanPlay Or init) = True Then
            Dim xPlayable As Short, yPlayable As Short 'Used to define the valid moves.
            For i As Integer = 0 To _gameSize
                For j As Integer = 0 To _gameSize
                    If gameArray(i, j) = caseType.Opponent Or gameArray(i, j) = caseType.Both Or gameArray(i, j) = caseType.Player Then
                        gameArray(i, j) = caseType.Know
                    End If
                Next
            Next
            xPlayable = -1
            yPlayable = -1

            'Traverses the whole board.
            For x As Integer = 0 To _gameSize
                For y As Integer = 0 To _gameSize
                    'Executes the code by checking available spots.
                    'If you're not at the right edge, then look for legal moves towards the right.
                    If x < _gameSize Then
                        If gameArray(x + 1, y) = caseType.Black And gameArray(x, y) = caseType.White Then
                            xPlayable = x + 1
                            While gameArray(xPlayable, y) = caseType.Black
                                If (xPlayable + 1) <= _gameSize Then
                                    xPlayable += 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, y) = caseType.White Or gameArray(xPlayable, y) = caseType.Black Then
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, y) = caseType.Opponent Or gameArray(xPlayable, y) = caseType.Both Then
                                    gameArray(xPlayable, y) = caseType.Both
                                Else
                                    gameArray(xPlayable, y) = caseType.Player
                                End If
                            End If
                        ElseIf gameArray(x + 1, y) = caseType.White And gameArray(x, y) = caseType.Black Then
                            xPlayable = x + 1
                            While gameArray(xPlayable, y) = caseType.White
                                If (xPlayable + 1) <= _gameSize Then
                                    xPlayable += 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, y) = caseType.White Or gameArray(xPlayable, y) = caseType.Black Then
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, y) = caseType.Player Or gameArray(xPlayable, y) = caseType.Both Then
                                    gameArray(xPlayable, y) = caseType.Both
                                Else
                                    gameArray(xPlayable, y) = caseType.Opponent
                                End If
                            End If
                        End If
                    End If

                    'If you're not at on the bottom edge, then look for legal moves towards the bottom.
                    If y < _gameSize Then
                        If gameArray(x, y + 1) = caseType.Black And gameArray(x, y) = caseType.White Then
                            yPlayable = y + 1
                            While gameArray(x, yPlayable) = caseType.Black
                                If (yPlayable + 1) <= _gameSize Then
                                    yPlayable += 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(x, yPlayable) = caseType.White Or gameArray(x, yPlayable) = caseType.Black Then
                                yPlayable = -1
                            Else
                                If gameArray(x, yPlayable) = caseType.Opponent Or gameArray(x, yPlayable) = caseType.Both Then
                                    gameArray(x, yPlayable) = caseType.Both
                                Else
                                    gameArray(x, yPlayable) = caseType.Player
                                End If
                            End If
                        ElseIf gameArray(x, y + 1) = caseType.White And gameArray(x, y) = caseType.Black Then
                            yPlayable = y + 1
                            While gameArray(x, yPlayable) = caseType.White
                                If (yPlayable + 1) <= _gameSize Then
                                    yPlayable += 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(x, yPlayable) = caseType.White Or gameArray(x, yPlayable) = caseType.Black Then
                                yPlayable = -1
                            Else
                                If gameArray(x, yPlayable) = caseType.Player Or gameArray(x, yPlayable) = caseType.Both Then
                                    gameArray(x, yPlayable) = caseType.Both
                                Else
                                    gameArray(x, yPlayable) = caseType.Opponent
                                End If
                            End If
                        End If
                    End If

                    'If you're not in the bottom right corner (which is on the right and bottom
                    'edges of the board), then look for legal moves diagonally down right.
                    If (x < _gameSize) And (y < _gameSize) Then
                        If gameArray(x + 1, y + 1) = caseType.Black And gameArray(x, y) = caseType.White Then
                            xPlayable = x + 1
                            yPlayable = y + 1
                            While gameArray(xPlayable, yPlayable) = caseType.Black
                                If (yPlayable + 1) <= _gameSize And (xPlayable + 1) <= _gameSize Then
                                    yPlayable += 1
                                    xPlayable += 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, yPlayable) = caseType.White Or gameArray(xPlayable, yPlayable) = caseType.Black Then
                                yPlayable = -1
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, yPlayable) = caseType.Opponent Or gameArray(xPlayable, yPlayable) = caseType.Both Then
                                    gameArray(xPlayable, yPlayable) = caseType.Both
                                Else
                                    gameArray(xPlayable, yPlayable) = caseType.Player
                                End If
                            End If
                        ElseIf gameArray(x + 1, y + 1) = caseType.White And gameArray(x, y) = caseType.Black Then
                            xPlayable = x + 1
                            yPlayable = y + 1
                            While gameArray(xPlayable, yPlayable) = caseType.White
                                If (yPlayable + 1) <= _gameSize And (xPlayable + 1) <= _gameSize Then
                                    yPlayable += 1
                                    xPlayable += 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, yPlayable) = caseType.White Or gameArray(xPlayable, yPlayable) = caseType.Black Then
                                yPlayable = -1
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, yPlayable) = caseType.Player Or gameArray(xPlayable, yPlayable) = caseType.Both Then
                                    gameArray(xPlayable, yPlayable) = caseType.Both
                                Else
                                    gameArray(xPlayable, yPlayable) = caseType.Opponent
                                End If
                            End If
                        End If
                    End If

                    'If you're not on the left edge of the board, then look for legal moves towards the left.
                    If x > 0 Then
                        If gameArray(x - 1, y) = caseType.Black And gameArray(x, y) = caseType.White Then
                            xPlayable = x - 1
                            While gameArray(xPlayable, y) = caseType.Black
                                If (xPlayable - 1) >= 0 Then
                                    xPlayable -= 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, y) = caseType.White Or gameArray(xPlayable, y) = caseType.Black Then
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, y) = caseType.Opponent Or gameArray(xPlayable, y) = caseType.Both Then
                                    gameArray(xPlayable, y) = caseType.Both
                                Else
                                    gameArray(xPlayable, y) = caseType.Player
                                End If
                            End If
                        ElseIf gameArray(x - 1, y) = caseType.White And gameArray(x, y) = caseType.Black Then
                            xPlayable = x - 1
                            While gameArray(xPlayable, y) = caseType.White
                                If (xPlayable - 1) >= 0 Then
                                    xPlayable -= 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, y) = caseType.White Or gameArray(xPlayable, y) = caseType.Black Then
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, y) = caseType.Player Or gameArray(xPlayable, y) = caseType.Both Then
                                    gameArray(xPlayable, y) = caseType.Both
                                Else
                                    gameArray(xPlayable, y) = caseType.Opponent
                                End If
                            End If
                        End If
                    End If

                    'If you're not at the top of the board, then look for legal moves towards the top.
                    If y > 0 Then
                        If gameArray(x, y - 1) = caseType.Black And gameArray(x, y) = caseType.White Then
                            yPlayable = y - 1
                            While gameArray(x, yPlayable) = caseType.Black
                                If (yPlayable - 1) >= 0 Then
                                    yPlayable -= 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(x, yPlayable) = caseType.White Or gameArray(x, yPlayable) = caseType.Black Then
                                yPlayable = -1
                            Else
                                If gameArray(x, yPlayable) = caseType.Opponent Or gameArray(x, yPlayable) = caseType.Both Then
                                    gameArray(x, yPlayable) = caseType.Both
                                Else
                                    gameArray(x, yPlayable) = caseType.Player
                                End If
                            End If
                        ElseIf gameArray(x, y - 1) = caseType.White And gameArray(x, y) = caseType.Black Then
                            yPlayable = y - 1
                            While gameArray(x, yPlayable) = caseType.White
                                If (yPlayable - 1) >= 0 Then
                                    yPlayable -= 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(x, yPlayable) = caseType.White Or gameArray(x, yPlayable) = caseType.Black Then
                                yPlayable = -1
                            Else
                                If gameArray(x, yPlayable) = caseType.Player Or gameArray(x, yPlayable) = caseType.Both Then
                                    gameArray(x, yPlayable) = caseType.Both
                                Else
                                    gameArray(x, yPlayable) = caseType.Opponent
                                End If
                            End If
                        End If
                    End If

                    'If you're not in the upper left corner, which puts you on both the
                    'left and top edge, then look for legal moves diagonally up left.
                    If (x > 0) And (y > 0) Then
                        If gameArray(x - 1, y - 1) = caseType.Black And gameArray(x, y) = caseType.White Then
                            xPlayable = x - 1
                            yPlayable = y - 1
                            While gameArray(xPlayable, yPlayable) = caseType.Black
                                If (yPlayable - 1) >= 0 And (xPlayable - 1) >= 0 Then
                                    yPlayable -= 1
                                    xPlayable -= 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, yPlayable) = caseType.White Or gameArray(xPlayable, yPlayable) = caseType.Black Then
                                yPlayable = -1
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, yPlayable) = caseType.Opponent Or gameArray(xPlayable, yPlayable) = caseType.Both Then
                                    gameArray(xPlayable, yPlayable) = caseType.Both
                                Else
                                    gameArray(xPlayable, yPlayable) = caseType.Player
                                End If
                            End If
                        ElseIf gameArray(x - 1, y - 1) = caseType.White And gameArray(x, y) = caseType.Black Then
                            xPlayable = x - 1
                            yPlayable = y - 1
                            While gameArray(xPlayable, yPlayable) = caseType.White
                                If (yPlayable - 1) >= 0 And (xPlayable - 1) >= 0 Then
                                    yPlayable -= 1
                                    xPlayable -= 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, yPlayable) = caseType.White Or gameArray(xPlayable, yPlayable) = caseType.Black Then
                                yPlayable = -1
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, yPlayable) = caseType.Player Or gameArray(xPlayable, yPlayable) = caseType.Both Then
                                    gameArray(xPlayable, yPlayable) = caseType.Both
                                Else
                                    gameArray(xPlayable, yPlayable) = caseType.Opponent
                                End If
                            End If
                        End If
                    End If

                    'If you're not in the upper right corner, which puts you on the top
                    'and right edge, then look for legal moves diagonally down left.
                    If (x < _gameSize) And (y > 0) Then
                        If gameArray(x + 1, y - 1) = caseType.Black And gameArray(x, y) = caseType.White Then
                            xPlayable = x + 1
                            yPlayable = y - 1
                            While gameArray(xPlayable, yPlayable) = caseType.Black
                                If (yPlayable - 1) >= 0 And (xPlayable + 1) <= _gameSize Then
                                    xPlayable += 1
                                    yPlayable -= 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, yPlayable) = caseType.White Or gameArray(xPlayable, yPlayable) = caseType.Black Then
                                yPlayable = -1
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, yPlayable) = caseType.Opponent Or gameArray(xPlayable, yPlayable) = caseType.Both Then
                                    gameArray(xPlayable, yPlayable) = caseType.Both
                                Else
                                    gameArray(xPlayable, yPlayable) = caseType.Player
                                End If
                            End If
                        ElseIf gameArray(x + 1, y - 1) = caseType.White And gameArray(x, y) = caseType.Black Then
                            xPlayable = x + 1
                            yPlayable = y - 1
                            While gameArray(xPlayable, yPlayable) = caseType.White
                                If (yPlayable - 1) >= 0 And (xPlayable + 1) <= _gameSize Then
                                    xPlayable += 1
                                    yPlayable -= 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, yPlayable) = caseType.White Or gameArray(xPlayable, yPlayable) = caseType.Black Then
                                yPlayable = -1
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, yPlayable) = caseType.Player Or gameArray(xPlayable, yPlayable) = caseType.Both Then
                                    gameArray(xPlayable, yPlayable) = caseType.Both
                                Else
                                    gameArray(xPlayable, yPlayable) = caseType.Opponent
                                End If
                            End If
                        End If
                    End If

                    'If you're not in the lower left corner, which puts you on the left and bottom edge,
                    'then look for legal moves diagonally up right.
                    If (x > 0) And (y < _gameSize) Then
                        If gameArray(x - 1, y + 1) = caseType.Black And gameArray(x, y) = caseType.White Then
                            xPlayable = x - 1
                            yPlayable = y + 1
                            While gameArray(xPlayable, yPlayable) = caseType.Black
                                If (yPlayable + 1) <= _gameSize And (xPlayable - 1) >= 0 Then
                                    xPlayable -= 1
                                    yPlayable += 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, yPlayable) = caseType.White Or gameArray(xPlayable, yPlayable) = caseType.Black Then
                                yPlayable = -1
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, yPlayable) = caseType.Opponent Or gameArray(xPlayable, yPlayable) = caseType.Both Then
                                    gameArray(xPlayable, yPlayable) = caseType.Both
                                Else
                                    gameArray(xPlayable, yPlayable) = caseType.Player
                                End If
                            End If
                        ElseIf gameArray(x - 1, y + 1) = caseType.White And gameArray(x, y) = caseType.Black Then
                            xPlayable = x - 1
                            yPlayable = y + 1
                            While gameArray(xPlayable, yPlayable) = caseType.White
                                If (yPlayable + 1) <= _gameSize And (xPlayable - 1) >= 0 Then
                                    xPlayable -= 1
                                    yPlayable += 1
                                Else : Exit While
                                End If
                            End While
                            If gameArray(xPlayable, yPlayable) = caseType.White Or gameArray(xPlayable, yPlayable) = caseType.Black Then
                                yPlayable = -1
                                xPlayable = -1
                            Else
                                If gameArray(xPlayable, yPlayable) = caseType.Player Or gameArray(xPlayable, yPlayable) = caseType.Both Then
                                    gameArray(xPlayable, yPlayable) = caseType.Both
                                Else
                                    gameArray(xPlayable, yPlayable) = caseType.Opponent
                                End If
                            End If
                        End If
                    End If
                Next y
            Next x
        End If
    End Sub 'FindGoodMoves

    Private Function drawGame() As Bitmap
        _gameGraph.DrawImage(My.Resources.board, New Rectangle(0, 0, My.Resources.board.Width, My.Resources.board.Height))
        For x As Integer = 0 To _gameSize
            For y As Integer = 0 To _gameSize
                Select Case gameArray(x, y)
                    Case caseType.Both, caseType.Player
                        _gameGraph.DrawImage(My.Resources.LegalMove, New Rectangle(x * (CaseSize + MarginSize), y * (CaseSize + MarginSize), CaseSize, CaseSize))
                    Case caseType.White
                        _gameGraph.DrawImage(My.Resources.WhitePawn, New Rectangle(x * (CaseSize + MarginSize), y * (CaseSize + MarginSize), CaseSize, CaseSize))
                    Case caseType.Black
                        _gameGraph.DrawImage(My.Resources.BlackPawn, New Rectangle(x * (CaseSize + MarginSize), y * (CaseSize + MarginSize), CaseSize, CaseSize))
                End Select
            Next y
        Next x
        RaiseEvent boardUpdated(_gameField)
        Return _gameField
    End Function 'drawGame

    Public Sub play(ByVal x As Short, ByVal y As Short, Optional ByVal LocalPlayer As Boolean = False)
        'If the game has started, executes this procedure.
        If _CanPlay = True Then
            'One verifies which player plays next.
            If LocalPlayer = True And _MyTurn = True Then
                'Verify that the piece was placed legally
                If (gameArray(x, y) = caseType.Player) Or (gameArray(x, y) = caseType.Both) Then
                    _GamePieces -= 1
                    'Changes the state of the spot to "legal," while changing to the color of the one who played (here the local player, therefore White)
                    gameArray(x, y) = caseType.White
                    Dim i As Short, j As Short
                    'Then one verifies in all the directions from the play to return the pawns that must be changed to the same color.
                    If x > 0 Then
                        'Verify first horizontally towards the left.
                        For i = (x - 1) To 0 Step -1
                            'If it we're finding Black pieces, then do nothing and continue until we no longer find Black pieces.
                            If gameArray(i, y) = caseType.Black Then
                                'If we find a White piece, then stop here and one go back along the path
                                'while transforming all in Black pieces between the two White ones.
                            ElseIf gameArray(i, y) = caseType.White Then
                                For j = i To x
                                    gameArray(j, y) = caseType.White
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If
                    If x < _gameSize Then
                        'Check horizontally towards the right.
                        For i = (x + 1) To _gameSize
                            If gameArray(i, y) = caseType.Black Then
                            ElseIf gameArray(i, y) = caseType.White Then
                                For j = x To i
                                    gameArray(j, y) = caseType.White
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If
                    If y > 0 Then
                        'Check vertically towards the top.
                        For i = (y - 1) To 0 Step -1
                            If gameArray(x, i) = caseType.Black Then
                            ElseIf gameArray(x, i) = caseType.White Then
                                For j = i To y
                                    gameArray(x, j) = caseType.White
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If
                    If y < _gameSize Then
                        'Check vertically towards the bottom.
                        For i = (y + 1) To _gameSize
                            If gameArray(x, i) = caseType.Black Then
                            ElseIf gameArray(x, i) = caseType.White Then
                                For j = y To i
                                    gameArray(x, j) = caseType.White
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    'Check diagonally up left.
                    i = x - 1
                    j = y - 1
                    While (i >= 0 And j >= 0)
                        If gameArray(i, j) = caseType.Black Then
                        ElseIf gameArray(i, j) = caseType.White Then
                            While (i <= x) And (j <= y)
                                gameArray(i, j) = caseType.White
                                i += 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j -= 1
                    End While

                    'Check diagonally down right.
                    i = x + 1
                    j = y + 1
                    While (i <= _gameSize And j <= _gameSize)
                        If gameArray(i, j) = caseType.Black Then
                        ElseIf gameArray(i, j) = caseType.White Then
                            While (i >= x) And (j >= y)
                                gameArray(i, j) = caseType.White
                                i -= 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j += 1
                    End While

                    'Check diagonally down left.
                    i = x - 1
                    j = y + 1
                    While (i >= 0 And j <= _gameSize)
                        If gameArray(i, j) = caseType.Black Then
                        ElseIf gameArray(i, j) = caseType.White Then
                            While (i <= x) And (j >= y)
                                gameArray(i, j) = caseType.White
                                i += 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j += 1
                    End While

                    'Check diagonally down right.
                    i = x + 1
                    j = y - 1
                    While (i <= _gameSize And j >= 0)
                        If gameArray(i, j) = caseType.Black Then
                        ElseIf gameArray(i, j) = caseType.White Then
                            While (x <= i) And (y >= j)
                                gameArray(i, j) = caseType.White
                                i -= 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j -= 1
                    End While

                    FindGoodMoves()
                    drawGame()
                    If TypeOfGame = gameType.remoteCon Or TypeOfGame = gameType.remoteHost Then
                        send(":Game:Play" & sep & x.ToString & ";" & y.ToString & sep & "<end>")
                    End If
                    _MyTurn = False
                End If
            ElseIf LocalPlayer = False And _MyTurn = False Then
                If (gameArray(x, y) = caseType.Opponent) Or (gameArray(x, y) = caseType.Both) Then
                    _GamePieces -= 1
                    'Changes the state of the spot to "legal," while changing to the color of the one who played (here the opponent, therefore Black)
                    gameArray(x, y) = caseType.Black
                    Dim i As Short, j As Short
                    'Then verify in all directions from the piece played to return the pawns that must be changed to that color.
                    If x > 0 Then
                        'Check first horizontally towards the left.
                        For i = (x - 1) To 0 Step -1
                            'If we're finding White pieces, then do nothing and continue until we no longer find Whites.
                            If gameArray(i, y) = caseType.White Then
                                'If we fina a Black piece, then stop here and go back along the path
                                'while transforming between the two Black pieces.
                            ElseIf gameArray(i, y) = caseType.Black Then
                                For j = i To x
                                    gameArray(j, y) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If x < _gameSize Then
                        'Check horizontally towards the right.
                        For i = (x + 1) To _gameSize
                            If gameArray(i, y) = caseType.White Then
                            ElseIf gameArray(i, y) = caseType.Black Then
                                For j = x To i
                                    gameArray(j, y) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If y > 0 Then
                        'Check vertically toward the top.
                        For i = (y - 1) To 0 Step -1
                            If gameArray(x, i) = caseType.White Then
                            ElseIf gameArray(x, i) = caseType.Black Then
                                For j = i To y
                                    gameArray(x, j) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If y < _gameSize Then
                        'Check vertically towards the bottom.
                        For i = (y + 1) To _gameSize
                            If gameArray(x, i) = caseType.White Then
                            ElseIf gameArray(x, i) = caseType.Black Then
                                For j = y To i
                                    gameArray(x, j) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    'Check diagonally down left.
                    i = x - 1
                    j = y - 1
                    While (i >= 0 And j >= 0)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            While (i <= x) And (j <= y)
                                gameArray(i, j) = caseType.Black
                                i += 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j -= 1
                    End While

                    'Check diagonally down right.
                    i = x + 1
                    j = y + 1
                    While (i <= _gameSize And j <= _gameSize)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            While (i >= x) And (j >= y)
                                gameArray(i, j) = caseType.Black
                                i -= 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j += 1
                    End While

                    'Check diagonally up right.
                    i = x - 1
                    j = y + 1
                    While (i >= 0 And j <= _gameSize)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            While (i <= x) And (j >= y)
                                gameArray(i, j) = caseType.Black
                                i += 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j += 1
                    End While

                    'Check diagnonally down left.
                    i = x + 1
                    j = y - 1
                    While (i <= _gameSize And j >= 0)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            While (x <= i) And (y >= j)
                                gameArray(i, j) = caseType.Black
                                i -= 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j -= 1
                    End While
                    _MyTurn = True
                End If
            End If

            'Once the move has been completed, make a new search for legal moves.
            FindGoodMoves()
            'Verify if the game is finished or tied.
            verifEnd()
            'Redraw the board.
            drawGame()

            If _MyTurn = False Then
                System.Threading.Thread.Sleep(1500)
                If frmSetting.CheckBox1.Checked = False Then
                    IA()
                End If

            End If
        End If
    End Sub 'play
    Public Sub play2(ByVal x As Short, ByVal y As Short, Optional ByVal LocalPlayer As Boolean = False)
        'If the game has started, executes this procedure.
        If _CanPlay = True Then
            'One verifies which player plays next.
            If LocalPlayer = True And _MyTurn = True Then
                'Verify that the piece was placed legally
                If (gameArray(x, y) = caseType.Player) Or (gameArray(x, y) = caseType.Both) Then
                    _GamePieces -= 1
                    'Changes the state of the spot to "legal," while changing to the color of the one who played (here the local player, therefore White)
                    gameArray(x, y) = caseType.White
                    Dim i As Short, j As Short
                    'Then one verifies in all the directions from the play to return the pawns that must be changed to the same color.
                    If x > 0 Then
                        'Verify first horizontally towards the left.
                        For i = (x - 1) To 0 Step -1
                            'If it we're finding Black pieces, then do nothing and continue until we no longer find Black pieces.
                            If gameArray(i, y) = caseType.Black Then
                                'If we find a White piece, then stop here and one go back along the path
                                'while transforming all in Black pieces between the two White ones.
                            ElseIf gameArray(i, y) = caseType.White Then
                                For j = i To x
                                    gameArray(j, y) = caseType.White
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If
                    If x < _gameSize Then
                        'Check horizontally towards the right.
                        For i = (x + 1) To _gameSize
                            If gameArray(i, y) = caseType.Black Then
                            ElseIf gameArray(i, y) = caseType.White Then
                                For j = x To i
                                    gameArray(j, y) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If
                    If y > 0 Then
                        'Check vertically towards the top.
                        For i = (y - 1) To 0 Step -1
                            If gameArray(x, i) = caseType.Black Then
                            ElseIf gameArray(x, i) = caseType.White Then
                                For j = i To y
                                    gameArray(x, j) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If
                    If y < _gameSize Then
                        'Check vertically towards the bottom.
                        For i = (y + 1) To _gameSize
                            If gameArray(x, i) = caseType.Black Then
                            ElseIf gameArray(x, i) = caseType.White Then
                                For j = y To i
                                    gameArray(x, j) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    'Check diagonally up left.
                    i = x - 1
                    j = y - 1
                    While (i >= 0 And j >= 0)
                        If gameArray(i, j) = caseType.Black Then
                        ElseIf gameArray(i, j) = caseType.White Then
                            While (i <= x) And (j <= y)
                                gameArray(i, j) = caseType.Black
                                i += 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j -= 1
                    End While

                    'Check diagonally down right.
                    i = x + 1
                    j = y + 1
                    While (i <= _gameSize And j <= _gameSize)
                        If gameArray(i, j) = caseType.Black Then
                        ElseIf gameArray(i, j) = caseType.White Then
                            While (i >= x) And (j >= y)
                                gameArray(i, j) = caseType.Black
                                i -= 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j += 1
                    End While

                    'Check diagonally down left.
                    i = x - 1
                    j = y + 1
                    While (i >= 0 And j <= _gameSize)
                        If gameArray(i, j) = caseType.Black Then
                        ElseIf gameArray(i, j) = caseType.White Then
                            While (i <= x) And (j >= y)
                                gameArray(i, j) = caseType.Black
                                i += 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j += 1
                    End While

                    'Check diagonally down right.
                    i = x + 1
                    j = y - 1
                    While (i <= _gameSize And j >= 0)
                        If gameArray(i, j) = caseType.Black Then
                        ElseIf gameArray(i, j) = caseType.White Then
                            While (x <= i) And (y >= j)
                                gameArray(i, j) = caseType.Black
                                i -= 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j -= 1
                    End While

                    FindGoodMoves()
                    drawGame()
                    If TypeOfGame = gameType.remoteCon Or TypeOfGame = gameType.remoteHost Then
                        send(":Game:Play" & sep & x.ToString & ";" & y.ToString & sep & "<end>")
                    End If
                    _MyTurn = False
                End If
            ElseIf LocalPlayer = False And _MyTurn = False Then
                If (gameArray(x, y) = caseType.Opponent) Or (gameArray(x, y) = caseType.Both) Then
                    _GamePieces -= 1
                    'Changes the state of the spot to "legal," while changing to the color of the one who played (here the opponent, therefore Black)
                    gameArray(x, y) = caseType.Black
                    Dim i As Short, j As Short
                    'Then verify in all directions from the piece played to return the pawns that must be changed to that color.
                    If x > 0 Then
                        'Check first horizontally towards the left.
                        For i = (x - 1) To 0 Step -1
                            'If we're finding White pieces, then do nothing and continue until we no longer find Whites.
                            If gameArray(i, y) = caseType.White Then
                                'If we fina a Black piece, then stop here and go back along the path
                                'while transforming between the two Black pieces.
                            ElseIf gameArray(i, y) = caseType.Black Then
                                For j = i To x
                                    gameArray(j, y) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If x < _gameSize Then
                        'Check horizontally towards the right.
                        For i = (x + 1) To _gameSize
                            If gameArray(i, y) = caseType.White Then
                            ElseIf gameArray(i, y) = caseType.Black Then
                                For j = x To i
                                    gameArray(j, y) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If y > 0 Then
                        'Check vertically toward the top.
                        For i = (y - 1) To 0 Step -1
                            If gameArray(x, i) = caseType.White Then
                            ElseIf gameArray(x, i) = caseType.Black Then
                                For j = i To y
                                    gameArray(x, j) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If y < _gameSize Then
                        'Check vertically towards the bottom.
                        For i = (y + 1) To _gameSize
                            If gameArray(x, i) = caseType.White Then
                            ElseIf gameArray(x, i) = caseType.Black Then
                                For j = y To i
                                    gameArray(x, j) = caseType.Black
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    'Check diagonally down left.
                    i = x - 1
                    j = y - 1
                    While (i >= 0 And j >= 0)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            While (i <= x) And (j <= y)
                                gameArray(i, j) = caseType.Black
                                i += 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j -= 1
                    End While

                    'Check diagonally down right.
                    i = x + 1
                    j = y + 1
                    While (i <= _gameSize And j <= _gameSize)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            While (i >= x) And (j >= y)
                                gameArray(i, j) = caseType.Black
                                i -= 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j += 1
                    End While

                    'Check diagonally up right.
                    i = x - 1
                    j = y + 1
                    While (i >= 0 And j <= _gameSize)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            While (i <= x) And (j >= y)
                                gameArray(i, j) = caseType.Black
                                i += 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j += 1
                    End While

                    'Check diagnonally down left.
                    i = x + 1
                    j = y - 1
                    While (i <= _gameSize And j >= 0)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            While (x <= i) And (y >= j)
                                gameArray(i, j) = caseType.Black
                                i -= 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j -= 1
                    End While
                    _MyTurn = True
                End If
            End If

            'Once the move has been completed, make a new search for legal moves.
            FindGoodMoves()
            'Verify if the game is finished or tied.
            verifEnd()
            'Redraw the board.
            drawGame()

            If _MyTurn = False Then
                System.Threading.Thread.Sleep(1500)
                If frmSetting.CheckBox1.Checked = False Then
                    IA()
                End If

            End If
        End If
    End Sub 'play
    Private Sub verifEnd()
        Dim isWhite As Boolean = False
        Dim isBlack As Boolean = False
        Dim isPlayer As Boolean = False
        Dim isOpponent As Boolean = False
        WhtPawns = 0
        BlkPawns = 0
        For i As Integer = 0 To _gameSize
            For j As Integer = 0 To _gameSize
                Select Case gameArray(i, j)
                    Case caseType.White
                        isWhite = True
                        WhtPawns += 1
                    Case caseType.Black
                        isBlack = True
                        BlkPawns += 1
                    Case caseType.Opponent
                        isOpponent = True
                    Case caseType.Player
                        isPlayer = True
                    Case caseType.Both
                        isOpponent = True
                        isPlayer = True
                End Select
            Next j
        Next i
        If GamePieces <= 0 Or isBlack = False Or isWhite = False Or (isOpponent = False And _MyTurn = False) Or (isPlayer = False And _MyTurn = True) Then
            _CanPlay = False
            If WhtPawns > BlkPawns Then
                MessageBox.Show("You won!  Final score:  " & WhtPawns.ToString & " to " & BlkPawns.ToString, "Game Over", MessageBoxButtons.OK)
            ElseIf WhtPawns < BlkPawns Then
                MessageBox.Show("You lose!  Final score:  " & WhtPawns.ToString & " to " & BlkPawns.ToString, "Game Over", MessageBoxButtons.OK)
            Else
                MessageBox.Show("The game is a tie! ", "Game Over", MessageBoxButtons.OK)
            End If
        End If
    End Sub 'verifEnd

    Private Sub dataReceive(ByVal data As String) Handles remEngine.dataReceived
        Dim parse() As String = data.Split(sep)
        Select Case parse(0)
            Case ":Game:Play"
                Dim parse2() As String = parse(1).Split(";")
                play(CInt(parse2(0)), CInt(parse2(1)), False)
        End Select
    End Sub 'dataReceive

    Private Sub send(ByVal data As String)
        remEngine.send(data)
    End Sub 'send
#End Region

#Region "properties"
    Public ReadOnly Property GamePieces()
        Get
            Return _GamePieces
        End Get
    End Property

    Public ReadOnly Property nombreCasesTotal()
        Get
            Return ((_gameSize + 1) ^ 2)
        End Get
    End Property

    Public ReadOnly Property gameSize()
        Get
            Return (_gameSize + 1)
        End Get
    End Property

    Public ReadOnly Property MyTurn() As Boolean
        Get
            Return _MyTurn
        End Get
    End Property

    Public ReadOnly Property CanPlay() As Boolean
        Get
            Return _CanPlay
        End Get
    End Property

    Public ReadOnly Property typePartie() As gameType
        Get
            Return TypeOfGame
        End Get
    End Property

    Public ReadOnly Property pawnsPlayer() As Short
        Get
            Return WhtPawns
        End Get
    End Property

    Public ReadOnly Property pawnsOpponent()
        Get
            Return BlkPawns
        End Get
    End Property
#End Region

#Region "IAs"
    Private Sub IA()
        'Play Level Easy:      Plays pieces randomly in the free spaces.
        'Play Level Difficult: Plays while selecting the edges, corners, and the "big" blows.
        Randomize()
        Dim tabHits() As Point = New Point() {}
        Dim count As Short = 0
        Dim rndPlay As Short
        For i As Integer = 0 To _gameSize
            For j As Integer = 0 To _gameSize
                If gameArray(i, j) = caseType.Opponent Or gameArray(i, j) = caseType.Both Then
                    ReDim Preserve tabHits(count)
                    tabHits(count) = New Point(i, j)
                    count += 1
                End If
            Next j
        Next i
        If count > 0 Then
            If TypeOfGame = gameType.localEasy Then
                rndPlay = CInt(Rnd() * (count - 1))
                play(tabHits(rndPlay).X, tabHits(rndPlay).Y, False)
            ElseIf TypeOfGame = gameType.localHard Then
                'Find the "big" blow.
                Dim x As Short
                Dim y As Short
                Dim tabMaxHits(count - 1) As Short
                Dim finalPlay As Short
                For k As Integer = 0 To count - 1
                    x = tabHits(k).X
                    y = tabHits(k).Y
                    If (x = 0 And y = 0) Or (x = 0 And y = _gameSize) Or (x = _gameSize And y = 0) Or (x = _gameSize And y = _gameSize) Then
                        play(tabHits(k).X, tabHits(k).Y, False)
                        Exit Sub
                    End If

                    Dim i As Short, j As Short
                    If x > 0 Then
                        'Verify first horizontally towards the left.
                        For i = (x - 1) To 0 Step -1
                            'If we're finding Black, then do nothing and continue until we no longer find Blacks.
                            If gameArray(i, y) = caseType.White Then '4
                                'If we find a White, then stop here and go back along the path
                                'while transforming between the two white pieces.
                            ElseIf gameArray(i, y) = caseType.Black Then
                                For j = i + 1 To x - 1
                                    tabMaxHits(k) += 1
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If x < _gameSize Then
                        'Check horizontally towards the right.
                        For i = (x + 1) To _gameSize
                            If gameArray(i, y) = caseType.White Then
                            ElseIf gameArray(i, y) = caseType.Black Then
                                For j = x + 1 To i - 1
                                    tabMaxHits(k) += 1
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If y > 0 Then
                        'Check vertically towards the top.
                        For i = (y - 1) To 0 Step -1
                            If gameArray(x, i) = caseType.White Then
                            ElseIf gameArray(x, i) = caseType.Black Then
                                For j = i + 1 To y - 1
                                    tabMaxHits(k) += 1
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    If y < _gameSize Then
                        'Check vertically towards the bottom.
                        For i = (y + 1) To _gameSize
                            If gameArray(x, i) = caseType.White Then
                            ElseIf gameArray(x, i) = caseType.Black Then
                                For j = y + 1 To i - 1
                                    tabMaxHits(k) += 1
                                Next
                                Exit For
                            Else : Exit For
                            End If
                        Next
                    End If

                    'Check diagonally up left.
                    i = x - 1
                    j = y - 1
                    While (i >= 0 And j >= 0)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            i += 1
                            j += 1
                            While (i <= (x - 1)) And (j <= (y - 1))
                                tabMaxHits(k) += 1
                                i += 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j -= 1
                    End While

                    'Check diagonally down right.
                    i = x + 1
                    j = y + 1
                    While (i <= _gameSize And j <= _gameSize)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            i -= 1
                            j -= 1
                            While (i >= (x + 1)) And (j >= (y + 1))
                                tabMaxHits(k) += 1
                                i -= 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j += 1
                    End While

                    'Check diagonally up right.
                    i = x - 1
                    j = y + 1
                    While (i >= 0 And j <= _gameSize)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            i += 1
                            j -= 1
                            While (i <= (x - 1)) And (j >= (y + 1))
                                tabMaxHits(k) += 1
                                i += 1
                                j -= 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i -= 1
                        j += 1
                    End While

                    'Check diagonally down left.
                    i = x + 1
                    j = y - 1
                    While (i <= _gameSize And j >= 0)
                        If gameArray(i, j) = caseType.White Then
                        ElseIf gameArray(i, j) = caseType.Black Then
                            i -= 1
                            j += 1
                            While ((x + 1) <= i) And ((y - 1) >= j)
                                tabMaxHits(k) += 1
                                i -= 1
                                j += 1
                            End While
                            Exit While
                        Else : Exit While
                        End If
                        i += 1
                        j -= 1
                    End While
                Next
                Dim tempAr() As Short = tabMaxHits.Clone
                Array.Sort(tempAr)
                Array.Reverse(tempAr)
                For k As Integer = 0 To count - 1
                    If tabMaxHits(k) = tempAr(0) Then
                        finalPlay = k
                        Exit For
                    End If
                Next
                play(tabHits(finalPlay).X, tabHits(finalPlay).Y, False)
            End If
        End If
    End Sub  'IA
#End Region
End Class

