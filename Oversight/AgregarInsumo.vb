Public Class AgregarInsumo

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public iprojectid As Integer = 0
    Public icardid As Integer = 0
    Public iinputid As Integer = 0

    Public IsEdit As Boolean = False
    Public IsRecover As Boolean = False

    Public IsBase As Boolean = False
    Public IsModel As Boolean = False
    Public IsHistoric As Boolean = False

    Public sinputdescription As String = ""
    Public sinputunit As String = ""

    Public ihistoricprojectid As Integer = 0
    Public ihistoriccardid As Integer = 0

    Public wasCreated As Boolean = False

    Private openPermission As Boolean = False
    Private openHistoricPermission As Boolean = False

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

                If permission = "Abrir Historicos" Then
                    openHistoricPermission = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Insumo', 'OK')")

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


    Private Sub AgregarInsumo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0
        Dim conteo7 As Integer = 0
        Dim conteo8 As Integer = 0
        Dim conteo9 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM inputs " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti WHERE inputs.iinputid = ti.iinputid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM inputtypes " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit WHERE inputtypes.iinputid = tit.iinputid) ")

        conteo7 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM inputcategories " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic WHERE inputcategories.iinputid = tic.iinputid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(ti.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti JOIN inputs i ON ti.iinputid = i.iinputid WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tit.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit JOIN inputtypes it ON tit.iinputid = it.iinputid WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tic.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic JOIN inputcategories ic ON tic.iinputid = ic.iinputid WHERE STR_TO_DATE(CONCAT(tic.iupdatedate, ' ', tic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ic.iupdatedate, ' ', ic.supdatetime), '%Y%c%d %T') ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
        "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
        "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") ")

        conteo9 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic " & _
        "WHERE NOT EXISTS (SELECT * FROM inputcategories ic WHERE ic.iinputid = tic.iinputid AND ic.iinputid = " & iinputid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 + conteo7 + conteo8 + conteo9 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaInsumo(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK And IsHistoric = False Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Insumo no está completo. Si sales ahora, se perderán los cambios que hayas hecho a este Insumo y por consiguiente, a las Tarjetas que lo contengan" & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then

            Dim timesInputIsOpen As Integer = 1

            timesInputIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Input" & iinputid & "'")

            If timesInputIsOpen > 1 And IsEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Insumo. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInputIsOpen > 1 And IsEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Input" & iinputid + newIdAddition & "'") > 1 And IsEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(6) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Types"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Categories"
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & " SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Types SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Categories SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    iinputid = iinputid + newIdAddition
                End If

            End If

            Dim queriesSave(10) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM inputs " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti WHERE inputs.iinputid = ti.iinputid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM inputtypes " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit WHERE inputtypes.iinputid = tit.iinputid) "

            queriesSave(2) = "" & _
            "DELETE " & _
            "FROM inputcategories " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic WHERE inputcategories.iinputid = tic.iinputid) "

            queriesSave(3) = "" & _
            "UPDATE inputs i JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti ON ti.iinputid = i.iinputid SET i.iupdatedate = ti.iupdatedate, i.supdatetime = ti.supdatetime, i.supdateusername = ti.supdateusername, i.sinputdescription = ti.sinputdescription, i.sinputunit = ti.sinputunit WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE inputtypes it JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit ON tit.iinputid = it.iinputid SET it.iupdatedate = tit.iupdatedate, it.supdatetime = tit.supdatetime, it.supdateusername = tit.supdateusername, it.sinputtypedescription = tit.sinputtypedescription WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') "

            queriesSave(5) = "" & _
            "UPDATE inputcategories ic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic ON tic.iinputid = ic.iinputid SET ic.iupdatedate = tic.iupdatedate, ic.supdatetime = tic.supdatetime, ic.supdateusername = tic.supdateusername, ic.sinputcategory = tic.sinputcategory WHERE STR_TO_DATE(CONCAT(tic.iupdatedate, ' ', tic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ic.iupdatedate, ' ', ic.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO inputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
            "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO inputtypes " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
            "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") "

            queriesSave(8) = "" & _
            "INSERT INTO inputcategories " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic " & _
            "WHERE NOT EXISTS (SELECT * FROM inputcategories ic WHERE ic.iinputid = tic.iinputid AND ic.iinputid = " & iinputid & ") "

            queriesSave(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Insumo " & iinputid & ": " & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queriesSave) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

        ElseIf result = MsgBoxResult.Cancel Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim queriesDelete(5) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories"
        queriesDelete(3) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Insumo " & iinputid & ": " & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"
        'queriesDelete(4) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Input', 'Insumo', '" & iinputid & "', '" & txtNombreDelInsumo.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarInsumo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarInsumo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesInputIsOpen As Integer = 0

        timesInputIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Input" & iinputid & "'")

        If timesInputIsOpen > 0 And IsEdit = True Then

            If MsgBox("Otro usuario tiene abierto el mismo Insumo. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

        End If

        If IsRecover = False Then

            Dim queriesCreation(6) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories ( `iinputid` int(11) NOT NULL, `sinputcategory` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        If IsEdit = True Then

            Dim queryInsumo As String = ""
            Dim dsInsumo As DataSet


            If IsRecover = False Then

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SELECT * FROM inputs WHERE iinputid = " & iinputid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SELECT * FROM inputtypes WHERE iinputid = " & iinputid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories SELECT * FROM inputcategories WHERE iinputid = " & iinputid

                executeTransactedSQLCommand(0, queriesInsert)

            End If


            If IsBase = True Then

                queryInsumo = "SELECT i.iinputid, i.sinputdescription, i.sinputunit, it.sinputtypedescription, bp.dinputpricewithoutIVA, " & _
                "bp.dinputprotectionpercentage, bp.dinputfinalprice, IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS sinputcategory, " & _
                "IF(iti.dinputtimberespesor IS NULL, 0.0, iti.dinputtimberespesor) AS dinputtimberespesor, " & _
                "IF(iti.dinputtimberancho IS NULL, 0.0, iti.dinputtimberancho) AS dinputtimberancho, " & _
                "IF(iti.dinputtimberlargo IS NULL, 0.0, iti.dinputtimberlargo) AS dinputtimberlargo, " & _
                "IF(iti.dinputtimberpreciopiecubico IS NULL, 0.0, iti.dinputtimberpreciopiecubico) AS dinputtimberpreciopiecubico " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types it ON it.iinputid = i.iinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories ic ON ic.iinputid = i.iinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber iti ON iti.iinputid = i.iinputid AND iti.ibaseid = " & iprojectid & " " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                "WHERE bp.ibaseid = " & iprojectid & " AND i.iinputid = " & iinputid

            Else

                If IsModel = True Then

                    queryInsumo = "SELECT i.iinputid, i.sinputdescription, i.sinputunit, it.sinputtypedescription, mp.dinputpricewithoutIVA, " & _
                    "mp.dinputprotectionpercentage, mp.dinputfinalprice, IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS sinputcategory, " & _
                    "IF(iti.dinputtimberespesor IS NULL, 0.0, iti.dinputtimberespesor) AS dinputtimberespesor, " & _
                    "IF(iti.dinputtimberancho IS NULL, 0.0, iti.dinputtimberancho) AS dinputtimberancho, " & _
                    "IF(iti.dinputtimberlargo IS NULL, 0.0, iti.dinputtimberlargo) AS dinputtimberlargo, " & _
                    "IF(iti.dinputtimberpreciopiecubico IS NULL, 0.0, iti.dinputtimberpreciopiecubico) AS dinputtimberpreciopiecubico " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types it ON it.iinputid = i.iinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories ic ON ic.iinputid = i.iinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber iti ON iti.iinputid = i.iinputid AND iti.imodelid = " & iprojectid & " " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON i.iinputid = mp.iinputid " & _
                    "WHERE mp.imodelid = " & iprojectid & " AND i.iinputid = " & iinputid

                Else

                    queryInsumo = "SELECT i.iinputid, i.sinputdescription, i.sinputunit, it.sinputtypedescription, pp.dinputpricewithoutIVA, " & _
                    "pp.dinputprotectionpercentage, pp.dinputfinalprice, IF(ic.sinputcategory IS NULL, '', ic.sinputcategory) AS sinputcategory, " & _
                    "IF(iti.dinputtimberespesor IS NULL, 0.0, iti.dinputtimberespesor) AS dinputtimberespesor, " & _
                    "IF(iti.dinputtimberancho IS NULL, 0.0, iti.dinputtimberancho) AS dinputtimberancho, " & _
                    "IF(iti.dinputtimberlargo IS NULL, 0.0, iti.dinputtimberlargo) AS dinputtimberlargo, " & _
                    "IF(iti.dinputtimberpreciopiecubico IS NULL, 0.0, iti.dinputtimberpreciopiecubico) AS dinputtimberpreciopiecubico " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types it ON it.iinputid = i.iinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories ic ON ic.iinputid = i.iinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber iti ON iti.iinputid = i.iinputid AND iti.iprojectid = " & iprojectid & " " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                    "WHERE pp.iprojectid = " & iprojectid & " AND i.iinputid = " & iinputid

                End If

            End If

            dsInsumo = getSQLQueryAsDataset(0, queryInsumo)

            txtNombreDelInsumo.Text = dsInsumo.Tables(0).Rows(0).Item("sinputdescription")
            cmbTipoDeInsumo.SelectedItem = dsInsumo.Tables(0).Rows(0).Item("sinputtypedescription")
            txtUnidadDeMedida.Text = dsInsumo.Tables(0).Rows(0).Item("sinputunit")
            txtCategoria.Text = dsInsumo.Tables(0).Rows(0).Item("sinputcategory")

            Dim costosiniva As Double = 0.0
            Dim porcentaje As Double = 0.0
            Dim precio As Double = 0.0

            Dim espesor As Double = 0.0
            Dim ancho As Double = 0.0
            Dim largo As Double = 0.0
            Dim preciopiecubico As Double = 0.0

            Try

                costosiniva = CDbl(dsInsumo.Tables(0).Rows(0).Item("dinputpricewithoutIVA"))
                porcentaje = CDbl(dsInsumo.Tables(0).Rows(0).Item("dinputprotectionpercentage"))
                precio = CDbl(dsInsumo.Tables(0).Rows(0).Item("dinputfinalprice"))

                espesor = CDbl(dsInsumo.Tables(0).Rows(0).Item("dinputtimberespesor"))
                ancho = CDbl(dsInsumo.Tables(0).Rows(0).Item("dinputtimberancho"))
                largo = CDbl(dsInsumo.Tables(0).Rows(0).Item("dinputtimberlargo"))
                preciopiecubico = CDbl(dsInsumo.Tables(0).Rows(0).Item("dinputtimberpreciopiecubico"))

                porcentaje = porcentaje * 100

            Catch ex As Exception

            End Try

            If espesor > 0 Or ancho > 0 Or largo > 0 Or preciopiecubico > 0 Then

                chkCubicar.Checked = True

                txtEspesor.Text = FormatCurrency(espesor, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtAncho.Text = FormatCurrency(ancho, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtLargo.Text = FormatCurrency(largo, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtPrecioPieCubico.Text = FormatCurrency(preciopiecubico, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            End If

            txtCostoSINIVA.Text = FormatCurrency(costosiniva, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtPorcentajeDeProteccion.Text = FormatCurrency(porcentaje, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtCostoParaTabulador.Text = FormatCurrency(precio, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            If IsHistoric = True Then

                txtNombreDelInsumo.Enabled = False
                cmbTipoDeInsumo.Enabled = False
                txtUnidadDeMedida.Enabled = False
                txtCostoSINIVA.Enabled = False
                txtPorcentajeDeProteccion.Enabled = False
                txtCostoParaTabulador.Enabled = False

                chkCubicar.Enabled = False

                txtEspesor.Enabled = False
                txtAncho.Enabled = False
                txtLargo.Enabled = False
                txtPrecioPieCubico.Enabled = False

            End If

        End If

        'Dim queryPreciosHistoricosInsumo As String

        'If IsBase = True Then

        '    queryPreciosHistoricosInsumo = "" & _
        '    "SELECT bp.ibaseid, STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
        '    "bp.dinputfinalprice AS 'Precio' " & _
        '    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i " & _
        '    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types it ON it.iinputid = i.iinputid " & _
        '    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices bp ON i.iinputid = bp.iinputid " & _
        '    "WHERE i.iinputid = " & iinputid & " " & _
        '    "ORDER BY 2 "

        'Else

        '    If IsModel = True Then

        '        queryPreciosHistoricosInsumo = "" & _
        '        "SELECT mp.imodelid, STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
        '        "mp.dinputfinalprice AS 'Precio' " & _
        '        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i " & _
        '        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types it ON it.iinputid = i.iinputid " & _
        '        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices mp ON i.iinputid = mp.iinputid " & _
        '        "WHERE i.iinputid = " & iinputid & " " & _
        '        "ORDER BY 2 "

        '    Else

        '        queryPreciosHistoricosInsumo = "" & _
        '        "SELECT pp.iprojectid, STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
        '        "pp.dinputfinalprice AS 'Precio' " & _
        '        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i " & _
        '        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types it ON it.iinputid = i.iinputid " & _
        '        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices pp ON i.iinputid = pp.iinputid " & _
        '        "WHERE i.iinputid = " & iinputid & " " & _
        '        "ORDER BY 2 "

        '    End If

        'End If



        'Dim dtPreciosHistoricosInsumo As DataTable
        'dtPreciosHistoricosInsumo = setDataGridView(dgvPreciosHistoricosInsumo, queryPreciosHistoricosInsumo, True)

        'dgvPreciosHistoricosInsumo.Columns(0).Visible = False

        'CODIGO PARA LA GRAFICA


        If IsHistoric = True Then

            btnGuardar.Visible = False

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Insumo " & iinputid & ": " & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
        'executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Input', 'Insumo', '" & iinputid & "', '" & txtNombreDelInsumo.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        txtNombreDelInsumo.Select()
        txtNombreDelInsumo.Focus()
        txtNombreDelInsumo.SelectionStart() = txtNombreDelInsumo.Text.Length

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


    Private Sub txtNombreDelInsumo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreDelInsumo.KeyUp

        Dim strcaracteresprohibidos As String = "|@'\"""
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNombreDelInsumo.Text.Contains(arrayCaractProhib(carp)) Then
                txtNombreDelInsumo.Text = txtNombreDelInsumo.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        txtNombreDelInsumo.Text = txtNombreDelInsumo.Text.Replace("--", "")

        If resultado = True Then
            txtNombreDelInsumo.Select(txtNombreDelInsumo.Text.Length, 0)
        End If

    End Sub


    Private Sub txtCategoria_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCategoria.KeyUp

        Dim strcaracteresprohibidos As String = "|@'\"""
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtCategoria.Text.Contains(arrayCaractProhib(carp)) Then
                txtCategoria.Text = txtCategoria.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        txtCategoria.Text = txtCategoria.Text.Replace("--", "")

        If resultado = True Then
            txtCategoria.Select(txtCategoria.Text.Length, 0)
        End If

    End Sub


    Private Sub txtUnidadDeMedida_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUnidadDeMedida.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtUnidadDeMedida.Text.Contains(arrayCaractProhib(carp)) Then
                txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtUnidadDeMedida.Select(txtUnidadDeMedida.Text.Length, 0)
        End If

    End Sub


    Private Sub txtUnidadDeMedida_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnidadDeMedida.LostFocus

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If IsHistoric = True Then
            Exit Sub
        End If

        Dim unitfound As Boolean = False
        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

        unitfound = getSQLQueryAsBoolean(0, "SELECT count(*) FROM transformationunits WHERE soriginunit = '" & txtUnidadDeMedida.Text.Replace("--", "") & "'")

        If unitfound = False Then

            MsgBox("¡No encontré esa unidad de Medida! ¿Podrías seleccionar una de la lista?", MsgBoxStyle.OkOnly, "Unidad de Medida No Encontrada")

            Dim bu As New BuscaUnidades
            bu.susername = susername
            bu.bactive = bactive
            bu.bonline = bonline
            bu.suserfullname = suserfullname

            bu.suseremail = suseremail
            bu.susersession = susersession
            bu.susermachinename = susermachinename
            bu.suserip = suserip

            bu.querystring = txtUnidadDeMedida.Text

            bu.isEdit = False

            If Me.WindowState = FormWindowState.Maximized Then
                bu.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            bu.ShowDialog(Me)
            Me.Visible = True

            If bu.DialogResult = Windows.Forms.DialogResult.OK Then

                txtUnidadDeMedida.Text = bu.sunit1
                unitfound = True

            Else

                txtUnidadDeMedida.Text = ""
                txtUnidadDeMedida.Focus()

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub chkCubicar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCubicar.CheckedChanged

        lblEspesor.Visible = chkCubicar.Checked
        txtEspesor.Visible = chkCubicar.Checked
        lblAncho.Visible = chkCubicar.Checked
        txtAncho.Visible = chkCubicar.Checked
        lblLargo.Visible = chkCubicar.Checked
        txtLargo.Visible = chkCubicar.Checked
        lblPrecioPieCubico.Visible = chkCubicar.Checked
        txtPrecioPieCubico.Visible = chkCubicar.Checked

        If chkCubicar.Checked = False Then
            txtCostoSINIVA.Enabled = True
        Else
            txtCostoSINIVA.Enabled = False
        End If

    End Sub


    Private Sub txtEspesor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEspesor.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtEspesor.Text.Contains(arrayCaractProhib(carp)) Then
                txtEspesor.Text = txtEspesor.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtEspesor.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtEspesor.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtEspesor.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtEspesor.Text.Length

                    If longitud > (lugar + 1) Then
                        txtEspesor.Text = txtEspesor.Text.Substring(0, lugar) & txtEspesor.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtEspesor.Text = txtEspesor.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtEspesor.Select(txtEspesor.Text.Length, 0)
        End If

    End Sub


    Private Sub txtAncho_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAncho.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtAncho.Text.Contains(arrayCaractProhib(carp)) Then
                txtAncho.Text = txtAncho.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtAncho.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtAncho.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtAncho.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtAncho.Text.Length

                    If longitud > (lugar + 1) Then
                        txtAncho.Text = txtAncho.Text.Substring(0, lugar) & txtAncho.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtAncho.Text = txtAncho.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtAncho.Select(txtAncho.Text.Length, 0)
        End If

    End Sub


    Private Sub txtLargo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLargo.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtLargo.Text.Contains(arrayCaractProhib(carp)) Then
                txtLargo.Text = txtLargo.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtLargo.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtLargo.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtLargo.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtLargo.Text.Length

                    If longitud > (lugar + 1) Then
                        txtLargo.Text = txtLargo.Text.Substring(0, lugar) & txtLargo.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtLargo.Text = txtLargo.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtLargo.Select(txtLargo.Text.Length, 0)
        End If

    End Sub


    Private Sub txtPrecioPieCubico_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPrecioPieCubico.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPrecioPieCubico.Text.Contains(arrayCaractProhib(carp)) Then
                txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPrecioPieCubico.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPrecioPieCubico.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPrecioPieCubico.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPrecioPieCubico.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Substring(0, lugar) & txtPrecioPieCubico.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPrecioPieCubico.Select(txtPrecioPieCubico.Text.Length, 0)
        End If

    End Sub


    Private Sub txtEspesor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEspesor.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "

        txtEspesor.Text = txtEspesor.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtAncho.Text = txtAncho.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtLargo.Text = txtLargo.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)

        Dim espesor As Double = 0.0
        Dim ancho As Double = 0.0
        Dim largo As Double = 0.0
        Dim preciopiecubico As Double = 0.0

        Try

            espesor = CDbl(txtEspesor.Text)
            ancho = CDbl(txtAncho.Text)
            largo = CDbl(txtLargo.Text)
            preciopiecubico = CDbl(txtPrecioPieCubico.Text)

        Catch ex As Exception

        End Try

        txtCostoSINIVA.Text = FormatCurrency(((espesor * ancho * largo) / 12) * preciopiecubico, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtAncho_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAncho.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "

        txtEspesor.Text = txtEspesor.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtAncho.Text = txtAncho.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtLargo.Text = txtLargo.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)

        Dim espesor As Double = 0.0
        Dim ancho As Double = 0.0
        Dim largo As Double = 0.0
        Dim preciopiecubico As Double = 0.0

        Try

            espesor = CDbl(txtEspesor.Text)
            ancho = CDbl(txtAncho.Text)
            largo = CDbl(txtLargo.Text)
            preciopiecubico = CDbl(txtPrecioPieCubico.Text)

        Catch ex As Exception

        End Try

        txtCostoSINIVA.Text = FormatCurrency(((espesor * ancho * largo) / 12) * preciopiecubico, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtLargo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLargo.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "

        txtEspesor.Text = txtEspesor.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtAncho.Text = txtAncho.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtLargo.Text = txtLargo.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)

        Dim espesor As Double = 0.0
        Dim ancho As Double = 0.0
        Dim largo As Double = 0.0
        Dim preciopiecubico As Double = 0.0

        Try

            espesor = CDbl(txtEspesor.Text)
            ancho = CDbl(txtAncho.Text)
            largo = CDbl(txtLargo.Text)
            preciopiecubico = CDbl(txtPrecioPieCubico.Text)

        Catch ex As Exception

        End Try

        txtCostoSINIVA.Text = FormatCurrency(((espesor * ancho * largo) / 12) * preciopiecubico, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtPrecioPieCubico_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrecioPieCubico.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "

        txtEspesor.Text = txtEspesor.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtAncho.Text = txtAncho.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtLargo.Text = txtLargo.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)

        Dim espesor As Double = 0.0
        Dim ancho As Double = 0.0
        Dim largo As Double = 0.0
        Dim preciopiecubico As Double = 0.0

        Try

            espesor = CDbl(txtEspesor.Text)
            ancho = CDbl(txtAncho.Text)
            largo = CDbl(txtLargo.Text)
            preciopiecubico = CDbl(txtPrecioPieCubico.Text)

        Catch ex As Exception

        End Try

        txtCostoSINIVA.Text = FormatCurrency(((espesor * ancho * largo) / 12) * preciopiecubico, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtPorcentajeDeProteccion_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeDeProteccion.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeDeProteccion.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeDeProteccion.Text = txtPorcentajeDeProteccion.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeDeProteccion.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeDeProteccion.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeDeProteccion.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeDeProteccion.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeDeProteccion.Text = txtPorcentajeDeProteccion.Text.Substring(0, lugar) & txtPorcentajeDeProteccion.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeDeProteccion.Text = txtPorcentajeDeProteccion.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeDeProteccion.Select(txtPorcentajeDeProteccion.Text.Length, 0)
        End If

    End Sub


    Private Sub txtCostoSINIVA_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCostoSINIVA.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtCostoSINIVA.Text.Contains(arrayCaractProhib(carp)) Then
                txtCostoSINIVA.Text = txtCostoSINIVA.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtCostoSINIVA.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtCostoSINIVA.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtCostoSINIVA.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtCostoSINIVA.Text.Length

                    If longitud > (lugar + 1) Then
                        txtCostoSINIVA.Text = txtCostoSINIVA.Text.Substring(0, lugar) & txtCostoSINIVA.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtCostoSINIVA.Text = txtCostoSINIVA.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtCostoSINIVA.Select(txtCostoSINIVA.Text.Length, 0)
        End If

    End Sub


    Private Sub txtPorcentajeDeProteccion_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeDeProteccion.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "

        txtPorcentajeDeProteccion.Text = txtPorcentajeDeProteccion.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtCostoSINIVA.Text = txtCostoSINIVA.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)

        Dim costosiniva As Double = 0.0
        Dim porcentaje As Double = 0.0

        Try

            costosiniva = CDbl(txtCostoSINIVA.Text)
            porcentaje = CDbl(txtPorcentajeDeProteccion.Text)

        Catch ex As Exception

        End Try

        txtCostoParaTabulador.Text = FormatCurrency(costosiniva * (1 + (porcentaje / 100)), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtCostoSINIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCostoSINIVA.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "

        txtPorcentajeDeProteccion.Text = txtPorcentajeDeProteccion.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtCostoSINIVA.Text = txtCostoSINIVA.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)

        Dim costosiniva As Double = 0.0
        Dim porcentaje As Double = 0.0

        Try

            costosiniva = CDbl(txtCostoSINIVA.Text)
            porcentaje = CDbl(txtPorcentajeDeProteccion.Text)

        Catch ex As Exception

        End Try

        txtCostoParaTabulador.Text = FormatCurrency(costosiniva * (1 + (porcentaje / 100)), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Function validaInsumo(ByVal silent As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos2 As String = "|@'\"""

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim(strcaracteresprohibidos.ToCharArray)
        txtNombreDelInsumo.Text = txtNombreDelInsumo.Text.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "")

        Dim strcaracteresprohibidos3 As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos3.ToCharArray

        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtCostoSINIVA.Text.Contains(arrayCaractProhib(carp)) Then
                txtCostoSINIVA.Text = txtCostoSINIVA.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtCostoSINIVA.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtCostoSINIVA.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtCostoSINIVA.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtCostoSINIVA.Text.Length

                    If longitud > (lugar + 1) Then
                        txtCostoSINIVA.Text = txtCostoSINIVA.Text.Substring(0, lugar) & txtCostoSINIVA.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtCostoSINIVA.Text = txtCostoSINIVA.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        Dim valorSINIVA As Double = 0.0
        Try
            valorSINIVA = CDbl(txtCostoSINIVA.Text)
        Catch ex As Exception

        End Try



        Dim resultado2 As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeDeProteccion.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeDeProteccion.Text = txtPorcentajeDeProteccion.Text.Replace(arrayCaractProhib(carp), "")
                resultado2 = True
            End If

        Next carp

        If txtPorcentajeDeProteccion.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeDeProteccion.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeDeProteccion.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeDeProteccion.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeDeProteccion.Text = txtPorcentajeDeProteccion.Text.Substring(0, lugar) & txtPorcentajeDeProteccion.Text.Substring(lugar + 1)
                        resultado2 = True
                        Exit For
                    Else
                        txtPorcentajeDeProteccion.Text = txtPorcentajeDeProteccion.Text.Substring(0, lugar)
                        resultado2 = True
                        Exit For
                    End If
                Next

            End If

        End If

        Dim valorPorcentaje As Double = 0.0
        Try
            valorPorcentaje = CDbl(txtPorcentajeDeProteccion.Text)
        Catch ex As Exception

        End Try





        If chkCubicar.Checked = True Then



            Dim resultado3 As Boolean = False

            For carp = 0 To arrayCaractProhib.Length - 1

                If txtEspesor.Text.Contains(arrayCaractProhib(carp)) Then
                    txtEspesor.Text = txtEspesor.Text.Replace(arrayCaractProhib(carp), "")
                    resultado = True
                End If

            Next carp

            If txtEspesor.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtEspesor.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtEspesor.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtEspesor.Text.Length

                        If longitud > (lugar + 1) Then
                            txtEspesor.Text = txtEspesor.Text.Substring(0, lugar) & txtEspesor.Text.Substring(lugar + 1)
                            resultado = True
                            Exit For
                        Else
                            txtEspesor.Text = txtEspesor.Text.Substring(0, lugar)
                            resultado = True
                            Exit For
                        End If
                    Next

                End If

            End If


            Dim valorEspesor As Double = 0.0
            Try
                valorEspesor = CDbl(txtEspesor.Text)
            Catch ex As Exception

            End Try



            Dim resultado4 As Boolean = False

            For carp = 0 To arrayCaractProhib.Length - 1

                If txtAncho.Text.Contains(arrayCaractProhib(carp)) Then
                    txtAncho.Text = txtAncho.Text.Replace(arrayCaractProhib(carp), "")
                    resultado = True
                End If

            Next carp

            If txtAncho.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtAncho.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtAncho.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtAncho.Text.Length

                        If longitud > (lugar + 1) Then
                            txtAncho.Text = txtAncho.Text.Substring(0, lugar) & txtAncho.Text.Substring(lugar + 1)
                            resultado = True
                            Exit For
                        Else
                            txtAncho.Text = txtAncho.Text.Substring(0, lugar)
                            resultado = True
                            Exit For
                        End If
                    Next

                End If

            End If


            Dim valorAncho As Double = 0.0
            Try
                valorAncho = CDbl(txtAncho.Text)
            Catch ex As Exception

            End Try



            Dim resultado5 As Boolean = False

            For carp = 0 To arrayCaractProhib.Length - 1

                If txtLargo.Text.Contains(arrayCaractProhib(carp)) Then
                    txtLargo.Text = txtLargo.Text.Replace(arrayCaractProhib(carp), "")
                    resultado = True
                End If

            Next carp

            If txtLargo.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtLargo.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtLargo.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtLargo.Text.Length

                        If longitud > (lugar + 1) Then
                            txtLargo.Text = txtLargo.Text.Substring(0, lugar) & txtLargo.Text.Substring(lugar + 1)
                            resultado = True
                            Exit For
                        Else
                            txtLargo.Text = txtLargo.Text.Substring(0, lugar)
                            resultado = True
                            Exit For
                        End If
                    Next

                End If

            End If


            Dim valorLargo As Double = 0.0
            Try
                valorLargo = CDbl(txtLargo.Text)
            Catch ex As Exception

            End Try



            Dim resultado6 As Boolean = False

            For carp = 0 To arrayCaractProhib.Length - 1

                If txtPrecioPieCubico.Text.Contains(arrayCaractProhib(carp)) Then
                    txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Replace(arrayCaractProhib(carp), "")
                    resultado = True
                End If

            Next carp

            If txtPrecioPieCubico.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtPrecioPieCubico.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtPrecioPieCubico.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtPrecioPieCubico.Text.Length

                        If longitud > (lugar + 1) Then
                            txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Substring(0, lugar) & txtPrecioPieCubico.Text.Substring(lugar + 1)
                            resultado = True
                            Exit For
                        Else
                            txtPrecioPieCubico.Text = txtPrecioPieCubico.Text.Substring(0, lugar)
                            resultado = True
                            Exit For
                        End If
                    Next

                End If

            End If


            Dim valorPrecioPieCubico As Double = 0.0
            Try
                valorPrecioPieCubico = CDbl(txtPrecioPieCubico.Text)
            Catch ex As Exception

            End Try





            If valorEspesor = 0 Or valorAncho = 0 Or valorLargo = 0 Or valorPrecioPieCubico = 0 Then
                If silent = False Then
                    MsgBox("¿Podrías especificar las medidas y/o precio del pie cúbico de la madera del Insumo?", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                Return False
            End If



        End If



        If txtNombreDelInsumo.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner una descripción a la Tarjeta?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If cmbTipoDeInsumo.SelectedIndex = -1 Then
            If silent = False Then
                MsgBox("¿Podrías escoger un Tipo de Insumo para este Insumo?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If txtUnidadDeMedida.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner una unidad de medida para el Insumo?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If txtCostoSINIVA.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner un Costo SIN IVA para el Insumo?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If txtPorcentajeDeProteccion.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner un Porcentaje de Protección para el Insumo?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If txtCostoSINIVA.Text = "0" Or txtCostoSINIVA.Text = "0.0" Or valorSINIVA = 0 Then
            If silent = False Then
                MsgBox("¿Podrías poner un Costo SIN IVA para el Insumo?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If txtPorcentajeDeProteccion.Text = "" Or txtPorcentajeDeProteccion.Text = "0" Or txtPorcentajeDeProteccion.Text = "0.0" Or valorPorcentaje = 0 Then

            If silent = True Then
                Return True
            End If

            If MsgBox("Seguro que deseas utilizar 0% como Porcentaje de Protección para el Insumo?", MsgBoxStyle.YesNo, "Confirmación Porcentaje en Cero") = MsgBoxResult.Yes Then
                txtPorcentajeDeProteccion.Text = valorPorcentaje / 100
                Return True
            Else
                Return False
            End If

        End If

        Return True

    End Function


    'Private Sub dgvPreciosHistoricosInsumo_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    Try
    '        ihistoricprojectid = CInt(dgvPreciosHistoricosInsumo.Rows(e.RowIndex).Cells(0).Value())
    '        ihistoriccardid = CInt(dgvPreciosHistoricosInsumo.Rows(e.RowIndex).Cells(1).Value())
    '    Catch ex As Exception

    '    End Try

    'End Sub


    'Private Sub dgvPreciosHistoricosInsumo_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    Try
    '        ihistoricprojectid = CInt(dgvPreciosHistoricosInsumo.Rows(e.RowIndex).Cells(0).Value())
    '        ihistoriccardid = CInt(dgvPreciosHistoricosInsumo.Rows(e.RowIndex).Cells(1).Value())
    '    Catch ex As Exception

    '    End Try

    'End Sub


    'Private Sub dgvPreciosHistoricosInsumo_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    If ihistoricprojectid = 0 Then
    '        Exit Sub
    '    End If

    '    If IsHistoric = False Then

    '        Dim ai As New AgregarInsumo
    '        ai.susername = susername
    '        ai.bactive = bactive
    '        ai.bonline = bonline
    '        ai.suserfullname = suserfullname

    '        ai.suseremail = suseremail
    '        ai.susersession = susersession
    '        ai.susermachinename = susermachinename
    '        ai.suserip = suserip


    '        ai.iprojectid = ihistoricprojectid
    '        ai.icardid = ihistoriccardid
    '        ai.iinputid = iinputid

    '        ai.IsEdit = True
    '        ai.IsHistoric = True
    '        ai.IsModel = IsModel
    '        ai.IsBase = IsBase

    'If Me.WindowState = FormWindowState.Maximized Then
    '        ai.WindowState = FormWindowState.Maximized
    '    End If

    '        Me.Visible = False
    '        ai.ShowDialog(Me)
    '        Me.Visible = True

    '    End If

    'End Sub


    'Private Sub dgvPreciosHistoricosInsumo_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    If ihistoricprojectid = 0 Then
    '        Exit Sub
    '    End If

    '    If IsHistoric = False Then

    '        Dim ai As New AgregarInsumo
    '        ai.susername = susername
    '        ai.bactive = bactive
    '        ai.bonline = bonline
    '        ai.suserfullname = suserfullname

    '        ai.suseremail = suseremail
    '        ai.susersession = susersession
    '        ai.susermachinename = susermachinename
    '        ai.suserip = suserip


    '        ai.iprojectid = ihistoricprojectid
    '        ai.icardid = ihistoriccardid
    '        ai.iinputid = iinputid

    '        ai.IsEdit = True
    '        ai.IsHistoric = True
    '        ai.IsModel = IsModel
    '        ai.IsBase = IsBase

    'If Me.WindowState = FormWindowState.Maximized Then
    '        ai.WindowState = FormWindowState.Maximized
    '    End If

    '        Me.Visible = False
    '        ai.ShowDialog(Me)
    '        Me.Visible = True

    '    End If

    'End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaInsumo(False) = False Then
            Exit Sub
        End If

        Dim timesInputIsOpen As Integer = 1

        timesInputIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Input" & iinputid & "'")

        If timesInputIsOpen > 1 And IsEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Insumo. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesInputIsOpen > 1 And IsEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Input" & iinputid + newIdAddition & "'") > 1 And IsEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(6) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Types"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Categories"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & " SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Types SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
            queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Categories SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iinputid = iinputid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        Dim porcentaje As Double = 0.0
        Dim precioSinIva As Double = 0.0

        Try

            porcentaje = CDbl(txtPorcentajeDeProteccion.Text)
            porcentaje = porcentaje / 100

            precioSinIva = CDbl(txtCostoSINIVA.Text)

        Catch ex As Exception

        End Try

        If IsEdit = False Then

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs")

            Dim queriesCreation(6) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Categories"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories ( `iinputid` int(11) NOT NULL, `sinputcategory` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            Dim queries(8) As String

            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            If chkCubicar.Checked = True Then
                queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber VALUES (" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
            End If

            queries(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories VALUES (" & iinputid & ", '" & txtCategoria.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            queries(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices VALUES (" & baseid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            If IsBase = True Then

                queries(5) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó Nuevo Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") a Presupuesto Base " & iprojectid & "', 'OK')"

            Else

                If IsModel = True Then

                    If chkCubicar.Checked = True Then
                        queries(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber VALUES (" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                    End If
                    queries(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices VALUES (" & iprojectid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                    queries(7) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó Nuevo Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") a Presupuesto Base y a Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"

                Else

                    If chkCubicar.Checked = True Then
                        queries(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber VALUES (" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                    End If
                    queries(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices VALUES (" & iprojectid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                    queries(7) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó Nuevo Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") a Presupuesto Base y a Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"

                End If

            End If

            If executeTransactedSQLCommand(0, queries) = True Then
                sinputdescription = txtNombreDelInsumo.Text
                sinputunit = txtUnidadDeMedida.Text
            End If

            Dim queriesSave(9) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM inputs " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti WHERE inputs.iinputid = ti.iinputid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM inputtypes " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit WHERE inputtypes.iinputid = tit.iinputid) "

            queriesSave(2) = "" & _
            "DELETE " & _
            "FROM inputcategories " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic WHERE inputcategories.iinputid = tic.iinputid) "

            queriesSave(3) = "" & _
            "UPDATE inputs i JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti ON ti.iinputid = i.iinputid SET i.iupdatedate = ti.iupdatedate, i.supdatetime = ti.supdatetime, i.supdateusername = ti.supdateusername, i.sinputdescription = ti.sinputdescription, i.sinputunit = ti.sinputunit WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE inputtypes it JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit ON tit.iinputid = it.iinputid SET it.iupdatedate = tit.iupdatedate, it.supdatetime = tit.supdatetime, it.supdateusername = tit.supdateusername, it.sinputtypedescription = tit.sinputtypedescription WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') "

            queriesSave(5) = "" & _
            "UPDATE inputcategories ic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic ON tic.iinputid = ic.iinputid SET ic.iupdatedate = tic.iupdatedate, ic.supdatetime = tic.supdatetime, ic.supdateusername = tic.supdateusername, ic.sinputcategory = tic.sinputcategory WHERE STR_TO_DATE(CONCAT(tic.iupdatedate, ' ', tic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ic.iupdatedate, ' ', ic.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO inputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
            "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO inputtypes " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
            "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") "

            queriesSave(8) = "" & _
            "INSERT INTO inputcategories " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic " & _
            "WHERE NOT EXISTS (SELECT * FROM inputcategories ic WHERE ic.iinputid = tic.iinputid AND ic.iinputid = " & iinputid & ") "

            If executeTransactedSQLCommand(0, queriesSave) = True Then

                wasCreated = True

                Cursor.Current = System.Windows.Forms.Cursors.Default

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.Default
                MsgBox("Error de Guardado. Intente nuevamente.")
                Exit Sub

            End If

        End If

        Dim aipru As New AgregarInsumoPreguntaReplicarUpdate

        aipru.susername = susername
        aipru.bactive = bactive
        aipru.bonline = bonline
        aipru.suserfullname = suserfullname

        aipru.suseremail = suseremail
        aipru.susersession = susersession
        aipru.susermachinename = susermachinename
        aipru.suserip = suserip

        aipru.IsModel = IsModel
        aipru.IsBase = IsBase

        If IsBase = True Then

            aipru.iselectedoption = 2
            aipru.DialogResult = Windows.Forms.DialogResult.OK

        Else

            aipru.ShowDialog(Me)

        End If


        If aipru.DialogResult = Windows.Forms.DialogResult.OK Then

            If aipru.iselectedoption = 0 Then
                Exit Sub
            End If

            sinputdescription = txtNombreDelInsumo.Text
            sinputunit = txtUnidadDeMedida.Text

            If IsEdit = True Then

                If IsBase = True Then

                    If aipru.iselectedoption = 2 Then    'Base y Modelos

                        fecha = getMySQLDate()
                        hora = getAppTime()

                        Dim baseid As Integer = 0
                        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        If baseid = 0 Then
                            baseid = 99999
                        End If

                        Dim dsModelos As DataSet
                        dsModelos = getSQLQueryAsDataset(0, "SELECT imodelid FROM models")

                        Dim queries(dsModelos.Tables(0).Rows.Count + 7) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername ='" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                        If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories WHERE iinputid = " & iinputid) = 1 Then
                            queries(2) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories SET sinputcategory = '" & txtCategoria.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                        Else
                            queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories VALUES(" & iinputid & ", '" & txtCategoria.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"
                        End If

                        If chkCubicar.Checked = True Then

                            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid) = 1 Then
                                queries(3) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid
                            Else
                                queries(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber VALUES(" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            End If

                        Else
                            queries(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid
                        End If

                        queries(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices VALUES (" & baseid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        For i = 0 To dsModelos.Tables(0).Rows.Count - 1

                            queries(i + 5) = "INSERT INTO modelprices VALUES (" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                            If chkCubicar.Checked = True Then

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM modeltimber WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid) = 1 Then
                                    queries(i + 6) = "UPDATE modeltimber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid
                                Else
                                    queries(i + 6) = "INSERT INTO modeltimber VALUES(" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                End If

                            Else
                                queries(i + 6) = "DELETE FROM modeltimber WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid
                            End If

                        Next i

                        queries(dsModelos.Tables(0).Rows.Count + 6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en Presupuesto Base y Modelos', 'OK')"

                        executeTransactedSQLCommand(0, queries)

                    End If

                Else

                    If IsModel = True Then

                        If aipru.iselectedoption = 2 Then 'Selecciona Actualizar Precio En General 


                            fecha = getMySQLDate()
                            hora = getAppTime()

                            Dim baseid As Integer = 0
                            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                            If baseid = 0 Then
                                baseid = 99999
                            End If

                            Dim dsModelos As DataSet
                            dsModelos = getSQLQueryAsDataset(0, "SELECT imodelid FROM models")

                            Dim queries(dsModelos.Tables(0).Rows.Count + 7) As String

                            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories WHERE iinputid = " & iinputid) = 1 Then
                                queries(2) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories SET sinputcategory = '" & txtCategoria.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                            Else
                                queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories VALUES(" & iinputid & ", '" & txtCategoria.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"
                            End If

                            If chkCubicar.Checked = True Then

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid) = 1 Then
                                    queries(3) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid
                                Else
                                    queries(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber VALUES(" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                End If

                            Else
                                queries(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid
                            End If

                            queries(4) = "INSERT INTO baseprices VALUES (" & baseid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                            For i = 0 To dsModelos.Tables(0).Rows.Count - 1

                                If dsModelos.Tables(0).Rows(i).Item(0) = iprojectid Then

                                    queries(i + 5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices VALUES (" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                    If chkCubicar.Checked = True Then

                                        If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber WHERE imodelid = " & iprojectid & " AND iinputid = " & iinputid) = 1 Then
                                            queries(i + 6) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND iinputid = " & iinputid
                                        Else
                                            queries(i + 6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber VALUES(" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                        End If

                                    Else
                                        queries(i + 6) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber WHERE imodelid = " & iprojectid & " AND iinputid = " & iinputid
                                    End If

                                Else

                                    queries(i + 5) = "INSERT INTO modelprices VALUES (" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                    If chkCubicar.Checked = True Then

                                        If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM modeltimber WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid) = 1 Then
                                            queries(i + 6) = "UPDATE modeltimber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid
                                        Else
                                            queries(i + 6) = "INSERT INTO modeltimber VALUES(" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                        End If

                                    Else
                                        queries(i + 6) = "DELETE FROM modeltimber WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid
                                    End If

                                End If

                            Next i

                            queries(dsModelos.Tables(0).Rows.Count + 6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en Presupuesto Base y (por venir desde un Proyecto) al Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"

                            executeTransactedSQLCommand(0, queries)


                        ElseIf aipru.iselectedoption = 3 Then    'Solo este modelo

                            fecha = getMySQLDate()
                            hora = getAppTime()

                            Dim queries(6) As String

                            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories WHERE iinputid = " & iinputid) = 1 Then
                                queries(2) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories SET sinputcategory = '" & txtCategoria.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                            Else
                                queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories VALUES(" & iinputid & ", '" & txtCategoria.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"
                            End If

                            If chkCubicar.Checked = True Then

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber WHERE imodelid = " & iprojectid & " AND iinputid = " & iinputid) = 1 Then
                                    queries(3) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND iinputid = " & iinputid
                                Else
                                    queries(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber VALUES(" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                End If

                            Else
                                queries(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber WHERE imodelid = " & iprojectid & " AND iinputid = " & iinputid
                            End If

                            queries(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices VALUES (" & iprojectid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                            queries(5) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"

                            executeTransactedSQLCommand(0, queries)

                        End If

                    Else

                        If aipru.iselectedoption = 2 Then 'Selecciona Actualizar Precio En General 


                            fecha = getMySQLDate()
                            hora = getAppTime()

                            Dim baseid As Integer = 0
                            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                            If baseid = 0 Then
                                baseid = 99999
                            End If

                            Dim dsModelos As DataSet
                            dsModelos = getSQLQueryAsDataset(0, "SELECT imodelid FROM models")

                            Dim queries(dsModelos.Tables(0).Rows.Count + 10) As String

                            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories WHERE iinputid = " & iinputid) = 1 Then
                                queries(2) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories SET sinputcategory = '" & txtCategoria.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                            Else
                                queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories VALUES(" & iinputid & ", '" & txtCategoria.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"
                            End If

                            If chkCubicar.Checked = True Then

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid) = 1 Then
                                    queries(3) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid
                                Else
                                    queries(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber VALUES(" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                End If

                            Else
                                queries(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber WHERE ibaseid = " & iprojectid & " AND iinputid = " & iinputid
                            End If

                            queries(4) = "INSERT INTO baseprices VALUES (" & baseid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                            For i = 0 To dsModelos.Tables(0).Rows.Count - 1

                                queries(i + 5) = "INSERT INTO modelprices VALUES (" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                If chkCubicar.Checked = True Then

                                    If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM modeltimber WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid) = 1 Then
                                        queries(i + 6) = "UPDATE modeltimber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid
                                    Else
                                        queries(i + 6) = "INSERT INTO modeltimber VALUES(" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                    End If

                                Else
                                    queries(i + 6) = "DELETE FROM modeltimber WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND iinputid = " & iinputid
                                End If

                            Next i

                            queries(dsModelos.Tables(0).Rows.Count + 7) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices VALUES (" & iprojectid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                            If chkCubicar.Checked = True Then

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber WHERE iprojectid = " & iprojectid & " AND iinputid = " & iinputid) = 1 Then
                                    queries(dsModelos.Tables(0).Rows.Count + 8) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid & " AND iinputid = " & iinputid
                                Else
                                    queries(dsModelos.Tables(0).Rows.Count + 8) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber VALUES(" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                End If

                            Else
                                queries(dsModelos.Tables(0).Rows.Count + 8) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber WHERE iprojectid = " & iprojectid & " AND iinputid = " & iinputid
                            End If

                            queries(dsModelos.Tables(0).Rows.Count + 9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en Presupuesto Base y (por venir desde un Proyecto) al Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"

                            executeTransactedSQLCommand(0, queries)

                        ElseIf aipru.iselectedoption = 1 Then    'Solo este proyecto

                            fecha = getMySQLDate()
                            hora = getAppTime()

                            Dim queries(6) As String

                            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories WHERE iinputid = " & iinputid) = 1 Then
                                queries(2) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories SET sinputcategory = '" & txtCategoria.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                            Else
                                queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories VALUES(" & iinputid & ", '" & txtCategoria.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"
                            End If

                            If chkCubicar.Checked = True Then

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber WHERE iprojectid = " & iprojectid & " AND iinputid = " & iinputid) = 1 Then
                                    queries(3) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber SET dinputtimberespesor = " & txtEspesor.Text & ", dinputtimberancho = " & txtAncho.Text & ", dinputtimberlargo = " & txtLargo.Text & " , dinputtimberpreciopiecubico = " & txtPrecioPieCubico.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid & " AND iinputid = " & iinputid
                                Else
                                    queries(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber VALUES(" & iprojectid & ", " & iinputid & ", " & txtEspesor.Text & ", " & txtAncho.Text & ", " & txtLargo.Text & ", " & txtPrecioPieCubico.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                End If

                            Else
                                queries(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber WHERE iprojectid = " & iprojectid & " AND iinputid = " & iinputid
                            End If

                            queries(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices VALUES (" & iprojectid & ", " & iinputid & ", " & txtCostoSINIVA.Text & ", " & porcentaje & ", " & precioSinIva * (1 + porcentaje) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                            queries(5) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"

                            executeTransactedSQLCommand(0, queries)

                        End If


                    End If

                End If


                Dim queriesSave(9) As String

                queriesSave(0) = "" & _
                "DELETE " & _
                "FROM inputs " & _
                "WHERE iinputid = " & iinputid & " AND " & _
                "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti WHERE inputs.iinputid = ti.iinputid) "

                queriesSave(1) = "" & _
                "DELETE " & _
                "FROM inputtypes " & _
                "WHERE iinputid = " & iinputid & " AND " & _
                "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit WHERE inputtypes.iinputid = tit.iinputid) "

                queriesSave(2) = "" & _
                "DELETE " & _
                "FROM inputcategories " & _
                "WHERE iinputid = " & iinputid & " AND " & _
                "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic WHERE inputcategories.iinputid = tic.iinputid) "

                queriesSave(3) = "" & _
                "UPDATE inputs i JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti ON ti.iinputid = i.iinputid SET i.iupdatedate = ti.iupdatedate, i.supdatetime = ti.supdatetime, i.supdateusername = ti.supdateusername, i.sinputdescription = ti.sinputdescription, i.sinputunit = ti.sinputunit WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') "

                queriesSave(4) = "" & _
                "UPDATE inputtypes it JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit ON tit.iinputid = it.iinputid SET it.iupdatedate = tit.iupdatedate, it.supdatetime = tit.supdatetime, it.supdateusername = tit.supdateusername, it.sinputtypedescription = tit.sinputtypedescription WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') "

                queriesSave(5) = "" & _
                "UPDATE inputcategories ic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic ON tic.iinputid = ic.iinputid SET ic.iupdatedate = tic.iupdatedate, ic.supdatetime = tic.supdatetime, ic.supdateusername = tic.supdateusername, ic.sinputcategory = tic.sinputcategory WHERE STR_TO_DATE(CONCAT(tic.iupdatedate, ' ', tic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ic.iupdatedate, ' ', ic.supdatetime), '%Y%c%d %T') "

                queriesSave(6) = "" & _
                "INSERT INTO inputs " & _
                "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
                "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") "

                queriesSave(7) = "" & _
                "INSERT INTO inputtypes " & _
                "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
                "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") "

                queriesSave(8) = "" & _
                "INSERT INTO inputcategories " & _
                "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories tic " & _
                "WHERE NOT EXISTS (SELECT * FROM inputcategories ic WHERE ic.iinputid = tic.iinputid AND ic.iinputid = " & iinputid & ") "

                If executeTransactedSQLCommand(0, queriesSave) = True Then

                    wasCreated = True

                    Cursor.Current = System.Windows.Forms.Cursors.Default

                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    MsgBox("Error de Guardado. Intente nuevamente.")
                    Exit Sub

                End If

            End If

        End If

    End Sub


End Class