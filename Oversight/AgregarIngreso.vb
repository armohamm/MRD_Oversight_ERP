Public Class AgregarIngreso

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

    Public iincomeid As Integer = 0
    Public sincomedescription As String = ""
    Public dincomeamount As Double = 0.0

    Private iselectedinputwithrelationid As Integer = 0
    Private iselectedrelationtypeid As Integer = 0
    Private iselectedrelationid As Integer = 0

    Public ireceiverid As Integer = 0

    Private isFormReadyForAction As Boolean = False

    Private openPermission As Boolean = False
    Private addRelationPermission As Boolean = False
    Private modifyRelationPermission As Boolean = False
    Private deleteRelationPermission As Boolean = False

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
                    btnGuardarRelacion.Enabled = True
                End If

                If permission = "Abrir Tipos de Ingreso" Then
                    btnTipoIngreso.Enabled = True
                End If

                If permission = "Abrir Bancos" Then
                    btnBancoOrigen.Enabled = True
                End If

                If permission = "Abrir Cuentas" Then
                    btnCuentaDestino.Enabled = True
                End If

                If permission = "Abrir Personas" Then
                    btnPersonas.Enabled = True
                End If

                If permission = "Ver Relaciones" Then
                    dgvConRelacion.Visible = True
                End If

                If permission = "Agregar Relacion" Then
                    addRelationPermission = True
                    btnBuscarRelacion.Enabled = True
                End If

                If permission = "Modificar Relacion" Then
                    modifyRelationPermission = True
                    dgvConRelacion.ReadOnly = False
                    dgvConRelacion.Enabled = True
                End If

                If permission = "Eliminar Relacion" Then
                    deleteRelationPermission = True
                    btnEliminarRelacion.Enabled = True
                End If

                If permission = "Ver Revisiones" Then
                    'btnRevisiones.Enabled = True
                    'btnRevisionesPagos.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Ingreso', 'OK')")

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


    Private Sub AgregarIngreso_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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
        "FROM incomes " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc WHERE incomes.iincomeid = tclc.iincomeid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc JOIN incomes clc ON tclc.iincomeid = clc.iincomeid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomes clc WHERE tclc.iincomeid = clc.iincomeid AND clc.iincomeid = " & iincomeid & ") ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM incomeassets " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc WHERE incomeassets.iincomeid = tclc.iincomeid AND incomeassets.iassetid = tclc.iassetid) ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc JOIN incomeassets clc ON tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomeassets clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid AND clc.iincomeid = " & iincomeid & ") ")

        conteo7 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM incomeprojects " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc WHERE incomeprojects.iincomeid = tclc.iincomeid AND incomeprojects.iprojectid = tclc.iprojectid) ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc JOIN incomeprojects clc ON tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo9 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomeprojects clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid AND clc.iincomeid = " & iincomeid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 + conteo7 + conteo8 + conteo9 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaIngreso(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Ingreso está incompleto. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesIncomeIsOpen As Integer = 1

            timesIncomeIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid & "'")

            If timesIncomeIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Ingreso. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Ingreso?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesIncomeIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(6) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects"
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & " SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
                queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
                queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    iincomeid = iincomeid + newIdAddition
                End If

            End If


            Dim queries(10) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM incomes " & _
            "WHERE iincomeid = " & iincomeid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc WHERE incomes.iincomeid = tclc.iincomeid) "

            queries(1) = "" & _
            "UPDATE incomes clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc ON tclc.iincomeid = clc.iincomeid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.iincomedate = tclc.iincomedate, clc.sincometime = tclc.sincometime, clc.iincometypeid = tclc.iincometypeid, clc.soriginaccount = tclc.soriginaccount, clc.soriginreference = tclc.soriginreference, clc.ioriginbankid = tclc.ioriginbankid, clc.idestinationaccountid = tclc.idestinationaccountid, clc.sincomedescription = tclc.sincomedescription, clc.dincomeamount = tclc.dincomeamount, clc.ireceiverid = tclc.ireceiverid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(2) = "" & _
            "INSERT INTO incomes " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM incomes clc WHERE tclc.iincomeid = clc.iincomeid AND clc.iincomeid = " & iincomeid & ") "

            queries(3) = "" & _
            "DELETE " & _
            "FROM incomeassets " & _
            "WHERE iincomeid = " & iincomeid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc WHERE incomeassets.iincomeid = tclc.iincomeid AND incomeassets.iassetid = tclc.iassetid) "

            queries(4) = "" & _
            "UPDATE incomeassets clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc ON tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sincomeextranote = tclc.sincomeextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(5) = "" & _
            "INSERT INTO incomeassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM incomeassets clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid AND clc.iincomeid = " & iincomeid & ") "

            queries(6) = "" & _
            "DELETE " & _
            "FROM incomeprojects " & _
            "WHERE iincomeid = " & iincomeid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc WHERE incomeprojects.iincomeid = tclc.iincomeid AND incomeprojects.iprojectid = tclc.iprojectid) "

            queries(7) = "" & _
            "UPDATE incomeprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc ON tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sincomeextranote = tclc.sincomeextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "INSERT INTO incomeprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM incomeprojects clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid AND clc.iincomeid = " & iincomeid & ") "

            queries(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Ingreso " & iincomeid & " : " & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

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



        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queriesDelete(5) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects"
        queriesDelete(3) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Ingreso " & iincomeid & " : " & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', 'OK')"
        queriesDelete(4) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Income', 'Ingreso', '" & iincomeid & "', '" & txtDescripcionIngreso.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarIngreso_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarIngreso_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesIncomeIsOpen As Integer = 0

        timesIncomeIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid & "'")

        If timesIncomeIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Ingreso. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Ingreso?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        If isRecover = False Then

            Dim queriesCreation(6) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ( `iincomeid` int(11) NOT NULL AUTO_INCREMENT, `iincomedate` int(11) NOT NULL, `sincometime` varchar(11) CHARACTER SET latin1 NOT NULL, `iincometypeid` int(11) NOT NULL, `ioriginbankid` int(11) NOT NULL, `soriginaccount` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `soriginreference` varchar(100) CHARACTER SET latin1 DEFAULT NULL, `idestinationaccountid` int(11) NOT NULL, `sincomedescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dincomeamount` decimal(65,4) NOT NULL, `ireceiverid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets ( `iincomeid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iassetid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects ( `iincomeid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iprojectid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        cmbTipoIngreso.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM incometypes")
        cmbTipoIngreso.DisplayMember = "sincometypedescription"
        cmbTipoIngreso.ValueMember = "iincometypeid"
        cmbTipoIngreso.SelectedIndex = -1

        cmbCuentaDestino.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM companyaccounts")
        cmbCuentaDestino.DisplayMember = "scompanyaccountname"
        cmbCuentaDestino.ValueMember = "iaccountid"
        cmbCuentaDestino.SelectedIndex = -1

        cmbBancoOrigen.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM banks")
        cmbBancoOrigen.DisplayMember = "sbankname"
        cmbBancoOrigen.ValueMember = "ibankid"
        cmbBancoOrigen.SelectedIndex = -1

        If isEdit = False Then

            txtReferenciaOrigen.Text = ""
            txtCuentaOrigen.Text = ""
            txtDescripcionIngreso.Text = ""
            txtImporte.Text = "0.00"

        Else

            If isRecover = False Then

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " SELECT * FROM incomes WHERE iincomeid = " & iincomeid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets SELECT * FROM incomeassets WHERE iincomeid = " & iincomeid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects SELECT * FROM incomeprojects WHERE iincomeid = " & iincomeid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsIngreso As DataSet
            dsIngreso = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " WHERE iincomeid = " & iincomeid)

            Try

                If dsIngreso.Tables(0).Rows.Count > 0 Then

                    dtFechaIngreso.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsIngreso.Tables(0).Rows(0).Item("iincomedate")) & " " & dsIngreso.Tables(0).Rows(0).Item("sincometime")
                    cmbTipoIngreso.SelectedValue = dsIngreso.Tables(0).Rows(0).Item("iincometypeid")
                    If cmbTipoIngreso.SelectedValue > 1 Then
                        txtReferenciaOrigen.Text = dsIngreso.Tables(0).Rows(0).Item("soriginreference")
                        cmbBancoOrigen.SelectedValue = dsIngreso.Tables(0).Rows(0).Item("ioriginbank")
                        txtCuentaOrigen.Text = dsIngreso.Tables(0).Rows(0).Item("soriginaccount")
                    Else
                        txtReferenciaOrigen.Enabled = False
                        cmbBancoOrigen.Enabled = False
                        btnBancoOrigen.Enabled = False
                        txtCuentaOrigen.Enabled = False
                    End If
                    cmbCuentaDestino.SelectedValue = dsIngreso.Tables(0).Rows(0).Item("idestinationaccountid")
                    txtDescripcionIngreso.Text = dsIngreso.Tables(0).Rows(0).Item("sincomedescription")
                    txtImporte.Text = FormatCurrency(dsIngreso.Tables(0).Rows(0).Item("dincomeamount"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    ireceiverid = dsIngreso.Tables(0).Rows(0).Item("ireceiverid")
                    txtNombrePersona.Text = getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = " & dsIngreso.Tables(0).Rows(0).Item("ireceiverid"))

                End If

            Catch ex As Exception

            End Try

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Ingreso " & iincomeid & " : " & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Income', 'Ingreso', '" & iincomeid & "', '" & txtDescripcionIngreso.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True

        dtFechaIngreso.Select()
        dtFechaIngreso.Focus()

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


    Private Sub cmbTipoIngreso_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoIngreso.SelectedIndexChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If cmbTipoIngreso.SelectedValue = 1 Then

            cmbBancoOrigen.SelectedIndex = -1
            cmbBancoOrigen.Enabled = False
            btnBancoOrigen.Enabled = False
            txtCuentaOrigen.Text = ""
            txtCuentaOrigen.Enabled = False
            txtReferenciaOrigen.Text = ""
            txtReferenciaOrigen.Enabled = False

        Else

            cmbBancoOrigen.Enabled = True
            btnBancoOrigen.Enabled = True
            txtCuentaOrigen.Enabled = True
            txtReferenciaOrigen.Enabled = True

        End If

    End Sub


    Private Sub btnTipoIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoIngreso.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bti As New BuscaTipoIngresos

        bti.susername = susername
        bti.bactive = bactive
        bti.bonline = bonline
        bti.suserfullname = suserfullname
        bti.suseremail = suseremail
        bti.susersession = susersession
        bti.susermachinename = susermachinename
        bti.suserip = suserip

        bti.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bti.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bti.ShowDialog(Me)
        Me.Visible = True

        If bti.DialogResult = Windows.Forms.DialogResult.OK Then

            cmbTipoIngreso.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM incometypes")
            cmbTipoIngreso.DisplayMember = "sincometypedescription"
            cmbTipoIngreso.ValueMember = "iincometypeid"
            cmbTipoIngreso.SelectedIndex = -1

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCuentaDestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCuentaDestino.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bcc As New BuscaCuentasCompania

        bcc.susername = susername
        bcc.bactive = bactive
        bcc.bonline = bonline
        bcc.suserfullname = suserfullname
        bcc.suseremail = suseremail
        bcc.susersession = susersession
        bcc.susermachinename = susermachinename
        bcc.suserip = suserip

        bcc.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bcc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bcc.ShowDialog(Me)
        Me.Visible = True

        If bcc.DialogResult = Windows.Forms.DialogResult.OK Then

            cmbCuentaDestino.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM companyaccounts")
            cmbCuentaDestino.DisplayMember = "scompanyaccountname"
            cmbCuentaDestino.ValueMember = "iaccountid"
            cmbCuentaDestino.SelectedIndex = -1

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnBancoOrigen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBancoOrigen.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bb As New BuscaBancos

        bb.susername = susername
        bb.bactive = bactive
        bb.bonline = bonline
        bb.suserfullname = suserfullname
        bb.suseremail = suseremail
        bb.susersession = susersession
        bb.susermachinename = susermachinename
        bb.suserip = suserip

        bb.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bb.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bb.ShowDialog(Me)
        Me.Visible = True

        If bb.DialogResult = Windows.Forms.DialogResult.OK Then

            cmbBancoOrigen.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM banks")
            cmbBancoOrigen.DisplayMember = "sbankname"
            cmbBancoOrigen.ValueMember = "ibankid"
            cmbBancoOrigen.SelectedIndex = -1

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtReferenciaOrigen_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtReferenciaOrigen.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtReferenciaOrigen.Text.Contains(arrayCaractProhib(carp)) Then
                txtReferenciaOrigen.Text = txtReferenciaOrigen.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtReferenciaOrigen.Select(txtReferenciaOrigen.Text.Length, 0)
        End If

        txtReferenciaOrigen.Text = txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtCuentaOrigen_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCuentaOrigen.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtCuentaOrigen.Text.Contains(arrayCaractProhib(carp)) Then
                txtCuentaOrigen.Text = txtCuentaOrigen.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtCuentaOrigen.Select(txtCuentaOrigen.Text.Length, 0)
        End If

        txtCuentaOrigen.Text = txtCuentaOrigen.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtDescripcionIngreso_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDescripcionIngreso.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtDescripcionIngreso.Text.Contains(arrayCaractProhib(carp)) Then
                txtDescripcionIngreso.Text = txtDescripcionIngreso.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtDescripcionIngreso.Select(txtDescripcionIngreso.Text.Length, 0)
        End If

        txtDescripcionIngreso.Text = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtImporte_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtImporte.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtImporte.Text.Contains(arrayCaractProhib(carp)) Then
                txtImporte.Text = txtImporte.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtImporte.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtImporte.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtImporte.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtImporte.Text.Length

                    If longitud > (lugar + 1) Then
                        txtImporte.Text = txtImporte.Text.Substring(0, lugar) & txtImporte.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtImporte.Text = txtImporte.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtImporte.Select(txtImporte.Text.Length, 0)
        End If

        txtImporte.Text = txtImporte.Text.Replace("--", "").Replace("'", "")
        txtImporte.Text = txtImporte.Text.Trim

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
            ireceiverid = bp.ipeopleid

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tcIngresos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcIngresos.SelectedIndexChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If validaIngreso(True) = True Then

            'Continue

            Dim timesIncomeIsOpen As Integer = 1

            timesIncomeIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid & "'")

            If timesIncomeIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Ingreso. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Ingreso?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesIncomeIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(6) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects"
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & " SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
                queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
                queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    iincomeid = iincomeid + newIdAddition
                End If

            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim importe As Double = 0.0
            Dim valueTipoIngreso As Integer = 0
            Dim valueBanco As Integer = 0
            Dim valueCuenta As Integer = 0

            Try
                importe = CDbl(txtImporte.Text)
            Catch ex As Exception
                importe = 0.0
            End Try

            Try
                valueTipoIngreso = cmbTipoIngreso.SelectedValue
            Catch ex As Exception

            End Try

            Try
                valueBanco = cmbBancoOrigen.SelectedValue
            Catch ex As Exception

            End Try

            Try
                valueCuenta = cmbCuentaDestino.SelectedValue
            Catch ex As Exception

            End Try


            If iincomeid = 0 Then

                Dim queriesCreation(7) As String

                iincomeid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iincomeid) + 1 IS NULL, 1, MAX(iincomeid) + 1) AS iincomeid FROM incomes ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ( `iincomeid` int(11) NOT NULL AUTO_INCREMENT, `iincomedate` int(11) NOT NULL, `sincometime` varchar(11) CHARACTER SET latin1 NOT NULL, `iincometypeid` int(11) NOT NULL, `ioriginbankid` int(11) NOT NULL, `soriginaccount` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `soriginreference` varchar(100) CHARACTER SET latin1 DEFAULT NULL, `idestinationaccountid` int(11) NOT NULL, `sincomedescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dincomeamount` decimal(65,4) NOT NULL, `ireceiverid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Assets"
                queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets ( `iincomeid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iassetid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Projects"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects ( `iincomeid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iprojectid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " VALUES (" & iincomeid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", '00:00:00', " & valueTipoIngreso & ", " & valueBanco & ", '" & txtCuentaOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', " & valueCuenta & ", '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ireceiverid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                executeTransactedSQLCommand(0, queriesCreation)

                sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
                dincomeamount = importe

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " SET iincomedate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", sincometime = '00:00:00', iincometypeid = " & valueTipoIngreso & ", ioriginbankid = " & valueBanco & ", soriginaccount = '" & txtCuentaOrigen.Text.Replace("'", "").Replace("--", "") & "', soriginreference = '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', idestinationaccountid = '" & valueCuenta & "', sincomedescription = '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', dincomeamount = " & importe & ", ireceiverid = " & ireceiverid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iincomeid = " & iincomeid)

                sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
                dincomeamount = importe

            End If


            If tcIngresos.SelectedTab Is tpRelaciones Then


                Dim queryRelacionados As String

                queryRelacionados = "" & _
                "SELECT * FROM ( " & _
                "SELECT ic.iincomeid, ic.sincomedescription AS 'Descripcion',  " & _
                "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
                "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A', sia.iinsertiondate, sia.sinsertiontime " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ic " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets sia ON ic.iincomeid = sia.iincomeid " & _
                "JOIN assets a ON sia.iassetid = a.iassetid " & _
                "WHERE " & _
                "ic.iincomeid = " & iincomeid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets WHERE iincomeid = " & iincomeid & ")) " & _
                "UNION " & _
                "SELECT ic.iincomeid, ic.sincomedescription AS 'Descripcion',  " & _
                "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
                "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A', sip.iinsertiondate, sip.sinsertiontime " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ic " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects sip ON ic.iincomeid = sip.iincomeid " & _
                "JOIN projects p ON sip.iprojectid = p.iprojectid " & _
                "WHERE " & _
                "ic.iincomeid = " & iincomeid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects WHERE iincomeid = " & iincomeid & ")) " & _
                ") tmpA ORDER BY iinsertiondate ASC, sinsertiontime ASC "

                setDataGridView(dgvConRelacion, queryRelacionados, True)

                dgvConRelacion.Columns(0).Visible = False
                dgvConRelacion.Columns(2).Visible = False
                dgvConRelacion.Columns(4).Visible = False
                dgvConRelacion.Columns(6).Visible = False
                dgvConRelacion.Columns(7).Visible = False

                dgvConRelacion.Columns(0).Width = 30
                dgvConRelacion.Columns(1).Width = 200
                dgvConRelacion.Columns(2).Width = 30
                dgvConRelacion.Columns(3).Width = 100
                dgvConRelacion.Columns(4).Width = 30
                dgvConRelacion.Columns(5).Width = 100


            End If


        Else

            tcIngresos.SelectedTab = tpIngreso

        End If

    End Sub


    Private Function validaIngreso(ByVal silent As Boolean) As Boolean

        txtReferenciaOrigen.Text = txtReferenciaOrigen.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtCuentaOrigen.Text = txtCuentaOrigen.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtDescripcionIngreso.Text = txtDescripcionIngreso.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtImporte.Text = txtImporte.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If cmbTipoIngreso.SelectedIndex = -1 Then

            If silent = False Then
                MsgBox("¿Podrías seleccionar un tipo de Ingreso?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            cmbTipoIngreso.Select()
            cmbTipoIngreso.Focus()
            Return False

        End If

        If cmbCuentaDestino.SelectedIndex = -1 Then

            If silent = False Then
                MsgBox("¿Podrías seleccionar la cuenta a la que se depositó el dinero del Ingreso?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            cmbCuentaDestino.Select()
            cmbCuentaDestino.Focus()
            Return False

        End If

        If txtDescripcionIngreso.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner una Descripción al Ingreso?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtDescripcionIngreso.Select()
            txtDescripcionIngreso.Focus()
            Return False

        End If

        Dim importe As Double = 0.0

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        If importe = 0.0 Then

            If silent = False Then
                MsgBox("¿Podrías poner la cantidad que Ingresó?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtImporte.Select()
            txtImporte.Focus()
            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click, btnCancelarRelacion.Click

        'iincomeid = 0
        'sincomedescription = ""
        'dincomeamount = 0.0
        'ireceiverid = 0

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaIngreso(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesIncomeIsOpen As Integer = 1

        timesIncomeIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid & "'")

        If timesIncomeIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Ingreso. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Ingreso?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesIncomeIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(6) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & " SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
            queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iincomeid = iincomeid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0
        Dim valueTipoIngreso As Integer = 0
        Dim valueBanco As Integer = 0
        Dim valueCuenta As Integer = 0

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        Try
            valueTipoIngreso = cmbTipoIngreso.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueBanco = cmbBancoOrigen.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueCuenta = cmbCuentaDestino.SelectedValue
        Catch ex As Exception

        End Try


        If iincomeid = 0 Then

            Dim queriesCreation(7) As String

            iincomeid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iincomeid) + 1 IS NULL, 1, MAX(iincomeid) + 1) AS iincomeid FROM incomes ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ( `iincomeid` int(11) NOT NULL AUTO_INCREMENT, `iincomedate` int(11) NOT NULL, `sincometime` varchar(11) CHARACTER SET latin1 NOT NULL, `iincometypeid` int(11) NOT NULL, `ioriginbankid` int(11) NOT NULL, `soriginaccount` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `soriginreference` varchar(100) CHARACTER SET latin1 DEFAULT NULL, `idestinationaccountid` int(11) NOT NULL, `sincomedescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dincomeamount` decimal(65,4) NOT NULL, `ireceiverid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets ( `iincomeid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iassetid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects ( `iincomeid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iprojectid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " VALUES (" & iincomeid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", '00:00:00', " & valueTipoIngreso & ", " & valueBanco & ", '" & txtCuentaOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', " & valueCuenta & ", '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ireceiverid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
            dincomeamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " SET iincomedate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", sincometime = '00:00:00', iincometypeid = " & valueTipoIngreso & ", ioriginbankid = " & valueBanco & ", soriginaccount = '" & txtCuentaOrigen.Text.Replace("'", "").Replace("--", "") & "', soriginreference = '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', idestinationaccountid = '" & valueCuenta & "', sincomedescription = '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', dincomeamount = " & importe & ", ireceiverid = " & ireceiverid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iincomeid = " & iincomeid)

            sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
            dincomeamount = importe

        End If

        Dim queries(10) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM incomes " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc WHERE incomes.iincomeid = tclc.iincomeid) "

        queries(1) = "" & _
        "UPDATE incomes clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc ON tclc.iincomeid = clc.iincomeid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.iincomedate = tclc.iincomedate, clc.sincometime = tclc.sincometime, clc.iincometypeid = tclc.iincometypeid, clc.soriginaccount = tclc.soriginaccount, clc.soriginreference = tclc.soriginreference, clc.ioriginbankid = tclc.ioriginbankid, clc.idestinationaccountid = tclc.idestinationaccountid, clc.sincomedescription = tclc.sincomedescription, clc.dincomeamount = tclc.dincomeamount, clc.ireceiverid = tclc.ireceiverid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO incomes " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomes clc WHERE tclc.iincomeid = clc.iincomeid AND clc.iincomeid = " & iincomeid & ") "

        queries(3) = "" & _
        "DELETE " & _
        "FROM incomeassets " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc WHERE incomeassets.iincomeid = tclc.iincomeid AND incomeassets.iassetid = tclc.iassetid) "

        queries(4) = "" & _
        "UPDATE incomeassets clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc ON tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sincomeextranote = tclc.sincomeextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(5) = "" & _
        "INSERT INTO incomeassets " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomeassets clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid AND clc.iincomeid = " & iincomeid & ") "

        queries(6) = "" & _
        "DELETE " & _
        "FROM incomeprojects " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc WHERE incomeprojects.iincomeid = tclc.iincomeid AND incomeprojects.iprojectid = tclc.iprojectid) "

        queries(7) = "" & _
        "UPDATE incomeprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc ON tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sincomeextranote = tclc.sincomeextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(8) = "" & _
        "INSERT INTO incomeprojects " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomeprojects clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid AND clc.iincomeid = " & iincomeid & ") "

        queries(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Ingreso " & iincomeid & " : " & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then

            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")

            wasCreated = True
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnBuscarRelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarRelacion.Click


        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0
        Dim valueTipoIngreso As Integer = 0
        Dim valueBanco As Integer = 0
        Dim valueCuenta As Integer = 0

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        Try
            valueTipoIngreso = cmbTipoIngreso.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueBanco = cmbBancoOrigen.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueCuenta = cmbCuentaDestino.SelectedValue
        Catch ex As Exception

        End Try


        If iincomeid = 0 Then

            Dim queriesCreation(7) As String

            iincomeid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iincomeid) + 1 IS NULL, 1, MAX(iincomeid) + 1) AS iincomeid FROM incomes ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ( `iincomeid` int(11) NOT NULL AUTO_INCREMENT, `iincomedate` int(11) NOT NULL, `sincometime` varchar(11) CHARACTER SET latin1 NOT NULL, `iincometypeid` int(11) NOT NULL, `ioriginbankid` int(11) NOT NULL, `soriginaccount` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `soriginreference` varchar(100) CHARACTER SET latin1 DEFAULT NULL, `idestinationaccountid` int(11) NOT NULL, `sincomedescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dincomeamount` decimal(65,4) NOT NULL, `ireceiverid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets ( `iincomeid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iassetid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects ( `iincomeid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iprojectid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " VALUES (" & iincomeid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", '00:00:00', " & valueTipoIngreso & ", " & valueBanco & ", '" & txtCuentaOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', " & valueCuenta & ", '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ireceiverid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
            dincomeamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " SET iincomedate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", sincometime = '00:00:00', iincometypeid = " & valueTipoIngreso & ", ioriginbankid = " & valueBanco & ", soriginaccount = '" & txtCuentaOrigen.Text.Replace("'", "").Replace("--", "") & "', soriginreference = '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', idestinationaccountid = '" & valueCuenta & "', sincomedescription = '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', dincomeamount = " & importe & ", ireceiverid = " & ireceiverid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iincomeid = " & iincomeid)

            sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
            dincomeamount = importe

        End If


        'Empieza codigo de Buscar Relacion


        If cmbTipoRelacion.SelectedItem = "" Then
            Exit Sub
        ElseIf cmbTipoRelacion.SelectedItem = "Relacionar a Activo" Then

            'Assets

            iselectedrelationtypeid = 0
            iselectedrelationid = 0

            Dim ba As New BuscaActivos

            ba.susername = susername
            ba.bactive = bactive
            ba.bonline = bonline
            ba.suserfullname = suserfullname

            ba.suseremail = suseremail
            ba.susersession = susersession
            ba.susermachinename = susermachinename
            ba.suserip = suserip

            ba.isEdit = False

            If Me.WindowState = FormWindowState.Maximized Then
                ba.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ba.ShowDialog(Me)
            Me.Visible = True

            If ba.DialogResult = Windows.Forms.DialogResult.OK Then

                iselectedrelationtypeid = 1
                iselectedrelationid = ba.iassetid

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets VALUES (" & iincomeid & ", " & iselectedrelationid & ", '', " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', '" & susername & "')")

                iselectedinputwithrelationid = iincomeid

            End If


        ElseIf cmbTipoRelacion.SelectedItem = "Relacionar a Proyecto" Then

            'Proyectos

            iselectedrelationtypeid = 0
            iselectedrelationid = 0

            Dim bi As New BuscaProyectos

            bi.susername = susername
            bi.bactive = bactive
            bi.bonline = bonline
            bi.suserfullname = suserfullname

            bi.suseremail = suseremail
            bi.susersession = susersession
            bi.susermachinename = susermachinename
            bi.suserip = suserip

            bi.isEdit = False

            If Me.WindowState = FormWindowState.Maximized Then
                bi.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            bi.ShowDialog(Me)
            Me.Visible = True

            If bi.DialogResult = Windows.Forms.DialogResult.OK Then

                iselectedrelationtypeid = 2
                iselectedrelationid = bi.iprojectid

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects VALUES (" & iincomeid & ", " & iselectedrelationid & ", '', " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', '" & susername & "')")

                iselectedinputwithrelationid = iincomeid
                
            End If

        End If


        Dim queryRelacionados As String

        queryRelacionados = "" & _
        "SELECT * FROM ( " & _
        "SELECT ic.iincomeid, ic.sincomedescription AS 'Insumo',  " & _
        "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
        "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A', sia.iinsertiondate, sia.sinsertiontime " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ic " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets sia ON ic.iincomeid = sia.iincomeid " & _
        "JOIN assets a ON sia.iassetid = a.iassetid " & _
        "WHERE " & _
        "ic.iincomeid = " & iincomeid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets WHERE iincomeid = " & iincomeid & ")) " & _
        "UNION " & _
        "SELECT ic.iincomeid, ic.sincomedescription AS 'Insumo',  " & _
        "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
        "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A', sip.iinsertiondate, sip.sinsertiontime " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ic " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects sip ON ic.iincomeid = sip.iincomeid " & _
        "JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "WHERE " & _
        "ic.iincomeid = " & iincomeid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects WHERE iincomeid = " & iincomeid & ")) " & _
        ") tmpA ORDER BY iinsertiondate ASC, sinsertiontime ASC "

        setDataGridView(dgvConRelacion, queryRelacionados, True)

        dgvConRelacion.Columns(0).Visible = False
        dgvConRelacion.Columns(2).Visible = False
        dgvConRelacion.Columns(4).Visible = False
        dgvConRelacion.Columns(6).Visible = False
        dgvConRelacion.Columns(7).Visible = False

        dgvConRelacion.Columns(0).Width = 30
        dgvConRelacion.Columns(1).Width = 200
        dgvConRelacion.Columns(2).Width = 30
        dgvConRelacion.Columns(3).Width = 100
        dgvConRelacion.Columns(4).Width = 30
        dgvConRelacion.Columns(5).Width = 100

    End Sub


    Private Sub btnEliminarRelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarRelacion.Click

        If iselectedrelationtypeid = 0 Then
            Exit Sub
        End If

        If MsgBox("¿Realmente deseas borrar esta relación?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Insumo Factura") = MsgBoxResult.Yes Then

            If iselectedrelationtypeid = 1 Then

                'Assets
                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets WHERE iincomeid = " & iincomeid & " AND iassetid = " & iselectedrelationid)

            ElseIf iselectedrelationtypeid = 2 Then

                'Projects
                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects WHERE iincomeid = " & iincomeid & " AND iprojectid = " & iselectedrelationid)

            End If

            Dim queryRelacionados As String

            queryRelacionados = "" & _
            "SELECT * FROM ( " & _
            "SELECT ic.iincomeid, ic.sincomedescription AS 'Insumo',  " & _
            "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
            "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A', sia.iinsertiondate, sia.sinsertiontime " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ic " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets sia ON ic.iincomeid = sia.iincomeid " & _
            "JOIN assets a ON sia.iassetid = a.iassetid " & _
            "WHERE " & _
            "ic.iincomeid = " & iincomeid & " " & _
            "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets WHERE iincomeid = " & iincomeid & ")) " & _
            "UNION " & _
            "SELECT ic.iincomeid, ic.sincomedescription AS 'Insumo',  " & _
            "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
            "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A', sip.iinsertiondate, sip.sinsertiontime " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ic " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects sip ON ic.iincomeid = sip.iincomeid " & _
            "JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "WHERE " & _
            "ic.iincomeid = " & iincomeid & " " & _
            "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects WHERE iincomeid = " & iincomeid & ")) " & _
            ") tmpA ORDER BY iinsertiondate ASC, sinsertiontime ASC "

            setDataGridView(dgvConRelacion, queryRelacionados, True)

            dgvConRelacion.Columns(0).Visible = False
            dgvConRelacion.Columns(2).Visible = False
            dgvConRelacion.Columns(4).Visible = False
            dgvConRelacion.Columns(6).Visible = False
            dgvConRelacion.Columns(7).Visible = False

            dgvConRelacion.Columns(0).Width = 30
            dgvConRelacion.Columns(1).Width = 200
            dgvConRelacion.Columns(2).Width = 30
            dgvConRelacion.Columns(3).Width = 100
            dgvConRelacion.Columns(4).Width = 30
            dgvConRelacion.Columns(5).Width = 100

        End If

    End Sub


    Private Sub btnGuardarRelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarRelacion.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaIngreso(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesIncomeIsOpen As Integer = 1

        timesIncomeIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid & "'")

        If timesIncomeIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Ingreso. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Ingreso?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesIncomeIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(6) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & " SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
            queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iincomeid = iincomeid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0
        Dim valueTipoIngreso As Integer = 0
        Dim valueBanco As Integer = 0
        Dim valueCuenta As Integer = 0

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        Try
            valueTipoIngreso = cmbTipoIngreso.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueBanco = cmbBancoOrigen.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueCuenta = cmbCuentaDestino.SelectedValue
        Catch ex As Exception

        End Try


        If iincomeid = 0 Then

            Dim queriesCreation(7) As String

            iincomeid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iincomeid) + 1 IS NULL, 1, MAX(iincomeid) + 1) AS iincomeid FROM incomes ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ( `iincomeid` int(11) NOT NULL AUTO_INCREMENT, `iincomedate` int(11) NOT NULL, `sincometime` varchar(11) CHARACTER SET latin1 NOT NULL, `iincometypeid` int(11) NOT NULL, `ioriginbankid` int(11) NOT NULL, `soriginaccount` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `soriginreference` varchar(100) CHARACTER SET latin1 DEFAULT NULL, `idestinationaccountid` int(11) NOT NULL, `sincomedescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dincomeamount` decimal(65,4) NOT NULL, `ireceiverid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets ( `iincomeid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iassetid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects ( `iincomeid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iprojectid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " VALUES (" & iincomeid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", '00:00:00', " & valueTipoIngreso & ", " & valueBanco & ", '" & txtCuentaOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', " & valueCuenta & ", '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ireceiverid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
            dincomeamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " SET iincomedate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", sincometime = '00:00:00', iincometypeid = " & valueTipoIngreso & ", ioriginbankid = " & valueBanco & ", soriginaccount = '" & txtCuentaOrigen.Text.Replace("'", "").Replace("--", "") & "', soriginreference = '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', idestinationaccountid = '" & valueCuenta & "', sincomedescription = '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', dincomeamount = " & importe & ", ireceiverid = " & ireceiverid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iincomeid = " & iincomeid)

            sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
            dincomeamount = importe

        End If

        Dim queries(10) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM incomes " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc WHERE incomes.iincomeid = tclc.iincomeid) "

        queries(1) = "" & _
        "UPDATE incomes clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc ON tclc.iincomeid = clc.iincomeid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.iincomedate = tclc.iincomedate, clc.sincometime = tclc.sincometime, clc.iincometypeid = tclc.iincometypeid, clc.soriginaccount = tclc.soriginaccount, clc.soriginreference = tclc.soriginreference, clc.ioriginbankid = tclc.ioriginbankid, clc.idestinationaccountid = tclc.idestinationaccountid, clc.sincomedescription = tclc.sincomedescription, clc.dincomeamount = tclc.dincomeamount, clc.ireceiverid = tclc.ireceiverid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO incomes " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomes clc WHERE tclc.iincomeid = clc.iincomeid AND clc.iincomeid = " & iincomeid & ") "

        queries(3) = "" & _
        "DELETE " & _
        "FROM incomeassets " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc WHERE incomeassets.iincomeid = tclc.iincomeid AND incomeassets.iassetid = tclc.iassetid) "

        queries(4) = "" & _
        "UPDATE incomeassets clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc ON tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sincomeextranote = tclc.sincomeextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(5) = "" & _
        "INSERT INTO incomeassets " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomeassets clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid AND clc.iincomeid = " & iincomeid & ") "

        queries(6) = "" & _
        "DELETE " & _
        "FROM incomeprojects " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc WHERE incomeprojects.iincomeid = tclc.iincomeid AND incomeprojects.iprojectid = tclc.iprojectid) "

        queries(7) = "" & _
        "UPDATE incomeprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc ON tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sincomeextranote = tclc.sincomeextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(8) = "" & _
        "INSERT INTO incomeprojects " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomeprojects clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid AND clc.iincomeid = " & iincomeid & ") "

        queries(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Ingreso " & iincomeid & " : " & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then

            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")

            wasCreated = True
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnRevisiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevisiones.Click, btnRevisionesRelaciones.Click

        If MsgBox("Revisar un Ingreso automáticamente guarda sus cambios. ¿Deseas guardar este Ingreso ahora?", MsgBoxStyle.YesNo, "Pregunta Guardado") = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesIncomeIsOpen As Integer = 1

        timesIncomeIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid & "'")

        If timesIncomeIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Ingreso. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Ingreso?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesIncomeIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Income" & iincomeid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(6) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & " SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Assets SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid
            queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid + newIdAddition & "Projects SET iincomeid = " & iincomeid + newIdAddition & " WHERE iincomeid = " & iincomeid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iincomeid = iincomeid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0
        Dim valueTipoIngreso As Integer = 0
        Dim valueBanco As Integer = 0
        Dim valueCuenta As Integer = 0

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        Try
            valueTipoIngreso = cmbTipoIngreso.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueBanco = cmbBancoOrigen.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueCuenta = cmbCuentaDestino.SelectedValue
        Catch ex As Exception

        End Try


        If iincomeid = 0 Then

            Dim queriesCreation(7) As String

            iincomeid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iincomeid) + 1 IS NULL, 1, MAX(iincomeid) + 1) AS iincomeid FROM incomes ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " ( `iincomeid` int(11) NOT NULL AUTO_INCREMENT, `iincomedate` int(11) NOT NULL, `sincometime` varchar(11) CHARACTER SET latin1 NOT NULL, `iincometypeid` int(11) NOT NULL, `ioriginbankid` int(11) NOT NULL, `soriginaccount` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `soriginreference` varchar(100) CHARACTER SET latin1 DEFAULT NULL, `idestinationaccountid` int(11) NOT NULL, `sincomedescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dincomeamount` decimal(65,4) NOT NULL, `ireceiverid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets ( `iincomeid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iassetid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income0Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects ( `iincomeid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sincomeextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iincomeid`,`iprojectid`) USING BTREE, KEY `incomeid` (`iincomeid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " VALUES (" & iincomeid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", '00:00:00', " & valueTipoIngreso & ", " & valueBanco & ", '" & txtCuentaOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', " & valueCuenta & ", '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ireceiverid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
            dincomeamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " SET iincomedate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaIngreso.Value) & ", sincometime = '00:00:00', iincometypeid = " & valueTipoIngreso & ", ioriginbankid = " & valueBanco & ", soriginaccount = '" & txtCuentaOrigen.Text.Replace("'", "").Replace("--", "") & "', soriginreference = '" & txtReferenciaOrigen.Text.Replace("--", "").Replace("'", "") & "', idestinationaccountid = '" & valueCuenta & "', sincomedescription = '" & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', dincomeamount = " & importe & ", ireceiverid = " & ireceiverid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iincomeid = " & iincomeid)

            sincomedescription = txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "")
            dincomeamount = importe

        End If

        Dim queries(10) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM incomes " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc WHERE incomes.iincomeid = tclc.iincomeid) "

        queries(1) = "" & _
        "UPDATE incomes clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc ON tclc.iincomeid = clc.iincomeid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.iincomedate = tclc.iincomedate, clc.sincometime = tclc.sincometime, clc.iincometypeid = tclc.iincometypeid, clc.soriginaccount = tclc.soriginaccount, clc.soriginreference = tclc.soriginreference, clc.ioriginbankid = tclc.ioriginbankid, clc.idestinationaccountid = tclc.idestinationaccountid, clc.sincomedescription = tclc.sincomedescription, clc.dincomeamount = tclc.dincomeamount, clc.ireceiverid = tclc.ireceiverid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO incomes " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomes clc WHERE tclc.iincomeid = clc.iincomeid AND clc.iincomeid = " & iincomeid & ") "

        queries(3) = "" & _
        "DELETE " & _
        "FROM incomeassets " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc WHERE incomeassets.iincomeid = tclc.iincomeid AND incomeassets.iassetid = tclc.iassetid) "

        queries(4) = "" & _
        "UPDATE incomeassets clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc ON tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sincomeextranote = tclc.sincomeextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(5) = "" & _
        "INSERT INTO incomeassets " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Assets tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomeassets clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iassetid = clc.iassetid AND clc.iincomeid = " & iincomeid & ") "

        queries(6) = "" & _
        "DELETE " & _
        "FROM incomeprojects " & _
        "WHERE iincomeid = " & iincomeid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc WHERE incomeprojects.iincomeid = tclc.iincomeid AND incomeprojects.iprojectid = tclc.iprojectid) "

        queries(7) = "" & _
        "UPDATE incomeprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc ON tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sincomeextranote = tclc.sincomeextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(8) = "" & _
        "INSERT INTO incomeprojects " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Income" & iincomeid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM incomeprojects clc WHERE tclc.iincomeid = clc.iincomeid AND tclc.iprojectid = clc.iprojectid AND clc.iincomeid = " & iincomeid & ") "

        queries(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Ingreso " & iincomeid & " : " & txtDescripcionIngreso.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then

            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            wasCreated = True

            Dim br As New BuscaRevisiones

            br.susername = susername
            br.bactive = bactive
            br.bonline = bonline
            br.suserfullname = suserfullname
            br.suseremail = suseremail
            br.susersession = susersession
            br.susermachinename = susermachinename
            br.suserip = suserip

            br.isEdit = True

            br.srevisiondocument = "Ingreso"
            br.sid = iincomeid

            If Me.WindowState = FormWindowState.Maximized Then
                br.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            br.ShowDialog(Me)
            Me.Visible = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class