Public Class AgregarGastoPorCaseta

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

    Public ipaytollid As Integer = 0
    Public spaytollorigin As String = ""
    Public spaytolldestination As String = ""
    Public dpaytollamount As Double = 0.0

    Private iselectedinputwithrelationid As Integer = 0
    Private iselectedrelationtypeid As Integer = 0
    Private iselectedrelationid As Integer = 0

    Public ipeopleid As Integer = 0

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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Caseta', 'OK')")

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


    Private Sub AgregarCaseta_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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
        "FROM paytolls " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc WHERE paytolls.ipaytollid = tclc.ipaytollid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc JOIN paytolls clc ON tclc.ipaytollid = clc.ipaytollid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytolls clc WHERE tclc.ipaytollid = clc.ipaytollid AND clc.ipaytollid = " & ipaytollid & ") ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM paytollassets " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc WHERE paytollassets.ipaytollid = tclc.ipaytollid AND paytollassets.iassetid = tclc.iassetid) ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc JOIN paytollassets clc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytollassets clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid AND clc.ipaytollid = " & ipaytollid & ") ")

        conteo7 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM paytollprojects " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc WHERE paytollprojects.ipaytollid = tclc.ipaytollid AND paytollprojects.iprojectid = tclc.iprojectid) ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc JOIN paytollprojects clc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo9 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytollprojects clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid AND clc.ipaytollid = " & ipaytollid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 + conteo7 + conteo8 + conteo9 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaCaseta(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Gasto por Caseta está incompleto. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesPaytollIsOpen As Integer = 1

            timesPaytollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid & "'")

            If timesPaytollIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Gasto por Caseta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando este Gasto por Caseta?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesPaytollIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(6) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects"
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & " SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
                queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
                queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    ipaytollid = ipaytollid + newIdAddition
                End If

            End If


            Dim queries(10) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM paytolls " & _
            "WHERE ipaytollid = " & ipaytollid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc WHERE paytolls.ipaytollid = tclc.ipaytollid) "

            queries(1) = "" & _
            "UPDATE paytolls clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc ON tclc.ipaytollid = clc.ipaytollid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipaytolldate = tclc.ipaytolldate, clc.spaytolltime = tclc.spaytolltime, clc.sorigin = tclc.sorigin, clc.sdestination = tclc.sdestination, clc.dpaytollamount = tclc.dpaytollamount, clc.ipeopleid = tclc.ipeopleid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(2) = "" & _
            "INSERT INTO paytolls " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM paytolls clc WHERE tclc.ipaytollid = clc.ipaytollid AND clc.ipaytollid = " & ipaytollid & ") "

            queries(3) = "" & _
            "DELETE " & _
            "FROM paytollassets " & _
            "WHERE ipaytollid = " & ipaytollid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc WHERE paytollassets.ipaytollid = tclc.ipaytollid AND paytollassets.iassetid = tclc.iassetid) "

            queries(4) = "" & _
            "UPDATE paytollassets clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(5) = "" & _
            "INSERT INTO paytollassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM paytollassets clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid AND clc.ipaytollid = " & ipaytollid & ") "

            queries(6) = "" & _
            "DELETE " & _
            "FROM paytollprojects " & _
            "WHERE ipaytollid = " & ipaytollid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc WHERE paytollprojects.ipaytollid = tclc.ipaytollid AND paytollprojects.iprojectid = tclc.iprojectid) "

            queries(7) = "" & _
            "UPDATE paytollprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "INSERT INTO paytollprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM paytollprojects clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid AND clc.ipaytollid = " & ipaytollid & ") "

            queries(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó este Gasto por Caseta " & ipaytollid & " : " & txtOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

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

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects"
        queriesDelete(3) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró este Gasto por Caseta " & ipaytollid & " : " & txtOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')"
        queriesDelete(4) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Paytoll', 'Caseta', '" & ipaytollid & "', '" & txtOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarCaseta_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarCaseta_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesPaytollIsOpen As Integer = 0

        timesPaytollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid & "'")

        If timesPaytollIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Gasto por Caseta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo este Gasto por Caseta?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

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

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ( `ipaytollid` int(11) NOT NULL AUTO_INCREMENT, `ipaytolldate` int(11) NOT NULL, `spaytolltime` varchar(11) NOT NULL, `sorigin` varchar(500) DEFAULT NULL, `sdestination` varchar(500) DEFAULT NULL, `dpaytollamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`ipaytollid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets ( `ipaytollid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iassetid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects ( `ipaytollid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iprojectid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        If isEdit = False Then

            txtDestino.Text = ""
            txtOrigen.Text = ""
            txtImporte.Text = "0.00"

        Else

            If isRecover = False Then

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " SELECT * FROM paytolls WHERE ipaytollid = " & ipaytollid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets SELECT * FROM paytollassets WHERE ipaytollid = " & ipaytollid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects SELECT * FROM paytollprojects WHERE ipaytollid = " & ipaytollid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsCaseta As DataSet
            dsCaseta = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " WHERE ipaytollid = " & ipaytollid)

            Try

                If dsCaseta.Tables(0).Rows.Count > 0 Then

                    dtFechaTicket.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsCaseta.Tables(0).Rows(0).Item("ipaytolldate")) & " " & dsCaseta.Tables(0).Rows(0).Item("spaytolltime")
                    txtOrigen.Text = dsCaseta.Tables(0).Rows(0).Item("sorigin")
                    txtDestino.Text = dsCaseta.Tables(0).Rows(0).Item("sdestination")
                    txtImporte.Text = FormatCurrency(dsCaseta.Tables(0).Rows(0).Item("dpaytollamount"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                    ipeopleid = dsCaseta.Tables(0).Rows(0).Item("ipeopleid")
                    txtNombrePersona.Text = getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = " & dsCaseta.Tables(0).Rows(0).Item("ipeopleid"))

                End If

            Catch ex As Exception

            End Try

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió este Gasto por Caseta " & ipaytollid & " : " & txtOrigen.Text.Replace("--", "").Replace("'", "") & " a " & txtDestino.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Paytoll', 'Caseta', '" & ipaytollid & "', '" & txtOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True

        dtFechaTicket.Select()
        dtFechaTicket.Focus()

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


    Private Sub txtDestino_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDestino.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtDestino.Text.Contains(arrayCaractProhib(carp)) Then
                txtDestino.Text = txtDestino.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtDestino.Select(txtDestino.Text.Length, 0)
        End If

        txtDestino.Text = txtDestino.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtOrigen_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrigen.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtOrigen.Text.Contains(arrayCaractProhib(carp)) Then
                txtOrigen.Text = txtOrigen.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtOrigen.Select(txtOrigen.Text.Length, 0)
        End If

        txtOrigen.Text = txtOrigen.Text.Replace("--", "").Replace("'", "")

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
            ipeopleid = bp.ipeopleid

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tcCasetas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcCasetas.SelectedIndexChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If validaCaseta(True) = True Then

            'Continue

            Dim timesPaytollIsOpen As Integer = 1

            timesPaytollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid & "'")

            If timesPaytollIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Gasto por Caseta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando este Gasto por Caseta?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesPaytollIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(6) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects"
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & " SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
                queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
                queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    ipaytollid = ipaytollid + newIdAddition
                End If

            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim importe As Double = 0.0

            Try
                importe = CDbl(txtImporte.Text)
            Catch ex As Exception
                importe = 0.0
            End Try

            If ipaytollid = 0 Then

                Dim queriesCreation(7) As String

                ipaytollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipaytollid) + 1 IS NULL, 1, MAX(ipaytollid) + 1) AS ipaytollid FROM paytolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ( `ipaytollid` int(11) NOT NULL AUTO_INCREMENT, `ipaytolldate` int(11) NOT NULL, `spaytolltime` varchar(11) NOT NULL, `sorigin` varchar(500) DEFAULT NULL, `sdestination` varchar(500) DEFAULT NULL, `dpaytollamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`ipaytollid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"

                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Assets"
                queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets ( `ipaytollid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iassetid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Projects"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects ( `ipaytollid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iprojectid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " VALUES (" & ipaytollid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", '00:00:00', '" & txtOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                executeTransactedSQLCommand(0, queriesCreation)

                spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
                spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
                dpaytollamount = importe

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " SET ipaytolldate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", spaytolltime = '00:00:00', sorigin = '" & txtOrigen.Text.Replace("'", "").Replace("--", "") & "', sdestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', dpaytollamount = " & importe & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipaytollid = " & ipaytollid)

                spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
                spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
                dpaytollamount = importe

            End If


            If tcCasetas.SelectedTab Is tpRelaciones Then


                Dim queryRelacionados As String

                queryRelacionados = "" & _
                "SELECT ic.ipaytollid, CONCAT(ic.sorigin, ' a ', ic.sdestination) AS 'Caseta',  " & _
                "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
                "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ic " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets sia ON ic.ipaytollid = sia.ipaytollid " & _
                "JOIN assets a ON sia.iassetid = a.iassetid " & _
                "WHERE " & _
                "ic.ipaytollid = " & ipaytollid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets WHERE ipaytollid = " & ipaytollid & ")) " & _
                "UNION " & _
                "SELECT ic.ipaytollid, CONCAT(ic.sorigin, ' a ', ic.sdestination) AS 'Caseta',  " & _
                "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
                "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ic " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects sip ON ic.ipaytollid = sip.ipaytollid " & _
                "JOIN projects p ON sip.iprojectid = p.iprojectid " & _
                "WHERE " & _
                "ic.ipaytollid = " & ipaytollid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects WHERE ipaytollid = " & ipaytollid & ")) "

                setDataGridView(dgvConRelacion, queryRelacionados, True)

                dgvConRelacion.Columns(0).Visible = False
                dgvConRelacion.Columns(2).Visible = False
                dgvConRelacion.Columns(4).Visible = False

                dgvConRelacion.Columns(0).Width = 30
                dgvConRelacion.Columns(1).Width = 200
                dgvConRelacion.Columns(2).Width = 30
                dgvConRelacion.Columns(3).Width = 100
                dgvConRelacion.Columns(4).Width = 30
                dgvConRelacion.Columns(5).Width = 100


            End If


        Else

            tcCasetas.SelectedTab = tpCaseta

        End If

    End Sub


    Private Function validaCaseta(ByVal silent As Boolean) As Boolean

        txtDestino.Text = txtDestino.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtOrigen.Text = txtOrigen.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtImporte.Text = txtImporte.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If txtOrigen.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner los datos que indica el Ticket?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtOrigen.Select()
            txtOrigen.Focus()
            Return False

        End If

        If txtDestino.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner los datos que indica el Ticket?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtDestino.Select()
            txtDestino.Focus()
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
                MsgBox("¿Podrías poner el Importe del ticket de la Caseta?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtImporte.Select()
            txtImporte.Focus()
            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click, btnCancelarRelacion.Click

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaCaseta(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesPaytollIsOpen As Integer = 1

        timesPaytollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid & "'")

        If timesPaytollIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Gasto por Caseta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando este Gasto por Caseta?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesPaytollIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(6) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & " SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
            queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                ipaytollid = ipaytollid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        If ipaytollid = 0 Then

            Dim queriesCreation(7) As String

            ipaytollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipaytollid) + 1 IS NULL, 1, MAX(ipaytollid) + 1) AS ipaytollid FROM paytolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ( `ipaytollid` int(11) NOT NULL AUTO_INCREMENT, `ipaytolldate` int(11) NOT NULL, `spaytolltime` varchar(11) NOT NULL, `sorigin` varchar(500) DEFAULT NULL, `sdestination` varchar(500) DEFAULT NULL, `dpaytollamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`ipaytollid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets ( `ipaytollid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iassetid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects ( `ipaytollid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iprojectid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " VALUES (" & ipaytollid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", '00:00:00', '" & txtOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
            spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
            dpaytollamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " SET ipaytolldate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", spaytolltime = '00:00:00', sorigin = '" & txtOrigen.Text.Replace("'", "").Replace("--", "") & "', sdestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', dpaytollamount = " & importe & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipaytollid = " & ipaytollid)

            spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
            spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
            dpaytollamount = importe

        End If

        Dim queries(10) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM paytolls " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc WHERE paytolls.ipaytollid = tclc.ipaytollid) "

        queries(1) = "" & _
        "UPDATE paytolls clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc ON tclc.ipaytollid = clc.ipaytollid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipaytolldate = tclc.ipaytolldate, clc.spaytolltime = tclc.spaytolltime, clc.sorigin = tclc.sorigin, clc.sdestination = tclc.sdestination, clc.dpaytollamount = tclc.dpaytollamount, clc.ipeopleid = tclc.ipeopleid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO paytolls " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytolls clc WHERE tclc.ipaytollid = clc.ipaytollid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(3) = "" & _
        "DELETE " & _
        "FROM paytollassets " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc WHERE paytollassets.ipaytollid = tclc.ipaytollid AND paytollassets.iassetid = tclc.iassetid) "

        queries(4) = "" & _
        "UPDATE paytollassets clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(5) = "" & _
        "INSERT INTO paytollassets " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytollassets clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(6) = "" & _
        "DELETE " & _
        "FROM paytollprojects " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc WHERE paytollprojects.ipaytollid = tclc.ipaytollid AND paytollprojects.iprojectid = tclc.iprojectid) "

        queries(7) = "" & _
        "UPDATE paytollprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(8) = "" & _
        "INSERT INTO paytollprojects " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytollprojects clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó este Gasto por Caseta " & ipaytollid & " : " & txtOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

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

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        If ipaytollid = 0 Then

            Dim queriesCreation(7) As String

            ipaytollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipaytollid) + 1 IS NULL, 1, MAX(ipaytollid) + 1) AS ipaytollid FROM paytolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ( `ipaytollid` int(11) NOT NULL AUTO_INCREMENT, `ipaytolldate` int(11) NOT NULL, `spaytolltime` varchar(11) NOT NULL, `sorigin` varchar(500) DEFAULT NULL, `sdestination` varchar(500) DEFAULT NULL, `dpaytollamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`ipaytollid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets ( `ipaytollid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iassetid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects ( `ipaytollid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iprojectid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " VALUES (" & ipaytollid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", '00:00:00', '" & txtOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
            spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
            dpaytollamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " SET ipaytolldate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", spaytolltime = '00:00:00', sorigin = '" & txtOrigen.Text.Replace("'", "").Replace("--", "") & "', sdestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', dpaytollamount = " & importe & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipaytollid = " & ipaytollid)

            spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
            spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
            dpaytollamount = importe

        End If


        'Empieza codigo de Buscar Relacion


        If cmbTipoRelacion.SelectedItem = "" Then
            Exit Sub
        ElseIf cmbTipoRelacion.SelectedItem = "Relacionar a Activo" Then

            'Assets

            iselectedrelationtypeid = 0

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets VALUES (" & ipaytollid & ", " & iselectedrelationid & ", '', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

                iselectedrelationtypeid = 0
                iselectedrelationid = 0

            End If


        ElseIf cmbTipoRelacion.SelectedItem = "Relacionar a Proyecto" Then

            'Proyectos

            iselectedrelationtypeid = 0

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects VALUES (" & ipaytollid & ", " & iselectedrelationid & ", '', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

                iselectedrelationtypeid = 0
                iselectedrelationid = 0

            End If

        End If


        Dim queryRelacionados As String

        queryRelacionados = "" & _
        "SELECT ic.ipaytollid, CONCAT(ic.sorigin, ' a ', ic.sdestination) AS 'Caseta',  " & _
        "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
        "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ic " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets sia ON ic.ipaytollid = sia.ipaytollid " & _
        "JOIN assets a ON sia.iassetid = a.iassetid " & _
        "WHERE " & _
        "ic.ipaytollid = " & ipaytollid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets WHERE ipaytollid = " & ipaytollid & ")) " & _
        "UNION " & _
        "SELECT ic.ipaytollid, CONCAT(ic.sorigin, ' a ', ic.sdestination) AS 'Caseta',  " & _
        "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
        "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ic " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects sip ON ic.ipaytollid = sip.ipaytollid " & _
        "JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "WHERE " & _
        "ic.ipaytollid = " & ipaytollid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects WHERE ipaytollid = " & ipaytollid & ")) "

        setDataGridView(dgvConRelacion, queryRelacionados, True)

        dgvConRelacion.Columns(0).Visible = False
        dgvConRelacion.Columns(2).Visible = False
        dgvConRelacion.Columns(4).Visible = False

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

        If MsgBox("¿Realmente deseas borrar esta relación?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Caseta") = MsgBoxResult.Yes Then

            If iselectedrelationtypeid = 1 Then

                'Assets
                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets WHERE ipaytollid = " & ipaytollid & " AND iassetid = " & iselectedrelationid)

            ElseIf iselectedrelationtypeid = 2 Then

                'Projects
                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects WHERE ipaytollid = " & ipaytollid & " AND iprojectid = " & iselectedrelationid)

            End If

            Dim queryRelacionados As String

            queryRelacionados = "" & _
            "SELECT ic.ipaytollid, CONCAT(ic.sorigin, ' a ', ic.sdestination) AS 'Caseta',  " & _
            "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
            "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ic " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets sia ON ic.ipaytollid = sia.ipaytollid " & _
            "JOIN assets a ON sia.iassetid = a.iassetid " & _
            "WHERE " & _
            "ic.ipaytollid = " & ipaytollid & " " & _
            "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets WHERE ipaytollid = " & ipaytollid & ")) " & _
            "UNION " & _
            "SELECT ic.ipaytollid, CONCAT(ic.sorigin, ' a ', ic.sdestination) AS 'Caseta',  " & _
            "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
            "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ic " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects sip ON ic.ipaytollid = sip.ipaytollid " & _
            "JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "WHERE " & _
            "ic.ipaytollid = " & ipaytollid & " " & _
            "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects WHERE ipaytollid = " & ipaytollid & ")) "

            setDataGridView(dgvConRelacion, queryRelacionados, True)

            dgvConRelacion.Columns(0).Visible = False
            dgvConRelacion.Columns(2).Visible = False
            dgvConRelacion.Columns(4).Visible = False

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

        If validaCaseta(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesPaytollIsOpen As Integer = 1

        timesPaytollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid & "'")

        If timesPaytollIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Gasto por Caseta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando este Gasto por Caseta?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesPaytollIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(6) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & " SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
            queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                ipaytollid = ipaytollid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        If ipaytollid = 0 Then

            Dim queriesCreation(7) As String

            ipaytollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipaytollid) + 1 IS NULL, 1, MAX(ipaytollid) + 1) AS ipaytollid FROM paytolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ( `ipaytollid` int(11) NOT NULL AUTO_INCREMENT, `ipaytolldate` int(11) NOT NULL, `spaytolltime` varchar(11) NOT NULL, `sorigin` varchar(500) DEFAULT NULL, `sdestination` varchar(500) DEFAULT NULL, `dpaytollamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`ipaytollid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets ( `ipaytollid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iassetid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects ( `ipaytollid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iprojectid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " VALUES (" & ipaytollid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", '00:00:00', '" & txtOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
            spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
            dpaytollamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " SET ipaytolldate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", spaytolltime = '00:00:00', sorigin = '" & txtOrigen.Text.Replace("'", "").Replace("--", "") & "', sdestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', dpaytollamount = " & importe & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipaytollid = " & ipaytollid)

            spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
            spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
            dpaytollamount = importe

        End If

        Dim queries(10) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM paytolls " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc WHERE paytolls.ipaytollid = tclc.ipaytollid) "

        queries(1) = "" & _
        "UPDATE paytolls clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc ON tclc.ipaytollid = clc.ipaytollid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipaytolldate = tclc.ipaytolldate, clc.spaytolltime = tclc.spaytolltime, clc.sorigin = tclc.sorigin, clc.sdestination = tclc.sdestination, clc.dpaytollamount = tclc.dpaytollamount, clc.ipeopleid = tclc.ipeopleid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO paytolls " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytolls clc WHERE tclc.ipaytollid = clc.ipaytollid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(3) = "" & _
        "DELETE " & _
        "FROM paytollassets " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc WHERE paytollassets.ipaytollid = tclc.ipaytollid AND paytollassets.iassetid = tclc.iassetid) "

        queries(4) = "" & _
        "UPDATE paytollassets clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(5) = "" & _
        "INSERT INTO paytollassets " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytollassets clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(6) = "" & _
        "DELETE " & _
        "FROM paytollprojects " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc WHERE paytollprojects.ipaytollid = tclc.ipaytollid AND paytollprojects.iprojectid = tclc.iprojectid) "

        queries(7) = "" & _
        "UPDATE paytollprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(8) = "" & _
        "INSERT INTO paytollprojects " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytollprojects clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó este Gasto por Caseta " & ipaytollid & " : " & txtOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

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

        If MsgBox("Revisar un Caseta automáticamente guarda sus cambios. ¿Deseas guardar este Caseta ahora?", MsgBoxStyle.YesNo, "Pregunta Guardado") = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesPaytollIsOpen As Integer = 1

        timesPaytollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid & "'")

        If timesPaytollIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Gasto por Caseta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando este Gasto por Caseta?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesPaytollIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Paytoll" & ipaytollid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(6) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & " SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Assets SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid
            queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid + newIdAddition & "Projects SET ipaytollid = " & ipaytollid + newIdAddition & " WHERE ipaytollid = " & ipaytollid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                ipaytollid = ipaytollid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0

        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try

        If ipaytollid = 0 Then

            Dim queriesCreation(7) As String

            ipaytollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipaytollid) + 1 IS NULL, 1, MAX(ipaytollid) + 1) AS ipaytollid FROM paytolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " ( `ipaytollid` int(11) NOT NULL AUTO_INCREMENT, `ipaytolldate` int(11) NOT NULL, `spaytolltime` varchar(11) NOT NULL, `sorigin` varchar(500) DEFAULT NULL, `sdestination` varchar(500) DEFAULT NULL, `dpaytollamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`ipaytollid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Assets"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets ( `ipaytollid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iassetid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `assetid` (`iassetid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll0Projects"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects ( `ipaytollid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaytollid`,`iprojectid`) USING BTREE, KEY `paytollid` (`ipaytollid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " VALUES (" & ipaytollid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", '00:00:00', '" & txtOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
            spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
            dpaytollamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " SET ipaytolldate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaTicket.Value) & ", spaytolltime = '00:00:00', sorigin = '" & txtOrigen.Text.Replace("'", "").Replace("--", "") & "', sdestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', dpaytollamount = " & importe & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipaytollid = " & ipaytollid)

            spaytollorigin = txtOrigen.Text.Replace("--", "").Replace("'", "")
            spaytolldestination = txtDestino.Text.Replace("--", "").Replace("'", "")
            dpaytollamount = importe

        End If

        Dim queries(10) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM paytolls " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc WHERE paytolls.ipaytollid = tclc.ipaytollid) "

        queries(1) = "" & _
        "UPDATE paytolls clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc ON tclc.ipaytollid = clc.ipaytollid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipaytolldate = tclc.ipaytolldate, clc.spaytolltime = tclc.spaytolltime, clc.sorigin = tclc.sorigin, clc.sdestination = tclc.sdestination, clc.dpaytollamount = tclc.dpaytollamount, clc.ipeopleid = tclc.ipeopleid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO paytolls " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytolls clc WHERE tclc.ipaytollid = clc.ipaytollid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(3) = "" & _
        "DELETE " & _
        "FROM paytollassets " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc WHERE paytollassets.ipaytollid = tclc.ipaytollid AND paytollassets.iassetid = tclc.iassetid) "

        queries(4) = "" & _
        "UPDATE paytollassets clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(5) = "" & _
        "INSERT INTO paytollassets " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Assets tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytollassets clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iassetid = clc.iassetid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(6) = "" & _
        "DELETE " & _
        "FROM paytollprojects " & _
        "WHERE ipaytollid = " & ipaytollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc WHERE paytollprojects.ipaytollid = tclc.ipaytollid AND paytollprojects.iprojectid = tclc.iprojectid) "

        queries(7) = "" & _
        "UPDATE paytollprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc ON tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(8) = "" & _
        "INSERT INTO paytollprojects " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Paytoll" & ipaytollid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM paytollprojects clc WHERE tclc.ipaytollid = clc.ipaytollid AND tclc.iprojectid = clc.iprojectid AND clc.ipaytollid = " & ipaytollid & ") "

        queries(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó este Gasto por Caseta " & ipaytollid & " : " & txtOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

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

            br.srevisiondocument = "Caseta"
            br.sid = ipaytollid

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