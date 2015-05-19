Public Class AgregarUsuario

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public ipeopleid As Integer = 0

    Public sselectedusername As String = ""
    Public sselecteduserfullname As String = ""

    Public slastusername As String = ""

    Public IsEdit As Boolean = False
    Public IsRecover As Boolean = False

    Public iselectedsessionid As Integer = 0
    Public iselectedlogindate As Integer = 0
    Public sselectedlogintime As String = ""

    Public sselectedIP As String = ""
    Public sselectedMachineName As String = ""

    Public sselectedWindowName As String = ""
    Public sselectedParentControlName As String = ""
    Public sselectedControlName As String = ""

    Public iselecteddate As Integer = 0
    Public sselectedtime As String = ""

    Private WithEvents txtVentanaDgvPermisosUsuario As TextBox
    Private WithEvents txtPermisoDgvPermisosUsuario As TextBox

    Private txtVentanaDgvPermisosUsuario_OldText As String = ""
    Private txtPermisoDgvPermisosUsuario_OldText As String = ""

    Private isFormReadyForAction As Boolean = False

    Private isPermisosUsuarioDGVReady As Boolean = False
    Private updateUsername As Boolean = False
    Private dropLastUserNameTables As Boolean = False

    Public wasCreated As Boolean = False

    Private openPermission As Boolean = False
    Private savePermission As Boolean = False

    Private openPermissionPermission As Boolean = False
    Private addPermission As Boolean = False
    Private insertPermission As Boolean = False
    Private modifyPermission As Boolean = False
    Private deletePermission As Boolean = False

    Private viewDgvLastModDate As Boolean = False

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

                If permission = "Abrir Persona" Then
                    openPermission = True
                    btnPersonas.Enabled = True
                End If

                If permission = "Abrir Permiso" Then
                    openPermissionPermission = True
                End If

                If permission = "Agregar Permiso" Then
                    addPermission = True
                    insertPermission = True
                End If

                If permission = "Modificar Permiso" Then
                    modifyPermission = True
                End If

                If permission = "Borrar Permiso" Then
                    deletePermission = True
                End If

                If permission = "Ver Fecha Ultima Modificacion" Then
                    viewDgvLastModDate = True
                End If

                If permission = "Ver Sesiones" Then
                    dgvSesionesAbiertas.Visible = True
                End If

                If permission = "Ver Logs" Then
                    dgvLogs.Visible = True
                End If

                If permission = "Lockout" Then
                    btnLockout.Enabled = True
                End If

                If permission = "Kickout" Then
                    btnKickout.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar/Modificar Usuario', 'OK')")

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


    Private Sub AgregarUsuario_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM users " & _
        "WHERE susername = '" & sselectedusername & "' AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc WHERE users.susername = tclc.susername) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc JOIN users clc ON tclc.susername = clc.susername WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM users clc WHERE tclc.susername = clc.susername AND clc.susername = '" & sselectedusername & "') ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM userpermissions " & _
        "WHERE susername = '" & sselectedusername & "' AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc WHERE userpermissions.susername = tclc.susername AND userpermissions.swindowname = tclc.swindowname AND userpermissions.spermission = tclc.spermission) ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc JOIN userpermissions clc ON tclc.susername = clc.susername AND tclc.swindowname = clc.swindowname AND tclc.spermission = clc.spermission WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM userpermissions clc WHERE tclc.susername = clc.susername AND tclc.swindowname = clc.swindowname AND tclc.spermission = clc.spermission AND clc.susername = '" & sselectedusername & "') ")


        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If txtNombreUsuario.Text.Trim = "" Or ipeopleid = 0 And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Usuario no está completo. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana y Cancelar este Usuario?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then

            Dim timesUserIsOpen As Integer = 1

            timesUserIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%User" & sselectedusername & "'")

            If timesUserIsOpen > 1 And IsEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Usuario. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesUserIsOpen > 1 And IsEdit = False Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario esta ingresando un usuario con tu mismo Nombre de Usuario. Esto podría causar que alguno de ustedes perdiera sus datos/cambios en el archivo. Recomiendo cambiar de Nombre de Usuario. ¿Deseas continuar guardando con este nombre de usuario (Sí) o Desear regresar a cambiarlo (No)?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            End If

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor


            Dim queries(7) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM users " & _
            "WHERE susername = '" & sselectedusername & "' AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc WHERE users.susername = tclc.susername) "

            queries(1) = "" & _
            "UPDATE users clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc ON tclc.susername = clc.susername SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipeopleid = tclc.ipeopleid, clc.suserpassword = tclc.suserpassword, clc.suserrescuequestion = tclc.suserrescuequestion, clc.suserrescueanswer = tclc.suserrescueanswer, clc.bactive = tclc.bactive WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(2) = "" & _
            "INSERT INTO users " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM users clc WHERE tclc.susername = clc.susername AND clc.susername = '" & sselectedusername & "') "

            queries(3) = "" & _
            "DELETE " & _
            "FROM userpermissions " & _
            "WHERE susername = '" & sselectedusername & "' AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc WHERE userpermissions.susername = tclc.susername AND userpermissions.swindowname = tclc.swindowname AND userpermissions.spermission = tclc.spermission) "

            queries(4) = "" & _
            "UPDATE userpermissions clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc ON tclc.susername = clc.susername AND tclc.swindowname = clc.swindowname AND tclc.spermission = clc.spermission SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.susername = tclc.susername, clc.swindowname = tclc.swindowname, clc.spermission = tclc.spermission WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(5) = "" & _
            "INSERT INTO userpermissions " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM userpermissions clc WHERE tclc.susername = clc.susername AND tclc.swindowname = clc.swindowname AND tclc.spermission = clc.spermission AND clc.susername = '" & sselectedusername & "') "

            queries(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Usuario " & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & " y sus Permisos', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Cursor.Current = System.Windows.Forms.Cursors.Default
                e.Cancel = True
                Exit Sub
            End If

            wasCreated = True


        ElseIf result = MsgBoxResult.Cancel Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        End If


        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim queriesDrop(4) As String

        queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername
        queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions"
        queriesDrop(2) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'User', 'Usuario', '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"
        queriesDrop(3) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Usuario " & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & " y sus Permisos', 'OK')"

        executeTransactedSQLCommand(0, queriesDrop)

        Dim queriesDrop2(4) As String

        If dropLastUserNameTables = True Then
            queriesDrop2(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & slastusername
            queriesDrop2(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & slastusername & "Permissions"
            queriesDrop2(2) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'User', 'Usuario', '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"
            queriesDrop2(3) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Usuario " & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & " y sus Permisos', 'OK')"
        End If

        executeTransactedSQLCommand(0, queriesDrop2)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Sub AgregarUsuario_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarUsuario_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        isFormReadyForAction = False

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesUserIsOpen As Integer = 0

        timesUserIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%User" & sselectedusername & "'")

        If timesUserIsOpen > 0 And IsEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Usuario. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Usuario?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If


        If IsRecover = False Then

            Dim queriesCreation(4) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) NOT NULL, `suserpassword` varchar(300) CHARACTER SET latin1 NOT NULL, `suserrescuequestion` varchar(150) CHARACTER SET latin1 NOT NULL, `suserrescueanswer` varchar(150) CHARACTER SET latin1 NOT NULL, `bactive` tinyint(1) NOT NULL, `bonline` tinyint(1) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`), KEY `user&approved` (`supdateusername`,`bactive`), KEY `user&online` (`supdateusername`,`bonline`), KEY `user&updateuser` (`susername`,`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `swindowname` varchar(300) CHARACTER SET latin1 NOT NULL, `spermission` varchar(300) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`,`swindowname`,`spermission`), KEY `windowname` (`swindowname`), KEY `username` (`susername`), KEY `updateuser` (`supdateusername`), KEY `parentpermission` (`spermission`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        If IsEdit = True Then

            If IsRecover = False Then

                Dim queriesInsert(2) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SELECT * FROM users WHERE susername = '" & sselectedusername & "'"
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SELECT * FROM userpermissions WHERE susername = '" & sselectedusername & "'"

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsDatosUsuario As DataSet
            dsDatosUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

            Me.Text = "Usuario " & sselectedusername

            txtUltimaModificacion.Text = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsDatosUsuario.Tables(0).Rows(0).Item("iupdatedate")) & " " & dsDatosUsuario.Tables(0).Rows(0).Item("supdatetime")
            txtUltimaModificacion.Enabled = False
            ipeopleid = dsDatosUsuario.Tables(0).Rows(0).Item("ipeopleid")
            txtNombrePersona.Text = getSQLQueryAsString(0, "SELECT speoplefullname from people where ipeopleid = " & dsDatosUsuario.Tables(0).Rows(0).Item("ipeopleid"))
            txtNombreUsuario.Text = dsDatosUsuario.Tables(0).Rows(0).Item("susername")
            txtPassword.Text = ""
            txtConfirmarPassword.Text = ""
            txtPreguntaRescate.Text = base64Decode(dsDatosUsuario.Tables(0).Rows(0).Item("suserrescuequestion"))
            txtRespuesta.Text = ""

            If dsDatosUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                chkActivo.Checked = True
            Else
                chkActivo.Checked = False
            End If

            If dsDatosUsuario.Tables(0).Rows(0).Item("bonline") = "1" Then
                chkOnline.Checked = True
            Else
                chkOnline.Checked = False
            End If

            Dim queryLogs As String = ""

            queryLogs = "" & _
            "SELECT STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha / Hora', supdateusername AS 'Usuario', susersession AS 'Sesion', suserip AS 'IP', susermachinename AS 'Nombre de Maquina', sactiondone AS 'Accion hecha', sresult AS 'Resultado' FROM logs WHERE supdateusername = '" & sselectedusername & "' " & _
            "ORDER BY 1 DESC, 2 DESC, 4 ASC "

            setDataGridView(dgvLogs, queryLogs, True)

            dgvLogs.Columns(1).Visible = False

            dgvLogs.Columns(0).Width = 100
            dgvLogs.Columns(1).Width = 100
            dgvLogs.Columns(2).Width = 70
            dgvLogs.Columns(3).Width = 100
            dgvLogs.Columns(4).Width = 200
            dgvLogs.Columns(5).Width = 70

        Else

            Me.Text = "Usuario Nuevo"

            txtUltimaModificacion.Text = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()
            txtUltimaModificacion.Enabled = False

        End If


        Dim querySesiones As String = ""

        If chkSesionesHistoricas.Checked = True Then

            querySesiones = "" & _
            "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
            "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
            "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
            "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
            "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
            "FROM sessions " & _
            "WHERE susername = '" & sselectedusername & "' " & _
            "ORDER BY 8, 2 "

        Else

            querySesiones = "" & _
            "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
            "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
            "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
            "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
            "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
            "FROM sessions " & _
            "WHERE susername = '" & sselectedusername & "' " & _
            "AND ilogoutdate IS NULL " & _
            "ORDER BY 8, 2 "

        End If

        setDataGridView(dgvSesionesAbiertas, querySesiones, True)

        dgvSesionesAbiertas.Columns(0).Visible = False
        dgvSesionesAbiertas.Columns(6).Visible = False
        dgvSesionesAbiertas.Columns(7).Visible = False


        dgvSesionesAbiertas.Columns(0).Width = 100
        dgvSesionesAbiertas.Columns(1).Width = 100
        dgvSesionesAbiertas.Columns(2).Width = 100
        dgvSesionesAbiertas.Columns(3).Width = 100
        dgvSesionesAbiertas.Columns(4).Width = 100
        dgvSesionesAbiertas.Columns(5).Width = 100
        dgvSesionesAbiertas.Columns(6).Width = 100
        dgvSesionesAbiertas.Columns(7).Width = 100


        setDataGridView(dgvPermisosUsuario, "SELECT susername AS 'Usuario', swindowname AS 'Ventana', spermission AS 'Permiso', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Act' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ORDER BY 1 ASC, 2 ASC, 3 ASC ", True)

        dgvPermisosUsuario.Columns(0).Visible = False

        If viewDgvLastModDate = False Then
            dgvPermisosUsuario.Columns(3).Visible = False
        End If

        dgvPermisosUsuario.Columns(0).Width = 100
        dgvPermisosUsuario.Columns(1).Width = 150
        dgvPermisosUsuario.Columns(2).Width = 150
        dgvPermisosUsuario.Columns(3).Width = 100


        If IsEdit = True And sselectedusername <> "" Then

            dgvPermisosUsuario.Enabled = True
            btnNuevoPermiso.Enabled = True
            btnInsertarPermiso.Enabled = True
            btnEliminarPermiso.Enabled = True

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'User', 'Usuario', '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")
        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Usuario " & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', 'OK')")

        isPermisosUsuarioDGVReady = True

        isFormReadyForAction = True

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


    Private Sub btnPersonas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPersonas.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        txtNombrePersona.Text = txtNombrePersona.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", "")

        Dim bp As New BuscaPersonas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname

        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.querystring = txtNombrePersona.Text.Trim

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            txtNombrePersona.Text = bp.speoplefullname
            ipeopleid = bp.ipeopleid

            txtNombreUsuario.Focus()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtNombreUsuario_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreUsuario.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNombreUsuario.Text.Contains(arrayCaractProhib(carp)) Then
                txtNombreUsuario.Text = txtNombreUsuario.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtNombreUsuario.Select(txtNombreUsuario.Text.Length, 0)
        End If

    End Sub


    Private Sub txtPassword_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPassword.KeyUp, txtConfirmarPassword.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPassword.Text.Contains(arrayCaractProhib(carp)) Then
                txtPassword.Text = txtPassword.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtPassword.Select(txtPassword.Text.Length, 0)
        End If

    End Sub


    Private Sub txtNombreUsuario_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreUsuario.LostFocus

        txtNombreUsuario.Text = txtNombreUsuario.Text.ToLower

    End Sub


    Private Sub txtNombreUsuario_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombreUsuario.TextChanged

        If txtNombreUsuario.Text.Length > 0 Then

            lblDisponibilidad.Visible = True

            Dim queryCheck As String = ""

            queryCheck = "SELECT susername FROM users WHERE susername = '" & sselectedusername & "'"


            If IsEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM users WHERE susername = '" & txtNombreUsuario.Text.ToLower & "'") >= 1 Then

                lblDisponibilidad.Text = "No Disponible"
                lblDisponibilidad.ForeColor = Color.Red

            ElseIf IsEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM users WHERE susername = '" & txtNombreUsuario.Text.ToLower & "'") = 0 Then

                lblDisponibilidad.Text = "Disponible"
                lblDisponibilidad.ForeColor = Color.ForestGreen

            ElseIf IsEdit = True And txtNombreUsuario.Text <> getSQLQueryAsString(0, queryCheck) Then

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM users WHERE susername = '" & txtNombreUsuario.Text.ToUpper & "'") >= 1 Then

                    lblDisponibilidad.Text = "No Disponible"
                    lblDisponibilidad.ForeColor = Color.Red

                ElseIf getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM users WHERE susername = '" & txtNombreUsuario.Text.ToUpper & "'") = 0 Then

                    lblDisponibilidad.Text = "Disponible"
                    lblDisponibilidad.ForeColor = Color.ForestGreen

                End If

            ElseIf IsEdit = True And txtNombreUsuario.Text = getSQLQueryAsString(0, queryCheck) Then

                lblDisponibilidad.Visible = False

            End If

            If IsEdit = True And txtNombreUsuario.Text.Trim.ToLower <> sselectedusername Then
                updateUsername = True
            End If

        Else

            lblDisponibilidad.Visible = False

            If IsEdit = True And txtNombreUsuario.Text.Trim.ToLower <> sselectedusername Then

                lblDisponibilidad.Visible = True
                lblDisponibilidad.Text = "No Disponible"
                lblDisponibilidad.ForeColor = Color.Red

            End If

        End If

    End Sub


    Private Sub txtPreguntaRescate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPreguntaRescate.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPreguntaRescate.Text.Contains(arrayCaractProhib(carp)) Then
                txtPreguntaRescate.Text = txtPreguntaRescate.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtPreguntaRescate.Select(txtPreguntaRescate.Text.Length, 0)
        End If

    End Sub


    Private Sub txtRespuesta_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRespuesta.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
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


    Private Sub txtRespuesta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRespuesta.TextChanged

        If savePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If validaDatosInicialesUsuario(True, False) = False Then
            Exit Sub
        Else
            dgvPermisosUsuario.Enabled = True
            btnNuevoPermiso.Enabled = True
            btnInsertarPermiso.Enabled = True
            btnEliminarPermiso.Enabled = True
        End If


        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If IsEdit = True Then

            Dim dsUsuario As DataSet
            dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

            Dim strUpdate As String = ""
            strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

            If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                updateUsername = True
                strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
            End If

            If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
            End If

            If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
            End If

            If base64Encode(txtPreguntaRescate.Text).Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                strUpdate = strUpdate & "suserrescuequestion = '" & base64Encode(txtPreguntaRescate.Text) & "', "
            End If

            If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
            End If

            Dim isActive As Boolean = False

            If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                isActive = True
            Else
                isActive = False
            End If

            If chkActivo.Checked <> isActive Then

                If chkActivo.Checked = True Then
                    strUpdate = strUpdate & "bactive = 1, "
                Else
                    strUpdate = strUpdate & "bactive = 0, "
                End If

            End If

            If strUpdate.EndsWith(", ") = True Then
                strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
            End If

            strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"


        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

            If checkIfItsOnlyTextUpdate = True Then

                Dim dsUsuario As DataSet
                dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

                Dim strUpdate As String = ""
                strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

                If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                    updateUsername = True
                    strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
                End If

                If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                    strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
                End If

                If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                    strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
                End If

                If base64Encode(txtPreguntaRescate.Text).Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                    strUpdate = strUpdate & "suserrescuequestion = '" & base64Encode(txtPreguntaRescate.Text) & "', "
                End If

                If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                    strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
                End If

                Dim isActive As Boolean = False

                If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                    isActive = True
                Else
                    isActive = False
                End If

                If chkActivo.Checked <> isActive Then

                    If chkActivo.Checked = True Then
                        strUpdate = strUpdate & "bactive = 1, "
                    Else
                        strUpdate = strUpdate & "bactive = 0, "
                    End If

                End If

                If strUpdate.EndsWith(", ") = True Then
                    strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
                End If

                strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"

                Dim queriesUpdate(2) As String

                queriesUpdate(0) = strUpdate

                If updateUsername = True Then
                    queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                sselectedusername = txtNombreUsuario.Text.ToLower
                sselecteduserfullname = txtNombrePersona.Text

                Dim queriesCreation(4) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) NOT NULL, `suserpassword` varchar(300) CHARACTER SET latin1 NOT NULL, `suserrescuequestion` varchar(150) CHARACTER SET latin1 NOT NULL, `suserrescueanswer` varchar(150) CHARACTER SET latin1 NOT NULL, `bactive` tinyint(1) NOT NULL, `bonline` tinyint(1) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`), KEY `user&approved` (`supdateusername`,`bactive`), KEY `user&online` (`supdateusername`,`bonline`), KEY `user&updateuser` (`susername`,`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "UserPermissions"
                queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `swindowname` varchar(300) CHARACTER SET latin1 NOT NULL, `spermission` varchar(300) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`,`swindowname`,`spermission`), KEY `windowname` (`swindowname`), KEY `username` (`susername`), KEY `updateuser` (`supdateusername`), KEY `parentpermission` (`spermission`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                If chkActivo.Checked = True Then
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & base64Encode(txtPreguntaRescate.Text) & "', '" & EncryptText(txtRespuesta.Text) & "', 1, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                Else
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & base64Encode(txtPreguntaRescate.Text) & "', '" & EncryptText(txtRespuesta.Text) & "', 0, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Function validaDatosInicialesUsuario(ByVal silent As Boolean, ByVal save As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"

        txtNombreUsuario.Text = txtNombreUsuario.Text.Trim(strcaracteresprohibidos.ToCharArray)
        txtPassword.Text = txtPassword.Text.Trim(strcaracteresprohibidos.ToCharArray)
        txtConfirmarPassword.Text = txtConfirmarPassword.Text.Trim(strcaracteresprohibidos.ToCharArray)
        txtPreguntaRescate.Text = txtPreguntaRescate.Text.Trim(strcaracteresprohibidos.ToCharArray)
        txtRespuesta.Text = txtRespuesta.Text.Trim(strcaracteresprohibidos.ToCharArray)

        If ipeopleid = 0 Or txtNombrePersona.Text.Trim = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner una persona asociada al Usuario?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If


        If txtNombreUsuario.Text.Trim = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner un nombre de usuario?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        If save = True Then

            If IsEdit = False Then

                If txtPassword.Text.Trim = "" Then

                    If silent = False Then
                        MsgBox("¿Podrías poner una contraseña?", MsgBoxStyle.OkOnly, "Dato Faltante")
                    End If

                    Return False

                End If

                If txtConfirmarPassword.Text.Trim = "" Then

                    If silent = False Then
                        MsgBox("¿Podrías confirmar la contraseña?", MsgBoxStyle.OkOnly, "Dato Faltante")
                    End If

                    Return False

                End If

                If txtPassword.Text.Trim <> txtConfirmarPassword.Text.Trim Then

                    If silent = False Then
                        MsgBox("Las contraseñas no coinciden. ¿Puedes corregir esto?", MsgBoxStyle.OkOnly, "Dato Faltante")
                    End If

                    Return False

                End If

                'If txtPreguntaRescate.Text = "" Then

                '    If silent = False Then
                '        MsgBox("¿Podrías poner una pregunta de rescate para el Usuario?", MsgBoxStyle.OkOnly, "Dato Faltante")
                '    End If

                '    Return False

                'End If

                'If txtRespuesta.Text = "" Then

                '    If silent = False Then
                '        MsgBox("¿Podrías poner una respuesta para la pregunta de rescate?", MsgBoxStyle.OkOnly, "Dato Faltante")
                '    End If

                '    Return False

                'End If

            Else

                If (txtPassword.Text.Trim <> "" Or txtConfirmarPassword.Text <> "") And txtPassword.Text.Trim <> txtConfirmarPassword.Text.Trim Then

                    If silent = False Then
                        MsgBox("Las contraseñas no coinciden. ¿Puedes corregir esto?", MsgBoxStyle.OkOnly, "Dato Faltante")
                    End If

                    Return False

                End If

            End If

        End If

        Return True

    End Function


    Private Sub DgvPermisosUsuario_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPermisosUsuario.CellClick

        If validaDatosInicialesUsuario(True, False) = False Then
            Exit Sub
        End If

        Try

            If dgvPermisosUsuario.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            sselectedWindowName = dgvPermisosUsuario.Rows(e.RowIndex).Cells(1).Value()
            sselectedControlName = dgvPermisosUsuario.Rows(e.RowIndex).Cells(2).Value()

        Catch ex As Exception

            sselectedWindowName = ""
            sselectedControlName = ""

        End Try

    End Sub


    Private Sub DgvPermisosUsuario_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPermisosUsuario.CellContentClick

        If validaDatosInicialesUsuario(True, False) = False Then
            Exit Sub
        End If

        Try

            If dgvPermisosUsuario.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            sselectedWindowName = dgvPermisosUsuario.Rows(e.RowIndex).Cells(1).Value()
            sselectedControlName = dgvPermisosUsuario.Rows(e.RowIndex).Cells(2).Value()

        Catch ex As Exception

            sselectedWindowName = ""
            sselectedControlName = ""

        End Try

    End Sub


    Private Sub DgvPermisosUsuario_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvPermisosUsuario.SelectionChanged

        txtVentanaDgvPermisosUsuario = Nothing
        txtVentanaDgvPermisosUsuario_OldText = Nothing
        txtPermisoDgvPermisosUsuario = Nothing
        txtPermisoDgvPermisosUsuario_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isPermisosUsuarioDGVReady = False Then
            Exit Sub
        End If

        Try

            If dgvPermisosUsuario.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            sselectedWindowName = dgvPermisosUsuario.CurrentRow.Cells(1).Value()
            sselectedControlName = dgvPermisosUsuario.CurrentRow.Cells(2).Value()

        Catch ex As Exception

            sselectedWindowName = ""
            sselectedControlName = ""

        End Try

    End Sub


    Private Sub dgvPermisosUsuario_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPermisosUsuario.CellDoubleClick

        If modifyPermission = False Then
            Exit Sub
        End If

        Dim ap As New AgregarPermiso

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.sselectedusername = sselectedusername
        ap.sselectedwindowname = sselectedWindowName
        ap.sselectedcontrolname = sselectedControlName

        ap.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        isPermisosUsuarioDGVReady = False

        setDataGridView(dgvPermisosUsuario, "SELECT susername AS 'Usuario', swindowname AS 'Ventana', spermission AS 'Permiso', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Act' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ORDER BY 1 ASC, 2 ASC, 3 ASC ", True)

        dgvPermisosUsuario.Columns(0).Visible = False

        If viewDgvLastModDate = False Then
            dgvPermisosUsuario.Columns(3).Visible = False
        End If

        dgvPermisosUsuario.Columns(0).Width = 100
        dgvPermisosUsuario.Columns(1).Width = 150
        dgvPermisosUsuario.Columns(2).Width = 150
        dgvPermisosUsuario.Columns(3).Width = 100

        isPermisosUsuarioDGVReady = True

    End Sub


    Private Sub dgvPermisosUsuario_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPermisosUsuario.CellContentDoubleClick

        If modifyPermission = False Then
            Exit Sub
        End If

        Dim ap As New AgregarPermiso

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.sselectedusername = sselectedusername
        ap.sselectedwindowname = sselectedWindowName
        ap.sselectedcontrolname = sselectedControlName

        ap.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        isPermisosUsuarioDGVReady = False

        setDataGridView(dgvPermisosUsuario, "SELECT susername AS 'Usuario', swindowname AS 'Ventana', spermission AS 'Permiso', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Act' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ORDER BY 1 ASC, 2 ASC, 3 ASC ", True)

        dgvPermisosUsuario.Columns(0).Visible = False

        If viewDgvLastModDate = False Then
            dgvPermisosUsuario.Columns(3).Visible = False
        End If

        dgvPermisosUsuario.Columns(0).Width = 100
        dgvPermisosUsuario.Columns(1).Width = 150
        dgvPermisosUsuario.Columns(2).Width = 150
        dgvPermisosUsuario.Columns(3).Width = 100

        isPermisosUsuarioDGVReady = True

    End Sub


    Private Sub DgvPermisosUsuario_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPermisosUsuario.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        setDataGridView(dgvPermisosUsuario, "SELECT susername AS 'Usuario', swindowname AS 'Ventana', spermission AS 'Permiso', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Act' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ORDER BY 1 ASC, 2 ASC, 3 ASC ", True)

        dgvPermisosUsuario.Columns(0).Visible = False

        If viewDgvLastModDate = False Then
            dgvPermisosUsuario.Columns(3).Visible = False
        End If

        dgvPermisosUsuario.Columns(0).Width = 100
        dgvPermisosUsuario.Columns(1).Width = 150
        dgvPermisosUsuario.Columns(2).Width = 150
        dgvPermisosUsuario.Columns(3).Width = 100

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub DgvPermisosUsuario_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvPermisosUsuario.EditingControlShowing

        If dgvPermisosUsuario.CurrentCell.ColumnIndex = 1 Then

            txtVentanaDgvPermisosUsuario = CType(e.Control, TextBox)
            txtVentanaDgvPermisosUsuario_OldText = txtVentanaDgvPermisosUsuario.Text

        ElseIf dgvPermisosUsuario.CurrentCell.ColumnIndex = 2 Then

            txtPermisoDgvPermisosUsuario = CType(e.Control, TextBox)
            txtPermisoDgvPermisosUsuario_OldText = txtPermisoDgvPermisosUsuario.Text

        Else

            txtVentanaDgvPermisosUsuario = Nothing
            txtVentanaDgvPermisosUsuario_OldText = Nothing
            txtPermisoDgvPermisosUsuario = Nothing
            txtPermisoDgvPermisosUsuario_OldText = Nothing

        End If

    End Sub


    Private Sub txtVentanaDgvPermisosUsuario_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVentanaDgvPermisosUsuario.KeyUp

        txtVentanaDgvPermisosUsuario.Text = txtVentanaDgvPermisosUsuario.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtVentanaDgvPermisosUsuario.Text.Contains(arrayForbidden1(fc)) Then
                txtVentanaDgvPermisosUsuario.Text = txtVentanaDgvPermisosUsuario.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtPermisoDgvPermisosUsuario_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPermisoDgvPermisosUsuario.KeyUp

        txtPermisoDgvPermisosUsuario.Text = txtPermisoDgvPermisosUsuario.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtPermisoDgvPermisosUsuario.Text.Contains(arrayForbidden1(fc)) Then
                txtPermisoDgvPermisosUsuario.Text = txtPermisoDgvPermisosUsuario.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub DgvPermisosUsuario_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvPermisosUsuario.UserAddedRow

        If addPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isPermisosUsuarioDGVReady = False

        Dim ap As New AgregarPermiso

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.sselectedusername = sselectedusername

        ap.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        isPermisosUsuarioDGVReady = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub DgvPermisosUsuario_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvPermisosUsuario.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deletePermission = False Then
                Exit Sub
            End If

            Try

                If dgvPermisosUsuario.CurrentRow.Index = -1 Then
                    Exit Sub
                End If

            Catch ex As Exception

                sselectedWindowName = ""
                sselectedControlName = ""
                Exit Sub

            End Try

            If MsgBox("¿Estás seguro de que quieres eliminar este Permiso del Usuario?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Permiso del Usuario") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor


                Try

                    sselectedWindowName = dgvPermisosUsuario.CurrentRow.Cells(1).Value()
                    sselectedControlName = dgvPermisosUsuario.CurrentRow.Cells(2).Value()

                Catch ex As Exception

                    sselectedWindowName = ""
                    sselectedControlName = ""

                End Try

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & sselectedusername & "' AND swindowname = '" & sselectedWindowName & "' AND spermission = '" & sselectedParentControlName & "' AND spermission = '" & sselectedControlName & "'")

                setDataGridView(dgvPermisosUsuario, "SELECT susername AS 'Usuario', swindowname AS 'Ventana', spermission AS 'Permiso', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Act' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ORDER BY 1 ASC, 2 ASC, 3 ASC ", True)

                dgvPermisosUsuario.Columns(0).Visible = False

                If viewDgvLastModDate = False Then
                    dgvPermisosUsuario.Columns(3).Visible = False
                End If

                dgvPermisosUsuario.Columns(0).Width = 100
                dgvPermisosUsuario.Columns(1).Width = 150
                dgvPermisosUsuario.Columns(2).Width = 150
                dgvPermisosUsuario.Columns(3).Width = 100

                isPermisosUsuarioDGVReady = True

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub DgvPermisosUsuario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvPermisosUsuario.Click

        If IsEdit = False And susername <> "" And validaDatosInicialesUsuario(True, False) = True Then

            dgvPermisosUsuario.Enabled = True
            btnNuevoPermiso.Enabled = True
            btnInsertarPermiso.Enabled = True
            btnEliminarPermiso.Enabled = True

        ElseIf IsEdit = False And susername = "" And validaDatosInicialesUsuario(True, False) = True Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            If validaDatosInicialesUsuario(True, False) = False Then
                Exit Sub
            Else

                dgvPermisosUsuario.Enabled = True
                btnNuevoPermiso.Enabled = True
                btnInsertarPermiso.Enabled = True
                btnEliminarPermiso.Enabled = True

            End If


            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If IsEdit = True Then

                Dim dsUsuario As DataSet
                dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

                Dim strUpdate As String = ""
                strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

                If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                    updateUsername = True
                    strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
                End If

                If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                    strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
                End If

                If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                    strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
                End If

                If txtPreguntaRescate.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                    strUpdate = strUpdate & "suserrescuequestion = '" & txtPreguntaRescate.Text & "', "
                End If

                If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                    strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
                End If

                Dim isActive As Boolean = False

                If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                    isActive = True
                Else
                    isActive = False
                End If

                If chkActivo.Checked <> isActive Then

                    If chkActivo.Checked = True Then
                        strUpdate = strUpdate & "bactive = 1, "
                    Else
                        strUpdate = strUpdate & "bactive = 0, "
                    End If

                End If

                If strUpdate.EndsWith(", ") = True Then
                    strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
                End If

                strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"

                Dim queriesUpdate(2) As String

                queriesUpdate(0) = strUpdate

                If updateUsername = True Then
                    queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

                If checkIfItsOnlyTextUpdate = True Then

                    Dim dsUsuario As DataSet
                    dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

                    Dim strUpdate As String = ""
                    strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

                    If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                        updateUsername = True
                        strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
                    End If

                    If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                        strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
                    End If

                    If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                        strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
                    End If

                    If txtPreguntaRescate.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                        strUpdate = strUpdate & "suserrescuequestion = '" & txtPreguntaRescate.Text & "', "
                    End If

                    If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                        strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
                    End If

                    Dim isActive As Boolean = False

                    If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                        isActive = True
                    Else
                        isActive = False
                    End If

                    If chkActivo.Checked <> isActive Then

                        If chkActivo.Checked = True Then
                            strUpdate = strUpdate & "bactive = 1, "
                        Else
                            strUpdate = strUpdate & "bactive = 0, "
                        End If

                    End If

                    If strUpdate.EndsWith(", ") = True Then
                        strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
                    End If

                    strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"

                    Dim queriesUpdate(2) As String

                    queriesUpdate(0) = strUpdate

                    If updateUsername = True Then
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
                    End If

                    executeTransactedSQLCommand(0, queriesUpdate)

                Else

                    sselectedusername = txtNombreUsuario.Text.ToLower
                    sselecteduserfullname = txtNombrePersona.Text

                    Dim queriesCreation(4) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User"
                    queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) NOT NULL, `suserpassword` varchar(300) CHARACTER SET latin1 NOT NULL, `suserrescuequestion` varchar(150) CHARACTER SET latin1 NOT NULL, `suserrescueanswer` varchar(150) CHARACTER SET latin1 NOT NULL, `bactive` tinyint(1) NOT NULL, `bonline` tinyint(1) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`), KEY `user&approved` (`supdateusername`,`bactive`), KEY `user&online` (`supdateusername`,`bonline`), KEY `user&updateuser` (`susername`,`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "UserPermissions"
                    queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `swindowname` varchar(300) CHARACTER SET latin1 NOT NULL, `spermission` varchar(300) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`,`swindowname`,`spermission`), KEY `windowname` (`swindowname`), KEY `username` (`susername`), KEY `updateuser` (`supdateusername`), KEY `parentpermission` (`spermission`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    executeTransactedSQLCommand(0, queriesCreation)

                    If chkActivo.Checked = True Then
                        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & txtPreguntaRescate.Text & "', '" & EncryptText(txtRespuesta.Text) & "', 1, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                    Else
                        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & txtPreguntaRescate.Text & "', '" & EncryptText(txtRespuesta.Text) & "', 0, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                    End If

                End If

            End If

            dgvPermisosUsuario.Enabled = True
            btnNuevoPermiso.Enabled = True
            btnInsertarPermiso.Enabled = True
            btnEliminarPermiso.Enabled = True

        ElseIf IsEdit = True And validaDatosInicialesUsuario(True, False) = True Then

            dgvPermisosUsuario.Enabled = True
            btnNuevoPermiso.Enabled = True
            btnInsertarPermiso.Enabled = True
            btnEliminarPermiso.Enabled = True

        End If

    End Sub


    Private Sub btnNuevoPermiso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoPermiso.Click

        If addPermission = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        If IsEdit = False And susername = "" And validaDatosInicialesUsuario(True, False) = True Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            fecha = getMySQLDate()
            hora = getAppTime()

            If validaDatosInicialesUsuario(True, False) = False Then
                Exit Sub
            Else

                dgvPermisosUsuario.Enabled = True
                btnNuevoPermiso.Enabled = True
                btnInsertarPermiso.Enabled = True
                btnEliminarPermiso.Enabled = True

            End If

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

            If checkIfItsOnlyTextUpdate = True Then

                Dim dsUsuario As DataSet
                dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

                Dim strUpdate As String = ""
                strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

                If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                    updateUsername = True
                    strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
                End If

                If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                    strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
                End If

                If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                    strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
                End If

                If txtPreguntaRescate.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                    strUpdate = strUpdate & "suserrescuequestion = '" & txtPreguntaRescate.Text & "', "
                End If

                If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                    strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
                End If

                Dim isActive As Boolean = False

                If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                    isActive = True
                Else
                    isActive = False
                End If

                If chkActivo.Checked <> isActive Then

                    If chkActivo.Checked = True Then
                        strUpdate = strUpdate & "bactive = 1, "
                    Else
                        strUpdate = strUpdate & "bactive = 0, "
                    End If

                End If

                If strUpdate.EndsWith(", ") = True Then
                    strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
                End If

                strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"

                Dim queriesUpdate(2) As String

                queriesUpdate(0) = strUpdate

                If updateUsername = True Then
                    queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                sselectedusername = txtNombreUsuario.Text.ToLower
                sselecteduserfullname = txtNombrePersona.Text

                Dim queriesCreation(4) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) NOT NULL, `suserpassword` varchar(300) CHARACTER SET latin1 NOT NULL, `suserrescuequestion` varchar(150) CHARACTER SET latin1 NOT NULL, `suserrescueanswer` varchar(150) CHARACTER SET latin1 NOT NULL, `bactive` tinyint(1) NOT NULL, `bonline` tinyint(1) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`), KEY `user&approved` (`supdateusername`,`bactive`), KEY `user&online` (`supdateusername`,`bonline`), KEY `user&updateuser` (`susername`,`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "UserPermissions"
                queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `swindowname` varchar(300) CHARACTER SET latin1 NOT NULL, `spermission` varchar(300) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`,`swindowname`,`spermission`), KEY `windowname` (`swindowname`), KEY `username` (`susername`), KEY `updateuser` (`supdateusername`), KEY `parentpermission` (`spermission`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                If chkActivo.Checked = True Then
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & txtPreguntaRescate.Text & "', '" & EncryptText(txtRespuesta.Text) & "', 1, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                Else
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & txtPreguntaRescate.Text & "', '" & EncryptText(txtRespuesta.Text) & "', 0, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                End If

            End If

        End If

        'Inicia Código de botón Nuevo Permiso

        isPermisosUsuarioDGVReady = False


        Dim ap As New AgregarPermiso

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.sselectedusername = sselectedusername

        ap.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True


        setDataGridView(dgvPermisosUsuario, "SELECT susername AS 'Usuario', swindowname AS 'Ventana', spermission AS 'Permiso', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Act' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ORDER BY 1 ASC, 2 ASC, 3 ASC ", True)

        dgvPermisosUsuario.Columns(0).Visible = False

        If viewDgvLastModDate = False Then
            dgvPermisosUsuario.Columns(3).Visible = False
        End If

        dgvPermisosUsuario.Columns(0).Width = 100
        dgvPermisosUsuario.Columns(1).Width = 150
        dgvPermisosUsuario.Columns(2).Width = 150
        dgvPermisosUsuario.Columns(3).Width = 100

        isPermisosUsuarioDGVReady = True


    End Sub


    Private Sub btnInsertarPermiso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarPermiso.Click

        If insertPermission = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        If IsEdit = False And susername = "" And validaDatosInicialesUsuario(True, False) = True Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            fecha = getMySQLDate()
            hora = getAppTime()

            If validaDatosInicialesUsuario(True, False) = False Then
                Exit Sub
            Else

                dgvPermisosUsuario.Enabled = True
                btnNuevoPermiso.Enabled = True
                btnInsertarPermiso.Enabled = True
                btnEliminarPermiso.Enabled = True

            End If

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

            If checkIfItsOnlyTextUpdate = True Then

                Dim dsUsuario As DataSet
                dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

                Dim strUpdate As String = ""
                strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

                If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                    updateUsername = True
                    strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
                End If

                If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                    strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
                End If

                If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                    strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
                End If

                If txtPreguntaRescate.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                    strUpdate = strUpdate & "suserrescuequestion = '" & txtPreguntaRescate.Text & "', "
                End If

                If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                    strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
                End If

                Dim isActive As Boolean = False

                If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                    isActive = True
                Else
                    isActive = False
                End If

                If chkActivo.Checked <> isActive Then

                    If chkActivo.Checked = True Then
                        strUpdate = strUpdate & "bactive = 1, "
                    Else
                        strUpdate = strUpdate & "bactive = 0, "
                    End If

                End If

                If strUpdate.EndsWith(", ") = True Then
                    strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
                End If

                strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"

                Dim queriesUpdate(2) As String

                queriesUpdate(0) = strUpdate

                If updateUsername = True Then
                    queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                sselectedusername = txtNombreUsuario.Text.ToLower
                sselecteduserfullname = txtNombrePersona.Text

                Dim queriesCreation(4) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) NOT NULL, `suserpassword` varchar(300) CHARACTER SET latin1 NOT NULL, `suserrescuequestion` varchar(150) CHARACTER SET latin1 NOT NULL, `suserrescueanswer` varchar(150) CHARACTER SET latin1 NOT NULL, `bactive` tinyint(1) NOT NULL, `bonline` tinyint(1) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`), KEY `user&approved` (`supdateusername`,`bactive`), KEY `user&online` (`supdateusername`,`bonline`), KEY `user&updateuser` (`susername`,`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "UserPermissions"
                queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `swindowname` varchar(300) CHARACTER SET latin1 NOT NULL, `spermission` varchar(300) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`,`swindowname`,`spermission`), KEY `windowname` (`swindowname`), KEY `username` (`susername`), KEY `updateuser` (`supdateusername`), KEY `parentpermission` (`spermission`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                If chkActivo.Checked = True Then
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & txtPreguntaRescate.Text & "', '" & EncryptText(txtRespuesta.Text) & "', 1, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                Else
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & txtPreguntaRescate.Text & "', '" & EncryptText(txtRespuesta.Text) & "', 0, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                End If

            End If

        End If

        'Inicia Código de botón Insertar Permiso


        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isPermisosUsuarioDGVReady = False

        Dim bp As New BuscaPermisos

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            If bp.selectedMany = True Then

                Try

                    If bp.swindows.Length > 0 And bp.spermissions.Length > 0 Then

                        fecha = getMySQLDate()
                        hora = getAppTime()

                        Dim queryInsert(bp.swindows.Length) As String

                        For i = 0 To bp.swindows.Length - 1

                            queryInsert(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SELECT '" & sselectedusername & "', '" & bp.swindows(i) & "', '" & bp.spermissions(i) & "', " & fecha & ", '" & hora & "', '" & susername & "'"

                        Next i

                        executeTransactedSQLCommand(0, queryInsert)

                    End If

                Catch ex As Exception

                    MsgBox("Error Importando Permisos", MsgBoxStyle.Information, "Error")

                End Try

            Else

                fecha = getMySQLDate()
                hora = getAppTime()

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SELECT '" & sselectedusername & "', '" & bp.sselectedwindowname & "', '" & bp.sselectedpermission & "', " & fecha & ", '" & hora & "', '" & susername & "'")

            End If

        End If


        setDataGridView(dgvPermisosUsuario, "SELECT susername AS 'Usuario', swindowname AS 'Ventana', spermission AS 'Permiso', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Act' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ORDER BY 1 ASC, 2 ASC, 3 ASC ", True)

        dgvPermisosUsuario.Columns(0).Visible = False

        If viewDgvLastModDate = False Then
            dgvPermisosUsuario.Columns(3).Visible = False
        End If

        dgvPermisosUsuario.Columns(0).Width = 100
        dgvPermisosUsuario.Columns(1).Width = 150
        dgvPermisosUsuario.Columns(2).Width = 150
        dgvPermisosUsuario.Columns(3).Width = 100

        isPermisosUsuarioDGVReady = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarPermiso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarPermiso.Click

        If deletePermission = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        If IsEdit = False And susername = "" And validaDatosInicialesUsuario(True, False) = True Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            fecha = getMySQLDate()
            hora = getAppTime()

            If validaDatosInicialesUsuario(True, False) = False Then
                Exit Sub
            Else

                dgvPermisosUsuario.Enabled = True
                btnNuevoPermiso.Enabled = True
                btnInsertarPermiso.Enabled = True
                btnEliminarPermiso.Enabled = True

            End If

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

            If checkIfItsOnlyTextUpdate = True Then

                Dim dsUsuario As DataSet
                dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

                Dim strUpdate As String = ""
                strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

                If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                    updateUsername = True
                    strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
                End If

                If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                    strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
                End If

                If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                    strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
                End If

                If txtPreguntaRescate.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                    strUpdate = strUpdate & "suserrescuequestion = '" & txtPreguntaRescate.Text & "', "
                End If

                If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                    strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
                End If

                Dim isActive As Boolean = False

                If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                    isActive = True
                Else
                    isActive = False
                End If

                If chkActivo.Checked <> isActive Then

                    If chkActivo.Checked = True Then
                        strUpdate = strUpdate & "bactive = 1, "
                    Else
                        strUpdate = strUpdate & "bactive = 0, "
                    End If

                End If

                If strUpdate.EndsWith(", ") = True Then
                    strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
                End If

                strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"

                Dim queriesUpdate(2) As String

                queriesUpdate(0) = strUpdate

                If updateUsername = True Then
                    queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                sselectedusername = txtNombreUsuario.Text.ToLower
                sselecteduserfullname = txtNombrePersona.Text

                Dim queriesCreation(4) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) NOT NULL, `suserpassword` varchar(300) CHARACTER SET latin1 NOT NULL, `suserrescuequestion` varchar(150) CHARACTER SET latin1 NOT NULL, `suserrescueanswer` varchar(150) CHARACTER SET latin1 NOT NULL, `bactive` tinyint(1) NOT NULL, `bonline` tinyint(1) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`), KEY `user&approved` (`supdateusername`,`bactive`), KEY `user&online` (`supdateusername`,`bonline`), KEY `user&updateuser` (`susername`,`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "UserPermissions"
                queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `swindowname` varchar(300) CHARACTER SET latin1 NOT NULL, `spermission` varchar(300) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`,`swindowname`,`spermission`), KEY `windowname` (`swindowname`), KEY `username` (`susername`), KEY `updateuser` (`supdateusername`), KEY `parentpermission` (`spermission`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                If chkActivo.Checked = True Then
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & txtPreguntaRescate.Text & "', '" & EncryptText(txtRespuesta.Text) & "', 1, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                Else
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & txtPreguntaRescate.Text & "', '" & EncryptText(txtRespuesta.Text) & "', 0, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                End If

            End If

        End If


        'Inicia Código de botón Eliminar Permiso

        Try

            If dgvPermisosUsuario.CurrentRow.Index = -1 Then
                Exit Sub
            End If

        Catch ex As Exception

            sselectedWindowName = ""
            sselectedParentControlName = ""
            sselectedControlName = ""
            Exit Sub

        End Try

        If MsgBox("¿Estás seguro de que quieres eliminar este Permiso del Usuario?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Permiso del Usuario") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor


            Try

                sselectedWindowName = dgvPermisosUsuario.CurrentRow.Cells(1).Value()
                sselectedControlName = dgvPermisosUsuario.CurrentRow.Cells(2).Value()

            Catch ex As Exception

                sselectedWindowName = ""
                sselectedControlName = ""

            End Try

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions WHERE susername = '" & sselectedusername & "' AND swindowname = '" & sselectedWindowName & "' AND spermission = '" & sselectedControlName & "'")

            setDataGridView(dgvPermisosUsuario, "SELECT susername AS 'Usuario', swindowname AS 'Ventana', spermission AS 'Permiso', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Act' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ORDER BY 1 ASC, 2 ASC, 3 ASC ", True)

            dgvPermisosUsuario.Columns(0).Visible = False

            If viewDgvLastModDate = False Then
                dgvPermisosUsuario.Columns(3).Visible = False
            End If

            dgvPermisosUsuario.Columns(0).Width = 100
            dgvPermisosUsuario.Columns(1).Width = 150
            dgvPermisosUsuario.Columns(2).Width = 150
            dgvPermisosUsuario.Columns(3).Width = 100

            isPermisosUsuarioDGVReady = True

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'sselectedusername = ""
        'sselecteduserfullname = ""

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaDatosInicialesUsuario(False, True) = False Then
            Exit Sub
        End If

        Dim timesUserIsOpen As Integer = 1

        timesUserIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%User" & sselectedusername & "'")

        If timesUserIsOpen > 1 And IsEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Usuario. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesUserIsOpen > 1 And IsEdit = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario esta ingresando un usuario con tu mismo Nombre de Usuario. Esto podría causar que alguno de ustedes perdiera sus datos/cambios en el archivo. Recomiendo cambiar de Nombre de Usuario. ¿Deseas continuar guardando con este nombre de usuario (Sí) o Desear regresar a cambiarlo (No)?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If


        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If IsEdit = True Then

            Dim dsUsuario As DataSet
            dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

            Dim strUpdate As String = ""
            strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

            If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                updateUsername = True
                strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
            End If

            If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
            End If

            If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
            End If

            If base64Encode(txtPreguntaRescate.Text).Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                strUpdate = strUpdate & "suserrescuequestion = '" & base64Encode(txtPreguntaRescate.Text) & "', "
            End If

            If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
            End If

            Dim isActive As Boolean = False

            If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                isActive = True
            Else
                isActive = False
            End If

            If chkActivo.Checked <> isActive Then

                If chkActivo.Checked = True Then
                    strUpdate = strUpdate & "bactive = 1, "
                Else
                    strUpdate = strUpdate & "bactive = 0, "
                End If

            End If

            If strUpdate.EndsWith(", ") = True Then
                strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
            End If

            strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"

            Dim queriesUpdate(2) As String

            queriesUpdate(0) = strUpdate

            If updateUsername = True Then
                queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
            End If

            executeTransactedSQLCommand(0, queriesUpdate)

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

            If checkIfItsOnlyTextUpdate = True Then

                Dim dsUsuario As DataSet
                dsUsuario = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " WHERE susername = '" & sselectedusername & "'")

                Dim strUpdate As String = ""
                strUpdate = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', "

                If txtNombreUsuario.Text.Trim.ToLower <> dsUsuario.Tables(0).Rows(0).Item("susername") Then
                    updateUsername = True
                    strUpdate = strUpdate & "susername = '" & txtNombreUsuario.Text.ToLower & "', "
                End If

                If ipeopleid <> dsUsuario.Tables(0).Rows(0).Item("ipeopleid") Then
                    strUpdate = strUpdate & "ipeopleid = " & ipeopleid & ", "
                End If

                If txtPassword.Text.Trim <> "" And txtConfirmarPassword.Text.Trim <> "" And txtPassword.Text.Trim = txtConfirmarPassword.Text.Trim And EncryptText(txtPassword.Text.Trim) <> dsUsuario.Tables(0).Rows(0).Item("suserpassword") Then
                    strUpdate = strUpdate & "suserpassword = '" & EncryptText(txtPassword.Text.Trim) & "', "
                End If

                If base64Encode(txtPreguntaRescate.Text).Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescuequestion") Then
                    strUpdate = strUpdate & "suserrescuequestion = '" & base64Encode(txtPreguntaRescate.Text) & "', "
                End If

                If txtRespuesta.Text.Trim <> dsUsuario.Tables(0).Rows(0).Item("suserrescueanswer") And txtRespuesta.Text.Trim <> "" Then
                    strUpdate = strUpdate & "suserrescueanswer = '" & EncryptText(txtRespuesta.Text) & "', "
                End If

                Dim isActive As Boolean = False

                If dsUsuario.Tables(0).Rows(0).Item("bactive") = "1" Then
                    isActive = True
                Else
                    isActive = False
                End If

                If chkActivo.Checked <> isActive Then

                    If chkActivo.Checked = True Then
                        strUpdate = strUpdate & "bactive = 1, "
                    Else
                        strUpdate = strUpdate & "bactive = 0, "
                    End If

                End If

                If strUpdate.EndsWith(", ") = True Then
                    strUpdate = strUpdate.Substring(0, strUpdate.Length - 2) & " "
                End If

                strUpdate = strUpdate & "WHERE susername = '" & sselectedusername & "'"

                Dim queriesUpdate(2) As String

                queriesUpdate(0) = strUpdate

                If updateUsername = True Then
                    queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                sselectedusername = txtNombreUsuario.Text.ToLower
                sselecteduserfullname = txtNombrePersona.Text

                Dim queriesCreation(4) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) NOT NULL, `suserpassword` varchar(300) CHARACTER SET latin1 NOT NULL, `suserrescuequestion` varchar(150) CHARACTER SET latin1 NOT NULL, `suserrescueanswer` varchar(150) CHARACTER SET latin1 NOT NULL, `bactive` tinyint(1) NOT NULL, `bonline` tinyint(1) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`), KEY `user&approved` (`supdateusername`,`bactive`), KEY `user&online` (`supdateusername`,`bonline`), KEY `user&updateuser` (`susername`,`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "UserPermissions"
                queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions ( `susername` varchar(100) CHARACTER SET latin1 NOT NULL, `swindowname` varchar(300) CHARACTER SET latin1 NOT NULL, `spermission` varchar(300) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`susername`,`swindowname`,`spermission`), KEY `windowname` (`swindowname`), KEY `username` (`susername`), KEY `updateuser` (`supdateusername`), KEY `parentpermission` (`spermission`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                If chkActivo.Checked = True Then
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & base64Encode(txtPreguntaRescate.Text) & "', '" & EncryptText(txtRespuesta.Text) & "', 1, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                Else
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " VALUES ('" & sselectedusername & "', " & ipeopleid & ", '" & EncryptText(txtPassword.Text) & "', '" & base64Encode(txtPreguntaRescate.Text) & "', '" & EncryptText(txtRespuesta.Text) & "', 0, 0, " & fecha & ", '" & hora & "', '" & susername & "')")
                End If

            End If

        End If


        Dim queries(7) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM users " & _
        "WHERE susername = '" & sselectedusername & "' AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc WHERE users.susername = tclc.susername) "

        queries(1) = "" & _
        "UPDATE users clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc ON tclc.susername = clc.susername SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipeopleid = tclc.ipeopleid, clc.susername = tclc.susername, clc.suserpassword = tclc.suserpassword, clc.suserrescuequestion = tclc.suserrescuequestion, clc.suserrescueanswer = tclc.suserrescueanswer, clc.bactive = tclc.bactive WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO users " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM users clc WHERE tclc.susername = clc.susername AND clc.susername = '" & sselectedusername & "') "

        queries(3) = "" & _
        "DELETE " & _
        "FROM userpermissions " & _
        "WHERE susername = '" & sselectedusername & "' AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc WHERE userpermissions.susername = tclc.susername AND userpermissions.swindowname = tclc.swindowname AND userpermissions.spermission = tclc.spermission) "

        queries(4) = "" & _
        "UPDATE userpermissions clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc ON tclc.susername = clc.susername AND tclc.swindowname = clc.swindowname AND tclc.spermission = clc.spermission SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.susername = tclc.susername, clc.swindowname = tclc.swindowname, clc.spermission = tclc.spermission WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(5) = "" & _
        "INSERT INTO userpermissions " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM userpermissions clc WHERE tclc.susername = clc.susername AND tclc.swindowname = clc.swindowname AND tclc.spermission = clc.spermission AND clc.susername = '" & sselectedusername & "') "

        queries(6) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Usuario " & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & " y sus Permisos', 'OK')"

        If updateUsername = True Then

            Dim queriesUpdate(90) As String

            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "User" & sselectedusername & "Permissions SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "'"
            queriesUpdate(1) = queries(0)
            queriesUpdate(2) = queries(1)
            queriesUpdate(3) = queries(2)
            queriesUpdate(4) = queries(3)
            queriesUpdate(5) = queries(4)
            queriesUpdate(6) = queries(5)

            queriesUpdate(7) = "UPDATE assets SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(8) = "UPDATE assetsphysicalinventory SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(9) = "UPDATE assetstypes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(10) = "UPDATE banks SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(11) = "UPDATE base SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(12) = "UPDATE basecardcompoundinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(13) = "UPDATE basecardinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(14) = "UPDATE basecards SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(15) = "UPDATE baseindirectcosts SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(16) = "UPDATE baseprices SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(17) = "UPDATE basetimber SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(18) = "UPDATE cardlegacycategories SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(19) = "UPDATE companyaccounts SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(20) = "UPDATE companyinfo SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(21) = "UPDATE gastypes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(23) = "UPDATE gasvoucherprojects SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(24) = "UPDATE gasvouchers SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(25) = "UPDATE incomeassets SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(26) = "UPDATE incomeprojects SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(27) = "UPDATE incomes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(28) = "UPDATE incometypes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(29) = "UPDATE inputcategories SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(30) = "UPDATE inputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(31) = "UPDATE inputsphysicalinventory SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(32) = "UPDATE inputtypes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(33) = "UPDATE logs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(34) = "UPDATE messages SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(35) = "UPDATE messages SET smessagecreatorusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE smessagecreatorusername = '" & susername & "'"
            queriesUpdate(36) = "UPDATE messages SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE susername = '" & susername & "'"
            queriesUpdate(37) = "UPDATE modelcardcompoundinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"

            queriesUpdate(22) = "UPDATE modelcardinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"

            queriesUpdate(38) = "UPDATE modelcards SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(39) = "UPDATE modelindirectcosts SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(40) = "UPDATE modelprices SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"

            queriesUpdate(87) = "UPDATE models SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(88) = "UPDATE modeltimber SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"

            queriesUpdate(41) = "UPDATE orderinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(42) = "UPDATE orders SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(43) = "UPDATE payments SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(44) = "UPDATE paymenttypes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(45) = "UPDATE payrollpayments SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(46) = "UPDATE payrollpeople SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(47) = "UPDATE payrolls SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(48) = "UPDATE people SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(49) = "UPDATE peoplephonenumbers SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"

            queriesUpdate(89) = "UPDATE projectadmincosts SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"

            queriesUpdate(50) = "UPDATE projectcardcompoundinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(51) = "UPDATE projectcardinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(52) = "UPDATE projectcards SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(53) = "UPDATE projectexplosion SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(54) = "UPDATE projectindirectcosts SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(55) = "UPDATE projectprices SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(56) = "UPDATE projects SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(57) = "UPDATE projecttimber SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            'queriesUpdate(58) = "UPDATE quotationinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            'queriesUpdate(59) = "UPDATE quotations SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(60) = "UPDATE recentlyopenedfiles SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE susername = '" & susername & "'"
            queriesUpdate(61) = "UPDATE recentlyopenedfiles SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(62) = "UPDATE reportcolumnssizes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(63) = "UPDATE reports SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(64) = "UPDATE revisionresults SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(65) = "UPDATE revisions SET srevisionusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE srevisionusername = '" & susername & "'"
            queriesUpdate(66) = "UPDATE revisions SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(67) = "UPDATE sessions SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "' AND ilogoutdate IS NOT NULL"
            queriesUpdate(68) = "UPDATE shipmentcarused SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(69) = "UPDATE shipmentinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(70) = "UPDATE shipments SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(71) = "UPDATE shipmentsuppliers SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(72) = "UPDATE suppliercontacts SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(73) = "UPDATE supplierestimations SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(74) = "UPDATE supplierinvoiceassets SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(75) = "UPDATE supplierinvoicediscounts SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(76) = "UPDATE supplierinvoicegasvouchers SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(77) = "UPDATE supplierinvoiceinputs SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(78) = "UPDATE supplierinvoicepayments SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(79) = "UPDATE supplierinvoiceprojects SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(80) = "UPDATE supplierinvoices SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(81) = "UPDATE supplierinvoicetypes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(82) = "UPDATE supplierphonenumbers SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(83) = "UPDATE suppliers SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(84) = "UPDATE transformationunits SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"

            queriesUpdate(85) = "UPDATE userspecialattributes SET supdateusername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE supdateusername = '" & susername & "'"
            queriesUpdate(86) = "UPDATE userspecialattributes SET susername = '" & txtNombreUsuario.Text.Replace("--", "").Replace("'", "") & "' WHERE susername = '" & susername & "'"

            dropLastUserNameTables = True
            slastusername = sselectedusername
            sselectedusername = txtNombreUsuario.Text.Replace("--", "").Replace("'", "")

            If executeTransactedSQLCommand(0, queriesUpdate) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If


        Else

            If executeTransactedSQLCommand(0, queries) = True Then

                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tcUsuario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcUsuario.SelectedIndexChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If sselectedusername <> "" Then

            If tcUsuario.SelectedTab Is tpSesionesRestricciones Then

                Dim querySesiones As String = ""

                If chkSesionesHistoricas.Checked = True Then

                    querySesiones = "" & _
                    "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
                    "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
                    "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
                    "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
                    "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
                    "FROM sessions " & _
                    "WHERE susername = '" & sselectedusername & "' " & _
                    "ORDER BY 8, 2 "

                Else

                    querySesiones = "" & _
                    "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
                    "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
                    "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
                    "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
                    "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
                    "FROM sessions " & _
                    "WHERE susername = '" & sselectedusername & "' " & _
                    "AND ilogoutdate IS NULL " & _
                    "ORDER BY 8, 2 "

                End If

                setDataGridView(dgvSesionesAbiertas, querySesiones, True)

                dgvSesionesAbiertas.Columns(0).Visible = False
                dgvSesionesAbiertas.Columns(6).Visible = False
                dgvSesionesAbiertas.Columns(7).Visible = False



            ElseIf tcUsuario.SelectedTab Is tpTablaAccionesUsuario Then

                txtBuscarLog.Text = ""

                Dim queryLogs As String = ""

                queryLogs = "" & _
                "SELECT STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha / Hora', supdateusername AS 'Usuario', susersession AS 'Sesion', suserip AS 'IP', susermachinename AS 'Nombre de Maquina', sactiondone AS 'Accion hecha', sresult AS 'Resultado' FROM logs " & _
                "WHERE supdateusername = '" & sselectedusername & "' AND (sactiondone LIKE '%" & txtBuscarLog.Text.Replace("--", "").Replace("'", "") & "%' OR CONCAT(STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T'), '') LIKE '%" & txtBuscarLog.Text.Replace("--", "").Replace("'", "") & "%') " & _
                "ORDER BY 1 DESC, 2 DESC, 4 ASC "

                setDataGridView(dgvLogs, queryLogs, True)

                dgvLogs.Columns(1).Visible = False

                dgvLogs.Columns(0).Width = 130
                dgvLogs.Columns(1).Width = 100
                dgvLogs.Columns(2).Width = 50
                dgvLogs.Columns(3).Width = 100
                dgvLogs.Columns(4).Width = 100
                dgvLogs.Columns(5).Width = 350
                dgvLogs.Columns(6).Width = 70

            End If

        Else
            tcUsuario.SelectedTab = tpAgregarUsuario
        End If

    End Sub


    Private Sub txtBuscarLog_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBuscarLog.KeyUp

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim strcaracteresprohibidos As String = "|°!#$&/()=?¡*¨[]_:;,{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtBuscarLog.Text.Contains(arrayCaractProhib(carp)) Then
                txtBuscarLog.Text = txtBuscarLog.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtBuscarLog.Select(txtBuscarLog.Text.Length, 0)
        End If

        txtBuscarLog.Text = txtBuscarLog.Text.Replace("--", "").Replace("'", "")



    End Sub


    Private Sub btnLockout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLockout.Click

        Dim queries(2) As String

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim kickMyselfOut As Boolean = False

        queries(0) = "UPDATE sessions SET blockedout = 1, ilogoutdate = " & fecha & ", slogouttime = '" & hora & "' WHERE susername = '" & sselectedusername & "' AND susersession = " & iselectedsessionid & " AND ilogindate = " & iselectedlogindate & " AND slogintime = '" & sselectedlogintime & "'"

        If sselectedusername = susername Then

            If iselectedsessionid = susersession Then
                queries(1) = "UPDATE users SET bonline = 0 WHERE susername = '" & sselectedusername & "'"
                kickMyselfOut = True
            End If

        Else
            queries(1) = "UPDATE users SET bonline = 0 WHERE susername = '" & sselectedusername & "'"
        End If

        If executeTransactedSQLCommand(0, queries) = True Then

            If kickMyselfOut = True Then

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

                Exit Sub

            End If

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Dejo afuera del sistema al Usuario " & sselectedusername & "', 'OK')")

            Dim querySesiones As String = ""

            If chkSesionesHistoricas.Checked = True Then

                querySesiones = "" & _
                "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
                "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
                "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
                "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
                "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
                "FROM sessions " & _
                "WHERE susername = '" & sselectedusername & "' " & _
                "ORDER BY 8, 2 "

            Else

                querySesiones = "" & _
                "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
                "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
                "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
                "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
                "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
                "FROM sessions " & _
                "WHERE susername = '" & sselectedusername & "' " & _
                "AND ilogoutdate IS NULL " & _
                "ORDER BY 8, 2 "

            End If

            setDataGridView(dgvSesionesAbiertas, querySesiones, True)

            dgvSesionesAbiertas.Columns(0).Visible = False
            dgvSesionesAbiertas.Columns(6).Visible = False
            dgvSesionesAbiertas.Columns(7).Visible = False

        End If

    End Sub


    Private Sub chkSesionesHistoricas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSesionesHistoricas.CheckedChanged

        Dim querySesiones As String = ""

        If chkSesionesHistoricas.Checked = True Then

            querySesiones = "" & _
            "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
            "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
            "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
            "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
            "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
            "FROM sessions " & _
            "WHERE susername = '" & sselectedusername & "' " & _
            "ORDER BY 8, 2 "

        Else

            querySesiones = "" & _
            "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
            "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
            "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
            "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
            "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
            "FROM sessions " & _
            "WHERE susername = '" & sselectedusername & "' " & _
            "AND ilogoutdate IS NULL " & _
            "ORDER BY 8, 2 "

        End If

        setDataGridView(dgvSesionesAbiertas, querySesiones, True)

        dgvSesionesAbiertas.Columns(0).Visible = False
        dgvSesionesAbiertas.Columns(6).Visible = False
        dgvSesionesAbiertas.Columns(7).Visible = False


    End Sub


    Private Sub btnKickout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKickout.Click

        Dim queries(2) As String

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim kickMyselfOut As Boolean = True

        queries(0) = "UPDATE sessions SET bkickedout = 1, ilogoutdate = " & fecha & ", slogouttime = '" & hora & "' WHERE susername = '" & sselectedusername & "' AND susersession = " & iselectedsessionid & " AND ilogindate = " & iselectedlogindate & " AND slogintime = '" & sselectedlogintime & "'"

        If sselectedusername = susername Then

            If iselectedsessionid = susersession Then
                queries(1) = "UPDATE users SET bonline = 0 WHERE susername = '" & sselectedusername & "'"
                kickMyselfOut = True
            End If

        Else
            queries(1) = "UPDATE users SET bonline = 0 WHERE susername = '" & sselectedusername & "'"
        End If


        If executeTransactedSQLCommand(0, queries) = True Then

            If kickMyselfOut = True Then

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

                Exit Sub

            End If

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Saco del sistema al Usuario " & sselectedusername & "', 'OK')")

            Dim querySesiones As String = ""

            If chkSesionesHistoricas.Checked = True Then

                querySesiones = "" & _
                "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
                "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
                "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
                "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
                "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
                "FROM sessions " & _
                "WHERE susername = '" & sselectedusername & "' " & _
                "ORDER BY 8, 2 "

            Else

                querySesiones = "" & _
                "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
                "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
                "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
                "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
                "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
                "FROM sessions " & _
                "WHERE susername = '" & sselectedusername & "' " & _
                "AND ilogoutdate IS NULL " & _
                "ORDER BY 8, 2 "

            End If

            setDataGridView(dgvSesionesAbiertas, querySesiones, True)

            dgvSesionesAbiertas.Columns(0).Visible = False
            dgvSesionesAbiertas.Columns(6).Visible = False
            dgvSesionesAbiertas.Columns(7).Visible = False

        End If

    End Sub


    Private Sub dgvSesionesAbiertas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSesionesAbiertas.CellClick

        Try

            If dgvSesionesAbiertas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedsessionid = CInt(dgvSesionesAbiertas.Rows(e.RowIndex).Cells(1).Value())
            iselectedlogindate = CInt(dgvSesionesAbiertas.Rows(e.RowIndex).Cells(6).Value())
            sselectedlogintime = dgvSesionesAbiertas.Rows(e.RowIndex).Cells(7).Value()
            sselectedIP = dgvSesionesAbiertas.Rows(e.RowIndex).Cells(4).Value()
            sselectedMachineName = dgvSesionesAbiertas.Rows(e.RowIndex).Cells(5).Value()

        Catch ex As Exception

            iselectedsessionid = 0
            iselectedlogindate = 0
            sselectedlogintime = 0
            sselectedIP = ""
            sselectedMachineName = ""

        End Try

    End Sub


    Private Sub dgvSesionesAbiertas_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSesionesAbiertas.CellContentClick

        Try

            If dgvSesionesAbiertas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedsessionid = CInt(dgvSesionesAbiertas.Rows(e.RowIndex).Cells(1).Value())
            iselectedlogindate = CInt(dgvSesionesAbiertas.Rows(e.RowIndex).Cells(6).Value())
            sselectedlogintime = dgvSesionesAbiertas.Rows(e.RowIndex).Cells(7).Value()
            sselectedIP = dgvSesionesAbiertas.Rows(e.RowIndex).Cells(4).Value()
            sselectedMachineName = dgvSesionesAbiertas.Rows(e.RowIndex).Cells(5).Value()

        Catch ex As Exception

            iselectedsessionid = 0
            iselectedlogindate = 0
            sselectedlogintime = 0
            sselectedIP = ""
            sselectedMachineName = ""

        End Try

    End Sub


    Private Sub dgvSesionesAbiertas_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvSesionesAbiertas.SelectionChanged

        Try

            If dgvSesionesAbiertas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedsessionid = CInt(dgvSesionesAbiertas.CurrentRow.Cells(1).Value())
            iselectedlogindate = CInt(dgvSesionesAbiertas.CurrentRow.Cells(6).Value())
            sselectedlogintime = dgvSesionesAbiertas.CurrentRow.Cells(7).Value()
            sselectedIP = dgvSesionesAbiertas.CurrentRow.Cells(4).Value()
            sselectedMachineName = dgvSesionesAbiertas.CurrentRow.Cells(5).Value()

        Catch ex As Exception

            iselectedsessionid = 0
            iselectedlogindate = 0
            sselectedlogintime = 0
            sselectedIP = ""
            sselectedMachineName = ""

        End Try

    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Dim querySesiones As String = ""

        If chkSesionesHistoricas.Checked = True Then

            querySesiones = "" & _
            "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
            "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
            "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
            "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
            "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
            "FROM sessions " & _
            "WHERE susername = '" & sselectedusername & "' " & _
            "ORDER BY 8, 2 "

        Else

            querySesiones = "" & _
            "SELECT susername, susersession AS Sesion, IF(bloggedinsuccesfully = 1, 'Concedido', 'DENEGADO') AS 'Acceso', " & _
            "IF(blockedout = 1, 'Sysadmin evito futuros loggeos', IF(bkickedout = 1, 'Sacado por el Sysadmin', '')) AS 'Acciones', " & _
            "suserip AS 'IP', susermachinename AS 'Nombre Maquina', ilogindate, slogintime, " & _
            "STR_TO_DATE(CONCAT(ilogindate, ' ', slogintime), '%Y%c%d %T') AS 'Fecha Login', " & _
            "STR_TO_DATE(CONCAT(ilogoutdate, ' ', slogouttime), '%Y%c%d %T') AS 'Fecha Logout' " & _
            "FROM sessions " & _
            "WHERE susername = '" & sselectedusername & "' " & _
            "AND ilogoutdate IS NULL " & _
            "ORDER BY 8, 2 "

        End If

        setDataGridView(dgvSesionesAbiertas, querySesiones, True)

        dgvSesionesAbiertas.Columns(0).Visible = False
        dgvSesionesAbiertas.Columns(6).Visible = False
        dgvSesionesAbiertas.Columns(7).Visible = False


        Dim queryLogs As String = ""

        queryLogs = "" & _
        "SELECT STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha / Hora', supdateusername AS 'Usuario', susersession AS 'Sesion', suserip AS 'IP', susermachinename AS 'Nombre de Maquina', sactiondone AS 'Accion hecha', sresult AS 'Resultado' FROM logs " & _
        "WHERE supdateusername = '" & sselectedusername & "' AND (sactiondone LIKE '%" & txtBuscarLog.Text.Replace("--", "").Replace("'", "") & "%' OR CONCAT(STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T'), '') LIKE '%" & txtBuscarLog.Text.Replace("--", "").Replace("'", "") & "%') " & _
        "ORDER BY 1 DESC, 2 DESC, 4 ASC "

        setDataGridView(dgvLogs, queryLogs, True)

        dgvLogs.Columns(1).Visible = False

        dgvLogs.Columns(0).Width = 130
        dgvLogs.Columns(1).Width = 100
        dgvLogs.Columns(2).Width = 50
        dgvLogs.Columns(3).Width = 100
        dgvLogs.Columns(4).Width = 100
        dgvLogs.Columns(5).Width = 350
        dgvLogs.Columns(6).Width = 70

    End Sub


    Private Sub txtBuscarLog_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscarLog.TextChanged

        Dim queryLogs As String = ""

        queryLogs = "" & _
        "SELECT STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha / Hora', supdateusername AS 'Usuario', susersession AS 'Sesion', suserip AS 'IP', susermachinename AS 'Nombre de Maquina', sactiondone AS 'Accion hecha', sresult AS 'Resultado' FROM logs " & _
        "WHERE supdateusername = '" & sselectedusername & "' AND (sactiondone LIKE '%" & txtBuscarLog.Text.Replace("--", "").Replace("'", "") & "%' OR CONCAT(STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T'), '') LIKE '%" & txtBuscarLog.Text.Replace("--", "").Replace("'", "") & "%') " & _
        "ORDER BY 1 DESC, 2 DESC, 4 ASC "

        setDataGridView(dgvLogs, queryLogs, True)

        dgvLogs.Columns(1).Visible = False

        dgvLogs.Columns(0).Width = 130
        dgvLogs.Columns(1).Width = 100
        dgvLogs.Columns(2).Width = 50
        dgvLogs.Columns(3).Width = 100
        dgvLogs.Columns(4).Width = 100
        dgvLogs.Columns(5).Width = 350
        dgvLogs.Columns(6).Width = 70

        txtBuscarLog.Focus()
        txtBuscarLog.SelectionStart() = txtBuscarLog.Text.Length

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class