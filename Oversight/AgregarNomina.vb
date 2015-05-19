Public Class AgregarNomina

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

    Public ipayrollid As Integer = 0
    Public iprojectid As Integer = 0
    Public iclientid As Integer = 0
    Public isupervisorid As Integer = 0

    Private iselectedpeopleid As Integer = 0
    Private dselecteddaysworked As Double = 0.0
    Private dselecteddaysalary As Double = 0.0
    Private dselectedextrahoursworked As Double = 0.0
    Private dselectedextrahoursalary As Double = 0.0
    Private sselecteddiscount As String = ""
    Private dselecteddiscountamount As Double = 0.0

    Private iselectedpaymentid As Integer = 0

    Private WithEvents txtNumeroDgvNominas As TextBox
    Private WithEvents txtNotaDgvNominas As TextBox
    Private txtNotaDgvNominas_OldText As String = ""
    Private txtNumeroDgvNominas_OldText As String = ""
    
    Private openPermission As Boolean = False

    Private addPersonPermission As Boolean = False
    Private modifyPersonPermission As Boolean = False
    Private deletePersonPermission As Boolean = False

    Private openPaymentPermission As Boolean = False
    Private addPaymentPermission As Boolean = False
    Private insertPaymentPermission As Boolean = False
    Private modifyPaymentPermission As Boolean = False
    Private deletePaymentPermission As Boolean = False

    Private isFormReadyForAction As Boolean = False

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
                    btnGuardarPago.Enabled = True
                    btnPaso2.Enabled = True
                End If

                If permission = "Agregar Persona" Then
                    addPersonPermission = True
                End If

                If permission = "Modificar Persona" Then
                    modifyPersonPermission = True
                End If

                If permission = "Eliminar Persona" Then
                    deletePersonPermission = True
                End If

                If permission = "Ver Pagos" Then
                    dgvPagos.Visible = True
                    lblSumaPagos.Visible = True
                    txtSumaPagos.Visible = True
                    lblRestanteAPagar.Visible = True
                    txtMontoRestante.Visible = True
                End If

                If permission = "Abrir Pago" Then
                    openPaymentPermission = True
                End If

                If permission = "Modificar Pago" Then
                    modifyPaymentPermission = True
                    dgvPagos.Enabled = True
                End If

                If permission = "Nuevo Pago" Then
                    addPaymentPermission = True
                    btnAgregarPago.Enabled = True
                End If

                If permission = "Insertar Pago" Then
                    insertPaymentPermission = True
                    btnInsertarPago.Enabled = True
                End If

                If permission = "Eliminar Pago" Then
                    deletePaymentPermission = True
                    btnEliminarPago.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Nomina', 'OK')")

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


    Private Sub AgregarNomina_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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
        "FROM payrolls " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp WHERE payrolls.ipayrollid = tp.ipayrollid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM payrollpeople " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp WHERE payrollpeople.ipayrollid = tp.ipayrollid AND payrollpeople.ipeopleid = tp.ipeopleid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM payrollpayments " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp WHERE payrollpayments.ipayrollid = tp.ipayrollid AND payrollpayments.ipaymentid = tp.ipaymentid) ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp JOIN payrolls p ON tp.ipayrollid = p.ipayrollid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp JOIN payrollpeople p ON tp.ipayrollid = p.ipayrollid AND tp.ipeopleid = p.ipeopleid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp JOIN payrollpayments p ON tp.ipayrollid = p.ipayrollid AND tp.ipaymentid = p.ipaymentid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo7 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrolls p WHERE p.ipayrollid = tp.ipayrollid AND p.ipayrollid = " & ipayrollid & ") ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrollpeople p WHERE p.ipayrollid = tp.ipayrollid AND p.ipeopleid = tp.ipeopleid AND p.ipayrollid = " & ipayrollid & ") ")

        conteo9 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrollpayments p WHERE p.ipayrollid = tp.ipayrollid AND p.ipaymentid = tp.ipaymentid AND p.ipayrollid = " & ipayrollid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 + conteo7 + conteo8 + conteo9 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0


        If validaNominaCompleta(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Los datos de esta Nómina están incompletos. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesPayrollIsOpen As Integer = 1

            timesPayrollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payroll" & ipayrollid & "'")

            If timesPayrollIsOpen > 1 Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Nómina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando esta Nómina?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            End If

            Dim queriesSave(10) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM payrolls " & _
            "WHERE ipayrollid = " & ipayrollid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp WHERE payrolls.ipayrollid = tp.ipayrollid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM payrollpeople " & _
            "WHERE ipayrollid = " & ipayrollid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp WHERE payrollpeople.ipayrollid = tp.ipayrollid AND payrollpeople.ipeopleid = tp.ipeopleid) "

            queriesSave(2) = "" & _
            "DELETE " & _
            "FROM payrollpayments " & _
            "WHERE ipayrollid = " & ipayrollid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp WHERE payrollpayments.ipayrollid = tp.ipayrollid AND payrollpayments.ipaymentid = tp.ipaymentid) "

            queriesSave(3) = "" & _
            "UPDATE payrolls p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp ON tp.ipayrollid = p.ipayrollid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.spayrollfrequency = tp.spayrollfrequency, p.spayrolltype = tp.spayrolltype, p.ipayrolldate = tp.ipayrolldate, p.spayrolltime = tp.spayrolltime, p.ipayrollstartdate = tp.ipayrollstartdate, p.spayrollstarttime = tp.spayrollstarttime, p.ipayrollenddate = tp.ipayrollenddate, p.spayrollendtime = tp.spayrollendtime, p.ipeopleid = tp.ipeopleid, p.iprojectid = tp.iprojectid, p.spayrolldescription = tp.spayrolldescription WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE payrollpeople p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp ON tp.ipayrollid = p.ipayrollid AND tp.ipeopleid = p.ipeopleid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.ddaysworked = tp.ddaysworked, p.ddaysalary = tp.ddaysalary, p.dextrahours = tp.dextrahours, p.dextrahoursalary = tp.dextrahoursalary, p.sdiscount = tp.sdiscount, p.ddiscountamount = tp.ddiscountamount, p.sextraincome = tp.sextraincome, p.dextraincomeamount = tp.dextraincomeamount WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(5) = "" & _
            "UPDATE payrollpayments p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp ON tp.ipayrollid = p.ipayrollid AND tp.ipaymentid = p.ipaymentid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sextranote = tp.sextranote WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO payrolls " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp " & _
            "WHERE NOT EXISTS (SELECT * FROM payrolls p WHERE p.ipayrollid = tp.ipayrollid AND p.ipayrollid = " & ipayrollid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO payrollpeople " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp " & _
            "WHERE NOT EXISTS (SELECT * FROM payrollpeople p WHERE p.ipayrollid = tp.ipayrollid AND p.ipeopleid = tp.ipeopleid AND p.ipayrollid = " & ipayrollid & ") "

            queriesSave(8) = "" & _
            "INSERT INTO payrollpayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp " & _
            "WHERE NOT EXISTS (SELECT * FROM payrollpayments p WHERE p.ipayrollid = tp.ipayrollid AND p.ipaymentid = tp.ipaymentid AND p.ipayrollid = " & ipayrollid & ") "

            queriesSave(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Nómina " & ipayrollid & "', 'OK')"

            If executeTransactedSQLCommand(0, queriesSave) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
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

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
        queriesDelete(3) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Nómina " & ipayrollid & "', 'OK')"
        queriesDelete(4) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Payroll', 'Nomina', '" & ipayrollid & "', 'Nómina', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarNomina_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarNomina_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesPayrollIsOpen As Integer = 0

        timesPayrollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payroll" & ipayrollid & "'")

        If timesPayrollIsOpen > 0 Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Nómina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo esta Nómina?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

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

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(4) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        If isEdit = False Then


            cmbFrecuencia.SelectedItem = "Semanal"
            dtFechaNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())

            'Dim auxFechaInicial As Date
            'Dim auxFechaFinal As Date

            'If dtFechaNomina.Value.DayOfWeek = 0 Then

            '    dtFechaInicioNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
            '    auxFechaFinal = dtFechaInicioNomina.Value
            '    auxFechaFinal.AddDays(6)
            '    dtFechaFinNomina.Value = auxFechaFinal

            'Else

            '    auxFechaInicial = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
            '    auxFechaInicial.AddDays(dtFechaNomina.Value.DayOfWeek * -1)
            '    dtFechaInicioNomina.Value = auxFechaInicial
            '    auxFechaFinal = auxFechaInicial
            '    auxFechaFinal.AddDays(6)
            '    dtFechaFinNomina.Value = auxFechaFinal

            'End If


        Else    'isEdit = true

            If isRecover = False Then

                Dim queriesInsert(6) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SELECT * FROM payrolls WHERE ipayrollid = " & ipayrollid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SELECT * FROM payrollpeople WHERE ipayrollid = " & ipayrollid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments SELECT * FROM payrollpayments WHERE ipayrollid = " & ipayrollid

                executeTransactedSQLCommand(0, queriesInsert)

            End If


            Dim dsNomina As DataSet

            dsNomina = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " WHERE ipayrollid = " & ipayrollid)

            If dsNomina.Tables(0).Rows.Count > 0 Then

                cmbFrecuencia.SelectedItem = dsNomina.Tables(0).Rows(0).Item("spayrollfrequency")
                dtFechaNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsNomina.Tables(0).Rows(0).Item("ipayrolldate"))
                dtFechaInicioNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsNomina.Tables(0).Rows(0).Item("ipayrollstartdate"))
                dtFechaFinNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsNomina.Tables(0).Rows(0).Item("ipayrollenddate"))
                cmbTipoNomina.SelectedItem = dsNomina.Tables(0).Rows(0).Item("spayrolltype")
                iprojectid = dsNomina.Tables(0).Rows(0).Item("iprojectid")
                txtProyecto.Text = getSQLQueryAsString(0, "SELECT sprojectname FROM projects WHERE iprojectid = " & iprojectid)
                iclientid = getSQLQueryAsInteger(0, "SELECT ipeopleid FROM projects WHERE iprojectid = " & iprojectid)
                txtCliente.Text = getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = " & iclientid)
                isupervisorid = dsNomina.Tables(0).Rows(0).Item("ipeopleid")
                txtSupervisor.Text = getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = " & isupervisorid)
                txtDescripcionNomina.Text = dsNomina.Tables(0).Rows(0).Item("spayrolldescription")

            End If

            btnRevisiones.Enabled = True
            btnRevisionesPagos.Enabled = True
            btnExportarAExcel.Enabled = True

        End If


        Dim queryPayrolls As String = ""

        queryPayrolls = "" & _
        "SELECT pp.ipeopleid, p.speoplefullname AS 'Persona', p.speopleobservations AS 'Datos Adicionales Persona', FORMAT(pp.ddaysworked, 2) AS 'Dias trabajados', " & _
        "FORMAT(pp.ddaysalary, 2) AS 'Sueldo diario', FORMAT(pp.ddaysworked * pp.ddaysalary, 2) AS 'Subtotal Sueldo', " & _
        "FORMAT(pp.dextrahours, 2) AS 'Horas extras trabajadas', FORMAT(pp.dextrahoursalary, 2) AS 'Sueldo Hora Extra', " & _
        "FORMAT(pp.dextrahours * pp.dextrahoursalary, 2) AS 'Subtotal Horas Extras', " & _
        "sextraincome AS 'Concepto Ingreso Extra (si alguno)', FORMAT(pp.dextraincomeamount, 2) AS 'Monto Ingreso Extra', " & _
        "sdiscount AS 'Concepto Descuento via Nomina (si alguno)', FORMAT(pp.ddiscountamount, 2) AS 'Monto Descuento', " & _
        "FORMAT((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount, 2) AS 'Total A Pagar' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp " & _
        "JOIN people p ON pp.ipeopleid = p.ipeopleid " & _
        "WHERE pp.ipayrollid = " & ipayrollid & " " & _
        "ORDER BY 2 "

        setDataGridView(dgvNominas, queryPayrolls, False)

        dgvNominas.Columns(0).Visible = False

        dgvNominas.Columns(1).ReadOnly = True
        dgvNominas.Columns(2).ReadOnly = True
        dgvNominas.Columns(5).ReadOnly = True
        dgvNominas.Columns(8).ReadOnly = True
        dgvNominas.Columns(13).ReadOnly = True

        dgvNominas.Columns(1).Width = 150
        dgvNominas.Columns(2).Width = 100
        dgvNominas.Columns(3).Width = 70
        dgvNominas.Columns(4).Width = 70
        dgvNominas.Columns(5).Width = 70
        dgvNominas.Columns(6).Width = 70
        dgvNominas.Columns(7).Width = 70
        dgvNominas.Columns(8).Width = 70
        dgvNominas.Columns(9).Width = 150
        dgvNominas.Columns(10).Width = 70
        dgvNominas.Columns(11).Width = 150
        dgvNominas.Columns(12).Width = 70
        dgvNominas.Columns(13).Width = 70

        txtTotalNomina.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
        txtTotalNominaSinDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Nómina " & ipayrollid & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Payroll', 'Nómina', '" & ipayrollid & "', 'Nómina', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True

        cmbFrecuencia.Focus()

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


    Private Sub cmbFrecuencia_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFrecuencia.SelectedIndexChanged


        dtFechaNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())

        If cmbFrecuencia.SelectedItem = "Semanal" Then

            Dim auxFechaInicial As Date
            Dim auxFechaFinal As Date

            If dtFechaNomina.Value.DayOfWeek = 0 Then

                dtFechaInicioNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
                auxFechaFinal = dtFechaInicioNomina.Value
                auxFechaFinal = auxFechaFinal.AddDays(6)
                dtFechaFinNomina.Value = auxFechaFinal

            Else

                auxFechaInicial = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
                auxFechaInicial = auxFechaInicial.AddDays(dtFechaNomina.Value.DayOfWeek * -1)
                dtFechaInicioNomina.Value = auxFechaInicial
                auxFechaFinal = auxFechaInicial
                auxFechaFinal = auxFechaFinal.AddDays(6)
                dtFechaFinNomina.Value = auxFechaFinal

            End If

        ElseIf cmbFrecuencia.SelectedItem = "Catorcenal" Then

            Dim auxFechaInicial As Date
            Dim auxFechaFinal As Date

            If dtFechaNomina.Value.Day = 1 Then

                dtFechaInicioNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
                auxFechaFinal = dtFechaInicioNomina.Value
                auxFechaFinal = auxFechaFinal.AddDays(14)
                dtFechaFinNomina.Value = auxFechaFinal

            ElseIf dtFechaNomina.Value.Day > 14 Then

                auxFechaInicial = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
                auxFechaInicial = auxFechaInicial.AddDays((dtFechaNomina.Value.Day - 14) * -1)
                dtFechaInicioNomina.Value = auxFechaInicial
                auxFechaFinal = auxFechaInicial
                auxFechaFinal = auxFechaFinal.AddDays(14)
                dtFechaFinNomina.Value = auxFechaFinal

            Else

                auxFechaInicial = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
                auxFechaInicial = auxFechaInicial.AddDays((dtFechaNomina.Value.Day - 1) * -1)
                dtFechaInicioNomina.Value = auxFechaInicial
                auxFechaFinal = auxFechaInicial
                auxFechaFinal = auxFechaFinal.AddDays(14)
                dtFechaFinNomina.Value = auxFechaFinal

            End If


        ElseIf cmbFrecuencia.SelectedItem = "Quincenal" Then

            Dim auxFechaInicial As Date
            Dim auxFechaFinal As Date

            If dtFechaNomina.Value.Day = 1 Then

                dtFechaInicioNomina.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
                auxFechaFinal = dtFechaInicioNomina.Value
                auxFechaFinal = auxFechaFinal.AddDays(15)
                dtFechaFinNomina.Value = auxFechaFinal

            ElseIf dtFechaNomina.Value.Day > 15 Then

                auxFechaInicial = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
                auxFechaInicial = auxFechaInicial.AddDays((dtFechaNomina.Value.Day - 15) * -1)
                dtFechaInicioNomina.Value = auxFechaInicial
                auxFechaFinal = auxFechaInicial
                auxFechaFinal = auxFechaFinal.AddDays(15)
                dtFechaFinNomina.Value = auxFechaFinal

            Else

                auxFechaInicial = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate())
                auxFechaInicial = auxFechaInicial.AddDays((dtFechaNomina.Value.Day - 1) * -1)
                dtFechaInicioNomina.Value = auxFechaInicial
                auxFechaFinal = auxFechaInicial
                auxFechaFinal = auxFechaFinal.AddDays(15)
                dtFechaFinNomina.Value = auxFechaFinal

            End If

        End If


        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If validaDatosNomina(True) = True And ipayrollid = 0 Then

            ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
            queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
            queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If


        ElseIf validaDatosNomina(True) = True And ipayrollid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If

        End If



    End Sub


    Private Sub dtFechaNomina_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtFechaNomina.ValueChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim fechaNomina As Date
        Dim fechaInicio As Date
        Dim fechaFin As Date

        fechaNomina = dtFechaNomina.Value
        fechaInicio = dtFechaInicioNomina.Value
        fechaFin = dtFechaFinNomina.Value

        If fechaNomina < fechaInicio.AddDays(-7) Then

            If fechaNomina.DayOfWeek = 0 Then

                dtFechaInicioNomina.Value = fechaNomina
                dtFechaFinNomina.Value = fechaNomina.AddDays(6)

            Else

                dtFechaInicioNomina.Value = fechaNomina.AddDays(fechaNomina.DayOfWeek * -1)
                dtFechaFinNomina.Value = fechaNomina.AddDays(fechaNomina.DayOfWeek * -1).AddDays(6)

            End If

        ElseIf fechaNomina > fechaFin.AddDays(7) Then

            If fechaNomina.DayOfWeek = 0 Then

                dtFechaInicioNomina.Value = fechaNomina
                dtFechaFinNomina.Value = fechaNomina.AddDays(6)

            Else

                dtFechaInicioNomina.Value = fechaNomina.AddDays(fechaNomina.DayOfWeek * -1)
                dtFechaFinNomina.Value = fechaNomina.AddDays(fechaNomina.DayOfWeek * -1).AddDays(6)

            End If

        End If


        If isEdit = True Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid)

        End If

    End Sub


    Private Sub dtFechaInicioNomina_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtFechaInicioNomina.ValueChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If isEdit = True Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid)

        End If

    End Sub


    Private Sub dtFechaFinNomina_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtFechaFinNomina.ValueChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If isEdit = True Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid)

        End If

    End Sub


    Private Sub cmbTipoNomina_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoNomina.SelectedIndexChanged

        iprojectid = 0
        iclientid = 0

        If cmbTipoNomina.SelectedItem = "Proyecto" Then

            btnProyecto.Enabled = True
            btnPersona.Enabled = True
            txtDescripcionNomina.Enabled = True

        Else

            btnProyecto.Enabled = False
            btnPersona.Enabled = False
            txtDescripcionNomina.Enabled = False

        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If validaDatosNomina(False) = True And ipayrollid = 0 Then

            ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
            queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
            queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If


        ElseIf validaDatosNomina(False) = True And ipayrollid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If

        End If

    End Sub


    Private Sub btnProyecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProyecto.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaProyectos

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

            iprojectid = bp.iprojectid
            txtProyecto.Text = bp.sprojectname

            iclientid = getSQLQueryAsInteger(0, "SELECT ipeopleid FROM projects WHERE iprojectid = " & bp.iprojectid)
            txtCliente.Text = getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = " & iclientid)

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If validaDatosNomina(False) = True And ipayrollid = 0 Then

                ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                Dim queriesCreation(9) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
                queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
                queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
                queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
                queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                    dgvNominas.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    If addPersonPermission = True Then
                        btnNuevaPersona.Enabled = True
                        btnInsertarPersona.Enabled = True
                    End If
                    If deletePersonPermission = True Then
                        btnEliminarPersona.Enabled = True
                    End If

                End If


            ElseIf validaDatosNomina(False) = True And ipayrollid > 0 Then

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                    dgvNominas.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    If addPersonPermission = True Then
                        btnNuevaPersona.Enabled = True
                        btnInsertarPersona.Enabled = True
                    End If
                    If deletePersonPermission = True Then
                        btnEliminarPersona.Enabled = True
                    End If

                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPersona.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaPersonas

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

            isupervisorid = bp.ipeopleid
            txtSupervisor.Text = bp.speoplefullname

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If validaDatosNomina(False) = True And ipayrollid = 0 Then

                ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                Dim queriesCreation(9) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
                queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
                queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
                queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
                queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                    dgvNominas.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    If addPersonPermission = True Then
                        btnNuevaPersona.Enabled = True
                        btnInsertarPersona.Enabled = True
                    End If
                    If deletePersonPermission = True Then
                        btnEliminarPersona.Enabled = True
                    End If

                End If


            ElseIf validaDatosNomina(False) = True And ipayrollid > 0 Then

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                    dgvNominas.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    If addPersonPermission = True Then
                        btnNuevaPersona.Enabled = True
                        btnInsertarPersona.Enabled = True
                    End If
                    If deletePersonPermission = True Then
                        btnEliminarPersona.Enabled = True
                    End If

                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtDescripcionNomina_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDescripcionNomina.KeyUp

        Dim strcaracteresprohibidosDescripcionNomina As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhibDescripcionNomina As Char() = strcaracteresprohibidosDescripcionNomina.ToCharArray
        Dim resultadoDescripcionNomina As Boolean = False

        For carpDescripcionNomina = 0 To arrayCaractProhibDescripcionNomina.Length - 1

            If txtDescripcionNomina.Text.Contains(arrayCaractProhibDescripcionNomina(carpDescripcionNomina)) Then
                txtDescripcionNomina.Text = txtDescripcionNomina.Text.Replace(arrayCaractProhibDescripcionNomina(carpDescripcionNomina), "")
                resultadoDescripcionNomina = True
            End If

        Next carpDescripcionNomina

        If resultadoDescripcionNomina = True Then
            txtDescripcionNomina.Select(txtDescripcionNomina.Text.Length, 0)
        End If

    End Sub


    Private Sub txtDescripcionNomina_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDescripcionNomina.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If isEdit = True Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrolldescription = '" & txtDescripcionNomina.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid)

        End If

        If validaDatosNomina(False) = True And ipayrollid = 0 Then

            ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
            queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
            queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If


        ElseIf validaDatosNomina(False) = True And ipayrollid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If

        End If


    End Sub


    Private Sub dgvNominas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvNominas.CellClick

        Try

            If dgvNominas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedpeopleid = CInt(dgvNominas.Rows(e.RowIndex).Cells(0).Value())

        Catch ex As Exception

            iselectedpeopleid = 0

        End Try

    End Sub


    Private Sub dgvNominas_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvNominas.CellContentClick

        Try

            If dgvNominas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedpeopleid = CInt(dgvNominas.Rows(e.RowIndex).Cells(0).Value())

        Catch ex As Exception

            iselectedpeopleid = 0

        End Try

    End Sub


    Private Sub dgvNominas_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvNominas.SelectionChanged

        Try

            If dgvNominas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedpeopleid = CInt(dgvNominas.CurrentRow.Cells(0).Value())

        Catch ex As Exception

            iselectedpeopleid = 0

        End Try

    End Sub


    Private Sub dgvNominas_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvNominas.CellEndEdit

        Dim queryPayrolls As String = ""

        queryPayrolls = "" & _
        "SELECT pp.ipeopleid, p.speoplefullname AS 'Persona', p.speopleobservations AS 'Datos Adicionales Persona', FORMAT(pp.ddaysworked, 2) AS 'Dias trabajados', " & _
        "FORMAT(pp.ddaysalary, 2) AS 'Sueldo diario', FORMAT(pp.ddaysworked * pp.ddaysalary, 2) AS 'Subtotal Sueldo', " & _
        "FORMAT(pp.dextrahours, 2) AS 'Horas extras trabajadas', FORMAT(pp.dextrahoursalary, 2) AS 'Sueldo Hora Extra', " & _
        "FORMAT(pp.dextrahours * pp.dextrahoursalary, 2) AS 'Subtotal Horas Extras', " & _
        "sextraincome AS 'Concepto Ingreso Extra (si alguno)', FORMAT(pp.dextraincomeamount, 2) AS 'Monto Ingreso Extra', " & _
        "sdiscount AS 'Concepto Descuento via Nomina (si alguno)', FORMAT(pp.ddiscountamount, 2) AS 'Monto Descuento', " & _
        "FORMAT((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount, 2) AS 'Total A Pagar' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp " & _
        "JOIN people p ON pp.ipeopleid = p.ipeopleid " & _
        "WHERE pp.ipayrollid = " & ipayrollid & " " & _
        "ORDER BY 2 "

        setDataGridView(dgvNominas, queryPayrolls, False)

        dgvNominas.Columns(0).Visible = False

        dgvNominas.Columns(1).ReadOnly = True
        dgvNominas.Columns(2).ReadOnly = True
        dgvNominas.Columns(5).ReadOnly = True
        dgvNominas.Columns(8).ReadOnly = True
        dgvNominas.Columns(13).ReadOnly = True

        dgvNominas.Columns(1).Width = 150
        dgvNominas.Columns(2).Width = 100
        dgvNominas.Columns(3).Width = 70
        dgvNominas.Columns(4).Width = 70
        dgvNominas.Columns(5).Width = 70
        dgvNominas.Columns(6).Width = 70
        dgvNominas.Columns(7).Width = 70
        dgvNominas.Columns(8).Width = 70
        dgvNominas.Columns(9).Width = 150
        dgvNominas.Columns(10).Width = 70
        dgvNominas.Columns(11).Width = 150
        dgvNominas.Columns(12).Width = 70
        dgvNominas.Columns(13).Width = 70

        txtTotalNomina.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
        txtTotalNominaSinDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

    End Sub


    Private Sub dgvNominas_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvNominas.CellValueChanged

        If modifyPersonPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If e.ColumnIndex = 3 Then 'ddaysworked


            Dim tmpValor As Double = 0.0

            Try

                tmpValor = CDbl(dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET ddaysworked = " & tmpValor & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)


        ElseIf e.ColumnIndex = 4 Then 'ddaysalary


            Dim tmpValor As Double = 0.0

            Try

                tmpValor = CDbl(dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET ddaysalary = " & tmpValor & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)


        ElseIf e.ColumnIndex = 6 Then 'dextrahours


            Dim tmpValor As Double = 0.0

            Try

                tmpValor = CDbl(dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET dextrahours = " & tmpValor & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)


        ElseIf e.ColumnIndex = 7 Then 'dextrahoursalary


            Dim tmpValor As Double = 0.0

            Try

                tmpValor = CDbl(dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET dextrahoursalary = " & tmpValor & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)


        ElseIf e.ColumnIndex = 9 Then 'sextraincome


            If dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET sextraincome = '', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET sextraincome = '" & dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)

            End If


        ElseIf e.ColumnIndex = 10 Then 'dextraincomeamount


            Dim tmpValor As Double = 0.0

            Try

                tmpValor = CDbl(dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET dextraincomeamount = " & tmpValor & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)


        ElseIf e.ColumnIndex = 11 Then 'sdiscount


            If dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET sdiscount = '', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET sdiscount = '" & dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)

            End If


        ElseIf e.ColumnIndex = 12 Then 'ddiscountamount


            Dim tmpValor As Double = 0.0

            Try

                tmpValor = CDbl(dgvNominas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People SET ddiscountamount = " & tmpValor & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipeopleid = " & iselectedpeopleid)


        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvNominas_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvNominas.EditingControlShowing

        If dgvNominas.CurrentCell.ColumnIndex = 3 Or dgvNominas.CurrentCell.ColumnIndex = 4 Or dgvNominas.CurrentCell.ColumnIndex = 6 Or dgvNominas.CurrentCell.ColumnIndex = 7 Or dgvNominas.CurrentCell.ColumnIndex = 10 Or dgvNominas.CurrentCell.ColumnIndex = 12 Then

            txtNumeroDgvNominas = CType(e.Control, TextBox)
            txtNumeroDgvNominas_OldText = txtNumeroDgvNominas.Text

            'ElseIf dgvNominas.CurrentCell.ColumnIndex = 9 Or dgvNominas.CurrentCell.ColumnIndex = 11 Then

            '    txtNotaDgvNominas = CType(e.Control, TextBox)
            '    txtNotaDgvNominas_OldText = txtNotaDgvNominas.Text

        Else

            txtNumeroDgvNominas = Nothing
            txtNumeroDgvNominas_OldText = Nothing
            txtNotaDgvNominas = Nothing
            txtNotaDgvNominas_OldText = Nothing

        End If

    End Sub


    Private Sub txtNumeroDgvNominas_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumeroDgvNominas.KeyUp

        If Not IsNumeric(txtNumeroDgvNominas.Text) Then

            Dim strForbidden3 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden3 As Char() = strForbidden3.ToCharArray

            For cp = 0 To arrayForbidden3.Length - 1

                If txtNumeroDgvNominas.Text.Contains(arrayForbidden3(cp)) Then
                    txtNumeroDgvNominas.Text = txtNumeroDgvNominas.Text.Replace(arrayForbidden3(cp), "")
                End If

            Next cp

            If txtNumeroDgvNominas.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtNumeroDgvNominas.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtNumeroDgvNominas.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtNumeroDgvNominas.Text.Length

                        If longitud > (lugar + 1) Then
                            txtNumeroDgvNominas.Text = txtNumeroDgvNominas.Text.Substring(0, lugar) & txtNumeroDgvNominas.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtNumeroDgvNominas.Text = txtNumeroDgvNominas.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtNumeroDgvNominas.Text = txtNumeroDgvNominas.Text.Replace("--", "").Replace("'", "")
            txtNumeroDgvNominas.Text = txtNumeroDgvNominas.Text.Trim

        Else
            txtNumeroDgvNominas_OldText = txtNumeroDgvNominas.Text
        End If

    End Sub


    Private Sub txtNotaDgvNominas_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNotaDgvNominas.KeyUp

        Dim strcaracteresprohibidos As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNotaDgvNominas.Text.Contains(arrayCaractProhib(carp)) Then
                txtNotaDgvNominas.Text = txtNotaDgvNominas.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtNotaDgvNominas.Select(txtNotaDgvNominas.Text.Length, 0)
        End If

    End Sub


    Private Sub dgvNominas_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvNominas.UserAddedRow

        If addPersonPermission = False Then
            Exit Sub
        End If

        Dim bp As New BuscaPersonas

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

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People VALUES (" & ipayrollid & ", " & bp.ipeopleid & ", 0, 0, 0, 0, '', 0.00, '', 0.00, " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvNominas_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvNominas.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deletePersonPermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas eliminar esta Persona de la Nómina?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Persona de la Nomina") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedpeopleid As Integer = 0
                Try
                    tmpselectedpeopleid = CInt(dgvNominas.CurrentRow.Cells(0).Value)
                Catch ex As Exception

                End Try

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People WHERE ipayrollid = " & ipayrollid & " and ipeopleid = " & tmpselectedpeopleid)

                Dim queryPayrolls As String = ""

                queryPayrolls = "" & _
                "SELECT pp.ipeopleid, p.speoplefullname AS 'Persona', p.speopleobservations AS 'Datos Adicionales Persona', FORMAT(pp.ddaysworked, 2) AS 'Dias trabajados', " & _
                "FORMAT(pp.ddaysalary, 2) AS 'Sueldo diario', FORMAT(pp.ddaysworked * pp.ddaysalary, 2) AS 'Subtotal Sueldo', " & _
                "FORMAT(pp.dextrahours, 2) AS 'Horas extras trabajadas', FORMAT(pp.dextrahoursalary, 2) AS 'Sueldo Hora Extra', " & _
                "FORMAT(pp.dextrahours * pp.dextrahoursalary, 2) AS 'Subtotal Horas Extras', " & _
                "sextraincome AS 'Concepto Ingreso Extra (si alguno)', FORMAT(pp.dextraincomeamount, 2) AS 'Monto Ingreso Extra', " & _
                "sdiscount AS 'Concepto Descuento via Nomina (si alguno)', FORMAT(pp.ddiscountamount, 2) AS 'Monto Descuento', " & _
                "FORMAT((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount, 2) AS 'Total A Pagar' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp " & _
                "JOIN people p ON pp.ipeopleid = p.ipeopleid " & _
                "WHERE pp.ipayrollid = " & ipayrollid & " " & _
                "ORDER BY 2 "

                setDataGridView(dgvNominas, queryPayrolls, False)

                dgvNominas.Columns(0).Visible = False

                dgvNominas.Columns(1).ReadOnly = True
                dgvNominas.Columns(2).ReadOnly = True
                dgvNominas.Columns(5).ReadOnly = True
                dgvNominas.Columns(8).ReadOnly = True
                dgvNominas.Columns(13).ReadOnly = True

                dgvNominas.Columns(1).Width = 150
                dgvNominas.Columns(2).Width = 100
                dgvNominas.Columns(3).Width = 70
                dgvNominas.Columns(4).Width = 70
                dgvNominas.Columns(5).Width = 70
                dgvNominas.Columns(6).Width = 70
                dgvNominas.Columns(7).Width = 70
                dgvNominas.Columns(8).Width = 70
                dgvNominas.Columns(9).Width = 150
                dgvNominas.Columns(10).Width = 70
                dgvNominas.Columns(11).Width = 150
                dgvNominas.Columns(12).Width = 70
                dgvNominas.Columns(13).Width = 70

                txtTotalNomina.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtTotalNominaSinDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub btnNuevaPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevaPersona.Click


        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        'Empieza código de Nueva Persona

        Dim ap As New AgregarPersona

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        If ap.DialogResult = Windows.Forms.DialogResult.OK Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If validaDatosNomina(False) = True And ipayrollid = 0 Then

                ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                Dim queriesCreation(9) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
                queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
                queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
                queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
                queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                    dgvNominas.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    If addPersonPermission = True Then
                        btnNuevaPersona.Enabled = True
                        btnInsertarPersona.Enabled = True
                    End If
                    If deletePersonPermission = True Then
                        btnEliminarPersona.Enabled = True
                    End If

                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People VALUES (" & ipayrollid & ", " & ap.ipeopleid & ", 0, 0, 0, 0, '', 0.00, '', 0.00, " & fecha & ", '" & hora & "', '" & susername & "')")

                End If


            ElseIf validaDatosNomina(False) = True And ipayrollid > 0 Then

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                    dgvNominas.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    If addPersonPermission = True Then
                        btnNuevaPersona.Enabled = True
                        btnInsertarPersona.Enabled = True
                    End If
                    If deletePersonPermission = True Then
                        btnEliminarPersona.Enabled = True
                    End If

                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People VALUES (" & ipayrollid & ", " & ap.ipeopleid & ", 0, 0, 0, 0, '', 0.00, '', 0.00, " & fecha & ", '" & hora & "', '" & susername & "')")

                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queryPayrolls As String = ""

        queryPayrolls = "" & _
        "SELECT pp.ipeopleid, p.speoplefullname AS 'Persona', p.speopleobservations AS 'Datos Adicionales Persona', FORMAT(pp.ddaysworked, 2) AS 'Dias trabajados', " & _
        "FORMAT(pp.ddaysalary, 2) AS 'Sueldo diario', FORMAT(pp.ddaysworked * pp.ddaysalary, 2) AS 'Subtotal Sueldo', " & _
        "FORMAT(pp.dextrahours, 2) AS 'Horas extras trabajadas', FORMAT(pp.dextrahoursalary, 2) AS 'Sueldo Hora Extra', " & _
        "FORMAT(pp.dextrahours * pp.dextrahoursalary, 2) AS 'Subtotal Horas Extras', " & _
        "sextraincome AS 'Concepto Ingreso Extra (si alguno)', FORMAT(pp.dextraincomeamount, 2) AS 'Monto Ingreso Extra', " & _
        "sdiscount AS 'Concepto Descuento via Nomina (si alguno)', FORMAT(pp.ddiscountamount, 2) AS 'Monto Descuento', " & _
        "FORMAT((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount, 2) AS 'Total A Pagar' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp " & _
        "JOIN people p ON pp.ipeopleid = p.ipeopleid " & _
        "WHERE pp.ipayrollid = " & ipayrollid & " " & _
        "ORDER BY 2 "

        setDataGridView(dgvNominas, queryPayrolls, False)

        dgvNominas.Columns(0).Visible = False

        dgvNominas.Columns(1).ReadOnly = True
        dgvNominas.Columns(2).ReadOnly = True
        dgvNominas.Columns(5).ReadOnly = True
        dgvNominas.Columns(8).ReadOnly = True
        dgvNominas.Columns(13).ReadOnly = True

        dgvNominas.Columns(1).Width = 150
        dgvNominas.Columns(2).Width = 100
        dgvNominas.Columns(3).Width = 70
        dgvNominas.Columns(4).Width = 70
        dgvNominas.Columns(5).Width = 70
        dgvNominas.Columns(6).Width = 70
        dgvNominas.Columns(7).Width = 70
        dgvNominas.Columns(8).Width = 70
        dgvNominas.Columns(9).Width = 150
        dgvNominas.Columns(10).Width = 70
        dgvNominas.Columns(11).Width = 150
        dgvNominas.Columns(12).Width = 70
        dgvNominas.Columns(13).Width = 70

        txtTotalNomina.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
        txtTotalNominaSinDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnInsertarPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarPersona.Click

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        'Empieza código de Insertar Persona

        Dim bp As New BuscaPersonas

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

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If validaDatosNomina(False) = True And ipayrollid = 0 Then

                ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                Dim queriesCreation(9) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
                queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
                queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
                queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
                queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                    dgvNominas.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    If addPersonPermission = True Then
                        btnNuevaPersona.Enabled = True
                        btnInsertarPersona.Enabled = True
                    End If
                    If deletePersonPermission = True Then
                        btnEliminarPersona.Enabled = True
                    End If

                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People VALUES (" & ipayrollid & ", " & bp.ipeopleid & ", 0, 0, 0, 0, '', 0.00, '', 0.00, " & fecha & ", '" & hora & "', '" & susername & "')")

                End If


            ElseIf validaDatosNomina(False) = True And ipayrollid > 0 Then

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                    dgvNominas.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    If addPersonPermission = True Then
                        btnNuevaPersona.Enabled = True
                        btnInsertarPersona.Enabled = True
                    End If
                    If deletePersonPermission = True Then
                        btnEliminarPersona.Enabled = True
                    End If

                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People VALUES (" & ipayrollid & ", " & bp.ipeopleid & ", 0, 0, 0, 0, '', 0.00, '', 0.00, " & fecha & ", '" & hora & "', '" & susername & "')")

                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queryPayrolls As String = ""

        queryPayrolls = "" & _
        "SELECT pp.ipeopleid, p.speoplefullname AS 'Persona', p.speopleobservations AS 'Datos Adicionales Persona', FORMAT(pp.ddaysworked, 2) AS 'Dias trabajados', " & _
        "FORMAT(pp.ddaysalary, 2) AS 'Sueldo diario', FORMAT(pp.ddaysworked * pp.ddaysalary, 2) AS 'Subtotal Sueldo', " & _
        "FORMAT(pp.dextrahours, 2) AS 'Horas extras trabajadas', FORMAT(pp.dextrahoursalary, 2) AS 'Sueldo Hora Extra', " & _
        "FORMAT(pp.dextrahours * pp.dextrahoursalary, 2) AS 'Subtotal Horas Extras', " & _
        "sextraincome AS 'Concepto Ingreso Extra (si alguno)', FORMAT(pp.dextraincomeamount, 2) AS 'Monto Ingreso Extra', " & _
        "sdiscount AS 'Concepto Descuento via Nomina (si alguno)', FORMAT(pp.ddiscountamount, 2) AS 'Monto Descuento', " & _
        "FORMAT((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount, 2) AS 'Total A Pagar' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp " & _
        "JOIN people p ON pp.ipeopleid = p.ipeopleid " & _
        "WHERE pp.ipayrollid = " & ipayrollid & " " & _
        "ORDER BY 2 "

        setDataGridView(dgvNominas, queryPayrolls, False)

        dgvNominas.Columns(0).Visible = False

        dgvNominas.Columns(1).ReadOnly = True
        dgvNominas.Columns(2).ReadOnly = True
        dgvNominas.Columns(5).ReadOnly = True
        dgvNominas.Columns(8).ReadOnly = True
        dgvNominas.Columns(13).ReadOnly = True

        dgvNominas.Columns(1).Width = 150
        dgvNominas.Columns(2).Width = 100
        dgvNominas.Columns(3).Width = 70
        dgvNominas.Columns(4).Width = 70
        dgvNominas.Columns(5).Width = 70
        dgvNominas.Columns(6).Width = 70
        dgvNominas.Columns(7).Width = 70
        dgvNominas.Columns(8).Width = 70
        dgvNominas.Columns(9).Width = 150
        dgvNominas.Columns(10).Width = 70
        dgvNominas.Columns(11).Width = 150
        dgvNominas.Columns(12).Width = 70
        dgvNominas.Columns(13).Width = 70

        txtTotalNomina.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
        txtTotalNominaSinDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarPersona.Click


        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If validaDatosNomina(False) = True And ipayrollid = 0 Then

            ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
            queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
            queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If


        ElseIf validaDatosNomina(False) = True And ipayrollid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If

        End If

        'Empieza código de Eliminar Persona

        If MsgBox("¿Está seguro que deseas eliminar esta Persona de la Nómina?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Persona de la Nomina") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedpeopleid As Integer = 0
            Try
                tmpselectedpeopleid = CInt(dgvNominas.CurrentRow.Cells(0).Value)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People WHERE ipayrollid = " & ipayrollid & " and ipeopleid = " & tmpselectedpeopleid)

            Dim queryPayrolls As String = ""

            queryPayrolls = "" & _
            "SELECT pp.ipeopleid, p.speoplefullname AS 'Persona', p.speopleobservations AS 'Datos Adicionales Persona', FORMAT(pp.ddaysworked, 2) AS 'Dias trabajados', " & _
            "FORMAT(pp.ddaysalary, 2) AS 'Sueldo diario', FORMAT(pp.ddaysworked * pp.ddaysalary, 2) AS 'Subtotal Sueldo', " & _
            "FORMAT(pp.dextrahours, 2) AS 'Horas extras trabajadas', FORMAT(pp.dextrahoursalary, 2) AS 'Sueldo Hora Extra', " & _
            "FORMAT(pp.dextrahours * pp.dextrahoursalary, 2) AS 'Subtotal Horas Extras', " & _
            "sextraincome AS 'Concepto Ingreso Extra (si alguno)', FORMAT(pp.dextraincomeamount, 2) AS 'Monto Ingreso Extra', " & _
            "sdiscount AS 'Concepto Descuento via Nomina (si alguno)', FORMAT(pp.ddiscountamount, 2) AS 'Monto Descuento', " & _
            "FORMAT((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount, 2) AS 'Total A Pagar' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp " & _
            "JOIN people p ON pp.ipeopleid = p.ipeopleid " & _
            "WHERE pp.ipayrollid = " & ipayrollid & " " & _
            "ORDER BY 2 "

            setDataGridView(dgvNominas, queryPayrolls, False)

            dgvNominas.Columns(0).Visible = False

            dgvNominas.Columns(1).ReadOnly = True
            dgvNominas.Columns(2).ReadOnly = True
            dgvNominas.Columns(5).ReadOnly = True
            dgvNominas.Columns(8).ReadOnly = True
            dgvNominas.Columns(13).ReadOnly = True

            dgvNominas.Columns(1).Width = 150
            dgvNominas.Columns(2).Width = 100
            dgvNominas.Columns(3).Width = 70
            dgvNominas.Columns(4).Width = 70
            dgvNominas.Columns(5).Width = 70
            dgvNominas.Columns(6).Width = 70
            dgvNominas.Columns(7).Width = 70
            dgvNominas.Columns(8).Width = 70
            dgvNominas.Columns(9).Width = 150
            dgvNominas.Columns(10).Width = 70
            dgvNominas.Columns(11).Width = 150
            dgvNominas.Columns(12).Width = 70
            dgvNominas.Columns(13).Width = 70

            txtTotalNomina.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtTotalNominaSinDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Function validaNominaCompleta(ByVal silent As Boolean) As Boolean

        If validaDatosNomina(silent) = False Or validaPersonasNomina(silent) = False Then
            Return False
        Else
            Return True
        End If

    End Function


    Private Function validaDatosNomina(ByVal silent As Boolean) As Boolean

        Dim strcaracproh As String = "|°!#$%&/()=?¡*¨[]_:;-{}+´¿'¬^`~@\<>"
        Dim diasFrecuencia As Integer = 0
        Dim auxFechaInicial As Date

        txtDescripcionNomina.Text = txtDescripcionNomina.Text.Trim(strcaracproh.ToCharArray).Replace("--", "").Replace("'", "")

        If cmbFrecuencia.SelectedIndex = -1 Then
            If silent = False Then
                MsgBox("¿Podrías indicar que frecuencia tiene la nómina?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        Else

            If cmbFrecuencia.SelectedItem = "Semanal" Then
                diasFrecuencia = 7
            ElseIf cmbFrecuencia.SelectedItem = "Catorcenal" Then
                diasFrecuencia = 14
            ElseIf cmbFrecuencia.SelectedItem = "Quincenal" Then
                diasFrecuencia = 15
            End If

        End If

        auxFechaInicial = dtFechaInicioNomina.Value

        If dtFechaFinNomina.Value > auxFechaInicial.AddDays(diasFrecuencia) Then

            If silent = False Then
                MsgBox("El periodo de fechas es más largo que lo que indica la Frecuencia. ¿Podrías corregir las fechas?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False

        End If

        If convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") > getMySQLDate() Then
            If silent = False Then
                MsgBox("La fecha de la Nómina está en el futuro. ¿Podriamos regresar al Presente, McFly?", MsgBoxStyle.OkOnly, "ONE POINT TWENTY-ONE GIGAWATTS?! ONE POINT TWENTY-ONE GIGAWATTS!!")
            End If
            Return False
        End If

        If cmbTipoNomina.SelectedIndex = -1 Then
            If silent = False Then
                MsgBox("¿Podrías indicar el tipo de Nómina que estás registrando?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If cmbTipoNomina.SelectedText = "Proyecto" And iprojectid = 0 Then
            If silent = False Then
                MsgBox("¿Podrías escoger el proyecto al que se refiere esta nómina?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If cmbTipoNomina.SelectedItem.ToString = "Proyecto" Then

            If isEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM payrolls WHERE iprojectid = " & iprojectid & " AND ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & " AND ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "")) > 0 Then
                If silent = False Then
                    MsgBox("Ya está registrada una nómina para este proyecto para esas fechas. Verifica si ya la registraron.", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                Return False
            End If

        Else

            If isEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM payrolls WHERE spayrolltype = '" & cmbTipoNomina.SelectedItem.ToString & "' AND ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & " AND ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "")) > 0 Then
                If silent = False Then
                    MsgBox("Ya está registrada una nómina para este proyecto para esas fechas. Verifica si ya la registraron.", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                Return False
            End If

        End If

        If cmbTipoNomina.SelectedText = "Proyecto" And isupervisorid = 0 Then
            If silent = False Then
                MsgBox("¿Podrías decirme quien superviso esta obra esa semana? (Supervisor)", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        Return True


    End Function


    Private Function validaPersonasNomina(ByVal silent As Boolean) As Boolean

        Dim totalnomina As Double = 0.0
        totalnomina = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid)

        If totalnomina <= 0.0 Then

            If silent = False Then
                MsgBox("¿Podrías poner a alguien en la Nómina? Está vacía...", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click, btnCancelarPago.Click

        'wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click, btnGuardarPago.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesPayrollIsOpen As Integer = 1

        timesPayrollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payroll" & ipayrollid & "'")

        If timesPayrollIsOpen > 1 Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Nómina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando esta Nómina?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        Dim queriesSave(10) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM payrolls " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp WHERE payrolls.ipayrollid = tp.ipayrollid) "

        queriesSave(1) = "" & _
        "DELETE " & _
        "FROM payrollpeople " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp WHERE payrollpeople.ipayrollid = tp.ipayrollid AND payrollpeople.ipeopleid = tp.ipeopleid) "

        queriesSave(2) = "" & _
        "DELETE " & _
        "FROM payrollpayments " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp WHERE payrollpayments.ipayrollid = tp.ipayrollid AND payrollpayments.ipaymentid = tp.ipaymentid) "

        queriesSave(3) = "" & _
        "UPDATE payrolls p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp ON tp.ipayrollid = p.ipayrollid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.spayrollfrequency = tp.spayrollfrequency, p.spayrolltype = tp.spayrolltype, p.ipayrolldate = tp.ipayrolldate, p.spayrolltime = tp.spayrolltime, p.ipayrollstartdate = tp.ipayrollstartdate, p.spayrollstarttime = tp.spayrollstarttime, p.ipayrollenddate = tp.ipayrollenddate, p.spayrollendtime = tp.spayrollendtime, p.ipeopleid = tp.ipeopleid, p.iprojectid = tp.iprojectid, p.spayrolldescription = tp.spayrolldescription WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(4) = "" & _
        "UPDATE payrollpeople p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp ON tp.ipayrollid = p.ipayrollid AND tp.ipeopleid = p.ipeopleid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.ddaysworked = tp.ddaysworked, p.ddaysalary = tp.ddaysalary, p.dextrahours = tp.dextrahours, p.dextrahoursalary = tp.dextrahoursalary, p.sdiscount = tp.sdiscount, p.ddiscountamount = tp.ddiscountamount, p.sextraincome = tp.sextraincome, p.dextraincomeamount = tp.dextraincomeamount WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(5) = "" & _
        "UPDATE payrollpayments p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp ON tp.ipayrollid = p.ipayrollid AND tp.ipaymentid = p.ipaymentid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sextranote = tp.sextranote WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(6) = "" & _
        "INSERT INTO payrolls " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrolls p WHERE p.ipayrollid = tp.ipayrollid AND p.ipayrollid = " & ipayrollid & ") "

        queriesSave(7) = "" & _
        "INSERT INTO payrollpeople " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrollpeople p WHERE p.ipayrollid = tp.ipayrollid AND p.ipeopleid = tp.ipeopleid AND p.ipayrollid = " & ipayrollid & ") "

        queriesSave(8) = "" & _
        "INSERT INTO payrollpayments " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrollpayments p WHERE p.ipayrollid = tp.ipayrollid AND p.ipaymentid = tp.ipaymentid AND p.ipayrollid = " & ipayrollid & ") "

        queriesSave(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Nómina " & ipayrollid & "', 'OK')"

        If executeTransactedSQLCommand(0, queriesSave) = True Then

            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")

            wasCreated = True

            btnRevisiones.Enabled = True
            btnRevisionesPagos.Enabled = True
            btnExportarAExcel.Enabled = True

        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnExportarAExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportarAExcel.Click

        Try

            Dim resultado As Boolean = False

            Dim fecha As String = ""
            Dim dayAux As String = ""
            Dim monthAux As String = ""
            Dim hourAux As String = ""
            Dim minuteAux As String = ""
            Dim secondAux As String = ""

            If DateTime.Today.Month.ToString.Length < 2 Then
                monthAux = "0" & DateTime.Today.Month
            Else
                monthAux = DateTime.Today.Month
            End If

            If DateTime.Today.Day.ToString.Length < 2 Then
                dayAux = "0" & DateTime.Today.Day
            Else
                dayAux = DateTime.Today.Day
            End If

            If Date.Now.Hour.ToString.Length < 2 Then
                hourAux = "0" & DateTime.Now.Hour
            Else
                hourAux = DateTime.Now.Hour
            End If

            If Date.Now.Minute.ToString.Length < 2 Then
                minuteAux = "0" & DateTime.Now.Minute
            Else
                minuteAux = DateTime.Now.Minute
            End If

            If Date.Now.Second.ToString.Length < 2 Then
                secondAux = "0" & DateTime.Now.Second
            Else
                secondAux = DateTime.Now.Second
            End If

            fecha = DateTime.Today.Year & monthAux & dayAux & hourAux & minuteAux & secondAux

            If cmbTipoNomina.SelectedItem = "Proyecto" Then

                msSaveFileDialog.FileName = "Nomina Proyecto " & txtProyecto.Text & " " & fecha

            ElseIf cmbTipoNomina.SelectedItem = "Oficina" Then

                msSaveFileDialog.FileName = "Nomina Oficina " & fecha

            ElseIf cmbTipoNomina.SelectedItem = "Aserradero" Then

                msSaveFileDialog.FileName = "Nomina Aserradero " & fecha

            ElseIf cmbTipoNomina.SelectedItem = "Monte" Then

                msSaveFileDialog.FileName = "Nomina Monte " & fecha

            End If

            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportNominaToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Nómina Exportada Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar la nómina. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar el presupuesto")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportNominaToExcel(ByVal pth As String) As Boolean

        Try

            Dim fs As New IO.StreamWriter(pth, False)
            fs.WriteLine("<?xml version=""1.0""?>")
            fs.WriteLine("<?mso-application progid=""Excel.Sheet""?>")
            fs.WriteLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:html=""http://www.w3.org/TR/REC-html40"">")

            ' Create the styles for the worksheet
            fs.WriteLine("  <Styles>")

            ' Style for the document name
            fs.WriteLine("  <Style ss:ID=""1"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("    <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("    <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("    <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("    <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""12"" ss:Bold=""1""></Font>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat ss:Format=""_-&quot;$&quot;* #,##0.00_-;\-&quot;$&quot;* #,##0.00_-;_-&quot;$&quot;* &quot;-&quot;??_-;_-@_-""></NumberFormat>")
            fs.WriteLine("   <Protection></Protection>")
            fs.WriteLine("  </Style>")

            ' Style for the column headers
            fs.WriteLine("  <Style ss:ID=""2"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("    <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("    <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("    <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("    <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""9"" ss:Bold=""1""></Font>")
            fs.WriteLine("  </Style>")


            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""9"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("      <NumberFormat ss:Format=""_-&quot;$&quot;* #,##0.00_-;\-&quot;$&quot;* #,##0.00_-;_-&quot;$&quot;* &quot;-&quot;??_-;_-@_-""></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""10"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("      <NumberFormat ss:Format=""_-&quot;$&quot;* #,##0.00_-;\-&quot;$&quot;* #,##0.00_-;_-&quot;$&quot;* &quot;-&quot;??_-;_-@_-""></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""11"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("      <NumberFormat ss:Format=""_-&quot;$&quot;* #,##0.00_-;\-&quot;$&quot;* #,##0.00_-;_-&quot;$&quot;* &quot;-&quot;??_-;_-@_-""></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the SUBtotals labels
            fs.WriteLine("    <Style ss:ID=""12"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("      <Interior ss:Color=""#FFCC00"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("      <NumberFormat ss:Format=""_-&quot;$&quot;* #,##0.00_-;\-&quot;$&quot;* #,##0.00_-;_-&quot;$&quot;* &quot;-&quot;??_-;_-@_-""></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals labels
            fs.WriteLine("    <Style ss:ID=""13"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("      <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("      <NumberFormat ss:Format=""_-&quot;$&quot;* #,##0.00_-;\-&quot;$&quot;* #,##0.00_-;_-&quot;$&quot;* &quot;-&quot;??_-;_-@_-""></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals
            fs.WriteLine("    <Style ss:ID=""14"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("      <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("      <NumberFormat ss:Format=""_-&quot;$&quot;* #,##0.00_-;\-&quot;$&quot;* #,##0.00_-;_-&quot;$&quot;* &quot;-&quot;??_-;_-@_-""></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""15"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""16"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""17"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            fs.WriteLine("  </Styles>")

            ' Write the worksheet contents
            fs.WriteLine("<Worksheet ss:Name=""Hoja1"">")
            fs.WriteLine("  <Table ss:DefaultColumnWidth=""60"" ss:DefaultRowHeight=""15"">")

            'Write the header info
            fs.WriteLine("   <Column ss:Width=""150.75""/>")
            fs.WriteLine("   <Column ss:Width=""154.5""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""65.25"" ss:Span=""13""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("   <Cell ss:MergeAcross=""12"" ss:StyleID=""1""><Data ss:Type=""String"">Maderería Río Dorado S.A. de C.V.</Data></Cell>")
            fs.WriteLine("   </Row>")

            If cmbTipoNomina.SelectedItem = "Proyecto" Then

                fs.WriteLine("  <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("   <Cell ss:MergeAcross=""12"" ss:StyleID=""1""><Data ss:Type=""String"">Nómina " & cmbFrecuencia.SelectedItem.ToString & ", del " & dtFechaInicioNomina.Value.ToString("d") & " al " & dtFechaFinNomina.Value.ToString("d") & "</Data></Cell>")
                fs.WriteLine("   </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(String.Format(" <Row ss:AutoFitHeight=""0"">"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(String.Format(" <Row ss:AutoFitHeight=""0"">"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", lblCliente.Text.Trim))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", txtCliente.Text.Trim))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", lblProyecto.Text.Trim))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", txtProyecto.Text.Trim))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                Dim dsProyecto As DataSet

                dsProyecto = getSQLQueryAsDataset(0, "SELECT * FROM projects WHERE iprojectid = " & iprojectid)

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Dimensiones:"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", dsProyecto.Tables(0).Rows(0).Item("dprojectlength") & " x " & dsProyecto.Tables(0).Rows(0).Item("dprojectwidth") & " mts"))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Lugar:"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", dsProyecto.Tables(0).Rows(0).Item("sterrainlocation")))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha de Inicio:"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsProyecto.Tables(0).Rows(0).Item("iprojectstarteddate"))))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha de Terminacion:"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsProyecto.Tables(0).Rows(0).Item("iprojectforecastedclosingdate"))))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Días de Ejecución:"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""Number"">{0}</Data></Cell>", howManyDaysBetween(convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsProyecto.Tables(0).Rows(0).Item("iprojectstarteddate")), convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsProyecto.Tables(0).Rows(0).Item("iprojectforecastedclosingdate")))))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Supervisor:"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", txtSupervisor.Text))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Concepto:"))
                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", txtDescripcionNomina.Text))
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

                fs.WriteLine(" <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
                fs.WriteLine(" </Row>")

            ElseIf cmbTipoNomina.SelectedItem = "Oficina" Then

                fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("   <Cell ss:MergeAcross=""12"" ss:StyleID=""1""><Data ss:Type=""String"">Nómina " & cmbFrecuencia.SelectedItem.ToString & ", del " & dtFechaInicioNomina.Value.ToString("d") & " al " & dtFechaFinNomina.Value.ToString("d") & "</Data></Cell>")
                fs.WriteLine("   </Row>")

                fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("   <Cell ss:MergeAcross=""12"" ss:StyleID=""1""><Data ss:Type=""String"">" & cmbTipoNomina.SelectedItem.ToString & "</Data></Cell>")
                fs.WriteLine("   </Row>")

                'fs.WriteLine(String.Format(" <Row ss:AutoFitHeight=""0"">"))
                'fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                'fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
                'fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
                'fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                'fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                'fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                'fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                'fs.WriteLine("  <Cell ss:StyleID=""15""></Cell>")
                'fs.WriteLine(" </Row>")

            ElseIf cmbTipoNomina.SelectedItem = "Aserradero" Then

                fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("   <Cell ss:MergeAcross=""12"" ss:StyleID=""1""><Data ss:Type=""String"">Nómina " & cmbFrecuencia.SelectedItem.ToString & ", del " & dtFechaInicioNomina.Value.ToString("d") & " al " & dtFechaFinNomina.Value.ToString("d") & "</Data></Cell>")
                fs.WriteLine("   </Row>")

                fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("   <Cell ss:MergeAcross=""12"" ss:StyleID=""1""><Data ss:Type=""String"">" & cmbTipoNomina.SelectedItem.ToString & "</Data></Cell>")
                fs.WriteLine("   </Row>")


            ElseIf cmbTipoNomina.SelectedItem = "Monte" Then

                fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("   <Cell ss:MergeAcross=""12"" ss:StyleID=""1""><Data ss:Type=""String"">Nómina " & cmbFrecuencia.SelectedItem.ToString & ", del " & dtFechaInicioNomina.Value.ToString("d") & " al " & dtFechaFinNomina.Value.ToString("d") & "</Data></Cell>")
                fs.WriteLine("   </Row>")

                fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
                fs.WriteLine("   <Cell ss:MergeAcross=""12"" ss:StyleID=""1""><Data ss:Type=""String"">" & cmbTipoNomina.SelectedItem.ToString & "</Data></Cell>")
                fs.WriteLine("   </Row>")

            End If




            'Write the grid headers columns (taken out since columns are already defined)
            'For Each col As DataGridViewColumn In dgv.Columns
            '    If col.Visible Then
            '        fs.WriteLine(String.Format("    <Column ss:Width=""{0}""></Column>", col.Width))
            '    End If
            'Next

            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""48"">")

            For Each col As DataGridViewColumn In dgvNominas.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next

            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvNominas.Rows

                If dgvNominas.AllowUserToAddRows = True And row.Index = dgvNominas.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvNominas.Columns

                    If col.Visible Then

                        If row.Cells(col.Name).Value.ToString = "" Then

                            'If row.Cells(0).Value.ToString.StartsWith("SUBTOTAL") = True Then
                            '    fs.WriteLine(String.Format("      <Cell ss:StyleID=""12""></Cell>"))
                            'Else
                            If col.Index = 1 Or col.Index = 2 Then
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""></Cell>"))
                            Else

                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""11""></Cell>"))
                            End If

                            'End If

                        Else

                            Dim isNumber As Boolean = False
                            Dim valor As Double = 0.0

                            Try
                                valor = CDbl(row.Cells(col.Name).Value.ToString)
                                isNumber = True
                            Catch ex As Exception
                                isNumber = False
                            End Try

                            'If row.Cells(0).Value.ToString.StartsWith("SUBTOTAL") = True Then
                            '    fs.WriteLine(String.Format("      <Cell ss:StyleID=""12""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                            'Else

                            If isNumber = True Then  ' Numeros

                                If col.Index = 1 Or col.Index = 2 Then

                                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""Number"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))

                                ElseIf col.Index = 3 Or col.Index = 6 Then

                                    If valor = 0 Then
                                        fs.WriteLine("      <Cell ss:StyleID=""17""></Cell>")
                                    Else
                                        fs.WriteLine(String.Format("      <Cell ss:StyleID=""17""><Data ss:Type=""Number"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                                    End If

                                ElseIf col.Index = 5 Then

                                    If valor = 0 Then
                                        fs.WriteLine("      <Cell ss:StyleID=""11""></Cell>")
                                    Else
                                        fs.WriteLine(String.Format("      <Cell ss:StyleID=""11"" ss:Formula=""=RC[-2]*RC[-1]""><Data ss:Type=""Number"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                                    End If

                                ElseIf col.Index = 8 Then

                                    If valor = 0 Then
                                        fs.WriteLine("      <Cell ss:StyleID=""11""></Cell>")
                                    Else
                                        fs.WriteLine(String.Format("      <Cell ss:StyleID=""11"" ss:Formula=""=RC[-2]*RC[-1]""><Data ss:Type=""Number"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                                    End If

                                ElseIf col.Index = 13 Then

                                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""11"" ss:Formula=""=RC[-8]+RC[-5]+RC[-3]-RC[-1]""><Data ss:Type=""Number"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))

                                Else
                                    If valor = 0 Then
                                        fs.WriteLine("      <Cell ss:StyleID=""11""></Cell>")
                                    Else
                                        fs.WriteLine(String.Format("      <Cell ss:StyleID=""11""><Data ss:Type=""Number"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                                    End If

                                End If

                            Else ' Texto

                                If col.Index = 1 Or col.Index = 2 Then
                                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""15""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                                Else
                                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""11""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                                End If

                            End If

                            'End If

                        End If

                    End If

                Next col

                fs.WriteLine("    </Row>")

            Next row

            'Write the separation between results and totals
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("    </Row>")

            'Write the totals 
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""15""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""13""></Cell>")
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""13""><Data ss:Type=""String"">{0}</Data></Cell>", lblTotalNomina.Text))
            'fs.WriteLine(String.Format("      <Cell ss:StyleID=""14""><Data ss:Type=""String"">{0}</Data></Cell>", txtTotalNomina.Text))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""14"" ss:Formula=""=SUM(R[-" & dgvNominas.Rows.Count + 1 & "]C:R[-1]C)""><Data ss:Type=""Number"">{0}</Data></Cell>", txtTotalNomina.Text))
            fs.WriteLine("    </Row>")

            ' Close up the document
            fs.WriteLine("  </Table>")

            fs.WriteLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            fs.WriteLine("   <PageSetup>")
            fs.WriteLine("  <Layout x:Orientation=""Landscape""/>")
            fs.WriteLine("  <Header x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <Footer x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <PageMargins x:Bottom=""0.98425196850393704"" x:Left=""0.74803149606299213"" x:Right=""0.74803149606299213"" x:Top=""0.98425196850393704""/>")
            fs.WriteLine("   </PageSetup>")
            fs.WriteLine("   <Unsynced/>")
            fs.WriteLine("   <Print>")
            fs.WriteLine("  <ValidPrinterInfo/>")
            fs.WriteLine("  <PaperSizeIndex>9</PaperSizeIndex>")
            fs.WriteLine("  <Scale>65</Scale>")
            fs.WriteLine("  <HorizontalResolution>200</HorizontalResolution>")
            fs.WriteLine("  <VerticalResolution>200</VerticalResolution>")
            fs.WriteLine("   </Print>")
            fs.WriteLine("   <Zoom>75</Zoom>")
            fs.WriteLine("   <Selected/>")
            fs.WriteLine("   <Panes>")
            fs.WriteLine("  <Pane>")
            fs.WriteLine("   <Number>3</Number>")
            fs.WriteLine("   <ActiveRow>36</ActiveRow>")
            fs.WriteLine("   <ActiveCol>10</ActiveCol>")
            fs.WriteLine("  </Pane>")
            fs.WriteLine("   </Panes>")
            fs.WriteLine("   <ProtectObjects>False</ProtectObjects>")
            fs.WriteLine("   <ProtectScenarios>False</ProtectScenarios>")
            fs.WriteLine("  </WorksheetOptions>")


            fs.WriteLine("</Worksheet>")
            fs.WriteLine("</Workbook>")
            fs.Close()

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


    Private Sub btnRevisiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevisiones.Click, btnRevisionesPagos.Click

        If MsgBox("Revisar una Nómina automáticamente guarda sus cambios. ¿Deseas guardar esta Nómina ahora?", MsgBoxStyle.YesNo, "Pregunta Guardado") = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesPayrollIsOpen As Integer = 1

        timesPayrollIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payroll" & ipayrollid & "'")

        If timesPayrollIsOpen > 1 Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Nómina. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando esta Nómina?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        Dim queriesSave(10) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM payrolls " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp WHERE payrolls.ipayrollid = tp.ipayrollid) "

        queriesSave(1) = "" & _
        "DELETE " & _
        "FROM payrollpeople " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp WHERE payrollpeople.ipayrollid = tp.ipayrollid AND payrollpeople.ipeopleid = tp.ipeopleid) "

        queriesSave(2) = "" & _
        "DELETE " & _
        "FROM payrollpayments " & _
        "WHERE ipayrollid = " & ipayrollid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp WHERE payrollpayments.ipayrollid = tp.ipayrollid AND payrollpayments.ipaymentid = tp.ipaymentid) "

        queriesSave(3) = "" & _
        "UPDATE payrolls p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp ON tp.ipayrollid = p.ipayrollid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.spayrollfrequency = tp.spayrollfrequency, p.spayrolltype = tp.spayrolltype, p.ipayrolldate = tp.ipayrolldate, p.spayrolltime = tp.spayrolltime, p.ipayrollstartdate = tp.ipayrollstartdate, p.spayrollstarttime = tp.spayrollstarttime, p.ipayrollenddate = tp.ipayrollenddate, p.spayrollendtime = tp.spayrollendtime, p.ipeopleid = tp.ipeopleid, p.iprojectid = tp.iprojectid, p.spayrolldescription = tp.spayrolldescription WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(4) = "" & _
        "UPDATE payrollpeople p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp ON tp.ipayrollid = p.ipayrollid AND tp.ipeopleid = p.ipeopleid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.ddaysworked = tp.ddaysworked, p.ddaysalary = tp.ddaysalary, p.dextrahours = tp.dextrahours, p.dextrahoursalary = tp.dextrahoursalary, p.sdiscount = tp.sdiscount, p.ddiscountamount = tp.ddiscountamount, p.sextraincome = tp.sextraincome, p.dextraincomeamount = tp.dextraincomeamount WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(5) = "" & _
        "UPDATE payrollpayments p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp ON tp.ipayrollid = p.ipayrollid AND tp.ipaymentid = p.ipaymentid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sextranote = tp.sextranote WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(6) = "" & _
        "INSERT INTO payrolls " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrolls p WHERE p.ipayrollid = tp.ipayrollid AND p.ipayrollid = " & ipayrollid & ") "

        queriesSave(7) = "" & _
        "INSERT INTO payrollpeople " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrollpeople p WHERE p.ipayrollid = tp.ipayrollid AND p.ipeopleid = tp.ipeopleid AND p.ipayrollid = " & ipayrollid & ") "

        queriesSave(8) = "" & _
        "INSERT INTO payrollpayments " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments tp " & _
        "WHERE NOT EXISTS (SELECT * FROM payrollpayments p WHERE p.ipayrollid = tp.ipayrollid AND p.ipaymentid = tp.ipaymentid AND p.ipayrollid = " & ipayrollid & ") "

        queriesSave(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Nómina " & ipayrollid & "', 'OK')"

        If executeTransactedSQLCommand(0, queriesSave) = True Then

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

            br.srevisiondocument = "Nómina"
            br.sid = ipayrollid

            If Me.WindowState = FormWindowState.Maximized Then
                br.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            br.ShowDialog(Me)
            Me.Visible = True

        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnPaso2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaso2.Click

        tcNominas.SelectedTab = tpPagos

    End Sub


    Private Sub tcNominas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcNominas.SelectedIndexChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If validaDatosNomina(True) = True And ipayrollid = 0 Then

            ipayrollid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipayrollid) + 1 IS NULL, 1, MAX(ipayrollid) + 1) AS ipayrollid FROM payrolls ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid
            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0People"
            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll0Payments"
            queriesCreation(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments"
            queriesCreation(6) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `spayrollfrequency` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `spayrolltype` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `ipayrolldate` int(11) NOT NULL, `spayrolltime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollstartdate` int(11) NOT NULL, `spayrollstarttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipayrollenddate` int(11) NOT NULL, `spayrollendtime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iprojectid` int(11) NOT NULL, `spayrolldescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `projectid` (`iprojectid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People ( `ipayrollid` int(11) NOT NULL AUTO_INCREMENT, `ipeopleid` int(11) NOT NULL DEFAULT '0', `ddaysworked` decimal(20,5) NOT NULL, `ddaysalary` decimal(20,5) NOT NULL, `dextrahours` decimal(20,5) NOT NULL, `dextrahoursalary` decimal(20,5) NOT NULL, `sextraincome` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dextraincomeamount` decimal(20,5) NOT NULL, `sdiscount` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ddiscountamount` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipeopleid`), KEY `payrollid` (`ipayrollid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments ( `ipayrollid` int(11) NOT NULL, `ipaymentid` int(11) NOT NULL, `sextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipayrollid`,`ipaymentid`) USING BTREE, KEY `payrollid` (`ipayrollid`), KEY `paymentid` (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " VALUES (" & ipayrollid & ", '" & cmbFrecuencia.SelectedItem & "', '" & cmbTipoNomina.SelectedItem & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', " & isupervisorid & ", " & iprojectid & ", '" & txtDescripcionNomina.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If


        ElseIf validaDatosNomina(True) = True And ipayrollid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & " SET spayrollfrequency = '" & cmbFrecuencia.SelectedItem & "', spayrolltype = '" & cmbTipoNomina.SelectedItem & "', ipayrolldate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrolltime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollstartdate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollstarttime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaInicioNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipayrollenddate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spayrollendtime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFinNomina.Value).Substring(10).Trim.Replace(".000", "") & "', ipeopleid = " & isupervisorid & ", iprojectid = '" & iprojectid & "', spayrolldescription = '" & txtDescripcionNomina.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid) = True Then

                dgvNominas.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                If addPersonPermission = True Then
                    btnNuevaPersona.Enabled = True
                    btnInsertarPersona.Enabled = True
                End If
                If deletePersonPermission = True Then
                    btnEliminarPersona.Enabled = True
                End If

            End If

        End If


        If validaDatosNomina(True) = True And (tcNominas.SelectedTab Is tpDatos) = False Then

            'Continue


            If validaPagos(True) = True And (tcNominas.SelectedTab Is tpDatos) = False And (tcNominas.SelectedTab Is tpPagos) = False Then
                'Continue
            Else
                tcNominas.SelectedTab = tpPagos
            End If


            If tcNominas.SelectedTab Is tpDatos Then


                Dim queryPayrolls As String = ""

                queryPayrolls = "" & _
                "SELECT pp.ipeopleid, p.speoplefullname AS 'Persona', p.speopleobservations AS 'Datos Adicionales Persona', FORMAT(pp.ddaysworked, 2) AS 'Dias trabajados', " & _
                "FORMAT(pp.ddaysalary, 2) AS 'Sueldo diario', FORMAT(pp.ddaysworked * pp.ddaysalary, 2) AS 'Subtotal Sueldo', " & _
                "FORMAT(pp.dextrahours, 2) AS 'Horas extras trabajadas', FORMAT(pp.dextrahoursalary, 2) AS 'Sueldo Hora Extra', " & _
                "FORMAT(pp.dextrahours * pp.dextrahoursalary, 2) AS 'Subtotal Horas Extras', " & _
                "sextraincome AS 'Concepto Ingreso Extra (si alguno)', FORMAT(pp.dextraincomeamount, 2) AS 'Monto Ingreso Extra', " & _
                "sdiscount AS 'Concepto Descuento via Nomina (si alguno)', FORMAT(pp.ddiscountamount, 2) AS 'Monto Descuento', " & _
                "FORMAT((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount, 2) AS 'Total A Pagar' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp " & _
                "JOIN people p ON pp.ipeopleid = p.ipeopleid " & _
                "WHERE pp.ipayrollid = " & ipayrollid & " " & _
                "ORDER BY 2 "

                setDataGridView(dgvNominas, queryPayrolls, False)

                dgvNominas.Columns(0).Visible = False

                dgvNominas.Columns(1).ReadOnly = True
                dgvNominas.Columns(2).ReadOnly = True
                dgvNominas.Columns(5).ReadOnly = True
                dgvNominas.Columns(8).ReadOnly = True
                dgvNominas.Columns(13).ReadOnly = True

                dgvNominas.Columns(1).Width = 150
                dgvNominas.Columns(2).Width = 100
                dgvNominas.Columns(3).Width = 70
                dgvNominas.Columns(4).Width = 70
                dgvNominas.Columns(5).Width = 70
                dgvNominas.Columns(6).Width = 70
                dgvNominas.Columns(7).Width = 70
                dgvNominas.Columns(8).Width = 70
                dgvNominas.Columns(9).Width = 150
                dgvNominas.Columns(10).Width = 70
                dgvNominas.Columns(11).Width = 150
                dgvNominas.Columns(12).Width = 70
                dgvNominas.Columns(13).Width = 70

                txtTotalNomina.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount - ddiscountamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtTotalNominaSinDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount) IS NULL, 0, SUM((pp.ddaysworked * pp.ddaysalary) + (pp.dextrahours * pp.dextrahoursalary) + dextraincomeamount)), 2) AS total FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "People pp WHERE pp.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            ElseIf tcNominas.SelectedTab Is tpPagos Then


                Dim queryPayrollPayments As String

                queryPayrollPayments = "" & _
                "SELECT prpy.ipaymentid, prpy.ipayrollid, " & _
                "STR_TO_DATE(CONCAT(prpy.iupdatedate, ' ', prpy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
                "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
                "py.spaymentdescription AS 'Descripcion', prpy.sextranote AS Observaciones " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy " & _
                "JOIN payments py ON prpy.ipaymentid = py.ipaymentid " & _
                "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "WHERE prpy.ipayrollid = " & ipayrollid

                setDataGridView(dgvPagos, queryPayrollPayments, False)

                dgvPagos.Columns(0).Visible = False
                dgvPagos.Columns(1).Visible = False

                dgvPagos.Columns(0).ReadOnly = True
                dgvPagos.Columns(1).ReadOnly = True
                dgvPagos.Columns(2).ReadOnly = True
                dgvPagos.Columns(3).ReadOnly = True
                dgvPagos.Columns(4).ReadOnly = True
                dgvPagos.Columns(5).ReadOnly = True

                dgvPagos.Columns(0).Width = 30
                dgvPagos.Columns(1).Width = 30
                dgvPagos.Columns(2).Width = 70
                dgvPagos.Columns(3).Width = 70
                dgvPagos.Columns(4).Width = 100
                dgvPagos.Columns(5).Width = 200
                dgvPagos.Columns(6).Width = 200

                txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpy.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(prpe.montonomina IS NULL, 0, prpe.montonomina) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM (SELECT ipayrollid, ipeopleid, SUM((ddaysworked * ddaysalary) + (dextrahours * dextrahoursalary) - ddiscountamount) AS montonomina FROM tmpMemozebaduaS1Payroll1People) prpe LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy ON prpe.ipayrollid = prpy.ipayrollid LEFT JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpe.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            End If


        Else

            tcNominas.SelectedTab = tpDatos

        End If

    End Sub


    Private Sub dgvPagos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPagos.CellClick

        Try

            iselectedpaymentid = CInt(dgvPagos.Rows(e.RowIndex).Cells(0).Value())

        Catch ex As Exception

            iselectedpaymentid = 0

        End Try

    End Sub


    Private Sub dgvPagos_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPagos.CellContentClick

        Try

            iselectedpaymentid = CInt(dgvPagos.Rows(e.RowIndex).Cells(0).Value())

        Catch ex As Exception

            iselectedpaymentid = 0

        End Try

    End Sub


    Private Sub dgvPagos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvPagos.SelectionChanged

        Try

            iselectedpaymentid = CInt(dgvPagos.CurrentRow.Cells(0).Value())

        Catch ex As Exception

            iselectedpaymentid = 0

        End Try

    End Sub


    Private Sub dgvPagos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPagos.CellValueChanged

        If modifyPaymentPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If e.ColumnIndex = 6 Then

            dgvPagos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvPagos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments SET sextranote = '" & dgvPagos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ipayrollid = " & ipayrollid & " AND ipaymentid = " & iselectedpaymentid)

            txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpy.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(prpe.montonomina IS NULL, 0, prpe.montonomina) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM (SELECT ipayrollid, ipeopleid, SUM((ddaysworked * ddaysalary) + (dextrahours * dextrahoursalary) - ddiscountamount) AS montonomina FROM tmpMemozebaduaS1Payroll1People) prpe LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy ON prpe.ipayrollid = prpy.ipayrollid LEFT JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpe.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvPagos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvPagos.EditingControlShowing

        If dgvPagos.CurrentCell.ColumnIndex = 6 Then

            txtNotaDgvNominas = CType(e.Control, TextBox)
            txtNotaDgvNominas_OldText = txtNotaDgvNominas.Text

        Else

            txtNotaDgvNominas = Nothing
            txtNotaDgvNominas_OldText = Nothing

        End If

    End Sub


    Private Sub dgvPagos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPagos.CellEndEdit

        Dim queryPayrollPayments As String

        queryPayrollPayments = "" & _
        "SELECT prpy.ipaymentid, prpy.ipayrollid, " & _
        "STR_TO_DATE(CONCAT(prpy.iupdatedate, ' ', prpy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
        "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
        "py.spaymentdescription AS 'Descripcion', prpy.sextranote AS Observaciones " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy " & _
        "JOIN payments py ON prpy.ipaymentid = py.ipaymentid " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "WHERE prpy.ipayrollid = " & ipayrollid

        setDataGridView(dgvPagos, queryPayrollPayments, False)

        dgvPagos.Columns(0).Visible = False
        dgvPagos.Columns(1).Visible = False

        dgvPagos.Columns(0).ReadOnly = True
        dgvPagos.Columns(1).ReadOnly = True
        dgvPagos.Columns(2).ReadOnly = True
        dgvPagos.Columns(3).ReadOnly = True
        dgvPagos.Columns(4).ReadOnly = True
        dgvPagos.Columns(5).ReadOnly = True

        dgvPagos.Columns(0).Width = 30
        dgvPagos.Columns(1).Width = 30
        dgvPagos.Columns(2).Width = 70
        dgvPagos.Columns(3).Width = 70
        dgvPagos.Columns(4).Width = 100
        dgvPagos.Columns(5).Width = 200
        dgvPagos.Columns(6).Width = 200

    End Sub


    Private Sub dgvPagos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvPagos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deletePaymentPermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Realmente deseas borrar la relación de este Pago con la Nómina?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Pago Nómina") = MsgBoxResult.Yes Then

                Dim queriesDelete(1) As String

                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments WHERE ipayrollid = " & ipayrollid & " AND ipaymentid = " & iselectedpaymentid
                'queriesDelete(1) = "DELETE FROM Payments WHERE ipaymentid = " & iselectedpaymentid

                executeTransactedSQLCommand(0, queriesDelete)

                Dim queryPayrollPayments As String

                queryPayrollPayments = "" & _
                "SELECT prpy.ipaymentid, prpy.ipayrollid, " & _
                "STR_TO_DATE(CONCAT(prpy.iupdatedate, ' ', prpy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
                "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
                "py.spaymentdescription AS 'Descripcion', prpy.sextranote AS Observaciones " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy " & _
                "JOIN payments py ON prpy.ipaymentid = py.ipaymentid " & _
                "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "WHERE prpy.ipayrollid = " & ipayrollid

                setDataGridView(dgvPagos, queryPayrollPayments, False)

                dgvPagos.Columns(0).Visible = False
                dgvPagos.Columns(1).Visible = False

                dgvPagos.Columns(0).ReadOnly = True
                dgvPagos.Columns(1).ReadOnly = True
                dgvPagos.Columns(2).ReadOnly = True
                dgvPagos.Columns(3).ReadOnly = True
                dgvPagos.Columns(4).ReadOnly = True
                dgvPagos.Columns(5).ReadOnly = True

                dgvPagos.Columns(0).Width = 30
                dgvPagos.Columns(1).Width = 30
                dgvPagos.Columns(2).Width = 70
                dgvPagos.Columns(3).Width = 70
                dgvPagos.Columns(4).Width = 100
                dgvPagos.Columns(5).Width = 200
                dgvPagos.Columns(6).Width = 200

                txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpy.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(prpe.montonomina IS NULL, 0, prpe.montonomina) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM (SELECT ipayrollid, ipeopleid, SUM((ddaysworked * ddaysalary) + (dextrahours * dextrahoursalary) - ddiscountamount) AS montonomina FROM tmpMemozebaduaS1Payroll1People) prpe LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy ON prpe.ipayrollid = prpy.ipayrollid LEFT JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpe.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            End If

        End If

    End Sub


    Private Sub dgvPagos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvPagos.UserAddedRow

        If insertPaymentPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bi As New BuscaPagos

        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname

        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.querystring = dgvPagos.CurrentCell.EditedFormattedValue

        bi.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments VALUES ( " & bi.ipaymentid & ", " & ipayrollid & ", '', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

            txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpy.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(prpe.montonomina IS NULL, 0, prpe.montonomina) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM (SELECT ipayrollid, ipeopleid, SUM((ddaysworked * ddaysalary) + (dextrahours * dextrahoursalary) - ddiscountamount) AS montonomina FROM tmpMemozebaduaS1Payroll1People) prpe LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy ON prpe.ipayrollid = prpy.ipayrollid LEFT JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpe.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        'dgvPagos.EndEdit()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnAgregarPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarPago.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ai As New AgregarPago

        ai.susername = susername
        ai.bactive = bactive
        ai.bonline = bonline
        ai.suserfullname = suserfullname

        ai.suseremail = suseremail
        ai.susersession = susersession
        ai.susermachinename = susermachinename
        ai.suserip = suserip

        ai.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ai.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ai.ShowDialog(Me)
        Me.Visible = True

        If ai.DialogResult = Windows.Forms.DialogResult.OK Then

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments VALUES ( " & ipayrollid & ", " & ai.ipaymentid & ", '', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        End If

        Dim queryPayrollPayments As String

        queryPayrollPayments = "" & _
        "SELECT prpy.ipaymentid, prpy.ipayrollid, " & _
        "STR_TO_DATE(CONCAT(prpy.iupdatedate, ' ', prpy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
        "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
        "py.spaymentdescription AS 'Descripcion', prpy.sextranote AS Observaciones " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy " & _
        "JOIN payments py ON prpy.ipaymentid = py.ipaymentid " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "WHERE prpy.ipayrollid = " & ipayrollid

        setDataGridView(dgvPagos, queryPayrollPayments, False)

        dgvPagos.Columns(0).Visible = False
        dgvPagos.Columns(1).Visible = False

        dgvPagos.Columns(0).ReadOnly = True
        dgvPagos.Columns(1).ReadOnly = True
        dgvPagos.Columns(2).ReadOnly = True
        dgvPagos.Columns(3).ReadOnly = True
        dgvPagos.Columns(4).ReadOnly = True
        dgvPagos.Columns(5).ReadOnly = True

        dgvPagos.Columns(0).Width = 30
        dgvPagos.Columns(1).Width = 30
        dgvPagos.Columns(2).Width = 70
        dgvPagos.Columns(3).Width = 70
        dgvPagos.Columns(4).Width = 100
        dgvPagos.Columns(5).Width = 200
        dgvPagos.Columns(6).Width = 200

        txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpy.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
        txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(prpe.montonomina IS NULL, 0, prpe.montonomina) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM (SELECT ipayrollid, ipeopleid, SUM((ddaysworked * ddaysalary) + (dextrahours * dextrahoursalary) - ddiscountamount) AS montonomina FROM tmpMemozebaduaS1Payroll1People) prpe LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy ON prpe.ipayrollid = prpy.ipayrollid LEFT JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpe.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnInsertarPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarPago.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bi As New BuscaPagos

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

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments VALUES ( " & ipayrollid & ", " & bi.ipaymentid & ", '', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        End If

        Dim queryPayrollPayments As String

        queryPayrollPayments = "" & _
        "SELECT prpy.ipaymentid, prpy.ipayrollid, " & _
        "STR_TO_DATE(CONCAT(prpy.iupdatedate, ' ', prpy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
        "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
        "py.spaymentdescription AS 'Descripcion', prpy.sextranote AS Observaciones " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy " & _
        "JOIN payments py ON prpy.ipaymentid = py.ipaymentid " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "WHERE prpy.ipayrollid = " & ipayrollid

        setDataGridView(dgvPagos, queryPayrollPayments, False)

        dgvPagos.Columns(0).Visible = False
        dgvPagos.Columns(1).Visible = False

        dgvPagos.Columns(0).ReadOnly = True
        dgvPagos.Columns(1).ReadOnly = True
        dgvPagos.Columns(2).ReadOnly = True
        dgvPagos.Columns(3).ReadOnly = True
        dgvPagos.Columns(4).ReadOnly = True
        dgvPagos.Columns(5).ReadOnly = True
        dgvPagos.Columns(6).ReadOnly = False

        dgvPagos.Columns(0).Width = 30
        dgvPagos.Columns(1).Width = 30
        dgvPagos.Columns(2).Width = 70
        dgvPagos.Columns(3).Width = 70
        dgvPagos.Columns(4).Width = 100
        dgvPagos.Columns(5).Width = 200
        dgvPagos.Columns(6).Width = 200

        txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpy.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
        txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(prpe.montonomina IS NULL, 0, prpe.montonomina) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM (SELECT ipayrollid, ipeopleid, SUM((ddaysworked * ddaysalary) + (dextrahours * dextrahoursalary) - ddiscountamount) AS montonomina FROM tmpMemozebaduaS1Payroll1People) prpe LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy ON prpe.ipayrollid = prpy.ipayrollid LEFT JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpe.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarPago.Click

        If MsgBox("¿Realmente deseas borrar la relación de este Pago con la Nómina?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Pago - Nómina") = MsgBoxResult.Yes Then

            Dim queriesDelete(1) As String

            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments WHERE ipayrollid = " & ipayrollid & " AND ipaymentid = " & iselectedpaymentid
            'queriesDelete(1) = "DELETE FROM Payments WHERE ipaymentid = " & iselectedpaymentid

            executeTransactedSQLCommand(0, queriesDelete)

            Dim queryPayrollPayments As String

            queryPayrollPayments = "" & _
            "SELECT prpy.ipaymentid, prpy.ipayrollid, " & _
            "STR_TO_DATE(CONCAT(prpy.iupdatedate, ' ', prpy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
            "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
            "py.spaymentdescription AS 'Descripcion', prpy.sextranote AS Observaciones " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy " & _
            "JOIN payments py ON prpy.ipaymentid = py.ipaymentid " & _
            "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "WHERE prpy.ipayrollid = " & ipayrollid

            setDataGridView(dgvPagos, queryPayrollPayments, False)

            dgvPagos.Columns(0).Visible = False
            dgvPagos.Columns(1).Visible = False

            dgvPagos.Columns(0).ReadOnly = True
            dgvPagos.Columns(1).ReadOnly = True
            dgvPagos.Columns(2).ReadOnly = True
            dgvPagos.Columns(3).ReadOnly = True
            dgvPagos.Columns(4).ReadOnly = True
            dgvPagos.Columns(5).ReadOnly = True

            dgvPagos.Columns(0).Width = 30
            dgvPagos.Columns(1).Width = 30
            dgvPagos.Columns(2).Width = 70
            dgvPagos.Columns(3).Width = 70
            dgvPagos.Columns(4).Width = 100
            dgvPagos.Columns(5).Width = 200
            dgvPagos.Columns(6).Width = 200

            txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpy.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(prpe.montonomina IS NULL, 0, prpe.montonomina) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM (SELECT ipayrollid, ipeopleid, SUM((ddaysworked * ddaysalary) + (dextrahours * dextrahoursalary) - ddiscountamount) AS montonomina FROM tmpMemozebaduaS1Payroll1People) prpe LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy ON prpe.ipayrollid = prpy.ipayrollid LEFT JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpe.ipayrollid = " & ipayrollid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

    End Sub


    Private Function validaPagos(ByVal silent As Boolean) As Boolean

        Dim queryResta As String = ""
        Dim resta As Double = 0.0

        queryResta = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(prpe.montonomina IS NULL, 0, prpe.montonomina) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM (SELECT ipayrollid, ipeopleid, SUM((ddaysworked * ddaysalary) + (dextrahours * dextrahoursalary) - ddiscountamount) AS montonomina FROM tmpMemozebaduaS1Payroll1People) prpe LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payroll" & ipayrollid & "Payments prpy ON prpe.ipayrollid = prpy.ipayrollid LEFT JOIN payments py ON prpy.ipaymentid = py.ipaymentid WHERE prpe.ipayrollid = " & ipayrollid)

        resta = getSQLQueryAsDouble(0, queryResta)

        If resta > 0.0 Then

            If silent = False Then
                MsgBox("Nota: Hay saldo pendiente para esta Nómina", MsgBoxStyle.OkOnly, "Información")
            End If

            Return True

        ElseIf resta < 0.0 Then

            If silent = False Then
                MsgBox("Nota: Hay saldo a favor para esta Nómina", MsgBoxStyle.OkOnly, "Información")
            End If

            Return True

        End If

        Return True

    End Function


End Class