Public Class AgregarVale

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

    Public igasvoucherid As Integer = 0
    Public iassetid As Integer = 0
    Public ipeopleid As Integer = 0

    Private isFormReadyForAction As Boolean = False

    Private iselectedgasvoucherid As Integer = 0
    Private sselectedgasvoucherdescription As String = ""
    Private iselectedgasvoucherwithoutrelationid As Integer = 0
    Private iselectedgasvoucherwithrelationid As Integer = 0
    Private iselectedrelationtypeid As Integer = 0
    Private iselectedrelationid As Integer = 0

    Private addRelationPermission As Boolean = False
    Private modifyRelationPermission As Boolean = False
    Private deleteRelationPermission As Boolean = False

    Private WithEvents txtCantidadDgvDetalle As TextBox
    Private WithEvents txtCantidadDgvDetalleConRelacion As TextBox

    Private txtCantidadDgvDetalle_OldText As String = ""
    Private txtCantidadDgvDetalleConRelacion_OldText As String = ""

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
                    btnGuardarRelaciones.Enabled = True
                End If

                If permission = "Ver Activos" Then
                    btnAuto.Enabled = True
                End If

                If permission = "Ver Lugares" Then
                    btnDestino.Enabled = True
                End If

                If permission = "Ver Personas" Then
                    btnPersona.Enabled = True
                End If

                If permission = "Ver Relaciones" = True Then
                    dgvDetalleConRelacion.Visible = True
                End If

                If permission = "Agregar Relacion" = True Then
                    btnBuscarRelacion.Enabled = True
                    addRelationPermission = True
                End If

                If permission = "Modificar Relacion" = True Then
                    dgvDetalleConRelacion.Enabled = True
                    modifyRelationPermission = True
                End If

                If permission = "Eliminar Relacion" Then
                    deleteRelationPermission = True
                    btnEliminarRelacion.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Vale de Gasolina', 'OK')")

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


    Private Sub AgregarVale_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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
        "FROM gasvouchers " & _
        "WHERE igasvoucherid = " & igasvoucherid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc WHERE gasvouchers.igasvoucherid = tclc.igasvoucherid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc JOIN gasvouchers clc ON tclc.igasvoucherid = clc.igasvoucherid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM gasvouchers clc WHERE tclc.igasvoucherid = clc.igasvoucherid AND clc.igasvoucherid = " & igasvoucherid & ") ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM gasvoucherprojects " & _
        "WHERE igasvoucherid = " & igasvoucherid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tsip WHERE gasvoucherprojects.igasvoucherid = tsip.igasvoucherid AND gasvoucherprojects.iprojectid = tsip.iprojectid) ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tsip.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tsip JOIN gasvoucherprojects sip ON tsip.igasvoucherid = sip.igasvoucherid AND tsip.iprojectid = sip.iprojectid WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tsip " & _
        "WHERE NOT EXISTS (SELECT * FROM gasvoucherprojects sip WHERE sip.igasvoucherid = tsip.igasvoucherid AND sip.iprojectid = tsip.iprojectid AND sip.igasvoucherid = " & igasvoucherid & ") ")


        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaVale(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Vale está incompleto. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesGasVoucherIsOpen As Integer = 1

            timesGasVoucherIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid & "'")

            If timesGasVoucherIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Vale de Gasolina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Vale?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesGasVoucherIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(4) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Projects"
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & " SET igasvoucherid = " & igasvoucherid + newIdAddition & " WHERE igasvoucherid = " & igasvoucherid
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Projects SET igasvoucherid = " & igasvoucherid + newIdAddition & " WHERE igasvoucherid = " & igasvoucherid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    igasvoucherid = igasvoucherid + newIdAddition
                End If

            End If


            Dim queries(6) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM gasvouchers " & _
            "WHERE igasvoucherid = " & igasvoucherid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc WHERE gasvouchers.igasvoucherid = tclc.igasvoucherid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM gasvoucherprojects " & _
            "WHERE igasvoucherid = " & igasvoucherid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc WHERE gasvoucherprojects.igasvoucherid = tclc.igasvoucherid AND gasvoucherprojects.iprojectid = tclc.iprojectid) "

            queries(2) = "" & _
            "UPDATE gasvouchers clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc ON tclc.igasvoucherid = clc.igasvoucherid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.igasvoucherfolio = tclc.igasvoucherfolio, clc.iassetid = tclc.iassetid, clc.scarmileageatrequest = tclc.scarmileageatrequest, clc.scarorigindestination = tclc.scarorigindestination, clc.svouchercomment = tclc.svouchercomment, clc.igastypeid = tclc.igastypeid, clc.dgasvoucheramount = tclc.dgasvoucheramount, clc.igasdate = tclc.igasdate, clc.sgastime = tclc.sgastime, clc.ipeopleid = tclc.ipeopleid, clc.dlitersdispensed = tclc.dlitersdispensed WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(3) = "" & _
            "UPDATE gasvoucherprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc ON tclc.igasvoucherid = clc.igasvoucherid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.iprojectid = tclc.iprojectid, clc.dliters = tclc.dliters, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(4) = "" & _
            "INSERT INTO gasvouchers " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM gasvouchers clc WHERE tclc.igasvoucherid = clc.igasvoucherid AND clc.igasvoucherid = " & igasvoucherid & ") "

            queries(5) = "" & _
            "INSERT INTO gasvoucherprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM gasvoucherprojects clc WHERE tclc.igasvoucherid = clc.igasvoucherid AND tclc.iprojectid = clc.iprojectid AND clc.igasvoucherid = " & igasvoucherid & ") "

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

        Dim queriesDelete(5) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance"
        queriesDelete(3) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Vale de Gasolina " & igasvoucherid & " : " & txtComentarios.Text.Replace("'", "").Replace("--", "") & "', 'OK')"
        queriesDelete(4) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'GasVoucher', 'Vale de Gasolina', '" & igasvoucherid & "', '" & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarVale_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarVale_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesGasVoucherIsOpen As Integer = 0

        timesGasVoucherIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid & "'")

        If timesGasVoucherIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Vale de Gasolina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo este Vale de Gasolina?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

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

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects"
            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " ( `igasvoucherid` int(11) NOT NULL AUTO_INCREMENT, `iassetid` int(11) NOT NULL, `igasvoucherfolio` int(11) NOT NULL, `igasdate` int(11) NOT NULL, `sgastime` varchar(11) NOT NULL, `scarmileageatrequest` varchar(15) DEFAULT NULL, `scarorigindestination` varchar(500) DEFAULT NULL, `svouchercomment` varchar(500) DEFAULT NULL, `ipeopleid` int(11) DEFAULT NULL, `dlitersdispensed` decimal(20,5) DEFAULT NULL, `igastypeid` int(1) DEFAULT NULL, `dgasvoucheramount` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`igasvoucherid`), KEY `assetid` (`iassetid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"
            queriesCreation(4) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects ( `igasvoucherid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `dliters` decimal(20,5) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`igasvoucherid`,`iprojectid`) USING BTREE, KEY `gasvoucherid` (`igasvoucherid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance ( `fecha` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `rendimiento` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `tipo` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `persona` varchar(500) COLLATE latin1_spanish_ci NOT NULL) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        cmbTipoGas.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM gastypes")
        cmbTipoGas.DisplayMember = "sgastype"
        cmbTipoGas.ValueMember = "igastypeid"
        cmbTipoGas.SelectedIndex = -1

        If isEdit = False Then

            txtFolio.Text = ""
            txtAuto.Text = ""
            txtDestino.Text = ""
            txtKms.Text = "000000"
            txtLitros.Text = "0.00"
            txtImporteVale.Text = "0.00"
            txtComentarios.Text = ""
            cmbTipoGas.SelectedIndex = -1

        Else

            If isRecover = False Then

                Dim queriesInsert(2) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SELECT * FROM gasvouchers WHERE igasvoucherid = " & igasvoucherid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects SELECT * FROM gasvoucherprojects WHERE igasvoucherid = " & igasvoucherid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsVale As DataSet
            dsVale = getSQLQueryAsDataset(0, "SELECT igasvoucherid, iassetid, igasvoucherfolio, igasdate, sgastime, scarmileageatrequest, scarorigindestination, svouchercomment, ipeopleid, FORMAT(dlitersdispensed, 2) AS dlitersdispensed, igastypeid, FORMAT(dgasvoucheramount, 2) AS dgasvoucheramount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " WHERE igasvoucherid = " & igasvoucherid & "")

            Try

                If dsVale.Tables(0).Rows.Count > 0 Then

                    iassetid = dsVale.Tables(0).Rows(0).Item("iassetid")

                    dtFecha.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsVale.Tables(0).Rows(0).Item("igasdate"))

                    txtFolio.Text = dsVale.Tables(0).Rows(0).Item("igasvoucherfolio")
                    txtAuto.Text = getSQLQueryAsString(0, "SELECT sassetdescription FROM assets WHERE iassetid = " & dsVale.Tables(0).Rows(0).Item("iassetid"))
                    txtKms.Text = dsVale.Tables(0).Rows(0).Item("scarmileageatrequest")
                    txtDestino.Text = dsVale.Tables(0).Rows(0).Item("scarorigindestination")
                    txtLitros.Text = dsVale.Tables(0).Rows(0).Item("dlitersdispensed")

                    cmbTipoGas.SelectedValue = dsVale.Tables(0).Rows(0).Item("igastypeid")

                    txtImporteVale.Text = dsVale.Tables(0).Rows(0).Item("dgasvoucheramount")
                    ipeopleid = dsVale.Tables(0).Rows(0).Item("ipeopleid")
                    txtPersona.Text = getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = " & dsVale.Tables(0).Rows(0).Item("ipeopleid"))

                    txtComentarios.Enabled = True
                    txtComentarios.Text = dsVale.Tables(0).Rows(0).Item("svouchercomment")

                End If

            Catch ex As Exception

            End Try

            btnRevisiones.Enabled = True
            btnRevisionesRelaciones.Enabled = True

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Vale de Gasolina " & igasvoucherid & " : " & txtComentarios.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'GasVoucher', 'Vale de Gasolina', '" & igasvoucherid & "', '" & txtDestino.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        dtFecha.Select()
        dtFecha.Focus()

        isFormReadyForAction = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick

        Dim n As New Loader

        n.isEdit = True

        n.ShowDialog()

        If n.DialogResult = Windows.Forms.DialogResult.OK Then

            NotifyIcon1.Visible = False
            fDone = True

        End If

    End Sub


    Private Sub txtFolio_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFolio.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtFolio.Text.Contains(arrayCaractProhib(carp)) Then
                txtFolio.Text = txtFolio.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtFolio.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtFolio.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtFolio.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtFolio.Text.Length

                    If longitud > (lugar + 1) Then
                        txtFolio.Text = txtFolio.Text.Substring(0, lugar) & txtFolio.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtFolio.Text = txtFolio.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtFolio.Select(txtFolio.Text.Length, 0)
        End If

        txtFolio.Text = txtFolio.Text.Replace("--", "").Replace("'", "")
        txtFolio.Text = txtFolio.Text.Trim

    End Sub


    Private Sub btnAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuto.Click

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

            iassetid = ba.iassetid
            txtAuto.Text = ba.sassetdescription

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnDestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDestino.Click

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

        bd.querystring = txtDestino.Text

        If Me.WindowState = FormWindowState.Maximized Then
            bd.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bd.ShowDialog(Me)
        Me.Visible = True

        If bd.DialogResult = Windows.Forms.DialogResult.OK Then

            txtDestino.Text = bd.sdireccion

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPersona.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        txtPersona.Text = txtPersona.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", "")

        Dim bp As New BuscaPersonas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname

        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.querystring = txtPersona.Text.Trim

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            txtPersona.Text = bp.speoplefullname
            ipeopleid = bp.ipeopleid

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtDestino_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDestino.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
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


    Private Sub txtComentarios_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtComentarios.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtComentarios.Text.Contains(arrayCaractProhib(carp)) Then
                txtComentarios.Text = txtComentarios.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtComentarios.Select(txtComentarios.Text.Length, 0)
        End If

        txtComentarios.Text = txtComentarios.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtLitros_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLitros.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtLitros.Text.Contains(arrayCaractProhib(carp)) Then
                txtLitros.Text = txtLitros.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtLitros.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtLitros.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtLitros.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtLitros.Text.Length

                    If longitud > (lugar + 1) Then
                        txtLitros.Text = txtLitros.Text.Substring(0, lugar) & txtLitros.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtLitros.Text = txtLitros.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtLitros.Select(txtLitros.Text.Length, 0)
        End If

        txtLitros.Text = txtLitros.Text.Replace("--", "").Replace("'", "")
        txtLitros.Text = txtLitros.Text.Trim

    End Sub


    Private Sub txtImporteVale_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtImporteVale.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtImporteVale.Text.Contains(arrayCaractProhib(carp)) Then
                txtImporteVale.Text = txtImporteVale.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtImporteVale.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtImporteVale.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtImporteVale.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtImporteVale.Text.Length

                    If longitud > (lugar + 1) Then
                        txtImporteVale.Text = txtImporteVale.Text.Substring(0, lugar) & txtImporteVale.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtImporteVale.Text = txtImporteVale.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtImporteVale.Select(txtImporteVale.Text.Length, 0)
        End If

        txtImporteVale.Text = txtImporteVale.Text.Replace("--", "").Replace("'", "")
        txtImporteVale.Text = txtImporteVale.Text.Trim

    End Sub


    Private Function validaVale(ByVal silent As Boolean) As Boolean

        txtComentarios.Text = txtComentarios.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        Dim folio As Double = 0.0

        Try

            folio = CDbl(txtFolio.Text)

        Catch ex As Exception

        End Try

        If txtFolio.Text = "" Or folio = 0.0 Then

            If silent = False Then
                MsgBox("¿Podrías poner el Folio/Número de Nota de Venta del Vale de Gasolina?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtFolio.Select()
            txtFolio.Focus()
            Return False

        End If

        If isEdit = False And getSQLQueryAsBoolean(0, "SELECT * FROM gasvouchers WHERE igasvoucherfolio =" & folio) = True Then

            If silent = False Then
                MsgBox("Ese Folio de Vale de Gasolina ya está registrado. No se puede registrar doble vez un folio. Verifica los números de los Vales.", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtFolio.Select()
            txtFolio.Focus()
            Return False

        End If

        If iassetid = 0 Then

            If silent = False Then
                MsgBox("¿Podrías escoger un Auto para este Vale?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            btnAuto.Focus()
            Return False

        End If


        If cmbTipoGas.SelectedIndex = -1 Then

            If silent = False Then
                MsgBox("¿Podrías escoger el tipo de Combustible de este Vale?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            cmbTipoGas.Focus()
            Return False

        End If


        Dim litros As Double = 0.0

        Try

            litros = CDbl(txtLitros.Text)

        Catch ex As Exception

        End Try

        If txtLitros.Text = "" Or litros = 0.0 Then

            If silent = False Then
                MsgBox("¿Podrías poner cuantos litros se despacharon en el Vale de Gasolina?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtLitros.Select()
            txtLitros.Focus()
            Return False

        End If


        Dim preciovale As Double = 0.0

        Try

            preciovale = CDbl(txtImporteVale.Text)

        Catch ex As Exception

        End Try

        'If txtImporteVale.Text = "" Or preciovale = 0.0 Then

        '    If silent = False Then
        '        MsgBox("¿Podrías poner el importe del Vale de Gasolina?", MsgBoxStyle.OkOnly, "Dato Faltante")
        '    End If

        '    txtLitros.Select()
        '    txtLitros.Focus()
        '    Return False

        'End If


        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click, btnCancelarRelaciones.Click

        'igasvoucherid = 0
        'iassetid = 0
        'ipeopleid = 0

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click, btnGuardarRelaciones.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaVale(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesGasVoucherIsOpen As Integer = 1

        timesGasVoucherIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid & "'")

        If timesGasVoucherIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Vale de Gasolina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Vale?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesGasVoucherIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(5) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Projects"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Performance"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & " SET igasvoucherid = " & igasvoucherid + newIdAddition & " WHERE igasvoucherid = " & igasvoucherid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Projects SET igasvoucherid = " & igasvoucherid + newIdAddition & " WHERE igasvoucherid = " & igasvoucherid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                igasvoucherid = igasvoucherid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        If igasvoucherid = 0 Then

            Dim queriesCreation(4) As String

            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0")
            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0Projects")
            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0Performance")

            igasvoucherid = getSQLQueryAsInteger(0, "SELECT IF(MAX(igasvoucherid) + 1 IS NULL, 1, MAX(igasvoucherid) + 1) AS igasvoucherid FROM gasvouchers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " ( `igasvoucherid` int(11) NOT NULL AUTO_INCREMENT, `iassetid` int(11) NOT NULL, `igasvoucherfolio` int(11) NOT NULL, `igasdate` int(11) NOT NULL, `sgastime` varchar(11) NOT NULL, `scarmileageatrequest` varchar(15) DEFAULT NULL, `scarorigindestination` varchar(500) DEFAULT NULL, `svouchercomment` varchar(500) DEFAULT NULL, `ipeopleid` int(11) DEFAULT NULL, `dlitersdispensed` decimal(20,5) DEFAULT NULL, `igastypeid` int(1) DEFAULT NULL, `dgasvoucheramount` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`igasvoucherid`), KEY `assetid` (`iassetid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects ( `igasvoucherid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `dliters` decimal(20,5) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`igasvoucherid`,`iprojectid`) USING BTREE, KEY `gasvoucherid` (`igasvoucherid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance ( `fecha` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `rendimiento` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `tipo` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `persona` varchar(500) COLLATE latin1_spanish_ci NOT NULL) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " VALUES (" & igasvoucherid & ", " & iassetid & ", " & txtFolio.Text.Replace("--", "").Replace("'", "") & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(10).Trim.Replace(".000", "") & "', '" & txtKms.Text.Replace("--", "").Replace("'", "") & "', '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', '" & txtComentarios.Text.Replace("--", "").Replace("'", "") & "', " & ipeopleid & ", " & txtLitros.Text.Replace("--", "").Replace("'", "") & ", " & cmbTipoGas.SelectedValue & ", " & txtImporteVale.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET iassetid = " & iassetid & ", igasvoucherfolio = " & txtFolio.Text.Replace("--", "").Replace("'", "") & ", igasdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sgastime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(10).Trim.Replace(".000", "") & "', svouchercomment = '" & txtComentarios.Text.Replace("--", "").Replace("'", "") & "', scarmileageatrequest = '" & txtKms.Text.Replace("--", "").Replace("'", "") & "', scarorigindestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', ipeopleid = " & ipeopleid & ", igastypeid = " & cmbTipoGas.SelectedValue & ", dlitersdispensed = " & txtLitros.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", dgasvoucheramount = " & txtImporteVale.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid & "")

        End If

        Dim queries(7) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM gasvouchers " & _
        "WHERE igasvoucherid = " & igasvoucherid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc WHERE gasvouchers.igasvoucherid = tclc.igasvoucherid) "

        queries(1) = "" & _
        "DELETE " & _
        "FROM gasvoucherprojects " & _
        "WHERE igasvoucherid = " & igasvoucherid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc WHERE gasvoucherprojects.igasvoucherid = tclc.igasvoucherid AND gasvoucherprojects.iprojectid = tclc.iprojectid) "

        queries(2) = "" & _
        "UPDATE gasvouchers clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc ON tclc.igasvoucherid = clc.igasvoucherid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.igasvoucherfolio = tclc.igasvoucherfolio, clc.iassetid = tclc.iassetid, clc.scarmileageatrequest = tclc.scarmileageatrequest, clc.scarorigindestination = tclc.scarorigindestination, clc.svouchercomment = tclc.svouchercomment, clc.igastypeid = tclc.igastypeid, clc.dgasvoucheramount = tclc.dgasvoucheramount, clc.igasdate = tclc.igasdate, clc.sgastime = tclc.sgastime, clc.ipeopleid = tclc.ipeopleid, clc.dlitersdispensed = tclc.dlitersdispensed WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(3) = "" & _
        "UPDATE gasvoucherprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc ON tclc.igasvoucherid = clc.igasvoucherid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.iprojectid = tclc.iprojectid, clc.dliters = tclc.dliters, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(4) = "" & _
        "INSERT INTO gasvouchers " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM gasvouchers clc WHERE tclc.igasvoucherid = clc.igasvoucherid AND clc.igasvoucherid = " & igasvoucherid & ") "

        queries(5) = "" & _
        "INSERT INTO gasvoucherprojects " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM gasvoucherprojects clc WHERE tclc.igasvoucherid = clc.igasvoucherid AND tclc.iprojectid = clc.iprojectid AND clc.igasvoucherid = " & igasvoucherid & ") "

        queries(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Vale de Gasolina " & igasvoucherid & " : " & txtComentarios.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then
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


    Private Sub btnRevisiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevisiones.Click, btnRevisionesRelaciones.Click

        If MsgBox("Revisar un Vale automáticamente guarda sus cambios. ¿Deseas guardar este Vale ahora?", MsgBoxStyle.YesNo, "Pregunta Guardado") = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaVale(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesGasVoucherIsOpen As Integer = 1

        timesGasVoucherIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid & "'")

        If timesGasVoucherIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Vale de Gasolina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Vale?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesGasVoucherIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(5) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Projects"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Performance"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & " SET igasvoucherid = " & igasvoucherid + newIdAddition & " WHERE igasvoucherid = " & igasvoucherid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Projects SET igasvoucherid = " & igasvoucherid + newIdAddition & " WHERE igasvoucherid = " & igasvoucherid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                igasvoucherid = igasvoucherid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        If igasvoucherid = 0 Then

            Dim queriesCreation(4) As String

            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0")
            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0Projects")
            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0Performance")

            igasvoucherid = getSQLQueryAsInteger(0, "SELECT IF(MAX(igasvoucherid) + 1 IS NULL, 1, MAX(igasvoucherid) + 1) AS igasvoucherid FROM gasvouchers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " ( `igasvoucherid` int(11) NOT NULL AUTO_INCREMENT, `iassetid` int(11) NOT NULL, `igasvoucherfolio` int(11) NOT NULL, `igasdate` int(11) NOT NULL, `sgastime` varchar(11) NOT NULL, `scarmileageatrequest` varchar(15) DEFAULT NULL, `scarorigindestination` varchar(500) DEFAULT NULL, `svouchercomment` varchar(500) DEFAULT NULL, `ipeopleid` int(11) DEFAULT NULL, `dlitersdispensed` decimal(20,5) DEFAULT NULL, `igastypeid` int(1) DEFAULT NULL, `dgasvoucheramount` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`igasvoucherid`), KEY `assetid` (`iassetid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects ( `igasvoucherid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `dliters` decimal(20,5) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`igasvoucherid`,`iprojectid`) USING BTREE, KEY `gasvoucherid` (`igasvoucherid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance ( `fecha` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `rendimiento` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `tipo` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `persona` varchar(500) COLLATE latin1_spanish_ci NOT NULL) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " VALUES (" & igasvoucherid & ", " & iassetid & ", " & txtFolio.Text.Replace("--", "").Replace("'", "") & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(10).Trim.Replace(".000", "") & "', '" & txtKms.Text.Replace("--", "").Replace("'", "") & "', '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', '" & txtComentarios.Text.Replace("--", "").Replace("'", "") & "', " & ipeopleid & ", " & txtLitros.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", " & cmbTipoGas.SelectedValue & ", " & txtImporteVale.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET iassetid = " & iassetid & ", igasvoucherfolio = " & txtFolio.Text.Replace("--", "").Replace("'", "") & ", igasdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sgastime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(10).Trim.Replace(".000", "") & "', svouchercomment = '" & txtComentarios.Text.Replace("--", "").Replace("'", "") & "', scarmileageatrequest = '" & txtKms.Text.Replace("--", "").Replace("'", "") & "', scarorigindestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', ipeopleid = " & ipeopleid & ", igastypeid = " & cmbTipoGas.SelectedValue & ", dlitersdispensed = " & txtLitros.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", dgasvoucheramount = " & txtImporteVale.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid & "")

        End If

        Dim queries(7) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM gasvouchers " & _
        "WHERE igasvoucherid = " & igasvoucherid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc WHERE gasvouchers.igasvoucherid = tclc.igasvoucherid) "

        queries(1) = "" & _
        "DELETE " & _
        "FROM gasvoucherprojects " & _
        "WHERE igasvoucherid = " & igasvoucherid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc WHERE gasvoucherprojects.igasvoucherid = tclc.igasvoucherid AND gasvoucherprojects.iprojectid = tclc.iprojectid) "

        queries(2) = "" & _
        "UPDATE gasvouchers clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc ON tclc.igasvoucherid = clc.igasvoucherid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.igasvoucherfolio = tclc.igasvoucherfolio, clc.iassetid = tclc.iassetid, clc.scarmileageatrequest = tclc.scarmileageatrequest, clc.scarorigindestination = tclc.scarorigindestination, clc.svouchercomment = tclc.svouchercomment, clc.igastypeid = tclc.igastypeid, clc.dgasvoucheramount = tclc.dgasvoucheramount, clc.igasdate = tclc.igasdate, clc.sgastime = tclc.sgastime, clc.ipeopleid = tclc.ipeopleid, clc.dlitersdispensed = tclc.dlitersdispensed WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(3) = "" & _
        "UPDATE gasvoucherprojects clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc ON tclc.igasvoucherid = clc.igasvoucherid AND tclc.iprojectid = clc.iprojectid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.iprojectid = tclc.iprojectid, clc.dliters = tclc.dliters, clc.sextranote = tclc.sextranote WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(4) = "" & _
        "INSERT INTO gasvouchers " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM gasvouchers clc WHERE tclc.igasvoucherid = clc.igasvoucherid AND clc.igasvoucherid = " & igasvoucherid & ") "

        queries(5) = "" & _
        "INSERT INTO gasvoucherprojects " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM gasvoucherprojects clc WHERE tclc.igasvoucherid = clc.igasvoucherid AND tclc.iprojectid = clc.iprojectid AND clc.igasvoucherid = " & igasvoucherid & ") "

        queries(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Vale de Gasolina " & igasvoucherid & " : " & txtComentarios.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then

            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            wasCreated = True

            Cursor.Current = System.Windows.Forms.Cursors.Default

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

            br.srevisiondocument = "Vales de Gasolina"
            br.sid = igasvoucherid

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

        wasCreated = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tcVales_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcVales.SelectedIndexChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaVale(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            tcVales.SelectedTab = tpDatos
            Exit Sub
        End If

        Dim timesGasVoucherIsOpen As Integer = 1

        timesGasVoucherIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid & "'")

        If timesGasVoucherIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Vale de Gasolina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Vale?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesGasVoucherIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%GasVoucher" & igasvoucherid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(5) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Projects"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Performance"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & " SET igasvoucherid = " & igasvoucherid + newIdAddition & " WHERE igasvoucherid = " & igasvoucherid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid + newIdAddition & "Projects SET igasvoucherid = " & igasvoucherid + newIdAddition & " WHERE igasvoucherid = " & igasvoucherid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                igasvoucherid = igasvoucherid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        If igasvoucherid = 0 Then

            Dim queriesCreation(4) As String

            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0")
            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0Projects")
            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher0Performance")

            igasvoucherid = getSQLQueryAsInteger(0, "SELECT IF(MAX(igasvoucherid) + 1 IS NULL, 1, MAX(igasvoucherid) + 1) AS igasvoucherid FROM gasvouchers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " ( `igasvoucherid` int(11) NOT NULL AUTO_INCREMENT, `iassetid` int(11) NOT NULL, `igasvoucherfolio` int(11) NOT NULL, `igasdate` int(11) NOT NULL, `sgastime` varchar(11) NOT NULL, `scarmileageatrequest` varchar(15) DEFAULT NULL, `scarorigindestination` varchar(500) DEFAULT NULL, `svouchercomment` varchar(500) DEFAULT NULL, `ipeopleid` int(11) DEFAULT NULL, `dlitersdispensed` decimal(20,5) DEFAULT NULL, `igastypeid` int(1) DEFAULT NULL, `dgasvoucheramount` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`igasvoucherid`), KEY `assetid` (`iassetid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects ( `igasvoucherid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `dliters` decimal(20,5) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`igasvoucherid`,`iprojectid`) USING BTREE, KEY `gasvoucherid` (`igasvoucherid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Performance ( `fecha` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `rendimiento` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `tipo` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `persona` varchar(500) COLLATE latin1_spanish_ci NOT NULL) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " VALUES (" & igasvoucherid & ", " & iassetid & ", " & txtFolio.Text.Replace("--", "").Replace("'", "") & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(10).Trim.Replace(".000", "") & "', '" & txtKms.Text.Replace("--", "").Replace("'", "") & "', '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', '" & txtComentarios.Text.Replace("--", "").Replace("'", "") & "', " & ipeopleid & ", " & txtLitros.Text.Replace("--", "").Replace("'", "") & ", " & cmbTipoGas.SelectedValue & ", " & txtImporteVale.Text.Replace("--", "").Replace("'", "") & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET iassetid = " & iassetid & ", igasvoucherfolio = " & txtFolio.Text.Replace("--", "").Replace("'", "") & ", igasdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sgastime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFecha.Value).Substring(10).Trim.Replace(".000", "") & "', svouchercomment = '" & txtComentarios.Text.Replace("--", "").Replace("'", "") & "', scarmileageatrequest = '" & txtKms.Text.Replace("--", "").Replace("'", "") & "', scarorigindestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', ipeopleid = " & ipeopleid & ", igastypeid = " & cmbTipoGas.SelectedValue & ", dlitersdispensed = " & txtLitros.Text.Replace("--", "").Replace("'", "") & ", dgasvoucheramount = " & txtImporteVale.Text.Replace("--", "").Replace("'", "") & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid & "")

        End If


        If validaVale(True) = True And (tcVales.SelectedTab Is tpDatos) = False Then

            'Continue


            If validaRelaciones(True) = True And (tcVales.SelectedTab Is tpDatos) = False And (tcVales.SelectedTab Is tpRelaciones) = False Then
                'Continue
            Else
                tcVales.SelectedTab = tpRelaciones
            End If


            If tcVales.SelectedTab Is tpRelaciones Then


                Dim queryNoRelacionados As String

                queryNoRelacionados = "" & _
                "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters), 2) AS 'Cantidad', " & _
                "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount, 2) AS 'Importe Total' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
                "JOIN assets a ON g.iassetid = a.iassetid " & _
                "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
                "LEFT JOIN (SELECT igasvoucherid, iprojectid, SUM(dliters) AS dliters, sextranote, iinsertiondate, sinsertiontime, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & " GROUP BY igasvoucherid) gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
                "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
                "WHERE " & _
                "g.igasvoucherid = " & igasvoucherid & " " & _
                "AND g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters) > 0 "

                setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

                dgvDetalleSinRelacion.Columns(0).Visible = False
                dgvDetalleSinRelacion.Columns(1).Visible = False

                dgvDetalleSinRelacion.Columns(0).Width = 30
                dgvDetalleSinRelacion.Columns(1).Width = 30
                dgvDetalleSinRelacion.Columns(2).Width = 200
                dgvDetalleSinRelacion.Columns(3).Width = 70
                dgvDetalleSinRelacion.Columns(4).Width = 70
                dgvDetalleSinRelacion.Columns(5).Width = 100
                dgvDetalleSinRelacion.Columns(6).Width = 70


                Dim queryRelacionados As String

                queryRelacionados = "" & _
                "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(gvp.dliters, 2) AS 'Cantidad', " & _
                "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount/g.dlitersdispensed, 2) AS 'Precio por Litro', " & _
                "FORMAT(gvp.dliters * (g.dgasvoucheramount/g.dlitersdispensed), 2) AS 'Importe Total', 1 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
                "p.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
                "JOIN assets a ON g.iassetid = a.iassetid " & _
                "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
                "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
                "WHERE " & _
                "g.igasvoucherid = " & igasvoucherid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & ")) ORDER BY iinsertiondate ASC, sinsertiontime ASC "

                setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

                dgvDetalleConRelacion.Columns(0).Visible = False
                dgvDetalleConRelacion.Columns(1).Visible = False
                dgvDetalleConRelacion.Columns(8).Visible = False
                dgvDetalleConRelacion.Columns(10).Visible = False

                dgvDetalleConRelacion.Columns(0).ReadOnly = True
                dgvDetalleConRelacion.Columns(1).ReadOnly = True
                dgvDetalleConRelacion.Columns(2).ReadOnly = True
                dgvDetalleConRelacion.Columns(3).ReadOnly = False
                dgvDetalleConRelacion.Columns(4).ReadOnly = True
                dgvDetalleConRelacion.Columns(5).ReadOnly = True
                dgvDetalleConRelacion.Columns(6).ReadOnly = True
                dgvDetalleConRelacion.Columns(7).ReadOnly = True
                dgvDetalleConRelacion.Columns(8).ReadOnly = True
                dgvDetalleConRelacion.Columns(9).ReadOnly = True
                dgvDetalleConRelacion.Columns(10).ReadOnly = True
                dgvDetalleConRelacion.Columns(11).ReadOnly = True

                dgvDetalleConRelacion.Columns(0).Width = 30
                dgvDetalleConRelacion.Columns(1).Width = 30
                dgvDetalleConRelacion.Columns(2).Width = 200
                dgvDetalleConRelacion.Columns(3).Width = 70
                dgvDetalleConRelacion.Columns(4).Width = 70
                dgvDetalleConRelacion.Columns(5).Width = 100
                dgvDetalleConRelacion.Columns(6).Width = 70
                dgvDetalleConRelacion.Columns(7).Width = 70
                dgvDetalleConRelacion.Columns(8).Width = 30
                dgvDetalleConRelacion.Columns(9).Width = 100
                dgvDetalleConRelacion.Columns(10).Width = 30
                dgvDetalleConRelacion.Columns(11).Width = 300



                cmbTipoRelacion.SelectedIndex = 0



                'ElseIf tcVales.SelectedTab Is tpPagos Then


                '    Dim querysupplierinvoicepayments As String
                '    querysupplierinvoicepayments = "" & _
                '    "SELECT sipy.ipaymentid, sipy.isupplierinvoiceid, " & _
                '    "STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
                '    "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
                '    "py.spaymentdescription AS 'Descripcion', sipy.ssupplierinvoiceextranote AS Observaciones " & _
                '    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy " & _
                '    "JOIN payments py ON sipy.ipaymentid = py.ipaymentid " & _
                '    "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                '    "WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid

                '    setDataGridView(dgvPagos, querysupplierinvoicepayments, False)

                '    dgvPagos.Columns(0).Visible = False
                '    dgvPagos.Columns(1).Visible = False

                '    dgvPagos.Columns(0).ReadOnly = True
                '    dgvPagos.Columns(1).ReadOnly = True
                '    dgvPagos.Columns(2).ReadOnly = True
                '    dgvPagos.Columns(3).ReadOnly = True
                '    dgvPagos.Columns(4).ReadOnly = True
                '    dgvPagos.Columns(5).ReadOnly = True

                '    dgvPagos.Columns(0).Width = 30
                '    dgvPagos.Columns(1).Width = 30
                '    dgvPagos.Columns(2).Width = 70
                '    dgvPagos.Columns(3).Width = 70
                '    dgvPagos.Columns(4).Width = 100
                '    dgvPagos.Columns(5).Width = 200
                '    dgvPagos.Columns(6).Width = 200

                '    txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                '    txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)) + IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            End If


        Else

            tcVales.SelectedTab = tpDatos

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvDetalleSinRelacion_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleSinRelacion.CellClick

        Try

            If dgvDetalleSinRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedgasvoucherwithoutrelationid = CInt(dgvDetalleSinRelacion.Rows(e.RowIndex).Cells(0).Value())

        Catch ex As Exception

            iselectedgasvoucherwithoutrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleSinRelacion_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleSinRelacion.CellContentClick

        Try

            If dgvDetalleSinRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedgasvoucherwithoutrelationid = CInt(dgvDetalleSinRelacion.Rows(e.RowIndex).Cells(0).Value())

        Catch ex As Exception

            iselectedgasvoucherwithoutrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleSinRelacion_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDetalleSinRelacion.SelectionChanged

        Try

            If dgvDetalleSinRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedgasvoucherwithoutrelationid = CInt(dgvDetalleSinRelacion.CurrentRow.Cells(0).Value())

        Catch ex As Exception

            iselectedgasvoucherwithoutrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleConRelacion_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleConRelacion.CellClick

        Try

            If dgvDetalleConRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedgasvoucherwithrelationid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(0).Value())
            iselectedrelationtypeid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(8).Value())
            iselectedrelationid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(10).Value())

        Catch ex As Exception

            iselectedgasvoucherwithrelationid = 0
            iselectedrelationtypeid = 0
            iselectedrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleConRelacion_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleConRelacion.CellContentClick

        Try

            If dgvDetalleConRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedgasvoucherwithrelationid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(0).Value())
            iselectedrelationtypeid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(8).Value())
            iselectedrelationid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(10).Value())

        Catch ex As Exception

            iselectedgasvoucherwithrelationid = 0
            iselectedrelationtypeid = 0
            iselectedrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleConRelacion_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDetalleConRelacion.SelectionChanged

        Try

            If dgvDetalleConRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedgasvoucherwithrelationid = CInt(dgvDetalleConRelacion.CurrentRow.Cells(0).Value())
            iselectedrelationtypeid = CInt(dgvDetalleConRelacion.CurrentRow.Cells(8).Value())
            iselectedrelationid = CInt(dgvDetalleConRelacion.CurrentRow.Cells(10).Value())

        Catch ex As Exception

            iselectedgasvoucherwithrelationid = 0
            iselectedrelationtypeid = 0
            iselectedrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleConRelacion_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvDetalleConRelacion.KeyUp

        If iselectedrelationtypeid = 0 Then
            Exit Sub
        End If

        If e.KeyCode = Keys.Delete Then

            If deleteRelationPermission = False Then
                Exit Sub
            End If

            If iselectedrelationtypeid = 0 Then
                Exit Sub
            End If

            If MsgBox("¿Realmente deseas borrar esta relación?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Insumo Factura") = MsgBoxResult.Yes Then

                'Projects
                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & " AND iprojectid = " & iselectedrelationid)

                Dim queryNoRelacionados As String

                queryNoRelacionados = "" & _
                "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters), 2) AS 'Cantidad', " & _
                "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount, 2) AS 'Importe Total' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
                "JOIN assets a ON g.iassetid = a.iassetid " & _
                "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
                "LEFT JOIN (SELECT igasvoucherid, iprojectid, SUM(dliters) AS dliters, sextranote, iinsertiondate, sinsertiontime, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & " GROUP BY igasvoucherid) gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
                "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
                "WHERE " & _
                "g.igasvoucherid = " & igasvoucherid & " " & _
                "AND g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters) > 0 "

                setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

                dgvDetalleSinRelacion.Columns(0).Visible = False
                dgvDetalleSinRelacion.Columns(1).Visible = False

                dgvDetalleSinRelacion.Columns(0).Width = 30
                dgvDetalleSinRelacion.Columns(1).Width = 30
                dgvDetalleSinRelacion.Columns(2).Width = 200
                dgvDetalleSinRelacion.Columns(3).Width = 70
                dgvDetalleSinRelacion.Columns(4).Width = 70
                dgvDetalleSinRelacion.Columns(5).Width = 100
                dgvDetalleSinRelacion.Columns(6).Width = 70


                Dim queryRelacionados As String

                queryRelacionados = "" & _
                "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(gvp.dliters, 2) AS 'Cantidad', " & _
                "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount/g.dlitersdispensed, 2) AS 'Precio por Litro', " & _
                "FORMAT(gvp.dliters * (g.dgasvoucheramount/g.dlitersdispensed), 2) AS 'Importe Total', 1 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
                "p.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
                "JOIN assets a ON g.iassetid = a.iassetid " & _
                "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
                "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
                "WHERE " & _
                "g.igasvoucherid = " & igasvoucherid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & ")) ORDER BY iinsertiondate ASC, sinsertiontime ASC "

                setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

                dgvDetalleConRelacion.Columns(0).Visible = False
                dgvDetalleConRelacion.Columns(1).Visible = False
                dgvDetalleConRelacion.Columns(8).Visible = False
                dgvDetalleConRelacion.Columns(10).Visible = False

                dgvDetalleConRelacion.Columns(0).ReadOnly = True
                dgvDetalleConRelacion.Columns(1).ReadOnly = True
                dgvDetalleConRelacion.Columns(2).ReadOnly = True
                dgvDetalleConRelacion.Columns(3).ReadOnly = False
                dgvDetalleConRelacion.Columns(4).ReadOnly = True
                dgvDetalleConRelacion.Columns(5).ReadOnly = True
                dgvDetalleConRelacion.Columns(6).ReadOnly = True
                dgvDetalleConRelacion.Columns(7).ReadOnly = True
                dgvDetalleConRelacion.Columns(8).ReadOnly = True
                dgvDetalleConRelacion.Columns(9).ReadOnly = True
                dgvDetalleConRelacion.Columns(10).ReadOnly = True
                dgvDetalleConRelacion.Columns(11).ReadOnly = True

                dgvDetalleConRelacion.Columns(0).Width = 30
                dgvDetalleConRelacion.Columns(1).Width = 30
                dgvDetalleConRelacion.Columns(2).Width = 200
                dgvDetalleConRelacion.Columns(3).Width = 70
                dgvDetalleConRelacion.Columns(4).Width = 70
                dgvDetalleConRelacion.Columns(5).Width = 100
                dgvDetalleConRelacion.Columns(6).Width = 70
                dgvDetalleConRelacion.Columns(7).Width = 70
                dgvDetalleConRelacion.Columns(8).Width = 30
                dgvDetalleConRelacion.Columns(9).Width = 100
                dgvDetalleConRelacion.Columns(10).Width = 30
                dgvDetalleConRelacion.Columns(11).Width = 300

            End If

        End If

    End Sub


    Private Sub dgvDetalleConRelacion_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvDetalleConRelacion.EditingControlShowing

        If dgvDetalleConRelacion.CurrentCell.ColumnIndex = 3 Then

            txtCantidadDgvDetalleConRelacion = CType(e.Control, TextBox)
            txtCantidadDgvDetalleConRelacion_OldText = txtCantidadDgvDetalleConRelacion.Text

        Else

            txtCantidadDgvDetalleConRelacion = Nothing
            txtCantidadDgvDetalleConRelacion_OldText = Nothing

        End If

    End Sub


    Private Sub txtCantidadDgvDetalleConRelacion_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidadDgvDetalleConRelacion.KeyUp

        If Not IsNumeric(txtCantidadDgvDetalleConRelacion.Text) Then

            Dim strForbidden4 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden4 As Char() = strForbidden4.ToCharArray

            For cp = 0 To arrayForbidden4.Length - 1

                If txtCantidadDgvDetalleConRelacion.Text.Contains(arrayForbidden4(cp)) Then
                    txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Replace(arrayForbidden4(cp), "")
                End If

            Next cp

            If txtCantidadDgvDetalleConRelacion.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidadDgvDetalleConRelacion.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidadDgvDetalleConRelacion.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidadDgvDetalleConRelacion.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Substring(0, lugar) & txtCantidadDgvDetalleConRelacion.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Replace("--", "").Replace("'", "")
            txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Trim

        Else
            txtCantidadDgvDetalleConRelacion_OldText = txtCantidadDgvDetalleConRelacion.Text
        End If

    End Sub


    Private Sub dgvDetalleConRelacion_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleConRelacion.CellValueChanged

        If modifyRelationPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LA UNICA COLUMNA EDITABLE ES LA 3: DQTY

        If e.ColumnIndex = 3 Then

            If dgvDetalleConRelacion.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Realmente deseas borrar esta relación?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Vale Proyecto") = MsgBoxResult.Yes Then

                    'Projects
                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & " AND iprojectid = " & iselectedrelationid)

                Else

                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                End If

            Else

                Dim cantidadrelacionada As Double = 0.0

                Dim queryVale As String = ""

                Try

                    cantidadrelacionada = CDbl(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(3).Value)

                Catch ex As Exception

                End Try

                queryVale = "" & _
                "SELECT g.dlitersdispensed as litros " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects gvp ON gvp.igasvoucherid = g.igasvoucherid " & _
                "WHERE g.igasvoucherid = " & igasvoucherid

                If cantidadrelacionada > getSQLQueryAsDouble(0, queryVale) Then
                    Exit Sub
                End If

                'Projects
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects SET dliters = " & cantidadrelacionada & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid & " AND iprojectid = " & iselectedrelationid)


            End If


        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvDetalleConRelacion_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleConRelacion.CellEndEdit

        Dim queryNoRelacionados As String

        queryNoRelacionados = "" & _
        "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters), 2) AS 'Cantidad', " & _
        "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount, 2) AS 'Importe Total' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "LEFT JOIN (SELECT igasvoucherid, iprojectid, SUM(dliters) AS dliters, sextranote, iinsertiondate, sinsertiontime, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & " GROUP BY igasvoucherid) gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
        "WHERE " & _
        "g.igasvoucherid = " & igasvoucherid & " " & _
        "AND g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters) > 0 "

        setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

        dgvDetalleSinRelacion.Columns(0).Visible = False
        dgvDetalleSinRelacion.Columns(1).Visible = False

        dgvDetalleSinRelacion.Columns(0).Width = 30
        dgvDetalleSinRelacion.Columns(1).Width = 30
        dgvDetalleSinRelacion.Columns(2).Width = 200
        dgvDetalleSinRelacion.Columns(3).Width = 70
        dgvDetalleSinRelacion.Columns(4).Width = 70
        dgvDetalleSinRelacion.Columns(5).Width = 100
        dgvDetalleSinRelacion.Columns(6).Width = 70


        Dim queryRelacionados As String

        queryRelacionados = "" & _
        "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(gvp.dliters, 2) AS 'Cantidad', " & _
        "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount/g.dlitersdispensed, 2) AS 'Precio por Litro', " & _
        "FORMAT(gvp.dliters * (g.dgasvoucheramount/g.dlitersdispensed), 2) AS 'Importe Total', 1 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
        "p.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
        "WHERE " & _
        "g.igasvoucherid = " & igasvoucherid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & ")) ORDER BY iinsertiondate ASC, sinsertiontime ASC "

        setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

        dgvDetalleConRelacion.Columns(0).Visible = False
        dgvDetalleConRelacion.Columns(1).Visible = False
        dgvDetalleConRelacion.Columns(8).Visible = False
        dgvDetalleConRelacion.Columns(10).Visible = False

        dgvDetalleConRelacion.Columns(0).ReadOnly = True
        dgvDetalleConRelacion.Columns(1).ReadOnly = True
        dgvDetalleConRelacion.Columns(2).ReadOnly = True
        dgvDetalleConRelacion.Columns(3).ReadOnly = False
        dgvDetalleConRelacion.Columns(4).ReadOnly = True
        dgvDetalleConRelacion.Columns(5).ReadOnly = True
        dgvDetalleConRelacion.Columns(6).ReadOnly = True
        dgvDetalleConRelacion.Columns(7).ReadOnly = True
        dgvDetalleConRelacion.Columns(8).ReadOnly = True
        dgvDetalleConRelacion.Columns(9).ReadOnly = True
        dgvDetalleConRelacion.Columns(10).ReadOnly = True
        dgvDetalleConRelacion.Columns(11).ReadOnly = True

        dgvDetalleConRelacion.Columns(0).Width = 30
        dgvDetalleConRelacion.Columns(1).Width = 30
        dgvDetalleConRelacion.Columns(2).Width = 200
        dgvDetalleConRelacion.Columns(3).Width = 70
        dgvDetalleConRelacion.Columns(4).Width = 70
        dgvDetalleConRelacion.Columns(5).Width = 100
        dgvDetalleConRelacion.Columns(6).Width = 70
        dgvDetalleConRelacion.Columns(7).Width = 70
        dgvDetalleConRelacion.Columns(8).Width = 30
        dgvDetalleConRelacion.Columns(9).Width = 100
        dgvDetalleConRelacion.Columns(10).Width = 30
        dgvDetalleConRelacion.Columns(11).Width = 300

    End Sub


    Private Sub btnEliminarRelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarRelacion.Click

        If iselectedrelationtypeid = 0 Then
            Exit Sub
        End If

        If MsgBox("¿Realmente deseas borrar esta relación?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Insumo Factura") = MsgBoxResult.Yes Then

            'Projects
            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & " AND iprojectid = " & iselectedrelationid)

            Dim queryNoRelacionados As String

            queryNoRelacionados = "" & _
            "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters), 2) AS 'Cantidad', " & _
            "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount, 2) AS 'Importe Total' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
            "JOIN assets a ON g.iassetid = a.iassetid " & _
            "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
            "LEFT JOIN (SELECT igasvoucherid, iprojectid, SUM(dliters) AS dliters, sextranote, iinsertiondate, sinsertiontime, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & " GROUP BY igasvoucherid) gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
            "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
            "WHERE " & _
            "g.igasvoucherid = " & igasvoucherid & " " & _
            "AND g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters) > 0 "

            setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

            dgvDetalleSinRelacion.Columns(0).Visible = False
            dgvDetalleSinRelacion.Columns(1).Visible = False

            dgvDetalleSinRelacion.Columns(0).Width = 30
            dgvDetalleSinRelacion.Columns(1).Width = 30
            dgvDetalleSinRelacion.Columns(2).Width = 200
            dgvDetalleSinRelacion.Columns(3).Width = 70
            dgvDetalleSinRelacion.Columns(4).Width = 70
            dgvDetalleSinRelacion.Columns(5).Width = 100
            dgvDetalleSinRelacion.Columns(6).Width = 70


            Dim queryRelacionados As String

            queryRelacionados = "" & _
            "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(gvp.dliters, 2) AS 'Cantidad', " & _
            "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount/g.dlitersdispensed, 2) AS 'Precio por Litro', " & _
            "FORMAT(gvp.dliters * (g.dgasvoucheramount/g.dlitersdispensed), 2) AS 'Importe Total', 1 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
            "p.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
            "JOIN assets a ON g.iassetid = a.iassetid " & _
            "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
            "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
            "WHERE " & _
            "g.igasvoucherid = " & igasvoucherid & " " & _
            "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & "))  ORDER BY iinsertiondate ASC, sinsertiontime ASC "

            setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

            dgvDetalleConRelacion.Columns(0).Visible = False
            dgvDetalleConRelacion.Columns(1).Visible = False
            dgvDetalleConRelacion.Columns(8).Visible = False
            dgvDetalleConRelacion.Columns(10).Visible = False

            dgvDetalleConRelacion.Columns(0).ReadOnly = True
            dgvDetalleConRelacion.Columns(1).ReadOnly = True
            dgvDetalleConRelacion.Columns(2).ReadOnly = True
            dgvDetalleConRelacion.Columns(3).ReadOnly = False
            dgvDetalleConRelacion.Columns(4).ReadOnly = True
            dgvDetalleConRelacion.Columns(5).ReadOnly = True
            dgvDetalleConRelacion.Columns(6).ReadOnly = True
            dgvDetalleConRelacion.Columns(7).ReadOnly = True
            dgvDetalleConRelacion.Columns(8).ReadOnly = True
            dgvDetalleConRelacion.Columns(9).ReadOnly = True
            dgvDetalleConRelacion.Columns(10).ReadOnly = True
            dgvDetalleConRelacion.Columns(11).ReadOnly = True

            dgvDetalleConRelacion.Columns(0).Width = 30
            dgvDetalleConRelacion.Columns(1).Width = 30
            dgvDetalleConRelacion.Columns(2).Width = 200
            dgvDetalleConRelacion.Columns(3).Width = 70
            dgvDetalleConRelacion.Columns(4).Width = 70
            dgvDetalleConRelacion.Columns(5).Width = 100
            dgvDetalleConRelacion.Columns(6).Width = 70
            dgvDetalleConRelacion.Columns(7).Width = 70
            dgvDetalleConRelacion.Columns(8).Width = 30
            dgvDetalleConRelacion.Columns(9).Width = 100
            dgvDetalleConRelacion.Columns(10).Width = 30
            dgvDetalleConRelacion.Columns(11).Width = 300

        End If

    End Sub


    Private Sub btnBuscarRelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarRelacion.Click

        If cmbTipoRelacion.SelectedItem = "" Then
            Exit Sub
        ElseIf cmbTipoRelacion.SelectedItem = "Relacionar a Proyecto" Then

            'Proyectos

            iselectedgasvoucherwithrelationid = 0
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

                iselectedrelationtypeid = 1
                iselectedrelationid = bi.iprojectid

                Dim fecha As Integer = getMySQLDate()
                Dim hora As String = getMySQLTime()

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects VALUES (" & igasvoucherid & ", " & iselectedrelationid & ", 0, '', " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', '" & susername & "')")

                iselectedgasvoucherwithrelationid = igasvoucherid

                txtComentarios.Enabled = True

            End If

        End If

        Dim queryNoRelacionados As String

        queryNoRelacionados = "" & _
        "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters), 2) AS 'Cantidad', " & _
        "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount, 2) AS 'Importe Total' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "LEFT JOIN (SELECT igasvoucherid, iprojectid, SUM(dliters) AS dliters, sextranote, iinsertiondate, sinsertiontime, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & " GROUP BY igasvoucherid) gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
        "WHERE " & _
        "g.igasvoucherid = " & igasvoucherid & " " & _
        "AND g.dlitersdispensed - IF(gvp.dliters IS NULL, 0, gvp.dliters) > 0 "

        setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

        dgvDetalleSinRelacion.Columns(0).Visible = False
        dgvDetalleSinRelacion.Columns(1).Visible = False

        dgvDetalleSinRelacion.Columns(0).Width = 30
        dgvDetalleSinRelacion.Columns(1).Width = 30
        dgvDetalleSinRelacion.Columns(2).Width = 200
        dgvDetalleSinRelacion.Columns(3).Width = 70
        dgvDetalleSinRelacion.Columns(4).Width = 70
        dgvDetalleSinRelacion.Columns(5).Width = 100
        dgvDetalleSinRelacion.Columns(6).Width = 70


        Dim queryRelacionados As String

        queryRelacionados = "" & _
        "SELECT g.igasvoucherid, g.iassetid, a.sassetdescription AS 'Activo', FORMAT(gvp.dliters, 2) AS 'Cantidad', " & _
        "'Litros', gt.sgastype AS 'Tipo Combustible', FORMAT(g.dgasvoucheramount/g.dlitersdispensed, 2) AS 'Precio por Litro', " & _
        "FORMAT(gvp.dliters * (g.dgasvoucheramount/g.dlitersdispensed), 2) AS 'Importe Total', 1 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
        "p.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " g " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects gvp ON g.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON p.iprojectid = gvp.iprojectid " & _
        "WHERE " & _
        "g.igasvoucherid = " & igasvoucherid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid & "))  ORDER BY iinsertiondate ASC, sinsertiontime ASC "

        setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

        dgvDetalleConRelacion.Columns(0).Visible = False
        dgvDetalleConRelacion.Columns(1).Visible = False
        dgvDetalleConRelacion.Columns(8).Visible = False
        dgvDetalleConRelacion.Columns(10).Visible = False

        dgvDetalleConRelacion.Columns(0).ReadOnly = True
        dgvDetalleConRelacion.Columns(1).ReadOnly = True
        dgvDetalleConRelacion.Columns(2).ReadOnly = True
        dgvDetalleConRelacion.Columns(3).ReadOnly = False
        dgvDetalleConRelacion.Columns(4).ReadOnly = True
        dgvDetalleConRelacion.Columns(5).ReadOnly = True
        dgvDetalleConRelacion.Columns(6).ReadOnly = True
        dgvDetalleConRelacion.Columns(7).ReadOnly = True
        dgvDetalleConRelacion.Columns(8).ReadOnly = True
        dgvDetalleConRelacion.Columns(9).ReadOnly = True
        dgvDetalleConRelacion.Columns(10).ReadOnly = True
        dgvDetalleConRelacion.Columns(11).ReadOnly = True

        dgvDetalleConRelacion.Columns(0).Width = 30
        dgvDetalleConRelacion.Columns(1).Width = 30
        dgvDetalleConRelacion.Columns(2).Width = 200
        dgvDetalleConRelacion.Columns(3).Width = 70
        dgvDetalleConRelacion.Columns(4).Width = 70
        dgvDetalleConRelacion.Columns(5).Width = 100
        dgvDetalleConRelacion.Columns(6).Width = 70
        dgvDetalleConRelacion.Columns(7).Width = 70
        dgvDetalleConRelacion.Columns(8).Width = 30
        dgvDetalleConRelacion.Columns(9).Width = 100
        dgvDetalleConRelacion.Columns(10).Width = 30
        dgvDetalleConRelacion.Columns(11).Width = 300

    End Sub


    Private Function validaRelaciones(ByVal silent As Boolean) As Boolean

        Dim totalqty As Double = 0.0
        Dim projectsqty As Double = 0.0
        
        Dim resta As Double = 0.0

        totalqty = getSQLQueryAsDouble(0, "SELECT SUM(dlitersdispensed) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " WHERE igasvoucherid = " & igasvoucherid)
        projectsqty = getSQLQueryAsDouble(0, "SELECT IF(SUM(dliters) IS NULL, 0, SUM(dliters)) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & "Projects WHERE igasvoucherid = " & igasvoucherid)
        
        resta = totalqty - projectsqty

        If resta < 0.0 Then

            If silent = False Then
                MsgBox("Estás intentando asignar más litros de los que tiene este vale...", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        Return True

    End Function


    Private Sub txtFolio_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolio.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Or (isEdit = False And igasvoucherid > 0) Then

            If txtFolio.Text.Replace("--", "").Replace("'", "").Trim = "" Then
                Exit Sub
            End If

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET igasvoucherfolio = " & txtFolio.Text.Replace("--", "").Replace("'", "") & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid)

        End If

    End Sub


    Private Sub txtKms_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKms.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Or (isEdit = False And igasvoucherid > 0) Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET scarmileageatrequest = '" & txtKms.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid)

        End If

    End Sub


    Private Sub txtDestino_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDestino.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Or (isEdit = False And igasvoucherid > 0) Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET scarorigindestination = '" & txtDestino.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid)

        End If

    End Sub


    Private Sub txtLitros_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLitros.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Or (isEdit = False And igasvoucherid > 0) Then

            If txtLitros.Text.Replace("--", "").Replace("'", "").Replace(",", "").Trim = "" Then
                Exit Sub
            End If

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET dlitersdispensed = " & txtLitros.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid)

        End If

    End Sub


    Private Sub txtImporteVale_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtImporteVale.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Or (isEdit = False And igasvoucherid > 0) Then

            If txtImporteVale.Text.Replace("--", "").Replace("'", "").Replace(",", "").Trim = "" Then
                Exit Sub
            End If

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET dgasvoucheramount = " & txtImporteVale.Text.Replace("--", "").Replace("'", "").Replace(",", "") & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid)

        End If

    End Sub


    Private Sub txtComentarios_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtComentarios.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Or (isEdit = False And igasvoucherid > 0) Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "GasVoucher" & igasvoucherid & " SET svouchercomment = '" & txtComentarios.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE igasvoucherid = " & igasvoucherid)

        End If

    End Sub

End Class