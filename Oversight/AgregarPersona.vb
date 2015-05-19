Public Class AgregarPersona

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

    Public ipeopleid As Integer = 0
    Public speoplefullname As String = ""

    Private iselectedphoneid As Integer = 0
    Private sselectedphone As String = ""
    Private sselectedphonetype As String = ""

    Private WithEvents txtNumeroDgvTelefonos As TextBox
    Private WithEvents txtTipoTelefonoDgvTelefonos As TextBox
    Private txtTipoTelefonoDgvTelefonos_OldText As String = ""
    Private txtNumeroDgvTelefonos_OldText As String = ""

    Private openPermission As Boolean = False

    Private addPhonePermission As Boolean = False
    Private modifyPhonePermission As Boolean = False
    Private deletePhonePermission As Boolean = False

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

                If permission = "Abrir Direccion" Then
                    openPermission = True
                    btnDireccion.Enabled = True
                End If

                If permission = "Agregar Telefono" Then
                    addPhonePermission = True
                    btnNuevoTelefono.Enabled = True
                End If

                If permission = "Modificar Telefono" Then
                    modifyPhonePermission = True
                End If

                If permission = "Eliminar Telefono" Then
                    deletePhonePermission = True
                    btnEliminarTelefono.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Persona', 'OK')")

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


    Private Sub AgregarPersona_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM people " & _
        "WHERE ipeopleid = " & ipeopleid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp WHERE people.ipeopleid = tp.ipeopleid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM peoplephonenumbers " & _
        "WHERE ipeopleid = " & ipeopleid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn WHERE peoplephonenumbers.ipeopleid = tppn.ipeopleid AND peoplephonenumbers.ipeoplephonenumberid = tppn.ipeoplephonenumberid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp JOIN people p ON tp.ipeopleid = p.ipeopleid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tppn.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn JOIN peoplephonenumbers ppn ON tppn.ipeopleid = ppn.ipeopleid AND tppn.ipeoplephonenumberid = ppn.ipeoplephonenumberid WHERE STR_TO_DATE(CONCAT(tppn.iupdatedate, ' ', tppn.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ppn.iupdatedate, ' ', ppn.supdatetime), '%Y%c%d %T') ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM people p WHERE p.ipeopleid = tp.ipeopleid AND p.ipeopleid = " & ipeopleid & ") ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn " & _
        "WHERE NOT EXISTS (SELECT * FROM peoplephonenumbers ppn WHERE ppn.ipeopleid = tppn.ipeopleid AND ppn.ipeoplephonenumberid = tppn.ipeoplephonenumberid AND ppn.ipeopleid = " & ipeopleid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0


        If validaContactoCompleto(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Los datos de esta Persona están incompletos. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesPersonIsOpen As Integer = 1

            timesPersonIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%People" & ipeopleid & "'")

            If timesPersonIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto a la misma Persona. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando a esta Persona?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesPersonIsOpen > 1 And isEdit = False Then

                'Change this Person id for an unused one
                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%People" & ipeopleid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (peopleid + newIdAddition)

                Dim queriesNewId(4) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid + newIdAddition & "PhoneNumbers"
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid + newIdAddition & " SET ipeopleid = " & ipeopleid + newIdAddition & " WHERE ipeopleid = " & ipeopleid
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid + newIdAddition & "PhoneNumbers SET ipeopleid = " & ipeopleid + newIdAddition & " WHERE ipeopleid = " & ipeopleid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    ipeopleid = ipeopleid + newIdAddition
                End If

            End If

            Dim queriesSave(7) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM people " & _
            "WHERE ipeopleid = " & ipeopleid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp WHERE people.ipeopleid = tp.ipeopleid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM peoplephonenumbers " & _
            "WHERE ipeopleid = " & ipeopleid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn WHERE peoplephonenumbers.ipeopleid = tppn.ipeopleid AND peoplephonenumbers.ipeoplephonenumberid = tppn.ipeoplephonenumberid) "

            queriesSave(2) = "" & _
            "UPDATE people p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp ON tp.ipeopleid = p.ipeopleid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.speoplefullname = tp.speoplefullname, p.speoplegender = tp.speoplegender, p.speopleaddress = tp.speopleaddress, p.speoplemail = tp.speoplemail, p.speopleobservations = tp.speopleobservations WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(3) = "" & _
            "UPDATE peoplephonenumbers ppn JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn ON tppn.ipeopleid = ppn.ipeopleid AND tppn.ipeoplephonenumberid = ppn.ipeoplephonenumberid SET ppn.iupdatedate = tppn.iupdatedate, ppn.supdatetime = tppn.supdatetime, ppn.supdateusername = tppn.supdateusername, ppn.speoplephonenumber = tppn.speoplephonenumber WHERE STR_TO_DATE(CONCAT(tppn.iupdatedate, ' ', tppn.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ppn.iupdatedate, ' ', ppn.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "INSERT INTO people " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp " & _
            "WHERE NOT EXISTS (SELECT * FROM people p WHERE p.ipeopleid = tp.ipeopleid AND p.ipeopleid = " & ipeopleid & ") "

            queriesSave(5) = "" & _
            "INSERT INTO peoplephonenumbers " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn " & _
            "WHERE NOT EXISTS (SELECT * FROM peoplephonenumbers ppn WHERE ppn.ipeopleid = tppn.ipeopleid AND ppn.ipeoplephonenumberid = tppn.ipeoplephonenumberid AND ppn.ipeopleid = " & ipeopleid & ") "

            queriesSave(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Persona " & ipeopleid & ": " & txtNombreCompleto.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queriesSave) = True Then
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

        Dim queriesDelete(4) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers"
        queriesDelete(2) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Persona " & ipeopleid & ": " & txtNombreCompleto.Text.Replace("--", "").Replace("'", "") & "', 'OK')"
        queriesDelete(3) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'People', 'Persona', '" & ipeopleid & "', '" & txtNombreCompleto.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarPersona_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarPersona_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesPersonIsOpen As Integer = 0

        timesPersonIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%People" & ipeopleid & "'")

        If timesPersonIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto a la misma Persona. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo a esta Persona?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If


        If isRecover = False Then

            Dim queriesCreation(4) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " ( `ipeopleid` int(11) NOT NULL AUTO_INCREMENT, `speoplefullname` varchar(500) CHARACTER SET latin1 NOT NULL, `speoplegender` varchar(1) CHARACTER SET latin1 NOT NULL, `speopleaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `speoplemail` varchar(500) CHARACTER SET latin1 NOT NULL, `speopleobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers ( `ipeopleid` int(11) NOT NULL, `ipeoplephonenumberid` int(11) NOT NULL, `speoplephonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `speoplephonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) COLLATE latin1_spanish_ci NOT NULL, `supdateusername` varchar(100) COLLATE latin1_spanish_ci NOT NULL, PRIMARY KEY (`ipeopleid`,`ipeoplephonenumberid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        If isEdit = False Then

            Me.Text = "Nueva Persona"

            txtNombreCompleto.Text = ""
            txtDireccion.Text = ""
            txtEmail.Text = ""
            txtObservaciones.Text = ""
            chkCliente.Checked = False

        Else

            If isRecover = False Then

                Dim queriesInsert(6) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SELECT * FROM people WHERE ipeopleid = " & ipeopleid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers SELECT * FROM peoplephonenumbers WHERE ipeopleid = " & ipeopleid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsPeople As DataSet
            dsPeople = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " WHERE ipeopleid = " & ipeopleid)

            If getSQLQueryAsBoolean(0, "SELECT * FROM projects WHERE ipeopleid = " & ipeopleid) = True Then
                chkCliente.Checked = True
                Me.Text = "Editar Datos de Cliente " & dsPeople.Tables(0).Rows(0).Item("speoplefullname")
            Else
                chkCliente.Checked = False
                Me.Text = "Editar Datos de Persona " & dsPeople.Tables(0).Rows(0).Item("speoplefullname")
            End If

            txtNombreCompleto.Text = dsPeople.Tables(0).Rows(0).Item("speoplefullname")

            If dsPeople.Tables(0).Rows(0).Item("speoplegender") = "F" Then
                cmbSexo.SelectedIndex = 0
            Else
                cmbSexo.SelectedIndex = 1
            End If

            txtDireccion.Text = dsPeople.Tables(0).Rows(0).Item("speopleaddress")
            txtEmail.Text = dsPeople.Tables(0).Rows(0).Item("speoplemail")
            txtObservaciones.Text = dsPeople.Tables(0).Rows(0).Item("speopleobservations")

        End If

        'txtNumeroDgvTelefonos.MaxLength = 100
        'txtTipoTelefonoDgvTelefonos.MaxLength = 200

        setDataGridView(dgvTelefonos, "SELECT ipeopleid, ipeoplephonenumberid, speoplephonenumber AS 'Telefono', speoplephonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100

        If isEdit = False Then
            dgvTelefonos.Enabled = False
        Else
            dgvTelefonos.Enabled = True
            btnNuevoTelefono.Enabled = True
            btnEliminarTelefono.Enabled = True
        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Persona " & ipeopleid & ": " & txtNombreCompleto.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'People', 'Persona', '" & ipeopleid & "', '" & txtNombreCompleto.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        txtNombreCompleto.Select()
        txtNombreCompleto.Focus()
        txtNombreCompleto.SelectionStart() = txtNombreCompleto.Text.Length

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


    Private Sub txtNombreCompleto_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreCompleto.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNombreCompleto.Text.Contains(arrayCaractProhib(carp)) Then
                txtNombreCompleto.Text = txtNombreCompleto.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtNombreCompleto.Select(txtNombreCompleto.Text.Length, 0)
        End If

    End Sub


    Private Sub txtDireccion_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDireccion.KeyUp

        Dim strcaracteresprohibidos As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtDireccion.Text.Contains(arrayCaractProhib(carp)) Then
                txtDireccion.Text = txtDireccion.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtDireccion.Select(txtDireccion.Text.Length, 0)
        End If

    End Sub


    Private Sub btnDireccion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDireccion.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bd As New BuscaDirecciones
        bd.susername = susername
        bd.bactive = bactive
        bd.bonline = bonline
        bd.suserfullname = suserfullname

        bd.suseremail = suseremail
        bd.susersession = susersession
        bd.susermachinename = susermachinename
        bd.suserip = suserip

        bd.querystring = txtDireccion.Text

        If Me.WindowState = FormWindowState.Maximized Then
            bd.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bd.ShowDialog(Me)
        Me.Visible = True

        If bd.DialogResult = Windows.Forms.DialogResult.OK Then

            txtDireccion.Text = bd.sdireccion

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtEmail_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmail.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]:;,{}+´¿'¬^`~\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtEmail.Text.Contains(arrayCaractProhib(carp)) Then
                txtEmail.Text = txtEmail.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtEmail.Select(txtEmail.Text.Length, 0)
        End If

    End Sub


    Private Sub txtObservaciones_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtObservaciones.KeyUp

        Dim strcaracteresprohibidos As String = "|°#$%&/()=¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtObservaciones.Text.Contains(arrayCaractProhib(carp)) Then
                txtObservaciones.Text = txtObservaciones.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtObservaciones.Select(txtObservaciones.Text.Length, 0)
        End If

    End Sub


    Private Sub cmbSexo_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSexo.SelectionChangeCommitted

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim sexo As String = "M"

        If validarPersona(True, False) = True Then
            dgvTelefonos.Enabled = True
            btnNuevoTelefono.Enabled = True
            btnEliminarTelefono.Enabled = True
        End If

        speoplefullname = txtNombreCompleto.Text
        sexo = cmbSexo.Text

        If sexo = "" Then
            sexo = "M"
        End If

        sexo = sexo.Substring(0, 1)

        If isEdit = True Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SET speoplefullname = '" & txtNombreCompleto.Text & "', speoplegender = '" & sexo & "', speopleaddress = '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', speoplemail = '" & txtEmail.Text & "', speopleobservations = '" & txtObservaciones.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid)

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " WHERE ipeopleid = " & ipeopleid)

            If checkIfItsOnlyTextUpdate = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SET speoplefullname = '" & speoplefullname & "', speoplegender = '" & sexo & "', speopleaddress = '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', speoplemail = '" & txtEmail.Text & "', speopleobservations = '" & txtObservaciones.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid)

            Else

                ipeopleid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipeopleid) + 1 IS NULL, 1, MAX(ipeopleid) + 1) AS ipeopleid FROM people")

                Dim queriesCreation(7) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid
                queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " ( `ipeopleid` int(11) NOT NULL AUTO_INCREMENT, `speoplefullname` varchar(500) CHARACTER SET latin1 NOT NULL, `speoplegender` varchar(1) CHARACTER SET latin1 NOT NULL, `speopleaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `speoplemail` varchar(500) CHARACTER SET latin1 NOT NULL, `speopleobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People0PhoneNumbers"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers ( `ipeopleid` int(11) NOT NULL, `ipeoplephonenumberid` int(11) NOT NULL, `speoplephonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `speoplephonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) COLLATE latin1_spanish_ci NOT NULL, `supdateusername` varchar(100) COLLATE latin1_spanish_ci NOT NULL, PRIMARY KEY (`ipeopleid`,`ipeoplephonenumberid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " VALUES (" & ipeopleid & ", '" & speoplefullname & "', '" & sexo & "', '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                executeTransactedSQLCommand(0, queriesCreation)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Function validarPersona(ByVal silent As Boolean, ByVal save As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos2 As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos3 As String = "|°#$%&/()=¡*¨[]_:;,-{}+´¿'¬^`~@\<>"

        txtNombreCompleto.Text = txtNombreCompleto.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray).Replace("--", "").Replace("'", "")
        txtDireccion.Text = txtDireccion.Text.Trim.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "").Replace("'", "")
        txtObservaciones.Text = txtObservaciones.Text.Trim.Trim(strcaracteresprohibidos3.ToCharArray).Replace("--", "").Replace("'", "")

        If txtNombreCompleto.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner el Nombre de la Persona?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If save = True Then

            If txtDireccion.Text = "" Then
                If silent = False Then
                    MsgBox("¿Podrías poner la Dirección de la Persona?", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                Return False
            End If

        End If

        Return True

    End Function


    Private Sub dgvTelefonos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try

            If dgvTelefonos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedphoneid = CInt(dgvTelefonos.Rows(e.RowIndex).Cells(1).Value())
            sselectedphone = dgvTelefonos.Rows(e.RowIndex).Cells(2).Value()
            sselectedphonetype = dgvTelefonos.Rows(e.RowIndex).Cells(3).Value()

        Catch ex As Exception

            iselectedphoneid = 0
            sselectedphone = ""
            sselectedphonetype = ""

        End Try

    End Sub


    Private Sub dgvTelefonos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try

            If dgvTelefonos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedphoneid = CInt(dgvTelefonos.Rows(e.RowIndex).Cells(1).Value())
            sselectedphone = dgvTelefonos.Rows(e.RowIndex).Cells(2).Value()
            sselectedphonetype = dgvTelefonos.Rows(e.RowIndex).Cells(3).Value()

        Catch ex As Exception

            iselectedphoneid = 0
            sselectedphone = ""
            sselectedphonetype = ""

        End Try

    End Sub


    Private Sub dgvTelefonos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTelefonos.SelectionChanged

        Try

            If dgvTelefonos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedphoneid = CInt(dgvTelefonos.CurrentRow.Cells(1).Value())
            sselectedphone = dgvTelefonos.CurrentRow.Cells(2).Value()
            sselectedphonetype = dgvTelefonos.CurrentRow.Cells(3).Value()

        Catch ex As Exception

            iselectedphoneid = 0
            sselectedphone = ""
            sselectedphonetype = ""

        End Try

    End Sub


    Private Sub dgvTelefonos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTelefonos.CellEndEdit

        setDataGridView(dgvTelefonos, "SELECT ipeopleid, ipeoplephonenumberid, speoplephonenumber AS 'Telefono', speoplephonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100

    End Sub


    Private Sub dgvTelefonos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTelefonos.CellValueChanged

        If modifyPhonePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If e.ColumnIndex = 2 Then

            dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

            If dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Teléfono?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Teléfono") = MsgBoxResult.Yes Then

                    If txtEmail.Text = "" And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid) = 1 Then

                        MsgBox("No puedo quedarme sin formas de contactar al cliente...", MsgBoxStyle.OkOnly, "Dato Faltante")
                        Exit Sub

                    Else

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid & " AND ipeoplephonenumberid = " & iselectedphoneid)

                    End If



                Else
                    dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedphone
                End If

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers SET speoplephonenumber = '" & dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid & " AND ipeoplephonenumberid = " & iselectedphoneid)

            End If

        ElseIf e.ColumnIndex = 3 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers SET speoplephonetype = '" & dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid & " AND ipeoplephonenumberid = " & iselectedphoneid)

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvTelefonos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvTelefonos.EditingControlShowing

        If dgvTelefonos.CurrentCell.ColumnIndex = 2 Then

            txtNumeroDgvTelefonos = CType(e.Control, TextBox)
            txtNumeroDgvTelefonos_OldText = txtNumeroDgvTelefonos.Text

        ElseIf dgvTelefonos.CurrentCell.ColumnIndex = 3 Then

            txtTipoTelefonoDgvTelefonos = CType(e.Control, TextBox)
            txtTipoTelefonoDgvTelefonos_OldText = txtTipoTelefonoDgvTelefonos.Text

        Else

            txtNumeroDgvTelefonos = Nothing
            txtNumeroDgvTelefonos_OldText = Nothing
            txtTipoTelefonoDgvTelefonos = Nothing
            txtTipoTelefonoDgvTelefonos_OldText = Nothing

        End If

    End Sub


    Private Sub txtNumeroDgvTelefonos_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumeroDgvTelefonos.KeyUp

        txtNumeroDgvTelefonos.Text = txtNumeroDgvTelefonos.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!$%&/()=?¡¨[]_:;,{}´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtNumeroDgvTelefonos.Text.Contains(arrayForbidden1(fc)) Then
                txtNumeroDgvTelefonos.Text = txtNumeroDgvTelefonos.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

        txtNumeroDgvTelefonos.Text = txtNumeroDgvTelefonos.Text.Replace("--", "").Replace("@", "").Replace("'", "")

    End Sub


    Private Sub txtTipoTelefonoDgvTelefonos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTipoTelefonoDgvTelefonos.KeyUp

        Dim strcaracteresprohibidos As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtTipoTelefonoDgvTelefonos.Text.Contains(arrayCaractProhib(carp)) Then
                txtTipoTelefonoDgvTelefonos.Text = txtTipoTelefonoDgvTelefonos.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtTipoTelefonoDgvTelefonos.Select(txtTipoTelefonoDgvTelefonos.Text.Length, 0)
        End If

    End Sub


    Private Sub dgvTelefonos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvTelefonos.UserAddedRow

        If addPhonePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        iselectedphoneid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipeoplephonenumberid) + 1 IS NULL, 1, MAX(ipeoplephonenumberid) + 1) AS ipeoplephonenumberid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid)

        dgvTelefonos.CurrentRow.Cells(0).Value = iselectedphoneid
        dgvTelefonos.CurrentRow.Cells(1).Value = ipeopleid
        dgvTelefonos.CurrentRow.Cells(2).Value = "1"
        dgvTelefonos.CurrentRow.Cells(3).Value = "General"

        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers VALUES (" & ipeopleid & ", " & iselectedphoneid & ", '1', 'General', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvTelefonos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvTelefonos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deletePhonePermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas eliminar este Teléfono?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Teléfono") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedphoneid As Integer = 0
                Try
                    tmpselectedphoneid = CInt(dgvTelefonos.CurrentRow.Cells(1).Value)
                Catch ex As Exception

                End Try

                If txtEmail.Text = "" And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid) = 1 Then

                    MsgBox("No puedo quedarme sin formas de contactar al cliente...", MsgBoxStyle.OkOnly, "Dato Faltante")
                    Exit Sub

                Else

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid & " AND ipeoplephonenumberid = " & tmpselectedphoneid)

                End If

                setDataGridView(dgvTelefonos, "SELECT ipeopleid, ipeoplephonenumberid, speoplephonenumber AS 'Telefono', speoplephonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid, False)

                dgvTelefonos.Columns(0).Visible = False
                dgvTelefonos.Columns(1).Visible = False

                dgvTelefonos.Columns(0).ReadOnly = True
                dgvTelefonos.Columns(1).ReadOnly = True

                dgvTelefonos.Columns(0).Width = 30
                dgvTelefonos.Columns(1).Width = 30
                dgvTelefonos.Columns(2).Width = 100

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvTelefonos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTelefonos.Click

        If validarPersona(True, False) = True And ipeopleid <> 0 Then
            dgvTelefonos.Enabled = True
            btnNuevoTelefono.Enabled = True
            btnEliminarTelefono.Enabled = True
        End If

    End Sub


    Private Sub btnNuevoTelefono_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoTelefono.Click


        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim sexo As String = "M"

        If validarPersona(True, False) = True Then
            dgvTelefonos.Enabled = True
            btnNuevoTelefono.Enabled = True
            btnEliminarTelefono.Enabled = True
        End If

        speoplefullname = txtNombreCompleto.Text
        sexo = cmbSexo.Text

        If sexo = "" Then
            sexo = "M"
        End If

        sexo = sexo.Substring(0, 1)

        If isEdit = True Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SET speoplefullname = '" & txtNombreCompleto.Text & "', speoplegender = '" & sexo & "', speopleaddress = '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', speoplemail = '" & txtEmail.Text & "', speopleobservations = '" & txtObservaciones.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid)

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " WHERE ipeopleid = " & ipeopleid)

            If checkIfItsOnlyTextUpdate = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SET speoplefullname = '" & speoplefullname & "', speoplegender = '" & sexo & "', speopleaddress = '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', speoplemail = '" & txtEmail.Text & "', speopleobservations = '" & txtObservaciones.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid)

            Else

                ipeopleid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipeopleid) + 1 IS NULL, 1, MAX(ipeopleid) + 1) AS ipeopleid FROM people")

                Dim queriesCreation(7) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid
                queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " ( `ipeopleid` int(11) NOT NULL AUTO_INCREMENT, `speoplefullname` varchar(500) CHARACTER SET latin1 NOT NULL, `speoplegender` varchar(1) CHARACTER SET latin1 NOT NULL, `speopleaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `speoplemail` varchar(500) CHARACTER SET latin1 NOT NULL, `speopleobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People0PhoneNumbers"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers ( `ipeopleid` int(11) NOT NULL, `ipeoplephonenumberid` int(11) NOT NULL, `speoplephonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `speoplephonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) COLLATE latin1_spanish_ci NOT NULL, `supdateusername` varchar(100) COLLATE latin1_spanish_ci NOT NULL, PRIMARY KEY (`ipeopleid`,`ipeoplephonenumberid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " VALUES (" & ipeopleid & ", '" & speoplefullname & "', '" & sexo & "', '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                executeTransactedSQLCommand(0, queriesCreation)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default


        'Empieza código de Nuevo Telefono

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        iselectedphoneid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipeoplephonenumberid) + 1 IS NULL, 1, MAX(ipeoplephonenumberid) + 1) AS ipeoplephonenumberid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid)

        'dgvTelefonos.CurrentRow.Cells(0).Value = iselectedphoneid
        'dgvTelefonos.CurrentRow.Cells(1).Value = ipeopleid
        'dgvTelefonos.CurrentRow.Cells(2).Value = "1"

        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers VALUES (" & ipeopleid & ", " & iselectedphoneid & ", '1', 'General', " & fecha & ", '" & hora & "', '" & susername & "')")

        setDataGridView(dgvTelefonos, "SELECT ipeopleid, ipeoplephonenumberid, speoplephonenumber AS 'Telefono', speoplephonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarTelefono_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarTelefono.Click


        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim sexo As String = "M"

        If validarPersona(True, False) = True Then
            dgvTelefonos.Enabled = True
            btnNuevoTelefono.Enabled = True
            btnEliminarTelefono.Enabled = True
        End If

        speoplefullname = txtNombreCompleto.Text
        sexo = cmbSexo.Text

        If sexo = "" Then
            sexo = "M"
        End If

        sexo = sexo.Substring(0, 1)

        If isEdit = True Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SET speoplefullname = '" & txtNombreCompleto.Text & "', speoplegender = '" & sexo & "', speopleaddress = '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', speoplemail = '" & txtEmail.Text & "', speopleobservations = '" & txtObservaciones.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid)

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " WHERE ipeopleid = " & ipeopleid)

            If checkIfItsOnlyTextUpdate = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SET speoplefullname = '" & speoplefullname & "', speoplegender = '" & sexo & "', speopleaddress = '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', speoplemail = '" & txtEmail.Text & "', speopleobservations = '" & txtObservaciones.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid)

            Else

                ipeopleid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipeopleid) + 1 IS NULL, 1, MAX(ipeopleid) + 1) AS ipeopleid FROM people")

                Dim queriesCreation(7) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid
                queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " ( `ipeopleid` int(11) NOT NULL AUTO_INCREMENT, `speoplefullname` varchar(500) CHARACTER SET latin1 NOT NULL, `speoplegender` varchar(1) CHARACTER SET latin1 NOT NULL, `speopleaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `speoplemail` varchar(500) CHARACTER SET latin1 NOT NULL, `speopleobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People0PhoneNumbers"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers ( `ipeopleid` int(11) NOT NULL, `ipeoplephonenumberid` int(11) NOT NULL, `speoplephonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `speoplephonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) COLLATE latin1_spanish_ci NOT NULL, `supdateusername` varchar(100) COLLATE latin1_spanish_ci NOT NULL, PRIMARY KEY (`ipeopleid`,`ipeoplephonenumberid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " VALUES (" & ipeopleid & ", '" & speoplefullname & "', '" & sexo & "', '" & txtDireccion.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                executeTransactedSQLCommand(0, queriesCreation)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default



        'Empieza código de Eliminar Telefono

        If MsgBox("¿Está seguro que deseas eliminar este Teléfono?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Teléfono") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedphoneid As Integer = 0
            Try
                tmpselectedphoneid = CInt(dgvTelefonos.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try

            If txtEmail.Text = "" And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid) = 1 Then

                MsgBox("No puedo quedarme sin formas de contactar al cliente...", MsgBoxStyle.OkOnly, "Dato Faltante")
                Exit Sub

            Else

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid & " AND ipeoplephonenumberid = " & tmpselectedphoneid)

            End If

            setDataGridView(dgvTelefonos, "SELECT ipeopleid, ipeoplephonenumberid, speoplephonenumber AS 'Telefono', speoplephonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid, False)

            dgvTelefonos.Columns(0).Visible = False
            dgvTelefonos.Columns(1).Visible = False

            dgvTelefonos.Columns(0).ReadOnly = True
            dgvTelefonos.Columns(1).ReadOnly = True

            dgvTelefonos.Columns(0).Width = 30
            dgvTelefonos.Columns(1).Width = 30
            dgvTelefonos.Columns(2).Width = 100

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Function validaFormasDeContacto(ByVal silent As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]:;,{}+´¿'¬^`~\<>"
        txtEmail.Text = txtEmail.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray).Replace("--", "").Replace("'", "")

        If txtEmail.Text = "" And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers WHERE ipeopleid = " & ipeopleid) < 1 Then
            If silent = False Then
                MsgBox("¿Podrías poner alguna forma de contactar a la persona? Un email o un teléfono...", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        Return True

    End Function


    Private Function validaContactoCompleto(ByVal silent As Boolean) As Boolean

        If validarPersona(silent, True) = False Or validaFormasDeContacto(silent) = False Then
            Return False
        Else
            Return True
        End If

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'ipeopleid = 0
        'speoplefullname = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesPersonIsOpen As Integer = 1

        timesPersonIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%People" & ipeopleid & "'")

        If timesPersonIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto la misma Persona. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando a esta Persona?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesPersonIsOpen > 1 And isEdit = False Then

            'Change this Person id for an unused one
            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%People" & ipeopleid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (peopleid + newIdAddition)

            Dim queriesNewId(4) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid + newIdAddition & "PhoneNumbers"
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid + newIdAddition & " SET ipeopleid = " & ipeopleid + newIdAddition & " WHERE ipeopleid = " & ipeopleid
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid + newIdAddition & "PhoneNumbers SET ipeopleid = " & ipeopleid + newIdAddition & " WHERE ipeopleid = " & ipeopleid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                ipeopleid = ipeopleid + newIdAddition
            End If

        End If

        If validaContactoCompleto(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim sexo As String = "M"

            speoplefullname = txtNombreCompleto.Text
            sexo = cmbSexo.Text

            If sexo = "" Then
                sexo = "M"
            End If

            sexo = sexo.Substring(0, 1)

            If isEdit = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SET speoplefullname = '" & speoplefullname & "', speoplegender = '" & sexo & "', speopleaddress = '" & txtDireccion.Text & "', speoplemail = '" & txtEmail.Text & "', speopleobservations = '" & txtObservaciones.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid)

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " WHERE ipeopleid = " & ipeopleid)

                If checkIfItsOnlyTextUpdate = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " SET speoplefullname = '" & speoplefullname & "', speoplegender = '" & sexo & "', speopleaddress = '" & txtDireccion.Text & "', speoplemail = '" & txtEmail.Text & "', speopleobservations = '" & txtObservaciones.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipeopleid = " & ipeopleid)

                Else

                    ipeopleid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipeopleid) + 1 IS NULL, 1, MAX(ipeopleid) + 1) AS ipeopleid FROM people")

                    Dim queriesCreation(7) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People0"
                    queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid
                    queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " ( `ipeopleid` int(11) NOT NULL AUTO_INCREMENT, `speoplefullname` varchar(500) CHARACTER SET latin1 NOT NULL, `speoplegender` varchar(1) CHARACTER SET latin1 NOT NULL, `speopleaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `speoplemail` varchar(500) CHARACTER SET latin1 NOT NULL, `speopleobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People0PhoneNumbers"
                    queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers"
                    queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers ( `ipeopleid` int(11) NOT NULL, `ipeoplephonenumberid` int(11) NOT NULL, `speoplephonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `speoplephonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) COLLATE latin1_spanish_ci NOT NULL, `supdateusername` varchar(100) COLLATE latin1_spanish_ci NOT NULL, PRIMARY KEY (`ipeopleid`,`ipeoplephonenumberid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " VALUES (" & ipeopleid & ", '" & speoplefullname & "', '" & sexo & "', '" & txtDireccion.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    executeTransactedSQLCommand(0, queriesCreation)

                End If

            End If

        End If

        Dim queriesSave(7) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM people " & _
        "WHERE ipeopleid = " & ipeopleid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp WHERE people.ipeopleid = tp.ipeopleid) "

        queriesSave(1) = "" & _
        "DELETE " & _
        "FROM peoplephonenumbers " & _
        "WHERE ipeopleid = " & ipeopleid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn WHERE peoplephonenumbers.ipeopleid = tppn.ipeopleid AND peoplephonenumbers.ipeoplephonenumberid = tppn.ipeoplephonenumberid) "

        queriesSave(2) = "" & _
        "UPDATE people p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp ON tp.ipeopleid = p.ipeopleid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.speoplefullname = tp.speoplefullname, p.speoplegender = tp.speoplegender, p.speopleaddress = tp.speopleaddress, p.speoplemail = tp.speoplemail, p.speopleobservations = tp.speopleobservations WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(3) = "" & _
        "UPDATE peoplephonenumbers ppn JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn ON tppn.ipeopleid = ppn.ipeopleid AND tppn.ipeoplephonenumberid = ppn.ipeoplephonenumberid SET ppn.iupdatedate = tppn.iupdatedate, ppn.supdatetime = tppn.supdatetime, ppn.supdateusername = tppn.supdateusername, ppn.speoplephonenumber = tppn.speoplephonenumber WHERE STR_TO_DATE(CONCAT(tppn.iupdatedate, ' ', tppn.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ppn.iupdatedate, ' ', ppn.supdatetime), '%Y%c%d %T') "

        queriesSave(4) = "" & _
        "INSERT INTO people " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM people p WHERE p.ipeopleid = tp.ipeopleid AND p.ipeopleid = " & ipeopleid & ") "

        queriesSave(5) = "" & _
        "INSERT INTO peoplephonenumbers " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "People" & ipeopleid & "PhoneNumbers tppn " & _
        "WHERE NOT EXISTS (SELECT * FROM peoplephonenumbers ppn WHERE ppn.ipeopleid = tppn.ipeopleid AND ppn.ipeoplephonenumberid = tppn.ipeoplephonenumberid AND ppn.ipeopleid = " & ipeopleid & ") "

        queriesSave(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Persona " & ipeopleid & ": " & txtNombreCompleto.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queriesSave) = True Then
            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        wasCreated = True

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class