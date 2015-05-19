Imports System.Windows.Forms

Public Class Loader

    Public isEdit As Boolean = False

    Private Declare Auto Function GetAsyncKeyState Lib "user32.dll" ( _
    ByVal nVirtKey As Keys) As Boolean


    Private Sub Loader_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.KeyPreview = True

        If GetAsyncKeyState(Keys.RControlKey) = True Or GetAsyncKeyState(Keys.LControlKey) = True Then

            'Go

            If isEdit = False Then

                Dim l As New Login
                l.Show()

            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Else

            Try
                'System.Diagnostics.Process.Start("WINMINE.EXE")
                Shell("C:\Windows\System32\winmine.exe", AppWinStyle.NormalFocus, False, 1)

            Catch ex As Exception

                Try
                    'System.Diagnostics.Process.Start("C:\Program Files\Microsoft Games\Minesweeper\MineSweeper.exe")
                    Shell("C:\Program Files\Microsoft Games\Minesweeper\MineSweeper.exe", AppWinStyle.NormalFocus, False, 1)

                Catch ex2 As Exception

                    Try
                        'System.Diagnostics.Process.Start("C:\Program Files(x64)\Microsoft Games\Minesweeper\MineSweeper.exe")
                        Shell("C:\Program Files(x64)\Microsoft Games\Minesweeper\MineSweeper.exe", AppWinStyle.NormalFocus, False, 1)
                    Catch ex3 As Exception

                    End Try

                End Try

            End Try

            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

        End If

    End Sub


    Private Sub Loader_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        Me.Visible = False

    End Sub


End Class
