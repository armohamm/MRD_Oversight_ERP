Public Class AgregarPermiso

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
    Public isRecover As Boolean = False

    Public wasCreated As Boolean = False

    Public isLoading As Boolean = False

    Public sselectedusername As String = ""
    Public sselectedwindowname As String = ""
    Public sselectedcontrolname As String = ""

    Private openPermission As Boolean = False

    Public messagesWindowIsAlreadyOpen As Boolean = False


    Private Sub checkMessages(ByVal username As String, ByVal x As Integer, ByVal y As Integer)

        Dim unreadmessagecount As Integer = 0
        unreadmessagecount = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM messages where susername = '" & username & "' AND bread = 0")

        If unreadmessagecount > 0 And messagesWindowIsAlreadyOpen = False Then

            Dim msg As New Mensajes
            Dim pt As Point

            msg.susername = username
            msg.bactive = bactive
            msg.bonline = bonline
            msg.suserfullname = suserfullname
            msg.suseremail = suseremail
            msg.susersession = susersession
            msg.susermachinename = susermachinename
            msg.suserip = suserip

            msg.StartPosition = FormStartPosition.Manual

            Dim tamañoPantalla As Integer = Screen.GetWorkingArea(Me).Height

            Dim tmpPt1 As Point = New Point(Me.Location.X, (tamañoPantalla - Me.Size.Height - msg.Size.Height) / 2) 'msg window
            Dim tmpPt2 As Point = New Point(Me.Location.X, tmpPt1.Y + msg.Size.Height) 'me

            If tmpPt1.Y > Screen.GetWorkingArea(Me).Location.Y Then

                pt = New Point(Me.Location.X, tmpPt1.Y)
                Me.Location = New Point(Me.Location.X, tmpPt2.Y)

            Else

                pt = New Point(x, y)

            End If

            msg.Location = pt
            msg.bAlreadyOpen = True

            messagesWindowIsAlreadyOpen = True

            msg.Show()

        End If

    End Sub


    Private Sub checkForKickoutsAndTimedOuts()

        Dim queryMySession As String = ""
        Dim dsMySession As DataSet

        queryMySession = "SELECT * FROM sessions s WHERE s.susername = '" & susername & "' AND s.susersession = '" & susersession & "' ORDER BY s.ilogindate DESC, s.slogintime DESC LIMIT 1 "

        dsMySession = getSQLQueryAsDataset(0, queryMySession)

        If dsMySession.Tables(0).Rows.Count > 0 Then

            If dsMySession.Tables(0).Rows(0).Item("btimedout") = "1" Then

                MsgBox("Tu sesión ha expirado. Es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Sesión expirada")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

            If dsMySession.Tables(0).Rows(0).Item("bkickedout") = "1" Then

                MsgBox("Has sido sacado del sistema. Para continuar es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Logged out")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

        End If

    End Sub


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        'Check for specific permissions on every window, but only for that unique window permissions, not the entire list!!

        Dim dsPermissions As DataSet

        Dim permission As String

        Dim viewPermission As Boolean = False

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Modificar" Then
                    btnGuardar.Enabled = True
                End If

            Catch ex As Exception

            End Try

            permission = ""

        Next j


        If viewPermission = False Then

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar/Modificar Permiso', 'OK')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedatetime, smessagecreatorusername, iupdatedatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Un usuario ha intentado propasar sus permisos. ¿Podrías revisar?', 0, '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM', '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM')")
                Next i

            End If

            MsgBox("No tienes los permisos necesarios para abrir esta Ventana. Este intento ha sido notificado al administrador.", MsgBoxStyle.Exclamation, "Access Denied")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

        End If

    End Sub


    Private Sub AgregarPermiso_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If validaPermiso(True) = False Then

            If MsgBox("Este Permiso está incompleto. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida") = MsgBoxResult.No Then

                e.Cancel = True
                Exit Sub

            End If

        End If

        'verifySuspiciousData()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Permiso " & txtUsuario.Text.Replace("--", "").Replace("'", "") & " : " & txtVentana.Text.Replace("'", "").Replace("--", "") & " : " & txtPermiso.Text.Replace("--", "").Replace("'", "") & "', 'OK')")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarPermiso_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarPermiso_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        If isEdit = False Then

            txtUsuario.Text = sselectedusername
            txtVentana.Text = ""
            txtPermiso.Text = ""

            txtUsuario.Enabled = False

        Else

            Dim dsPermiso As DataSet
            dsPermiso = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & sselectedusername & "' AND swindowname = '" & sselectedwindowname & "' AND spermission = '" & sselectedcontrolname & "'")

            Try

                isLoading = True

                If dsPermiso.Tables(0).Rows.Count > 0 Then

                    txtUsuario.Text = dsPermiso.Tables(0).Rows(0).Item("susername")
                    txtVentana.Text = dsPermiso.Tables(0).Rows(0).Item("swindowname")
                    txtPermiso.Text = dsPermiso.Tables(0).Rows(0).Item("spermission")

                End If

                isLoading = False

            Catch ex As Exception

            End Try

            txtUsuario.Enabled = False

        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Permiso " & txtUsuario.Text.Replace("--", "").Replace("'", "") & " : " & txtVentana.Text.Replace("'", "").Replace("--", "") & " : " & txtPermiso.Text.Replace("--", "").Replace("'", "") & "', 'OK')")

        txtVentana.Select()
        txtVentana.Focus()
        txtVentana.SelectionStart() = txtVentana.Text.Length

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


    Private Sub txtUsuario_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUsuario.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtUsuario.Text.Contains(arrayCaractProhib(carp)) Then
                txtUsuario.Text = txtUsuario.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtUsuario.Select(txtUsuario.Text.Length, 0)
        End If

        txtUsuario.Text = txtUsuario.Text.Replace("--", "").Replace("'", "")
        txtUsuario.Text = txtUsuario.Text.Trim

    End Sub


    Private Sub txtVentana_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtVentana.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtVentana.Text.Contains(arrayCaractProhib(carp)) Then
                txtVentana.Text = txtVentana.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtVentana.Select(txtVentana.Text.Length, 0)
        End If

        txtVentana.Text = txtVentana.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtPermiso_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPermiso.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPermiso.Text.Contains(arrayCaractProhib(carp)) Then
                txtPermiso.Text = txtPermiso.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtPermiso.Select(txtPermiso.Text.Length, 0)
        End If

        txtPermiso.Text = txtPermiso.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtUsuario_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUsuario.TextChanged, txtVentana.TextChanged, txtPermiso.TextChanged

        If isLoading = True Then
            Exit Sub
        End If

        If txtUsuario.Text.Length > 0 Then

            lblDisponibilidadUsuario.Visible = True

            If isEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & txtUsuario.Text.Replace("--", "") & "' AND swindowname = '" & txtVentana.Text.Replace("--", "") & "' AND spermission = '" & txtPermiso.Text.Replace("--", "") & "'") > 0 Then

                lblDisponibilidadUsuario.Text = "No Disponible"
                lblDisponibilidadUsuario.ForeColor = Color.Red

            ElseIf isEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & txtUsuario.Text.Replace("--", "") & "' AND swindowname = '" & txtVentana.Text.Replace("--", "") & "' AND spermission = '" & txtPermiso.Text.Replace("--", "") & "'") = 0 Then

                lblDisponibilidadUsuario.Text = "Disponible"
                lblDisponibilidadUsuario.ForeColor = Color.ForestGreen

            ElseIf isEdit = True And txtUsuario.Text <> sselectedusername Then

                lblDisponibilidadUsuario.Visible = False

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & txtUsuario.Text.Replace("--", "") & "' AND swindowname = '" & txtVentana.Text.Replace("--", "") & "' AND spermission = '" & txtPermiso.Text.Replace("--", "") & "'") > 0 Then

                    lblDisponibilidadUsuario.Visible = True
                    lblDisponibilidadUsuario.Text = "No Disponible"
                    lblDisponibilidadUsuario.ForeColor = Color.Red

                Else

                    lblDisponibilidadUsuario.Visible = True
                    lblDisponibilidadUsuario.Text = "Disponible"
                    lblDisponibilidadUsuario.ForeColor = Color.ForestGreen

                End If

            ElseIf isEdit = True And txtVentana.Text <> sselectedwindowname Then

                lblDisponibilidadUsuario.Visible = False

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & txtUsuario.Text.Replace("--", "") & "' AND swindowname = '" & txtVentana.Text.Replace("--", "") & "' AND spermission = '" & txtPermiso.Text.Replace("--", "") & "'") > 0 Then

                    lblDisponibilidadUsuario.Visible = True
                    lblDisponibilidadUsuario.Text = "No Disponible"
                    lblDisponibilidadUsuario.ForeColor = Color.Red

                Else

                    lblDisponibilidadUsuario.Visible = True
                    lblDisponibilidadUsuario.Text = "Disponible"
                    lblDisponibilidadUsuario.ForeColor = Color.ForestGreen

                End If

            ElseIf isEdit = True And txtPermiso.Text <> sselectedcontrolname Then

                lblDisponibilidadUsuario.Visible = False

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & txtUsuario.Text.Replace("--", "") & "' AND swindowname = '" & txtVentana.Text.Replace("--", "") & "' AND spermission = '" & txtPermiso.Text.Replace("--", "") & "'") > 0 Then

                    lblDisponibilidadUsuario.Visible = True
                    lblDisponibilidadUsuario.Text = "No Disponible"
                    lblDisponibilidadUsuario.ForeColor = Color.Red

                Else

                    lblDisponibilidadUsuario.Visible = True
                    lblDisponibilidadUsuario.Text = "Disponible"
                    lblDisponibilidadUsuario.ForeColor = Color.ForestGreen

                End If

            End If

        Else

            lblDisponibilidadUsuario.Visible = False

        End If

    End Sub


    Private Function validaPermiso(ByVal silent As Boolean) As Boolean

        txtUsuario.Text = txtUsuario.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtVentana.Text = txtVentana.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtPermiso.Text = txtPermiso.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If txtUsuario.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner el Usuario al que estás asignando el Permiso?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtUsuario.Select()
            txtUsuario.Focus()
            Return False

        End If

        If txtVentana.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner de que Ventana es el Permiso que estás asignando?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtVentana.Select()
            txtVentana.Focus()
            Return False

        End If

        If txtPermiso.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner el Permiso que deseas asignar?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtPermiso.Select()
            txtPermiso.Focus()
            Return False

        End If

        If isEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM userpermissions WHERE susername = '" & txtUsuario.Text.Replace("--", "") & "' AND swindowname = '" & txtVentana.Text.Replace("--", "") & "' AND spermission = '" & txtPermiso.Text.Replace("--", "") & "'") > 0 Then

            If silent = False Then
                MsgBox("Ya existe este Permiso en el sistema.", MsgBoxStyle.OkOnly, "Error")
            End If

            txtVentana.Select()
            txtVentana.Focus()
            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'sselectedusername = ""
        'sselectedwindowname = ""
        'sselectedcontrolname = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaPermiso(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If


        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()


        If isEdit = False Then

            sselectedusername = txtUsuario.Text.Replace("--", "").Replace("'", "")
            sselectedwindowname = txtVentana.Text.Replace("--", "").Replace("'", "")
            sselectedcontrolname = txtPermiso.Text.Replace("--", "").Replace("'", "")

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions VALUES ('" & txtUsuario.Text.Replace("--", "").Replace("'", "") & "', '" & txtVentana.Text.Replace("--", "").Replace("'", "") & "', '" & txtPermiso.Text.Replace("--", "") & "', " & fecha & ", '" & hora & "', '" & susername & "')")

        Else

            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & sselectedusername & "' AND swindowname = '" & sselectedwindowname & "' AND spermission = '" & sselectedcontrolname & "'")

            sselectedusername = txtUsuario.Text.Replace("--", "").Replace("'", "")
            sselectedwindowname = txtVentana.Text.Replace("--", "").Replace("'", "")
            sselectedcontrolname = txtPermiso.Text.Replace("--", "").Replace("'", "")

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions VALUES ('" & txtUsuario.Text.Replace("--", "").Replace("'", "") & "', '" & txtVentana.Text.Replace("--", "").Replace("'", "") & "', '" & txtPermiso.Text.Replace("--", "") & "', " & fecha & ", '" & hora & "', '" & susername & "')")

        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Permiso " & txtUsuario.Text.Replace("--", "").Replace("'", "") & " : " & txtVentana.Text.Replace("'", "").Replace("--", "") & " : " & txtPermiso.Text.Replace("--", "").Replace("'", "") & "', 'OK')")

        wasCreated = True

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub

End Class