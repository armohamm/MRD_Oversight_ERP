
Public Class AgregarInsumoCompuesto

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

    Public sinputdescription As String = ""
    Public sinputunit As String = ""

    Public IsEdit As Boolean = False
    Public IsRecover As Boolean = False

    Public IsBase As Boolean = False
    Public IsModel As Boolean = False
    Public IsHistoric As Boolean = False

    Public iselectedinputid As Integer = 0
    Public sselectedinputdescription As String = ""
    Public sselectedunit As String = ""
    Public dselectedinputqty As Double = 0.0

    Public ihistoricprojectid As Integer = 0
    Public ihistoriccardid As Integer = 0

    Private isFormReadyForAction As Boolean = False
    Private isUnidadReady As Boolean = False

    Private WithEvents txtCantidadDgvInsumos As TextBox
    Private WithEvents txtNombreDgvInsumos As TextBox

    Private txtCantidadDgvInsumos_OldText As String = ""
    Private txtNombreDgvInsumos_OldText As String = ""

    Public wasCreated As Boolean = False

    Private openInputPermission As Boolean = False
    Private openHistoricPermission As Boolean = False

    Private newInputPermission As Boolean = False
    Private modifyInputQtyPermission As Boolean = False
    Private insertInputPermission As Boolean = False
    Private deleteInputPermission As Boolean = False

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

                If permission = "Abrir Insumo" Then
                    openInputPermission = True
                End If

                If permission = "Nuevo Insumo" Then
                    newInputPermission = True
                End If

                If permission = "Insertar Insumo" Then
                    insertInputPermission = True
                End If

                If permission = "Eliminar Insumo" Then
                    deleteInputPermission = True
                End If

                If permission = "Modificar Cantidad" Then
                    modifyInputQtyPermission = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Insumo Compuesto', 'OK')")

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


    Private Sub AgregarInsumoCompuesto_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tit.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit JOIN inputtypes it ON tit.iinputid = it.iinputid WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(ti.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti JOIN inputs i ON ti.iinputid = i.iinputid WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
        "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
        "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaInsumosCompuestos(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK And IsHistoric = False Then
            incomplete = True
        End If

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

                Dim queriesNewId(11) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Types"
                'queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Categories"
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & " SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Types SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                'queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Categories SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

                If IsBase = True Then

                    queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(8) = "UPDATE oversight.modelcardcompoundinputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

                Else

                    If IsModel = True Then

                        queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                        queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                        queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                        queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                        queriesNewId(10) = "UPDATE oversight.modelcardcompoundinputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

                    Else

                        queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                        queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                        queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                        queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                        queriesNewId(10) = "UPDATE oversight.projectcardcompoundinputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

                    End If

                End If

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

            queriesSave(3) = "" & _
            "UPDATE inputs i JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti ON ti.iinputid = i.iinputid SET i.iupdatedate = ti.iupdatedate, i.supdatetime = ti.supdatetime, i.supdateusername = ti.supdateusername, i.sinputdescription = ti.sinputdescription, i.sinputunit = ti.sinputunit WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE inputtypes it JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit ON tit.iinputid = it.iinputid SET it.iupdatedate = tit.iupdatedate, it.supdatetime = tit.supdatetime, it.supdateusername = tit.supdateusername, it.sinputtypedescription = tit.sinputtypedescription WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO inputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
            "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO inputtypes " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
            "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") "

            queriesSave(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " : " & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

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

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
        queriesDelete(2) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Insumo Compuesto " & iinputid & ": " & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"
        'queriesDelete(3) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Input', 'Insumo Compuesto', '" & iinputid & "', '" & txtNombreDelInsumo.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarInsumoCompuesto_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarInsumoCompuesto_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesInputIsOpen As Integer = 0

        timesInputIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Input" & iinputid & "'")

        If timesInputIsOpen > 0 And IsEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Insumo. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Insumo?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If


        If IsRecover = False Then

            Dim queriesCreation(6) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        If IsEdit = True Then

            Dim queryInsumo As String
            Dim dsInsumo As DataSet

            If IsRecover = False Then

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SELECT * FROM inputs WHERE iinputid = " & iinputid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SELECT * FROM inputtypes WHERE iinputid = " & iinputid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            queryInsumo = "SELECT i.iinputid, i.sinputdescription, i.sinputunit, it.sinputtypedescription " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types it ON it.iinputid = i.iinputid " & _
            "WHERE i.iinputid = " & iinputid

            dsInsumo = getSQLQueryAsDataset(0, queryInsumo)

            txtNombreDelInsumo.Text = dsInsumo.Tables(0).Rows(0).Item("sinputdescription")
            cmbTipoDeInsumo.SelectedItem = dsInsumo.Tables(0).Rows(0).Item("sinputtypedescription")
            txtUnidadDeMedida.Text = dsInsumo.Tables(0).Rows(0).Item("sinputunit")


            Dim querySumaInsumoCompuesto As String = ""
            Dim sumaInsumoCompuesto As Double

            If IsBase = True Then

                querySumaInsumoCompuesto = "" & _
                "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                "GROUP BY btfci.iinputid "

            Else

                If IsModel = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                    "GROUP BY mtfci.iinputid "

                Else

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                    "GROUP BY ptfci.iinputid "

                End If

            End If


            sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

            txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            If IsHistoric = True Then

                txtNombreDelInsumo.Enabled = False
                cmbTipoDeInsumo.Enabled = False
                txtUnidadDeMedida.Enabled = False
                dgvInsumos.ReadOnly = True
                txtCostoParaTabulador.Enabled = False

            Else

                dgvInsumos.Enabled = True
                dgvInsumos.ReadOnly = False
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True

            End If


        End If


        Dim queryInsumos As String = ""

        If IsBase = True Then

            queryInsumos = "" & _
            "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
            "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
            "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

        Else

            If IsModel = True Then

                queryInsumos = "" & _
                "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

            Else

                queryInsumos = "" & _
                "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

            End If

        End If


        setDataGridView(dgvInsumos, queryInsumos, False)

        dgvInsumos.Columns(0).Visible = False

        dgvInsumos.Columns(0).ReadOnly = True
        dgvInsumos.Columns(2).ReadOnly = True
        dgvInsumos.Columns(3).ReadOnly = True
        dgvInsumos.Columns(5).ReadOnly = True


        'Dim queryPreciosHistoricosInsumoCompuesto As String = ""

        'If IsBase = True Then

        '    queryPreciosHistoricosInsumoCompuesto = "" & _
        '    "SELECT btfci.ibaseid, btfci.icardid, STR_TO_DATE(CONCAT(btfci.iupdatedate, ' ', btfci.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
        '    "FORMAT(SUM(btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Precio' " & _
        '    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
        '    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
        '    "GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid "

        'Else

        '    If IsModel = True Then

        '        queryPreciosHistoricosInsumoCompuesto = "" & _
        '        "SELECT mtfci.imodelid, mtfci.icardid, STR_TO_DATE(CONCAT(mtfci.iupdatedate, ' ', mtfci.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
        '        "FORMAT(SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Precio' " & _
        '        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
        '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
        '        "GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid "

        '    Else

        '        queryPreciosHistoricosInsumoCompuesto = "" & _
        '        "SELECT ptfci.iprojectid, ptfci.icardid, STR_TO_DATE(CONCAT(ptfci.iupdatedate, ' ', ptfci.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
        '        "FORMAT(SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Precio' " & _
        '        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
        '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
        '        "GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid "

        '    End If

        'End If



        'Dim dtPreciosHistoricosInsumoCompuesto As DataTable
        'dtPreciosHistoricosInsumoCompuesto = setDataGridView(dgvPreciosHistoricosInsumo, queryPreciosHistoricosInsumoCompuesto, True)

        'dgvPreciosHistoricosInsumo.Columns(0).Visible = False
        'dgvPreciosHistoricosInsumo.Columns(1).Visible = False


        'CODIGO PARA LA GRAFICA


        If IsHistoric = True Then

            btnGuardar.Visible = False

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Insumo Compuesto " & iinputid & ": " & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
        'executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Input', 'Insumo Compuesto', '" & iinputid & "', '" & txtNombreDelInsumo.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True
        isUnidadReady = True

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

        If isFormReadyForAction = False Then
            Exit Sub
        End If

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

            Else

                txtUnidadDeMedida.Focus()
                Exit Sub

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub cmbTipoDeInsumo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTipoDeInsumo.SelectedIndexChanged

        If validaInsumo(True) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        If IsEdit = True Then

            Dim queries(3) As String

            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

            If IsBase = True Then
                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
            Else
                If IsModel = True Then
                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                Else
                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                End If
            End If

            executeTransactedSQLCommand(0, queries)

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

            If checkIfItsOnlyTextUpdate = True Then

                Dim queries(3) As String

                queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                If iinputid <> 0 Then
                    queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                End If

                If IsBase = True Then
                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                Else
                    If IsModel = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                    Else
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                    End If
                End If

                executeTransactedSQLCommand(0, queries)

            Else

                Dim queries(3) As String

                iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                Dim queriesCreation(9) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                executeTransactedSQLCommand(0, queriesCreation)

                queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                If IsBase = True Then
                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                Else
                    If IsModel = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                    Else
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                    End If
                End If

                executeTransactedSQLCommand(0, queries)

            End If

        End If

        dgvInsumos.Enabled = True
        btnNuevoInsumo.Enabled = True
        btnInsertarInsumo.Enabled = True
        btnEliminarInsumo.Enabled = True

    End Sub


    Private Function validaInsumo(ByVal silent As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos2 As String = "|@'\"""

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim(strcaracteresprohibidos.ToCharArray)
        txtNombreDelInsumo.Text = txtNombreDelInsumo.Text.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "")


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

        Return True


    End Function


    Private Function validaInsumosCompuestos(ByVal silent As Boolean) As Boolean

        Dim valorInsumoCompuesto As Double = 0.0
        Dim queryValorInsumoCompuesto As String

        If IsBase = True Then

            queryValorInsumoCompuesto = "" & _
            "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
            "GROUP BY btfci.iinputid "

        Else

            If IsModel = True Then

                queryValorInsumoCompuesto = "" & _
                "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                "GROUP BY mtfci.iinputid "

            Else

                queryValorInsumoCompuesto = "" & _
                "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                "GROUP BY ptfci.iinputid "

            End If

        End If


        Try
            valorInsumoCompuesto = getSQLQueryAsDouble(0, queryValorInsumoCompuesto)
        Catch ex As Exception

        End Try


        If valorInsumoCompuesto = 0 Or valorInsumoCompuesto = 0.0 Then

            If silent = False Then


                MsgBox("¿Podrías poner algún Insumo para este Insumo Compuesto?", MsgBoxStyle.OkOnly, "Dato Faltante")


            End If

            Return False

        End If


        If validaInsumo(silent) = False Then
            Return False
        End If


        Return True

    End Function


    Private Sub dgvInsumos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellClick

        If validaInsumo(True) = False Then
            Exit Sub
        End If

        Try

            If dgvInsumos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(3).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

    End Sub


    Private Sub dgvInsumos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellContentClick

        If validaInsumo(True) = False Then
            Exit Sub
        End If

        Try

            If dgvInsumos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(3).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

    End Sub


    Private Sub dgvInsumos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvInsumos.SelectionChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        txtNombreDgvInsumos = Nothing
        txtNombreDgvInsumos_OldText = Nothing
        txtCantidadDgvInsumos = Nothing
        txtCantidadDgvInsumos_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Try

            If dgvInsumos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvInsumos.CurrentRow.Cells(0).Value)
            sselectedinputdescription = dgvInsumos.CurrentRow.Cells(1).Value()
            sselectedunit = dgvInsumos.CurrentRow.Cells(2).Value()
            dselectedinputqty = CDbl(dgvInsumos.CurrentRow.Cells(3).Value)

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queryInsumos As String = ""

        If IsBase = True Then

            queryInsumos = "" & _
            "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
            "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
            "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

        Else

            If IsModel = True Then

                queryInsumos = "" & _
                "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

            Else

                queryInsumos = "" & _
                "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

            End If

        End If

        setDataGridView(dgvInsumos, queryInsumos, False)

        dgvInsumos.Columns(0).Visible = False

        dgvInsumos.Columns(0).ReadOnly = True
        dgvInsumos.Columns(2).ReadOnly = True
        dgvInsumos.Columns(3).ReadOnly = True
        dgvInsumos.Columns(5).ReadOnly = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellValueChanged

        If modifyInputQtyPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LAS UNICAS COLUMNAS EDITABLES SON LAS COLUMNAS 1 y 3: sinputdescription Y dcompoundinputqty

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If e.ColumnIndex = 1 Then

            'sinputdescription
            If dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de Insumo Compuesto") = MsgBoxResult.Yes Then

                    If IsBase = True Then

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid)

                    Else

                        If IsModel = True Then

                            Dim queriesDelete(2) As String

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid

                            executeTransactedSQLCommand(0, queriesDelete)

                        Else

                            Dim queriesDelete(2) As String

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid

                            executeTransactedSQLCommand(0, queriesDelete)

                        End If

                    End If


                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY btfci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY mtfci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ptfci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                Else

                    dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                End If

            Else

                'Si pone un texto, e.g. una descripcion de otro producto

                dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

                Dim bi As New BuscaInsumos
                bi.susername = susername
                bi.bactive = bactive
                bi.bonline = bonline
                bi.suserfullname = suserfullname

                bi.suseremail = suseremail
                bi.susersession = susersession
                bi.susermachinename = susermachinename
                bi.suserip = suserip



                bi.querystring = dgvInsumos.CurrentCell.EditedFormattedValue

                bi.IsEdit = False

                bi.IsBase = IsBase
                bi.IsModel = IsModel

                If Me.WindowState = FormWindowState.Maximized Then
                    bi.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                bi.ShowDialog(Me)
                Me.Visible = True

                If bi.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim dsBusquedaInsumosRepetidos As DataSet

                    If IsBase = True Then

                        dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

                    Else

                        If IsModel = True Then

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

                        Else

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

                        End If

                    End If


                    If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                        MsgBox("Ya tienes ese Insumo insertado en este Insumo Compuesto. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido")
                        dgvInsumos.EndEdit()
                        Exit Sub

                    End If


                    If MsgBox("¿Estás seguro de que deseas reemplazar el Insumo " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "?", MsgBoxStyle.YesNo, "Confirmación de Reemplazo de Insumo") = MsgBoxResult.Yes Then

                        Dim cantidaddeinsumo As Double = 1

                        Try
                            cantidaddeinsumo = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(3).Value)
                        Catch ex As Exception

                        End Try

                        If IsBase = True Then

                            Dim queries(3) As String

                            queries(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                            executeTransactedSQLCommand(0, queries)

                        Else

                            If IsModel = True Then

                                Dim queries(4) As String

                                queries(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                                queries(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                                queries(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                                executeTransactedSQLCommand(0, queries)

                            Else

                                Dim queries(4) As String

                                queries(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                                queries(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                                queries(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                                executeTransactedSQLCommand(0, queries)

                            End If

                        End If

                        Dim querySumaInsumoCompuesto As String
                        Dim sumaInsumoCompuesto As Double

                        If IsBase = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                            "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                            "GROUP BY btfci.iinputid "

                        Else

                            If IsModel = True Then

                                querySumaInsumoCompuesto = "" & _
                                "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                                "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                                "GROUP BY mtfci.iinputid "

                            Else

                                querySumaInsumoCompuesto = "" & _
                                "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                                "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                                "GROUP BY ptfci.iinputid "

                            End If

                        End If

                        sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                        txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                    Else

                        'Si cancela el reemplazo de insumo
                        dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                    End If


                Else

                    'Si cancela el reemplazo de insumo
                    dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                End If

            End If

        ElseIf e.ColumnIndex = 4 Then

            'dcompoundinputqty

            If dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de Insumo Compuesto") = MsgBoxResult.Yes Then

                    If IsBase = True Then

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid)

                    Else

                        If IsModel = True Then

                            Dim queriesDelete(2) As String

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid

                            executeTransactedSQLCommand(0, queriesDelete)

                        Else

                            Dim queriesDelete(2) As String

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid

                            executeTransactedSQLCommand(0, queriesDelete)

                        End If

                    End If


                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY btfci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY mtfci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ptfci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                Else

                    dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedinputqty

                End If


            ElseIf dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de Insumo Compuesto") = MsgBoxResult.Yes Then

                    If IsBase = True Then

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid)

                    Else

                        If IsModel = True Then

                            Dim queriesDelete(2) As String

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid

                            executeTransactedSQLCommand(0, queriesDelete)

                        Else

                            Dim queriesDelete(2) As String

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid

                            executeTransactedSQLCommand(0, queriesDelete)

                        End If

                    End If


                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY btfci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY mtfci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ptfci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                Else

                    dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedinputqty

                End If

            Else

                'Si pone un número

                Dim cantidaddeinsumo As Double = 1.0

                Try
                    cantidaddeinsumo = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(4).Value)
                Catch ex As Exception
                    cantidaddeinsumo = dselectedinputqty
                End Try


                If IsBase = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET dcompoundinputqty = " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid)

                Else

                    If IsModel = True Then

                        Dim queriesUpdate(2) As String

                        'queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET dcompoundinputqty = " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SET dcompoundinputqty = " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid

                        executeTransactedSQLCommand(0, queriesUpdate)

                    Else

                        Dim queriesUpdate(2) As String

                        'queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET dcompoundinputqty = " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SET dcompoundinputqty = " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid

                        executeTransactedSQLCommand(0, queriesUpdate)

                    End If

                End If


                Dim querySumaInsumoCompuesto As String
                Dim sumaInsumoCompuesto As Double

                If IsBase = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                    "GROUP BY btfci.iinputid "

                Else

                    If IsModel = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                        "GROUP BY mtfci.iinputid "

                    Else

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                        "GROUP BY ptfci.iinputid "

                    End If

                End If

                sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvInsumos.EditingControlShowing

        If dgvInsumos.CurrentCell.ColumnIndex = 2 Then

            txtNombreDgvInsumos = CType(e.Control, TextBox)
            txtNombreDgvInsumos_OldText = txtNombreDgvInsumos.Text

        ElseIf dgvInsumos.CurrentCell.ColumnIndex = 4 Then

            txtCantidadDgvInsumos = CType(e.Control, TextBox)
            txtCantidadDgvInsumos_OldText = txtCantidadDgvInsumos.Text

        Else

            txtNombreDgvInsumos = Nothing
            txtNombreDgvInsumos_OldText = Nothing
            txtCantidadDgvInsumos = Nothing
            txtCantidadDgvInsumos_OldText = Nothing

        End If

    End Sub


    Private Sub dgvInsumos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvInsumos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deleteInputPermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmar Eliminación de Insumo de Insumo Compuesto") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedinputid As Integer = 0
                Try
                    tmpselectedinputid = CInt(dgvInsumos.CurrentRow.Cells(0).Value)
                Catch ex As Exception

                End Try


                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 99999
                End If

                If IsBase = True Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & tmpselectedinputid)

                Else

                    If IsModel = True Then

                        Dim queriesDelete(2) As String

                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                        queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & tmpselectedinputid

                        executeTransactedSQLCommand(0, queriesDelete)

                    Else

                        Dim queriesDelete(2) As String

                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                        queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & tmpselectedinputid

                        executeTransactedSQLCommand(0, queriesDelete)

                    End If

                End If


                Dim querySumaInsumoCompuesto As String
                Dim sumaInsumoCompuesto As Double

                If IsBase = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                    "GROUP BY btfci.iinputid "

                Else

                    If IsModel = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                        "GROUP BY mtfci.iinputid "

                    Else

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                        "GROUP BY ptfci.iinputid "

                    End If

                End If

                sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)


                Dim queryInsumos As String = ""

                If IsBase = True Then

                    queryInsumos = "" & _
                    "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

                Else

                    If IsModel = True Then

                        queryInsumos = "" & _
                        "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

                    Else

                        queryInsumos = "" & _
                        "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

                    End If

                End If

                setDataGridView(dgvInsumos, queryInsumos, False)

                dgvInsumos.Columns(0).Visible = False

                dgvInsumos.Columns(0).ReadOnly = True
                dgvInsumos.Columns(2).ReadOnly = True
                dgvInsumos.Columns(3).ReadOnly = True
                dgvInsumos.Columns(5).ReadOnly = True

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvInsumos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvInsumos.Click

        If validaInsumo(True) = True And iinputid <> 0 Then

            dgvInsumos.Enabled = True
            btnNuevoInsumo.Enabled = True
            btnInsertarInsumo.Enabled = True
            btnEliminarInsumo.Enabled = True

        ElseIf validaInsumo(True) = True And iinputid = 0 Then


            'Inicia Save de Master del Insumo Compuesto

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            If IsHistoric = True Then
                Exit Sub
            End If

            Dim unitfound As Boolean = False
            Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

            txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
            txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

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

                    If validaInsumo(True) = False Then
                        Cursor.Current = System.Windows.Forms.Cursors.Default
                        Exit Sub
                    Else
                        dgvInsumos.Enabled = True
                        btnNuevoInsumo.Enabled = True
                        btnInsertarInsumo.Enabled = True
                        btnEliminarInsumo.Enabled = True
                    End If

                    If IsEdit = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                        queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                        If IsBase = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    Else

                        Dim checkIfItsOnlyTextUpdate As Boolean = False

                        checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                        If checkIfItsOnlyTextUpdate = True Then

                            Dim queries(3) As String

                            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            If iinputid <> 0 Then
                                queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                            End If

                            If IsBase = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                            Else
                                If IsModel = True Then
                                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                                Else
                                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                                End If
                            End If

                            executeTransactedSQLCommand(0, queries)

                        Else

                            Dim queries(3) As String

                            iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                            Dim queriesCreation(9) As String

                            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                            executeTransactedSQLCommand(0, queriesCreation)

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                            If IsBase = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                            Else
                                If IsModel = True Then
                                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                                Else
                                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                                End If
                            End If

                            executeTransactedSQLCommand(0, queries)

                        End If

                    End If

                Else

                    txtUnidadDeMedida.Focus()
                    Exit Sub

                End If

            Else


                If validaInsumo(True) = False Then
                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    Exit Sub
                Else
                    dgvInsumos.Enabled = True
                    btnNuevoInsumo.Enabled = True
                    btnInsertarInsumo.Enabled = True
                    btnEliminarInsumo.Enabled = True
                End If

                If IsEdit = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                    queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                    If IsBase = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim checkIfItsOnlyTextUpdate As Boolean = False

                    checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                    If checkIfItsOnlyTextUpdate = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        If iinputid <> 0 Then
                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                        End If

                        If IsBase = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    Else

                        Dim queries(3) As String

                        iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        Dim queriesCreation(9) As String

                        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                        queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                        queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                        queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                        queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                        executeTransactedSQLCommand(0, queriesCreation)

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        If IsBase = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    End If

                End If


            End If


            'Activamos el grid


            dgvInsumos.Enabled = True
            btnNuevoInsumo.Enabled = True
            btnInsertarInsumo.Enabled = True
            btnEliminarInsumo.Enabled = True

        End If

    End Sub


    Private Sub dgvInsumos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvInsumos.UserAddedRow

        If insertInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isFormReadyForAction = False

        Dim bi As New BuscaInsumos
        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname
        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.querystring = dgvInsumos.CurrentCell.EditedFormattedValue

        bi.IsEdit = False

        bi.IsBase = IsBase
        bi.IsModel = IsModel

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim dsBusquedaInsumosRepetidos As DataSet

            If IsBase = True Then

                dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

            Else

                If IsModel = True Then

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

                Else

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

                End If

            End If


            If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                MsgBox("Ya tienes ese Insumo insertado en este Insumo Compuesto. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido")
                dgvInsumos.EndEdit()
                Exit Sub

            End If

            Dim chequeoPrimeraVezQueSeInsertaAlgo As Integer = 0

            If IsBase = True Then 'And baseid <> iprojectid

                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)

                If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then

                    Dim queriesPrimeraVez(2) As String
                    queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                    queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                    executeTransactedSQLCommand(0, queriesPrimeraVez)

                End If

            ElseIf IsModel = True Then

                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices WHERE imodelid = " & iprojectid)

                If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then

                    Dim queriesPrimeraVez(2) As String
                    queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                    queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                    executeTransactedSQLCommand(0, queriesPrimeraVez)

                End If

            ElseIf IsModel = False And IsBase = False Then

                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid)

                If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then

                    Dim queriesPrimeraVez(2) As String
                    queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                    queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                    executeTransactedSQLCommand(0, queriesPrimeraVez)

                End If

            End If


            If IsBase = True Then

                Dim queries(2) As String

                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                executeTransactedSQLCommand(0, queries)

            Else

                If IsModel = True Then

                    Dim queries(2) As String

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim queries(2) As String

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                    executeTransactedSQLCommand(0, queries)

                End If

            End If

            Dim querySumaInsumoCompuesto As String
            Dim sumaInsumoCompuesto As Double

            If IsBase = True Then

                querySumaInsumoCompuesto = "" & _
                "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                "GROUP BY btfci.iinputid "

            Else

                If IsModel = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                    "GROUP BY mtfci.iinputid "

                Else

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                    "GROUP BY ptfci.iinputid "

                End If

            End If

            sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

            txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

            dgvInsumos.EndEdit()

            isFormReadyForAction = True

        Else

            dgvInsumos.EndEdit()

            isFormReadyForAction = True

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevoInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoInsumo.Click

        If newInputPermission = False Then
            Exit Sub
        End If

        'Inicia Save de Master del Insumo Compuesto

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If IsHistoric = True Then
            Exit Sub
        End If

        Dim unitfound As Boolean = False
        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

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

                If validaInsumo(True) = False Then
                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    Exit Sub
                Else
                    dgvInsumos.Enabled = True
                    btnNuevoInsumo.Enabled = True
                    btnInsertarInsumo.Enabled = True
                    btnEliminarInsumo.Enabled = True
                End If

                If IsEdit = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                    queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                    If IsBase = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    If executeTransactedSQLCommand(0, queries) = True Then
                        sinputdescription = txtNombreDelInsumo.Text
                        sinputunit = txtUnidadDeMedida.Text
                    End If

                Else

                    Dim checkIfItsOnlyTextUpdate As Boolean = False

                    checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                    If checkIfItsOnlyTextUpdate = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        If iinputid <> 0 Then
                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                        End If

                        If IsBase = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        If executeTransactedSQLCommand(0, queries) = True Then
                            sinputdescription = txtNombreDelInsumo.Text
                            sinputunit = txtUnidadDeMedida.Text
                        End If

                    Else

                        Dim queries(3) As String

                        iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        Dim queriesCreation(9) As String

                        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                        queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                        queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                        queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                        queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                        executeTransactedSQLCommand(0, queriesCreation)

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        If IsBase = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        If executeTransactedSQLCommand(0, queries) = True Then
                            sinputdescription = txtNombreDelInsumo.Text
                            sinputunit = txtUnidadDeMedida.Text
                        End If

                    End If

                End If

            Else

                txtUnidadDeMedida.Focus()
                Exit Sub

            End If

        Else


            If validaInsumo(True) = False Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub
            Else
                dgvInsumos.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
            End If

            If IsEdit = True Then

                Dim queries(3) As String

                queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                If IsBase = True Then
                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                Else
                    If IsModel = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                    Else
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                    End If
                End If

                If executeTransactedSQLCommand(0, queries) = True Then
                    sinputdescription = txtNombreDelInsumo.Text
                    sinputunit = txtUnidadDeMedida.Text
                End If

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                If checkIfItsOnlyTextUpdate = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                    If iinputid <> 0 Then
                        queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                    End If

                    If IsBase = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    If executeTransactedSQLCommand(0, queries) = True Then
                        sinputdescription = txtNombreDelInsumo.Text
                        sinputunit = txtUnidadDeMedida.Text
                    End If

                Else

                    Dim queries(3) As String

                    iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                    Dim queriesCreation(9) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                    queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                    queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                    queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                    queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                    executeTransactedSQLCommand(0, queriesCreation)

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    If IsBase = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    If executeTransactedSQLCommand(0, queries) = True Then
                        sinputdescription = txtNombreDelInsumo.Text
                        sinputunit = txtUnidadDeMedida.Text
                    End If

                End If

            End If


        End If



        'Inicia código del Botón Nuevo Insumo



        Dim bipni As New BuscaInsumosPreguntaTipoNuevoInsumo
        bipni.susername = susername
        bipni.bactive = bactive
        bipni.bonline = bonline
        bipni.suserfullname = suserfullname

        bipni.suseremail = suseremail
        bipni.susersession = susersession
        bipni.susermachinename = susermachinename
        bipni.suserip = suserip

        bipni.ShowDialog(Me)

        If bipni.DialogResult = Windows.Forms.DialogResult.OK Then

            If bipni.iselectedoption = 1 Then

                'Nuevo Insumo Normal

                Dim ai As New AgregarInsumo
                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname

                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iprojectid = iprojectid
                ai.icardid = icardid

                ai.IsEdit = False
                ai.IsHistoric = False
                ai.IsModel = IsModel
                ai.IsBase = IsBase

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Visible = True

                If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim cantidaddeinsumo As Double = 1

                    If IsBase = True Then

                        Dim queries(2) As String

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & ai.iinputid & ", '" & ai.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & ai.iinputid

                        executeTransactedSQLCommand(0, queries)

                    Else

                        If IsModel = True Then

                            Dim queries(2) As String

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & ai.iinputid & ", '" & ai.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & ai.iinputid & ", '" & ai.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                            executeTransactedSQLCommand(0, queries)

                        Else

                            Dim queries(2) As String

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & ai.iinputid & ", '" & ai.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & ai.iinputid & ", '" & ai.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                            executeTransactedSQLCommand(0, queries)

                        End If

                    End If

                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY btfci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY mtfci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ptfci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                End If

            ElseIf bipni.iselectedoption = 2 Then

                Dim aic As New AgregarInsumoCompuesto
                aic.susername = susername
                aic.bactive = bactive
                aic.bonline = bonline
                aic.suserfullname = suserfullname

                aic.suseremail = suseremail
                aic.susersession = susersession
                aic.susermachinename = susermachinename
                aic.suserip = suserip

                aic.iprojectid = iprojectid
                aic.icardid = icardid

                aic.IsEdit = False
                aic.IsHistoric = False
                aic.IsModel = IsModel
                aic.IsBase = IsBase

                If Me.WindowState = FormWindowState.Maximized Then
                    aic.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aic.ShowDialog(Me)
                Me.Visible = True

                If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim cantidaddeinsumo As Double = 1

                    If IsBase = True Then

                        Dim queries(2) As String

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & aic.iinputid & ", " & aic.sinputunit & ", 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                        executeTransactedSQLCommand(0, queries)

                    Else

                        If IsModel = True Then

                            Dim queries(2) As String

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & aic.iinputid & ", " & aic.sinputunit & ", 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & aic.iinputid & ", " & aic.sinputunit & ", 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                            executeTransactedSQLCommand(0, queries)

                        Else

                            Dim queries(2) As String

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & aic.iinputid & ", " & aic.sinputunit & ", 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & aic.iinputid & ", " & aic.sinputunit & ", 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                            executeTransactedSQLCommand(0, queries)

                        End If

                    End If

                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY btfci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY mtfci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ptfci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                End If

            End If

            Dim queryInsumos As String = ""

            If IsBase = True Then

                queryInsumos = "" & _
                "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
                "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

            Else

                If IsModel = True Then

                    queryInsumos = "" & _
                    "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

                Else

                    queryInsumos = "" & _
                    "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

                End If

            End If

            setDataGridView(dgvInsumos, queryInsumos, False)

            dgvInsumos.Columns(0).Visible = False

            dgvInsumos.Columns(0).ReadOnly = True
            dgvInsumos.Columns(2).ReadOnly = True
            dgvInsumos.Columns(3).ReadOnly = True
            dgvInsumos.Columns(5).ReadOnly = True

            isFormReadyForAction = True

        End If

    End Sub


    Private Sub btnInsertarInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarInsumo.Click


        If insertInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'Inicia Save de Master del Insumo Compuesto

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If IsHistoric = True Then
            Exit Sub
        End If

        Dim unitfound As Boolean = False
        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

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

                If validaInsumo(True) = False Then
                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    Exit Sub
                Else
                    dgvInsumos.Enabled = True
                    btnNuevoInsumo.Enabled = True
                    btnInsertarInsumo.Enabled = True
                    btnEliminarInsumo.Enabled = True
                End If

                If IsEdit = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                    queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                    If IsBase = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    If executeTransactedSQLCommand(0, queries) = True Then
                        sinputdescription = txtNombreDelInsumo.Text
                        sinputunit = txtUnidadDeMedida.Text
                    End If

                Else

                    Dim checkIfItsOnlyTextUpdate As Boolean = False

                    checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                    If checkIfItsOnlyTextUpdate = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        If iinputid <> 0 Then
                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                        End If

                        If IsBase = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        If executeTransactedSQLCommand(0, queries) = True Then
                            sinputdescription = txtNombreDelInsumo.Text
                            sinputunit = txtUnidadDeMedida.Text
                        End If

                    Else

                        Dim queries(3) As String

                        iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        Dim queriesCreation(9) As String

                        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                        queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                        queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                        queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                        queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                        executeTransactedSQLCommand(0, queriesCreation)

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        If IsBase = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        If executeTransactedSQLCommand(0, queries) = True Then
                            sinputdescription = txtNombreDelInsumo.Text
                            sinputunit = txtUnidadDeMedida.Text
                        End If

                    End If

                End If

            Else

                txtUnidadDeMedida.Focus()
                Exit Sub

            End If

        Else


            If validaInsumo(True) = False Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub
            Else
                dgvInsumos.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
            End If

            If IsEdit = True Then

                Dim queries(3) As String

                queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                If IsBase = True Then
                    queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                Else
                    If IsModel = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                    Else
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                    End If
                End If

                If executeTransactedSQLCommand(0, queries) = True Then
                    sinputdescription = txtNombreDelInsumo.Text
                    sinputunit = txtUnidadDeMedida.Text
                End If

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                If checkIfItsOnlyTextUpdate = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                    If iinputid <> 0 Then
                        queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                    End If

                    If IsBase = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    If executeTransactedSQLCommand(0, queries) = True Then
                        sinputdescription = txtNombreDelInsumo.Text
                        sinputunit = txtUnidadDeMedida.Text
                    End If

                Else

                    Dim queries(3) As String

                    iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                    Dim queriesCreation(9) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                    queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                    queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                    queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                    queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                    executeTransactedSQLCommand(0, queriesCreation)

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    If IsBase = True Then
                        queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    If executeTransactedSQLCommand(0, queries) = True Then
                        sinputdescription = txtNombreDelInsumo.Text
                        sinputunit = txtUnidadDeMedida.Text
                    End If

                End If

            End If


        End If


        'Inicia código del Botón Insertar Insumo


        isFormReadyForAction = False

        Dim chequeoPrimeraVezQueSeInsertaAlgo As Integer = 0


        If IsBase = True Then 'And baseid <> iprojectid

            chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            End If

        ElseIf IsModel = True Then

            chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices WHERE imodelid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            End If

        ElseIf IsModel = False And IsBase = False Then

            chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            End If

        End If

        Dim bi As New BuscaInsumos
        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname

        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.IsEdit = IsEdit
        bi.IsBase = IsBase
        bi.IsModel = IsModel
        bi.IsHistoric = IsHistoric

        bi.iprojectid = iprojectid
        bi.icardid = icardid

        bi.IsEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim dsBusquedaInsumosRepetidos As DataSet

            If IsBase = True Then

                dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

            Else

                If IsModel = True Then

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

                Else

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & bi.iinputid)

                End If

            End If


            If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                MsgBox("Ya tienes ese Insumo insertado en este Insumo Compuesto. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido")
                isFormReadyForAction = True
                Exit Sub

            End If

            Dim cantidaddeinsumo As Double = 1

            If IsBase = True Then

                Dim queries(2) As String

                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                executeTransactedSQLCommand(0, queries)

            Else

                If IsModel = True Then

                    Dim queries(2) As String

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim queries(2) As String

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & iinputid & ", " & bi.iinputid & ", '" & bi.sinputunit & "', 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                    executeTransactedSQLCommand(0, queries)

                End If

            End If

            Dim querySumaInsumoCompuesto As String
            Dim sumaInsumoCompuesto As Double

            If IsBase = True Then

                querySumaInsumoCompuesto = "" & _
                "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                "GROUP BY btfci.iinputid "

            Else

                If IsModel = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                    "GROUP BY mtfci.iinputid "

                Else

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                    "GROUP BY ptfci.iinputid "

                End If

            End If

            sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

            txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

        End If

        Dim queryInsumos As String = ""

        If IsBase = True Then

            queryInsumos = "" & _
            "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
            "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
            "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

        Else

            If IsModel = True Then

                queryInsumos = "" & _
                "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

            Else

                queryInsumos = "" & _
                "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

            End If

        End If

        setDataGridView(dgvInsumos, queryInsumos, False)

        dgvInsumos.Columns(0).Visible = False

        dgvInsumos.Columns(0).ReadOnly = True
        dgvInsumos.Columns(2).ReadOnly = True
        dgvInsumos.Columns(3).ReadOnly = True
        dgvInsumos.Columns(5).ReadOnly = True

        isFormReadyForAction = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarInsumo.Click

        If deleteInputPermission = False Then
            Exit Sub
        End If

        If MsgBox("¿Está seguro que deseas eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmar Eliminación de Insumo de Insumo Compuesto") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedinputid As Integer = 0
            Try
                tmpselectedinputid = CInt(dgvInsumos.CurrentRow.Cells(0).Value)
            Catch ex As Exception

            End Try


            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            If IsBase = True Then

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & tmpselectedinputid)

            Else

                If IsModel = True Then

                    Dim queriesDelete(2) As String

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                    queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & tmpselectedinputid

                    executeTransactedSQLCommand(0, queriesDelete)

                Else

                    Dim queriesDelete(2) As String

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & iselectedinputid
                    queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iinputid & " AND icompoundinputid = " & tmpselectedinputid

                    executeTransactedSQLCommand(0, queriesDelete)

                End If

            End If


            Dim querySumaInsumoCompuesto As String
            Dim sumaInsumoCompuesto As Double

            If IsBase = True Then

                querySumaInsumoCompuesto = "" & _
                "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                "GROUP BY btfci.iinputid "

            Else

                If IsModel = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                    "GROUP BY mtfci.iinputid "

                Else

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                    "GROUP BY ptfci.iinputid "

                End If

            End If

            sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

            txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)


            Dim queryInsumos As String = ""

            If IsBase = True Then

                queryInsumos = "" & _
                "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
                "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

            Else

                If IsModel = True Then

                    queryInsumos = "" & _
                    "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

                Else

                    queryInsumos = "" & _
                    "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

                End If

            End If

            setDataGridView(dgvInsumos, queryInsumos, False)

            dgvInsumos.Columns(0).Visible = False

            dgvInsumos.Columns(0).ReadOnly = True
            dgvInsumos.Columns(2).ReadOnly = True
            dgvInsumos.Columns(3).ReadOnly = True
            dgvInsumos.Columns(5).ReadOnly = True

            isFormReadyForAction = True

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaInsumosCompuestos(False) = False Then
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

            Dim queriesNewId(11) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Types"
            'queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Categories RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Categories"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & " SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Types SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
            'queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid + newIdAddition & "Categories SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

            If IsBase = True Then

                queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                queriesNewId(8) = "UPDATE oversight.modelcardcompoundinputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

            Else

                If IsModel = True Then

                    queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(10) = "UPDATE oversight.modelcardcompoundinputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

                Else

                    queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid
                    queriesNewId(10) = "UPDATE oversight.projectcardcompoundinputs SET iinputid = " & iinputid + newIdAddition & " WHERE iinputid = " & iinputid

                End If

            End If

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iinputid = iinputid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        If IsEdit = False Then

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            If IsBase = True Then

                Dim queriesInsert(2) As String
                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs VALUES (" & baseid & ", " & icardid & ", " & iinputid & ", '" & txtUnidadDeMedida.Text.Trim.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')"
                queriesInsert(1) = ""

                executeTransactedSQLCommand(0, queriesInsert)

            ElseIf IsModel = True Then

                Dim queriesInsert(2) As String

                queriesInsert(0) = "INSERT INTO basecardinputs VALUES (" & baseid & ", " & icardid & ", " & iinputid & ", '" & txtUnidadDeMedida.Text.Trim.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')"
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs VALUES (" & iprojectid & ", " & icardid & ", " & iinputid & ", '" & txtUnidadDeMedida.Text.Trim.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')"

                executeTransactedSQLCommand(0, queriesInsert)

            Else

                Dim queriesInsert(2) As String

                queriesInsert(0) = "INSERT INTO basecardinputs VALUES (" & baseid & ", " & icardid & ", " & iinputid & ", '" & txtUnidadDeMedida.Text.Trim.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')"
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs VALUES (" & iprojectid & ", " & icardid & ", " & iinputid & ", '" & txtUnidadDeMedida.Text.Trim.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')"

                executeTransactedSQLCommand(0, queriesInsert)

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

            queriesSave(3) = "" & _
            "UPDATE inputs i JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti ON ti.iinputid = i.iinputid SET i.iupdatedate = ti.iupdatedate, i.supdatetime = ti.supdatetime, i.supdateusername = ti.supdateusername, i.sinputdescription = ti.sinputdescription, i.sinputunit = ti.sinputunit WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE inputtypes it JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit ON tit.iinputid = it.iinputid SET it.iupdatedate = tit.iupdatedate, it.supdatetime = tit.supdatetime, it.supdateusername = tit.supdateusername, it.sinputtypedescription = tit.sinputtypedescription WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO inputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
            "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO inputtypes " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
            "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") "

            queriesSave(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " : " & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

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

            If IsEdit = True Then

                If IsBase = True Then

                    If aipru.iselectedoption = 2 Then    'Base y Modelos

                        fecha = getMySQLDate()
                        hora = getAppTime()

                        Dim queriesUpdate(5) As String
                        Dim queriesInsert2(dgvInsumos.Rows.Count - 1) As String

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                        If IsBase = True Then
                            queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        Dim baseid As Integer = 0
                        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        If baseid = 0 Then
                            baseid = 99999
                        End If

                        'queriesUpdate(4) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                        'Try

                        '    For i = 0 To dgvInsumos.Rows.Count - 2

                        '        queriesInsert2(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs VALUES (" & baseid & ", " & icardid & ", " & iinputid & ", " & dgvInsumos.Rows(i).Cells(0).Value & ", '" & dgvInsumos.Rows(i).Cells(2).Value & "', " & dgvInsumos.Rows(i).Cells(4).Value & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        '    Next i

                        'Catch ex As Exception

                        'End Try

                        Dim dsModelos As DataSet
                        dsModelos = getSQLQueryAsDataset(0, "SELECT imodelid FROM models")

                        Dim queriesModelos(dsModelos.Tables(0).Rows.Count) As String
                        Dim queriesModelos2(dgvInsumos.Rows.Count - 1) As String

                        Try

                            For i = 0 To dsModelos.Tables(0).Rows.Count - 1

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM modelcardcompoundinputs WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND icardid = " & icardid) > 0 Then

                                    queriesModelos(i) = "DELETE FROM modelcardcompoundinputs WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND icardid = " & icardid

                                    For k = 0 To dgvInsumos.Rows.Count - 2

                                        queriesModelos2(k) = "INSERT INTO modelcardcompoundinputs VALUES (" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & icardid & ", " & iinputid & ", " & dgvInsumos.Rows(k).Cells(0).Value & ", '" & dgvInsumos.Rows(k).Cells(2).Value & "', " & dgvInsumos.Rows(k).Cells(4).Value & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                    Next k

                                End If

                            Next i

                        Catch ex As Exception

                        End Try

                        Array.Resize(queriesUpdate, 5 + dgvInsumos.Rows.Count + dsModelos.Tables(0).Rows.Count + dgvInsumos.Rows.Count)

                        Array.ConstrainedCopy(queriesInsert2, 0, queriesUpdate, 5, queriesInsert2.Length - 1)
                        Array.ConstrainedCopy(queriesModelos, 0, queriesUpdate, 5 + dgvInsumos.Rows.Count, queriesModelos.Length - 1)
                        Array.ConstrainedCopy(queriesModelos2, 0, queriesUpdate, 5 + dgvInsumos.Rows.Count + dsModelos.Tables(0).Rows.Count, queriesModelos2.Length - 1)

                        executeTransactedSQLCommand(0, queriesUpdate)

                    End If

                Else

                    If IsModel = True Then

                        If aipru.iselectedoption = 2 Then  'Selecciona Actualizar Precio En General por lo que actualizamos todo

                            fecha = getMySQLDate()
                            hora = getAppTime()

                            Dim queriesUpdate(5) As String
                            Dim queriesInsert2(dgvInsumos.Rows.Count - 1) As String
                            Dim queriesInsert3(dgvInsumos.Rows.Count - 1) As String

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                            If IsBase = True Then
                                queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                            Else
                                If IsModel = True Then
                                    queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                                Else
                                    queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                                End If
                            End If

                            Dim baseid As Integer = 0
                            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                            If baseid = 0 Then
                                baseid = 99999
                            End If

                            queriesUpdate(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                            Try

                                For i = 0 To dgvInsumos.Rows.Count - 2

                                    queriesInsert2(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs VALUES (" & baseid & ", " & icardid & ", " & iinputid & ", " & dgvInsumos.Rows(i).Cells(0).Value & ", '" & dgvInsumos.Rows(i).Cells(2).Value & "', " & dgvInsumos.Rows(i).Cells(4).Value & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                Next i

                            Catch ex As Exception

                            End Try

                            Dim dsModelos As DataSet
                            dsModelos = getSQLQueryAsDataset(0, "SELECT imodelid FROM models")

                            Dim queriesDelete(dsModelos.Tables(0).Rows.Count) As String


                            Try

                                For i = 0 To dsModelos.Tables(0).Rows.Count - 1

                                    If dsModelos.Tables(0).Rows(i).Item(0) = iprojectid Then

                                        queriesDelete(i) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND icardid = " & icardid

                                        For k = 0 To dgvInsumos.Rows.Count - 2

                                            queriesInsert3(k) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs VALUES (" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & icardid & ", " & iinputid & ", " & dgvInsumos.Rows(k).Cells(0).Value & ", '" & dgvInsumos.Rows(k).Cells(2).Value & "', " & dgvInsumos.Rows(k).Cells(4).Value & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                        Next k

                                    Else

                                        If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM modelcardcompoundinputs WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND icardid = " & icardid) > 0 Then

                                            queriesDelete(i) = "DELETE FROM modelcardcompoundinputs WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND icardid = " & icardid

                                            For k = 0 To dgvInsumos.Rows.Count - 2

                                                queriesInsert3(k) = "INSERT INTO modelcardcompoundinputs VALUES (" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & icardid & ", " & iinputid & ", " & dgvInsumos.Rows(k).Cells(0).Value & ", '" & dgvInsumos.Rows(k).Cells(2).Value & "', " & dgvInsumos.Rows(k).Cells(4).Value & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                            Next k

                                        End If

                                    End If

                                Next i

                            Catch ex As Exception

                            End Try

                            Array.Resize(queriesUpdate, 5 + dgvInsumos.Rows.Count + dsModelos.Tables(0).Rows.Count + dgvInsumos.Rows.Count)

                            Array.ConstrainedCopy(queriesInsert2, 0, queriesUpdate, 5, queriesInsert2.Length - 1)
                            Array.ConstrainedCopy(queriesDelete, 0, queriesUpdate, 5 + dgvInsumos.Rows.Count, queriesDelete.Length - 1)
                            Array.ConstrainedCopy(queriesInsert3, 0, queriesUpdate, 5 + dgvInsumos.Rows.Count + dsModelos.Tables(0).Rows.Count, queriesInsert3.Length - 1)

                            executeTransactedSQLCommand(0, queriesUpdate)


                        ElseIf aipru.iselectedoption = 3 Then    'Solo este modelo

                            fecha = getMySQLDate()
                            hora = getAppTime()

                            Dim queriesUpdate(4) As String

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                            If IsBase = True Then
                                queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                            Else
                                If IsModel = True Then
                                    queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                                Else
                                    queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                                End If
                            End If

                            executeTransactedSQLCommand(0, queriesUpdate)

                        End If

                    Else

                        If aipru.iselectedoption = 2 Then  'Selecciona Actualizar Precio En General por lo que actualizamos todo

                            fecha = getMySQLDate()
                            hora = getAppTime()

                            Dim queriesUpdate(5) As String

                            Dim queriesInsert2(dgvInsumos.Rows.Count - 1) As String
                            Dim queriesInsert3(dgvInsumos.Rows.Count - 1) As String
                            Dim queriesInsert4(dgvInsumos.Rows.Count - 1) As String

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                            If IsBase = True Then
                                queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                            Else
                                If IsModel = True Then
                                    queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                                Else
                                    queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                                End If
                            End If

                            Dim baseid As Integer = 0
                            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                            If baseid = 0 Then
                                baseid = 99999
                            End If

                            queriesUpdate(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                            Try

                                For i = 0 To dgvInsumos.Rows.Count - 2

                                    queriesInsert2(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs VALUES (" & baseid & ", " & icardid & ", " & iinputid & ", " & dgvInsumos.Rows(i).Cells(0).Value & ", '" & dgvInsumos.Rows(i).Cells(2).Value & "', " & dgvInsumos.Rows(i).Cells(4).Value & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                Next i

                            Catch ex As Exception

                            End Try

                            Dim dsModelos As DataSet
                            dsModelos = getSQLQueryAsDataset(0, "SELECT imodelid FROM models")

                            Dim queriesDelete(dsModelos.Tables(0).Rows.Count) As String


                            Try

                                For i = 0 To dsModelos.Tables(0).Rows.Count - 1

                                    If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM modelcardcompoundinputs WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND icardid = " & icardid) > 0 Then

                                        queriesDelete(i) = "DELETE FROM modelcardcompoundinputs WHERE imodelid = " & dsModelos.Tables(0).Rows(i).Item(0) & " AND icardid = " & icardid

                                        For k = 0 To dgvInsumos.Rows.Count - 2

                                            queriesInsert3(k) = "INSERT INTO modelcardcompoundinputs VALUES (" & dsModelos.Tables(0).Rows(i).Item(0) & ", " & icardid & ", " & iinputid & ", " & dgvInsumos.Rows(k).Cells(0).Value & ", '" & dgvInsumos.Rows(k).Cells(2).Value & "', " & dgvInsumos.Rows(k).Cells(4).Value & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                                        Next k

                                    End If

                                Next i

                            Catch ex As Exception

                            End Try

                            'queriesUpdate(4) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                            'Try

                            '    For k = 0 To dgvInsumos.Rows.Count - 2

                            '        queriesInsert4(k) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs VALUES (" & iprojectid & ", " & icardid & ", " & iinputid & ", " & dgvInsumos.Rows(k).Cells(0).Value & ", '" & dgvInsumos.Rows(k).Cells(2).Value & "', " & dgvInsumos.Rows(k).Cells(4).Value & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                            '    Next k

                            'Catch ex As Exception

                            'End Try

                            Array.Resize(queriesUpdate, 5 + dgvInsumos.Rows.Count + dgvInsumos.Rows.Count + dgvInsumos.Rows.Count + dgvInsumos.Rows.Count + dsModelos.Tables(0).Rows.Count)

                            Array.ConstrainedCopy(queriesInsert2, 0, queriesUpdate, 5, queriesInsert2.Length - 1)
                            Array.ConstrainedCopy(queriesDelete, 0, queriesUpdate, 5 + dgvInsumos.Rows.Count, queriesDelete.Length - 1)
                            Array.ConstrainedCopy(queriesInsert3, 0, queriesUpdate, 5 + dgvInsumos.Rows.Count + dsModelos.Tables(0).Rows.Count, queriesInsert3.Length - 1)
                            Array.ConstrainedCopy(queriesInsert4, 0, queriesUpdate, 5 + dgvInsumos.Rows.Count + dsModelos.Tables(0).Rows.Count + dgvInsumos.Rows.Count, queriesInsert4.Length - 1)

                            executeTransactedSQLCommand(0, queriesUpdate)


                        ElseIf aipru.iselectedoption = 1 Then    'Solo este proyecto

                            fecha = getMySQLDate()
                            hora = getAppTime()

                            Dim queriesUpdate(4) As String

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                            If IsBase = True Then
                                queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                            Else
                                If IsModel = True Then
                                    queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                                Else
                                    queriesUpdate(2) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                                End If
                            End If

                            executeTransactedSQLCommand(0, queriesUpdate)

                        End If


                    End If

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

            queriesSave(3) = "" & _
            "UPDATE inputs i JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti ON ti.iinputid = i.iinputid SET i.iupdatedate = ti.iupdatedate, i.supdatetime = ti.supdatetime, i.supdateusername = ti.supdateusername, i.sinputdescription = ti.sinputdescription, i.sinputunit = ti.sinputunit WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE inputtypes it JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit ON tit.iinputid = it.iinputid SET it.iupdatedate = tit.iupdatedate, it.supdatetime = tit.supdatetime, it.supdateusername = tit.supdateusername, it.sinputtypedescription = tit.sinputtypedescription WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO inputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
            "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO inputtypes " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
            "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") "

            queriesSave(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " : " & txtNombreDelInsumo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

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

    End Sub


    Private Sub dgvInsumos_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellContentDoubleClick

        If openInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvInsumos.CurrentRow.IsNewRow Then

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            iselectedinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(3).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

        If iselectedinputid = 0 Then
            Exit Sub
        End If

        Dim conteoCompound As Integer = 0

        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

        If conteoCompound > 0 Then

            'Es un material compuesto
            Dim aic As New AgregarInsumoCompuesto

            aic.susername = susername
            aic.bactive = bactive
            aic.bonline = bonline
            aic.suserfullname = suserfullname

            aic.suseremail = suseremail
            aic.susersession = susersession
            aic.susermachinename = susermachinename
            aic.suserip = suserip

            aic.iprojectid = iprojectid
            aic.icardid = icardid
            aic.iinputid = iselectedinputid

            aic.IsHistoric = IsHistoric

            aic.IsBase = IsBase
            aic.IsModel = IsModel

            aic.IsEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                aic.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            aic.ShowDialog(Me)
            Me.Visible = True

            If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                isFormReadyForAction = False

                Dim querySumaInsumoCompuesto As String = ""
                Dim sumaInsumoCompuesto As Double

                If IsBase = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                    "GROUP BY btfci.iinputid "

                Else

                    If IsModel = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                        "GROUP BY mtfci.iinputid "

                    Else

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                        "GROUP BY ptfci.iinputid "

                    End If

                End If


                sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                Dim queryInsumos As String = ""

                If IsBase = True Then

                    queryInsumos = "" & _
                    "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

                Else

                    If IsModel = True Then

                        queryInsumos = "" & _
                        "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

                    Else

                        queryInsumos = "" & _
                        "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

                    End If

                End If


                setDataGridView(dgvInsumos, queryInsumos, False)

                dgvInsumos.Columns(0).Visible = False

                dgvInsumos.Columns(0).ReadOnly = True
                dgvInsumos.Columns(2).ReadOnly = True
                dgvInsumos.Columns(3).ReadOnly = True
                dgvInsumos.Columns(5).ReadOnly = True

                isFormReadyForAction = True

            End If

        Else

            'NO Es un material compuesto
            Dim ai As New AgregarInsumo

            ai.susername = susername
            ai.bactive = bactive
            ai.bonline = bonline
            ai.suserfullname = suserfullname

            ai.suseremail = suseremail
            ai.susersession = susersession
            ai.susermachinename = susermachinename
            ai.suserip = suserip

            ai.iprojectid = iprojectid
            ai.icardid = icardid
            ai.iinputid = iselectedinputid

            ai.IsHistoric = IsHistoric

            ai.IsBase = IsBase
            ai.IsModel = IsModel

            ai.IsEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ai.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ai.ShowDialog(Me)
            Me.Visible = True

            If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                isFormReadyForAction = False

                Dim querySumaInsumoCompuesto As String = ""
                Dim sumaInsumoCompuesto As Double

                If IsBase = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                    "GROUP BY btfci.iinputid "

                Else

                    If IsModel = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                        "GROUP BY mtfci.iinputid "

                    Else

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                        "GROUP BY ptfci.iinputid "

                    End If

                End If


                sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                Dim queryInsumos As String = ""

                If IsBase = True Then

                    queryInsumos = "" & _
                    "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

                Else

                    If IsModel = True Then

                        queryInsumos = "" & _
                        "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

                    Else

                        queryInsumos = "" & _
                        "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

                    End If

                End If


                setDataGridView(dgvInsumos, queryInsumos, False)

                dgvInsumos.Columns(0).Visible = False

                dgvInsumos.Columns(0).ReadOnly = True
                dgvInsumos.Columns(2).ReadOnly = True
                dgvInsumos.Columns(3).ReadOnly = True
                dgvInsumos.Columns(5).ReadOnly = True

                isFormReadyForAction = True

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellDoubleClick

        If openInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvInsumos.CurrentRow.IsNewRow Then

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            iselectedinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(3).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

        If iselectedinputid = 0 Then
            Exit Sub
        End If

        Dim conteoCompound As Integer = 0

        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

        If conteoCompound > 0 Then

            'Es un material compuesto
            Dim aic As New AgregarInsumoCompuesto

            aic.susername = susername
            aic.bactive = bactive
            aic.bonline = bonline
            aic.suserfullname = suserfullname

            aic.suseremail = suseremail
            aic.susersession = susersession
            aic.susermachinename = susermachinename
            aic.suserip = suserip

            aic.iprojectid = iprojectid
            aic.icardid = icardid
            aic.iinputid = iselectedinputid

            aic.IsHistoric = IsHistoric

            aic.IsBase = IsBase
            aic.IsModel = IsModel

            aic.IsEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                aic.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            aic.ShowDialog(Me)
            Me.Visible = True

            If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                isFormReadyForAction = False

                Dim querySumaInsumoCompuesto As String = ""
                Dim sumaInsumoCompuesto As Double

                If IsBase = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                    "GROUP BY btfci.iinputid "

                Else

                    If IsModel = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                        "GROUP BY mtfci.iinputid "

                    Else

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                        "GROUP BY ptfci.iinputid "

                    End If

                End If


                sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                Dim queryInsumos As String = ""

                If IsBase = True Then

                    queryInsumos = "" & _
                    "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

                Else

                    If IsModel = True Then

                        queryInsumos = "" & _
                        "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

                    Else

                        queryInsumos = "" & _
                        "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

                    End If

                End If


                setDataGridView(dgvInsumos, queryInsumos, False)

                dgvInsumos.Columns(0).Visible = False

                dgvInsumos.Columns(0).ReadOnly = True
                dgvInsumos.Columns(2).ReadOnly = True
                dgvInsumos.Columns(3).ReadOnly = True
                dgvInsumos.Columns(5).ReadOnly = True

                isFormReadyForAction = True

            End If

        Else

            'NO Es un material compuesto
            Dim ai As New AgregarInsumo

            ai.susername = susername
            ai.bactive = bactive
            ai.bonline = bonline
            ai.suserfullname = suserfullname

            ai.suseremail = suseremail
            ai.susersession = susersession
            ai.susermachinename = susermachinename
            ai.suserip = suserip

            ai.iprojectid = iprojectid
            ai.icardid = icardid
            ai.iinputid = iselectedinputid

            ai.IsHistoric = IsHistoric

            ai.IsBase = IsBase
            ai.IsModel = IsModel

            ai.IsEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ai.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ai.ShowDialog(Me)
            Me.Visible = True

            If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                isFormReadyForAction = False

                Dim querySumaInsumoCompuesto As String = ""
                Dim sumaInsumoCompuesto As Double

                If IsBase = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                    "GROUP BY btfci.iinputid "

                Else

                    If IsModel = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                        "GROUP BY mtfci.iinputid "

                    Else

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                        "GROUP BY ptfci.iinputid "

                    End If

                End If


                sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                Dim queryInsumos As String = ""

                If IsBase = True Then

                    queryInsumos = "" & _
                    "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(bp.dinputfinalprice, 2) AS 'Precio', " & _
                    "FORMAT(btfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 2) AS 'Importe' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND btfci.iinputid = " & iinputid

                Else

                    If IsModel = True Then

                        queryInsumos = "" & _
                        "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(mp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(mtfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND mtfci.iinputid = " & iinputid

                    Else

                        queryInsumos = "" & _
                        "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', i.sinputunit AS 'Unidad', FORMAT(pp.dinputfinalprice, 2) AS 'Precio', " & _
                        "FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 2) AS 'Importe' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ptfci.iinputid = " & iinputid

                    End If

                End If


                setDataGridView(dgvInsumos, queryInsumos, False)

                dgvInsumos.Columns(0).Visible = False

                dgvInsumos.Columns(0).ReadOnly = True
                dgvInsumos.Columns(2).ReadOnly = True
                dgvInsumos.Columns(3).ReadOnly = True
                dgvInsumos.Columns(5).ReadOnly = True

                isFormReadyForAction = True

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class