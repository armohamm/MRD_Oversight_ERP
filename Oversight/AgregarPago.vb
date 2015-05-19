Public Class AgregarPago

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

    Public ipaymentid As Integer = 0
    Public spaymentdescription As String = ""
    Public dpaymentamount As Double = 0.0
    Public ipeopleid As Integer = 0

    Private isFormReadyForAction As Boolean = False

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

                If permission = "Abrir Tipos de Pago" Then
                    btnTipoPago.Enabled = True
                End If

                If permission = "Abrir Bancos" Then
                    btnBancoDestino.Enabled = True
                End If

                If permission = "Abrir Cuentas" Then
                    btnCuentaOrigen.Enabled = True
                End If

                If permission = "Abrir Personas" Then
                    btnPersonas.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Pago', 'OK')")

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


    Private Sub AgregarPago_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM payments " & _
        "WHERE ipaymentid = " & ipaymentid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc WHERE payments.ipaymentid = tclc.ipaymentid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc JOIN payments clc ON tclc.ipaymentid = clc.ipaymentid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM payments clc WHERE tclc.ipaymentid = clc.ipaymentid AND clc.ipaymentid = " & ipaymentid & ") ")

        If conteo1 + conteo2 + conteo3 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaPago(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Pago está incompleto. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesPaymentIsOpen As Integer = 1

            timesPaymentIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payment" & ipaymentid & "'")

            If timesPaymentIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Pago. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Pago?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesPaymentIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payment" & ipaymentid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(4) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid + newIdAddition
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid + newIdAddition & " SET ipaymentid = " & ipaymentid + newIdAddition & " WHERE ipaymentid = " & ipaymentid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    ipaymentid = ipaymentid + newIdAddition
                End If

            End If


            Dim queries(4) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM payments " & _
            "WHERE ipaymentid = " & ipaymentid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc WHERE payments.ipaymentid = tclc.ipaymentid) "

            queries(1) = "" & _
            "UPDATE payments clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc ON tclc.ipaymentid = clc.ipaymentid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipaymentdate = tclc.ipaymentdate, clc.spaymenttime = tclc.spaymenttime, clc.ipaymenttypeid = tclc.ipaymenttypeid, clc.ioriginaccountid = tclc.ioriginaccountid, clc.idestinationbankid = tclc.idestinationbankid, clc.sdestinationaccount = tclc.sdestinationaccount, clc.sdestinationreference = tclc.sdestinationreference, clc.spaymentdescription = tclc.spaymentdescription, clc.dpaymentamount = tclc.dpaymentamount, clc.ipeopleid = tclc.ipeopleid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(2) = "" & _
            "INSERT INTO payments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM payments clc WHERE tclc.ipaymentid = clc.ipaymentid AND clc.ipaymentid = " & ipaymentid & ") "

            queries(3) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Pago " & ipaymentid & " : " & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

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

        Dim queriesDelete(3) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid
        queriesDelete(1) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Pago " & ipaymentid & " : " & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & "', 'OK')"
        queriesDelete(2) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Payment', 'Pago', '" & ipaymentid & "', '" & txtDescripcionPago.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarPago_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarPago_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesPaymentIsOpen As Integer = 0

        timesPaymentIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payment" & ipaymentid & "'")

        If timesPaymentIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Pago. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Pago?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        If isRecover = False Then

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " ( `ipaymentid` int(11) NOT NULL AUTO_INCREMENT, `ipaymentdate` int(11) NOT NULL, `spaymenttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipaymenttypeid` int(11) NOT NULL, `ioriginaccountid` int(11) DEFAULT NULL, `idestinationbankid` int(11) DEFAULT NULL, `sdestinationaccount` varchar(100) COLLATE latin1_spanish_ci DEFAULT NULL, `sdestinationreference` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `spaymentdescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dpaymentamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        cmbTipoPago.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM paymenttypes")
        cmbTipoPago.DisplayMember = "spaymenttypedescription"
        cmbTipoPago.ValueMember = "ipaymenttypeid"
        cmbTipoPago.SelectedIndex = -1

        cmbCuentaOrigen.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM companyaccounts")
        cmbCuentaOrigen.DisplayMember = "scompanyaccountname"
        cmbCuentaOrigen.ValueMember = "iaccountid"
        cmbCuentaOrigen.SelectedIndex = -1

        cmbBancoDestino.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM banks")
        cmbBancoDestino.DisplayMember = "sbankname"
        cmbBancoDestino.ValueMember = "ibankid"
        cmbBancoDestino.SelectedIndex = -1

        If isEdit = False Then

            txtCuentaDestino.Text = ""
            txtReferenciaDestino.Text = ""
            txtDescripcionPago.Text = ""
            txtImporte.Text = "0.00"

        Else

            If isRecover = False Then

                Dim queriesInsert(1) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " SELECT * FROM payments WHERE ipaymentid = " & ipaymentid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsPago As DataSet
            dsPago = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " WHERE ipaymentid = " & ipaymentid)

            Try

                If dsPago.Tables(0).Rows.Count > 0 Then

                    dtFechaPago.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsPago.Tables(0).Rows(0).Item("ipaymentdate")) & " " & dsPago.Tables(0).Rows(0).Item("spaymenttime")

                    cmbCuentaOrigen.SelectedValue = dsPago.Tables(0).Rows(0).Item("ioriginaccountid")
                    cmbTipoPago.SelectedValue = dsPago.Tables(0).Rows(0).Item("ipaymenttypeid")

                    If cmbTipoPago.SelectedValue > 1 Then
                        txtReferenciaDestino.Text = dsPago.Tables(0).Rows(0).Item("sdestinationreference")
                        cmbBancoDestino.SelectedValue = dsPago.Tables(0).Rows(0).Item("idestinationbankid")
                        txtCuentaDestino.Text = dsPago.Tables(0).Rows(0).Item("sdestinationaccount")
                    Else
                        txtReferenciaDestino.Enabled = False
                        cmbBancoDestino.Enabled = False
                        btnBancoDestino.Enabled = False
                        txtCuentaDestino.Enabled = False
                    End If

                    txtDescripcionPago.Text = dsPago.Tables(0).Rows(0).Item("spaymentdescription")
                    txtImporte.Text = FormatCurrency(dsPago.Tables(0).Rows(0).Item("dpaymentamount"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    ipeopleid = dsPago.Tables(0).Rows(0).Item("ipeopleid")

                    txtNombrePersona.Text = getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = " & dsPago.Tables(0).Rows(0).Item("ipeopleid"))

                End If

            Catch ex As Exception

            End Try

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Pago " & ipaymentid & " : " & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Payment', 'Pago', '" & ipaymentid & "', '" & txtDescripcionPago.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True

        dtFechaPago.Select()
        dtFechaPago.Focus()

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


    Private Sub cmbTipoPago_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoPago.SelectedIndexChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If cmbTipoPago.SelectedValue = 1 Then

            cmbBancoDestino.SelectedIndex = -1
            cmbBancoDestino.Enabled = False
            btnBancoDestino.Enabled = False
            txtCuentaDestino.Text = ""
            txtCuentaDestino.Enabled = False
            txtReferenciaDestino.Text = ""
            txtReferenciaDestino.Enabled = False

        Else

            cmbBancoDestino.Enabled = True
            btnBancoDestino.Enabled = True
            txtCuentaDestino.Enabled = True
            txtReferenciaDestino.Enabled = True

        End If

    End Sub


    Private Sub btnTipoPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoPago.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim btp As New BuscaTipoPagos

        btp.susername = susername
        btp.bactive = bactive
        btp.bonline = bonline
        btp.suserfullname = suserfullname
        btp.suseremail = suseremail
        btp.susersession = susersession
        btp.susermachinename = susermachinename
        btp.suserip = suserip

        btp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            btp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        btp.ShowDialog(Me)
        Me.Visible = True

        If btp.DialogResult = Windows.Forms.DialogResult.OK Then

            cmbTipoPago.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM paymenttypes")
            cmbTipoPago.DisplayMember = "spaymenttypedescription"
            cmbTipoPago.ValueMember = "ipaymenttypeid"
            cmbTipoPago.SelectedIndex = -1

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCuentaOrigen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCuentaOrigen.Click

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

            cmbCuentaOrigen.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM companyaccounts")
            cmbCuentaOrigen.DisplayMember = "scompanyaccountname"
            cmbCuentaOrigen.ValueMember = "iaccountid"
            cmbCuentaOrigen.SelectedIndex = -1

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnBancoDestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBancoDestino.Click

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

            cmbBancoDestino.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM banks")
            cmbBancoDestino.DisplayMember = "sbankname"
            cmbBancoDestino.ValueMember = "ibankid"
            cmbBancoDestino.SelectedIndex = -1

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtCuentaDestino_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCuentaDestino.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtCuentaDestino.Text.Contains(arrayCaractProhib(carp)) Then
                txtCuentaDestino.Text = txtCuentaDestino.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtCuentaDestino.Select(txtCuentaDestino.Text.Length, 0)
        End If

        txtCuentaDestino.Text = txtCuentaDestino.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtReferenciaDestino_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtReferenciaDestino.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtReferenciaDestino.Text.Contains(arrayCaractProhib(carp)) Then
                txtReferenciaDestino.Text = txtReferenciaDestino.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtReferenciaDestino.Select(txtReferenciaDestino.Text.Length, 0)
        End If

        txtReferenciaDestino.Text = txtReferenciaDestino.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtDescripcionPago_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDescripcionPago.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtDescripcionPago.Text.Contains(arrayCaractProhib(carp)) Then
                txtDescripcionPago.Text = txtDescripcionPago.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtDescripcionPago.Select(txtDescripcionPago.Text.Length, 0)
        End If

        txtDescripcionPago.Text = txtDescripcionPago.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtImporte_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtImporte.KeyUp, txtImporte.KeyUp

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


    Private Function validaPago(ByVal silent As Boolean) As Boolean

        txtCuentaDestino.Text = txtCuentaDestino.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtReferenciaDestino.Text = txtReferenciaDestino.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtDescripcionPago.Text = txtDescripcionPago.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtImporte.Text = txtImporte.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If cmbTipoPago.SelectedIndex = -1 Then

            If silent = False Then
                MsgBox("¿Podrías seleccionar un tipo de Pago?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            cmbTipoPago.Select()
            cmbTipoPago.Focus()
            Return False

        End If

        If cmbCuentaOrigen.SelectedIndex = -1 Then

            If silent = False Then
                MsgBox("¿Podrías seleccionar la Cuenta de donde se está sacando el dinero del Pago?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            cmbCuentaOrigen.Select()
            cmbCuentaOrigen.Focus()
            Return False

        End If

        If txtDescripcionPago.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner una Descripción al Pago?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtDescripcionPago.Select()
            txtDescripcionPago.Focus()
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
                MsgBox("¿Podrías poner un Importe a Pagar?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtImporte.Select()
            txtImporte.Focus()
            Return False

        End If

        If ipeopleid = 0 Then

            If silent = False Then
                MsgBox("¿Podrías poner quien va a entregar/depositar el dinero?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            btnPersonas.Select()
            btnPersonas.Focus()
            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'ipaymentid = 0
        'spaymentdescription = ""
        'dpaymentamount = 0.0
        'ipeopleid = 0

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaPago(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesPaymentIsOpen As Integer = 1

        timesPaymentIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payment" & ipaymentid & "'")

        If timesPaymentIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Pago. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Pago?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesPaymentIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payment" & ipaymentid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(4) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid + newIdAddition
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid + newIdAddition & " SET ipaymentid = " & ipaymentid + newIdAddition & " WHERE ipaymentid = " & ipaymentid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                ipaymentid = ipaymentid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0
        Dim valueTipoPago As Integer = 0
        Dim valueBanco As Integer = 0
        Dim valueCuenta As Integer = 0


        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try


        Try
            valueTipoPago = cmbTipoPago.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueBanco = cmbBancoDestino.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueCuenta = cmbCuentaOrigen.SelectedValue
        Catch ex As Exception

        End Try


        If ipaymentid = 0 Then

            Dim queriesCreation(3) As String

            ipaymentid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipaymentid) + 1 IS NULL, 1, MAX(ipaymentid) + 1) AS ipaymentid FROM payments ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " ( `ipaymentid` int(11) NOT NULL AUTO_INCREMENT, `ipaymentdate` int(11) NOT NULL, `spaymenttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipaymenttypeid` int(11) NOT NULL, `ioriginaccountid` int(11) DEFAULT NULL, `idestinationbankid` int(11) DEFAULT NULL, `sdestinationaccount` varchar(100) COLLATE latin1_spanish_ci DEFAULT NULL, `sdestinationreference` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `spaymentdescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dpaymentamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " VALUES (" & ipaymentid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaPago.Value) & ", '00:00:00', " & valueTipoPago & ", " & valueCuenta & ", " & valueBanco & ", '" & txtCuentaDestino.Text.Replace("--", "").Replace("'", "") & "', '" & txtReferenciaDestino.Text.Replace("--", "").Replace("'", "") & "', '" & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            spaymentdescription = txtDescripcionPago.Text.Replace("--", "").Replace("'", "")
            dpaymentamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " SET ipaymentdate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaPago.Value) & ", spaymenttime = '00:00:00', ipaymenttypeid = " & valueTipoPago & ", ioriginaccountid = " & valueCuenta & ", idestinationbankid = " & valueBanco & ", sdestinationaccount = '" & txtCuentaDestino.Text.Replace("--", "").Replace("'", "") & "', sdestinationreference = '" & txtReferenciaDestino.Text.Replace("--", "").Replace("'", "") & "', spaymentdescription = '" & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & ", dpaymentamount = " & importe & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipaymentid = " & ipaymentid)

            spaymentdescription = txtDescripcionPago.Text.Replace("--", "").Replace("'", "")
            dpaymentamount = importe

        End If

        Dim queries(4) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM payments " & _
        "WHERE ipaymentid = " & ipaymentid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc WHERE payments.ipaymentid = tclc.ipaymentid) "

        queries(1) = "" & _
        "UPDATE payments clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc ON tclc.ipaymentid = clc.ipaymentid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipaymentdate = tclc.ipaymentdate, clc.spaymenttime = tclc.spaymenttime, clc.ipaymenttypeid = tclc.ipaymenttypeid, clc.ioriginaccountid = tclc.ioriginaccountid, clc.idestinationbankid = tclc.idestinationbankid, clc.sdestinationaccount = tclc.sdestinationaccount, clc.sdestinationreference = tclc.sdestinationreference, clc.spaymentdescription = tclc.spaymentdescription, clc.dpaymentamount = tclc.dpaymentamount, clc.ipeopleid = tclc.ipeopleid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO payments " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM payments clc WHERE tclc.ipaymentid = clc.ipaymentid AND clc.ipaymentid = " & ipaymentid & ") "

        queries(3) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Pago " & ipaymentid & " : " & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then

            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")

            wasCreated = True
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Else

            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnRevisiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevisiones.Click

        If MsgBox("Revisar un Pago automáticamente guarda sus cambios. ¿Deseas guardar este Pago ahora?", MsgBoxStyle.YesNo, "Pregunta Guardado") = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesPaymentIsOpen As Integer = 1

        timesPaymentIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payment" & ipaymentid & "'")

        If timesPaymentIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Pago. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Pago?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesPaymentIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Payment" & ipaymentid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(4) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid + newIdAddition
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid + newIdAddition & " SET ipaymentid = " & ipaymentid + newIdAddition & " WHERE ipaymentid = " & ipaymentid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                ipaymentid = ipaymentid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim importe As Double = 0.0
        Dim valueTipoPago As Integer = 0
        Dim valueBanco As Integer = 0
        Dim valueCuenta As Integer = 0


        Try
            importe = CDbl(txtImporte.Text)
        Catch ex As Exception
            importe = 0.0
        End Try


        Try
            valueTipoPago = cmbTipoPago.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueBanco = cmbBancoDestino.SelectedValue
        Catch ex As Exception

        End Try

        Try
            valueCuenta = cmbCuentaOrigen.SelectedValue
        Catch ex As Exception

        End Try


        If ipaymentid = 0 Then

            Dim queriesCreation(3) As String

            ipaymentid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipaymentid) + 1 IS NULL, 1, MAX(ipaymentid) + 1) AS ipaymentid FROM payments ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment0"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " ( `ipaymentid` int(11) NOT NULL AUTO_INCREMENT, `ipaymentdate` int(11) NOT NULL, `spaymenttime` varchar(11) CHARACTER SET latin1 NOT NULL, `ipaymenttypeid` int(11) NOT NULL, `ioriginaccountid` int(11) DEFAULT NULL, `idestinationbankid` int(11) DEFAULT NULL, `sdestinationaccount` varchar(100) COLLATE latin1_spanish_ci DEFAULT NULL, `sdestinationreference` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `spaymentdescription` varchar(500) CHARACTER SET latin1 NOT NULL, `dpaymentamount` decimal(65,4) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaymentid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " VALUES (" & ipaymentid & ", " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaPago.Value) & ", '00:00:00', " & valueTipoPago & ", " & valueCuenta & ", " & valueBanco & ", '" & txtCuentaDestino.Text.Replace("--", "").Replace("'", "") & "', '" & txtReferenciaDestino.Text.Replace("--", "").Replace("'", "") & "', '" & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & "', " & importe & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            spaymentdescription = txtDescripcionPago.Text.Replace("--", "").Replace("'", "")
            dpaymentamount = importe

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " SET ipaymentdate = " & convertDDdashMMdashYYYYtoYYYYMMDD(dtFechaPago.Value) & ", spaymenttime = '00:00:00', ipaymenttypeid = " & valueTipoPago & ", ioriginaccountid = " & valueCuenta & ", idestinationbankid = " & valueBanco & ", sdestinationaccount = '" & txtCuentaDestino.Text.Replace("--", "").Replace("'", "") & "', sdestinationreference = '" & txtReferenciaDestino.Text.Replace("--", "").Replace("'", "") & "', spaymentdescription = '" & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & ", dpaymentamount = " & importe & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipaymentid = " & ipaymentid)

            spaymentdescription = txtDescripcionPago.Text.Replace("--", "").Replace("'", "")
            dpaymentamount = importe

        End If

        Dim queries(4) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM payments " & _
        "WHERE ipaymentid = " & ipaymentid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc WHERE payments.ipaymentid = tclc.ipaymentid) "

        queries(1) = "" & _
        "UPDATE payments clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc ON tclc.ipaymentid = clc.ipaymentid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ipaymentdate = tclc.ipaymentdate, clc.spaymenttime = tclc.spaymenttime, clc.ipaymenttypeid = tclc.ipaymenttypeid, clc.ioriginaccountid = tclc.ioriginaccountid, clc.idestinationbankid = tclc.idestinationbankid, clc.sdestinationaccount = tclc.sdestinationaccount, clc.sdestinationreference = tclc.sdestinationreference, clc.spaymentdescription = tclc.spaymentdescription, clc.dpaymentamount = tclc.dpaymentamount, clc.ipeopleid = tclc.ipeopleid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO payments " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Payment" & ipaymentid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM payments clc WHERE tclc.ipaymentid = clc.ipaymentid AND clc.ipaymentid = " & ipaymentid & ") "

        queries(3) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó el Pago " & ipaymentid & " : " & txtDescripcionPago.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

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

            br.srevisiondocument = "Pago"
            br.sid = ipaymentid

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

        wasCreated = True

    End Sub


End Class