Imports System.Windows.Forms

Public Class PreguntaRescate

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public isEdit As Boolean = False


    Private Sub PreguntaRescate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.F5 Then

            If My.Computer.Info.OSFullName.StartsWith("Microsoft Windows 7") = True Then
                NotifyIcon1.Icon = Oversight.My.Resources.winmineVista16x16
            Else
                NotifyIcon1.Icon = Oversight.My.Resources.winmineXP16x16
            End If

            NotifyIcon1.Text = "Buscaminas"

            NotifyIcon1.Visible = True

            Me.Visible = False
            Do While Not fDone
                System.Windows.Forms.Application.DoEvents()
            Loop
            fDone = False
            Me.Visible = True

        End If

    End Sub


    Private Sub PreguntaRescate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        If defineDBServer() = False Then

            updateOversightIfNewVersionIsAvailable()

            MsgBox("¿Está prendida la máquina con la base de datos? No logro conectarme. ¿O quizás esté mal la configuración?", MsgBoxStyle.OkOnly, "Error")

            Dim op As New Opciones

            op.ShowDialog(Me)

            If op.DialogResult = Windows.Forms.DialogResult.OK Then
                If defineDBServer() = False Then
                    MsgBox("No encontré conexión con la base de datos de Oversight. Cerrando aplicación...", MsgBoxStyle.OkOnly, "Error")
                    System.Environment.Exit(0)
                End If
            Else
                System.Environment.Exit(0)
            End If

        Else

            updateOversightIfNewVersionIsAvailable()

        End If

        lblPregunta.Text = base64Decode(getSQLQueryAsString(0, "SELECT suserrescuequestion FROM users where susername = '" & susername & "'"))

        txtRespuesta.Select()
        txtRespuesta.Focus()
        txtRespuesta.SelectionStart() = txtRespuesta.Text.Length

    End Sub


    Private Sub PreguntaRescate_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        CenterToScreen()
        txtRespuesta.Focus()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick

        Dim n As New Loader

        n.isEdit = True

        n.ShowDialog()

        If n.DialogResult = Windows.Forms.DialogResult.OK Then

            fDone = True

        End If

    End Sub


    Public Sub findMachineName()

        susermachinename = Environ$("ComputerName")

    End Sub


    Private Sub txtRespuesta_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRespuesta.KeyUp

        'Acepta puntos, espacios, _, ?, ! en password
        Dim strcaracteresprohibidos As String = "|°#@$%&/()=¡*¨[]:;,-{}+´¿'¬^`~\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtRespuesta.Text.Contains(arrayCaractProhib(carp)) Then
                txtRespuesta.Text = txtRespuesta.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtRespuesta.Select(txtRespuesta.Text.Length, 0)
        End If

    End Sub


    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If txtRespuesta.Text.Trim = "" Then
            MsgBox("Es necesario que pongas la respuesta a tu pregunta para continuar", MsgBoxStyle.Information, "Login")
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim dsUser As DataSet
        dsUser = getSQLQueryAsDataset(0, "SELECT * FROM users WHERE susername = '" & susername & "' AND suserrescueanswer = '" & EncryptText(txtRespuesta.Text.Trim().Replace("'", "").Replace("@", "").Replace("%", "").Replace("--", "")) & "'")

        If dsUser.Tables(0).Rows.Count > 0 Then

            closeTimedOutConnections()

            If getSQLQueryAsInteger(0, "SELECT blockedout FROM sessions WHERE susername = '" & susername & "' ORDER BY ilogindate DESC, slogintime DESC LIMIT 1") = 1 Then

                Cursor.Current = System.Windows.Forms.Cursors.Default
                MsgBox("Tu acceso a Oversight ha sido revocado. Contacta al administrador para más información", MsgBoxStyle.Information, "Access denied")
                Exit Sub

            End If

            If getSQLQueryAsInteger(0, "SELECT bactive FROM users WHERE susername = '" & susername & "'") = 0 Then

                Cursor.Current = System.Windows.Forms.Cursors.Default
                MsgBox("Tu acceso a Oversight ha sido revocado. Contacta al administrador para más información", MsgBoxStyle.Information, "Access denied")
                Exit Sub

            End If

            Try

                susername = dsUser.Tables(0).Rows(0).Item("susername")
                bactive = CBool(dsUser.Tables(0).Rows(0).Item("bactive"))
                bonline = CBool(dsUser.Tables(0).Rows(0).Item("bonline"))

                If CInt(dsUser.Tables(0).Rows(0).Item("ipeopleid")) > 0 Then

                    Dim dsPeople As DataSet
                    dsPeople = getSQLQueryAsDataset(0, "SELECT * FROM people WHERE ipeopleid = " & dsUser.Tables(0).Rows(0).Item("ipeopleid"))

                    If dsPeople.Tables(0).Rows.Count > 0 Then
                        suserfullname = dsPeople.Tables(0).Rows(0).Item("speoplefullname")
                        suseremail = dsPeople.Tables(0).Rows(0).Item("speoplemail")
                    End If

                End If

                findMachineName()

                If bonline = True Then
                    Dim sessionid As Integer = 1
                    sessionid = getSQLQueryAsInteger(0, "SELECT IF(CONVERT(s.susersession, DECIMAL) + 1 IS NULL, 1, CONVERT(s.susersession, DECIMAL) + 1) FROM (SELECT * FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL ORDER BY ilogindate DESC, slogintime DESC) s")
                    susersession = sessionid
                Else
                    susersession = 1
                End If

            Catch ex As Exception

            End Try

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            Dim queries(3) As String

            queries(0) = "INSERT IGNORE INTO logs VALUES(" & fecha & ", '" & hora & "', '" & susername & "', '" & susersession & "', '" & suserip & "', '" & susermachinename & "', 'Login attempt', 'Login succeded - Access granted')"
            queries(1) = "INSERT INTO sessions (susername, susersession, bloggedinsuccesfully, btimedout, blockedout, bkickedout, suserip, susermachinename, ilogindate, slogintime) VALUES('" & susername & "', '" & susersession & "', 1, 0, 0, 0, '" & suserip & "', '" & susermachinename & "', " & fecha & ", '" & hora & "')"
            queries(2) = "UPDATE users SET bonline = 1 WHERE susername = '" & susername & "'"

            executeTransactedSQLCommand(0, queries)

            Me.DialogResult = System.Windows.Forms.DialogResult.OK

            If isEdit = False Then

                executeSQLCommand(0, "UPDATE users SET suserrescuequestion = '', suserrescueanswer = '' WHERE susername = '" & susername & "'")

                MsgBox("Bienvenido " & suserfullname & ". Es necesario que cambies tu contraseña." & vbNewLine & "Para asegurarme de esto, tu Pregunta y Respuesta de Rescate han sido eliminadas, para evitar futuras entradas sin que hayas actualizado tu contraseña. Por favor, actualiza tu Pregunta y Respuesta de Rescate tambien.")

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim au As New AgregarUsuario

                au.susername = susername
                au.bactive = bactive
                au.bonline = bonline
                au.suserfullname = suserfullname
                au.suseremail = suseremail
                au.susersession = susersession
                au.susermachinename = susermachinename
                au.suserip = suserip

                au.sselectedusername = susername
                au.sselecteduserfullname = suserfullname

                au.IsEdit = True

                Me.Visible = False
                au.ShowDialog(Me)
                Me.Visible = True

                If au.DialogResult = Windows.Forms.DialogResult.OK Then

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                    Dim ms As New MissionControl

                    Try

                        ms.susername = susername
                        ms.bactive = bactive
                        ms.bonline = bonline
                        ms.suserfullname = suserfullname
                        ms.suseremail = suseremail
                        ms.susersession = susersession
                        ms.susermachinename = susermachinename
                        ms.suserip = suserip

                    Catch ex As Exception

                    End Try

                    ms.Text = ms.Text & " - " & susername & " (" & susersession & ") @ " & servidor

                    ms.Show()

                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()

                End If

                Cursor.Current = System.Windows.Forms.Cursors.Default



            End If

            Me.Close()

        Else

            txtRespuesta.Text = txtRespuesta.Text.Trim().Replace("'", "").Replace("@", "").Replace("%", "").Replace("--", "")

            Try

                Dim sessionid As Integer = 1
                sessionid = getSQLQueryAsInteger(0, "SELECT IF(CONVERT(s.susersession, DECIMAL) + 1 IS NULL, 1, CONVERT(s.susersession, DECIMAL) + 1) FROM (SELECT * FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL ORDER BY ilogindate DESC, slogintime DESC) s")
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES(" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', '" & sessionid + 1 & "', '" & suserip & "', '" & susermachinename & "', 'Login attempt', 'Login failed - Access denied')")

            Catch ex As Exception

            End Try

            MsgBox("Respuesta incorrecta", MsgBoxStyle.Exclamation, "Recuperacion Contraseña")
            txtRespuesta.Text = ""
            txtRespuesta.Focus()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


End Class
