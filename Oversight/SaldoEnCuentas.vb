Public Class SaldoEnCuentas

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

    Private isFormReadyForAction As Boolean = False
    Private isDgvSaldoEnCuentasReady As Boolean = False

    Public querystring As String = ""

    Private iselectedid As Integer = 0
    Private sselectedtype As String = ""

    Private viewPermission As Boolean = False

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

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Modificar" Then
                    'btnGuardar.Enabled = True
                    'btnGuardarYCerrar.Enabled = True
                End If

                If permission = "Ver Entradas" Then
                    dgvEntradas.Visible = True
                End If

                If permission = "Ver Salidas" Then
                    dgvSalidas.Visible = True
                End If

                If permission = "Ver Saldo En Cuentas" Then
                    dgvSaldoEnCuentas.Visible = True
                End If

                If permission = "Exportar Saldo En Cuentas" Then
                    btnExportarExcel.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Saldo En Cuentas', 'OK')")

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


    Private Sub SaldoEnCuentas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "DROP TABLE IF EXISTS tmpSaldo")
        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Saldo En Cuentas', 'OK')")

    End Sub


    Private Sub SaldoEnCuentasDeInsumos_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub SaldoEnCuentas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        cmbCuentaDestino.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM companyaccounts")
        cmbCuentaDestino.DisplayMember = "scompanyaccountname"
        cmbCuentaDestino.ValueMember = "iaccountid"
        cmbCuentaDestino.SelectedIndex = 0

        querystring = ""

        Dim querySaldoEnCuentas As String

        Dim queries(3) As String

        executeSQLCommand(0, "SELECT @a:=0, @b:=-1")

        queries(0) = "DROP TABLE IF EXISTS tmpSaldo"

        queries(1) = "" & _
        "CREATE TABLE tmpSaldo AS SELECT @b:=@b+1 AS previousid, @a:=@a+1 AS id, tmpA.iid, tmpA.tipo, tmpA.fecha, tmpA.tipomov, tmpA.banco, " & _
        "tmpA.cuenta, tmpA.referencia, tmpA.descripcion, tmpA.monto, tmpA.persona, 0 AS saldo " & _
        "FROM ( " & _
        "SELECT ic.iincomeid AS 'iid', " & _
        "'Ingreso' AS 'tipo', " & _
        "STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "ict.sincometypedescription AS 'tipomov', " & _
        "IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'banco', " & _
        "ic.soriginaccount AS 'cuenta', " & _
        "ic.soriginreference AS 'referencia', " & _
        "ic.sincomedescription AS 'descripcion', " & _
        "ic.dincomeamount AS 'monto', " & _
        "pe.speoplefullname AS 'persona' " & _
        "FROM incomes ic " & _
        "JOIN incometypes ict ON ic.iincometypeid = ict.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN people pe ON ic.ireceiverid = pe.ipeopleid " & _
        "LEFT JOIN incomeprojects icp ON ic.iincomeid = icp.iincomeid " & _
        "LEFT JOIN projects p ON icp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe2 ON p.ipeopleid = pe2.ipeopleid " & _
        "LEFT JOIN incomeassets ica ON ic.iincomeid = ica.iincomeid " & _
        "LEFT JOIN assets a ON ica.iassetid = a.iassetid " & _
        "WHERE " & _
        "ic.idestinationaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR ict.sincometypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR ic.soriginaccount LIKE '%" & querystring & "%' " & _
        "OR ic.soriginreference LIKE '%" & querystring & "%' OR ic.sincomedescription LIKE '%" & querystring & "%' OR FORMAT(ic.dincomeamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe.speoplefullname LIKE '%" & querystring & "%' OR pe.speopleaddress LIKE '%" & querystring & "%' OR pe.speoplemail LIKE '%" & querystring & "%' OR pe.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe2.speoplemail LIKE '%" & querystring & "%' OR pe2.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR p.sprojectname LIKE '%" & querystring & "%' OR p.sprojectcompanyname LIKE '%" & querystring & "%' OR p.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectforecastedclosingdate, ' ', p.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR a.sassetdescription LIKE '%" & querystring & "%') OR icp.sincomeextranote LIKE '%" & querystring & "%' OR ica.sincomeextranote LIKE '%" & querystring & "%' " & _
        "UNION " & _
        "SELECT py.ipaymentid AS 'iid', " & _
        "'Pago' AS 'tipo', " & _
        "STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyt.spaymenttypedescription AS 'tipomov', " & _
        "IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'banco', " & _
        "py.sdestinationaccount AS 'cuenta', " & _
        "py.sdestinationreference AS 'referencia', " & _
        "py.spaymentdescription AS 'descripcion', " & _
        "py.dpaymentamount*-1 AS 'monto', " & _
        "pe1.speoplefullname AS 'persona' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN people pe1 ON py.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN payrollpayments pap ON py.ipaymentid = pap.ipaymentid " & _
        "LEFT JOIN payrolls par ON pap.ipayrollid = par.ipayrollid " & _
        "LEFT JOIN payrollpeople pape ON par.ipayrollid = pape.ipayrollid " & _
        "LEFT JOIN people pe3 ON pape.ipeopleid = pe3.ipeopleid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN supplierinvoices si ON sipy.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "LEFT JOIN people pe4 ON si.ipeopleid = pe4.ipeopleid " & _
        "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
        "LEFT JOIN projects p2 ON sip.iprojectid = p2.iprojectid " & _
        "LEFT JOIN supplierinvoicediscounts sid ON si.isupplierinvoiceid = sid.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoiceassets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
        "LEFT JOIN assets a2 ON sia.iassetid = a2.iassetid " & _
        "LEFT JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN assets a1 ON gv.iassetid = a1.iassetid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gvp.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN projects p1 ON gvp.iprojectid = p1.iprojectid " & _
        "LEFT JOIN people pe2 ON p1.ipeopleid = pe2.ipeopleid " & _
        "WHERE " & _
        "py.ioriginaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pyt.spaymenttypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR py.sdestinationaccount LIKE '%" & querystring & "%' " & _
        "OR py.sdestinationreference LIKE '%" & querystring & "%' OR py.spaymentdescription LIKE '%" & querystring & "%' OR FORMAT(py.dpaymentamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe1.speoplefullname LIKE '%" & querystring & "%' OR pe1.speopleaddress LIKE '%" & querystring & "%' OR gv.scarmileageatrequest LIKE '%" & querystring & "%' OR gv.scarorigindestination LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(gv.igasdate, ' ', gv.sgastime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR gv.svouchercomment LIKE '%" & querystring & "%' OR gv.dlitersdispensed LIKE '%" & querystring & "%' OR gvp.sextranote LIKE '%" & querystring & "%' OR gvp.dliters LIKE '%" & querystring & "%' " & _
        "OR a1.sassetdescription LIKE '%" & querystring & "%' OR p1.sprojectname LIKE '%" & querystring & "%' OR p1.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectdate, ' ', p1.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectstarteddate, ' ', p1.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectforecastedclosingdate, ' ', p1.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe3.speoplefullname LIKE '%" & querystring & "%' OR pe3.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sipy.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR s.ssuppliername LIKE '%" & querystring & "%' OR s.ssupplierofficialname LIKE '%" & querystring & "%' OR s.ssupplieraddress LIKE '%" & querystring & "%' " & _
        "OR s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR s.ssupplierrfc LIKE '%" & querystring & "%' OR s.ssupplieremail LIKE '%" & querystring & "%' OR s.ssupplierobservations LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(si.iinvoicedate, ' ', si.sinvoicetime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sit.ssupplierinvoicetypedescription LIKE '%" & querystring & "%' OR si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR si.sexpensedescription LIKE '%" & querystring & "%' " & _
        "OR si.sexpenselocation LIKE '%" & querystring & "%' OR pe4.speoplefullname LIKE '%" & querystring & "%' OR pe4.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sii.sinputextranote LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputtotalprice LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputunitprice LIKE '%" & querystring & "%' " & _
        "OR i.sinputdescription LIKE '%" & querystring & "%' OR sip.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR p2.sprojectname LIKE '%" & querystring & "%' OR p2.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectdate, ' ', p2.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectstarteddate, ' ', p2.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectforecastedclosingdate, ' ', p2.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR sia.ssupplierinvoiceextranote LIKE '%" & querystring & "%' " & _
        "OR a2.sassetdescription LIKE '%" & querystring & "%') " & _
        ") tmpA " & _
        "LEFT JOIN payments py ON tmpA.tipo = 'Pago' AND tmpA.iid = py.ipaymentid " & _
        "LEFT JOIN incomes ic ON tmpA.tipo = 'Ingreso' AND tmpA.iid = ic.iincomeid " & _
        "ORDER BY 5 ASC "

        If executeTransactedSQLCommand(0, queries) = True Then

            Dim lineas As Integer = getSQLQueryAsInteger(0, "SELECT MAX(id) FROM tmpSaldo")
            Dim queriesUpdate(lineas + 1) As String

            For i = 0 To lineas
                queriesUpdate(i) = "UPDATE tmpSaldo tmpA LEFT JOIN tmpSaldo tmpB ON tmpB.id = tmpA.previousid SET tmpA.saldo = IF(tmpB.saldo IS NULL, 0, tmpB.saldo) + tmpA.monto WHERE tmpA.id = " & i + 1
            Next i

            If executeTransactedSQLCommand(0, queriesUpdate) = True Then

                querySaldoEnCuentas = "" & _
                "SELECT iid, id AS 'Operacion', tipo AS 'Tipo Movimiento', fecha AS 'Fecha', tipomov AS 'Tipo de Movimiento', " & _
                "banco AS 'Banco', cuenta AS 'Cuenta', referencia AS 'Referencia', descripcion AS 'Descripcion Movimiento', " & _
                "FORMAT(monto, 2) AS 'Monto', persona AS 'Persona', FORMAT(saldo, 2) AS 'Saldo tras Movimiento' " & _
                "FROM tmpSaldo ORDER BY 4 DESC "

                'txtBuscar.Enabled = True

                setDataGridView(dgvSaldoEnCuentas, querySaldoEnCuentas, True)

                dgvSaldoEnCuentas.Columns(0).Visible = False

                dgvSaldoEnCuentas.Columns(0).Width = 30
                dgvSaldoEnCuentas.Columns(1).Width = 100
                dgvSaldoEnCuentas.Columns(2).Width = 200
                dgvSaldoEnCuentas.Columns(3).Width = 150

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Cuentas de la Compañía', 'OK')")

            Else

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Cuentas de la Compañía', 'Failed')")
                MsgBox("Ocurrió un error mientras se creaba la tabla. Favor de consultar al administrador del Sistema", MsgBoxStyle.OkOnly)

            End If


        End If

        dgvSaldoEnCuentas.Select()

        'txtBuscar.Focus()

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


    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        querystring = ""

        Dim querySaldoEnCuentas As String

        Dim queries(3) As String

        executeSQLCommand(0, "SELECT @a:=0, @b:=-1")

        queries(0) = "DROP TABLE IF EXISTS tmpSaldo"

        queries(1) = "" & _
        "CREATE TABLE tmpSaldo AS SELECT @b:=@b+1 AS previousid, @a:=@a+1 AS id, tmpA.iid, tmpA.tipo, tmpA.fecha, tmpA.tipomov, tmpA.banco, " & _
        "tmpA.cuenta, tmpA.referencia, tmpA.descripcion, tmpA.monto, tmpA.persona, 0 AS saldo " & _
        "FROM ( " & _
        "SELECT ic.iincomeid AS 'iid', " & _
        "'Ingreso' AS 'tipo', " & _
        "STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "ict.sincometypedescription AS 'tipomov', " & _
        "IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'banco', " & _
        "ic.soriginaccount AS 'cuenta', " & _
        "ic.soriginreference AS 'referencia', " & _
        "ic.sincomedescription AS 'descripcion', " & _
        "ic.dincomeamount AS 'monto', " & _
        "pe.speoplefullname AS 'persona' " & _
        "FROM incomes ic " & _
        "JOIN incometypes ict ON ic.iincometypeid = ict.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN people pe ON ic.ireceiverid = pe.ipeopleid " & _
        "LEFT JOIN incomeprojects icp ON ic.iincomeid = icp.iincomeid " & _
        "LEFT JOIN projects p ON icp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe2 ON p.ipeopleid = pe2.ipeopleid " & _
        "LEFT JOIN incomeassets ica ON ic.iincomeid = ica.iincomeid " & _
        "LEFT JOIN assets a ON ica.iassetid = a.iassetid " & _
        "WHERE " & _
        "ic.idestinationaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR ict.sincometypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR ic.soriginaccount LIKE '%" & querystring & "%' " & _
        "OR ic.soriginreference LIKE '%" & querystring & "%' OR ic.sincomedescription LIKE '%" & querystring & "%' OR FORMAT(ic.dincomeamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe.speoplefullname LIKE '%" & querystring & "%' OR pe.speopleaddress LIKE '%" & querystring & "%' OR pe.speoplemail LIKE '%" & querystring & "%' OR pe.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe2.speoplemail LIKE '%" & querystring & "%' OR pe2.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR p.sprojectname LIKE '%" & querystring & "%' OR p.sprojectcompanyname LIKE '%" & querystring & "%' OR p.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectforecastedclosingdate, ' ', p.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR a.sassetdescription LIKE '%" & querystring & "%') OR icp.sincomeextranote LIKE '%" & querystring & "%' OR ica.sincomeextranote LIKE '%" & querystring & "%' " & _
        "UNION " & _
        "SELECT py.ipaymentid AS 'iid', " & _
        "'Pago' AS 'tipo', " & _
        "STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyt.spaymenttypedescription AS 'tipomov', " & _
        "IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'banco', " & _
        "py.sdestinationaccount AS 'cuenta', " & _
        "py.sdestinationreference AS 'referencia', " & _
        "py.spaymentdescription AS 'descripcion', " & _
        "py.dpaymentamount*-1 AS 'monto', " & _
        "pe1.speoplefullname AS 'persona' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN people pe1 ON py.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN payrollpayments pap ON py.ipaymentid = pap.ipaymentid " & _
        "LEFT JOIN payrolls par ON pap.ipayrollid = par.ipayrollid " & _
        "LEFT JOIN payrollpeople pape ON par.ipayrollid = pape.ipayrollid " & _
        "LEFT JOIN people pe3 ON pape.ipeopleid = pe3.ipeopleid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN supplierinvoices si ON sipy.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "LEFT JOIN people pe4 ON si.ipeopleid = pe4.ipeopleid " & _
        "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
        "LEFT JOIN projects p2 ON sip.iprojectid = p2.iprojectid " & _
        "LEFT JOIN supplierinvoicediscounts sid ON si.isupplierinvoiceid = sid.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoiceassets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
        "LEFT JOIN assets a2 ON sia.iassetid = a2.iassetid " & _
        "LEFT JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN assets a1 ON gv.iassetid = a1.iassetid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gvp.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN projects p1 ON gvp.iprojectid = p1.iprojectid " & _
        "LEFT JOIN people pe2 ON p1.ipeopleid = pe2.ipeopleid " & _
        "WHERE " & _
        "py.ioriginaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pyt.spaymenttypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR py.sdestinationaccount LIKE '%" & querystring & "%' " & _
        "OR py.sdestinationreference LIKE '%" & querystring & "%' OR py.spaymentdescription LIKE '%" & querystring & "%' OR FORMAT(py.dpaymentamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe1.speoplefullname LIKE '%" & querystring & "%' OR pe1.speopleaddress LIKE '%" & querystring & "%' OR gv.scarmileageatrequest LIKE '%" & querystring & "%' OR gv.scarorigindestination LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(gv.igasdate, ' ', gv.sgastime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR gv.svouchercomment LIKE '%" & querystring & "%' OR gv.dlitersdispensed LIKE '%" & querystring & "%' OR gvp.sextranote LIKE '%" & querystring & "%' OR gvp.dliters LIKE '%" & querystring & "%' " & _
        "OR a1.sassetdescription LIKE '%" & querystring & "%' OR p1.sprojectname LIKE '%" & querystring & "%' OR p1.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectdate, ' ', p1.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectstarteddate, ' ', p1.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectforecastedclosingdate, ' ', p1.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe3.speoplefullname LIKE '%" & querystring & "%' OR pe3.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sipy.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR s.ssuppliername LIKE '%" & querystring & "%' OR s.ssupplierofficialname LIKE '%" & querystring & "%' OR s.ssupplieraddress LIKE '%" & querystring & "%' " & _
        "OR s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR s.ssupplierrfc LIKE '%" & querystring & "%' OR s.ssupplieremail LIKE '%" & querystring & "%' OR s.ssupplierobservations LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(si.iinvoicedate, ' ', si.sinvoicetime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sit.ssupplierinvoicetypedescription LIKE '%" & querystring & "%' OR si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR si.sexpensedescription LIKE '%" & querystring & "%' " & _
        "OR si.sexpenselocation LIKE '%" & querystring & "%' OR pe4.speoplefullname LIKE '%" & querystring & "%' OR pe4.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sii.sinputextranote LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputtotalprice LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputunitprice LIKE '%" & querystring & "%' " & _
        "OR i.sinputdescription LIKE '%" & querystring & "%' OR sip.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR p2.sprojectname LIKE '%" & querystring & "%' OR p2.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectdate, ' ', p2.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectstarteddate, ' ', p2.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectforecastedclosingdate, ' ', p2.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR sia.ssupplierinvoiceextranote LIKE '%" & querystring & "%' " & _
        "OR a2.sassetdescription LIKE '%" & querystring & "%') " & _
        ") tmpA " & _
        "LEFT JOIN payments py ON tmpA.tipo = 'Pago' AND tmpA.iid = py.ipaymentid " & _
        "LEFT JOIN incomes ic ON tmpA.tipo = 'Ingreso' AND tmpA.iid = ic.iincomeid " & _
        "ORDER BY 5 ASC "

        If executeTransactedSQLCommand(0, queries) = True Then

            Dim lineas As Integer = getSQLQueryAsInteger(0, "SELECT MAX(id) FROM tmpSaldo")
            Dim queriesUpdate(lineas + 1) As String

            For i = 0 To lineas
                queriesUpdate(i) = "UPDATE tmpSaldo tmpA LEFT JOIN tmpSaldo tmpB ON tmpB.id = tmpA.previousid SET tmpA.saldo = IF(tmpB.saldo IS NULL, 0, tmpB.saldo) + tmpA.monto WHERE tmpA.id = " & i + 1
            Next i

            If executeTransactedSQLCommand(0, queriesUpdate) = True Then

                querySaldoEnCuentas = "" & _
                "SELECT iid, id AS 'Operacion', tipo AS 'Tipo Movimiento', fecha AS 'Fecha', tipomov AS 'Tipo de Movimiento', " & _
                "banco AS 'Banco', cuenta AS 'Cuenta', referencia AS 'Referencia', descripcion AS 'Descripcion Movimiento', " & _
                "FORMAT(monto, 2) AS 'Monto', persona AS 'Persona', FORMAT(saldo, 2) AS 'Saldo tras Movimiento' " & _
                "FROM tmpSaldo ORDER BY 4 DESC "

                'txtBuscar.Enabled = True

                setDataGridView(dgvSaldoEnCuentas, querySaldoEnCuentas, True)

                dgvSaldoEnCuentas.Columns(0).Visible = False

                dgvSaldoEnCuentas.Columns(0).Width = 30
                dgvSaldoEnCuentas.Columns(1).Width = 100
                dgvSaldoEnCuentas.Columns(2).Width = 200
                dgvSaldoEnCuentas.Columns(3).Width = 150

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Cuentas de la Compañía', 'OK')")

            Else

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Cuentas de la Compañía', 'Failed')")
                MsgBox("Ocurrió un error mientras se creaba la tabla. Favor de consultar al administrador del Sistema", MsgBoxStyle.OkOnly)

            End If

        End If



        Dim queryEntradas As String

        queryEntradas = "" & _
        "SELECT ic.iincomeid, 'Ingreso' AS 'Tipo Movimiento', STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'Fecha', " & _
        "ict.sincometypedescription AS 'Tipo de Movimiento', IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'Banco', " & _
        "ic.soriginaccount AS 'Cuenta', ic.soriginreference AS 'Referencia', ic.sincomedescription AS 'Descripcion Movimiento', " & _
        "FORMAT(ic.dincomeamount, 2) AS 'Monto', pe.speoplefullname AS 'Persona' " & _
        "FROM incomes ic " & _
        "JOIN incometypes ict ON ic.iincometypeid = ict.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN people pe ON ic.ireceiverid = pe.ipeopleid " & _
        "LEFT JOIN incomeprojects icp ON ic.iincomeid = icp.iincomeid " & _
        "LEFT JOIN projects p ON icp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe2 ON p.ipeopleid = pe2.ipeopleid " & _
        "LEFT JOIN incomeassets ica ON ic.iincomeid = ica.iincomeid " & _
        "LEFT JOIN assets a ON ica.iassetid = a.iassetid " & _
        "WHERE " & _
        "ic.idestinationaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR ict.sincometypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR ic.soriginaccount LIKE '%" & querystring & "%' " & _
        "OR ic.soriginreference LIKE '%" & querystring & "%' OR ic.sincomedescription LIKE '%" & querystring & "%' OR FORMAT(ic.dincomeamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe.speoplefullname LIKE '%" & querystring & "%' OR pe.speopleaddress LIKE '%" & querystring & "%' OR pe.speoplemail LIKE '%" & querystring & "%' OR pe.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe2.speoplemail LIKE '%" & querystring & "%' OR pe2.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR p.sprojectname LIKE '%" & querystring & "%' OR p.sprojectcompanyname LIKE '%" & querystring & "%' OR p.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectforecastedclosingdate, ' ', p.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR a.sassetdescription LIKE '%" & querystring & "%' OR icp.sincomeextranote LIKE '%" & querystring & "%' OR ica.sincomeextranote LIKE '%" & querystring & "%') ORDER BY 3 DESC "

        setDataGridView(dgvEntradas, queryEntradas, True)

        dgvEntradas.Columns(0).Visible = False



        Dim querySalidas As String

        querySalidas = "" & _
        "SELECT py.ipaymentid, 'Pago' AS 'Tipo Movimiento', STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'Fecha', " & _
        "pyt.spaymenttypedescription AS 'Tipo de Movimiento', IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'Banco', " & _
        "py.sdestinationaccount AS 'Cuenta', py.sdestinationreference AS 'Referencia', py.spaymentdescription AS 'Descripcion Movimiento', " & _
        "FORMAT(py.dpaymentamount*-1, 2) AS 'Monto', pe1.speoplefullname AS 'Persona' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN people pe1 ON py.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN payrollpayments pap ON py.ipaymentid = pap.ipaymentid " & _
        "LEFT JOIN payrolls par ON pap.ipayrollid = par.ipayrollid " & _
        "LEFT JOIN payrollpeople pape ON par.ipayrollid = pape.ipayrollid " & _
        "LEFT JOIN people pe3 ON pape.ipeopleid = pe3.ipeopleid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN supplierinvoices si ON sipy.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "LEFT JOIN people pe4 ON si.ipeopleid = pe4.ipeopleid " & _
        "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
        "LEFT JOIN projects p2 ON sip.iprojectid = p2.iprojectid " & _
        "LEFT JOIN supplierinvoicediscounts sid ON si.isupplierinvoiceid = sid.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoiceassets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
        "LEFT JOIN assets a2 ON sia.iassetid = a2.iassetid " & _
        "LEFT JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN assets a1 ON gv.iassetid = a1.iassetid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gvp.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN projects p1 ON gvp.iprojectid = p1.iprojectid " & _
        "LEFT JOIN people pe2 ON p1.ipeopleid = pe2.ipeopleid " & _
        "WHERE " & _
        "py.ioriginaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pyt.spaymenttypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR py.sdestinationaccount LIKE '%" & querystring & "%' " & _
        "OR py.sdestinationreference LIKE '%" & querystring & "%' OR py.spaymentdescription LIKE '%" & querystring & "%' OR FORMAT(py.dpaymentamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe1.speoplefullname LIKE '%" & querystring & "%' OR pe1.speopleaddress LIKE '%" & querystring & "%' OR gv.scarmileageatrequest LIKE '%" & querystring & "%' OR gv.scarorigindestination LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(gv.igasdate, ' ', gv.sgastime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR gv.svouchercomment LIKE '%" & querystring & "%' OR gv.dlitersdispensed LIKE '%" & querystring & "%' OR gvp.sextranote LIKE '%" & querystring & "%' OR gvp.dliters LIKE '%" & querystring & "%' " & _
        "OR a1.sassetdescription LIKE '%" & querystring & "%' OR p1.sprojectname LIKE '%" & querystring & "%' OR p1.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectdate, ' ', p1.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectstarteddate, ' ', p1.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectforecastedclosingdate, ' ', p1.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe3.speoplefullname LIKE '%" & querystring & "%' OR pe3.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sipy.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR s.ssuppliername LIKE '%" & querystring & "%' OR s.ssupplierofficialname LIKE '%" & querystring & "%' OR s.ssupplieraddress LIKE '%" & querystring & "%' " & _
        "OR s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR s.ssupplierrfc LIKE '%" & querystring & "%' OR s.ssupplieremail LIKE '%" & querystring & "%' OR s.ssupplierobservations LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(si.iinvoicedate, ' ', si.sinvoicetime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sit.ssupplierinvoicetypedescription LIKE '%" & querystring & "%' OR si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR si.sexpensedescription LIKE '%" & querystring & "%' " & _
        "OR si.sexpenselocation LIKE '%" & querystring & "%' OR pe4.speoplefullname LIKE '%" & querystring & "%' OR pe4.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sii.sinputextranote LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputtotalprice LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputunitprice LIKE '%" & querystring & "%' " & _
        "OR i.sinputdescription LIKE '%" & querystring & "%' OR sip.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR p2.sprojectname LIKE '%" & querystring & "%' OR p2.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectdate, ' ', p2.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectstarteddate, ' ', p2.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectforecastedclosingdate, ' ', p2.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR sia.ssupplierinvoiceextranote LIKE '%" & querystring & "%' " & _
        "OR a2.sassetdescription LIKE '%" & querystring & "%') " & _
        "ORDER BY 3 DESC "

        setDataGridView(dgvSalidas, querySalidas, True)

        dgvSalidas.Columns(0).Visible = False

        'txtBuscar.Focus()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub cmbCuentaDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCuentaDestino.SelectedIndexChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim querySaldoEnCuentas As String

        Dim queries(3) As String

        executeSQLCommand(0, "SELECT @a:=0, @b:=-1")

        queries(0) = "DROP TABLE IF EXISTS tmpSaldo"

        queries(1) = "" & _
        "CREATE TABLE tmpSaldo AS SELECT @b:=@b+1 AS previousid, @a:=@a+1 AS id, tmpA.iid, tmpA.tipo, tmpA.fecha, tmpA.tipomov, tmpA.banco, " & _
        "tmpA.cuenta, tmpA.referencia, tmpA.descripcion, tmpA.monto, tmpA.persona, 0 AS saldo " & _
        "FROM ( " & _
        "SELECT ic.iincomeid AS 'iid', " & _
        "'Ingreso' AS 'tipo', " & _
        "STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "ict.sincometypedescription AS 'tipomov', " & _
        "IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'banco', " & _
        "ic.soriginaccount AS 'cuenta', " & _
        "ic.soriginreference AS 'referencia', " & _
        "ic.sincomedescription AS 'descripcion', " & _
        "ic.dincomeamount AS 'monto', " & _
        "pe.speoplefullname AS 'persona' " & _
        "FROM incomes ic " & _
        "JOIN incometypes ict ON ic.iincometypeid = ict.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN people pe ON ic.ireceiverid = pe.ipeopleid " & _
        "LEFT JOIN incomeprojects icp ON ic.iincomeid = icp.iincomeid " & _
        "LEFT JOIN projects p ON icp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe2 ON p.ipeopleid = pe2.ipeopleid " & _
        "LEFT JOIN incomeassets ica ON ic.iincomeid = ica.iincomeid " & _
        "LEFT JOIN assets a ON ica.iassetid = a.iassetid " & _
        "WHERE " & _
        "ic.idestinationaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR ict.sincometypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR ic.soriginaccount LIKE '%" & querystring & "%' " & _
        "OR ic.soriginreference LIKE '%" & querystring & "%' OR ic.sincomedescription LIKE '%" & querystring & "%' OR FORMAT(ic.dincomeamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe.speoplefullname LIKE '%" & querystring & "%' OR pe.speopleaddress LIKE '%" & querystring & "%' OR pe.speoplemail LIKE '%" & querystring & "%' OR pe.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe2.speoplemail LIKE '%" & querystring & "%' OR pe2.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR p.sprojectname LIKE '%" & querystring & "%' OR p.sprojectcompanyname LIKE '%" & querystring & "%' OR p.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectforecastedclosingdate, ' ', p.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR a.sassetdescription LIKE '%" & querystring & "%') OR icp.sincomeextranote LIKE '%" & querystring & "%' OR ica.sincomeextranote LIKE '%" & querystring & "%' " & _
        "UNION " & _
        "SELECT py.ipaymentid AS 'iid', " & _
        "'Pago' AS 'tipo', " & _
        "STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyt.spaymenttypedescription AS 'tipomov', " & _
        "IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'banco', " & _
        "py.sdestinationaccount AS 'cuenta', " & _
        "py.sdestinationreference AS 'referencia', " & _
        "py.spaymentdescription AS 'descripcion', " & _
        "py.dpaymentamount*-1 AS 'monto', " & _
        "pe1.speoplefullname AS 'persona' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN people pe1 ON py.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN payrollpayments pap ON py.ipaymentid = pap.ipaymentid " & _
        "LEFT JOIN payrolls par ON pap.ipayrollid = par.ipayrollid " & _
        "LEFT JOIN payrollpeople pape ON par.ipayrollid = pape.ipayrollid " & _
        "LEFT JOIN people pe3 ON pape.ipeopleid = pe3.ipeopleid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN supplierinvoices si ON sipy.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "LEFT JOIN people pe4 ON si.ipeopleid = pe4.ipeopleid " & _
        "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
        "LEFT JOIN projects p2 ON sip.iprojectid = p2.iprojectid " & _
        "LEFT JOIN supplierinvoicediscounts sid ON si.isupplierinvoiceid = sid.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoiceassets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
        "LEFT JOIN assets a2 ON sia.iassetid = a2.iassetid " & _
        "LEFT JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN assets a1 ON gv.iassetid = a1.iassetid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gvp.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN projects p1 ON gvp.iprojectid = p1.iprojectid " & _
        "LEFT JOIN people pe2 ON p1.ipeopleid = pe2.ipeopleid " & _
        "WHERE " & _
        "py.ioriginaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pyt.spaymenttypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR py.sdestinationaccount LIKE '%" & querystring & "%' " & _
        "OR py.sdestinationreference LIKE '%" & querystring & "%' OR py.spaymentdescription LIKE '%" & querystring & "%' OR FORMAT(py.dpaymentamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe1.speoplefullname LIKE '%" & querystring & "%' OR pe1.speopleaddress LIKE '%" & querystring & "%' OR gv.scarmileageatrequest LIKE '%" & querystring & "%' OR gv.scarorigindestination LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(gv.igasdate, ' ', gv.sgastime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR gv.svouchercomment LIKE '%" & querystring & "%' OR gv.dlitersdispensed LIKE '%" & querystring & "%' OR gvp.sextranote LIKE '%" & querystring & "%' OR gvp.dliters LIKE '%" & querystring & "%' " & _
        "OR a1.sassetdescription LIKE '%" & querystring & "%' OR p1.sprojectname LIKE '%" & querystring & "%' OR p1.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectdate, ' ', p1.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectstarteddate, ' ', p1.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectforecastedclosingdate, ' ', p1.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe3.speoplefullname LIKE '%" & querystring & "%' OR pe3.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sipy.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR s.ssuppliername LIKE '%" & querystring & "%' OR s.ssupplierofficialname LIKE '%" & querystring & "%' OR s.ssupplieraddress LIKE '%" & querystring & "%' " & _
        "OR s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR s.ssupplierrfc LIKE '%" & querystring & "%' OR s.ssupplieremail LIKE '%" & querystring & "%' OR s.ssupplierobservations LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(si.iinvoicedate, ' ', si.sinvoicetime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sit.ssupplierinvoicetypedescription LIKE '%" & querystring & "%' OR si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR si.sexpensedescription LIKE '%" & querystring & "%' " & _
        "OR si.sexpenselocation LIKE '%" & querystring & "%' OR pe4.speoplefullname LIKE '%" & querystring & "%' OR pe4.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sii.sinputextranote LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputtotalprice LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputunitprice LIKE '%" & querystring & "%' " & _
        "OR i.sinputdescription LIKE '%" & querystring & "%' OR sip.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR p2.sprojectname LIKE '%" & querystring & "%' OR p2.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectdate, ' ', p2.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectstarteddate, ' ', p2.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectforecastedclosingdate, ' ', p2.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR sia.ssupplierinvoiceextranote LIKE '%" & querystring & "%' " & _
        "OR a2.sassetdescription LIKE '%" & querystring & "%') " & _
        ") tmpA " & _
        "LEFT JOIN payments py ON tmpA.tipo = 'Pago' AND tmpA.iid = py.ipaymentid " & _
        "LEFT JOIN incomes ic ON tmpA.tipo = 'Ingreso' AND tmpA.iid = ic.iincomeid " & _
        "ORDER BY 5 ASC "

        If executeTransactedSQLCommand(0, queries) = True Then

            Dim lineas As Integer = getSQLQueryAsInteger(0, "SELECT MAX(id) FROM tmpSaldo")
            Dim queriesUpdate(lineas + 1) As String

            For i = 0 To lineas
                queriesUpdate(i) = "UPDATE tmpSaldo tmpA LEFT JOIN tmpSaldo tmpB ON tmpB.id = tmpA.previousid SET tmpA.saldo = IF(tmpB.saldo IS NULL, 0, tmpB.saldo) + tmpA.monto WHERE tmpA.id = " & i + 1
            Next i

            If executeTransactedSQLCommand(0, queriesUpdate) = True Then

                querySaldoEnCuentas = "" & _
                "SELECT iid, id AS 'Operacion', tipo AS 'Tipo Movimiento', fecha AS 'Fecha', tipomov AS 'Tipo de Movimiento', " & _
                "banco AS 'Banco', cuenta AS 'Cuenta', referencia AS 'Referencia', descripcion AS 'Descripcion Movimiento', " & _
                "FORMAT(monto, 2) AS 'Monto', persona AS 'Persona', FORMAT(saldo, 2) AS 'Saldo tras Movimiento' " & _
                "FROM tmpSaldo ORDER BY 4 DESC "

                'txtBuscar.Enabled = True

                setDataGridView(dgvSaldoEnCuentas, querySaldoEnCuentas, True)

                dgvSaldoEnCuentas.Columns(0).Visible = False

                dgvSaldoEnCuentas.Columns(0).Width = 30
                dgvSaldoEnCuentas.Columns(1).Width = 100
                dgvSaldoEnCuentas.Columns(2).Width = 200
                dgvSaldoEnCuentas.Columns(3).Width = 150

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Cuentas de la Compañía', 'OK')")

            Else

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Cuentas de la Compañía', 'Failed')")
                MsgBox("Ocurrió un error mientras se creaba la tabla. Favor de consultar al administrador del Sistema", MsgBoxStyle.OkOnly)

            End If

        End If



        Dim queryEntradas As String

        queryEntradas = "" & _
        "SELECT ic.iincomeid, 'Ingreso' AS 'Tipo Movimiento', STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'Fecha', " & _
        "ict.sincometypedescription AS 'Tipo de Movimiento', IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'Banco', " & _
        "ic.soriginaccount AS 'Cuenta', ic.soriginreference AS 'Referencia', ic.sincomedescription AS 'Descripcion Movimiento', " & _
        "FORMAT(ic.dincomeamount, 2) AS 'Monto', pe.speoplefullname AS 'Persona' " & _
        "FROM incomes ic " & _
        "JOIN incometypes ict ON ic.iincometypeid = ict.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN people pe ON ic.ireceiverid = pe.ipeopleid " & _
        "LEFT JOIN incomeprojects icp ON ic.iincomeid = icp.iincomeid " & _
        "LEFT JOIN projects p ON icp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe2 ON p.ipeopleid = pe2.ipeopleid " & _
        "LEFT JOIN incomeassets ica ON ic.iincomeid = ica.iincomeid " & _
        "LEFT JOIN assets a ON ica.iassetid = a.iassetid " & _
        "WHERE " & _
        "ic.idestinationaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR ict.sincometypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR ic.soriginaccount LIKE '%" & querystring & "%' " & _
        "OR ic.soriginreference LIKE '%" & querystring & "%' OR ic.sincomedescription LIKE '%" & querystring & "%' OR FORMAT(ic.dincomeamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe.speoplefullname LIKE '%" & querystring & "%' OR pe.speopleaddress LIKE '%" & querystring & "%' OR pe.speoplemail LIKE '%" & querystring & "%' OR pe.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe2.speoplemail LIKE '%" & querystring & "%' OR pe2.speopleobservations LIKE '%" & querystring & "%' " & _
        "OR p.sprojectname LIKE '%" & querystring & "%' OR p.sprojectcompanyname LIKE '%" & querystring & "%' OR p.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectforecastedclosingdate, ' ', p.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR a.sassetdescription LIKE '%" & querystring & "%' OR icp.sincomeextranote LIKE '%" & querystring & "%' OR ica.sincomeextranote LIKE '%" & querystring & "%') ORDER BY 3 DESC "

        setDataGridView(dgvEntradas, queryEntradas, True)

        dgvEntradas.Columns(0).Visible = False


        Dim querySalidas As String

        querySalidas = "" & _
        "SELECT py.ipaymentid, 'Pago' AS 'Tipo Movimiento', STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'Fecha', " & _
        "pyt.spaymenttypedescription AS 'Tipo de Movimiento', IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'Banco', " & _
        "py.sdestinationaccount AS 'Cuenta', py.sdestinationreference AS 'Referencia', py.spaymentdescription AS 'Descripcion Movimiento', " & _
        "FORMAT(py.dpaymentamount*-1, 2) AS 'Monto', pe1.speoplefullname AS 'Persona' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN people pe1 ON py.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN payrollpayments pap ON py.ipaymentid = pap.ipaymentid " & _
        "LEFT JOIN payrolls par ON pap.ipayrollid = par.ipayrollid " & _
        "LEFT JOIN payrollpeople pape ON par.ipayrollid = pape.ipayrollid " & _
        "LEFT JOIN people pe3 ON pape.ipeopleid = pe3.ipeopleid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN supplierinvoices si ON sipy.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "LEFT JOIN people pe4 ON si.ipeopleid = pe4.ipeopleid " & _
        "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
        "LEFT JOIN projects p2 ON sip.iprojectid = p2.iprojectid " & _
        "LEFT JOIN supplierinvoicediscounts sid ON si.isupplierinvoiceid = sid.isupplierinvoiceid " & _
        "LEFT JOIN supplierinvoiceassets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
        "LEFT JOIN assets a2 ON sia.iassetid = a2.iassetid " & _
        "LEFT JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN assets a1 ON gv.iassetid = a1.iassetid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gvp.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN projects p1 ON gvp.iprojectid = p1.iprojectid " & _
        "LEFT JOIN people pe2 ON p1.ipeopleid = pe2.ipeopleid " & _
        "WHERE " & _
        "py.ioriginaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
        "CONCAT(STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pyt.spaymenttypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR py.sdestinationaccount LIKE '%" & querystring & "%' " & _
        "OR py.sdestinationreference LIKE '%" & querystring & "%' OR py.spaymentdescription LIKE '%" & querystring & "%' OR FORMAT(py.dpaymentamount, 2) LIKE '%" & querystring & "%' " & _
        "OR pe1.speoplefullname LIKE '%" & querystring & "%' OR pe1.speopleaddress LIKE '%" & querystring & "%' OR gv.scarmileageatrequest LIKE '%" & querystring & "%' OR gv.scarorigindestination LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(gv.igasdate, ' ', gv.sgastime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR gv.svouchercomment LIKE '%" & querystring & "%' OR gv.dlitersdispensed LIKE '%" & querystring & "%' OR gvp.sextranote LIKE '%" & querystring & "%' OR gvp.dliters LIKE '%" & querystring & "%' " & _
        "OR a1.sassetdescription LIKE '%" & querystring & "%' OR p1.sprojectname LIKE '%" & querystring & "%' OR p1.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectdate, ' ', p1.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectstarteddate, ' ', p1.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectforecastedclosingdate, ' ', p1.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe3.speoplefullname LIKE '%" & querystring & "%' OR pe3.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sipy.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR s.ssuppliername LIKE '%" & querystring & "%' OR s.ssupplierofficialname LIKE '%" & querystring & "%' OR s.ssupplieraddress LIKE '%" & querystring & "%' " & _
        "OR s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR s.ssupplierrfc LIKE '%" & querystring & "%' OR s.ssupplieremail LIKE '%" & querystring & "%' OR s.ssupplierobservations LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(si.iinvoicedate, ' ', si.sinvoicetime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sit.ssupplierinvoicetypedescription LIKE '%" & querystring & "%' OR si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR si.sexpensedescription LIKE '%" & querystring & "%' " & _
        "OR si.sexpenselocation LIKE '%" & querystring & "%' OR pe4.speoplefullname LIKE '%" & querystring & "%' OR pe4.speopleaddress LIKE '%" & querystring & "%' " & _
        "OR sii.sinputextranote LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputtotalprice LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputunitprice LIKE '%" & querystring & "%' " & _
        "OR i.sinputdescription LIKE '%" & querystring & "%' OR sip.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR p2.sprojectname LIKE '%" & querystring & "%' OR p2.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectdate, ' ', p2.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectstarteddate, ' ', p2.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectforecastedclosingdate, ' ', p2.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
        "OR sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR sia.ssupplierinvoiceextranote LIKE '%" & querystring & "%' " & _
        "OR a2.sassetdescription LIKE '%" & querystring & "%') " & _
        "ORDER BY 3 DESC "

        setDataGridView(dgvSalidas, querySalidas, True)

        dgvSalidas.Columns(0).Visible = False


    End Sub


    Private Sub dgvSaldoEnCuentas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSaldoEnCuentas.CellClick

        Try

            If dgvSaldoEnCuentas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            If dgvSaldoEnCuentas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedid = CInt(dgvSaldoEnCuentas.Rows(e.RowIndex).Cells(2).Value)
            sselectedtype = dgvSaldoEnCuentas.Rows(e.RowIndex).Cells(3).Value

        Catch ex As Exception

            iselectedid = 0
            sselectedtype = ""

        End Try

    End Sub


    Private Sub dgvSaldoEnCuentas_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSaldoEnCuentas.CellContentClick

        Try

            If dgvSaldoEnCuentas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            If dgvSaldoEnCuentas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedid = CInt(dgvSaldoEnCuentas.Rows(e.RowIndex).Cells(2).Value)
            sselectedtype = dgvSaldoEnCuentas.Rows(e.RowIndex).Cells(3).Value

        Catch ex As Exception

            iselectedid = 0
            sselectedtype = ""

        End Try

    End Sub


    Private Sub dgvSaldoEnCuentas_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvSaldoEnCuentas.SelectionChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Try

            If dgvSaldoEnCuentas.CurrentRow Is Nothing Then

                If dgvSaldoEnCuentas.CurrentRow.IsNewRow Then
                    Exit Sub
                End If

                iselectedid = CInt(dgvSaldoEnCuentas.CurrentRow.Cells(2).Value)
                sselectedtype = dgvSaldoEnCuentas.CurrentRow.Cells(3).Value

            End If

        Catch ex As Exception

            iselectedid = 0
            sselectedtype = ""

        End Try

    End Sub


    Private Sub tcSaldoEnCuentas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcSaldo.SelectedIndexChanged


        querystring = ""

        If tcSaldo.SelectedTab Is tpSaldoEnCuentas Then

            Dim querySaldoEnCuentas As String

            Dim queries(3) As String

            executeSQLCommand(0, "SELECT @a:=0, @b:=-1")

            queries(0) = "DROP TABLE IF EXISTS tmpSaldo"

            queries(1) = "" & _
            "CREATE TABLE tmpSaldo AS SELECT @b:=@b+1 AS previousid, @a:=@a+1 AS id, tmpA.iid, tmpA.tipo, tmpA.fecha, tmpA.tipomov, tmpA.banco, " & _
            "tmpA.cuenta, tmpA.referencia, tmpA.descripcion, tmpA.monto, tmpA.persona, 0 AS saldo " & _
            "FROM ( " & _
            "SELECT ic.iincomeid AS 'iid', " & _
            "'Ingreso' AS 'tipo', " & _
            "STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
            "ict.sincometypedescription AS 'tipomov', " & _
            "IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'banco', " & _
            "ic.soriginaccount AS 'cuenta', " & _
            "ic.soriginreference AS 'referencia', " & _
            "ic.sincomedescription AS 'descripcion', " & _
            "ic.dincomeamount AS 'monto', " & _
            "pe.speoplefullname AS 'persona' " & _
            "FROM incomes ic " & _
            "JOIN incometypes ict ON ic.iincometypeid = ict.iincometypeid " & _
            "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
            "LEFT JOIN people pe ON ic.ireceiverid = pe.ipeopleid " & _
            "LEFT JOIN incomeprojects icp ON ic.iincomeid = icp.iincomeid " & _
            "LEFT JOIN projects p ON icp.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON p.ipeopleid = pe2.ipeopleid " & _
            "LEFT JOIN incomeassets ica ON ic.iincomeid = ica.iincomeid " & _
            "LEFT JOIN assets a ON ica.iassetid = a.iassetid " & _
            "WHERE " & _
            "ic.idestinationaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
            "CONCAT(STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR ict.sincometypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR ic.soriginaccount LIKE '%" & querystring & "%' " & _
            "OR ic.soriginreference LIKE '%" & querystring & "%' OR ic.sincomedescription LIKE '%" & querystring & "%' OR FORMAT(ic.dincomeamount, 2) LIKE '%" & querystring & "%' " & _
            "OR pe.speoplefullname LIKE '%" & querystring & "%' OR pe.speopleaddress LIKE '%" & querystring & "%' OR pe.speoplemail LIKE '%" & querystring & "%' OR pe.speopleobservations LIKE '%" & querystring & "%' " & _
            "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe2.speoplemail LIKE '%" & querystring & "%' OR pe2.speopleobservations LIKE '%" & querystring & "%' " & _
            "OR p.sprojectname LIKE '%" & querystring & "%' OR p.sprojectcompanyname LIKE '%" & querystring & "%' OR p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectforecastedclosingdate, ' ', p.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR a.sassetdescription LIKE '%" & querystring & "%') OR icp.sincomeextranote LIKE '%" & querystring & "%' OR ica.sincomeextranote LIKE '%" & querystring & "%' " & _
            "UNION " & _
            "SELECT py.ipaymentid AS 'iid', " & _
            "'Pago' AS 'tipo', " & _
            "STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
            "pyt.spaymenttypedescription AS 'tipomov', " & _
            "IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'banco', " & _
            "py.sdestinationaccount AS 'cuenta', " & _
            "py.sdestinationreference AS 'referencia', " & _
            "py.spaymentdescription AS 'descripcion', " & _
            "py.dpaymentamount*-1 AS 'monto', " & _
            "pe1.speoplefullname AS 'persona' " & _
            "FROM payments py " & _
            "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
            "LEFT JOIN people pe1 ON py.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN payrollpayments pap ON py.ipaymentid = pap.ipaymentid " & _
            "LEFT JOIN payrolls par ON pap.ipayrollid = par.ipayrollid " & _
            "LEFT JOIN payrollpeople pape ON par.ipayrollid = pape.ipayrollid " & _
            "LEFT JOIN people pe3 ON pape.ipeopleid = pe3.ipeopleid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN supplierinvoices si ON sipy.isupplierinvoiceid = si.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
            "LEFT JOIN people pe4 ON si.ipeopleid = pe4.ipeopleid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN projects p2 ON sip.iprojectid = p2.iprojectid " & _
            "LEFT JOIN supplierinvoicediscounts sid ON si.isupplierinvoiceid = sid.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoiceassets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
            "LEFT JOIN assets a2 ON sia.iassetid = a2.iassetid " & _
            "LEFT JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
            "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
            "LEFT JOIN assets a1 ON gv.iassetid = a1.iassetid " & _
            "LEFT JOIN gasvoucherprojects gvp ON gvp.igasvoucherid = gv.igasvoucherid " & _
            "LEFT JOIN projects p1 ON gvp.iprojectid = p1.iprojectid " & _
            "LEFT JOIN people pe2 ON p1.ipeopleid = pe2.ipeopleid " & _
            "WHERE " & _
            "py.ioriginaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
            "CONCAT(STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR pyt.spaymenttypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR py.sdestinationaccount LIKE '%" & querystring & "%' " & _
            "OR py.sdestinationreference LIKE '%" & querystring & "%' OR py.spaymentdescription LIKE '%" & querystring & "%' OR FORMAT(py.dpaymentamount, 2) LIKE '%" & querystring & "%' " & _
            "OR pe1.speoplefullname LIKE '%" & querystring & "%' OR pe1.speopleaddress LIKE '%" & querystring & "%' OR gv.scarmileageatrequest LIKE '%" & querystring & "%' OR gv.scarorigindestination LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(gv.igasdate, ' ', gv.sgastime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR gv.svouchercomment LIKE '%" & querystring & "%' OR gv.dlitersdispensed LIKE '%" & querystring & "%' OR gvp.sextranote LIKE '%" & querystring & "%' OR gvp.dliters LIKE '%" & querystring & "%' " & _
            "OR a1.sassetdescription LIKE '%" & querystring & "%' OR p1.sprojectname LIKE '%" & querystring & "%' OR p1.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectdate, ' ', p1.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectstarteddate, ' ', p1.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectforecastedclosingdate, ' ', p1.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe3.speoplefullname LIKE '%" & querystring & "%' OR pe3.speopleaddress LIKE '%" & querystring & "%' " & _
            "OR sipy.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR s.ssuppliername LIKE '%" & querystring & "%' OR s.ssupplierofficialname LIKE '%" & querystring & "%' OR s.ssupplieraddress LIKE '%" & querystring & "%' " & _
            "OR s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR s.ssupplierrfc LIKE '%" & querystring & "%' OR s.ssupplieremail LIKE '%" & querystring & "%' OR s.ssupplierobservations LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(si.iinvoicedate, ' ', si.sinvoicetime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR sit.ssupplierinvoicetypedescription LIKE '%" & querystring & "%' OR si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR si.sexpensedescription LIKE '%" & querystring & "%' " & _
            "OR si.sexpenselocation LIKE '%" & querystring & "%' OR pe4.speoplefullname LIKE '%" & querystring & "%' OR pe4.speopleaddress LIKE '%" & querystring & "%' " & _
            "OR sii.sinputextranote LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputtotalprice LIKE '%" & querystring & "%' OR sii.dsupplierinvoiceinputunitprice LIKE '%" & querystring & "%' " & _
            "OR i.sinputdescription LIKE '%" & querystring & "%' OR sip.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR p2.sprojectname LIKE '%" & querystring & "%' OR p2.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectdate, ' ', p2.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectstarteddate, ' ', p2.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p2.iprojectforecastedclosingdate, ' ', p2.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR sia.ssupplierinvoiceextranote LIKE '%" & querystring & "%' " & _
            "OR a2.sassetdescription LIKE '%" & querystring & "%') " & _
            ") tmpA " & _
            "LEFT JOIN payments py ON tmpA.tipo = 'Pago' AND tmpA.iid = py.ipaymentid " & _
            "LEFT JOIN incomes ic ON tmpA.tipo = 'Ingreso' AND tmpA.iid = ic.iincomeid " & _
            "ORDER BY 5 ASC "

            If executeTransactedSQLCommand(0, queries) = True Then

                Dim lineas As Integer = getSQLQueryAsInteger(0, "SELECT MAX(id) FROM tmpSaldo")
                Dim queriesUpdate(lineas + 1) As String

                For i = 0 To lineas
                    queriesUpdate(i) = "UPDATE tmpSaldo tmpA LEFT JOIN tmpSaldo tmpB ON tmpB.id = tmpA.previousid SET tmpA.saldo = IF(tmpB.saldo IS NULL, 0, tmpB.saldo) + tmpA.monto WHERE tmpA.id = " & i + 1
                Next i

                If executeTransactedSQLCommand(0, queriesUpdate) = True Then

                    querySaldoEnCuentas = "" & _
                    "SELECT iid, id AS 'Operacion', tipo AS 'Tipo Movimiento', fecha AS 'Fecha', tipomov AS 'Tipo de Movimiento', " & _
                    "banco AS 'Banco', cuenta AS 'Cuenta', referencia AS 'Referencia', descripcion AS 'Descripcion Movimiento', " & _
                    "FORMAT(monto, 2) AS 'Monto', persona AS 'Persona', FORMAT(saldo, 2) AS 'Saldo tras Movimiento' " & _
                    "FROM tmpSaldo ORDER BY 4 DESC "

                    'txtBuscar.Enabled = True

                    setDataGridView(dgvSaldoEnCuentas, querySaldoEnCuentas, True)

                    dgvSaldoEnCuentas.Columns(0).Visible = False

                    dgvSaldoEnCuentas.Columns(0).Width = 30
                    dgvSaldoEnCuentas.Columns(1).Width = 100
                    dgvSaldoEnCuentas.Columns(2).Width = 200
                    dgvSaldoEnCuentas.Columns(3).Width = 150

                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Cuentas de la Compañía', 'OK')")

                Else

                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Cuentas de la Compañía', 'Failed')")
                    MsgBox("Ocurrió un error mientras se creaba la tabla. Favor de consultar al administrador del Sistema", MsgBoxStyle.OkOnly)

                End If

            End If


        ElseIf tcSaldo.SelectedTab Is tpEntradas Then

            Dim queryEntradas As String

            queryEntradas = "" & _
            "SELECT ic.iincomeid, 'Ingreso' AS 'Tipo Movimiento', STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'Fecha', " & _
            "ict.sincometypedescription AS 'Tipo de Movimiento', IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'Banco', " & _
            "ic.soriginaccount AS 'Cuenta', ic.soriginreference AS 'Referencia', ic.sincomedescription AS 'Descripcion Movimiento', " & _
            "FORMAT(ic.dincomeamount, 2) AS 'Monto', pe.speoplefullname AS 'Persona' " & _
            "FROM incomes ic " & _
            "JOIN incometypes ict ON ic.iincometypeid = ict.iincometypeid " & _
            "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
            "LEFT JOIN people pe ON ic.ireceiverid = pe.ipeopleid " & _
            "LEFT JOIN incomeprojects icp ON ic.iincomeid = icp.iincomeid " & _
            "LEFT JOIN projects p ON icp.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON p.ipeopleid = pe2.ipeopleid " & _
            "LEFT JOIN incomeassets ica ON ic.iincomeid = ica.iincomeid " & _
            "LEFT JOIN assets a ON ica.iassetid = a.iassetid " & _
            "WHERE " & _
            "ic.idestinationaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
            "CONCAT(STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR ict.sincometypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR ic.soriginaccount LIKE '%" & querystring & "%' " & _
            "OR ic.soriginreference LIKE '%" & querystring & "%' OR ic.sincomedescription LIKE '%" & querystring & "%' OR FORMAT(ic.dincomeamount, 2) LIKE '%" & querystring & "%' " & _
            "OR pe.speoplefullname LIKE '%" & querystring & "%' OR pe.speopleaddress LIKE '%" & querystring & "%' OR pe.speoplemail LIKE '%" & querystring & "%' OR pe.speopleobservations LIKE '%" & querystring & "%' " & _
            "OR pe2.speoplefullname LIKE '%" & querystring & "%' OR pe2.speopleaddress LIKE '%" & querystring & "%' OR pe2.speoplemail LIKE '%" & querystring & "%' OR pe2.speopleobservations LIKE '%" & querystring & "%' " & _
            "OR p.sprojectname LIKE '%" & querystring & "%' OR p.sprojectcompanyname LIKE '%" & querystring & "%' OR p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p.iprojectforecastedclosingdate, ' ', p.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR a.sassetdescription LIKE '%" & querystring & "%' OR icp.sincomeextranote LIKE '%" & querystring & "%' OR ica.sincomeextranote LIKE '%" & querystring & "%') ORDER BY 3 DESC "

            setDataGridView(dgvEntradas, queryEntradas, True)

            dgvEntradas.Columns(0).Visible = False

        ElseIf tcSaldo.SelectedTab Is tpSalidas Then

            Dim querySalidas As String

            querySalidas = "" & _
            "SELECT py.ipaymentid, 'Pago' AS 'Tipo Movimiento', STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'Fecha', " & _
            "pyt.spaymenttypedescription AS 'Tipo de Movimiento', IF(ba.sbankname IS NULL, '', ba.sbankname) AS 'Banco', " & _
            "py.sdestinationaccount AS 'Cuenta', py.sdestinationreference AS 'Referencia', py.spaymentdescription AS 'Descripcion Movimiento', " & _
            "FORMAT(py.dpaymentamount*-1, 2) AS 'Monto', pe1.speoplefullname AS 'Persona' " & _
            "FROM payments py " & _
            "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
            "LEFT JOIN people pe1 ON py.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
            "LEFT JOIN payrollpayments pap ON py.ipaymentid = pap.ipaymentid " & _
            "LEFT JOIN payrolls par ON pap.ipayrollid = par.ipayrollid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN supplierinvoices si ON sipy.isupplierinvoiceid = si.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
            "LEFT JOIN supplierinvoicediscounts sid ON si.isupplierinvoiceid = sid.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoiceassets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
            "LEFT JOIN assets a2 ON sia.iassetid = a2.iassetid " & _
            "LEFT JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
            "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
            "LEFT JOIN assets a1 ON gv.iassetid = a1.iassetid " & _
            "LEFT JOIN gasvoucherprojects gvp ON gvp.igasvoucherid = gv.igasvoucherid " & _
            "LEFT JOIN projects p1 ON gvp.iprojectid = p1.iprojectid " & _
            "WHERE " & _
            "py.ioriginaccountid = " & cmbCuentaDestino.SelectedValue & " AND ( " & _
            "CONCAT(STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR pyt.spaymenttypedescription LIKE '%" & querystring & "%' OR ba.sbankname LIKE '%" & querystring & "%' OR py.sdestinationaccount LIKE '%" & querystring & "%' " & _
            "OR py.sdestinationreference LIKE '%" & querystring & "%' OR py.spaymentdescription LIKE '%" & querystring & "%' OR FORMAT(py.dpaymentamount, 2) LIKE '%" & querystring & "%' " & _
            "OR gv.scarmileageatrequest LIKE '%" & querystring & "%' OR gv.scarorigindestination LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(gv.igasdate, ' ', gv.sgastime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR gv.svouchercomment LIKE '%" & querystring & "%' OR gv.dlitersdispensed LIKE '%" & querystring & "%' OR gvp.sextranote LIKE '%" & querystring & "%' OR gvp.dliters LIKE '%" & querystring & "%' " & _
            "OR a1.sassetdescription LIKE '%" & querystring & "%' OR p1.sprojectname LIKE '%" & querystring & "%' OR p1.sprojectcompanyname LIKE '%" & querystring & "%' OR p1.sterrainlocation LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectdate, ' ', p1.sprojecttime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectstarteddate, ' ', p1.sprojectstartedtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(p1.iprojectforecastedclosingdate, ' ', p1.sprojectforecastedclosingtime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR sipy.ssupplierinvoiceextranote LIKE '%" & querystring & "%' OR s.ssuppliername LIKE '%" & querystring & "%' OR s.ssupplierofficialname LIKE '%" & querystring & "%' OR s.ssupplieraddress LIKE '%" & querystring & "%' " & _
            "OR s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR s.ssupplierrfc LIKE '%" & querystring & "%' OR s.ssupplieremail LIKE '%" & querystring & "%' OR s.ssupplierobservations LIKE '%" & querystring & "%' " & _
            "OR CONCAT(STR_TO_DATE(CONCAT(si.iinvoicedate, ' ', si.sinvoicetime), '%Y%c%d %T'), '') LIKE '%" & querystring & "%' " & _
            "OR sit.ssupplierinvoicetypedescription LIKE '%" & querystring & "%' OR si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR si.sexpensedescription LIKE '%" & querystring & "%' " & _
            "OR si.sexpenselocation LIKE '%" & querystring & "%' " & _
            "OR sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR sia.ssupplierinvoiceextranote LIKE '%" & querystring & "%' " & _
            "OR a2.sassetdescription LIKE '%" & querystring & "%') " & _
            "ORDER BY 3 DESC "

            setDataGridView(dgvSalidas, querySalidas, True)

            dgvSalidas.Columns(0).Visible = False

        End If

    End Sub


    Private Sub btnExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportarExcel.Click

        If dgvSaldoEnCuentas.RowCount < 1 Then
            MsgBox("No hay nada que exportar", MsgBoxStyle.OkOnly, "No se hizo la exportación")
            Exit Sub
        End If

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

            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            msSaveFileDialog.FileName = "Desglose de Saldo En Cuentas " & fecha

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Desglose de Saldo En Cuentas Exportado Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar el Desglose de Saldo En Cuentas. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar el Desglose de Saldo En Cuentas")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportToExcel(ByVal pth As String) As Boolean

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
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""12"" ss:Bold=""1""></Font>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("   <Protection></Protection>")
            fs.WriteLine("  </Style>")

            ' Style for the column headers
            fs.WriteLine("   <Style ss:ID=""2"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""9"" ss:Bold=""1""></Font>")
            fs.WriteLine("  </Style>")


            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""9"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""10"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""11"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the SUBtotals labels
            fs.WriteLine("    <Style ss:ID=""12"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FFCC00"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals labels
            fs.WriteLine("    <Style ss:ID=""13"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals
            fs.WriteLine("    <Style ss:ID=""14"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat ss:Format=""Standard""></NumberFormat>")
            fs.WriteLine("    </Style>")

            fs.WriteLine("  </Styles>")

            ' Write the worksheet contents
            fs.WriteLine("<Worksheet ss:Name=""Hoja1"">")
            fs.WriteLine("  <Table ss:DefaultColumnWidth=""60"" ss:DefaultRowHeight=""15"">")

            'Write the project header info
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""62.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""72""/>")
            fs.WriteLine("   <Column ss:Width=""123.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""114.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""102""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""99""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""91.5""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""177.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""70.5""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""162""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:MergeAcross=""10"" ss:StyleID=""1""><Data ss:Type=""String"">SALDO EN CUENTAS</Data></Cell>")
            fs.WriteLine("   </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""45"">")

            For Each col As DataGridViewColumn In dgvSaldoEnCuentas.Columns

                If col.Visible Then

                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))

                End If

            Next


            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvSaldoEnCuentas.Rows

                If dgvSaldoEnCuentas.AllowUserToAddRows = True And row.Index = dgvSaldoEnCuentas.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvSaldoEnCuentas.Columns

                    If col.Visible Then

                        If row.Cells(col.Name).Value.ToString = "" Then

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""></Cell>"))

                        Else

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))

                        End If

                    End If

                Next col

                fs.WriteLine("    </Row>")

            Next row

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
            fs.WriteLine("   <ActiveRow>16</ActiveRow>")
            fs.WriteLine("   <ActiveCol>7</ActiveCol>")
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


End Class